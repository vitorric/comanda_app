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

    [HideInInspector]
    public bool MenuAtivo = false;

    private List<GameObject> lstMenu;

    private void Awake()
    {
        lstMenu = new List<GameObject>
        {
            BtnEstabInfoComanda.gameObject,
            BtnConquistaComanda.gameObject,
            BtnSairEstabelecimento.gameObject
        };

        configurarListener();
    }

    #region configurarListener
    private void configurarListener()
    {
        BtnMenuEstabComanda.onClick.AddListener(() => BtnAbrirMenuEstabelecimentoComanda());
        BtnEstabInfoComanda.onClick.AddListener(() => btnAbrirInfoEstabelecimento(0));
        BtnConquistaComanda.onClick.AddListener(() => btnAbrirInfoEstabelecimento(1));
        BtnSairEstabelecimento.onClick.AddListener(() => btnSairDoEstabelecimento());
    }
    #endregion

    #region BtnAbrirMenuEstabelecimentoComanda
    public void BtnAbrirMenuEstabelecimentoComanda(bool fecharAutomatico = false)
    {
        MenuAtivo = (fecharAutomatico) ? false : !MenuAtivo;

        Main.Instance.AbrirMenu("BtnEstabelecimentoComanda", (fecharAutomatico) ? false : MenuAtivo, lstMenu, fecharAutomatico);
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
        Dictionary<string, string> form = new Dictionary<string, string>
        {
            { "_idEstabelecimento", Cliente.ClienteLogado.configClienteAtual.estabelecimento }
        };

        StartCoroutine(EstabelecimentoAPI.ObterEstabelecimento(form,
        (response, error) =>
        {
            Main.Instance.MenuEstabelecimento.PreencherInfoEstabelecimento(response, false, aba);

            EasyAudioUtility.Instance.Play(EasyAudioUtility.Som.Error);
            //StartCoroutine(FindObjectOfType<Alerta>().ChamarAlerta(retornoAPI.msg, comunicadorAPI.PnlPrincipal));
        }));
    }
    #endregion

    #region btnSairDoEstabelecimento
    public void btnSairDoEstabelecimento()
    {
        EasyAudioUtility.Instance.Play(EasyAudioUtility.Som.Click_Cancel);

        Dictionary<string, string> form = new Dictionary<string, string>
        {
            { "_idCliente", Cliente.ClienteLogado._id },
            { "_idEstabelecimento", Cliente.ClienteLogado.configClienteAtual.estabelecimento }
        };

        StartCoroutine(ClienteAPI.SairDoEstabelecimento(
        form,
        (response, error) =>
        {
            APIManager.Retorno<string> retornoAPI = JsonConvert.DeserializeObject<APIManager.Retorno<string>>(response);

            if (retornoAPI.sucesso)
            {
                Main.Instance.ManipularMenus("FecharTodos");
                Cliente.ClienteLogado.configClienteAtual.estaEmUmEstabelecimento = false;
                Cliente.ClienteLogado.configClienteAtual.estabelecimento = null;
                Cliente.ClienteLogado.configClienteAtual.nomeEstabelecimento = null;
                Main.Instance.ClienteEstaNoEstabelecimento();
            }
            else
            {
                EasyAudioUtility.Instance.Play(EasyAudioUtility.Som.Error);
                //StartCoroutine(FindObjectOfType<Alerta>().ChamarAlerta(retornoAPI.msg, comunicadorAPI.PnlPrincipal));
            }
        }));

    }
    #endregion




    //public void FecharPnlEstabInfo()
    //{
    //    EasyAudioUtility.Instance.Play(EasyAudioUtility.Som.Click_Cancel);
    //    AnimacoesTween.AnimarObjeto(EventSystem.current.currentSelectedGameObject, AnimacoesTween.TiposAnimacoes.Button_Click, () =>
    //    {
    //        Main.Instance.PnlPopUp.SetActive(false);
    //        AnimacoesTween.AnimarObjeto(Main.Instance.MenuEstabelecimento.PnlEstabInfo, AnimacoesTween.TiposAnimacoes.Scala, () =>
    //        {
    //            Main.Instance.MenuEstabelecimento.PnlEstabInfo.SetActive(false);
    //        }, 0.1f, new Vector2(0, 0));
    //    },
    //    0.1f);

    //    Main.Instance.MenuEstabelecimento.ScvEstabelecimentoShopContent.GetComponentsInChildren<ItemObj>().ToList().ForEach(x => Destroy(x.gameObject));
    //}

}
