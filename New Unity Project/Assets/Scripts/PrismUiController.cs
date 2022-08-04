using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PrismUiController : MonoBehaviour
{
    [SerializeField] private TMP_Text prismText;
    private void OnEnable()
    {
        // se inscreve no canal de coins
        PlayerObserverManager.OnPrismchanged += UpdatePrismText;
    }

    private void OnDisable()
    {
        // retira a inscrição no canal de coins 
        PlayerObserverManager.OnPrismchanged -= UpdatePrismText; 
    }
    
    // função usada para tratar a notificação do canal  de coins
    private void UpdatePrismText(int newPrismValue)
    {
        prismText.text = newPrismValue.ToString();
    }
}
