using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class GameplayIntermission : MonoBehaviour {

    [Header("Button Recover")] 
    [SerializeField] private GameObject _buttonRecover;
    [SerializeField] private PriceBlock _priceBlockRecover;
    [Header("Button Mutate")] 
    [SerializeField] private GameObject _buttonMutate;
    [SerializeField] private PriceBlock _priceBlockMutate;
    [Header("Button Continue")] 
    [SerializeField] private GameObject _buttonContinue;

    private BloodAmount _priceRecover = default;
    private BloodAmount _priceMutate = default;
    bool _mutated = false;
    
    public void Open() {
        gameObject.SetActive(true);
        SetupState();
        _mutated = false;
    }

    public void Close() {
        gameObject.SetActive(false);
    }

    public void Resetup() {
        SetupState();
    }

    public void OnMutate() {
        _mutated = true;
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

        if ( _mutated ) {
            _buttonMutate.SetActive(false);
        }
        else {
            _buttonMutate.SetActive(true);
            _priceMutate = Scene.GameController.GetPriceMutate();
            _priceBlockMutate.Setup(_priceMutate);
        }
    }
}
