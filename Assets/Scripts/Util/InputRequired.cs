using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputRequired : MonoBehaviour
{
    public GameObject ObjAlerta;

    public void AtivarDesativarAlerta(bool ehParaAtivar)
    {
        ObjAlerta.SetActive(ehParaAtivar);
    }
}
