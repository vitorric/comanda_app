using APIModel;
using FirebaseModel;
using Network;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MenuComanda : MonoBehaviour
{
    [Header("Button Aba Config")]
    public ButtonControl buttonControl;

    [Header("Botoes")]
    public Button BtnAbrirInfoEstab;
    public Button BtnAbrirHistoricoCompra;
    public Button BtnFecharHistoricoCompra;
    public Button BtnConvidarGrupo;
    public Button BtnConvitesEnviado;

    [Header("Paineis")]
    public List<GameObject> PnlAbasComanda;

    [Header("Paineis")]
    public GameObject PnlHistoricoComanda;
    public GameObject PnlComComanda;
    public GameObject PnlSemComanda;

    [Header("Itens Comanda")]
    public Transform ScvItensComanda;
    public ItemComandaObj ItensComandaRef;

    [Header("Grupo Comanda")]
    public Transform ScvGrupoComanda;
    public GrupoObj GrupoComandaRef;
    public Text LblValorAPagarGrupo;
    public GameObject ObjLiderancaGrupo;

    [Header("Convidar Grupo")]
    public ConvidarGrupo ConvidarGrupo;

    [Header("Totais")]
    public Text LblValorTotal;
    public Text LblValorPago;
    public Text LblValorRestante;

    [HideInInspector]
    public ComandaFirebase ComandaFirebase;
    [HideInInspector]
    public GenericFirebase<double> ComandaValorTotalFirebase;
    [HideInInspector]
    public string ComandaId;

    private bool clienteLogadoEhLider = false;
    private List<GrupoObj> lstGrupoComanda;
    private List<ItemComandaObj> lstProdutosComanda;
    private Comanda.Grupo ClienteComanda;
    private double valorTotalComanda;

    private void Awake()
    {
        lstGrupoComanda = new List<GrupoObj>();
        lstProdutosComanda = new List<ItemComandaObj>();

        //colocar real time para adicionar uma comanda...
        ComandaFirebase = new ComandaFirebase
        {
            AcaoGrupo = (grupo, tipoAcao) =>
            {
                if (tipoAcao == ComandaFirebase.TipoAcao.Adicionar)
                {
                    adicionarIntegranteAoGrupo(grupo);
                    return;
                }

                if (tipoAcao == ComandaFirebase.TipoAcao.Modificar)
                {
                    modificarIntegranteDoGrupo(grupo);
                    return;
                }

                if (tipoAcao == ComandaFirebase.TipoAcao.Remover)
                {
                    removerIntegranteDoGrupo(grupo);
                    return;
                }
            },
            AcaoProdutos = (produto, tipoAcao) =>
            {
                if (tipoAcao == ComandaFirebase.TipoAcao.Adicionar)
                {
                    adicionarProdutoAoGrupo(produto);
                    return;
                }

                if (tipoAcao == ComandaFirebase.TipoAcao.Modificar)
                {
                    modificarProdutoDoGrupo(produto);
                    return;
                }

                if (tipoAcao == ComandaFirebase.TipoAcao.Remover)
                {
                    removerProdutoDoGrupo(produto);
                    return;
                }
            }
        };

        configurarListener();
    }

    #region configurarListener
    private void configurarListener()
    {
        BtnAbrirHistoricoCompra.onClick.AddListener(() => btnAbrirHistoriaCompra());
        BtnFecharHistoricoCompra.onClick.AddListener(() => btnFecharHistoricoCompra());
        BtnAbrirInfoEstab.onClick.AddListener(() => btnAbrirInfoEstab());
        BtnConvidarGrupo.onClick.AddListener(() => btnConvidarGrupo());
        BtnConvitesEnviado.onClick.AddListener(() => btnConvitesEnviado());

        buttonControl.BtnAbas[0].onClick.AddListener(() => mudarAba(0, true));
        buttonControl.BtnAbas[1].onClick.AddListener(() => mudarAba(1, true));
    }
    #endregion

    #region btnAbrirInfoEstab
    private void btnAbrirInfoEstab()
    {
        obterEstabelecimento();
    }
    #endregion

    #region btnConvidarGrupo
    private void btnConvidarGrupo()
    {
        ConvidarGrupo.AbrirPnlConvidarGrupo();
    }
    #endregion

    #region  btnConvitesEnviado
    private void btnConvitesEnviado()
    {
        ConvidarGrupo.AbrirPnlConvitesEnviados();
    }
    #endregion

    #region mudarAba
    private void mudarAba(int numeroAba, bool tocarSom = false)
    {
        if (tocarSom)
            EasyAudioUtility.Instance.Play(EasyAudioUtility.Som.Click_OK);

        PnlAbasComanda.ForEach(x => x.SetActive(false));
        PnlAbasComanda[numeroAba].SetActive(true);
        buttonControl.TrocarAba(numeroAba);
    }
    #endregion

    #region obterEstabelecimento
    private void obterEstabelecimento()
    {
        Dictionary<string, object> form = new Dictionary<string, object>
        {
            { "estabelecimentoId", Cliente.ClienteLogado.configClienteAtual.estabelecimento }
        };

        StartCoroutine(EstabelecimentoAPI.ObterEstabelecimento(form,
        (response, error) =>
        {
            if (error != null)
            {
                Debug.Log(error);
                StartCoroutine(AlertaManager.Instance.ChamarAlertaMensagem(error, false));
                return;
            }

            Main.Instance.MenuEstabelecimento.PreencherInfoEstabelecimento(response);
        }));
    }
    #endregion

    #region IniciarWatchComanda
    public void IniciarWatchComanda(bool ehParaAdicionar)
    {
        PnlSemComanda.SetActive(!ehParaAdicionar);
        PnlComComanda.SetActive(ehParaAdicionar);

        if (ehParaAdicionar) this.ComandaId = Cliente.ClienteLogado.configClienteAtual.comanda;

        string comandaIdAtual = (ehParaAdicionar) ? Cliente.ClienteLogado.configClienteAtual.comanda : this.ComandaId;

        ComandaFirebase.Watch(comandaIdAtual, ehParaAdicionar);

        if (ComandaValorTotalFirebase == null)
        {
            ComandaValorTotalFirebase = new GenericFirebase<double>($"comandas/{comandaIdAtual}/valorTotal")
            {
                Callback = (valorTotal) =>
                {
                    valorTotalComanda = valorTotal;
                    StartCoroutine(calcularTotaisComanda());
                }
            };
        }

        ComandaValorTotalFirebase.Watch(ehParaAdicionar);

        if (!ehParaAdicionar)
        {
            this.ComandaId = string.Empty;
            ComandaValorTotalFirebase = null;
        }
    }
    #endregion

    #region adicionarIntegranteAoGrupo
    private void adicionarIntegranteAoGrupo(Comanda.Grupo grupo)
    {
        if (grupo != null)
        {
            if (grupo.cliente._id == Cliente.ClienteLogado._id)
            {
                ClienteComanda = grupo;
                preencherComandaCliente();
                return;
            }

            GrupoObj grupoComanda = Instantiate(GrupoComandaRef, ScvGrupoComanda);
            obterAvatarMembroGrupo(grupo, grupoComanda);
            lstGrupoComanda.Add(grupoComanda);
        }
    }
    #endregion

    #region modificarIntegranteDoGrupo
    private void modificarIntegranteDoGrupo(Comanda.Grupo grupo)
    {
        if (grupo.cliente._id == Cliente.ClienteLogado._id)
        {
            ClienteComanda = grupo;
            preencherComandaCliente();
            return;
        }

        GrupoObj grupoObj = lstGrupoComanda.Find(x => x.Integrante.cliente._id == grupo.cliente._id);

        if (grupoObj != null)
            obterAvatarMembroGrupo(grupo, grupoObj);
    }
    #endregion

    #region removerIntegranteDoGrupo
    private void removerIntegranteDoGrupo(Comanda.Grupo grupo)
    {
        if (grupo.cliente._id == Cliente.ClienteLogado._id)
        {
            return;
        }

        GrupoObj grupoObj = lstGrupoComanda.Find(x => x.Integrante.cliente._id == grupo.cliente._id);

        if (grupoObj != null)
        {
            Destroy(grupoObj.gameObject);
            lstGrupoComanda.Remove(grupoObj);

            //if (lstGrupoComanda.Count == 0)
            //    PnlShopVazio.SetActive(true);
        }

    }
    #endregion

    #region adicionarProdutoAoGrupo
    private void adicionarProdutoAoGrupo(Comanda.Produto produto)
    {
        if (produto != null)
        {
            ItemComandaObj itemComandaObj = Instantiate(ItensComandaRef, ScvItensComanda);

            Main.Instance.ObterIcones(produto.infoProduto.icon, FileManager.Directories.produto, (textura) =>
            {
                itemComandaObj.PreencherIcone(textura);
            });
            itemComandaObj.PreencherInfo(produto);
            lstProdutosComanda.Add(itemComandaObj);
        }
    }
    #endregion

    #region modificarProdutoDoGrupo
    private void modificarProdutoDoGrupo(Comanda.Produto produto)
    {
        ItemComandaObj itemComandaObj = lstProdutosComanda.Find(x => x.Produto.infoProduto._id == produto.infoProduto._id);

        if (itemComandaObj != null)
            itemComandaObj.PreencherInfo(produto);
    }
    #endregion

    #region removerProdutoDoGrupo
    private void removerProdutoDoGrupo(Comanda.Produto produto)
    {
        ItemComandaObj itemComandaObj = lstProdutosComanda.Find(x => x.Produto.infoProduto._id == produto.infoProduto._id);

        if (itemComandaObj != null)
        {
            Destroy(itemComandaObj.gameObject);
            lstProdutosComanda.Remove(itemComandaObj);
        }
    }
    #endregion

    #region preencherComanda
    private void preencherComandaCliente()
    {
        clienteLogadoEhLider = ClienteComanda.lider;
        ObjLiderancaGrupo.SetActive(ClienteComanda.lider);
        BtnConvidarGrupo.gameObject.SetActive(ClienteComanda.lider);
        BtnConvitesEnviado.gameObject.SetActive(ClienteComanda.lider);
        StartCoroutine(calcularTotaisComanda());
    }
    #endregion

    #region obterAvatarMembroGrupo
    private void obterAvatarMembroGrupo(Comanda.Grupo grupo, GrupoObj grupoComanda)
    {
        Dictionary<string, object> form = new Dictionary<string, object>
        {
            { "avatarId", grupo.cliente.avatar }
        };

        //é feito essa conferencia pra nao ficar batendo toda hora na rota do avatar
        if (grupoComanda.Integrante != null && grupo.avatarAlterado.Subtract(grupoComanda.Integrante.avatarAlterado).TotalSeconds <= 0)
        {
            grupoComanda.PreencherInfo(grupo, null, clienteLogadoEhLider, (clienteId, nomeCliente) => {
                ConvidarGrupo.AbrirPnlTransferirLideranca(clienteId, nomeCliente);
            });

            StartCoroutine(calcularTotaisComanda());

            return;
        }

        StartCoroutine(AvatarAPI.ObterAvatar(form,
        (response, error) =>
        {
            if (error != null)
            {
                Debug.Log("ObterAvatar: " + error);
                StartCoroutine(AlertaManager.Instance.ChamarAlertaMensagem(error, false));
                return;
            }

            grupoComanda.PreencherInfo(grupo, response, clienteLogadoEhLider, (clienteId, nomeCliente) => {
                ConvidarGrupo.AbrirPnlTransferirLideranca(clienteId, nomeCliente);
            });

            StartCoroutine(calcularTotaisComanda());
        }));
    }
    #endregion

    #region calcularTotaisComanda
    private IEnumerator calcularTotaisComanda()
    {
        double valorPago = 0;
        double valorRestante = 0;
        double valorAPagarGrupo = 0;
        double pessoasNaoPagaramNoGrupo = 0;

        if (lstGrupoComanda != null && lstGrupoComanda.Count > 0)
        {
            for (int i =0; i < lstGrupoComanda.Count; i++)
            {
                yield return new WaitUntil(() => lstGrupoComanda[i].Integrante != null);

                valorPago += lstGrupoComanda[i].Integrante.valorPago;
            }
        }

        if (ClienteComanda != null)
        {
            valorPago += ClienteComanda.valorPago;

            if (!ClienteComanda.jaPagou)
                pessoasNaoPagaramNoGrupo = 1;
        }

        valorRestante = valorTotalComanda - valorPago;

        LblValorTotal.text = valorTotalComanda.ToString("C2");
        LblValorPago.text = "- " + valorPago.ToString("C2");
        LblValorRestante.text = valorRestante.ToString("C2");

        //+1 porque conta com o usuario do app que nao esta na lista
        if (lstGrupoComanda != null && lstGrupoComanda.Count > 0)
        {
            pessoasNaoPagaramNoGrupo += lstGrupoComanda.FindAll(x => x.Integrante.jaPagou == false).Count();
        }

        valorAPagarGrupo = valorRestante / pessoasNaoPagaramNoGrupo;

        if (lstGrupoComanda != null)
            lstGrupoComanda.ForEach(x => x.preencherValorAPagar(valorAPagarGrupo));

        if (ClienteComanda != null)
            LblValorAPagarGrupo.text = valorAPagarGrupo.ToString("C2");

    }
    #endregion

    //------------------------------------

    #region btnAbrirHistoriaCompra
    private void btnAbrirHistoriaCompra()
    {
        //PnlPopUp.AbrirPopUp(PnlHistoricoComanda, () =>
        //{

        //});
    }
    #endregion

    #region btnFecharHistoricoCompra
    private void btnFecharHistoricoCompra()
    {
        PnlPopUp.FecharPopUpSemDesligarPopUP(PnlHistoricoComanda, () =>
        {

        });
    }
    #endregion

}
