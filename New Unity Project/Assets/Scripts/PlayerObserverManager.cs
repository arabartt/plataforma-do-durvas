using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Isso seria o nosso YouTube.com
// modificador static diz que pode ser acessado de qualquer lugar no código 
public static class PlayerObserverManager 
{
    // Canal varável coins do PlayerController 
    // 1 - Parte da inscrição
    public static Action<int> OnCoinsChanged;
    
    // 2 - parte do sininho (notificação)
    public static void CoinsChanged(int value)
    {
        // existe alguém inscrito no OnCoinsChanged?
        // caso tenha, mande o value para todos 
        OnCoinsChanged.Invoke(value);
    }

}
