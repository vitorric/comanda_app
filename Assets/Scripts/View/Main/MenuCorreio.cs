using APIModel;
using FirebaseModel;
using Network;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuCorreio : MonoBehaviour
{
    public Button BtnFecharPnlMensagem;

    public GameObject AlertaNotificacao;
    public Text TxtQuantNotificacao;
    public Transform ScvMensagens;
    public CorreioObj CorreioObjRef;
    public GameObject PnlMensagemVazia;

    [Header("Painel Mensagem")]
    public Canvas CanvasMensagem;
    public GameObject PnlMensagem;
    public Text TxtTitulo;
    public Text TxtMensagem;
    public Text TxtExecutouAcao;

    [Header("Acao Convite Grupo Comanda")]
    public GameObject PnlDecisaoConviteGrupo;
    public Button BtnAceitarConvite;
    public Button BtnRecusarConvite;

    private List<CorreioObj> lstCorreioObj;
    private CorreioFirebase correioFirebase;
    private Correio.Mensagem mensagem;

    private void Awake()
    {
        lstCorreioObj = new List<CorreioObj>();

        configurarListener();

        correioFirebase = new CorreioFirebase
        {
            AcaoCorreio = (mensagem, tipoAcao) =>
            {
                if (tipoAcao == CorreioFirebase.TipoAcao.Adicionar)
                {
                    adicionarMensagemNoCorreio(mensagem);
                    return;
                }

                if (tipoAcao == CorreioFirebase.TipoAcao.Modificar)
                {
                    modificarMensagemDoCorreio(mensagem);
                    return;
                }

                if (tipoAcao == CorreioFirebase.TipoAcao.Remover)
                {
                    removerMensagemDoCorreio(mensagem);
                    return;
                }
            }

        };
        correioFirebase.Watch(Cliente.ClienteLogado._id, true);
    }

    #region configurarListener
    private void configurarListener()
    {
        BtnFecharPnlMensagem.onClick.AddListener(() => fecharPnlMensagem());
        BtnAceitarConvite.onClick.AddListener(() => aceitarRecusarConviteGrupo(true));
        BtnRecusarConvite.onClick.AddListener(() => aceitarRecusarConviteGrupo(false));
    }
    #endregion

    #region PararWatch
    public void PararWatch()
    {
        correioFirebase.Watch(Cliente.ClienteLogado._id, false);
    }
    #endregion

    #region adicionarMensagemNoCorreio
    private void adicionarMensagemNoCorreio(Correio.Mensagem mensagem)
    {
        if (mensagem != null)
        {
            CorreioObj correioObj = Instantiate(CorreioObjRef, ScvMensagens);
            correioObj.PreencherInfo(mensagem);
            lstCorreioObj.Add(correioObj);

            PnlMensagemVazia.SetActive(false);

            verificarNotificacoes();
            ordernarMensagens();
        }
    }
    #endregion

    #region modificarMensagemDoCorreio
    private void modificarMensagemDoCorreio(Correio.Mensagem mensagem)
    {
        CorreioObj correioObj = lstCorreioObj.Find(x => x.mensagem._id == mensagem._id);

        if (correioObj != null)
            correioObj.PreencherInfo(mensagem);

        verificarNotificacoes();
    }
    #endregion

    #region removerMensagemDoCorreio
    private void removerMensagemDoCorreio(Correio.Mensagem mensagem)
    {
        CorreioObj correioObj = lstCorreioObj.Find(x => x.mensagem._id == mensagem._id);

        if (correioObj != null)
        {
            Destroy(correioObj.gameObject);
            lstCorreioObj.Remove(correioObj);

            if (lstCorreioObj.Count == 0)
                PnlMensagemVazia.SetActive(true);
        }

        verificarNotificacoes();
    }
    #endregion

    #region verificarNotificacoes
    private void verificarNotificacoes()
    {
        if (lstCorreioObj.FindAll(x => !x.mensagem.lida).Count > 0)
        {
            AlertaNotificacao.SetActive(true);
            TxtQuantNotificacao.text = lstCorreioObj.FindAll(x => !x.mensagem.lida).Count.ToString();
            return;
        }

        AlertaNotificacao.SetActive(false);
    }
    #endregion

    #region abrirPnlMensagem
    public void AbrirPnlMensagem(Correio.Mensagem mensagem)
    {
        PnlDecisaoConviteGrupo.SetActive(false);
        this.mensagem = mensagem;
        TxtMensagem.text = mensagem.mensagemGrande;
        TxtTitulo.text = mensagem.titulo;
        configurarMensagemAcao();
        marcarMensagemComoLida();

        PnlPopUp.AbrirPopUpCanvas(CanvasMensagem, PnlMensagem, null);
    }
    #endregion

    #region fecharPnlMensagem
    private void fecharPnlMensagem()
    {
        PnlPopUp.FecharPnlCanvas(CanvasMensagem, PnlMensagem, null);
    }
    #endregion

    #region configurarMensagemAcao
    private void configurarMensagemAcao()
    {
        TxtExecutouAcao.gameObject.SetActive(false);

        if (mensagem.acao != null)
        {
            if (mensagem.acao.executouAcao)
            {
                TxtExecutouAcao.text = "Resposta já enviada!";
                TxtExecutouAcao.gameObject.SetActive(true);
                return;
            }
            if (mensagem.acao.tipo == Correio.TiposAcao.ConviteGrupo.ToString())
            {
                PnlDecisaoConviteGrupo.SetActive(true);
            }
        }
    }
    #endregion

    #region marcarMensagemComoLida
    private void marcarMensagemComoLida()
    {
        Dictionary<string, object> form = new Dictionary<string, object>()
        {
            { "mensagemId", mensagem._id }
        };

        if (!mensagem.lida)
        {
            StartCoroutine(CorreioAPI.MarcarMensagemComoLida(form,
            (response, error) =>
            {

                if (error != null)
                {
                    Debug.Log(error);
                    AlertaManager.Instance.ChamarAlertaMensagem(error, false);
                    return;
                }

            }));
        }
    }
    #endregion

    #region aceitarRecusarConviteGrupo
    private void aceitarRecusarConviteGrupo(bool aceitar)
    {
        Dictionary<string, string> coordenadas = new Dictionary<string, string>();

        StartCoroutine(GPSManager.Instance.IniciarGPS((lat, longi, sucesso) =>
        {
            if (sucesso)
            {
                coordenadas.Add("lat", lat.ToString());
                coordenadas.Add("long", longi.ToString());

                Dictionary<string, object> form = new Dictionary<string, object>()
                {
                    {"comanda", mensagem.acao.comanda },
                    {"aceitou", aceitar },
                    { "coordenadas", coordenadas }
                };

                StartCoroutine(ComandaAPI.RespostaConviteGrupo(form,
                    (response, error) =>
                    {
                        if (error != null)
                        {
                            Debug.Log(error);
                            AlertaManager.Instance.ChamarAlertaMensagem(error, false);
                            return;
                        }

                        fecharPnlMensagem();
                    }));

                return;
            }

            AlertaManager.Instance.ChamarAlertaMensagem(AlertaManager.MsgAlerta.GPSError, false);
        }));
    }
    #endregion

    #region ordenarMensagens
    private void ordernarMensagens()
    {
        for (int i = 0; i < lstCorreioObj.Count; i++)
        {
            if (lstCorreioObj[i].mensagem.lida)
            {
                lstCorreioObj[i].gameObject.transform.SetAsLastSibling();
            }
        }
    }
    #endregion
}

