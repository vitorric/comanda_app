using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using APIModel;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ItemObj : MonoBehaviour
{
    private MenuEstabelecimento menuEstabelecimento;

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
    private Estabelecimento.ItensLoja itemLoja;
    private string _idEstabelecimento;
    private bool pararConferenciaTempo = false;
    private bool lojaAberta = false;

    void Update()
    {
        if (!pararConferenciaTempo)
        {
            pararConferenciaTempo = !rodarRelogio();
            if (pararConferenciaTempo == false)
            {
                TimeSpan data = itemLoja.tempoDisponivel.ToLocalTime().Subtract((DateTime.Now.ToLocalTime()));
                TxtTempo.text = string.Format("{0:00}:{1:00}:{2:00}", data.Hours + (data.Days * 24), data.Minutes, data.Seconds);
            }
            else
            {
                pararConferenciaTempo = true;
                configurarPainelAlerta();
            }
        }
    }

    public void PreencherInfo(Estabelecimento.ItensLoja itemLoja, bool lojaAberta, string _idEstabelecimento)
    {
        this.lojaAberta = lojaAberta;
        PnlHotSale.SetActive(itemLoja.hotSale);

        this.itemLoja = itemLoja;
        this._idEstabelecimento = _idEstabelecimento;

        TxtNome.text = itemLoja.item.nome;
        TxtPreco.text = Util.FormatarValorDisponivel(itemLoja.item.preco);

        configurarPainelAlerta();
    }

    public bool rodarRelogio()
    {
        return (itemLoja.tempoDisponivel.ToLocalTime().Subtract((DateTime.Now.ToLocalTime())).TotalSeconds > 0) ? true : false;
    }

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
        else if (!Cliente.ClienteLogado.configClienteAtual.estaEmUmEstabelecimento && Cliente.ClienteLogado.configClienteAtual.estabelecimento != _idEstabelecimento)
        {
            alerta = "É necessário estar no estabelecimento";
            corAlerta = 3;
        }
        else if (itemLoja.quantidadeDisponivel == 0)
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

    public void BtnSelecionarItem()
    {
        FindObjectsOfType<ItemObj>().ToList().ForEach(x => x.IconSelecionado.SetActive(false));
        IconSelecionado.SetActive(true);
        SomController.Tocar(SomController.Som.Click_OK);
        AnimacoesTween.AnimarObjeto(EventSystem.current.currentSelectedGameObject, AnimacoesTween.TiposAnimacoes.Button_Click, () =>
        {
            Main.Instance.MenuEstabelecimento.PreencherDescricaoItem(itemLoja.item.descricao);
        });
    }

    public void BtnComprarItem()
    {
        SomController.Tocar(SomController.Som.Click_OK);
        AnimacoesTween.AnimarObjeto(EventSystem.current.currentSelectedGameObject, AnimacoesTween.TiposAnimacoes.Button_Click, () =>
        {
            int gold = Cliente.ClienteLogado.RetornoGoldEstabelecimento(_idEstabelecimento);

            if (itemLoja.item.preco > gold)
            {
                SomController.Tocar(SomController.Som.Error);
                //StartCoroutine(FindObjectOfType<Alerta>().ChamarAlerta(Alerta.MsgAlerta.SemDinheiro, FindObjectOfType<ComunicadorAPI>().PnlPrincipal));
                return;
            }

            GameObject btnConfirmarCompra = Main.Instance.MenuEstabelecimento.BtnConfirmarCompraItem;
            btnConfirmarCompra.GetComponent<Button>().onClick.RemoveAllListeners();
            btnConfirmarCompra.GetComponent<Button>().onClick.AddListener(() => confirmarCompraItem());
            Main.Instance.MenuEstabelecimento.PreencherInfoConfirmacaoItem(itemLoja.item, gold);
        });
    }

    private void confirmarCompraItem()
    {
        SomController.Tocar(SomController.Som.Compra_Item);
        AnimacoesTween.AnimarObjeto(EventSystem.current.currentSelectedGameObject, AnimacoesTween.TiposAnimacoes.Button_Click, () =>
        {
            Main.Instance.MenuEstabelecimento.PnlConfirmarItemCompra.SetActive(false);
            Cliente.Dados usuario = Cliente.ClienteLogado;

            WWWForm form = new WWWForm();
            form.AddField("cliente", usuario._id);
            form.AddField("estabelecimento", _idEstabelecimento);
            form.AddField("itemLoja", itemLoja._id);

            StartCoroutine(APIManager.Instance.Post(APIManager.URLs.ClienteComprarItem, form,
            (response) =>
            {
                APIManager.Retorno<string> retornoAPI =
                    JsonConvert.DeserializeObject<APIManager.Retorno<string>>(response);

                if (retornoAPI.sucesso)
                {
                    int novoGold = usuario.AlterarGoldEstabelecimento(_idEstabelecimento, (int)itemLoja.item.preco, false);
                    Cliente.ClienteLogado = usuario;

                    Main.Instance.MenuEstabelecimento.AtualizarInfoGold(_idEstabelecimento, novoGold);

                    itemLoja.quantidadeDisponivel -= 1;
                    Main.Instance.AdicionarExp(Configuracoes.LevelSystem.Acao.CompraItem, (int)itemLoja.item.preco);
                    configurarPainelAlerta();
                }
                else
                {
                    SomController.Tocar(SomController.Som.Error);
                }

                //StartCoroutine(FindObjectOfType<Alerta>().ChamarAlerta(retornoAPI.msg, comunicadorAPI.PnlPrincipal));

            },
            (error) =>
            {
                //TODO: Tratar Error
            }));
        });
    }
}
