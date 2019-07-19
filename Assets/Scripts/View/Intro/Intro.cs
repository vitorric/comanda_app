﻿using APIModel;
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
        yield return new WaitUntil(() => FirebaseManager.Instance.isReady);

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

    private async void buscarClienteNoFireBase()
    {
        Cliente.ClienteLogado = await FirebaseManager.Instance.ObterUsuario(AppManager.Instance.Obter());

        AlterarProgressoSlider(0.2f);
    }

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

}
