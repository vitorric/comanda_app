using System;
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
    [Header("Botoes")]
    public Button BtnAbrirListaEstab;
    public Button BtnFecharListaEstab;
    public Button BtnFecharPnlInfo;
    public Button BtnFecharPnlConfirmacaoItem;

    [Header("Toggle")]
    public Toggle BtnAbaInfo;
    public Toggle BtnAbaConquista;
    public Toggle BtnAbaShop;

    [Header("Estabelecimento Lista")]
    public GameObject PnlEstabelecimentos;
    public Transform ScvEstabelecimentos;
    public EstabelecimentoObj EstabelecimentoRef;

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
    public List<GameObject> PnlAbasEdicao;

    [Header("Estabelecimento Info Conquista")]
    public Transform ScvConquista;
    public ConquistaObj ConquistaRef;
    public GameObject TxtCarregandoConquista;

    [Header("Estabelecimento Info Shop")]
    public Transform ScvShop;
    public ItemObj ItemShopRef;
    public Text TxtDescricaoItemShop;

    [Header("Confirmarmacao Compra Shop")]
    public GameObject PnlConfirmarItemCompra;
    public RawImage IconItem;
    public Text TxtNomeCompraItem;
    public Text TxtCaixaCompraItem;
    public Text TxtCustoCompraItem;
    public Text TxtSaldoCompraItem;
    public Button BtnConfirmarCompraItem;

    private void Awake()
    {
        configurarListener();
    }

    #region configurarListener
    private void configurarListener()
    {
        BtnAbrirListaEstab.onClick.AddListener(() => btnAbrirPnlEstabelecimento());
        BtnFecharListaEstab.onClick.AddListener(() => PnlPopUp.FecharPopUp(PnlEstabelecimentos, () =>
        {
            ScvEstabelecimentos.GetComponentsInChildren<EstabelecimentoObj>().ToList().ForEach(x => Destroy(x.gameObject));

            PnlAbasEdicao.ForEach(x => x.SetActive(false));
            PnlAbasEdicao[0].SetActive(true);
        }));

        BtnAbaInfo.onValueChanged.AddListener((BtnAbaInfo) => mudarAba(0, true));
        BtnAbaConquista.onValueChanged.AddListener((BtnAbaConquista) => mudarAba(1, true));
        BtnAbaShop.onValueChanged.AddListener((BtnAbaShop) => mudarAba(2, true));

        BtnFecharPnlConfirmacaoItem.onClick.AddListener(() => PnlPopUp.FecharPnl(PnlConfirmarItemCompra, null));
    }
    #endregion

    #region btnAbrirPnlEstabelecimento
    private void btnAbrirPnlEstabelecimento()
    {
        Main.Instance.ManipularMenus("FecharTodos");

        ScvEstabelecimentos.GetComponentsInChildren<EstabelecimentoObj>().ToList().ForEach(x => Destroy(x.gameObject));

        listarEstabelecimento((lstEstabelecimentos) =>
        {
            PnlPopUp.AbrirPopUp(PnlEstabelecimentos,
             () =>
             {
                 foreach (Estabelecimento estabelecimento in lstEstabelecimentos)
                 {
                     EstabelecimentoObj objEstab = Instantiate(EstabelecimentoRef, ScvEstabelecimentos);
                     objEstab.PreencherInfo(estabelecimento, Cliente.ClienteLogado.RetornoGoldEstabelecimento(estabelecimento._id));
                 }
             });
        });
    }
    #endregion

    #region listarEstabelecimento
    private void listarEstabelecimento(Action<List<Estabelecimento>> callback)
    {
        StartCoroutine(APIManager.Instance.Post(APIManager.URLs.ListarEstabelecimento, null,
        (response) =>
        {
            APIManager.Retorno<List<Estabelecimento>> retornoAPI =
                JsonConvert.DeserializeObject<APIManager.Retorno<List<Estabelecimento>>>(response);

            if (retornoAPI.sucesso)
            {
                callback(retornoAPI.retorno);
            }
            else
            {
                EasyAudioUtility.Instance.Play(EasyAudioUtility.Som.Error);
                callback(new List<Estabelecimento>());
                //StartCoroutine(comunicadorAPI.Alerta.ChamarAlerta(retornoAPI.msg, comunicadorAPI.PnlPrincipal));
            }
        },
        (error) =>
        {
            //TODO: Tratar Error
        }));
    }
    #endregion    

    #region mudarAba
    private void mudarAba(int numeroAba, bool tocarSom = false)
    {
        if (tocarSom)
            EasyAudioUtility.Instance.Play(EasyAudioUtility.Som.Click_OK);

        AnimacoesTween.AnimarObjeto(EventSystem.current.currentSelectedGameObject,
        AnimacoesTween.TiposAnimacoes.Button_Click, () =>
        {
            PnlAbasEdicao.ForEach(x => x.SetActive(false));
            PnlAbasEdicao[numeroAba].SetActive(true);
        },
        AppManager.TEMPO_ANIMACAO_ABRIR_CLICK_BOTAO);
    }
    #endregion

    #region PreencherInfoEstabelecimento
    public void PreencherInfoEstabelecimento(Estabelecimento estabelecimento, bool vemDaListaDeEstab, int aba = 0)
    {
        try
        {

            BtnAbaInfo.isOn = false;
            BtnAbaConquista.isOn = false;
            BtnAbaShop.isOn = false;

            if (aba == 0)
                BtnAbaInfo.isOn = true;

            if (aba == 1)
                BtnAbaConquista.isOn = true;

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

            if (vemDaListaDeEstab)
            {
                BtnFecharPnlInfo.onClick.AddListener(() => fecharPnlEstabInfo(false));
            }
            else
            {
                BtnFecharPnlInfo.onClick.AddListener(() => fecharPnlEstabInfo(true));
            }

            PnlPopUp.AbrirPopUp(PnlEstabInfo,
            () =>
            {
                iniciarShop(estabelecimento.itensLoja, estabelecimento.configEstabelecimentoAtual.estaAberta, estabelecimento._id);

                Main.Instance.ResgatarConquistasUsuario(estabelecimento.conquistas, estabelecimento._id);
            });
        }
        catch (Exception e)
        {
            Debug.Log(Util.GetExceptionDetails(e));
        }
    }

    #endregion

    #region fecharPnlEstabInfo
    private void fecharPnlEstabInfo(bool fecharPopup)
    {
        if (fecharPopup)
        {
            PnlPopUp.FecharPopUp(PnlEstabInfo, () =>
            {
                limparPnlEstabInfo();
            });
            return;
        }

        PnlPopUp.FecharPnl(PnlEstabInfo, () =>
        {
            limparPnlEstabInfo();
        });
    }
    #endregion

    private void limparPnlEstabInfo()
    {
        ScvShop.GetComponentsInChildren<ItemObj>().ToList().ForEach(x => Destroy(x.gameObject));
        ScvConquista.GetComponentsInChildren<ConquistaObj>().ToList().ForEach(x => Destroy(x.gameObject));
        TxtCarregandoConquista.SetActive(true);
    }

    #region iniciarShop
    private void iniciarShop(List<Estabelecimento.ItensLoja> lojaItens, bool lojaAberta, string _idEstabelecimento)
    {
        foreach (Estabelecimento.ItensLoja item in lojaItens)
        {
            if (item._id != null)
            {
                ItemObj objItemShop = Instantiate(ItemShopRef, ScvShop.transform);
                objItemShop.PreencherInfo(item, lojaAberta, _idEstabelecimento);
            }
        }
    }
    #endregion

    #region IniciarConquistas
    public void IniciarConquistas(List<Estabelecimento.Conquista> conquistas, List<Cliente.Conquista> conquistasUsuario, string _idEstabelecimento)
    {
        TxtCarregandoConquista.SetActive(false);

        foreach (Estabelecimento.Conquista conquista in conquistas)
        {
            if (conquista._id != null)
            {
                ConquistaObj objConquista = Instantiate(ConquistaRef, ScvConquista);
                objConquista.PreencherInfo(conquista, conquistasUsuario.FirstOrDefault(x => x.conquista == conquista._id), _idEstabelecimento);
            }
        }
    }
    #endregion

    #region PreencherDescricaoItem
    public void PreencherDescricaoItem(string descricao)
    {
        TxtDescricaoItemShop.text = descricao;
    }
    #endregion

    #region PreencherInfoConfirmacaoItem
    public void PreencherInfoConfirmacaoItem(Estabelecimento.Item item, float dinheiroEstab)
    {
        // IconItem;
        PnlConfirmarItemCompra.SetActive(true);
        TxtNomeCompraItem.text = item.nome;
        TxtCaixaCompraItem.text = dinheiroEstab.ToString();
        TxtCustoCompraItem.text = "- " + item.preco.ToString();
        TxtSaldoCompraItem.text = (dinheiroEstab - item.preco).ToString();
    }
    #endregion

    #region AtualizarInfoGold
    //quando se compra um item
    public void AtualizarInfoGold(string _idEstab, int novoGold)
    {
        string gold = Util.FormatarValorDisponivel(novoGold);
        TxtGoldEstabInfo.text = gold;
    }
    #endregion
}
