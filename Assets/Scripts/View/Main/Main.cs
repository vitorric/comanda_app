using System;
using System.Collections;
using System.Collections.Generic;
using APIModel;
using FirebaseModel;
using Network;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.Events;
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
    public Slider SliderExp;

    public GameObject PnlUpLevel;

    [Header("Geral")]
    public Configuracoes ConfigApp;
    public GameObject PnlNaoEstaNoEstabelecimento;
    public GameObject PnlEstaNoEstabelecimento;

    public MenuComanda MenuComanda;
    public MenuCorreio MenuCorreio;
    public MenuDesafios MenuDesafio;

    public UnityEvent PreencherAvatares;

    private bool estabelecimentosCarregados = false;

    //private GenericFirebase<string> comandaClienteFirebase;
    private GenericFirebase<Cliente.ConfigClienteAtual> clienteConfigAtualFirebase;
    private GenericFirebase<Cliente.AvatarInfo> clienteAvatarInfoFirebase;
    private ClienteFirebase clieteGoldFirebase;

    void Awake()
    {

        ConfigApp = new Configuracoes();

        if (Instance != null)
            Destroy(this);

        Instance = this;

        instanciarListener();

        iniciarWatchsFirebase();

        preencherInfoUsuario();

        AppManager.Instance.DesativarLoaderAsync();
    }

    private void instanciarListener()
    {
        BtnQrCode.onClick.AddListener(() => btnAbrirQRCode());
    }

    #region PararWatch
    public void PararWatch()
    {
        //comandaClienteFirebase.Watch(false);
        clienteConfigAtualFirebase.Watch(false);
        clienteAvatarInfoFirebase.Watch(false);
        clieteGoldFirebase.WatchGoldPorEstab(Cliente.ClienteLogado._id, false);
        MenuComanda.IniciarWatchComanda(false);
        MenuCorreio.PararWatch();
        MenuEstabelecimento.PararWatch();
        MenuDesafio.PararWatch();
    }
    #endregion

    #region iniciarWatchsFirebase
    private void iniciarWatchsFirebase()
    {
        //inicia o watch na comanda do fire
        //comandaClienteFirebase = new GenericFirebase<string>($"clientes/{Cliente.ClienteLogado._id}/configClienteAtual/comanda")
        //{
        //    Callback = (data) =>
        //    {
        //        Cliente.ClienteLogado.configClienteAtual.comanda = data;
        //        Debug.Log("iniciarWatchsFirebase");
        //        bool temComanda = (string.IsNullOrEmpty(Cliente.ClienteLogado.configClienteAtual.comanda)) ? false : true;
        //        MenuComanda.IniciarWatchComanda(temComanda);
        //    }
        //}; 

        clienteConfigAtualFirebase = new GenericFirebase<Cliente.ConfigClienteAtual>($"clientes/{Cliente.ClienteLogado._id}/configClienteAtual")
        {
            Callback = (data) =>
            {
                if (data != null)
                {
                    Cliente.ClienteLogado.configClienteAtual = data;

                    ClienteEstaNoEstabelecimento();
                }
            }
        };

        clienteAvatarInfoFirebase = new GenericFirebase<Cliente.AvatarInfo>($"clientes/{Cliente.ClienteLogado._id}/avatar/info")
        {
            Callback = (data) =>
            {
                bool upouLevel = data.level > Cliente.ClienteLogado.avatar.info.level;

                Cliente.ClienteLogado.avatar.info = data;
                if (upouLevel)
                {
                    AppManager.Instance.AtivarLevelUp();
                }

                atualizarExp();
            }
        };

        clieteGoldFirebase = new ClienteFirebase()
        {
            AcaoGoldPorEstabelecimento = (goldEstab, tipoAcao) =>
            {
                if (tipoAcao == ClienteFirebase.TipoAcao.Adicionar)
                {
                    Cliente.ClienteLogado.goldPorEstabelecimento.Add(goldEstab);
                    return;
                }

                int indexGold = Cliente.ClienteLogado.goldPorEstabelecimento.FindIndex(x => x.estabelecimento == goldEstab.estabelecimento);

                if (tipoAcao == ClienteFirebase.TipoAcao.Modificar)
                {
                    Cliente.ClienteLogado.goldPorEstabelecimento.RemoveAt(indexGold);
                    Cliente.ClienteLogado.goldPorEstabelecimento.Add(goldEstab);
                    MenuEstabelecimento.AtualizarInfoGold(goldEstab.estabelecimento);
                }

                if (tipoAcao == ClienteFirebase.TipoAcao.Remover)
                {
                    Cliente.ClienteLogado.goldPorEstabelecimento.RemoveAt(indexGold);
                }
            }
        };

        clienteConfigAtualFirebase.Watch(true);
        clienteAvatarInfoFirebase.Watch(true);
        clieteGoldFirebase.WatchGoldPorEstab(Cliente.ClienteLogado._id, true);
    }
    #endregion

    #region Manipular Usuario

    private void preencherInfoUsuario()
    {
        TxtApelido.text = Cliente.ClienteLogado.apelido;
        PreencherAvatares.Invoke();
        atualizarExp();
    }

    private void atualizarExp()
    {
        int pctExp = Cliente.ClienteLogado.RetornarPctExp();

        TxtLevel.text = Cliente.ClienteLogado.avatar.info.level.ToString();
        TxtPctExp.text = pctExp + "%";
        SliderExp.value = pctExp / 100f;
    }

    #endregion

    #region btnAbrirQRCode
    private void btnAbrirQRCode()
    {
        EasyAudioUtility.Instance.Play(EasyAudioUtility.Som.Click_OK);
        StartCoroutine(permissaoCamera());
    }

    private IEnumerator permissaoCamera()
    {

#if UNITY_ANDROID
        if (!Permission.HasUserAuthorizedPermission(Permission.Camera))
        {
            Permission.RequestUserPermission(Permission.Camera);
        }

        yield return new WaitUntil(() => Permission.HasUserAuthorizedPermission(Permission.Camera));

#elif UNITY_IOS
        yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);

        if (!Application.HasUserAuthorization(UserAuthorization.WebCam))
        {
            txtMsg.text = "Precisamos da permissão da câmera";
            yield break;
        }
