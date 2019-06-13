using System;
using System.Collections.Generic;
using System.Linq;
using APIModel;
using Network;
using UnityEngine;
using UnityEngine.UI;

public class MenuEstabelecimento : MonoBehaviour
{
    [Header("Canvas")]
    public Canvas CanvasEstabs;
    public Canvas CanvasEstabInfo;

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
    public DesafioObj ConquistaRef;
    public GameObject TxtCarregandoConquista;
    public GameObject PnlConquistaVazio;

    [Header("Estabelecimento Info Shop")]
    public Transform ScvShop;
    public ItemObj ItemShopRef;
    public Text TxtDescricaoItemShop;
    public GameObject PnlShopVazio;

    [Header("Confirmarmacao Compra Shop")]
    public GameObject PnlConfirmarItemCompra;
    public RawImage IconItem;
    public Text TxtNomeCompraItem;
    public Text TxtCaixaCompraItem;
    public Text TxtCustoCompraItem;
    public Text TxtSaldoCompraItem;
    public Button BtnConfirmarCompraItem;

    public DesafioInfo DesafioInfo;

    private EstabelecimentoFirebase estabelecimentoFirebase;
    private List<ItemObj> lstItensLoja;
    private List<DesafioObj> lstDesafios;
    private string estabelecimentoId_aberto;

    private void Awake()
    {
        lstItensLoja = new List<ItemObj>();
        lstDesafios = new List<DesafioObj>();
        configurarListener();
    }

