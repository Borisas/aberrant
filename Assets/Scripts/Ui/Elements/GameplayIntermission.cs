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
    [Header("Button MoreLife")] 
    [SerializeField] private GameObject _buttonMoreLife;
    [SerializeField] private PriceBlock _priceBlockMoreLife;
    [Header("Button Continue")] 
    [SerializeField] private GameObject _buttonContinue;

    private BloodAmount _priceRecover = default;
    private BloodAmount _priceMutate = default;
    private BloodAmount _priceMoreLife = default;
    bool _mutated = false;
    bool _lifeIncreased = false;
    bool _recovered = false;
    
    public void Open() {
        _mutated = false;
        _recovered = false;
        _lifeIncreased = false;
        gameObject.SetActive(true);
        SetupState();
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

    public void OnRecover() {
        _recovered = true;
    }

    public void OnMoreLife() {
        _lifeIncreased = true;
    }

    void SetupState() {

        _buttonContinue.SetActive(true);

        _priceRecover = Scene.GameController.GetPriceRecovery();
        _priceMoreLife = Scene.GameController.GetPriceMoreLife();
        _priceMutate = Scene.GameController.GetPriceMutate();

        SetupPurchaseButton(_recovered, _priceRecover, _buttonRecover, _priceBlockRecover);
        SetupPurchaseButton(_mutated, _priceMutate, _buttonMutate, _priceBlockMutate);
        SetupPurchaseButton(_lifeIncreased, _priceMoreLife, _buttonMoreLife, _priceBlockMoreLife);
    }

    void SetupPurchaseButton(bool purchased, BloodAmount price, GameObject button, PriceBlock priceBlock) {

        if ( purchased ) {
            button.SetActive(false);
        }
        else {
            button.SetActive(true);
            priceBlock.Setup(price);
        }
    }
}
