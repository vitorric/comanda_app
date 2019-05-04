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

public class Main : MonoBehaviour
{
    public GameObject PnlPrincipal;

    public static Main Instance { get; set; }

    [Header("Classes")]
    public MenuEstabelecimento MenuEstabelecimento;
    public PnlDecisao PnlDecisao;

    [Header("InfoUsuario")]
    public Text TxtApelido;
    public Text TxtLevel;
    public Text TxtPctExp;
    public Image ImgBarExp;

    public GameObject PnlUpLevel;

    [Header("Geral")]
    public GameObject PnlPopUp;
    public List<GameObject> LstBotoesMenu;
    public Configuracoes ConfigApp;
    public GameObject PnlNaoEstaNoEstabelecimento;
    public GameObject PnlEstaNoEstabelecimento;

    [Header("Esta No Estabelecimento")]
    public Text TxtNomeEstabComanda;

    void Awake()
    {
        ConfigApp = new Configuracoes();

        if (Instance != null)
            Destroy(this);

        Instance = this;

        iniciarWatchsFirebase();

        preencherInfoUsuario();
    }

    public void BtnAdicionarExp()
    {
        AdicionarExp(Configuracoes.LevelSystem.Acao.CompraItem, 100);
    }

    #region iniciarWatchsFirebase
    private void iniciarWatchsFirebase()
    {
        ClienteFirebase clienteFirebase = new ClienteFirebase();
        clienteFirebase.IniciarWatch();
    }
    #endregion

    #region Manipular Usuario

    private void preencherInfoUsuario()
    {
        TxtApelido.text = Cliente.ClienteLogado.apelido;
        FindObjectOfType<MenuUsuario>().PreencherAvatares();
        Cliente.ClienteLogado.ConfigurarExpProLevel();
        atualizarExp();
        ClienteEstaNoEstabelecimento();
    }

    private void atualizarExp()
    {
        int pctExp = Cliente.ClienteLogado.AtualizarPctExp();

        TxtLevel.text = Cliente.ClienteLogado.avatar.level.ToString();
        TxtPctExp.text = pctExp + "%";
        ImgBarExp.transform.localScale = new Vector3((pctExp / 100f), 1, 1);
    }

    public void AdicionarExp(Configuracoes.LevelSystem.Acao acao, int parametro)
    {
        if (Cliente.ClienteLogado.AdicionarExp(ConfigApp.levelSystem.CalculoExpProAvatar(acao, parametro)))
        {
            GameObject objLevelUp = Instantiate(PnlUpLevel, PnlPrincipal.transform);
            objLevelUp.GetComponent<LevelUpObj>().TxtLevel.GetComponent<Text>().text = Cliente.ClienteLogado.avatar.level.ToString();
        }
        atualizarAvatarNoBanco();
        atualizarExp();
    }

    private void atualizarAvatarNoBanco()
    {
        WWWForm form = new WWWForm();
        form.AddField("_id", Cliente.ClienteLogado._id);
        form.AddField("avatar", JsonConvert.SerializeObject(Cliente.ClienteLogado.avatar));

        StartCoroutine(APIManager.Instance.Post(APIManager.URLs.AvatarAlterar, form,
        (response) =>
        {
            APIManager.Retorno<string> retornoAPI = JsonConvert.DeserializeObject<APIManager.Retorno<string>>(response);

            if (retornoAPI.sucesso)
            {
                //sucesso
            }
        },
        (error) =>
        {
            //TODO: Tratar Error
        }));
    }

    public void ResgatarConquistasUsuario(List<Estabelecimento.Conquista> conquistas, string _idEstabelecimento)
    {
        WWWForm form = new WWWForm();
        form.AddField("_idCliente", Cliente.ClienteLogado._id);
        form.AddField("_idEstabelecimento", _idEstabelecimento);

        StartCoroutine(APIManager.Instance.Post(APIManager.URLs.ListarClienteConquistas, form,
        (response) =>
        {
            APIManager.Retorno<List<Cliente.Conquista>> retornoAPI =
                JsonConvert.DeserializeObject<APIManager.Retorno<List<Cliente.Conquista>>>(response);

            if (retornoAPI.sucesso)
            {
                Main.Instance.MenuEstabelecimento.IniciarConquistas(conquistas, retornoAPI.retorno, _idEstabelecimento);
            }
        },
        (error) =>
        {
            //TODO: Tratar Error
        }));
    }
    #endregion

    #region Manipular Componentes
    public void ManipularMenus(string menuQueVaiAbrir)
    {

        LstBotoesMenu.ForEach(x =>
        {
            if (x.name != menuQueVaiAbrir)
            {
                switch (x.name)
                {
                    //Menus que vao ser fechados automaticamente
                    case "btnPerfil":
                        this.gameObject.GetComponent<MenuUsuario>().BtnAbrirPnlUsuario(true);
                        break;
                    case "btnConfiguracoes":
                        this.gameObject.GetComponent<MenuConfig>().BtnAbrirConfiguracoes(true);
                        break;
                    case "btnComanda":
                        if (Cliente.ClienteLogado.configClienteAtual.estaEmUmEstabelecimento)
                            this.gameObject.GetComponent<MenuComanda>().BtnAbrirMenuComanda(true);
                        break;
                    case "btnEstabelecimentoComanda":
                        if (Cliente.ClienteLogado.configClienteAtual.estaEmUmEstabelecimento)
                            this.gameObject.GetComponent<MenuEstabComanda>().BtnAbrirMenuEstabelecimentoComanda(true);
                        break;
                    default:
                        this.gameObject.GetComponent<MenuConfig>().BtnAbrirConfiguracoes(true);
                        this.gameObject.GetComponent<MenuUsuario>().BtnAbrirPnlUsuario(true);
                        this.gameObject.GetComponent<MenuComanda>().BtnAbrirMenuComanda(true);
                        break;
                }
            }
        });
    }