    #region configurarListener
    private void configurarListener()
    {
        BtnAbrirListaEstab.onClick.AddListener(() => btnAbrirPnlEstabelecimento());
        BtnFecharListaEstab.onClick.AddListener(() => PnlPopUp.FecharPopUp(CanvasEstabs, PnlEstabelecimentos, () =>
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
            PnlPopUp.AbrirPopUpCanvas(CanvasEstabs, 
             PnlEstabelecimentos,
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
        StartCoroutine(EstabelecimentoAPI.ListarEstabelecimento(null,
        (response, error) =>
        {
            callback(response);
        }));
    }
    #endregion    

    #region mudarAba
    private void mudarAba(int numeroAba, bool tocarSom = false)
    {
        if (tocarSom)
            EasyAudioUtility.Instance.Play(EasyAudioUtility.Som.Click_OK);

        PnlAbasEdicao.ForEach(x => x.SetActive(false));
        PnlAbasEdicao[numeroAba].SetActive(true);
    }
    #endregion

    #region PreencherInfoEstabelecimento
    public void PreencherInfoEstabelecimento(Estabelecimento estabelecimento, bool vemDaListaDeEstab, int aba = 0)
    {
        try
        {
            estabelecimentoId_aberto = estabelecimento._id;

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

            PnlPopUp.AbrirPopUpCanvas(CanvasEstabInfo,
            PnlEstabInfo,
            () =>
            {
                estabelecimentoFirebase = new EstabelecimentoFirebase
                {
                    AcaoItemLoja = (item, tipoAcao) =>
                    {
                        if (tipoAcao == EstabelecimentoFirebase.TipoAcao.Adicionar)
                        {
                            adicionarItemShop(item, estabelecimento.configEstabelecimentoAtual.estaAberta, estabelecimento._id);
                            return;
                        }

                        if (tipoAcao == EstabelecimentoFirebase.TipoAcao.Modificar)
                        {
                            modificarItemShop(item, estabelecimento.configEstabelecimentoAtual.estaAberta, estabelecimento._id);
                            return;
                        }

                        if (tipoAcao == EstabelecimentoFirebase.TipoAcao.Remover)
                        {
                            //ScvShop.GetComponentsInChildren<ItemObj>().ToList().ForEach(x => Destroy(x.gameObject));
                            removerItemShop(item);
                            return;
                        }
                    },
                    AcaoDesafio = (desafio, tipoAcao) =>
                    {
                        if (tipoAcao == EstabelecimentoFirebase.TipoAcao.Adicionar)
                        {
                            adicionarDesafio(desafio, estabelecimento._id);
                            return;
                        }

                        if (tipoAcao == EstabelecimentoFirebase.TipoAcao.Modificar)
                        {
                            modificarDesafio(desafio, estabelecimento._id);
                            return;
                        }

                        if (tipoAcao == EstabelecimentoFirebase.TipoAcao.Remover)
                        {
                            //ScvShop.GetComponentsInChildren<ItemObj>().ToList().ForEach(x => Destroy(x.gameObject));
                            removerDesafio(desafio);
                            return;
                        }
                    }
                };

                estabelecimentoFirebase.Watch_TelaEstabelecimento(estabelecimento._id, true);

                //Main.Instance.ResgatarConquistasUsuario(estabelecimento.conquistas, estabelecimento._id);
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
            PnlPopUp.FecharPopUp(CanvasEstabInfo,PnlEstabInfo, () =>
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

    #region limparPnlEstabInfo
    private void limparPnlEstabInfo()
    {
        estabelecimentoFirebase.Watch_TelaEstabelecimento(estabelecimentoId_aberto, false);
        estabelecimentoId_aberto = string.Empty;
        ScvShop.GetComponentsInChildren<ItemObj>().ToList().ForEach(x => Destroy(x.gameObject));
        ScvConquista.GetComponentsInChildren<DesafioObj>().ToList().ForEach(x => Destroy(x.gameObject));
    }
    #endregion

    #region adicionarItemShop
    private void adicionarItemShop(ItemLoja item, bool lojaAberta, string estabelecimentoId)
    {
        if (item._id != null)
        {
            ItemObj objItemShop = Instantiate(ItemShopRef, ScvShop.transform);

            objItemShop.PreencherInfo(item, lojaAberta, estabelecimentoId);
            lstItensLoja.Add(objItemShop);

            PnlShopVazio.SetActive(false);
        }
    }
    #endregion

    #region modificarItemShop
    private void modificarItemShop(ItemLoja item, bool lojaAberta, string estabelecimentoId)
    {
        ItemObj objItemShop = lstItensLoja.Find(x => x.ItemLoja._id == item._id);

        if (objItemShop != null)
            objItemShop.PreencherInfo(item, lojaAberta, estabelecimentoId);
    }
    #endregion

    #region removerItemShop
    private void removerItemShop(ItemLoja item)
    {
        ItemObj objItemShop = lstItensLoja.Find(x => x.ItemLoja._id == item._id);

        if (objItemShop != null)
        {
            Destroy(objItemShop.gameObject);
            lstItensLoja.Remove(objItemShop);

            if (lstItensLoja.Count == 0)
                PnlShopVazio.SetActive(true);
        }

    }
    #endregion

    #region adicionarDesafio
    public void adicionarDesafio(Desafio desafio, string estabelecimentoId)
    {
        if (desafio._id != null)
        {
            DesafioObj objDesafio = Instantiate(ConquistaRef, ScvConquista);
            objDesafio.PreencherInfo(desafio, estabelecimentoId);

            lstDesafios.Add(objDesafio);

            PnlConquistaVazio.SetActive(false);
        }
    }
    #endregion

    #region modificarDesafio
    private void modificarDesafio(Desafio desafio, string estabelecimentoId)
    {
        DesafioObj objItemDesafio = lstDesafios.Find(x => x.Desafio._id == desafio._id);

        if (objItemDesafio != null)
            objItemDesafio.PreencherInfo(desafio, estabelecimentoId);
    }
    #endregion

    #region removerDesafio
    private void removerDesafio(Desafio desafio)
    {
        DesafioObj objItemDesafio = lstDesafios.Find(x => x.Desafio._id == desafio._id);


        if (objItemDesafio != null)
        {
            Destroy(objItemDesafio.gameObject);
            lstDesafios.Remove(objItemDesafio);

            if (lstDesafios.Count == 0)
                PnlConquistaVazio.SetActive(true);
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
    public void PreencherInfoConfirmacaoItem(ItemLoja item, float dinheiroEstab)
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
