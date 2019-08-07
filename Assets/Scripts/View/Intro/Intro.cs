using APIModel;
using FirebaseModel;
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

    private bool estaLogado = false;

    private IEnumerator Start()
    {

#if UNITY_STANDALONE
        Screen.SetResolution(480, 960, false);
        Screen.fullScreen = false;
#endif

        yield return new WaitUntil(() => FirebaseManager.Instance.isReady);

        criarDiretoriosImagens();

        AlterarProgressoSlider(0.3f);

        yield return StartCoroutine(relogar());

        AlterarProgressoSlider(0.2f);

        if (estaLogado)
            buscarClienteNoFireBase();
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
            if (estaLogado)
            {
                SceneManager.LoadSceneAsync("Main");
                return;
            }

            SceneManager.LoadScene("Login");
        }
    }
    #endregion

    #region buscarClienteNoFireBase
    private async void buscarClienteNoFireBase()
    {
        Cliente.ClienteLogado = await FirebaseManager.Instance.ObterUsuario(AppManager.Instance.Obter());

        AlterarProgressoSlider(0.2f);
    }
    #endregion

    #region Post Login Cliente
    private IEnumerator relogar()
    {
        Cliente.Credenciais credenciais = AppManager.Instance.ObterCredenciais();

        if (credenciais != null)
        {
            Dictionary<string, object> data = new Dictionary<string, object>
            {
                { "email", credenciais.email },
                { "password", credenciais.password }
            };

            yield return StartCoroutine(ClienteAPI.ClienteLogin(data,
            (response, error) =>
            {
                if (error != null)
                {
                    Debug.Log(error);
                    AlterarProgressoSlider(0.5f);
                    return;
                }

                estaLogado = true;
                AppManager.Instance.RefazerToken(response.token);
                AlterarProgressoSlider(0.3f);
            }));

            yield break;
        }

        AlterarProgressoSlider(0.5f);
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
