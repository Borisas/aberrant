
using Sirenix.OdinInspector;
using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(Animation))]
[RequireComponent(typeof(SpriteRenderer))]
public class SpriteAnimationWrapper : MonoBehaviour {

    public enum StopAction {
        Destroy = 0,
        Disable = 1,
    }
    
    [SerializeField] [ReadOnly] SpriteRenderer _rend = null;
    [SerializeField] private Sprite[] _sprites = null;
    [SerializeField] private int _frame = 0;
    [SerializeField] private StopAction _stopAction = StopAction.Disable;
    
    private void Awake() {
        _rend = GetComponent<SpriteRenderer>();
    }

    private void Reset() {
        if (_rend == null) {
            _rend = GetComponent<SpriteRenderer>();
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