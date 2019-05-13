using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using APIModel;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuUsuario : MonoBehaviour
{
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
    public Button BtnAbrirEdicaoAvatar;

    [Header("Toggle")]
    public Toggle BtnPerfilDados;
    public Toggle BtnInfoAvatar;

    [Header("PnlPrincipal")]
    public GameObject PnlPerfil;

    public Text TxtTitulo;

    [Header("Avatar")]
    public GameObject PnlAvatar;

    [Header("Perfil")]
    public GameObject PnlPerfilGeral;
    public GameObject PnlAvatarInfo;
    public Text LblPerfilApelido;
    public Text LblPerfilLevel;
    public Text LblPerfilExp;
    public Text LblPerfilPontos;
    public Text LblPerfilGold;

    [Header("Perfil Edicao")]
    public GameObject PnlAvatarEdicao;
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
    public Cliente.Avatar AvatarEditado;

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
        BtnFechar.onClick.AddListener(() => PnlPopUp.FecharPopUp(PnlPerfil, null));

        BtnVoltarInfo.onClick.AddListener(() => PnlPopUp.FecharPnl(PnlPerfilEdicao, () =>
        {
            PnlPerfilGeral.SetActive(true);
        }));

        BtnFecharHistorico.onClick.AddListener(() => PnlPopUp.FecharPnl(PnlHistoricoCompra, () =>
        {
            TxtTitulo.text = "Perfil";
            BtnFechar.gameObject.SetActive(true);
            PnlPerfilGeral.SetActive(true);
        }));

        BtnAbrirHistorico.onClick.AddListener(() => btnAbrirHistoricoCompra());

        BtnAbrirEdicaoAvatar.onClick.AddListener(() =>
        {
            EasyAudioUtility.Instance.Play(EasyAudioUtility.Som.Click_OK);

            AnimacoesTween.AnimarObjeto(BtnAbrirEdicaoAvatar.gameObject,
                AnimacoesTween.TiposAnimacoes.Button_Click, () =>
                {
                    SceneManager.LoadSceneAsync("EdicaoChar", LoadSceneMode.Additive);
                },
                AppManager.TEMPO_ANIMACAO_ABRIR_CLICK_BOTAO);
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

        PnlPopUp.AbrirPopUp(PnlPerfil, () =>
        {
            BtnAbrirMenu(true);
        });
    }
    #endregion

    #region btnAbrirInfoPerfil
    private void btnAbrirInfoPerfil()
    {
        EasyAudioUtility.Instance.Play(EasyAudioUtility.Som.Click_OK);

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
        WWWForm form = new WWWForm();
        form.AddField("_idCliente", Cliente.ClienteLogado._id);

        StartCoroutine(APIManager.Instance.Post(APIManager.URLs.ListarHistoricoCompra, form, (response) =>
        {
            APIManager.Retorno<List<HistoricoCompra>> retornoAPI =
                JsonConvert.DeserializeObject<APIManager.Retorno<List<HistoricoCompra>>>(response);

            if (retornoAPI.sucesso)
            {
                callback(retornoAPI.retorno);
            }
            else
            {
                EasyAudioUtility.Instance.Play(EasyAudioUtility.Som.Error);
                callback(new List<HistoricoCompra>());
                //StartCoroutine(comunicadorAPI.Alerta.ChamarAlerta(retornoAPI.msg, comunicadorAPI.PnlPrincipal));
            }

        },
        (error) =>
        {
            //TODO: Tratar Error
        }));
    }
    #endregion

    #region preencherPerfil
    private void preencherPerfil()
    {
        try
        {
            LblPerfilApelido.text = Cliente.ClienteLogado.apelido;
            LblPerfilExp.text = Cliente.ClienteLogado.avatar.exp + "/" + Cliente.ClienteLogado.avatar.expProximoLevel;
            LblPerfilLevel.text = Cliente.ClienteLogado.avatar.level.ToString();
            LblPerfilPontos.text = Cliente.ClienteLogado.pontos.ToString();
            LblPerfilGold.text = Cliente.ClienteLogado.RetornarGoldTotal().ToString();
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
        PnlAvatarEdicao.GetComponent<AvatarObj>().PreencherInfo(Cliente.ClienteLogado.sexo, Cliente.ClienteLogado.avatar);
        AvatarEditado = null;
        TxtEmailInfo.text = Cliente.ClienteLogado.email;
        TxtSenhaInfo.text = string.Empty;
        TxtSenhaInfoConfirmar.text = string.Empty;
        TxtNomeInfo.text = Cliente.ClienteLogado.nome;
        TxtCPFInfo.text = Cliente.ClienteLogado.cpf;
        TxtIdadeInfo.text = Cliente.ClienteLogado.dataNascimento.ToString("dd/MM/yyyy");
        TxtApelidoInfo.text = Cliente.ClienteLogado.apelido;
        TxtSexoInfo.text = Cliente.ClienteLogado.sexo;
    }
    #endregion

    #region MudarAba
    public void MudarAba(int numeroAba,bool tocarSom = false, GameObject objClicado = null)
    {
        if (tocarSom)
            EasyAudioUtility.Instance.Play(EasyAudioUtility.Som.Click_OK);

        AnimacoesTween.AnimarObjeto(
            objClicado, 
            AnimacoesTween.TiposAnimacoes.Button_Click, () =>
        {
            PnlAbasEdicao.ForEach(x => x.SetActive(false));
            PnlAbasEdicao[numeroAba].SetActive(true);
        },
        AppManager.TEMPO_ANIMACAO_ABRIR_CLICK_BOTAO);
    }
    #endregion




    //------------------------------------------------------------------------------------
    #region "TO DO - ARRUMAR"

    public void PreencherAvatares()
    {
        PnlAvatar.GetComponent<AvatarObj>().PreencherInfo(Cliente.ClienteLogado.sexo, Cliente.ClienteLogado.avatar);
        PnlAvatarInfo.GetComponent<AvatarObj>().PreencherInfo(Cliente.ClienteLogado.sexo, Cliente.ClienteLogado.avatar);
    }

    public void BtnSalvarEdicaoPerfil()
    {
        salvarPerfil();
    }

    public void AlterarAvatar(Cliente.Avatar avatar)
    {
        AvatarEditado = avatar;

        PnlAvatarEdicao.GetComponent<AvatarObj>().PreencherInfo(TxtSexoInfo.text, AvatarEditado);
    }

    private void salvarPerfil()
    {
        EasyAudioUtility.Instance.Play(EasyAudioUtility.Som.Click_OK);

        Cliente.ClienteLogado.dataNascimento = Convert.ToDateTime(Util.formatarDataParaAPI(TxtIdadeInfo.text));
        Cliente.ClienteLogado.nome = TxtNomeInfo.text;

        Cliente.ClienteLogado.avatar = AvatarEditado ?? Cliente.ClienteLogado.avatar;

        WWWForm form = new WWWForm();
        form.AddField("_id", Cliente.ClienteLogado._id);
        form.AddField("password", Cliente.ClienteLogado.password);
        form.AddField("dataNascimento", Util.formatarDataParaAPI(TxtIdadeInfo.text));
        form.AddField("nome", Cliente.ClienteLogado.nome);
        form.AddField("endereco", JsonConvert.SerializeObject(Cliente.ClienteLogado.endereco));
        form.AddField("avatar", JsonConvert.SerializeObject(Cliente.ClienteLogado.avatar));

        StartCoroutine(APIManager.Instance.Post(APIManager.URLs.ClienteAlterar, form,
        (response) =>
        {
            APIManager.Retorno<string> retornoAPI = new APIManager.Retorno<string>();
            retornoAPI = JsonConvert.DeserializeObject<APIManager.Retorno<string>>(response);

            if (retornoAPI.sucesso)
            {
                PreencherAvatares();
                //StartCoroutine(comunicadorAPI.Alerta.ChamarAlerta(retornoAPI.msg, comunicadorAPI.PnlPrincipal));
            }
            else
            {
                EasyAudioUtility.Instance.Play(EasyAudioUtility.Som.Error);
                //StartCoroutine(comunicadorAPI.Alerta.ChamarAlerta(retornoAPI.msg, comunicadorAPI.PnlPrincipal));
            }
        },
        (error) =>
        {
            //TODO: Tratar Error
        }));
    }


    #endregion

    #region "Amigos"

    public void BtnAbrirPnlAmigos()
    {

        EasyAudioUtility.Instance.Play(EasyAudioUtility.Som.Click_OK);
        AnimacoesTween.AnimarObjeto(EventSystem.current.currentSelectedGameObject, AnimacoesTween.TiposAnimacoes.SubMenu_Click, () =>
        {
            Main.Instance.PnlPopUp.SetActive(true);
            PnlAmigos.SetActive(true);
            AnimacoesTween.AnimarObjeto(PnlAmigos, AnimacoesTween.TiposAnimacoes.Scala, null, 0.5f, new Vector2(1, 1));
        },
        0.1f);
    }
    #endregion
}
