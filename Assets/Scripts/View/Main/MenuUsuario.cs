using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using APIModel;
using Network;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuUsuario : MonoBehaviour
{
    [Header("Canvas")]
    public Canvas CanvasPerfil;

    [Header("Botoes")]
    public Button BtnMenuPerfil;
    public Button BtnPerfil;
    public Button BtnInfoPerfil;
    public Button BtnAmigos;
    public Button BtnConquistas;
    public Button BtnLoja;
    public Button BtnFechar;
    public Button BtnAbrirHistorico;
    public Button BtnFecharHistorico;
    public Button BtnVoltarInfo;
    public Button BtnSalvarInfo;
    public Button BtnAbrirEdicaoAvatar;

    [Header("Toggle")]
    public Toggle BtnPerfilDados;
    public Toggle BtnInfoAvatar;

    [Header("PnlPrincipal")]
    public GameObject PnlPerfil;

    public Text TxtTitulo;

    [Header("Avatar")]
    public AvatarObj PnlAvatar;

    [Header("Perfil")]
    public GameObject PnlPerfilGeral;
    public AvatarObj PnlAvatarInfo;
    public Text LblPerfilApelido;
    public Text LblPerfilLevel;
    public Text LblPerfilExp;
    public Text LblPerfilPontos;
    public Text LblPerfilGold;
    public Text LblChaveAmigavel;

    [Header("Perfil Edicao")]
    public AvatarObj PnlAvatarEdicao;
    public GameObject PnlPerfilEdicao;
    public List<GameObject> PnlAbasEdicao;
    public InputField TxtEmailInfo;
    public InputField TxtSenhaInfo;
    public InputField TxtSenhaInfoConfirmar;
    public InputField TxtNomeInfo;
    public InputField TxtCPFInfo;
    public InputField TxtIdadeInfo;
    public InputField TxtApelidoInfo;
    public Text TxtSexoInfo;

    [Header("Amigos")]
    public GameObject PnlAmigos;

    [Header("Historico Compra")]
    public GameObject PnlHistoricoCompra;
    public Transform ScvHistoricoCompra;
    public HistoricoCompraObj HistoricoCompraRef;
    public GameObject TxtHistoricoVazio;

    [HideInInspector]
    public bool MenuAtivo = false;

    private List<GameObject> lstMenus;

    void Awake()
    {
        configurarListener();

        lstMenus = new List<GameObject>
        {
            BtnPerfil.gameObject,
            BtnAmigos.gameObject,
            BtnConquistas.gameObject,
            BtnLoja.gameObject
        };
    }

    #region configurarListener
    private void configurarListener()
    {
        BtnMenuPerfil.onClick.AddListener(() => BtnAbrirMenu());
        BtnPerfil.onClick.AddListener(() => btnAbrirPerfil());
        BtnInfoPerfil.onClick.AddListener(() => btnAbrirInfoPerfil());
        BtnFechar.onClick.AddListener(() => PnlPopUp.FecharPopUp(CanvasPerfil, PnlPerfil, null));
        BtnSalvarInfo.onClick.AddListener(() => salvarPerfil());

        BtnVoltarInfo.onClick.AddListener(() => PnlPopUp.FecharPnl(PnlPerfilEdicao, () =>
        {
            PnlPerfilGeral.SetActive(true);
        }));

        BtnFecharHistorico.onClick.AddListener(() => PnlPopUp.FecharPnl(PnlHistoricoCompra, () =>
        {
            ScvHistoricoCompra.GetComponentsInChildren<HistoricoCompraObj>().ToList().ForEach(x => Destroy(x.gameObject));
            TxtTitulo.text = "Perfil";
            BtnFechar.gameObject.SetActive(true);
            PnlPerfilGeral.SetActive(true);
        }));

        BtnAbrirHistorico.onClick.AddListener(() => btnAbrirHistoricoCompra());

        BtnAbrirEdicaoAvatar.onClick.AddListener(() =>
        {
            EasyAudioUtility.Instance.Play(EasyAudioUtility.Som.Click_OK);

            SceneManager.LoadSceneAsync("EdicaoChar", LoadSceneMode.Additive);
        });

        BtnPerfilDados.onValueChanged.AddListener((result) => MudarAba(0, true, BtnPerfilDados.gameObject));
        BtnInfoAvatar.onValueChanged.AddListener((result) => MudarAba(1, true, BtnInfoAvatar.gameObject));
    }
    #endregion

    #region BtnAbrirMenu
    public void BtnAbrirMenu(bool fecharAutomatico = false)
    {
        MenuAtivo = (fecharAutomatico) ? false : !MenuAtivo;

        Main.Instance.AbrirMenu("BtnPerfil", (fecharAutomatico) ? false : MenuAtivo, lstMenus, fecharAutomatico);
    }
    #endregion

    #region btnAbrirPerfil
    private void btnAbrirPerfil()
    {
        TxtTitulo.text = "Perfil";
        PnlPerfilEdicao.SetActive(false);
        PnlHistoricoCompra.SetActive(false);
        PnlPerfilGeral.SetActive(true);

        preencherPerfil();

        PnlPopUp.AbrirPopUpCanvas(CanvasPerfil, PnlPerfil, () =>
        {
            BtnAbrirMenu(true);
        });
    }
    #endregion

    #region btnAbrirInfoPerfil
    private void btnAbrirInfoPerfil()
    {
        PnlPopUp.AbrirPopUp(PnlPerfilEdicao, () =>
        {
            BtnPerfilDados.isOn = true;
            configurarEdicaoPerfil();
        },
        PnlPerfilGeral);
    }
    #endregion

    #region btnAbrirHistoricoCompra
    private void btnAbrirHistoricoCompra()
    {
        ScvHistoricoCompra.GetComponentsInChildren<HistoricoCompraObj>().ToList().ForEach(x => Destroy(x.gameObject));

        buscarHistorico((lstHistoricoCompra) =>
        {
            TxtHistoricoVazio.SetActive(false);

            PnlPopUp.AbrirPopUp(PnlHistoricoCompra,
                () =>
                {
                    TxtTitulo.text = "Histórico de Compras";

                    if (lstHistoricoCompra.Count == 0)
                    {
                        TxtHistoricoVazio.SetActive(true);
                        return;
                    }

                    foreach (HistoricoCompra historico in lstHistoricoCompra)
                    {
                        HistoricoCompraObj objEstab = Instantiate(HistoricoCompraRef, ScvHistoricoCompra.transform);
                        objEstab.PreencherInfo(historico);
                    }
                },
                PnlPerfilGeral);

        });
    }
    #endregion

    #region buscarHistorico
    private void buscarHistorico(Action<List<HistoricoCompra>> callback)
    {
        StartCoroutine(ClienteAPI.ListarHistoricoCompra(
        (response, error) =>
        {
            if (error != null)
            {
                Debug.Log(error);
                StartCoroutine(AppManager.Instance.DesativarLoader());

                StartCoroutine(AlertaManager.Instance.ChamarAlertaMensagem(error, false));
                return;
            }

            callback(response);
        }));
    }
    #endregion

    #region preencherPerfil
    private void preencherPerfil()
    {
        try
        {
            PnlAvatarInfo.PreencherInfo(Cliente.ClienteLogado.sexo, Cliente.ClienteLogado.avatar);

            LblPerfilApelido.text = Cliente.ClienteLogado.apelido;
            LblPerfilExp.text = Cliente.ClienteLogado.avatar.exp + "/" + Cliente.ClienteLogado.avatar.expProximoLevel;
            LblPerfilLevel.text = Cliente.ClienteLogado.avatar.level.ToString();
            LblPerfilPontos.text = Cliente.ClienteLogado.pontos.ToString();
            LblPerfilGold.text = Cliente.ClienteLogado.RetornarGoldTotal().ToString();
            LblChaveAmigavel.text = Cliente.ClienteLogado.chaveAmigavel;
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }
    #endregion

    #region configurarEdicaoPerfil
    private void configurarEdicaoPerfil()
    {
        PnlAvatarEdicao.PreencherInfo(Cliente.ClienteLogado.sexo, Cliente.ClienteLogado.avatar);
        TxtEmailInfo.text = Cliente.ClienteLogado.email;
        TxtSenhaInfo.text = string.Empty;
        TxtSenhaInfoConfirmar.text = string.Empty;
        TxtNomeInfo.text = Cliente.ClienteLogado.nome;
        TxtCPFInfo.text = Cliente.ClienteLogado.cpf;
        TxtIdadeInfo.text = (Cliente.ClienteLogado.dataNascimento != null && 
                            Cliente.ClienteLogado.dataNascimento != DateTime.MinValue) ? 
                            Convert.ToDateTime(Cliente.ClienteLogado.dataNascimento).ToString("dd/MM/yyyy") : "";
        TxtApelidoInfo.text = Cliente.ClienteLogado.apelido;
        TxtSexoInfo.text = Cliente.ClienteLogado.sexo;
    }
    #endregion

    #region MudarAba
    public void MudarAba(int numeroAba, bool tocarSom = false, GameObject objClicado = null)
    {
        if (EventSystem.current.currentSelectedGameObject == objClicado)
        {
            if (tocarSom)
                EasyAudioUtility.Instance.Play(EasyAudioUtility.Som.Click_OK);

            BtnSalvarInfo.gameObject.SetActive((numeroAba == 0) ? true : false);
            PnlAbasEdicao.ForEach(x => x.SetActive(false));
            PnlAbasEdicao[numeroAba].SetActive(true);
        }
        else
        {
            PnlAbasEdicao.ForEach(x => x.SetActive(false));
            PnlAbasEdicao[0].SetActive(true);
        }
    }
    #endregion

    #region PreencherAvatares
    public void PreencherAvatares()
    {
        PnlAvatar.PreencherInfo(Cliente.ClienteLogado.sexo, Cliente.ClienteLogado.avatar);
        PnlAvatarInfo.PreencherInfo(Cliente.ClienteLogado.sexo, Cliente.ClienteLogado.avatar);
        PnlAvatarEdicao.PreencherInfo(Cliente.ClienteLogado.sexo, Cliente.ClienteLogado.avatar);
    }
    #endregion



    //------------------------------------------------------------------------------------
    #region "TO DO - ARRUMAR"

    private void salvarPerfil()
    {
        EasyAudioUtility.Instance.Play(EasyAudioUtility.Som.Click_OK);

        Dictionary<string, string> data = new Dictionary<string, string>
        {
            { "nome", TxtNomeInfo.text },
            { "cpf", TxtCPFInfo.text },
            { "dataNascimento", Util.formatarDataParaAPI(TxtIdadeInfo.text) }
        };

        StartCoroutine(ClienteAPI.ClienteAlterar(data,
        (response, error) =>
        {
            if (error != null)
            {
                Debug.Log(error);
                StartCoroutine(AlertaManager.Instance.ChamarAlertaMensagem(error, false));

                return;
            }

            AlertaManager.Instance.IniciarAlerta(response);

            Cliente.ClienteLogado.cpf = TxtCPFInfo.text;
            Cliente.ClienteLogado.dataNascimento = Convert.ToDateTime(Util.formatarDataParaAPI(TxtIdadeInfo.text));
            Cliente.ClienteLogado.nome = TxtNomeInfo.text;

        }));
    }


    #endregion

    #region "Amigos"

    public void BtnAbrirPnlAmigos()
    {

        EasyAudioUtility.Instance.Play(EasyAudioUtility.Som.Click_OK);

        Main.Instance.PnlPopUp.SetActive(true);
        PnlAmigos.SetActive(true);
        AnimacoesTween.AnimarObjeto(PnlAmigos, AnimacoesTween.TiposAnimacoes.Scala, null, 0.5f, new Vector2(1, 1));
    }
    #endregion
}
