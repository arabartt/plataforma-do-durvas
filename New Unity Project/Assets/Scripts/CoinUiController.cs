using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoinUiController : MonoBehaviour
{
    // referencia para objeto de texto
    [SerializeField] private TMP_Text coinText;

    private void OnEnable()
    {
        // se inscreve no canal de coins
        PlayerObserverManager.OnCoinsChanged += UpdateCoinText;
    }

    private void OnDisable()
    {
        // retira a inscrição no canal de coins 
        PlayerObserverManager.OnCoinsChanged -= UpdateCoinText; 
    }
    
    // função usada para tratar a notificação do canal  de coins
    private void UpdateCoinText(int newCoinsValue)
    {
        coinText.text = newCoinsValue.ToString();
    }
    
}
