using APIModel;
using Network;
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
        Screen.SetResolution(480, 960, false);
        Screen.fullScreen = false;
#endif

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

        if (credenciais != null)
        {
            if (credenciais.tipoLogin == "normal")
                relogar("normal");

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
    private void relogar(string tipoLogin)
    {
        if (credenciais != null)
        {
            StartCoroutine(postLoginCliente(tipoLogin));
        }
    }
    #endregion

    #region Post Login Cliente
    private IEnumerator postLoginCliente(string tipoLogin)
    {
        Dictionary<string, object> data = new Dictionary<string, object>
            {
                { "email", credenciais.email },
                { "password", credenciais.password },
                { "tipoLogin", tipoLogin },
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
                AlterarProgressoSlider(0.5f);
                return;
            }

            estaLogado = true;

            AppManager.Instance.RefazerToken(response.token);
            AlterarProgressoSlider(0.3f);

            buscarClienteNoFirebase();
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
    }
    #endregion

}
