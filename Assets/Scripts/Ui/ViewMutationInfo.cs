using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ViewMutationInfo : UiView {

    [SerializeField] private TMP_Text _title = null;
    [SerializeField] private TMP_Text _description = null;
    [SerializeField] private Image _icon = null;

    private Stack<MutationId> _mutationStack = new Stack<MutationId>();
    
    public static void Open(MutationInstance mi) {

        var view = Scene.UiDirector.GetView<ViewMutationInfo>();
        if (view == null) return;

        if (mi.Level > 1) return;//only open for level 1 mutations
        
        view.AppendStack(mi.Id);
    }

    void AppendStack(MutationId id) {
        var cfg = Database.GetInstance().Main.GetMutationConfig(id);
        if (cfg == null) return;

        _mutationStack.Push(id);

        if (gameObject.activeSelf) return;//do nothing

        PopStack();
    }

    void PopStack() {
        if (_mutationStack.Count <= 0) return;//stack completed

        var id = _mutationStack.Pop();
        var cfg = Database.GetInstance().Main.GetMutationConfig(id);

        _icon.sprite = cfg.Icon;
        _title.text = cfg.Title;
        _description.text = cfg.Description;
        
        Scene.UiDirector.OpenViewAdditive<ViewMutationInfo>();
    }

    public void OnAnimationFinished() {
        gameObject.SetActive(false);
        PopStack();// open next in stack
    }
}
