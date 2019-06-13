using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class AnimacoesTween : MonoBehaviour
{

    public enum TiposAnimacoes
    {
        //Menu,
        //Button_Click,
        //SubMenu_Click,
        //RotacaoSubMenu,
        Scala
    }

    public GameObject ObjAnimar;
    public TiposAnimacoes TipoAnimacao;

    [Range(0.0F, 10.0F)]
    public float TempoAnimacao;

    public Vector2 VectorScala;

    //nem todas as animacoes usam a variavel TempoAnimacao

    void Start()
    {
        //AnimarObjeto(ObjAnimar, TipoAnimacao, null, TempoAnimacao, VectorScala);
    }

    public static void AnimarObjeto(GameObject ObjAnimar,
                                     TiposAnimacoes TipoAnimacao,
                                     TweenCallback onFinished = null,
                                     float TempoAnimacao = 0,
                                     Vector2 VectorScala = new Vector2())
    {
        if (ObjAnimar != null)
        {
            switch (TipoAnimacao)
            {
                case TiposAnimacoes.Scala:
                    ObjAnimar.transform.DOScale(VectorScala, TempoAnimacao).OnComplete(onFinished);
                    break;
            }
        }
    }


}
