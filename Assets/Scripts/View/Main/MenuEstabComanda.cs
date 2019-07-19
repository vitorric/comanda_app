using System;
using System.Collections.Generic;
using System.Linq;
using APIModel;
using Network;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuEstabComanda : MonoBehaviour
{
    [Header("Botoes")]
    public Button BtnMenuEstabComanda;
    public Button BtnEstabInfoComanda;
    public Button BtnConquistaComanda;
    public Button BtnSairEstabelecimento;

    private void Awake()
    {
        configurarListener();
    }

    #region configurarListener
    private void configurarListener()
    {
        BtnSairEstabelecimento.onClick.AddListener(() => btnSairDoEstabelecimento());
    }
    #endregion

    #region btnAbrirInfoEstabelecimento
    private void btnAbrirInfoEstabelecimento(int aba)
    {
        obterEstabelecimento(aba);
    }
    #endregion

    #region obterEstabelecimento
    private void obterEstabelecimento(int aba)
    {
        Dictionary<string, object> form = new Dictionary<string, object>
        {
            { "_idEstabelecimento", Cliente.ClienteLogado.configClienteAtual.estabelecimento }
        };

        StartCoroutine(EstabelecimentoAPI.ObterEstabelecimento(form,
        (response, error) =>
        {
            Main.Instance.MenuEstabelecimento.PreencherInfoEstabelecimento(response, aba);

            EasyAudioUtility.Instance.Play(EasyAudioUtility.Som.Error);
            //StartCoroutine(FindObjectOfType<Alerta>().ChamarAlerta(retornoAPI.msg, comunicadorAPI.PnlPrincipal));
        }));
    }
    #endregion

    #region btnSairDoEstabelecimento
    public void btnSairDoEstabelecimento()
    {
        EasyAudioUtility.Instance.Play(EasyAudioUtility.Som.Click_Cancel);

        StartCoroutine(ClienteAPI.SairDoEstabelecimento(
        (response, error) =>
        {
            if (error != null)
            {
                Debug.Log(error);
                StartCoroutine(AlertaManager.Instance.ChamarAlertaMensagem(error, false));
                return;
            }

            if (!response)
            {
                EasyAudioUtility.Instance.Play(EasyAudioUtility.Som.Error);
            }
        }));
    }
    #endregion


}
