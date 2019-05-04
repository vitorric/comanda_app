using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuComanda : MonoBehaviour
{

    public GameObject BtnItensComanda;
    public GameObject BtnGrupoComanda;

    public bool MenuAtivo = false;

    public void BtnAbrirMenuComanda(bool fecharAutomatico = false)
    {

        MenuAtivo = (fecharAutomatico) ? false : !MenuAtivo;

        Main.Instance.AbrirMenu("btnComanda", (fecharAutomatico) ? false : MenuAtivo, new List<GameObject>{
            BtnItensComanda,
            BtnGrupoComanda
        }, fecharAutomatico);
    }
}
