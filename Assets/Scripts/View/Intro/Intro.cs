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

    private void Start()
    {
        //PlayerPrefs.DeleteAll();
        AlterarProgressoSlider(0.5f);
        StartCoroutine(aguardarFireBase());
    }


    #region Inicia o Firebase
    private IEnumerator aguardarFireBase()
    {
        yield return new WaitUntil(() => FirebaseManager.Instance.isReady || !FirebaseManager.Instance.ConexaoOK);

        if (!FirebaseManager.Instance.ConexaoOK)
        {
            Debug.Log("Sem Net");
            yield break;
        }

        AlterarProgressoSlider(0.3f);
        validarLoginUsuario();
    }
    #endregion

    #region Manipula o progresso
    public void AlterarProgressoSlider(float value)
    {
        sliderProgresso.value += value;
        txtProgresso.text = (sliderProgresso.value * 100) + "%";
        StartCoroutine(conferirProgresso());
    }

    private IEnumerator conferirProgresso()
    {
        if (sliderProgresso.value == 1)
        {
            if (estaLogado)
            {
                yield return StartCoroutine(relogar());
                SceneManager.LoadScene("Main");
                yield break;
            }

            SceneManager.LoadScene("Login");
        }
    }
    #endregion

    #region Verifica se esta logado
    private async void validarLoginUsuario()
    {
        if (Cliente.EstaLogado())
        {
            ClienteFirebase cliente = new ClienteFirebase();
            Cliente.ClienteLogado = await cliente.ObterUsuario(Cliente.Obter());

            if (Cliente.ClienteLogado == null)
                estaLogado = false;

            estaLogado = true;
            AlterarProgressoSlider(0.2f);

            return;
        }

        estaLogado = false;
        AlterarProgressoSlider(0.2f);
    }
    #endregion

    #region Post Login Cliente
    private IEnumerator relogar()
    {
        Cliente.Credenciais credenciais = Cliente.ObterCredenciais();

        Dictionary<string, string> data = new Dictionary<string, string>
        {
            { "email", credenciais.email },
            { "password", credenciais.password }
        };

        yield return StartCoroutine(ClienteAPI.ClienteLogin(data,
        (response, error) =>
        {
            Cliente.RefazerToken(response.token);

            StartCoroutine(AppManager.Instance.DesativarLoader());
            //StartCoroutine(comunicadorAPI.Alerta.ChamarAlerta(retornoAPI.msg, comunicadorAPI.PnlPrincipal));
        }));
    }
    #endregion
}
