
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
[RequireComponent(typeof(Animation))]
[RequireComponent(typeof(Image))]
public class ImageAnimationWrapper : MonoBehaviour {

    public enum StopAction {
        Destroy = 0,
        Disable = 1,
    }
    
    [SerializeField] [ReadOnly] Image _rend = null;
    [SerializeField] private Sprite[] _sprites = null;
    [SerializeField] private int _frame = 0;
    [SerializeField] private StopAction _stopAction = StopAction.Disable;
    
    private void Awake() {
        _rend = GetComponent<Image>();
    }

    private void Reset() {
        if (_rend == null) {
            _rend = GetComponent<Image>();
        }
        if (gameObject.activeInHierarchy && !Application.isPlaying) {

            _rend.sprite = _sprites[_frame % _sprites.Length];

        }
    }
    

    private void LateUpdate() {
        _rend.sprite = _sprites[_frame % _sprites.Length];
    }

    public void Stop() {
        
        if (!Application.isPlaying) {
            return;
        }

        switch (_stopAction) {
            case StopAction.Destroy:
                Destroy(gameObject);
                break;
            case StopAction.Disable:
                gameObject.SetActive(false);
                break;
        }
    }

}