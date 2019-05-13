using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class AnimacoesTween : MonoBehaviour
{

    public enum TiposAnimacoes
    {
        Menu,
        Button_Click,
        SubMenu_Click,
        Scala,
        RotacaoSubMenu
    }

    public GameObject ObjAnimar;
    public TiposAnimacoes TipoAnimacao;

    [Range(0.0F, 10.0F)]
    public float TempoAnimacao;

    public Vector2 VectorScala;

    //nem todas as animacoes usam a variavel TempoAnimacao

    void Start()
    {
        AnimarObjeto(ObjAnimar, TipoAnimacao, null, TempoAnimacao, VectorScala);
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
                case TiposAnimacoes.Menu:
                    ObjAnimar.transform.DOScale(VectorScala, TempoAnimacao).SetLoops(-1, LoopType.Yoyo);
                    break;
                case TiposAnimacoes.Button_Click:
                    ObjAnimar.transform.DOScale(new Vector2(0.8f, 0.8f), 0.3f).SetEase(Ease.OutBounce).OnComplete(() => ObjAnimar.transform.DOScale(new Vector2(1, 1), 0.1f).SetEase(Ease.InBounce).OnComplete(onFinished));
                    break;
                case TiposAnimacoes.SubMenu_Click:
                    ObjAnimar.transform.DOLocalRotate(new Vector3(0, 0, 30), TempoAnimacao).OnComplete(() =>
                          ObjAnimar.transform.DOLocalRotate(new Vector3(0, 0, -30), TempoAnimacao).OnComplete(() =>
                            ObjAnimar.transform.DOLocalRotate(new Vector3(0, 0, 0), TempoAnimacao).OnComplete(onFinished)
                      )
                    );
                    ObjAnimar.transform.DOScale(new Vector2(0.8f, 0.8f), 0.3f).SetEase(Ease.OutBounce).OnComplete(() => ObjAnimar.transform.DOScale(new Vector2(1, 1), 0.1f).SetEase(Ease.InBounce).OnComplete(onFinished));
                    break;
                case TiposAnimacoes.Scala:
                    ObjAnimar.transform.DOScale(VectorScala, TempoAnimacao).OnComplete(onFinished);
                    break;
            }
        }
    }


}
