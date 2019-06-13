using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PnlPopUp : MonoBehaviour
{

    //#region FecharPopUp
    //public static void FecharPopUp(GameObject pnl,
    //                               Action acaoFechar)
    //{
    //    EasyAudioUtility.Instance.Play(EasyAudioUtility.Som.Click_Cancel);

    //    AnimacoesTween.AnimarObjeto(pnl, AnimacoesTween.TiposAnimacoes.Scala, () =>
    //    {
    //        Main.Instance.PnlPopUp.SetActive(false);
    //        pnl.SetActive(false);

    //        acaoFechar?.Invoke();
    //    },
    //    AppManager.TEMPO_ANIMACAO_FECHAR_MODAL,
    //    Vector2.zero);
    //}
    //#endregion

    #region FecharPopUp
    public static void FecharPopUp(Canvas canvas,
                                   GameObject pnl,
                                   Action acaoFechar)
    {
        EasyAudioUtility.Instance.Play(EasyAudioUtility.Som.Click_Cancel);

        AnimacoesTween.AnimarObjeto(pnl, AnimacoesTween.TiposAnimacoes.Scala, () =>
        {
            canvas.enabled = false;
            pnl.SetActive(false);

            acaoFechar?.Invoke();
        },
        AppManager.TEMPO_ANIMACAO_FECHAR_MODAL,
        Vector2.zero);
    }
    #endregion

    #region FecharPopUp
    public static void FecharPopUpSemDesligarPopUP(GameObject pnl,
                                   Action acaoFechar)
    {
        EasyAudioUtility.Instance.Play(EasyAudioUtility.Som.Click_Cancel);

        AnimacoesTween.AnimarObjeto(pnl, AnimacoesTween.TiposAnimacoes.Scala, () =>
        {
            pnl.SetActive(false);

            acaoFechar?.Invoke();
        },
        AppManager.TEMPO_ANIMACAO_FECHAR_MODAL,
        Vector2.zero);
    }
    #endregion

    #region FecharPnl
    public static void FecharPnl(GameObject pnl,
                                 Action acaoFechar)
    {
        EasyAudioUtility.Instance.Play(EasyAudioUtility.Som.Click_Cancel);

        AnimacoesTween.AnimarObjeto(pnl, AnimacoesTween.TiposAnimacoes.Scala, () =>
        {
            pnl.SetActive(false);

            acaoFechar?.Invoke();
        },
        AppManager.TEMPO_ANIMACAO_FECHAR_MODAL,
        Vector2.zero);
    }
    #endregion

    #region AbrirPopUp
    public static void AbrirPopUp(GameObject pnl,
                                  Action acaoAbrir,
                                  GameObject pnlFecharAntes = null)
    {
        EasyAudioUtility.Instance.Play(EasyAudioUtility.Som.Click_OK);


        pnlFecharAntes?.SetActive(false);
        pnl.SetActive(true);

        AnimacoesTween.AnimarObjeto(pnl,
            AnimacoesTween.TiposAnimacoes.Scala,
            () => acaoAbrir?.Invoke(),
            AppManager.TEMPO_ANIMACAO_ABRIR_MODEL,
            Vector2.one);
    }
    #endregion


    #region AbrirPopUpCanvas
    public static void AbrirPopUpCanvas(Canvas canvas,
                                  GameObject pnl,
                                  Action acaoAbrir,
                                  GameObject pnlFecharAntes = null)
    {
        EasyAudioUtility.Instance.Play(EasyAudioUtility.Som.Click_OK);


        canvas.enabled = true;

        pnlFecharAntes?.SetActive(false);
        pnl.SetActive(true);

        AnimacoesTween.AnimarObjeto(pnl,
            AnimacoesTween.TiposAnimacoes.Scala,
            () => acaoAbrir?.Invoke(),
            AppManager.TEMPO_ANIMACAO_ABRIR_MODEL,
            Vector2.one);
    }
    #endregion

}
