using System;
using System.Collections;
using System.Collections.Generic;
using APIModel;
using FirebaseModel;
using Network;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Main : MonoBehaviour
{

    public static Main Instance { get; set; }

    [Header("Botoes")]
    public Button BtnQrCode;

    [Header("Objetos de referencia")]
    public GameObject PnlPrincipal;
    public GameObject ObjScript;

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

    void Awake()
    {
        ConfigApp = new Configuracoes();

        if (Instance != null)
            Destroy(this);

        Instance = this;

        instanciarListener();

        iniciarWatchsFirebase();

        preencherInfoUsuario();
    }

    private void instanciarListener()
    {
        BtnQrCode.onClick.AddListener(() => btnAbrirQRCode());
    }

    public void BtnAdicionarExp()
    {
        AdicionarExp(Configuracoes.LevelSystem.Acao.CompraItem, 100);
    }

    #region iniciarWatchsFirebase
    private void iniciarWatchsFirebase()
    {
        ClienteFirebase clienteFirebase = new ClienteFirebase();
        clienteFirebase.IniciarWatch(Cliente.ClienteLogado._id);
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
        Dictionary<string, string> form = new Dictionary<string, string>();
        form.Add("_id", Cliente.ClienteLogado._id);
        form.Add("avatar", JsonConvert.SerializeObject(Cliente.ClienteLogado.avatar));

        StartCoroutine(AvatarAPI.AvatarAlterar(form,
        (response, error) =>
        {

            if (response)
            {
                //sucesso
            }
        }));
    }

    //public void ResgatarConquistasUsuario(List<Estabelecimento.Conquista> conquistas, string _idEstabelecimento)
    //{
    //    Dictionary<string, string> form = new Dictionary<string, string>
    //    {
    //        { "_idCliente", Cliente.ClienteLogado._id },
    //        { "_idEstabelecimento", _idEstabelecimento }
    //    };

    //    //StartCoroutine(ClienteAPI.ListarClienteConquistas(form,
    //    //(response, error) =>
    //    //{
    //    //    Instance.MenuEstabelecimento.IniciarConquistas(conquistas, response, _idEstabelecimento);
    //    //}));
    //}
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
                    case "BtnPerfil":
                        ObjScript.GetComponentInChildren<MenuUsuario>().BtnAbrirMenu(true);
                        break;
                    case "BtnConfiguracoes":
                        ObjScript.GetComponentInChildren<MenuConfig>().BtnAbrirConfiguracoes(true);
                        break;
                    case "BtnComanda":
                        if (Cliente.ClienteLogado.configClienteAtual.estaEmUmEstabelecimento)
                            ObjScript.GetComponentInChildren<MenuComanda>().BtnAbrirMenuComanda(true);
                        break;
                    case "BtnEstabelecimentoComanda":
                        if (Cliente.ClienteLogado.configClienteAtual.estaEmUmEstabelecimento)
                            ObjScript.GetComponentInChildren<MenuEstabComanda>().BtnAbrirMenuEstabelecimentoComanda(true);
                        break;
                    default:
                        Debug.Log(x.name);
                        ObjScript.GetComponentInChildren<MenuConfig>().BtnAbrirConfiguracoes(true);
                        ObjScript.GetComponentInChildren<MenuUsuario>().BtnAbrirMenu(true);
                        ObjScript.GetComponentInChildren<MenuComanda>().BtnAbrirMenuComanda(true);
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
                EasyAudioUtility.Instance.Play(EasyAudioUtility.Som.Click_OK);
        }
        else
        {
            valorScala = 0;
            tempoAnimacao = objAnimar.Count / 10f;
            if (!fecharAutomatico)
                EasyAudioUtility.Instance.Play(EasyAudioUtility.Som.Click_Cancel);
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

    #region btnAbrirQRCode
    private void btnAbrirQRCode()
    {
        EasyAudioUtility.Instance.Play(EasyAudioUtility.Som.Click_OK);

        AnimacoesTween.AnimarObjeto(EventSystem.current.currentSelectedGameObject, AnimacoesTween.TiposAnimacoes.Button_Click, () =>
        {
            StartCoroutine(permissaoCamera());
            SceneManager.LoadSceneAsync("LeitorQRCode", LoadSceneMode.Additive);
        },
        AppManager.TEMPO_ANIMACAO_ABRIR_CLICK_BOTAO);
    }

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
                Dictionary<string, string> form = new Dictionary<string, string>
                {
                    { "_idCliente", Cliente.ClienteLogado._id },
                    { "_idEstabelecimento", Cliente.ClienteLogado.configClienteAtual.estabelecimento } //na web
                };

                StartCoroutine(ClienteAPI.EntrarNoEstabelecimento(form,
                (response, error) =>
                {
                    APIManager.Retorno<string> retornoAPI =
                        JsonConvert.DeserializeObject<APIManager.Retorno<string>>(response);

                    if (retornoAPI.sucesso)
                    {
                        ManipularMenus("FecharTodos");
                    }
                    else
                    {
                        EasyAudioUtility.Instance.Play(EasyAudioUtility.Som.Error);
                    }
                }));
            },
            () =>
            {
                Dictionary<string, string> form = new Dictionary<string, string>
                {
                    { "_idCliente", Cliente.ClienteLogado._id }
                };

                StartCoroutine(ClienteAPI.RecusarConviteEstabelecimento(form, (response, error) =>
                {
                    APIManager.Retorno<string> retornoAPI =
                        JsonConvert.DeserializeObject<APIManager.Retorno<string>>(response);

                    if (retornoAPI.sucesso)
                    {

                    }
                    else
                    {
                        EasyAudioUtility.Instance.Play(EasyAudioUtility.Som.Error);
                    }
                }));
            },
            Cliente.ClienteLogado.configClienteAtual.nomeEstabelecimento);

            return;
        }

        if (Cliente.ClienteLogado.configClienteAtual.estaEmUmEstabelecimento)
        {
            PnlEstaNoEstabelecimento.SetActive(true);
            PnlNaoEstaNoEstabelecimento.SetActive(false);
            return;
        }

        //nao esta
        PnlEstaNoEstabelecimento.SetActive(false);
        PnlNaoEstaNoEstabelecimento.SetActive(true);

    }
    #endregion

}
