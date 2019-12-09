using APIModel;
using Network;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Intro : MonoBehaviour
{
    public Text txtProgresso;
    public Text txtCarregando;
    public Slider sliderProgresso;
    public FirebaseManager firebaseManager;

    private bool estaLogado = false;
    private bool apiForaDoAr = false;

    Cliente.Credenciais credenciais;
    FacebookManager fbManager;

    private IEnumerator Start()
    {

#if UNITY_STANDALONE
        //PlayerPrefs.DeleteAll();
        Screen.SetResolution(480, 960, false);
        Screen.fullScreen = false;
#endif

        EasyAudioUtility.Instance.AjustarSomBG(0);
        //EasyAudioUtility.Instance.AjustarSomSFX(0);

        yield return new WaitUntil(() => firebaseManager.isReady);

        criarDiretoriosImagens();

        AlterarProgressoSlider(0.3f);

        credenciais = AppManager.Instance.ObterCredenciais();

        fbManager = new FacebookManager()
        {
            tipoLogin = (credenciais == null) ? "normal" : credenciais.tipoLogin,
            relogar = relogar
        };

        fbManager.Init();

        Debug.Log(JsonConvert.SerializeObject(credenciais));

        if (credenciais != null)
        {
            if (credenciais.tipoLogin == "normal")
                relogar("normal", null);

            yield break;
        }

        AlterarProgressoSlider(0.7f);
    }

    #region Manipula o progresso
    public void AlterarProgressoSlider(float value)
    {
        sliderProgresso.value += value;
        txtProgresso.text = (sliderProgresso.value * 100) + "%";
        conferirProgresso();
    }

    private void conferirProgresso()
    {
        if (sliderProgresso.value == 1)
        {
            if (apiForaDoAr)
            {
                txtCarregando.text = "Ops, aplicativo em manutenção! Tente novamente mais tarde!";
                return;
            }

            if (estaLogado)
            {
                SceneManager.LoadSceneAsync("Main");
                return;
            }

            SceneManager.LoadSceneAsync("Login");
        }
    }
    #endregion

    #region buscarClienteNoFireBase
    private async void buscarClienteNoFireBase()
    {
        Cliente.ClienteLogado = await firebaseManager.ObterUsuario(AppManager.Instance.Obter());

        AlterarProgressoSlider(0.2f);
    }
    #endregion

    #region relogar
    private void relogar(string tipoLogin, string socialId)
    {
        if (credenciais != null)
        {

            if (tipoLogin == "normal")
            {
                StartCoroutine(postLoginCliente());
                return;
            }

            if (tipoLogin == "facebook")
            {
                if (string.IsNullOrEmpty(socialId))
                {
                    AlterarProgressoSlider(0.7f);
                    return;
                }

                StartCoroutine(postLoginFacebook(socialId));
            }
        }
    }
    #endregion

    #region Post Login Cliente
    private IEnumerator postLoginCliente()
    {
        Dictionary<string, object> data = new Dictionary<string, object>
            {
                { "email", credenciais.email },
                { "password", credenciais.password },
                { "deviceId", AppManager.Instance.deviceId },
                { "tokenFirebase", AppManager.Instance.tokenFirebase }
            };

        yield return StartCoroutine(ClienteAPI.ClienteLogin(data,
        (response, error) =>
        {
            if (error != null)
            {
                apiForaDoAr = true;
                Debug.Log(error);
                AlterarProgressoSlider(0.7f);
                return;
            }

            estaLogado = true;

            AppManager.Instance.RefazerToken(response.token);
            AlterarProgressoSlider(0.3f);

            buscarClienteNoFirebase();
        }));
    }
    #endregion

    #region Post Login Facebook
    private IEnumerator postLoginFacebook(string socialId)
    {
        Dictionary<string, object> data = new Dictionary<string, object>
            {
                { "socialId", socialId },
                { "deviceId", AppManager.Instance.deviceId },
                { "tokenFirebase", AppManager.Instance.tokenFirebase }
            };

        yield return StartCoroutine(ClienteAPI.ClienteLoginFacebook(data,
        (response, error) =>
        {
            if (error != null)
            {
                apiForaDoAr = true;
                Debug.Log(error);
                AlterarProgressoSlider(0.7f);
                return;
            }

            if (response != null)
            {
                estaLogado = true;

                AppManager.Instance.RefazerToken(response.token);
                AlterarProgressoSlider(0.3f);

                buscarClienteNoFirebase();
                return;
            }

            AlterarProgressoSlider(0.5f);
        }));
    }
    #endregion

    #region buscarClienteNoFirebase
    private void buscarClienteNoFirebase()
    {
        AlterarProgressoSlider(0.2f);

        if (estaLogado)
            buscarClienteNoFireBase();
    }
    #endregion

    #region criarDiretoriosImagens
    private void criarDiretoriosImagens()
    {
        FileManager.CreateFileDirectory(FileManager.Directories.desafio);
        FileManager.CreateFileDirectory(FileManager.Directories.item_Loja);
        FileManager.CreateFileDirectory(FileManager.Directories.produto);
        FileManager.CreateFileDirectory(FileManager.Directories.estabelecimento);
    }
    #endregion

}
