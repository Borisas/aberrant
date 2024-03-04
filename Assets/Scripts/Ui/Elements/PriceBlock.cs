using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PriceBlock : MonoBehaviour {

    [SerializeField] private TMP_Text _labelPrice = null;

    public void Setup(BloodAmount ba) {
        if (ba.Amount <= 0) {
            gameObject.SetActive(false);
            return;
        }

        gameObject.SetActive(true);
        _labelPrice.text = ba.Amount.ToString();
    }
}