    public void AbrirMenu(string nomeMenu, bool abrirMenu, List<GameObject> objAnimar, bool fecharAutomatico)
    {
        float tempoAnimacao = 0.1f;
        float valorScala = 1;

        if (abrirMenu)
        {
            if (!fecharAutomatico)
                SomController.Tocar(SomController.Som.Click_OK);
        }
        else
        {
            valorScala = 0;
            tempoAnimacao = objAnimar.Count / 10f;
            if (!fecharAutomatico)
                SomController.Tocar(SomController.Som.Click_Cancel);
        }

        if (!fecharAutomatico)
            ManipularMenus(nomeMenu);

        AnimacoesTween.AnimarObjeto(EventSystem.current.currentSelectedGameObject, AnimacoesTween.TiposAnimacoes.Button_Click, () =>
        {
            for (int i = 0; i < objAnimar.Count; i++)
            {

                AnimacoesTween.AnimarObjeto(objAnimar[i], AnimacoesTween.TiposAnimacoes.Scala, null, tempoAnimacao, new Vector2(valorScala, valorScala));

                tempoAnimacao = (abrirMenu) ? tempoAnimacao + 0.1f : tempoAnimacao - 0.1f;
            }
        });
    }

    #endregion

    #region QRCode
    public void BtnAbrirQRCode()
    {
        SomController.Tocar(SomController.Som.Click_OK);

        AnimacoesTween.AnimarObjeto(EventSystem.current.currentSelectedGameObject, AnimacoesTween.TiposAnimacoes.Button_Click, () =>
        {
            StartCoroutine(permissaoCamera());
            StartCoroutine(new SceneController().CarregarCenaAdditiveAsync("LeitorQRCode"));
        });
    }


    // IEnumerator Start()
    // {
    // 	// When the app start, ask for the authorization to use the webcam
    // 	yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);

    // 	if (!Application.HasUserAuthorization(UserAuthorization.WebCam))
    // 	{
    // 		throw new Exception("This Webcam library can't work without the webcam authorization");
    // 	}
    // }

    private IEnumerator permissaoCamera()
    {
        yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);

        if (!Application.HasUserAuthorization(UserAuthorization.WebCam))
        {
            throw new Exception("This Webcam library can't work without the webcam authorization");
        }
    }
    #endregion

    #region Esta No Estabelecimento
    public void ClienteEstaNoEstabelecimento()
    {

        if (Cliente.ClienteLogado.configClienteAtual.conviteEstabPendente)
        {
            PnlDecisao.AbrirPainelConviteEstab(() =>
            {
                WWWForm form = new WWWForm();
                form.AddField("_idCliente", Cliente.ClienteLogado._id);
                form.AddField("_idEstabelecimento", Cliente.ClienteLogado.configClienteAtual.estabelecimento); //na web

                StartCoroutine(APIManager.Instance.Post(APIManager.URLs.EntrarNoEstabelecimento, form, (response) =>
                {
                    APIManager.Retorno<string> retornoAPI =
                        JsonConvert.DeserializeObject<APIManager.Retorno<string>>(response);

                    if (retornoAPI.sucesso)
                    {
                        ManipularMenus("FecharTodos");
                    }
                    else
                    {
                        SomController.Tocar(SomController.Som.Error);
                    }
                },
                (error) => { }));
            },
            () =>
            {
                WWWForm form = new WWWForm();
                form.AddField("_idCliente", Cliente.ClienteLogado._id);

                StartCoroutine(APIManager.Instance.Post(APIManager.URLs.RecusarConviteEstabelecimento, form, (response) =>
                {
                    APIManager.Retorno<string> retornoAPI =
                        JsonConvert.DeserializeObject<APIManager.Retorno<string>>(response);

                    if (retornoAPI.sucesso)
                    {
                        
                    }
                    else
                    {
                        SomController.Tocar(SomController.Som.Error);
                    }
                },
                (error) => { }));
            },
            Cliente.ClienteLogado.configClienteAtual.nomeEstabelecimento);

            return;
        }

        if (Cliente.ClienteLogado.configClienteAtual.estaEmUmEstabelecimento)
        {
            PnlEstaNoEstabelecimento.SetActive(true);
            PnlNaoEstaNoEstabelecimento.SetActive(false);
            TxtNomeEstabComanda.text = Cliente.ClienteLogado.configClienteAtual.nomeEstabelecimento;
            return;
        }

        //nao esta
        PnlEstaNoEstabelecimento.SetActive(false);
        PnlNaoEstaNoEstabelecimento.SetActive(true);

    }
    #endregion

}
