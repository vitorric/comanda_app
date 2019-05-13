using System.Collections;
using System.Collections.Generic;
using APIModel;
using FirebaseModel;
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

    private Cliente.Avatar avatar;

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
        AnimacoesTween.AnimarObjeto(BtnEntrar.gameObject, AnimacoesTween.TiposAnimacoes.Button_Click, () => logar(), AppManager.TEMPO_ANIMACAO_ABRIR_CLICK_BOTAO);

    }
    #endregion

    #region BtnAbrirPnlRecuperarSenha
    public void BtnAbrirPnlRecuperarSenha()
    {
        EasyAudioUtility.Instance.Play(EasyAudioUtility.Som.Click_OK);

        AnimacoesTween.AnimarObjeto(BtnRecupSenha.gameObject, AnimacoesTween.TiposAnimacoes.Button_Click, () =>
        {
            PnlRecuperarSenha.SetActive(true);
            PnlLogin.SetActive(false);
            TxtEmailRecSenha.text = string.Empty;
        },
        AppManager.TEMPO_ANIMACAO_ABRIR_CLICK_BOTAO);
    }
    #endregion

    #region BtnRecuperarSenhaVoltarLogin
    public void BtnRecuperarSenhaVoltarLogin()
    {
        EasyAudioUtility.Instance.Play(EasyAudioUtility.Som.Click_Cancel);

        AnimacoesTween.AnimarObjeto(BtnRecupSenhaVoltar.gameObject, AnimacoesTween.TiposAnimacoes.Button_Click, 
        () => fecharPnlRecuperarSenha(),
        AppManager.TEMPO_ANIMACAO_ABRIR_CLICK_BOTAO);
    }
    #endregion

    #region BtnRecuperarSenha
    public void BtnRecuperarSenha()
    {
        EasyAudioUtility.Instance.Play(EasyAudioUtility.Som.Click_OK);
        AnimacoesTween.AnimarObjeto(BtnRecupSenhaEnviar.gameObject, AnimacoesTween.TiposAnimacoes.Button_Click, 
        () => recuperarSenha(),
        AppManager.TEMPO_ANIMACAO_ABRIR_CLICK_BOTAO);
    }
    #endregion

    #region BtnCadastrarAvantarEtapa
    public void BtnCadastrarAvantarEtapa(int etapa, GameObject objClicado)
    {
        EasyAudioUtility.Instance.Play(EasyAudioUtility.Som.Click_OK);
        AnimacoesTween.AnimarObjeto(objClicado, AnimacoesTween.TiposAnimacoes.Button_Click, 
            () => avancarEtapa(etapa), 
            AppManager.TEMPO_ANIMACAO_ABRIR_CLICK_BOTAO);
    }
    #endregion

    #region BtnCadastrarVoltar
    public void BtnCadastrarVoltar(int etapa, GameObject objClicado)
    {
        EasyAudioUtility.Instance.Play(EasyAudioUtility.Som.Click_Cancel);
        AnimacoesTween.AnimarObjeto(objClicado, AnimacoesTween.TiposAnimacoes.Button_Click, 
        () => voltarEtapa(etapa),
        AppManager.TEMPO_ANIMACAO_ABRIR_CLICK_BOTAO);
    }
    #endregion

    #region BtnConfirmarCadastro
    public void BtnConfirmarCadastro()
    {
        EasyAudioUtility.Instance.Play(EasyAudioUtility.Som.Click_OK);
        AnimacoesTween.AnimarObjeto(BtnEtapa2Confirmar.gameObject, AnimacoesTween.TiposAnimacoes.Button_Click, 
            () => StartCoroutine(cadastrar()),
            AppManager.TEMPO_ANIMACAO_ABRIR_CLICK_BOTAO);
    }
    #endregion

    #region BtnAbrirEdicaoAvatar
    public void BtnAbrirEdicaoAvatar()
    {
        EasyAudioUtility.Instance.Play(EasyAudioUtility.Som.Click_OK);
        AnimacoesTween.AnimarObjeto(BtnEdicaoAvatar.gameObject, AnimacoesTween.TiposAnimacoes.Button_Click, () =>
        {
            SceneManager.LoadSceneAsync("EdicaoChar", LoadSceneMode.Additive);
        },
        AppManager.TEMPO_ANIMACAO_ABRIR_CLICK_BOTAO);
    }
    #endregion

    #endregion

    #region Checkbox

    #region ChkChanged
    public void ChkChanged(GameObject objClicado, bool tocarSom = false)
    {
        if (tocarSom)
            EasyAudioUtility.Instance.Play(EasyAudioUtility.Som.Click_OK);

        PnlAvatar.PreencherInfo((BtnSexoMasc.isOn) ? "Masculino" : "Feminino", avatar);
        avatar = PnlAvatar.Avatar;
    }
    #endregion

    #endregion

    #region Metodos privados

    #region logar
    private void logar()
    {
        if (TxtEmail.text == string.Empty || TxtSenha.text == string.Empty)
        {
            Debug.Log("vazios campos");
            //StartCoroutine(comunicadorAPI.Alerta.ChamarAlerta(Alerta.MsgAlerta.PreenchaOsCampos, comunicadorAPI.PnlPrincipal));
            return;
        }
        Debug.Log(Util.GerarHashMd5(TxtSenha.text));
        WWWForm form = new WWWForm();
        form.AddField("email", TxtEmail.text);
        form.AddField("password", Util.GerarHashMd5(TxtSenha.text));

        AppManager.Instance.AtivarLoader();

        StartCoroutine(APIManager.Instance.Post(APIManager.URLs.ClienteLogin, form,
        (response) =>
        {
            APIManager.Retorno<Cliente.SessaoCliente> retornoAPI = JsonConvert.DeserializeObject<APIManager.Retorno<Cliente.SessaoCliente>>(response);

            if (retornoAPI.sucesso)
            {
                Cliente.GravarSession(retornoAPI.retorno.token, retornoAPI.retorno._id,
                    JsonConvert.SerializeObject(new Cliente.Credenciais
                    {
                        email = TxtEmail.text,
                        password = Util.GerarHashMd5(TxtSenha.text)
                    }));

                buscarClienteNoFireBase();
                return;
            }

            AppManager.Instance.DesativarLoader();
            EasyAudioUtility.Instance.Play(EasyAudioUtility.Som.Error);
            //StartCoroutine(comunicadorAPI.Alerta.ChamarAlerta(retornoAPI.msg, comunicadorAPI.PnlPrincipal));
        },
        (error) =>
        {
            AppManager.Instance.DesativarLoader();
            EasyAudioUtility.Instance.Play(EasyAudioUtility.Som.Error);
            //StartCoroutine(comunicadorAPI.Alerta.ChamarAlerta(error, comunicadorAPI.PnlPrincipal));
        }));

    }
    #endregion

    #region recuperarSenha
    private void recuperarSenha()
    {

        if (TxtEmailRecSenha.text == string.Empty)
        {
            //StartCoroutine(comunicadorAPI.Alerta.ChamarAlerta(Alerta.MsgAlerta.PreenchaOsCampos, comunicadorAPI.PnlPrincipal));
            return;
        }

        WWWForm form = new WWWForm();
        form.AddField("email", TxtEmailRecSenha.text);

        StartCoroutine(APIManager.Instance.Post(APIManager.URLs.ClienteRecuperarSenha, form,
        (response) =>
        {
            APIManager.Retorno<string> retornoAPI = JsonConvert.DeserializeObject<APIManager.Retorno<string>>(response);

            //StartCoroutine(comunicadorAPI.Alerta.ChamarAlerta(retornoAPI.msg, comunicadorAPI.PnlPrincipal));

            if (retornoAPI.sucesso)
            {
                fecharPnlRecuperarSenha();
            }

        },
        (error) =>
        {
            //TODO: Tratar Error
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
                //StartCoroutine(comunicadorAPI.Alerta.ChamarAlerta(Alerta.MsgAlerta.PreenchaOsCampos, comunicadorAPI.PnlPrincipal));
                return;
            }

            if (ValidarSenhas() == false)
            {
                //StartCoroutine(comunicadorAPI.Alerta.ChamarAlerta(Alerta.MsgAlerta.SenhasNaoConferem, comunicadorAPI.PnlPrincipal));
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
        WWWForm form = new WWWForm();
        form.AddField("email", TxtEmailCadastro.text);
        form.AddField("password", senha);
        form.AddField("apelido", TxtApelidoCadastro.text);
        form.AddField("nome", TxtNomeCadastro.text);
        form.AddField("sexo", (BtnSexoMasc.isOn) ? "Masculino" : "Feminino");
        form.AddField("avatar", JsonConvert.SerializeObject(prepararAvatarCadastro()));

        AppManager.Instance.AtivarLoader();

        yield return StartCoroutine(APIManager.Instance.Post(APIManager.URLs.ClienteCadastrar, form,
        (response) =>
        {
            APIManager.Retorno<Cliente.SessaoCliente> retornoAPI = JsonConvert.DeserializeObject<APIManager.Retorno<Cliente.SessaoCliente>>(response);
            Debug.Log(response);
            if (retornoAPI.sucesso)
            {
                Cliente.GravarSession(retornoAPI.retorno.token, retornoAPI.retorno._id,
                JsonConvert.SerializeObject(new Cliente.Credenciais
                {
                    email = TxtEmail.text,
                    password = senha
                }));

                buscarClienteNoFireBase();
                return;
            }

            AppManager.Instance.DesativarLoader();

            EasyAudioUtility.Instance.Play(EasyAudioUtility.Som.Error);
            //StartCoroutine(comunicadorAPI.Alerta.ChamarAlerta(retornoAPI.msg, comunicadorAPI.PnlPrincipal));
        },
        (error) =>
        {
            AppManager.Instance.DesativarLoader();
            EasyAudioUtility.Instance.Play(EasyAudioUtility.Som.Error);
            //StartCoroutine(comunicadorAPI.Alerta.ChamarAlerta(error, comunicadorAPI.PnlPrincipal));
        }));
    }
    #endregion

    #region buscarNoFireBase
    private async void buscarClienteNoFireBase()
    {
        ClienteFirebase cliente = new ClienteFirebase();
        Cliente.ClienteLogado = await cliente.ObterUsuario(Cliente.Obter());

        AppManager.Instance.DesativarLoader();

        SceneManager.LoadSceneAsync("Main");
    }

    #region 
    private Dictionary<string, object> prepararAvatarCadastro()
    {
        Dictionary<string, object> avatarDic = new Dictionary<string, object>
        {
            { "corpo", avatar.corpo },
            { "cabeca", avatar.cabeca },
            { "nariz", avatar.nariz },
            { "olhos", avatar.olhos },
            { "boca", avatar.boca },
            { "roupa", avatar.roupa },
            { "cabeloTraseiro", avatar.cabeloTraseiro },
            { "cabeloFrontal", avatar.cabeloFrontal },
            { "barba", avatar.barba },
            { "sombrancelhas", avatar.sombrancelhas },
            { "orelha", avatar.orelha },
            { "corPele", avatar.corPele },
            { "corCabelo", avatar.corCabelo },
            { "corBarba", avatar.corBarba }
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

        avatar = null;
        PnlAvatar.PreencherInfo("Masculino", null);
        avatar = PnlAvatar.Avatar;
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
        this.avatar = avatar;
        PnlAvatar.PreencherInfo(sexo, avatar);

        if (sexo == "Masculino")
            BtnSexoMasc.isOn = true;
        else
            BtnSexoFem.isOn = true;
    }
    #endregion

    #endregion

}
