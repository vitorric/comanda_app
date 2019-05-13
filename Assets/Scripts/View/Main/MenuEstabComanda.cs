using System;
using System.Collections.Generic;
using System.Linq;
using APIModel;
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
        WWWForm form = new WWWForm();
        form.AddField("_idEstabelecimento", Cliente.ClienteLogado.configClienteAtual.estabelecimento);

        StartCoroutine(APIManager.Instance.Post(APIManager.URLs.ObterEstabelecimento, form, (response) =>
        {
            APIManager.Retorno<Estabelecimento> retornoAPI =
                       JsonConvert.DeserializeObject<APIManager.Retorno<Estabelecimento>>(response);

            if (retornoAPI.sucesso)
            {
                Main.Instance.MenuEstabelecimento.PreencherInfoEstabelecimento(retornoAPI.retorno, false, aba);
            }
            else
            {
                EasyAudioUtility.Instance.Play(EasyAudioUtility.Som.Error);
                //StartCoroutine(FindObjectOfType<Alerta>().ChamarAlerta(retornoAPI.msg, comunicadorAPI.PnlPrincipal));
            }
        },
        (error) =>
        {
            //TODO: Tratar Error
        }));
    }
    #endregion

    #region btnSairDoEstabelecimento
    public void btnSairDoEstabelecimento()
    {
        EasyAudioUtility.Instance.Play(EasyAudioUtility.Som.Click_Cancel);

        AnimacoesTween.AnimarObjeto(BtnSairEstabelecimento.gameObject, AnimacoesTween.TiposAnimacoes.Button_Click, () =>
        {
            WWWForm form = new WWWForm();
            form.AddField("_idCliente", Cliente.ClienteLogado._id);
            form.AddField("_idEstabelecimento", Cliente.ClienteLogado.configClienteAtual.estabelecimento);

            StartCoroutine(APIManager.Instance.Post(APIManager.URLs.SairDoEstabelecimento, form,
            (response) =>
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
            },
            (error) =>
            {
                //TODO: Tratar Error
            }));
        },
        AppManager.TEMPO_ANIMACAO_ABRIR_CLICK_BOTAO);

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