#else
        yield return null;
#endif

        if (!Application.HasUserAuthorization(UserAuthorization.WebCam))
        {
            throw new Exception("This Webcam library can't work without the webcam authorization");
        }

        SceneManager.LoadSceneAsync("LeitorQRCode", LoadSceneMode.Additive);
    }
    #endregion

    #region Esta No Estabelecimento
    public void ClienteEstaNoEstabelecimento()
    {
        if (existeConviteEstab()) return;

        if (clienteEstaNoEstab()) return;

        clienteNaoEstaNoEstab();
    }
    #endregion

    #region configurarConviteEstab
    private bool existeConviteEstab()
    {
        if (Cliente.ClienteLogado.configClienteAtual.conviteEstabPendente)
        {
            PnlDecisao.AbrirPainelConviteEstab(() =>
            {
                Dictionary<string, object> form = new Dictionary<string, object>
                {
                    { "estabelecimentoId", Cliente.ClienteLogado.configClienteAtual.estabelecimento } //na web
                };

                StartCoroutine(ClienteAPI.EntrarNoEstabelecimento(form,
                (response, error) =>
                {
                    if (error != null)
                    {
                        Debug.Log(error);
                        StartCoroutine(AlertaManager.Instance.ChamarAlertaMensagem(error, false));
                        return;
                    }

                    if (!response)
                    {
                        EasyAudioUtility.Instance.Play(EasyAudioUtility.Som.Error);
                    }
                }));
            },
            () =>
            {
                StartCoroutine(ClienteAPI.RecusarConviteEstabelecimento((response, error) =>
                {
                    if (error != null)
                    {
                        Debug.Log(error);
                        StartCoroutine(AlertaManager.Instance.ChamarAlertaMensagem(error, false));
                        return;
                    }

                    if (!response)
                        EasyAudioUtility.Instance.Play(EasyAudioUtility.Som.Error);
                }));
            },
            Cliente.ClienteLogado.configClienteAtual.nomeEstabelecimento);

            return true;
        }

        return false;
    }
    #endregion

    #region clienteEstaNoEstab
    private bool clienteEstaNoEstab()
    {
        //esta no estabelecimento
        if (Cliente.ClienteLogado.configClienteAtual.estaEmUmEstabelecimento)
        {
            PnlEstaNoEstabelecimento.SetActive(true);
            PnlNaoEstaNoEstabelecimento.SetActive(false);

            if (!string.IsNullOrEmpty(Cliente.ClienteLogado.configClienteAtual.comanda) && string.IsNullOrEmpty(MenuComanda.ComandaId))
            {
                bool temComanda = (string.IsNullOrEmpty(Cliente.ClienteLogado.configClienteAtual.comanda)) ? false : true;
                MenuComanda.IniciarWatchComanda(temComanda);
            }

            //comandaClienteFirebase.Watch(true);

            return true;
        }
        return false;
    }
    #endregion

    #region clienteNaoEstaNoEstab
    private void clienteNaoEstaNoEstab()
    {
        //nao esta
        PnlEstaNoEstabelecimento.SetActive(false);
        PnlNaoEstaNoEstabelecimento.SetActive(true);

        if (!estabelecimentosCarregados)
        {
            MenuEstabelecimento.ListarEstabelecimento();
            estabelecimentosCarregados = true;
        }

        //comandaClienteFirebase.Watch(false);

        if (string.IsNullOrEmpty(Cliente.ClienteLogado.configClienteAtual.comanda) && !String.IsNullOrEmpty(MenuComanda.ComandaId))
        {
            MenuComanda.IniciarWatchComanda(false);
        }
    }
    #endregion

    #region ObterIcones
    public void ObterIcones(string nomeIcon, FileManager.Directories tipo, Action<Texture2D> callback)
    {
        if (!string.IsNullOrEmpty(nomeIcon))
        {
            if (FileManager.Exists(tipo, nomeIcon))
            {
                Texture2D texture2d = FileManager.ConvertToTexture2D(FileManager.LoadFile(tipo, nomeIcon));
                callback(texture2d);
                return;
            }

            StartCoroutine(DownloadAPI.DownloadImage(nomeIcon, tipo.ToString(), (texture, bytes) =>
            {
                FileManager.SaveFile(tipo, nomeIcon, bytes);
                callback(texture);
            }));
        }
    }
    #endregion

    private void OnApplicationQuit()
    {
        FileManager.RemoveAllFiles();
    }
}
