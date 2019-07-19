﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using APIModel;
using Network;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ItemObj : MonoBehaviour
{
    private MenuEstabelecimento menuEstabelecimento;

    [Header("Botoes")]
    public Button BtnSelecionarItem;
    public Button BtnComprarItem;

    public Text TxtNome;
    public Text TxtPreco;
    public RawImage Icon;
    public GameObject IconSelecionado;
    public GameObject BtnComprar;
    public GameObject PnlAlerta;
    public Text TxtAlerta;
    public List<Color> CorPnlAlerta;
    public List<Color> CorTxtAlerta;
    public GameObject PnlTempo;
    public GameObject PnlHotSale;
    public Text TxtTempo;

    public ItemLoja ItemLoja;
    private string estabelecimentoId;
    private bool pararConferenciaTempo = false;
    private bool lojaAberta = false;

    #region PreencherInfo
    public void PreencherInfo(ItemLoja itemLoja, bool lojaAberta, string _idEstabelecimento)
    {
        this.lojaAberta = lojaAberta;
        PnlHotSale.SetActive(itemLoja.hotSale);

        this.ItemLoja = itemLoja;
        this.estabelecimentoId = _idEstabelecimento;

        TxtNome.text = itemLoja.nome;
        TxtPreco.text = Util.FormatarValores(itemLoja.preco);

        configurarPainelAlerta();

        configurarListener();
        rodarRelogio();
    }
    #endregion

    #region rodarRelogio
    void rodarRelogio()
    {
        TimeSpan ts = ItemLoja.tempoDisponivel.ToLocalTime().Subtract((DateTime.Now.ToLocalTime()));

        if (ts.TotalSeconds <= 0)
        {
            pararConferenciaTempo = true;
            configurarPainelAlerta();
        }
        else
        {
            TxtTempo.text = string.Format("{0:00}:{1:00}:{2:00}", ts.Hours + (ts.Days * 24), ts.Minutes, ts.Seconds);
            //txtTime.text = ts.ToString(@"hh\h\ mm\m\ ss\s");
            Invoke("rodarRelogio", 1f);
        }
    }
    #endregion

    #region configurarListener
    private void configurarListener()
    {
        BtnSelecionarItem.onClick.AddListener(() => btnSelecionarItem());
        BtnComprarItem.onClick.AddListener(() => btnConfirmarComprarItem());
    }
    #endregion

    #region btnSelecionarItem
    private void btnSelecionarItem()
    {
        FindObjectsOfType<ItemObj>().ToList().ForEach(x => x.IconSelecionado.SetActive(false));
        IconSelecionado.SetActive(true);

        EasyAudioUtility.Instance.Play(EasyAudioUtility.Som.Click_OK);

        Main.Instance.MenuEstabelecimento.PreencherDescricaoItem(ItemLoja.descricao);
    }
    #endregion

    #region configurarPainelAlerta
    private void configurarPainelAlerta()
    {
        string alerta = string.Empty;
        int corAlerta = 0;

        if (!lojaAberta)
        {
            alerta = "Loja Fechada";
        }
        else if (pararConferenciaTempo)
        {
            PnlTempo.SetActive(false);
            alerta = "Tempo de Compra Esgotado";
            corAlerta = 1;
        }
        else if (!Cliente.ClienteLogado.configClienteAtual.estaEmUmEstabelecimento && Cliente.ClienteLogado.configClienteAtual.estabelecimento != estabelecimentoId)
        {
            alerta = "É necessário estar no estabelecimento";
            corAlerta = 3;
        }
        else if (ItemLoja.quantidadeDisponivel == 0)
        {
            alerta = "Esgotado";
            corAlerta = 2;
        }


        if (!string.IsNullOrEmpty(alerta))
        {
            TxtAlerta.text = alerta;
            BtnComprar.SetActive(false);
            PnlAlerta.GetComponent<Image>().color = CorPnlAlerta[corAlerta];
            TxtAlerta.color = CorTxtAlerta[corAlerta];
            PnlAlerta.SetActive(true);
        }
    }
    #endregion

    #region btnConfirmarComprarItem
    private void btnConfirmarComprarItem()
    {
        EasyAudioUtility.Instance.Play(EasyAudioUtility.Som.Click_OK);

        int gold = Cliente.ClienteLogado.RetornoGoldEstabelecimento(estabelecimentoId);

        if (ItemLoja.preco > gold)
        {
            EasyAudioUtility.Instance.Play(EasyAudioUtility.Som.Error);
            return;
        }

        Button btnConfirmarCompra = Main.Instance.MenuEstabelecimento.BtnConfirmarCompraItem;

        btnConfirmarCompra.onClick.RemoveAllListeners();
        btnConfirmarCompra.onClick.AddListener(() => confirmarCompraItem(btnConfirmarCompra.gameObject));

        Main.Instance.MenuEstabelecimento.PreencherInfoConfirmacaoItem(ItemLoja, gold);
    }
    #endregion

    #region confirmarCompraItem
    private void confirmarCompraItem(GameObject objClicado)
    {
        EasyAudioUtility.Instance.Play(EasyAudioUtility.Som.Compra_Item);

        Main.Instance.MenuEstabelecimento.PnlConfirmarItemCompra.SetActive(false);
        Cliente.Dados usuario = Cliente.ClienteLogado;

        Dictionary<string, object> data = new Dictionary<string, object>
            {
                { "estabelecimento", estabelecimentoId },
                { "itemLoja", ItemLoja._id }
            };

        StartCoroutine(ClienteAPI.ClienteComprarItem(data,
        (response, error) =>
        {
            APIManager.Retorno<string> retornoAPI =
                JsonConvert.DeserializeObject<APIManager.Retorno<string>>(response);

            if (retornoAPI.sucesso)
            {
                int novoGold = usuario.AlterarGoldEstabelecimento(estabelecimentoId, (int)ItemLoja.preco, false);
                Cliente.ClienteLogado = usuario;

                Main.Instance.MenuEstabelecimento.AtualizarInfoGold(estabelecimentoId, novoGold);

                ItemLoja.quantidadeDisponivel -= 1;
                configurarPainelAlerta();
            }
            else
            {
                EasyAudioUtility.Instance.Play(EasyAudioUtility.Som.Error);
            }

            //StartCoroutine(FindObjectOfType<Alerta>().ChamarAlerta(retornoAPI.msg, comunicadorAPI.PnlPrincipal));

        }));
    }
    #endregion

}
