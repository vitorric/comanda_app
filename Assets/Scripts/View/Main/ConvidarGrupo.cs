using APIModel;
using Network;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConvidarGrupo : MonoBehaviour
{
    [Header("Botoes")]
    public Button BtnCancelar;
    public Button BtnConvidarGrupo;
    public Button BtnFecharConvitesEnviados;
    public Button BtnTransferirLideranca;
    public Button BtnFecharTransferirLideranca;

    [Header("Outros Objetos")]
    public Canvas CanvasConvidar;
    public GameObject PnlMainConvitesGrupo;
    public GameObject PnlConvidarGrupo;
    public Text TxtTitulo;

    [Header("Enviar Convite Grupo")]
    public GameObject PnlConvidar;
    public TMP_InputField TxtChaveAmigavel;

    [Header("Convites Enviado")]
    public GameObject PnlConvitesEnviados;
    public Transform ScvConvitesEnviados;
    public MembroConvidadoObj MembroConvidadoRef;

    [Header("Transferir Lideranca Grupo")]
    public Text LblTransferirLideranca;
    public GameObject PnlTransferirLideranca;

    private string clienteIdTransferirLider;

    private void Awake()
    {
        configurarListener();
    }


    #region configurarListener
    private void configurarListener()
    {
        BtnCancelar.onClick.AddListener(() => PnlPopUp.FecharPnlCanvas(CanvasConvidar, PnlConvidarGrupo, () =>
        {
            PnlMainConvitesGrupo.SetActive(false);
            PnlConvidar.SetActive(false);
            TxtChaveAmigavel.text = string.Empty;
        }));

        BtnFecharConvitesEnviados.onClick.AddListener(() => PnlPopUp.FecharPnlCanvas(CanvasConvidar, PnlConvidarGrupo, () =>
        {
            PnlMainConvitesGrupo.SetActive(false);
            PnlConvitesEnviados.SetActive(false);
            ScvConvitesEnviados.GetComponentsInChildren<MembroConvidadoObj>().ToList().ForEach(x => Destroy(x.gameObject));
        }));

        BtnFecharTransferirLideranca.onClick.AddListener(() => fecharPnlTransferirLideranca());

        BtnConvidarGrupo.onClick.AddListener(() => btnConvidarGrupo());

        BtnTransferirLideranca.onClick.AddListener(() => btnTransferirLideranca());
    }
    #endregion

    #region AbrirPnlTransferirLideranca
    public void AbrirPnlTransferirLideranca(string clienteId, string nomeCliente)
    {
        PnlPopUp.AbrirPopUpCanvas(CanvasConvidar, PnlConvidarGrupo, () =>
        {
            PnlTransferirLideranca.SetActive(true);
            LblTransferirLideranca.text = LblTransferirLideranca.text.Replace("@@NOMECLIENTE", nomeCliente);
            clienteIdTransferirLider = clienteId;
        });
    }
    #endregion

    #region AbrirPnlConvidarGrupo
    public void AbrirPnlConvidarGrupo()
    {
        PnlMainConvitesGrupo.SetActive(true);

        PnlPopUp.AbrirPopUpCanvas(CanvasConvidar, PnlConvidarGrupo, () =>
        {
            TxtTitulo.text = "Enviar Convite";
            PnlConvidar.SetActive(true);
        });
    }
    #endregion

    #region AbrirPnlConvitesEnviados
    public void AbrirPnlConvitesEnviados()
    {
        PnlMainConvitesGrupo.SetActive(true);

        PnlPopUp.AbrirPopUpCanvas(CanvasConvidar, PnlConvidarGrupo, () =>
        {
            TxtTitulo.text = "Convites Enviado";
            PnlConvitesEnviados.SetActive(true);

            Dictionary<string, object> form = new Dictionary<string, object>
            {
                { "comandaId", Cliente.ClienteLogado.configClienteAtual.comanda }
            };

            StartCoroutine(ComandaAPI.ConvitesEnviadoGrupo(form,
            (response, error) =>
            {

                if (error != null)
                {
                    Debug.Log("ConvitesEnviadoGrupo: " + error);
                    AlertaManager.Instance.ChamarAlertaMensagem(error, false);
                    return;
                }

                for (int i = 0; i < response.Count; i++)
                {
                    Instantiate(MembroConvidadoRef, ScvConvitesEnviados).PreencherInfo(response[i]);
                }
            }));

        });
    }
    #endregion

    #region fecharPnlTransferirLideranca
    private void fecharPnlTransferirLideranca()
    {
        PnlPopUp.FecharPnlCanvas(CanvasConvidar, PnlConvidarGrupo, () =>
        {
            PnlTransferirLideranca.SetActive(false);
        });
    }
    #endregion

    #region btnConvidarGrupo
    private void btnConvidarGrupo()
    {
        Dictionary<string, object> form = new Dictionary<string, object>
        {
            { "chaveAmigavel", TxtChaveAmigavel.text.ToUpper() }
        };

        StartCoroutine(ComandaAPI.ConvidarMembroGrupo(form,
        (response, error) =>
        {

            if (error != null)
            {
                Debug.Log("ConvidarMembroGrupo: " + error);
                AlertaManager.Instance.ChamarAlertaMensagem(error, false);
                return;
            }

            TxtChaveAmigavel.text = string.Empty;
            AlertaManager.Instance.ChamarAlertaMensagem(response, true);
        }));
    }
    #endregion

    #region btnTransferirLideranca
    private void btnTransferirLideranca()
    {
        Dictionary<string, object> form = new Dictionary<string, object>
        {
            { "comandaId",  Cliente.ClienteLogado.configClienteAtual.comanda },
            { "clienteNovoLiderId", clienteIdTransferirLider }
        };

        StartCoroutine(ComandaAPI.TransferirLiderancaGrupo(form,
        (response, error) =>
        {

            if (error != null)
            {
                Debug.Log("TransferirLiderancaGrupo: " + error);
                AlertaManager.Instance.ChamarAlertaMensagem(error, false);
                return;
            }

            fecharPnlTransferirLideranca();
            AlertaManager.Instance.IniciarAlerta(true);
        }));
    }
    #endregion
}
