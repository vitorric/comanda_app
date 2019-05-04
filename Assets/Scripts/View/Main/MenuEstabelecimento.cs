using System.Collections.Generic;
using System.Linq;
using APIModel;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;

public class MenuEstabelecimento : MonoBehaviour
{
    [Header("Estabelecimento Lista")]
    public GameObject PnlEstabelecimentos;
    public GameObject ScvEstabelecimento;
    public GameObject ScvEstabelecimentoContent;
    public GameObject EstabelecimentoRef;

    [Header("Estabelecimento Info")]
    public GameObject PnlEstabInfo;
    public Text TxtNomeEstabInfo;
    public Text TxtDescricaoEstabInfo;
    public Text TxtTipoEstabInfo;
    public Text TxtHorarioAtendTipoEstabInfo;
    public Text TxtPessoasNoLocalEstabInfo;
    public Text TxtCelularEstabInfo;
    public Text TxtTelefoneEstabInfo;
    public Text TxtEmailEstabInfo;
    public Text TxtEndEstabInfo;
    public Text TxtGoldEstabInfo;
    public Text TxtStatusEstabInfo;
    public Toggle BtnAbaInfo;
    public Toggle BtnAbaConquista;
    public Toggle BtnAbaShop;
    public List<GameObject> PnlAbasEdicao;

    [Header("Estabelecimento Info Conquista")]
    public GameObject ScvEstabelecimentoConquista;
    public GameObject ScvEstabelecimentoConquistaContent;
    public GameObject ConquistaRef;
    public GameObject TxtCarregandoConquista;

    [Header("Estabelecimento Info Shop")]
    public GameObject ScvEstabelecimentoShop;
    public GameObject ScvEstabelecimentoShopContent;
    public GameObject ItemShopRef;
    public Text TxtDescricaoItemShop;
    public Button BtnFecharPnlInfo;
    public Button BtnTopoFecharPnlInfo;

    [Header("Confirmarmacao Compra Shop")]
    public GameObject PnlConfirmarItemCompra;
    public RawImage IconItem;
    public Text TxtNomeCompraItem;
    public Text TxtCaixaCompraItem;
    public Text TxtCustoCompraItem;
    public Text TxtSaldoCompraItem;
    public GameObject BtnConfirmarCompraItem;


    public void BtnAbrirPnlEstabelecimento()
    {
        Main.Instance.ManipularMenus("FecharTodos");
        SomController.Tocar(SomController.Som.Click_OK);
        
        ScvEstabelecimentoContent.GetComponentsInChildren<EstabelecimentoObj>().ToList().ForEach(x => Destroy(x.gameObject));

        StartCoroutine(APIManager.Instance.Post(APIManager.URLs.ListarEstabelecimento, null, 
        (response) =>
        {
            APIManager.Retorno<List<Estabelecimento>> retornoAPI =
                JsonConvert.DeserializeObject<APIManager.Retorno<List<Estabelecimento>>>(response);

            if (retornoAPI.sucesso)
            {

                Main.Instance.PnlPopUp.SetActive(true);
                PnlEstabelecimentos.SetActive(true);
                AnimacoesTween.AnimarObjeto(PnlEstabelecimentos, AnimacoesTween.TiposAnimacoes.Scala, () =>
                {
                    foreach (Estabelecimento estabelecimento in retornoAPI.retorno)
                    {
                        GameObject objEstab = Instantiate(EstabelecimentoRef, ScvEstabelecimento.transform);
                        objEstab.transform.SetParent(ScvEstabelecimentoContent.transform);
                        objEstab.name = "EstabId_" + estabelecimento._id;

                        objEstab.GetComponent<EstabelecimentoObj>().PreencherInfo(estabelecimento, Cliente.ClienteLogado.RetornoGoldEstabelecimento(estabelecimento._id));
                    }
                }, 0.5f, new Vector2(1, 1));
            }
            else
            {
                SomController.Tocar(SomController.Som.Error);
                //StartCoroutine(comunicadorAPI.Alerta.ChamarAlerta(retornoAPI.msg, comunicadorAPI.PnlPrincipal));
            }
        },
        (error) =>
        {
            //TODO: Tratar Error
        }));
    }


