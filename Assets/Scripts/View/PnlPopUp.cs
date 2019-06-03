using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PnlPopUp : MonoBehaviour
{

    #region FecharPopUp
    public static void FecharPopUp(GameObject pnl,
                                   Action acaoFechar)
    {
        EasyAudioUtility.Instance.Play(EasyAudioUtility.Som.Click_Cancel);

        AnimacoesTween.AnimarObjeto(EventSystem.current.currentSelectedGameObject,
            AnimacoesTween.TiposAnimacoes.Button_Click, () =>
            {
                AnimacoesTween.AnimarObjeto(pnl, AnimacoesTween.TiposAnimacoes.Scala, () =>
                {
                    Main.Instance.PnlPopUp.SetActive(false);
                    pnl.SetActive(false);

                    acaoFechar?.Invoke();
                },
                AppManager.TEMPO_ANIMACAO_FECHAR_MODAL,
                Vector2.zero);
            },
            AppManager.TEMPO_ANIMACAO_ABRIR_CLICK_BOTAO);
    }
    #endregion

    #region FecharPopUp
    public static void FecharPopUpSemDesligarPopUP(GameObject pnl,
                                   Action acaoFechar)
    {
        EasyAudioUtility.Instance.Play(EasyAudioUtility.Som.Click_Cancel);

        AnimacoesTween.AnimarObjeto(EventSystem.current.currentSelectedGameObject,
            AnimacoesTween.TiposAnimacoes.Button_Click, () =>
            {
                AnimacoesTween.AnimarObjeto(pnl, AnimacoesTween.TiposAnimacoes.Scala, () =>
                {
                    pnl.SetActive(false);

                    acaoFechar?.Invoke();
                },
                AppManager.TEMPO_ANIMACAO_FECHAR_MODAL,
                Vector2.zero);
            },
            AppManager.TEMPO_ANIMACAO_ABRIR_CLICK_BOTAO);
    }
    #endregion

    #region FecharPnl
    public static void FecharPnl(GameObject pnl,
                                 Action acaoFechar)
    {
        EasyAudioUtility.Instance.Play(EasyAudioUtility.Som.Click_Cancel);

        AnimacoesTween.AnimarObjeto(EventSystem.current.currentSelectedGameObject,
            AnimacoesTween.TiposAnimacoes.Button_Click, () =>
            {
                AnimacoesTween.AnimarObjeto(pnl, AnimacoesTween.TiposAnimacoes.Scala, () =>
                {
                    pnl.SetActive(false);

                    acaoFechar?.Invoke();
                },
                AppManager.TEMPO_ANIMACAO_FECHAR_MODAL,
                Vector2.zero);
            },
            AppManager.TEMPO_ANIMACAO_ABRIR_CLICK_BOTAO);
    }
    #endregion

    #region AbrirPopUp
    public static void AbrirPopUp(GameObject pnl,
                                  Action acaoAbrir,
                                  GameObject pnlFecharAntes = null)
    {
        EasyAudioUtility.Instance.Play(EasyAudioUtility.Som.Click_OK);

        AnimacoesTween.AnimarObjeto(EventSystem.current.currentSelectedGameObject,
            AnimacoesTween.TiposAnimacoes.Button_Click,
            () =>
            {
                Main.Instance.PnlPopUp.SetActive(true);
                pnlFecharAntes?.SetActive(false);
                pnl.SetActive(true);

                AnimacoesTween.AnimarObjeto(pnl,
                    AnimacoesTween.TiposAnimacoes.Scala,
                    () => acaoAbrir?.Invoke(),
                    AppManager.TEMPO_ANIMACAO_ABRIR_MODEL,
                    Vector2.one);
            },
            AppManager.TEMPO_ANIMACAO_ABRIR_CLICK_BOTAO);
    }
    #endregion

}
