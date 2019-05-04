using System;
using System.Collections;
using System.Collections.Generic;
using APIModel;
using FirebaseModel;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Login : MonoBehaviour
{

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
    public InputField TxtSenhaCadastro;
    public InputField TxtSenhaCadastroConfirm;

    [Header("Cadastro Etapa - 2")]
    public InputField TxtApelidoCadastro;
    public InputField TxtNomeCadastro;
    public InputField TxtCPFCadastro;
    public InputField TxtIdadeCadastro;

    [Header("Cadastro Etapa - 3")]
    public Toggle ChkMasculinoCadastro;
    public Toggle ChkFemininoCadastro;
    public GameObject PnlAvatar;

    [Header("Cadastro Etapa - 4")]
    public Text txtEmailInfo;
    public Text txtApelidoInfo;
    public Text txtNomeInfo;
    public Text txtCPFInfo;
    public Text txtIdadeInfo;
    public Text txtSexoInfo;
    public GameObject PnlAvatarInfo;

    private Cliente.Avatar avatar;

    void Awake()
    {
        if (Application.isEditor)
        {
            TxtEmail.text = "email1@email.com";
            TxtSenha.text = "1234";
        }
    }

    #region Login
    public void BtnLogar()
    {
        SomController.Tocar(SomController.Som.Click_OK);
        AnimacoesTween.AnimarObjeto(EventSystem.current.currentSelectedGameObject, AnimacoesTween.TiposAnimacoes.Button_Click, () =>
        logar());

    }

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

        #region Post Login Cliente
        AppManager.Instance.AtivarLoader();
        
        StartCoroutine(APIManager.Instance.Post(APIManager.URLs.ClienteLogin, form,
        (response) =>
        {
            APIManager.Retorno<Cliente.SessaoCliente> retornoAPI = JsonConvert.DeserializeObject<APIManager.Retorno<Cliente.SessaoCliente>>(response);
            
            if (retornoAPI.sucesso)
            {
                Cliente.GravarSession(retornoAPI.retorno.token, retornoAPI.retorno._id,
                    JsonConvert.SerializeObject(new Cliente.Credenciais {
                        email = TxtEmail.text,
                        password = Util.GerarHashMd5(TxtSenha.text)
                    }));

                buscarNoFireBase();
                return;
            }

            AppManager.Instance.DesativarLoader();
            SomController.Tocar(SomController.Som.Error);
            //StartCoroutine(comunicadorAPI.Alerta.ChamarAlerta(retornoAPI.msg, comunicadorAPI.PnlPrincipal));
        },
        (error) =>
        {
            AppManager.Instance.DesativarLoader();
            SomController.Tocar(SomController.Som.Error);
            //StartCoroutine(comunicadorAPI.Alerta.ChamarAlerta(error, comunicadorAPI.PnlPrincipal));
        }));
        #endregion

    }

    public void BtnAbrirPnlRecuperarSenha()
    {
        SomController.Tocar(SomController.Som.Click_OK);
        AnimacoesTween.AnimarObjeto(EventSystem.current.currentSelectedGameObject, AnimacoesTween.TiposAnimacoes.Button_Click, () =>
        {
            PnlRecuperarSenha.SetActive(true);
            PnlLogin.SetActive(false);
            TxtEmailRecSenha.text = string.Empty;
        });
    }

    #endregion

    #region Recuperar Senha
    public void BtnRecuperarSenhaVoltarLogin()
    {
        SomController.Tocar(SomController.Som.Click_Cancel);
        AnimacoesTween.AnimarObjeto(EventSystem.current.currentSelectedGameObject, AnimacoesTween.TiposAnimacoes.Button_Click, () =>
        fecharPnlRecuperarSenha());
    }
    public void BtnRecuperarSenha()
    {
        SomController.Tocar(SomController.Som.Click_OK);
        AnimacoesTween.AnimarObjeto(EventSystem.current.currentSelectedGameObject, AnimacoesTween.TiposAnimacoes.Button_Click, () => recuperarSenha());
    }

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

    private void fecharPnlRecuperarSenha()
    {
        PnlRecuperarSenha.SetActive(false);
        PnlLogin.SetActive(true);
        TxtEmailRecSenha.text = string.Empty;
    }
    #endregion    

    #region Cadastrar
    public void BtnCadastrar(int etapa)
    {
        SomController.Tocar(SomController.Som.Click_OK);
        AnimacoesTween.AnimarObjeto(EventSystem.current.currentSelectedGameObject, AnimacoesTween.TiposAnimacoes.Button_Click, () => avancarEtapa(etapa));
    }

    private void avancarEtapa(int etapa)
    {

        if (validarEtapa(etapa) == false)
        {
            //StartCoroutine(comunicadorAPI.Alerta.ChamarAlerta(Alerta.MsgAlerta.PreenchaOsCampos, comunicadorAPI.PnlPrincipal));
            return;
        }

        if (etapa == 1)
        {
            if (validarSenhas() == false)
            {
                //StartCoroutine(comunicadorAPI.Alerta.ChamarAlerta(Alerta.MsgAlerta.SenhasNaoConferem, comunicadorAPI.PnlPrincipal));
                return;
            }
        }

        if (etapa == 2)
        {
            string[] valoresDatas = TxtIdadeCadastro.text.Split('/');

            if (Convert.ToInt32(valoresDatas[0]) > 31 || Convert.ToInt32(valoresDatas[0]) < 1 ||
                Convert.ToInt32(valoresDatas[1]) > 12 || Convert.ToInt32(valoresDatas[1]) < 1)
            {
                //StartCoroutine(comunicadorAPI.Alerta.ChamarAlerta(Alerta.MsgAlerta.DataInvalida, comunicadorAPI.PnlPrincipal));
                return;
            }

            if (DateTime.Now.Year - Convert.ToInt32(valoresDatas[2]) < 18 || (DateTime.Now.Year - Convert.ToInt32(valoresDatas[2])) == 18 && Convert.ToInt32(valoresDatas[1]) > DateTime.Now.Month)
            {
                //StartCoroutine(comunicadorAPI.Alerta.ChamarAlerta(Alerta.MsgAlerta.MenorIdade, comunicadorAPI.PnlPrincipal));
                return;
            }
        }

        if (etapa == 0)
        {
            PnlLogin.SetActive(false);
            limparFormCadastro();
        }
        else
        {
            PnlCadastrarEtapas[etapa - 1].SetActive(false);
        }

        PnlCadastrarEtapas[etapa].SetActive(true);
    }

    public void BtnCadastrarVoltar(int etapa)
    {
        SomController.Tocar(SomController.Som.Click_Cancel);
        AnimacoesTween.AnimarObjeto(EventSystem.current.currentSelectedGameObject, AnimacoesTween.TiposAnimacoes.Button_Click, () => voltarEtapa(etapa));
    }

    private void voltarEtapa(int etapa)
    {

        if (etapa == 0)
            PnlLogin.SetActive(true);
        else
            PnlCadastrarEtapas[etapa - 1].SetActive(true);

        PnlCadastrarEtapas[etapa].SetActive(false);

    }

    public void ChkChanged()
    {
        if (EventSystem.current.currentSelectedGameObject.name.Contains("Sexo"))
            SomController.Tocar(SomController.Som.Click_OK);

        PnlAvatar.GetComponent<AvatarObj>().PreencherInfo((ChkMasculinoCadastro.isOn) ? "Masculino" : "Feminino", avatar);
        avatar = PnlAvatar.GetComponent<AvatarObj>().Avatar;
    }

    public void limparFormCadastro()
    {
        TxtEmailCadastro.text = string.Empty;
        TxtSenhaCadastro.text = string.Empty;
        TxtSenhaCadastroConfirm.text = string.Empty;
        TxtApelidoCadastro.text = string.Empty;
        TxtNomeCadastro.text = string.Empty;
        TxtCPFCadastro.text = string.Empty;
        TxtIdadeCadastro.text = string.Empty;
        ChkMasculinoCadastro.isOn = true;
        ChkFemininoCadastro.isOn = false;

        avatar = null;
        PnlAvatar.GetComponent<AvatarObj>().PreencherInfo("Masculino", null);
        avatar = PnlAvatar.GetComponent<AvatarObj>().Avatar;
    }

    public bool validarEtapa(int etapa)
    {
        bool retorno = true;
        switch (etapa)
        {
            case 1:
                if (TxtEmailCadastro.text == string.Empty ||
                    TxtSenhaCadastro.text == string.Empty ||
                    TxtSenhaCadastroConfirm.text == string.Empty)
                    retorno = false;
                break;
            case 2:
                if (TxtApelidoCadastro.text == string.Empty ||
                    TxtNomeCadastro.text == string.Empty ||
                    TxtCPFCadastro.text == string.Empty ||
                    TxtIdadeCadastro.text == string.Empty)
                    retorno = false;
                break;
            case 3:
                string sexo = (ChkMasculinoCadastro.isOn) ? "Masculino" : "Feminino";
                txtEmailInfo.text = TxtEmailCadastro.text;
                txtApelidoInfo.text = TxtApelidoCadastro.text;
                txtNomeInfo.text = TxtNomeCadastro.text;
                txtCPFInfo.text = TxtCPFCadastro.text;
                PnlAvatarInfo.GetComponent<AvatarObj>().PreencherInfo(sexo, avatar);
                txtIdadeInfo.text = TxtIdadeCadastro.text;
                txtSexoInfo.text = sexo;
                break;
        }

        return retorno;
    }

    public bool validarSenhas()
    {
        if (TxtSenhaCadastro.text == TxtSenhaCadastroConfirm.text)
            return true;

        return false;
    }

    public void BtnAbrirEdicaoAvatar()
    {
        SomController.Tocar(SomController.Som.Click_OK);
        AnimacoesTween.AnimarObjeto(EventSystem.current.currentSelectedGameObject, AnimacoesTween.TiposAnimacoes.Button_Click, () =>
        {
            StartCoroutine(new SceneController().CarregarCenaAdditiveAsync("EdicaoChar"));
        });
    }

    public void AlterarAvatar(string sexo, Cliente.Avatar avatar)
    {
        this.avatar = avatar;
        PnlAvatar.GetComponent<AvatarObj>().PreencherInfo(sexo, avatar);

        if (sexo == "Masculino")
            ChkMasculinoCadastro.isOn = true;
        else
            ChkFemininoCadastro.isOn = true;
    }

    public void btnConfirmarCadastro()
    {
        SomController.Tocar(SomController.Som.Click_OK);
        AnimacoesTween.AnimarObjeto(EventSystem.current.currentSelectedGameObject, AnimacoesTween.TiposAnimacoes.Button_Click, () => StartCoroutine(cadastrar()));
    }

    private IEnumerator cadastrar()
    {
        string senha = Util.GerarHashMd5(TxtSenhaCadastro.text);
        WWWForm form = new WWWForm();
        form.AddField("email", TxtEmailCadastro.text);
        form.AddField("password", senha);
        form.AddField("apelido", TxtApelidoCadastro.text);
        form.AddField("dataNascimento", Util.formatarDataParaAPI(TxtIdadeCadastro.text));
        form.AddField("nome", TxtNomeCadastro.text);
        form.AddField("cpf", TxtCPFCadastro.text);
        form.AddField("sexo", txtSexoInfo.text);
        form.AddField("avatar", JsonConvert.SerializeObject(prepararAvatarCadastro()));

        #region Post Cadastrar Cliente
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
                
                buscarNoFireBase();
                return;
            }

            AppManager.Instance.DesativarLoader();

            SomController.Tocar(SomController.Som.Error);
            //StartCoroutine(comunicadorAPI.Alerta.ChamarAlerta(retornoAPI.msg, comunicadorAPI.PnlPrincipal));
        },
        (error) =>
        {
            AppManager.Instance.DesativarLoader();
            SomController.Tocar(SomController.Som.Error);
            //StartCoroutine(comunicadorAPI.Alerta.ChamarAlerta(error, comunicadorAPI.PnlPrincipal));
        }));
        #endregion

    }

    private async void buscarNoFireBase()
    {
        ClienteFirebase cliente = new ClienteFirebase();
        Cliente.ClienteLogado = await cliente.ObterUsuario(Cliente.Obter());

        AppManager.Instance.DesativarLoader();

        SceneManager.LoadSceneAsync("Main");
    }

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

}
