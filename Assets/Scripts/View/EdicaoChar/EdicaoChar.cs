using APIModel;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EdicaoChar : MonoBehaviour
{
    public GameObject ScvEdicaoChar;
    public GameObject ScvEdicaoCharContent;
    public GameObject ImgItensRef;
    public GameObject PnlCharacter;

    public GameObject CorItemRef;

    public GameObject PnlSexo;
    public Toggle ChkMasculino;
    public Toggle ChkFeminino;

    public GameObject BtnBarbaItem;

    public GameObject BtnBarbaCor;

    public GameObject PnlOpcaoPeleCor;
    public GameObject PnlOpcaoCabeloCor;
    public GameObject PnlOpcaoBarbaCor;
    public Toggle ChkCorPele;

    private string _modulo = string.Empty;

    private string cenaAtiva;
    private string sexo;
    private Cliente.Avatar avatar;

    void Start()
    {
        preencherCoresPele();
        carregarDados();

        PnlCharacter.GetComponent<AvatarObj>().PreencherInfo(sexo, avatar);

        if (sexo == "Masculino")
            ChkMasculino.isOn = true;
        else
            ChkFeminino.isOn = true;

        carregarItensDoPersonagem((_modulo == string.Empty) ? "cabeca" : _modulo);
    }

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
            objItem.GetComponent<Button>().onClick.AddListener(() => BtnTrocarCor("pele", x.nome));
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
            objItem.GetComponent<Button>().onClick.AddListener(() => BtnTrocarCor("cabelo", x.nome));
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
            objItem.GetComponent<Button>().onClick.AddListener(() => BtnTrocarCor("barba", x.nome));
            objItem.GetComponent<Image>().color = corUtil.TransformHexToColor(lstCores[index].hexadecimal);

            index++;
        });
    }

    private void carregarDados()
    {
        cenaAtiva = SceneController.NomeCenaAtiva();

        if (cenaAtiva == "Login")
        {
            sexo = FindObjectOfType<Login>().PnlAvatar.GetComponent<AvatarObj>().Sexo;
            avatar = FindObjectOfType<Login>().PnlAvatar.GetComponent<AvatarObj>().Avatar;

            return;
        }

        if (cenaAtiva == "Main")
        {
            PnlSexo.SetActive(false);
            Cliente.Avatar novoAvatar = FindObjectOfType<MenuUsuario>().AvatarEditado;
            sexo = Cliente.ClienteLogado.sexo;
            avatar = (novoAvatar != null) ? novoAvatar : Cliente.ClienteLogado.avatar;
            return;
        }
    }

    private void descarregaDados()
    {
        this.avatar = PnlCharacter.GetComponent<AvatarObj>().Avatar;

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

    public void ChkAlterarSexo(string sexo)
    {
        if (EventSystem.current.currentSelectedGameObject.name.Contains("Sexo"))
            SomController.Tocar(SomController.Som.Click_OK);

        AnimacoesTween.AnimarObjeto(EventSystem.current.currentSelectedGameObject, AnimacoesTween.TiposAnimacoes.Button_Click, () =>
        {
            this.sexo = sexo;
            ChkCorPele.isOn = true;
            BtnBarbaItem.SetActive((sexo == "Masculino") ? true : false);
            BtnBarbaCor.SetActive((sexo == "Masculino") ? true : false);
            PnlCharacter.GetComponent<AvatarObj>().PreencherInfo(sexo, avatar);
            carregarItensDoPersonagem(_modulo, true);
        });
    }

    public void ChkAlterarCor(string opcao)
    {
        if (EventSystem.current.currentSelectedGameObject.name.Contains("Cor"))
            SomController.Tocar(SomController.Som.Click_OK);

        AnimacoesTween.AnimarObjeto(EventSystem.current.currentSelectedGameObject, AnimacoesTween.TiposAnimacoes.Button_Click, () =>
        {
            PnlOpcaoPeleCor.SetActive((opcao == "pele") ? true : false);
            PnlOpcaoCabeloCor.SetActive((opcao == "cabelo") ? true : false);
            PnlOpcaoBarbaCor.SetActive((opcao == "barba" && sexo == "Masculino") ? true : false);
        });
    }

    public void BtnTrocarCor(string modulo, string nomeCor)
    {
        SomController.Tocar(SomController.Som.Click_OK);
        AnimacoesTween.AnimarObjeto(EventSystem.current.currentSelectedGameObject, AnimacoesTween.TiposAnimacoes.SubMenu_Click, () =>
        {
            PnlCharacter.GetComponent<AvatarObj>().TrocarCor(EventSystem.current.currentSelectedGameObject.GetComponent<Image>().color, nomeCor, modulo);
        });
    }

    public void BtnCarregarImagem(string modulo)
    {
        SomController.Tocar(SomController.Som.Click_OK);
        AnimacoesTween.AnimarObjeto(EventSystem.current.currentSelectedGameObject, AnimacoesTween.TiposAnimacoes.Button_Click, () =>
        {
            carregarItensDoPersonagem(modulo);
        });
    }

    private void carregarItensDoPersonagem(string modulo, bool mudouSexo = false)
    {
        string nomeTexturaModuloSelecionado = FindObjectOfType<AvatarObj>().NomeTexturaModuloSelecionado(modulo);

        FindObjectsOfType<SlotItemPersonagem>().ToList().ForEach(x => x.gameObject.GetComponent<Outline>().enabled = false);

        if (_modulo != modulo || mudouSexo)
        {
            _modulo = modulo;
            ScvEdicaoCharContent.GetComponentsInChildren<SlotItemPersonagem>().ToList().ForEach(x => Destroy(x.gameObject));

            List<Texture2D> lstImages = Resources.LoadAll<Texture2D>("Character/" + sexo + "/" + _modulo + "/Habilitado").ToList();

            if (_modulo == "cabeloFrontal" || _modulo == "cabeloTraseiro" || _modulo == "barba")
            {
                GameObject objItem = Instantiate(ImgItensRef, ScvEdicaoChar.transform);
                objItem.transform.SetParent(ScvEdicaoCharContent.transform);
                objItem.name = _modulo + "vazio";

                objItem.GetComponent<SlotItemPersonagem>().PreencherInfo(null, modulo, PnlCharacter);
            }

            foreach (Texture2D img in lstImages)
            {
                GameObject objItem = Instantiate(ImgItensRef, ScvEdicaoChar.transform);
                objItem.transform.SetParent(ScvEdicaoCharContent.transform);
                objItem.name = sexo + "_" + img.name;

                objItem.GetComponent<SlotItemPersonagem>().PreencherInfo(img, modulo, PnlCharacter);
            }
        }

        GameObject.Find(sexo + "_" + nomeTexturaModuloSelecionado).GetComponent<Outline>().enabled = true;
    }

    public void BtnAleatorio()
    {
        SomController.Tocar(SomController.Som.Click_Cancel);
        AnimacoesTween.AnimarObjeto(EventSystem.current.currentSelectedGameObject, AnimacoesTween.TiposAnimacoes.Button_Click, () =>
        {
            PnlCharacter.GetComponent<AvatarObj>().PreencherInfo(sexo, null);
            avatar = PnlCharacter.GetComponent<AvatarObj>().Avatar;
            carregarItensDoPersonagem((_modulo == string.Empty) ? "cabeca" : _modulo);
        });
    }

    public void BtnFechar()
    {
        SomController.Tocar(SomController.Som.Click_Cancel);
        AnimacoesTween.AnimarObjeto(EventSystem.current.currentSelectedGameObject, AnimacoesTween.TiposAnimacoes.Button_Click, () =>
        {
            descarregaDados();
            StartCoroutine(new SceneController().DescarregarCenaAdditive("EdicaoChar"));
        });


    }
}
