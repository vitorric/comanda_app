using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using APIModel;
using FirebaseModel;
using Network;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Login : MonoBehaviour
{
    public FirebaseManager firebaseManager;

    [Header("Canvas")]
    public Canvas PnlLogin;
    public Canvas PnlRecuperarSenha;
    public Canvas[] PnlEtapas;

    [Header("Button")]
    public Button BtnEntrar;
    public Button BtnCadastrar;
    public Button BtnFacebook;
    public Button BtnRecupSenha;
    public Button BtnRecupSenhaVoltar;
    public Button BtnRecupSenhaEnviar;
    public Button BtnEtapa0Voltar;
    public Button BtnEtapa0Continuar;
    public Button BtnEtapa1Voltar;
    public Button BtnEtapa1Continuar;
    public Button BtnEtapa2Voltar;
    public Button BtnEtapa2Continuar;
    public Button BtnEtapa3Voltar;
    public Button BtnEtapa3Confirmar;
    public Button BtnEdicaoAvatar;

    [Header("Toggle")]
    public Toggle BtnSexoMasc;
    public Toggle BtnSexoFem;

    [Header("Tela de Login")]
    public TMP_InputField TxtEmail;
    public TMP_InputField TxtSenha;

    [Header("Recuperar Senha")]
    public TMP_InputField TxtEmailRecSenha;

    [Header("Cadastro Etapa - 0")]
    public TMP_InputField TxtEmailCadastro;
    public TMP_InputField TxtNomeCadastro;

    [Header("Cadastro Etapa - 1")]
    public TMP_InputField TxtCPFCadastro;
    public TMP_InputField TxtDataNascCadastro;

    [Header("Cadastro Etapa - 2")]
    public TMP_InputField TxtApelidoCadastro;
    public AvatarObj PnlAvatar;

    [Header("Cadastro Etapa - 3")]
    public TMP_InputField TxtSenhaCadastro;
    public TMP_InputField TxtSenhaCadastroConfirm;

    private string tipoLogin = "normal";
    private string socialId = string.Empty;
    FacebookManager fbManager;

    #region Awake
    void Awake()
    {
        if (Application.isEditor)
        {
            TxtEmail.text = "arus@email.com";
            TxtSenha.text = "1234";
        }

        fbManager = new FacebookManager() { cadastrar = cadastroSocial };

        configurarListener();
        configurarInputListener();
    }
    #endregion

    #region configurarListener
    private void configurarListener()
    {
        BtnEntrar.onClick.AddListener(() => BtnLogar());
        BtnCadastrar.onClick.AddListener(() => BtnCadastrarAvantarEtapa(0));
        BtnFacebook.onClick.AddListener(() => btnFacebook());
        BtnRecupSenha.onClick.AddListener(() => BtnAbrirPnlRecuperarSenha());
        BtnRecupSenhaVoltar.onClick.AddListener(() => BtnRecuperarSenhaVoltarLogin());
        BtnRecupSenhaEnviar.onClick.AddListener(() => BtnRecuperarSenha());

        BtnEtapa0Voltar.onClick.AddListener(() => BtnCadastrarVoltar(0));
        BtnEtapa1Voltar.onClick.AddListener(() => BtnCadastrarVoltar(1));
        BtnEtapa2Voltar.onClick.AddListener(() => BtnCadastrarVoltar(2));
        BtnEtapa3Voltar.onClick.AddListener(() => BtnCadastrarVoltar(3));


        BtnEtapa0Continuar.onClick.AddListener(() => BtnCadastrarAvantarEtapa(1));
        BtnEtapa1Continuar.onClick.AddListener(() => BtnCadastrarAvantarEtapa(2));
        BtnEtapa2Continuar.onClick.AddListener(() => BtnCadastrarAvantarEtapa(3));
        BtnEtapa3Confirmar.onClick.AddListener(() => BtnCadastrarAvantarEtapa(4));

        BtnEdicaoAvatar.onClick.AddListener(() => BtnAbrirEdicaoAvatar());

        BtnSexoMasc.onValueChanged.AddListener((result) => ChkChanged(BtnSexoMasc.gameObject, true));
        BtnSexoFem.onValueChanged.AddListener((result) => ChkChanged(BtnSexoFem.gameObject, true));
    }
    #endregion

    #region configurarInputListener
    private void configurarInputListener()
    {
        TxtEmail.onSubmit.AddListener(delegate { TxtSenha.Select(); });
        TxtSenha.onSubmit.AddListener(delegate { logar(); });

        //etapa 0
        TxtEmailCadastro.onSubmit.AddListener(delegate { TxtNomeCadastro.Select(); });
        TxtNomeCadastro.onSubmit.AddListener(delegate { StartCoroutine(avancarEtapa(1)); TxtCPFCadastro.Select(); });

        //etapa 1
        //TxtCPFCadastro.onValueChanged.AddListener(delegate { Util.FormatarCPF(TxtCPFCadastro); });
        TxtCPFCadastro.onSubmit.AddListener(delegate { TxtDataNascCadastro.Select(); });
        //TxtDataNascCadastro.onValueChanged.AddListener(delegate { Util.FormatarDataNasc(TxtDataNascCadastro); });
        TxtDataNascCadastro.onSubmit.AddListener(delegate { StartCoroutine(avancarEtapa(2)); TxtApelidoCadastro.Select(); });

        //etapa 2 nao tem
        //etapa 3
        TxtSenhaCadastro.onSubmit.AddListener(delegate { TxtSenhaCadastroConfirm.Select(); });
        TxtSenhaCadastroConfirm.onSubmit.AddListener(delegate { StartCoroutine(avancarEtapa(4)); });

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

    #region btnFacebook
    private void btnFacebook()
    {
        fbManager.LoginFacebook();
    }
    #endregion

    #region BtnAbrirPnlRecuperarSenha
    public void BtnAbrirPnlRecuperarSenha()
    {
        EasyAudioUtility.Instance.Play(EasyAudioUtility.Som.Click_OK);

        PnlRecuperarSenha.enabled = true;
        PnlLogin.enabled = false;
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
    public void BtnCadastrarAvantarEtapa(int etapa)
    {
        EasyAudioUtility.Instance.Play(EasyAudioUtility.Som.Click_OK);
        StartCoroutine(avancarEtapa(etapa));
    }
    #endregion

    #region BtnCadastrarVoltar
    public void BtnCadastrarVoltar(int etapa)
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

        Dictionary<string, object> form = new Dictionary<string, object>
        {
            { "email", TxtEmail.text },
            { "password", Util.GerarHashMd5(TxtSenha.text) },
            { "tipoLogin", tipoLogin },
            { "socialId", socialId},
            { "deviceId", AppManager.Instance.deviceId },
            { "tokenFirebase", AppManager.Instance.tokenFirebase }
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

            AppManager.Instance.GravarSession(response.token, response._id,
            JsonConvert.SerializeObject(new Cliente.Credenciais
            {
                email = TxtEmail.text,
                password = Util.GerarHashMd5(TxtSenha.text),
                tipoLogin = tipoLogin
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

        Dictionary<string, object> form = new Dictionary<string, object>
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
        PnlRecuperarSenha.enabled = false;
        PnlLogin.enabled = true;
        TxtEmailRecSenha.text = string.Empty;
    }
    #endregion

    #region avancarEtapa
    private IEnumerator avancarEtapa(int proximaEtapa)
    {
        int etapaAtual = proximaEtapa - 1;

        Dictionary<string, object> form = new Dictionary<string, object>();

        bool podeAvancar = false;

        //nome e email
        if (etapaAtual == 0)
        {
            if (TxtEmailCadastro.text == string.Empty ||
                TxtNomeCadastro.text == string.Empty)
            {
                StartCoroutine(AlertaManager.Instance.ChamarAlertaMensagem(AlertaManager.MsgAlerta.PreenchaOsCampos, false));
                yield break;
            }

            if (!verificarEmailValido(TxtEmailCadastro.text))
            {
                StartCoroutine(AlertaManager.Instance.ChamarAlertaMensagem(AlertaManager.MsgAlerta.EmailNaoValido, false));
                yield break;
            }

            if (TxtNomeCadastro.text.Split(' ').Length < 2)
            {
                StartCoroutine(AlertaManager.Instance.ChamarAlertaMensagem(AlertaManager.MsgAlerta.NomeSobreNome, false));
                yield break;
            }

            form.Add("email", TxtEmailCadastro.text);

            AppManager.Instance.AtivarLoader();

            yield return StartCoroutine(ClienteAPI.ClienteCadastrarValidarEmail(form, (sucesso, error) =>
            {
                if (error != null)
                {
                    Debug.Log(error);
                    AppManager.Instance.DesativarLoaderAsync();

                    StartCoroutine(AlertaManager.Instance.ChamarAlertaMensagem(error, false));
                    return;
                }

                podeAvancar = true;

                AppManager.Instance.DesativarLoaderAsync();
            }));
        }

        //cpf e data nasc
        if (etapaAtual == 1)
        {
            if (TxtDataNascCadastro.text == string.Empty ||
                TxtCPFCadastro.text == string.Empty)
            {
                StartCoroutine(AlertaManager.Instance.ChamarAlertaMensagem(AlertaManager.MsgAlerta.PreenchaOsCampos, false));
                yield break;
            }

            if (!verificarCPFValido(TxtCPFCadastro.text))
            {
                StartCoroutine(AlertaManager.Instance.ChamarAlertaMensagem(AlertaManager.MsgAlerta.PreenchaCPFValido, false));
                yield break;
            }

            if (!verificarDataValido(TxtDataNascCadastro.text))
            {
                StartCoroutine(AlertaManager.Instance.ChamarAlertaMensagem(AlertaManager.MsgAlerta.DataInvalida, false));
                yield break;
            }

            if (!verificarMaiorIdade(TxtDataNascCadastro.text))
            {
                StartCoroutine(AlertaManager.Instance.ChamarAlertaMensagem(AlertaManager.MsgAlerta.Menor18Anos, false));
                yield break;
            }

            form.Add("cpf", TxtCPFCadastro.text);

            AppManager.Instance.AtivarLoader();

            yield return StartCoroutine(ClienteAPI.ClienteCadastrarValidarCPF(form, (sucesso, error) =>
            {
                if (error != null)
                {
                    Debug.Log(error);
                    AppManager.Instance.DesativarLoaderAsync();

                    StartCoroutine(AlertaManager.Instance.ChamarAlertaMensagem(error, false));
                    return;
                }

                podeAvancar = true;

                AppManager.Instance.DesativarLoaderAsync();
            }));
        }

        //apelido e avatar
        if (etapaAtual == 2)
        {
            if (TxtApelidoCadastro.text == string.Empty)
            {
                StartCoroutine(AlertaManager.Instance.ChamarAlertaMensagem(AlertaManager.MsgAlerta.PreenchaOsCampos, false));
                yield break;
            }

            if (TxtApelidoCadastro.text.Length < 3)
            {
                StartCoroutine(AlertaManager.Instance.ChamarAlertaMensagem(AlertaManager.MsgAlerta.ApelidoMenor3Char, false));
                yield break;
            }

            form.Add("apelido", TxtApelidoCadastro.text);

            AppManager.Instance.AtivarLoader();

            yield return StartCoroutine(ClienteAPI.ClienteCadastrarValidarApelido(form, (sucesso, error) =>
            {
                if (error != null)
                {
                    Debug.Log(error);
                    AppManager.Instance.DesativarLoaderAsync();

                    StartCoroutine(AlertaManager.Instance.ChamarAlertaMensagem(error, false));
                    return;
                }

                if (tipoLogin == "normal")
                {
                    podeAvancar = true;

                    AppManager.Instance.DesativarLoaderAsync();
                }
                else
                {
                    StartCoroutine(cadastrar());
                }
            }));
        }

        //confirmacao senha e criacao
        if (etapaAtual == 3)
        {
            if (tipoLogin != "normal")
            {
                StartCoroutine(cadastrar());
                yield break;
            }

            if (TxtSenhaCadastro.text == string.Empty ||
                TxtSenhaCadastroConfirm.text == string.Empty)
            {
                StartCoroutine(AlertaManager.Instance.ChamarAlertaMensagem(AlertaManager.MsgAlerta.PreenchaOsCampos, false));
                yield break;
            }

            if (TxtSenhaCadastro.text.Length < 6)
            {
                StartCoroutine(AlertaManager.Instance.ChamarAlertaMensagem(AlertaManager.MsgAlerta.SenhaMenor6Char, false));
                yield break;
            }

            if (ValidarSenhas() == false)
            {
                StartCoroutine(AlertaManager.Instance.ChamarAlertaMensagem(AlertaManager.MsgAlerta.SenhasNaoConferem, false));
                yield break;
            }

            StartCoroutine(cadastrar());

            yield break;
        }


        if (proximaEtapa == 0)
        {
            PnlLogin.enabled = false;
            PnlEtapas[0].enabled = true;
            yield break;
        }

        if (podeAvancar)
        {
            if (etapaAtual > -1)
                PnlEtapas[etapaAtual].enabled = false;

            PnlEtapas[proximaEtapa].enabled = true;
        }

        //if (proximaEtapa == 1)
        //{
        //    PnlCadasEtapa1.enabled = false;
        //    PnlCadasEtapa2.enabled = true;
        //    return;
        //}

    }
    #endregion

    #region voltarEtapa
    private void voltarEtapa(int etapaAtual)
    {
        int etapaAnterior = etapaAtual - 1;

        PnlEtapas[etapaAtual].enabled = false;

        if (etapaAnterior < 0)
        {
            LimparFormCadastro();
            PnlLogin.enabled = true;
            return;
        }

        PnlEtapas[etapaAnterior].enabled = true;

    }
    #endregion

    #region cadastroSocial
    private void cadastroSocial(string socialId, string nome, string email, string tipo)
    {
        Dictionary<string, object> form = new Dictionary<string, object>
        {
            { "socialId", socialId },
            { "deviceId", AppManager.Instance.deviceId },
            { "tokenFirebase", AppManager.Instance.tokenFirebase }
        };

        StartCoroutine(ClienteAPI.ClienteLoginFacebook(form, (response, error) => {

            if (error != null)
            {
                Debug.Log(error);
                AppManager.Instance.DesativarLoaderAsync();
                StartCoroutine(AlertaManager.Instance.ChamarAlertaMensagem(error, false));
                return;
            }

            if (response != null)
            {

                AppManager.Instance.GravarSession(response.token, response._id,
                JsonConvert.SerializeObject(new Cliente.Credenciais
                {
                    email = TxtEmail.text,
                    tipoLogin = tipoLogin
                }));

                buscarClienteNoFireBase();

                return;
            }

            LimparFormCadastro();

            TxtEmailCadastro.text = email;
            TxtNomeCadastro.text = nome;
            this.socialId = socialId;
            TxtEmailCadastro.interactable = (string.IsNullOrEmpty(email)) ? true : false;
            TxtNomeCadastro.interactable = false;

            tipoLogin = tipo;

            PnlLogin.enabled = false;
            PnlEtapas[0].enabled = true;

            AppManager.Instance.DesativarLoaderAsync();
        }));

    }
    #endregion

    #region cadastrar
    private IEnumerator cadastrar()
    {
        string senha = Util.GerarHashMd5(TxtSenhaCadastro.text);
        Dictionary<string, object> data = new Dictionary<string, object>
        {
            { "email", TxtEmailCadastro.text },
            { "password", senha },
            { "apelido", TxtApelidoCadastro.text },
            { "nome", TxtNomeCadastro.text },
            { "sexo", (BtnSexoMasc.isOn) ? "Masculino" : "Feminino" },
            { "avatar", JsonConvert.SerializeObject(prepararAvatarCadastro()) },
            { "tipoLogin", tipoLogin },
            { "deviceId", AppManager.Instance.deviceId },
            { "tokenFirebase", AppManager.Instance.tokenFirebase },
            { "cpf", TxtCPFCadastro.text },
            { "dataNascimento", Util.formatarDataParaAPI(TxtDataNascCadastro.text) },
            { "socialId", socialId }
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

            AppManager.Instance.GravarSession(response.token, response._id,
            JsonConvert.SerializeObject(new Cliente.Credenciais
            {
                email = TxtEmailCadastro.text,
                password = senha,
                tipoLogin = tipoLogin
            }));

            buscarClienteNoFireBase();
            return;
        }));
    }
    #endregion

    #region buscarNoFireBase
    private async void buscarClienteNoFireBase()
    {
        Cliente.ClienteLogado = await firebaseManager.ObterUsuario(AppManager.Instance.Obter());

        SceneManager.LoadSceneAsync("Main");
    }

    #region prepararAvatarCadastro
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

    #region verificarEmailValido
    private bool verificarEmailValido(string email)
    {
        Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
        Match match = regex.Match(email);
        if (match.Success)
            return true;
        else
            return false;
    }
    #endregion

    #region verificarCPFValido
    private bool verificarCPFValido(string cpf)
    {
        Regex regex = new Regex(@"^\d{3}\.\d{3}\.\d{3}-\d{2}$");
        Match match = regex.Match(cpf);
        if (match.Success)
            return true;
        else
            return false;
    }
    #endregion

    #endregion

    #region Metodos Publicos

    #region LimparFormCadastro
    public void LimparFormCadastro()
    {
        socialId = string.Empty;
        tipoLogin = "normal";
        TxtEmailCadastro.interactable = true;
        TxtNomeCadastro.interactable = true;
        TxtSenhaCadastro.gameObject.SetActive(true);
        TxtSenhaCadastroConfirm.gameObject.SetActive(true);

        TxtEmailCadastro.text = string.Empty;
        TxtSenhaCadastro.text = string.Empty;
        TxtSenhaCadastroConfirm.text = string.Empty;
        TxtApelidoCadastro.text = string.Empty;
        TxtNomeCadastro.text = string.Empty;
        TxtCPFCadastro.text = string.Empty;
        TxtDataNascCadastro.text = string.Empty;
        BtnSexoMasc.isOn = true;
        BtnSexoFem.isOn = false;

        PnlAvatar.PreencherInfo("Masculino", null);

        fbManager.LogoutFacebook();
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

    #region verificarDataValido
    private bool verificarDataValido(string data)
    {
        try
        {
            Regex regex = new Regex(@"(((0|1)[0-9]|2[0-9]|3[0-1])\/(0[1-9]|1[0-2])\/((19|20)\d\d))$");

            //Verify whether date entered in dd/MM/yyyy format.
            bool isValid = regex.IsMatch(data.Trim());

            //Verify whether entered date is Valid date.
            DateTime dt;
            isValid = DateTime.TryParseExact(data, "dd/MM/yyyy", new CultureInfo("pt-BR"), DateTimeStyles.None, out dt);

            return isValid;
        }
        catch
        {
            return false;
        }
    }
    #endregion

    #region verificarMaiorIdade
    private bool verificarMaiorIdade(string data)
    {
        try
        {
            int idade = Util.CalcularIdade(Convert.ToDateTime(data, new CultureInfo("pt-BR")));
            return (idade > 18) ? true : false;
        }
        catch (Exception e)
        {
            Debug.Log(Util.GetExceptionDetails(e));
            return false;
        }

    }
    #endregion

    #endregion

}
