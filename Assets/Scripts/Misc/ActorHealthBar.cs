
using System;
using UnityEngine;
using UnityEngine.UI;

public class ActorHealthBar : MonoBehaviour {
    private static RectTransform _parentHealthBar = null;
    
    [SerializeField] private float _lift = 1.0f;
    private Actor _actor = null;
    private GameObject _barVisual = null;
    private Transform _barInstance = null;
    private Image _barFill = null;
    private bool _initialised = false;
    
    void Awake() {
        _actor = GetComponent<Actor>();
        _actor.OnDie += OnDie;
    }

    private void OnDie(Actor obj) {
        _barInstance.gameObject.SetActive(false);
        _barInstance = null;//
        _initialised = false;
    }

    private void OnEnable() {

        if (GetBarParent() == null) return;
        
        _barInstance = ObjectPool.Get(Database.GetInstance().Main.HealthBar,GetBarParent()).transform;
        if (_barInstance == null) return;
        
        _barVisual = _barInstance.Find("Bar").gameObject;
        _barFill = _barInstance.Find("Bar/Fill").GetComponent<Image>();

        _initialised = true;
        _barInstance.transform.localScale = Vector3.one;
        UpdateBarFill();
    }

    private void LateUpdate() {

        if (!_initialised) return;
        bool fullHp = Mathf.Abs(_actor.GetHealth() - _actor.GetMaxHealth()) < Mathf.Epsilon;

        if (fullHp) {
            _barVisual.gameObject.SetActive(false);
        }
        else {
            _barVisual.gameObject.SetActive(true);
            _barInstance.position = _actor.transform.position + Vector3.up * _lift;
            UpdateBarFill();
        }

    }

    void UpdateBarFill() {
        _barFill.fillAmount = Mathf.Clamp01(_actor.GetHealth() / _actor.GetMaxHealth());
    }

    static Transform GetBarParent() {
        if (_parentHealthBar == null) {
            _parentHealthBar = Scene.WorldUiController.GetHealthBarParent();
        }

        return _parentHealthBar;
    }
}