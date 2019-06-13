using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LevelUpObj : MonoBehaviour
{

    public GameObject ImgCirculoCentral;
    public GameObject ImgAsaDireita;
    public GameObject ImgAsaEsquerda;
    public GameObject ImgMiniAsaEsquerda;
    public GameObject ImgMiniAsaDireita;
    public GameObject ImgBandeira;
    public GameObject TxtLevel;
    public GameObject TxtLevelUp;
    public GameObject BtnFechar;

    void Start()
    {
        EasyAudioUtility.Instance.Play(EasyAudioUtility.Som.LevelUp);
        //animarPainel();
    }

    private void animarPainel()
    {

        AnimacoesTween.AnimarObjeto(ImgCirculoCentral, AnimacoesTween.TiposAnimacoes.Scala, () =>
        {
            animarAsaDireita();
            animarAsaEsquerda();
            ImgCirculoCentral.GetComponent<AnimacoesTween>().enabled = true;
            TxtLevelUp.GetComponent<AnimacoesTween>().enabled = true;
        },
        0.5f,
        new Vector3(1, 1));
    }

    private void animarAsaDireita()
    {
        AnimacoesTween.AnimarObjeto(ImgAsaDireita, AnimacoesTween.TiposAnimacoes.Scala, () =>
        {
            AnimacoesTween.AnimarObjeto(ImgMiniAsaDireita, AnimacoesTween.TiposAnimacoes.Scala, () =>
            { },
            0.3f,
            new Vector3(1, 1));

            animarBandeira();
        },
        0.3f,
        new Vector3(1, 1));
    }
    private void animarAsaEsquerda()
    {
        AnimacoesTween.AnimarObjeto(ImgAsaEsquerda, AnimacoesTween.TiposAnimacoes.Scala, () =>
        {
            AnimacoesTween.AnimarObjeto(ImgMiniAsaEsquerda, AnimacoesTween.TiposAnimacoes.Scala, () =>
            { },
            0.3f,
            new Vector3(1, 1));
        },
        0.3f,
        new Vector3(1, 1));
    }

    private void animarBandeira()
    {
        AnimacoesTween.AnimarObjeto(ImgBandeira, AnimacoesTween.TiposAnimacoes.Scala, () =>
        {
            //EasyAudioUtility.Instance.Play(EasyAudioUtility.Som.LevelUp2);
            //AnimacoesTween.AnimarObjeto(TxtLevel, AnimacoesTween.TiposAnimacoes.SubMenu_Click, () =>
            //{ },
            //0.2f,
            //new Vector3(1, 1));

            AnimacoesTween.AnimarObjeto(BtnFechar, AnimacoesTween.TiposAnimacoes.Scala, () =>
            { },
            0.2f,
            new Vector3(1, 1));
        },
        0.55f,
        new Vector3(1, 1));
    }

    public void BtnPnlFechar()
    {
        EasyAudioUtility.Instance.Play(EasyAudioUtility.Som.Click_Cancel);

        Destroy(this.gameObject);
    }
}
