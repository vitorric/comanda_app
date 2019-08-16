using System;
using System.Collections.Generic;
using System.Linq;
using APIModel;
using FirebaseModel;
using Network;
using UnityEngine;
using UnityEngine.UI;

public class MenuEstabelecimento : MonoBehaviour
{
    [Header("Button Aba Config")]
    public ButtonControl buttonControl;

    [Header("Canvas")]
    public Canvas CanvasEstabInfo;
    public Canvas CanvasDesafioInfo;

    [Header("Botoes")]
    public Button BtnFecharPnlInfo;
    public Button BtnFecharPnlConfirmacaoItem;
    public Button BtnSairEstabelecimento;

    [Header("Estabelecimento Lista")]
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
        buttonControl.BtnAbas[0].onClick.AddListener(() => mudarAba(0, true));
        buttonControl.BtnAbas[1].onClick.AddListener(() => mudarAba(1, true));
        buttonControl.BtnAbas[2].onClick.AddListener(() => mudarAba(2, true));

        BtnFecharPnlConfirmacaoItem.onClick.AddListener(() => PnlPopUp.FecharPnl(PnlConfirmarItemCompra, null));
        BtnFecharPnlInfo.onClick.AddListener(() => fecharPnlEstabInfo());
        BtnSairEstabelecimento.onClick.AddListener(() => btnSairEstabelecimento());
    }
    #endregion

    #region listarEstabelecimento
    public void ListarEstabelecimento()
    {
        StartCoroutine(EstabelecimentoAPI.ListarEstabelecimento(null,
        (response, error) =>
        {
            if (error != null)
            {
                Debug.Log(error);
                StartCoroutine(AlertaManager.Instance.ChamarAlertaMensagem(error, false));
                return;
            }

            foreach (Estabelecimento estabelecimento in response)
            {
                EstabelecimentoObj objEstab = Instantiate(EstabelecimentoRef, ScvEstabelecimentos);
                objEstab.PreencherInfo(estabelecimento, Cliente.ClienteLogado.RetornoGoldEstabelecimento(estabelecimento._id));
            }
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
        buttonControl.TrocarAba(numeroAba);
    }
    #endregion

    #region btnSairEstabelecimento
    private void btnSairEstabelecimento()
    {
        if (!string.IsNullOrEmpty(Cliente.ClienteLogado.configClienteAtual.comanda))
        {
            StartCoroutine(AlertaManager.Instance.ChamarAlertaMensagem(AlertaManager.MsgAlerta.SairEstabComandaAberta, false));
            return;
        }

        StartCoroutine(ClienteAPI.SairDoEstabelecimento(
        (response, error) =>
        {
            if (error != null)
            {
                Debug.Log(error);
                StartCoroutine(AlertaManager.Instance.ChamarAlertaMensagem(error, false));
                return;
            }

            if (response)
            {
                PnlPopUp.FecharPnlCanvas(CanvasEstabInfo, PnlEstabInfo, () =>
                {
                    limparPnlEstabInfo();
                });
                return;
            }

            if (!response)
            {
                Debug.Log("error");
                EasyAudioUtility.Instance.Play(EasyAudioUtility.Som.Error);
            }
        }));

    }
    #endregion

    #region PreencherInfoEstabelecimento
    public void PreencherInfoEstabelecimento(Estabelecimento estabelecimento, int aba = 0)
    {
        try
        {
            BtnSairEstabelecimento.gameObject.SetActive(Cliente.ClienteLogado.configClienteAtual.estaEmUmEstabelecimento);

            estabelecimentoId_aberto = estabelecimento._id;

            mudarAba(aba, false);

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

            TxtGoldEstabInfo.text = Util.FormatarValores(Cliente.ClienteLogado.RetornoGoldEstabelecimento(estabelecimento._id));

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
                            removerItemShop(item);
                            return;
                        }
                    },
                    AcaoDesafio = (desafio, tipoAcao) =>
                    {
                        if (tipoAcao == EstabelecimentoFirebase.TipoAcao.Adicionar)
                        {
                            adicionarDesafio(desafio);
                            return;
                        }

                        if (tipoAcao == EstabelecimentoFirebase.TipoAcao.Modificar)
                        {
                            modificarDesafio(desafio);
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
    private void fecharPnlEstabInfo()
    {
        PnlPopUp.FecharPnlCanvas(CanvasEstabInfo, PnlEstabInfo, () =>
        {
            limparPnlEstabInfo();
        });
    }
    #endregion

    #region PararWatch
    public void PararWatch()
    {
        if (estabelecimentoFirebase != null)
            estabelecimentoFirebase.Watch_TelaEstabelecimento(estabelecimentoId_aberto, false);
    }
    #endregion

    #region limparPnlEstabInfo
    private void limparPnlEstabInfo()
    {
        estabelecimentoFirebase.Watch_TelaEstabelecimento(estabelecimentoId_aberto, false);
        estabelecimentoId_aberto = string.Empty;
        lstItensLoja.Clear();
        lstDesafios.Clear();
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

            Main.Instance.ObterIcones(item.icon, FileManager.Directories.item_Loja, (textura) =>
            {
                objItemShop.PreencherIcone(textura);
            });

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
    public void adicionarDesafio(Desafio desafio)
    {
        if (desafio._id != null)
        {
            if (Main.Instance.MenuDesafio.ConferirDesafioConcluido(desafio._id))
                return;

            Cliente.Desafio clienteDesafio = Main.Instance.MenuDesafio.BuscarDesafio(desafio._id);

            DesafioObj objDesafio = Instantiate(ConquistaRef, ScvConquista);

            Main.Instance.ObterIcones(desafio.icon, FileManager.Directories.desafio, (textura) =>
            {
                objDesafio.PreencherIcone(textura);
            });

            lstDesafios.Add(objDesafio);

            PnlConquistaVazio.SetActive(false);

            if (clienteDesafio == null)
            {
                objDesafio.PreencherInfo(desafio, null);
                return;
            }

            if (!clienteDesafio.concluido)
            {
                objDesafio.PreencherInfo(desafio, clienteDesafio);
            }
        }
    }
    #endregion

    #region modificarDesafio
    private void modificarDesafio(Desafio desafio)
    {
        DesafioObj objItemDesafio = lstDesafios.Find(x => x.Desafio._id == desafio._id);

        if (objItemDesafio != null)
        {
            Cliente.Desafio clienteDesafio = Main.Instance.MenuDesafio.BuscarDesafio(desafio._id);
            objItemDesafio.PreencherInfo(desafio, clienteDesafio);
        }
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
        string gold = Util.FormatarValores(novoGold);
        TxtGoldEstabInfo.text = gold;
    }
    #endregion

}
