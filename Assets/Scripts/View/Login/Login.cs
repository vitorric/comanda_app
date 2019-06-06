using System.Collections;
using System.Collections.Generic;
using APIModel;
using FirebaseModel;
using Network;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Login : MonoBehaviour
{
    [Header("Button")]
    public Button BtnEntrar;
    public Button BtnCadastrar;
    public Button BtnRecupSenha;
    public Button BtnRecupSenhaVoltar;
    public Button BtnRecupSenhaEnviar;
    public Button BtnEtapa1Voltar;
    public Button BtnEtapa1Continuar;
    public Button BtnEtapa2Voltar;
    public Button BtnEtapa2Confirmar;
    public Button BtnEdicaoAvatar;

    [Header("Toggle")]
    public Toggle BtnSexoMasc;
    public Toggle BtnSexoFem;

    [Header("Tela de Login")]
    public GameObject PnlLogin;
    public InputField TxtEmail;
    public InputField TxtSenha;

    [Header("Recuperar Senha")]
    public GameObject PnlRecuperarSenha;
    public InputField TxtEmailRecSenha;


    [Header("Cadastrar")]
    public List<GameObject> PnlCadastrarEtapas;

    [Header("Cadastro Etapa - 1")]
    public InputField TxtEmailCadastro;
    public InputField TxtNomeCadastro;
    public InputField TxtApelidoCadastro;
    public InputField TxtSenhaCadastro;
    public InputField TxtSenhaCadastroConfirm;

    [Header("Cadastro Etapa - 2")]
    public AvatarObj PnlAvatar;

    #region Awake
    void Awake()
    {
        if (Application.isEditor)
        {
            TxtEmail.text = "email1@email.com";
            TxtSenha.text = "1234";
        }

        configurarListener();

    }
    #endregion

    #region configurarListener
    private void configurarListener()
    {
        BtnEntrar.onClick.AddListener(() => BtnLogar());
        BtnCadastrar.onClick.AddListener(() => BtnCadastrarAvantarEtapa(0, BtnCadastrar.gameObject));
        BtnRecupSenha.onClick.AddListener(() => BtnAbrirPnlRecuperarSenha());
        BtnRecupSenhaVoltar.onClick.AddListener(() => BtnRecuperarSenhaVoltarLogin());
        BtnRecupSenhaEnviar.onClick.AddListener(() => BtnRecuperarSenha());
        BtnEtapa1Voltar.onClick.AddListener(() => BtnCadastrarVoltar(0, BtnEtapa1Voltar.gameObject));
        BtnEtapa1Continuar.onClick.AddListener(() => BtnCadastrarAvantarEtapa(1, BtnEtapa1Continuar.gameObject));
        BtnEtapa2Voltar.onClick.AddListener(() => BtnCadastrarVoltar(1, BtnEtapa2Voltar.gameObject));
        BtnEtapa2Confirmar.onClick.AddListener(() => BtnConfirmarCadastro());
        BtnEdicaoAvatar.onClick.AddListener(() => BtnAbrirEdicaoAvatar());

        BtnSexoMasc.onValueChanged.AddListener((result) => ChkChanged(BtnSexoMasc.gameObject, true));
        BtnSexoFem.onValueChanged.AddListener((result) => ChkChanged(BtnSexoFem.gameObject, true));
    }
    #endregion

    #region Botoes

    #region BtnLogar
    public void BtnLogar()
    {       
        EasyAudioUtility.Instance.Play(EasyAudioUtility.Som.Click_OK);
        logar();
        //AnimacoesTween.AnimarObjeto(BtnEntrar.gameObject, AnimacoesTween.TiposAnimacoes.Button_Click, () => logar(), AppManager.TEMPO_ANIMACAO_ABRIR_CLICK_BOTAO);
    }
    #endregion

    #region BtnAbrirPnlRecuperarSenha
    public void BtnAbrirPnlRecuperarSenha()
    {
        EasyAudioUtility.Instance.Play(EasyAudioUtility.Som.Click_OK);

        PnlRecuperarSenha.SetActive(true);
        PnlLogin.SetActive(false);
        TxtEmailRecSenha.text = string.Empty;
    }
    #endregion

    #region BtnRecuperarSenhaVoltarLogin
    public void BtnRecuperarSenhaVoltarLogin()
    {
        EasyAudioUtility.Instance.Play(EasyAudioUtility.Som.Click_Cancel);

        fecharPnlRecuperarSenha();
    }
    #endregion

    #region BtnRecuperarSenha
    public void BtnRecuperarSenha()
    {
        EasyAudioUtility.Instance.Play(EasyAudioUtility.Som.Click_OK);
        recuperarSenha();
    }
    #endregion

    #region BtnCadastrarAvantarEtapa
    public void BtnCadastrarAvantarEtapa(int etapa, GameObject objClicado)
    {
        EasyAudioUtility.Instance.Play(EasyAudioUtility.Som.Click_OK);
        avancarEtapa(etapa);
    }
    #endregion

    #region BtnCadastrarVoltar
    public void BtnCadastrarVoltar(int etapa, GameObject objClicado)
    {
        EasyAudioUtility.Instance.Play(EasyAudioUtility.Som.Click_Cancel);
        voltarEtapa(etapa);
    }
    #endregion

    #region BtnConfirmarCadastro
    public void BtnConfirmarCadastro()
    {
        EasyAudioUtility.Instance.Play(EasyAudioUtility.Som.Click_OK);
        StartCoroutine(cadastrar());
    }
    #endregion

    #region BtnAbrirEdicaoAvatar
    public void BtnAbrirEdicaoAvatar()
    {
        EasyAudioUtility.Instance.Play(EasyAudioUtility.Som.Click_OK);

        if (SceneManager.GetSceneByName("EdicaoChar").isLoaded)
        {
            return;
        }

        SceneManager.LoadSceneAsync("EdicaoChar", LoadSceneMode.Additive);
    }
    #endregion

    #endregion

    #region Checkbox

    #region ChkChanged
    public void ChkChanged(GameObject objClicado, bool tocarSom = false)
    {
        if (UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject == objClicado)
        {
            if (tocarSom)
                EasyAudioUtility.Instance.Play(EasyAudioUtility.Som.Click_OK);

            PnlAvatar.PreencherInfo((BtnSexoMasc.isOn) ? "Masculino" : "Feminino", PnlAvatar.Avatar);
            //avatar = PnlAvatar.Avatar;
        }
    }
    #endregion

    #endregion

    #region Metodos privados

    #region logar
    private void logar()
    {
        if (TxtEmail.text == string.Empty || TxtSenha.text == string.Empty)
        {
            StartCoroutine(AlertaManager.Instance.ChamarAlertaMensagem(AlertaManager.MsgAlerta.PreenchaOsCampos, false));
            return;
        }

        Dictionary<string, string> form = new Dictionary<string, string>
        {
            { "email", TxtEmail.text },
            { "password", Util.GerarHashMd5(TxtSenha.text) }
        };

        AppManager.Instance.AtivarLoader();

        StartCoroutine(ClienteAPI.ClienteLogin(form,
        (response, error) =>
        {
            if (error != null)
            {
                Debug.Log(error);
                AppManager.Instance.DesativarLoaderAsync();
                StartCoroutine(AlertaManager.Instance.ChamarAlertaMensagem(error, false));
                return;
            }

            Cliente.GravarSession(response.token, response._id,
                JsonConvert.SerializeObject(new Cliente.Credenciais
                {
                    email = TxtEmail.text,
                    password = Util.GerarHashMd5(TxtSenha.text)
                }));

            buscarClienteNoFireBase();
        }));

    }
    #endregion

    #region recuperarSenha
    private void recuperarSenha()
    {

        if (TxtEmailRecSenha.text == string.Empty)
        {
            StartCoroutine(AlertaManager.Instance.ChamarAlertaMensagem(AlertaManager.MsgAlerta.PreenchaOsCampos, false));
            return;
        }

        Dictionary<string, string> form = new Dictionary<string, string>
        {
            { "email", TxtEmailRecSenha.text }
        };

        StartCoroutine(ClienteAPI.ClienteRecuperarSenha(form,
        (response, error) =>
        {
            if (error != null)
            {

                Debug.Log(error);
                AppManager.Instance.DesativarLoaderAsync();

                StartCoroutine(AlertaManager.Instance.ChamarAlertaMensagem(error, false));
                return;
            }

            StartCoroutine(AlertaManager.Instance.ChamarAlertaMensagem(response, true));

            fecharPnlRecuperarSenha();

        }));
    }
    #endregion

    #region fecharPnlRecuperarSenha
    private void fecharPnlRecuperarSenha()
    {
        PnlRecuperarSenha.SetActive(false);
        PnlLogin.SetActive(true);
        TxtEmailRecSenha.text = string.Empty;
    }
    #endregion

    #region avancarEtapa
    private void avancarEtapa(int etapa)
    {
        if (etapa == 1)
        {
            if (TxtEmailCadastro.text == string.Empty ||
                        TxtSenhaCadastro.text == string.Empty ||
                        TxtSenhaCadastroConfirm.text == string.Empty ||
                        TxtApelidoCadastro.text == string.Empty ||
                        TxtNomeCadastro.text == string.Empty)
            {
                StartCoroutine(AlertaManager.Instance.ChamarAlertaMensagem(AlertaManager.MsgAlerta.PreenchaOsCampos, false));
                return;
            }

            if (ValidarSenhas() == false)
            {
                StartCoroutine(AlertaManager.Instance.ChamarAlertaMensagem(AlertaManager.MsgAlerta.SenhasNaoConferem, false));
                return;
            }
        }

        if (etapa == 0)
        {
            PnlLogin.SetActive(false);
            LimparFormCadastro();
        }
        else
        {
            PnlCadastrarEtapas[etapa - 1].SetActive(false);
        }

        PnlCadastrarEtapas[etapa].SetActive(true);
    }
    #endregion

    #region voltarEtapa
    private void voltarEtapa(int etapa)
    {

        if (etapa == 0)
            PnlLogin.SetActive(true);
        else
            PnlCadastrarEtapas[etapa - 1].SetActive(true);

        PnlCadastrarEtapas[etapa].SetActive(false);

    }
    #endregion

    #region cadastrar
    private IEnumerator cadastrar()
    {
        string senha = Util.GerarHashMd5(TxtSenhaCadastro.text);
        Dictionary<string, string> data = new Dictionary<string, string>
        {
            { "email", TxtEmailCadastro.text },
            { "password", senha },
            { "apelido", TxtApelidoCadastro.text },
            { "nome", TxtNomeCadastro.text },
            { "sexo", (BtnSexoMasc.isOn) ? "Masculino" : "Feminino" },
            { "avatar", JsonConvert.SerializeObject(prepararAvatarCadastro()) }
        };

        AppManager.Instance.AtivarLoader();

        yield return StartCoroutine(ClienteAPI.ClienteCadastrar(data,
        (response, error) =>
        {
            if (error != null)
            {
                Debug.Log(error);
                AppManager.Instance.DesativarLoaderAsync();

                StartCoroutine(AlertaManager.Instance.ChamarAlertaMensagem(error, false));
                return;
            }

            Cliente.GravarSession(response.token, response._id,
                JsonConvert.SerializeObject(new Cliente.Credenciais
                {
                    email = TxtEmailCadastro.text,
                    password = senha
                }));

            buscarClienteNoFireBase();
            return;
        }));
    }
    #endregion

    #region buscarNoFireBase
    private async void buscarClienteNoFireBase()
    {
        ClienteFirebase cliente = new ClienteFirebase();
        Cliente.ClienteLogado = await cliente.ObterUsuario(Cliente.Obter());
        Debug.Log(JsonConvert.SerializeObject(Cliente.ClienteLogado));

        AppManager.Instance.DesativarLoaderAsync();

        SceneManager.LoadSceneAsync("Main");
    }

    #region 
    private Dictionary<string, object> prepararAvatarCadastro()
    {
        Dictionary<string, object> avatarDic = new Dictionary<string, object>
        {
            { "corpo", PnlAvatar.Avatar.corpo },
            { "cabeca", PnlAvatar.Avatar.cabeca },
            { "nariz", PnlAvatar.Avatar.nariz },
            { "olhos", PnlAvatar.Avatar.olhos },
            { "boca", PnlAvatar.Avatar.boca },
            { "roupa", PnlAvatar.Avatar.roupa },
            { "cabeloTraseiro", PnlAvatar.Avatar.cabeloTraseiro },
            { "cabeloFrontal", PnlAvatar.Avatar.cabeloFrontal },
            { "barba", PnlAvatar.Avatar.barba },
            { "sombrancelhas", PnlAvatar.Avatar.sombrancelhas },
            { "orelha", PnlAvatar.Avatar.orelha },
            { "corPele", PnlAvatar.Avatar.corPele },
            { "corCabelo", PnlAvatar.Avatar.corCabelo },
            { "corBarba", PnlAvatar.Avatar.corBarba }
        };

        return avatarDic;
    }
    #endregion

    #endregion

    #endregion

    #region Metodos Publicos

    #region LimparFormCadastro
    public void LimparFormCadastro()
    {
        TxtEmailCadastro.text = string.Empty;
        TxtSenhaCadastro.text = string.Empty;
        TxtSenhaCadastroConfirm.text = string.Empty;
        TxtApelidoCadastro.text = string.Empty;
        TxtNomeCadastro.text = string.Empty;
        BtnSexoMasc.isOn = true;
        BtnSexoFem.isOn = false;

        PnlAvatar.PreencherInfo("Masculino", null);
    }
    #endregion

    #region ValidarSenhas
    public bool ValidarSenhas()
    {
        if (TxtSenhaCadastro.text == TxtSenhaCadastroConfirm.text)
            return true;

        return false;
    }
    #endregion

    #region AlterarAvatar
    public void AlterarAvatar(string sexo, Cliente.Avatar avatar)
    {
        PnlAvatar.PreencherInfo(sexo, avatar);

        if (sexo == "Masculino")
            BtnSexoMasc.isOn = true;
        else
            BtnSexoFem.isOn = true;
    }
    #endregion

    #endregion

}
