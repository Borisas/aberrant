using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonAnimation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler {

    [SerializeField] private GameObject _buttonVisual = null;
    [SerializeField] private Sprite _regular = null;
    [SerializeField] private Sprite _hover = null;
    [SerializeField] private Sprite _down = null;
    
    private Shadow _shadow = null;
    private Image _visual = null;
    private RectTransform _rect = null;

    private Vector2 _shadowSize = default;
    private Vector2 _anchoredPosition = default;

    private bool _hoveredState = false;
    private bool _downState = false;
    
    private void Awake() {

        if (_buttonVisual == null) {
            _buttonVisual = gameObject;
        }

        _shadow = _buttonVisual.GetComponent<Shadow>();
        _visual = _buttonVisual.GetComponent<Image>();
        _rect = _buttonVisual.GetComponent<RectTransform>();

        _anchoredPosition = _rect.anchoredPosition;

        _shadowSize = _shadow.effectDistance;
    }

    private void OnDisable() {
        _hoveredState = false;
        _downState = false;
        UpdateVisual();
    }

    public void OnPointerEnter(PointerEventData eventData) {
        _hoveredState = true;
        UpdateVisual();
    }

    public void OnPointerExit(PointerEventData eventData) {
        _hoveredState = false;
        UpdateVisual();
    }

    public void OnPointerDown(PointerEventData eventData) {
        _downState = true;
        UpdateVisual();
    }

    public void OnPointerUp(PointerEventData eventData) {
        _downState = false;
        UpdateVisual();
    }

    void UpdateVisual() {

        Sprite vis = _regular;
        if (_downState) {
            vis = _down;
        }
        else if (_hoveredState) {
            vis = _hover;
        }

        Vector2 ss = _shadowSize;
        Vector2 pOffset = Vector2.zero;
        if (_downState) {
            ss = Vector2.zero;
            pOffset = _shadowSize;
        }

        _visual.sprite = vis;
        _shadow.effectDistance = ss;

        _rect.anchoredPosition = _anchoredPosition + pOffset;
    }
}
