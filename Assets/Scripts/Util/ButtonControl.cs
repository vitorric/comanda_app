using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class ButtonControl
{
    public List<Button> BtnAbas;

    public void TrocarAba(int abaAtual)
    {
        BtnAbas.ForEach(x =>
        {
            x.interactable = true;
        });

        BtnAbas[abaAtual].interactable = false;
    }
}