    public void PreencherInfoEstabelecimento(Estabelecimento estabelecimento, bool vemDaListaDeEstab, int aba = 0)
    {
        Main.Instance.PnlPopUp.SetActive(true);

        BtnAbaInfo.isOn = false;
        BtnAbaConquista.isOn = false;
        BtnAbaShop.isOn = false;

        if (aba == 0)
            BtnAbaInfo.isOn = true;

        if (aba == 1)
            BtnAbaConquista.isOn = true;

        PnlEstabInfo.SetActive(true);
        TxtNomeEstabInfo.text = estabelecimento.nome;
        TxtDescricaoEstabInfo.text = estabelecimento.descricao;
        TxtTipoEstabInfo.text = estabelecimento.tipo;
        TxtHorarioAtendTipoEstabInfo.text = estabelecimento.horarioAtendimentoInicio + " às " + estabelecimento.horarioAtendimentoFim;
        TxtPessoasNoLocalEstabInfo.text = estabelecimento.configEstabelecimentoAtual.clientesNoLocal.Count.ToString();
        TxtCelularEstabInfo.text = estabelecimento.celular;
        TxtTelefoneEstabInfo.text = estabelecimento.telefone;
        TxtEmailEstabInfo.text = estabelecimento.emailContato;

        if (estabelecimento.endereco != null)
            TxtEndEstabInfo.text = string.Format("{0}, {1} - {2}, {3}/{4}", estabelecimento.endereco.rua, estabelecimento.endereco.numero, estabelecimento.endereco.bairro, estabelecimento.endereco.cidade, estabelecimento.endereco.estado);
        else
            TxtEndEstabInfo.text = string.Empty;
        TxtGoldEstabInfo.text = Util.FormatarValorDisponivel(Cliente.ClienteLogado.RetornoGoldEstabelecimento(estabelecimento._id));
        if (estabelecimento.configEstabelecimentoAtual.estaAberta)
        {
            TxtStatusEstabInfo.text = "ABERTO!";
            TxtStatusEstabInfo.color = Color.green;
        }
        else
        {
            TxtStatusEstabInfo.text = "FECHADO!";
            TxtStatusEstabInfo.color = Color.red;
        }

        TxtDescricaoItemShop.text = string.Empty;
        BtnFecharPnlInfo.onClick.RemoveAllListeners();
        BtnTopoFecharPnlInfo.onClick.RemoveAllListeners();

        if (vemDaListaDeEstab)
        {
            BtnFecharPnlInfo.onClick.AddListener(() => FecharPnlEstabInfo());
            BtnTopoFecharPnlInfo.onClick.AddListener(() => FecharPnlEstabInfo());
        }
        else
        {
            BtnFecharPnlInfo.onClick.AddListener(() => Main.Instance.MenuEstabelecimento.FecharPnlEstabInfo());
            BtnTopoFecharPnlInfo.onClick.AddListener(() => Main.Instance.MenuEstabelecimento.FecharPnlEstabInfo());
        }

        AnimacoesTween.AnimarObjeto(PnlEstabInfo, AnimacoesTween.TiposAnimacoes.Scala, () =>
        {

            iniciarShop(estabelecimento.itensLoja, estabelecimento.configEstabelecimentoAtual.estaAberta, estabelecimento._id);

            Main.Instance.ResgatarConquistasUsuario(estabelecimento.conquistas, estabelecimento._id);
        }, 0.2f, new Vector2(1, 1));

    }

