using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlaySoundButtonClick : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler {

    [SerializeField] private string _soundUp = "";
    [SerializeField] private string _soundDown = "";
    private bool _play = false;
    
    public void OnPointerUp(PointerEventData eventData) {

        if (!_play) return;
        
        if (!string.IsNullOrEmpty(_soundUp)) {
            SoundManager.Play(_soundUp);
        }
    }

    public void OnPointerEnter(PointerEventData eventData) {
        _play = true;
    }

    public void OnPointerExit(PointerEventData eventData) {
        _play = false;
    }

    public void OnPointerDown(PointerEventData eventData) {
        
        if (!_play) return;
        
        if (!string.IsNullOrEmpty(_soundDown)) {
            SoundManager.Play(_soundDown);
        }
    }
}
