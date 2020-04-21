using System;
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

    private void Awake()
    {
        configurarListener();
    }

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

        rodarRelogio();
    }
    #endregion

    #region PreencherIcone
    public void PreencherIcone(Texture2D icone)
    {
        Icon.texture = icone;
        Icon = Util.ImgResize(Icon, 180, 180);
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
            if (ts.Days > 0)
            {
                TxtTempo.text = string.Format("{0:0}d {1:0}h", ts.Days, ts.Hours);
            }
            else if (ts.Days == 0 && ts.Hours > 0)
            {
                TxtTempo.text = string.Format("{0:0}h {1:0}m", ts.Hours, ts.Minutes);
            }
            else if (ts.Hours == 0 && ts.Minutes > 0)
            {
                TxtTempo.text = string.Format("{0:0}m", ts.Minutes);
            }
            else if (ts.Minutes == 0)
            {
                TxtTempo.text = string.Format("{0:0}s", ts.Seconds);
            }

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
            Debug.Log(Cliente.ClienteLogado.configClienteAtual.estabelecimento);
            Debug.Log(estabelecimentoId);
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
        else
        {
            BtnComprar.SetActive(true);
            PnlAlerta.SetActive(false);
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
            AlertaManager.Instance.ChamarAlertaMensagem(AlertaManager.MsgAlerta.CPGoldInsuficiente, false);
            return;
        }

        Button btnConfirmarCompra = Main.Instance.MenuEstabelecimento.BtnConfirmarCompraItem;

        btnConfirmarCompra.onClick.RemoveAllListeners();
        btnConfirmarCompra.onClick.AddListener(() => confirmarCompraItem());

        Main.Instance.MenuEstabelecimento.PreencherInfoConfirmacaoItem(ItemLoja, gold);
    }
    #endregion

    #region confirmarCompraItem
    private void confirmarCompraItem()
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
            if (error != null)
            {
                Debug.Log(error);
                AlertaManager.Instance.ChamarAlertaMensagem(error, false);
                return;
            }

            AlertaManager.Instance.ChamarAlertaResponse(true);
        }));
    }
    #endregion

}
