using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PnlDecisao : MonoBehaviour
{
    public GameObject ObjPnlDecisao;
    public Button BtnSim;
    public Button BtnNao;
    public Text TxtMensagem;

    public void AbrirPainel(Action actionSim,
                            Action actionNao,
                            string mensagem)
    {
        BtnSim.onClick.RemoveAllListeners();
        BtnNao.onClick.RemoveAllListeners();

        ObjPnlDecisao.SetActive(true);

        TxtMensagem.text = mensagem;

        BtnSim.onClick.AddListener(() =>
        {
            actionSim();
        });

        BtnNao.onClick.AddListener(() =>
        {
            actionNao();
        });

        ObjPnlDecisao.SetActive(false);
    }

    public void AbrirPainelConviteEstab(Action actionSim,
                            Action actionNao,
                            string nomeEstab)
    {
        BtnSim.onClick.RemoveAllListeners();
        BtnNao.onClick.RemoveAllListeners();

        ObjPnlDecisao.SetActive(true);

        TxtMensagem.text = "Hey! O estabelecimento <color=cyan>" + nomeEstab + "</color> está te convidando para entrar nele. Deseja aceitar?";

        BtnSim.onClick.AddListener(() =>
        {

            EasyAudioUtility.Instance.Play(EasyAudioUtility.Som.Click_OK);

            actionSim();

            ObjPnlDecisao.SetActive(false);

        });

        BtnNao.onClick.AddListener(() =>
        {
            EasyAudioUtility.Instance.Play(EasyAudioUtility.Som.Click_Cancel);

            actionNao();

            ObjPnlDecisao.SetActive(false);
        });
    }
}

