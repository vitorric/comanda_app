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

    [Header("Button Aba Config")]
    public ButtonControl buttonControlItens;
    public ButtonControl buttonControlCores;
    public ButtonControl buttonControlSexo;

    [Header("Button")]
    public Button BtnAleatorio;
    public Button BtnFechar;
    public Button BtnFecharCriacao;
    public Button BtnSalvar;

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

    private string _modulo = "cabeca";

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
            btnAlterarSexo(0, "Masculino", false);
        else
            btnAlterarSexo(1, "Feminino", false);


        carregarItensDoPersonagem(_modulo);
    }

    #region configurarListener
    private void configurarListener()
    {
        BtnAleatorio.onClick.AddListener(() => btnAleatorio());
        BtnFechar.onClick.AddListener(() => btnFechar(false));
        BtnFecharCriacao.onClick.AddListener(() => btnFechar(true));
        BtnSalvar.onClick.AddListener(() => btnSalvarAlteracao());


        buttonControlItens.BtnAbas[0].onClick.AddListener(() => btnTrocarItem(0, "cabeca", true));
        buttonControlItens.BtnAbas[1].onClick.AddListener(() => btnTrocarItem(1, "olhos", true));
        buttonControlItens.BtnAbas[2].onClick.AddListener(() => btnTrocarItem(2, "boca", true));
        buttonControlItens.BtnAbas[3].onClick.AddListener(() => btnTrocarItem(3, "nariz", true));
        buttonControlItens.BtnAbas[4].onClick.AddListener(() => btnTrocarItem(4, "barba", true));
        buttonControlItens.BtnAbas[5].onClick.AddListener(() => btnTrocarItem(5, "cabeloFrontal", true));
        buttonControlItens.BtnAbas[6].onClick.AddListener(() => btnTrocarItem(6, "cabeloTraseiro", true));
        buttonControlItens.BtnAbas[7].onClick.AddListener(() => btnTrocarItem(7, "sombrancelhas", true));
        buttonControlItens.BtnAbas[8].onClick.AddListener(() => btnTrocarItem(8, "roupa", true));


        buttonControlCores.BtnAbas[0].onClick.AddListener(() => btnAlterarCor(0, "pele", true));
        buttonControlCores.BtnAbas[1].onClick.AddListener(() => btnAlterarCor(1, "cabelo", true));
        buttonControlCores.BtnAbas[2].onClick.AddListener(() => btnAlterarCor(2, "barba", true));

        buttonControlSexo.BtnAbas[0].onClick.AddListener(() => btnAlterarSexo(0, "Masculino", true));
        buttonControlSexo.BtnAbas[1].onClick.AddListener(() => btnAlterarSexo(1, "Feminino", true));
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
    private void btnAlterarSexo(int numeroAba, string sexo, bool tocarSom = false)
    {
        BtnBarbaItem.SetActive((sexo == "Masculino") ? true : false);
        BtnBarbaCor.SetActive((sexo == "Masculino") ? true : false);

        if (tocarSom)
            EasyAudioUtility.Instance.Play(EasyAudioUtility.Som.Click_OK);

        this.sexo = sexo;
        btnAlterarCor(0, "pele", false);
        PnlCharacter.PreencherInfo(sexo, PnlCharacter.Avatar);

        carregarItensDoPersonagem(_modulo, true);

        buttonControlSexo.TrocarAba(numeroAba);
    }
    #endregion

    #region btnAlterarCor
    private void btnAlterarCor(int numeroAba, string opcao, bool tocarSom = false)
    {

        if (tocarSom)
            EasyAudioUtility.Instance.Play(EasyAudioUtility.Som.Click_OK);

        PnlOpcaoPeleCor.SetActive((opcao == "pele") ? true : false);
        PnlOpcaoCabeloCor.SetActive((opcao == "cabelo") ? true : false);
        PnlOpcaoBarbaCor.SetActive((opcao == "barba" && sexo == "Masculino") ? true : false);

        buttonControlCores.TrocarAba(numeroAba);
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
    private void btnTrocarItem(int numeroAba, string modulo, bool tocarSom = false)
    {
        if (tocarSom)
            EasyAudioUtility.Instance.Play(EasyAudioUtility.Som.Click_OK);

        carregarItensDoPersonagem(modulo);
        buttonControlItens.TrocarAba(numeroAba);
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
        Dictionary<string, object> data = new Dictionary<string, object>
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

        StartCoroutine(AvatarAPI.AlterarAvatar(data,
        (response, error) =>
        {

            if (error != null)
            {
                Debug.Log(JsonConvert.SerializeObject(response));
                Debug.Log("btnSalvarAlteracao" + error);
                AlertaManager.Instance.ChamarAlertaMensagem(error, false);
                return;
            }

            EasyAudioUtility.Instance.Play(EasyAudioUtility.Som.Success);

            AlertaManager.Instance.IniciarAlerta(response);

            Debug.Log(JsonConvert.SerializeObject(PnlCharacter.Avatar));

            PnlCharacter.Avatar.info.level = Cliente.ClienteLogado.avatar.info.level;
            PnlCharacter.Avatar.info.exp = Cliente.ClienteLogado.avatar.info.exp;
            PnlCharacter.Avatar.info.expProximoLevel = Cliente.ClienteLogado.avatar.info.expProximoLevel;

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
