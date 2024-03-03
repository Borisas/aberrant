using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class GameplayIntermission : MonoBehaviour {

    [Header("Button Recover")] 
    [SerializeField] private GameObject _buttonRecover;
    [SerializeField] private PriceBlock _priceBlockRecover;
    [Header("Button Continue")] 
    [SerializeField] private GameObject _buttonContinue;

    private BloodAmount _priceRecover = default;
    
    public void Open() {
        gameObject.SetActive(true);
        SetupState();
    }

    public void Close() {
        gameObject.SetActive(false);
    }

    public void Resetup() {
        SetupState();
    }

    void SetupState() {

        _buttonContinue.SetActive(true);

        bool fullHealth = Scene.Player.IsFullHealth();
        if (fullHealth) {
            _buttonRecover.SetActive(false);
        }
        else {
            _buttonRecover.SetActive(true);
            _priceRecover = Scene.GameController.GetPriceRecovery();
            _priceBlockRecover.Setup(_priceRecover);
        }
    }
}
