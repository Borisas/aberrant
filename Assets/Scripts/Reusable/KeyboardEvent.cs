using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class KeyboardEvent : MonoBehaviour {

    enum InputType {
        Down,
        Up,
        Held
    }

#if UNITY_EDITOR
    [ValueDropdown("GetAllInputs")] 
#endif
    [SerializeField] private string _inputName = "";
    [SerializeField] private InputType _type = InputType.Up;
    public UnityEvent OnClick;

    private bool _wasDown = false;

    private void Update() {

        bool isDown = Input.GetAxisRaw(_inputName) > Mathf.Epsilon;
        
        if (isDown && (_type == InputType.Down || _type == InputType.Held)) {
            if (!_wasDown && _type == InputType.Down) {
                EmitEvent();
            }
            else if (_type == InputType.Held) {
                EmitEvent();
            }
        }
        else if (!isDown && _wasDown && _type == InputType.Up) {
            EmitEvent();
        }

        _wasDown = isDown;
    }

    void EmitEvent() {
        OnClick?.Invoke();
    }


#if UNITY_EDITOR
    static IEnumerable<string> GetAllInputs() {
        var inputManager = AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/InputManager.asset")[0];

        SerializedObject obj = new SerializedObject(inputManager);

        SerializedProperty axisArray = obj.FindProperty("m_Axes");

        if (axisArray.arraySize == 0)
            Debug.Log("No Axes");


        List<string> inputs = new List<string>();
        for (int i = 0; i < axisArray.arraySize; ++i) {
            var axis = axisArray.GetArrayElementAtIndex(i);

            var name = axis.FindPropertyRelative("m_Name").stringValue;
            inputs.Add(name);
        }

        return inputs;

    }
#endif
}