    public void MudarAba(int numeroAba)
    {
        string nomeBotao = EventSystem.current.currentSelectedGameObject.name;

        if (!nomeBotao.Contains("EstabId") && !nomeBotao.Contains("Comanda"))
            SomController.Tocar(SomController.Som.Click_OK);

        AnimacoesTween.AnimarObjeto(EventSystem.current.currentSelectedGameObject, AnimacoesTween.TiposAnimacoes.Button_Click, () =>
        {
            PnlAbasEdicao.ForEach(x => x.SetActive(false));
            PnlAbasEdicao[numeroAba].SetActive(true);
        });
    }

    public void FecharPnlEstabInfo()
    {
        SomController.Tocar(SomController.Som.Click_Cancel);
        AnimacoesTween.AnimarObjeto(PnlEstabInfo, AnimacoesTween.TiposAnimacoes.Scala, null, 0.5f, new Vector2(0, 0));
        ScvEstabelecimentoShopContent.GetComponentsInChildren<ItemObj>().ToList().ForEach(x => Destroy(x.gameObject));
        ScvEstabelecimentoConquistaContent.GetComponentsInChildren<ConquistaObj>().ToList().ForEach(x => Destroy(x.gameObject));
        TxtCarregandoConquista.SetActive(true);
    }

    private void iniciarShop(List<Estabelecimento.ItensLoja> lojaItens, bool lojaAberta, string _idEstabelecimento)
    {

        foreach (Estabelecimento.ItensLoja item in lojaItens)
        {

            if (item._id != null)
            {
                GameObject objItemShop = Instantiate(ItemShopRef, ScvEstabelecimentoShop.transform);
                objItemShop.transform.SetParent(ScvEstabelecimentoShopContent.transform);
                objItemShop.name = "itemShop_" + item._id;
                objItemShop.GetComponent<ItemObj>().PreencherInfo(item, lojaAberta, _idEstabelecimento);
            }
        }
    }

    public void IniciarConquistas(List<Estabelecimento.Conquista> conquistas, List<Cliente.Conquista> conquistasUsuario, string _idEstabelecimento)
    {
        TxtCarregandoConquista.SetActive(false);
        foreach (Estabelecimento.Conquista conquista in conquistas)
        {
            if (conquista._id != null)
            {
                GameObject objConquista = Instantiate(ConquistaRef, ScvEstabelecimentoConquista.transform);
                objConquista.transform.SetParent(ScvEstabelecimentoConquistaContent.transform);
                objConquista.name = "conquista_" + conquista._id;
                objConquista.GetComponent<ConquistaObj>().PreencherInfo(conquista, conquistasUsuario.FirstOrDefault(x => x.conquista == conquista._id), _idEstabelecimento);
            }
        }
    }

    public void PreencherDescricaoItem(string descricao)
    {
        TxtDescricaoItemShop.text = descricao;
    }

    public void PreencherInfoConfirmacaoItem(Estabelecimento.Item item, float dinheiroEstab)
    {
        // IconItem;
        PnlConfirmarItemCompra.SetActive(true);
        TxtNomeCompraItem.text = item.nome;
        TxtCaixaCompraItem.text = dinheiroEstab.ToString();
        TxtCustoCompraItem.text = "- " + item.preco.ToString();
        TxtSaldoCompraItem.text = (dinheiroEstab - item.preco).ToString();
    }

    public void BtnFecharPnlConfirmacaoItem()
    {

        SomController.Tocar(SomController.Som.Click_Cancel);
        AnimacoesTween.AnimarObjeto(EventSystem.current.currentSelectedGameObject, AnimacoesTween.TiposAnimacoes.Button_Click, () =>
        {
            PnlConfirmarItemCompra.SetActive(false);
        });
    }

    //quando se compra um item
    public void AtualizarInfoGold(string _idEstab, int novoGold)
    {
        string gold = Util.FormatarValorDisponivel(novoGold);
        TxtGoldEstabInfo.text = gold;
        // GameObject.Find("EstabId_" + _idEstab).GetComponent<EstabelecimentoObj>().TxtGold.text = gold;
    }
}
