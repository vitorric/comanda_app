using APIModel;
using Network;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EdicaoChar : MonoBehaviour
{
    [Header("Button")]
    public Button BtnAleatorio;
    public Button BtnFechar;
    public Button BtnFecharCriacao;
    public Button BtnSalvar;

    [Header("Toggle")]
    public Toggle BtnCabeca;
    public Toggle BtnOlhos;
    public Toggle BtnBoca;
    public Toggle BtnNariz;
    public Toggle BtnBarba;
    public Toggle BtnCabeloFrontal;
    public Toggle BtnCabeloTraseiro;
    public Toggle BtnSombrancelhas;
    public Toggle BtnRoupa;

    public Toggle BtnCorPele;
    public Toggle BtnCorBarba;
    public Toggle BtnCorCabelo;

    public Toggle BtnSexoMasc;
    public Toggle BtnSexoFem;

    public Transform ScvEdicaoChar;
    public SlotItemPersonagem SlotItemPers;
    public AvatarObj PnlCharacter;

    public GameObject CorItemRef;

    public GameObject PnlSexo;

    public GameObject BtnBarbaItem;

    public GameObject BtnBarbaCor;

    public GameObject PnlOpcaoPeleCor;
    public GameObject PnlOpcaoCabeloCor;
    public GameObject PnlOpcaoBarbaCor;

    private string _modulo = string.Empty;

    private string cenaAtiva;
    private string sexo;

    private void Awake()
    {
        configurarListener();
    }

    void Start()
    {
        preencherCoresPele();
        carregarDados();

        PnlCharacter.PreencherInfo(sexo, Cliente.ClienteLogado.avatar);

        if (sexo == "Masculino")
            BtnSexoMasc.isOn = true;
        else
            BtnSexoFem.isOn = true;

        carregarItensDoPersonagem((_modulo == string.Empty) ? "cabeca" : _modulo);
    }

    #region configurarListener
    private void configurarListener()
    {
        BtnAleatorio.onClick.AddListener(() => btnAleatorio());
        BtnFechar.onClick.AddListener(() => btnFechar(false));
        BtnFecharCriacao.onClick.AddListener(() => btnFechar(true));
        BtnSalvar.onClick.AddListener(() => btnSalvarAlteracao());

        BtnOlhos.onValueChanged.AddListener((result) => btnTrocarItem("olhos", BtnOlhos.gameObject, true));
        BtnBoca.onValueChanged.AddListener((result) => btnTrocarItem("boca", BtnBoca.gameObject, true));
        BtnNariz.onValueChanged.AddListener((result) => btnTrocarItem("nariz", BtnNariz.gameObject, true));
        BtnBarba.onValueChanged.AddListener((result) => btnTrocarItem("barba", BtnBarba.gameObject, true));
        BtnCabeloFrontal.onValueChanged.AddListener((result) => btnTrocarItem("cabeloFrontal", BtnCabeloFrontal.gameObject, true));
        BtnCabeloTraseiro.onValueChanged.AddListener((result) => btnTrocarItem("cabeloTraseiro", BtnCabeloTraseiro.gameObject, true));
        BtnSombrancelhas.onValueChanged.AddListener((result) => btnTrocarItem("sombrancelhas", BtnSombrancelhas.gameObject, true));
        BtnRoupa.onValueChanged.AddListener((result) => btnTrocarItem("roupa", BtnRoupa.gameObject, true));
        BtnCabeca.onValueChanged.AddListener((result) => btnTrocarItem("cabeca", BtnCabeca.gameObject, true));

        BtnCorPele.onValueChanged.AddListener((result) => btnAlterarCor("pele", true, BtnCorPele.gameObject));
        BtnCorBarba.onValueChanged.AddListener((result) => btnAlterarCor("cabelo", true, BtnCorBarba.gameObject));
        BtnCorCabelo.onValueChanged.AddListener((result) => btnAlterarCor("barba", true, BtnCorCabelo.gameObject));

        BtnSexoMasc.onValueChanged.AddListener((result) => btnAlterarSexo("Masculino", true, BtnSexoMasc.gameObject));
        BtnSexoFem.onValueChanged.AddListener((result) => btnAlterarSexo("Feminino", true, BtnSexoFem.gameObject));
    }
    #endregion

    #region preencherCoresPele
    private void preencherCoresPele()
    {
        CoresUtil corUtil = new CoresUtil();

        //-------------------------- instancia cores da pele
        int index = 0;
        List<CoresUtil.Cores> lstCores = corUtil.CoresAvatar("pele");

        lstCores.ForEach(x =>
        {
            GameObject objItem = Instantiate(CorItemRef, PnlOpcaoPeleCor.transform);
            objItem.transform.SetParent(PnlOpcaoPeleCor.transform);
            objItem.name = x.nome;
            objItem.GetComponent<Button>().onClick.AddListener(() => btnTrocarCor("pele", x.nome, objItem));
            objItem.GetComponent<Image>().color = corUtil.TransformHexToColor(lstCores[index].hexadecimal);

            index++;
        });

        //-------------------------- instancia cores do cabelo
        index = 0;
        lstCores = corUtil.CoresAvatar("cabelo");

        lstCores.ForEach(x =>
        {
            GameObject objItem = Instantiate(CorItemRef, PnlOpcaoCabeloCor.transform);
            objItem.transform.SetParent(PnlOpcaoCabeloCor.transform);
            objItem.name = x.nome;
            objItem.GetComponent<Button>().onClick.AddListener(() => btnTrocarCor("cabelo", x.nome, objItem));
            objItem.GetComponent<Image>().color = corUtil.TransformHexToColor(lstCores[index].hexadecimal);

            index++;
        });

        //-------------------------- instancia cores da barba
        index = 0;
        lstCores = corUtil.CoresAvatar("barba");

        lstCores.ForEach(x =>
        {
            GameObject objItem = Instantiate(CorItemRef, PnlOpcaoBarbaCor.transform);
            objItem.transform.SetParent(PnlOpcaoBarbaCor.transform);
            objItem.name = x.nome;
            objItem.GetComponent<Button>().onClick.AddListener(() => btnTrocarCor("barba", x.nome, objItem));
            objItem.GetComponent<Image>().color = corUtil.TransformHexToColor(lstCores[index].hexadecimal);

            index++;
        });
    }
    #endregion

    #region carregarDados
    private void carregarDados()
    {
        cenaAtiva = SceneController.NomeCenaAtiva();

        if (cenaAtiva == "Login")
        {
            Login login = FindObjectOfType<Login>();
            sexo = login.PnlAvatar.Sexo;

            Cliente.ClienteLogado = new Cliente.Dados
            {
                avatar = login.PnlAvatar.Avatar
            };

            BtnFecharCriacao.gameObject.SetActive(true);
            BtnFechar.gameObject.SetActive(false);
            BtnSalvar.gameObject.SetActive(false);

            return;
        }

        if (cenaAtiva == "Main")
        {

            PnlSexo.SetActive(false);
            sexo = Cliente.ClienteLogado.sexo;

            BtnFecharCriacao.gameObject.SetActive(false);
            BtnFechar.gameObject.SetActive(true);
            BtnSalvar.gameObject.SetActive(true);

            BtnBarbaItem.SetActive((sexo == "Masculino") ? true : false);
            BtnBarbaCor.SetActive((sexo == "Masculino") ? true : false);
            return;
        }
    }
    #endregion

    #region descarregaDados
    private void descarregaDados()
    {
        if (cenaAtiva == "Login")
        {
            Cliente.ClienteLogado.avatar = PnlCharacter.Avatar;

            FindObjectOfType<Login>().AlterarAvatar(sexo, Cliente.ClienteLogado.avatar);
            return;
        }

        if (cenaAtiva == "Main")
        {
            FindObjectOfType<MenuUsuario>().PreencherAvatares();
            return;
        }
    }
    #endregion

    #region btnAlterarSexo
    private void btnAlterarSexo(string sexo, bool tocarSom = false, GameObject objClicado = null)
    {
        BtnBarbaItem.SetActive((sexo == "Masculino") ? true : false);
        BtnBarbaCor.SetActive((sexo == "Masculino") ? true : false);

        if (EventSystem.current.currentSelectedGameObject == objClicado)
        {
            if (tocarSom)
                EasyAudioUtility.Instance.Play(EasyAudioUtility.Som.Click_OK);

            this.sexo = sexo;
            BtnCorPele.isOn = true;
            PnlCharacter.PreencherInfo(sexo, PnlCharacter.Avatar);

            carregarItensDoPersonagem(_modulo, true);
        }
    }
    #endregion

    #region btnAlterarCor
    private void btnAlterarCor(string opcao, bool tocarSom = false, GameObject objClicado = null)
    {
        if (EventSystem.current.currentSelectedGameObject == objClicado)
        {
            if (tocarSom)
                EasyAudioUtility.Instance.Play(EasyAudioUtility.Som.Click_OK);

            PnlOpcaoPeleCor.SetActive((opcao == "pele") ? true : false);
            PnlOpcaoCabeloCor.SetActive((opcao == "cabelo") ? true : false);
            PnlOpcaoBarbaCor.SetActive((opcao == "barba" && sexo == "Masculino") ? true : false);
        }
    }
    #endregion

    #region btnTrocarCor
    private void btnTrocarCor(string modulo, string nomeCor, GameObject objClicado)
    {
        if (EventSystem.current.currentSelectedGameObject == objClicado)
        {
            EasyAudioUtility.Instance.Play(EasyAudioUtility.Som.Click_OK);

            PnlCharacter.TrocarCor(objClicado.GetComponent<Image>().color, nomeCor, modulo);
        }
    }
    #endregion

    #region btnTrocarItem
    private void btnTrocarItem(string modulo, GameObject objClicado, bool tocarSom = false)
    {
        if (EventSystem.current.currentSelectedGameObject == objClicado)
        {
            if (tocarSom)
                EasyAudioUtility.Instance.Play(EasyAudioUtility.Som.Click_OK);

            carregarItensDoPersonagem(modulo);
        }
    }
    #endregion

    #region btnAleatorio
    private void btnAleatorio()
    {
        EasyAudioUtility.Instance.Play(EasyAudioUtility.Som.Click_OK);

        PnlCharacter.PreencherInfo(sexo, null);

        carregarItensDoPersonagem((_modulo == string.Empty) ? "cabeca" : _modulo);
    }
    #endregion

    #region btnSalvarAlteracao
    private void btnSalvarAlteracao()
    {

        Dictionary<string, string> data = new Dictionary<string, string>
        {
            {   "corpo", PnlCharacter.Avatar.corpo  },
            {   "cabeca", PnlCharacter.Avatar.cabeca },
            {   "nariz", PnlCharacter.Avatar.nariz  },
            {   "olhos", PnlCharacter.Avatar.olhos  },
            {   "boca", PnlCharacter.Avatar.boca },
            {   "roupa", PnlCharacter.Avatar.roupa },
            {   "cabeloTraseiro", PnlCharacter.Avatar.cabeloTraseiro },
            {   "cabeloFrontal", PnlCharacter.Avatar.cabeloFrontal },
            {   "barba", PnlCharacter.Avatar.barba },
            {   "sombrancelhas", PnlCharacter.Avatar.sombrancelhas },
            {   "orelha", PnlCharacter.Avatar.orelha },
            {   "corPele", PnlCharacter.Avatar.corPele },
            {   "corCabelo", PnlCharacter.Avatar.corCabelo },
            {   "corBarba", PnlCharacter.Avatar.corBarba }
        };

        StartCoroutine(AvatarAPI.AvatarAlterar(data,
        (response, error) =>
        {

            if (error != null)
            {
                Debug.Log(error);

                StartCoroutine(AlertaManager.Instance.ChamarAlertaMensagem(error, false));
                return;
            }

            EasyAudioUtility.Instance.Play(EasyAudioUtility.Som.Success);

            AlertaManager.Instance.IniciarAlerta(response);

            PnlCharacter.Avatar.level = Cliente.ClienteLogado.avatar.level;
            PnlCharacter.Avatar.exp = Cliente.ClienteLogado.avatar.exp;
            PnlCharacter.Avatar.expProximoLevel = Cliente.ClienteLogado.avatar.expProximoLevel;

            Cliente.ClienteLogado.avatar = PnlCharacter.Avatar;

            fecharCena(true);

        }));
    }
    #endregion

    #region btnfechar
    private void btnFechar(bool estaAlterando)
    {
        //Debug.Log(avatar.barba);
        EasyAudioUtility.Instance.Play(EasyAudioUtility.Som.Click_Cancel);

        fecharCena(estaAlterando);
    }
    #endregion

    #region fecharCena
    private void fecharCena(bool estaAlterando)
    {
        if (estaAlterando)
            descarregaDados();

        SceneManager.UnloadSceneAsync("EdicaoChar");
    }
    #endregion

    #region carregarItensDoPersonagem
    private void carregarItensDoPersonagem(string modulo, bool mudouSexo = false)
    {
        string nomeTexturaModuloSelecionado = PnlCharacter.NomeTexturaModuloSelecionado(modulo);
        Debug.Log(nomeTexturaModuloSelecionado);

        FindObjectsOfType<SlotItemPersonagem>().ToList().ForEach(x => x.gameObject.GetComponent<Outline>().enabled = false);

        if (_modulo != modulo || mudouSexo)
        {
            _modulo = modulo;
            ScvEdicaoChar.GetComponentsInChildren<SlotItemPersonagem>().ToList().ForEach(x => Destroy(x.gameObject));

            List<Texture2D> lstImages = Resources.LoadAll<Texture2D>("Character/" + sexo + "/" + _modulo + "/Habilitado").ToList();

            if (_modulo == "cabeloFrontal" || _modulo == "cabeloTraseiro" || _modulo == "barba")
            {
                SlotItemPersonagem objItem = Instantiate(SlotItemPers, ScvEdicaoChar.transform);
                objItem.name = _modulo + "vazio";

                objItem.PreencherInfo(null, modulo, PnlCharacter);
            }

            foreach (Texture2D img in lstImages)
            {
                SlotItemPersonagem objItem = Instantiate(SlotItemPers, ScvEdicaoChar.transform);
                objItem.name = sexo + "_" + img.name;

                objItem.PreencherInfo(img, modulo, PnlCharacter);
            }
        }


        GameObject.Find(nomeTexturaModuloSelecionado.Contains("vazio") ? nomeTexturaModuloSelecionado : sexo + "_" + nomeTexturaModuloSelecionado).GetComponent<Outline>().enabled = true;
    }
    #endregion

}
