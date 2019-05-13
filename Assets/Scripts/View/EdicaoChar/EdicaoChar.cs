using APIModel;
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
    private Cliente.Avatar avatar;

    private void Awake()
    {
        configurarListener();
    }

    void Start()
    {
        preencherCoresPele();
        carregarDados();

        PnlCharacter.PreencherInfo(sexo, avatar);

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
        BtnFechar.onClick.AddListener(() => btnFechar());

        BtnCabeca.onValueChanged.AddListener((result) => btnTrocarItem("cabeca", BtnCabeca.gameObject));
        BtnOlhos.onValueChanged.AddListener((result) => btnTrocarItem("olhos", BtnOlhos.gameObject));
        BtnBoca.onValueChanged.AddListener((result) => btnTrocarItem("boca", BtnBoca.gameObject));
        BtnNariz.onValueChanged.AddListener((result) => btnTrocarItem("nariz", BtnNariz.gameObject));
        BtnBarba.onValueChanged.AddListener((result) => btnTrocarItem("barba", BtnBarba.gameObject));
        BtnCabeloFrontal.onValueChanged.AddListener((result) => btnTrocarItem("cabeloFrontal", BtnCabeloFrontal.gameObject));
        BtnCabeloTraseiro.onValueChanged.AddListener((result) => btnTrocarItem("cabeloTraseiro", BtnCabeloTraseiro.gameObject));
        BtnSombrancelhas.onValueChanged.AddListener((result) => btnTrocarItem("sombrancelhas", BtnSombrancelhas.gameObject));
        BtnRoupa.onValueChanged.AddListener((result) => btnTrocarItem("roupa", BtnRoupa.gameObject));

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
            sexo = FindObjectOfType<Login>().PnlAvatar.Sexo;
            avatar = FindObjectOfType<Login>().PnlAvatar.Avatar;

            return;
        }

        if (cenaAtiva == "Main")
        {
            PnlSexo.SetActive(false);
            Cliente.Avatar novoAvatar = FindObjectOfType<MenuUsuario>().AvatarEditado;
            sexo = Cliente.ClienteLogado.sexo;
            avatar = novoAvatar ?? Cliente.ClienteLogado.avatar;
            return;
        }
    }
    #endregion

    #region descarregaDados
    private void descarregaDados()
    {
        this.avatar = PnlCharacter.Avatar;

        if (cenaAtiva == "Login")
        {
            FindObjectOfType<Login>().AlterarAvatar(sexo, avatar);
            return;
        }

        if (cenaAtiva == "Main")
        {
            FindObjectOfType<MenuUsuario>().AlterarAvatar(avatar);
            return;
        }
    }
    #endregion

    #region btnAlterarSexo
    private void btnAlterarSexo(string sexo, bool tocarSom = false, GameObject objClicado = null)
    {
        if (tocarSom)
            EasyAudioUtility.Instance.Play(EasyAudioUtility.Som.Click_OK);

        AnimacoesTween.AnimarObjeto(objClicado,
            AnimacoesTween.TiposAnimacoes.Button_Click, () =>
            {
                this.sexo = sexo;
                BtnCorPele.isOn = true;
                BtnBarbaItem.SetActive((sexo == "Masculino") ? true : false);
                BtnBarbaCor.SetActive((sexo == "Masculino") ? true : false);
                PnlCharacter.PreencherInfo(sexo, avatar);
                carregarItensDoPersonagem(_modulo, true);
            },
            AppManager.TEMPO_ANIMACAO_ABRIR_CLICK_BOTAO);
    }
    #endregion

    #region btnAlterarCor
    private void btnAlterarCor(string opcao, bool tocarSom = false, GameObject objClicado = null)
    {
        if (tocarSom)
            EasyAudioUtility.Instance.Play(EasyAudioUtility.Som.Click_OK);

        AnimacoesTween.AnimarObjeto(objClicado, AnimacoesTween.TiposAnimacoes.Button_Click, () =>
        {
            PnlOpcaoPeleCor.SetActive((opcao == "pele") ? true : false);
            PnlOpcaoCabeloCor.SetActive((opcao == "cabelo") ? true : false);
            PnlOpcaoBarbaCor.SetActive((opcao == "barba" && sexo == "Masculino") ? true : false);
        },
        AppManager.TEMPO_ANIMACAO_ABRIR_CLICK_BOTAO);
    }
    #endregion

    #region btnTrocarCor
    private void btnTrocarCor(string modulo, string nomeCor, GameObject objClicado)
    {
        EasyAudioUtility.Instance.Play(EasyAudioUtility.Som.Click_OK);

        AnimacoesTween.AnimarObjeto(objClicado, AnimacoesTween.TiposAnimacoes.SubMenu_Click, () =>
        {
            PnlCharacter.TrocarCor(objClicado.GetComponent<Image>().color, nomeCor, modulo);
        },
        AppManager.TEMPO_ANIMACAO_ABRIR_CLICK_BOTAO);
    }
    #endregion

    #region btnTrocarItem
    private void btnTrocarItem(string modulo, GameObject objClicado)
    {
        EasyAudioUtility.Instance.Play(EasyAudioUtility.Som.Click_OK);

        AnimacoesTween.AnimarObjeto(objClicado.gameObject, AnimacoesTween.TiposAnimacoes.Button_Click, () =>
        {
            carregarItensDoPersonagem(modulo);
        },
        AppManager.TEMPO_ANIMACAO_ABRIR_CLICK_BOTAO);
    }
    #endregion

    #region btnAleatorio
    private void btnAleatorio()
    {
        EasyAudioUtility.Instance.Play(EasyAudioUtility.Som.Click_OK);

        AnimacoesTween.AnimarObjeto(BtnAleatorio.gameObject, AnimacoesTween.TiposAnimacoes.Button_Click, () =>
        {
            PnlCharacter.PreencherInfo(sexo, null);
            avatar = PnlCharacter.Avatar;
            carregarItensDoPersonagem((_modulo == string.Empty) ? "cabeca" : _modulo);
        },
        AppManager.TEMPO_ANIMACAO_ABRIR_CLICK_BOTAO);
    }
    #endregion

    #region btnfechar
    private void btnFechar()
    {
        EasyAudioUtility.Instance.Play(EasyAudioUtility.Som.Click_Cancel);

        AnimacoesTween.AnimarObjeto(BtnFechar.gameObject, AnimacoesTween.TiposAnimacoes.Button_Click, () =>
        {
            descarregaDados();
            SceneManager.UnloadSceneAsync("EdicaoChar");
        },
        AppManager.TEMPO_ANIMACAO_ABRIR_CLICK_BOTAO);
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

        GameObject.Find(sexo + "_" + nomeTexturaModuloSelecionado).GetComponent<Outline>().enabled = true;
    }
    #endregion

}
