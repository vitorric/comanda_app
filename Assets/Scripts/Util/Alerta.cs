﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Alerta : MonoBehaviour {

    
    public GameObject PnlAlertaRef;

	[HideInInspector]
    public MsgAlerta MsgAlertas;

    public enum MsgAlerta
    {
        EmailSenhaIncorreta,
        PreenchaOsCampos,
        PreenchaAsSenhas,
        SenhasNaoConferem,
        DataInvalida,
        MenorIdade,
        SemDinheiro
	}

    private string mensagemAlerta(MsgAlerta msgAlerta, string algumValor)
    {
        switch (msgAlerta)
        {
            case MsgAlerta.EmailSenhaIncorreta:
                return "Email ou senha incorreta!";
            case MsgAlerta.PreenchaOsCampos:
                return "Preencha todos os campos!";
            case MsgAlerta.SenhasNaoConferem:
                return "Senhas não conferem!";
            case MsgAlerta.DataInvalida:
                return "Data inválida!";
            case MsgAlerta.MenorIdade:
                return "Este é um aplicativo para maiores de idade!";
            case MsgAlerta.PreenchaAsSenhas:
                return "É necessário preencher os campos de senha!";  
            case MsgAlerta.SemDinheiro:
                return "Saldo insuficiente!";              
            default:
                return null;
        }
    }

    public IEnumerator ChamarAlerta(MsgAlerta msgAlerta, GameObject ObjPai)
    {
        SomController.Tocar(SomController.Som.Error);
        if (GameObject.Find(msgAlerta.ToString()) == null)
        {
            GameObject objAlerta = Instantiate(PnlAlertaRef, ObjPai.transform);
            objAlerta.GetComponentInChildren<Text>().text = mensagemAlerta(msgAlerta, null);
            objAlerta.name = msgAlerta.ToString();
            objAlerta.GetComponentInChildren<Button>().onClick.AddListener(() => fechar(objAlerta));
            StartCoroutine(Animacoes.Mover(objAlerta, Animacoes.Posicao.Y, -10, 495));
            yield return new WaitForSeconds(2);
            StartCoroutine(Animacoes.Mover(objAlerta, Animacoes.Posicao.Y, 10, 765));
            yield return new WaitForSeconds(0.6f);
            Destroy(objAlerta);
        }
    }


    public IEnumerator ChamarAlerta(MsgAlerta msgAlerta, GameObject ObjPai, string algumValor)
    {
        SomController.Tocar(SomController.Som.Error);
        if (GameObject.Find(msgAlerta.ToString()) == null)
        {
            GameObject objAlerta = Instantiate(PnlAlertaRef, ObjPai.transform);
            objAlerta.GetComponentInChildren<Text>().text = mensagemAlerta(msgAlerta, algumValor);
            objAlerta.name = msgAlerta.ToString();
            objAlerta.GetComponentInChildren<Button>().onClick.AddListener(() => fechar(objAlerta));
            StartCoroutine(Animacoes.Mover(objAlerta, Animacoes.Posicao.Y, -10, 495));
            yield return new WaitForSeconds(2);
            StartCoroutine(Animacoes.Mover(objAlerta, Animacoes.Posicao.Y, 10, 725));
            yield return new WaitForSeconds(0.5f);
            Destroy(objAlerta);
        }
    }



    public IEnumerator ChamarAlerta(string msgAlerta, GameObject ObjPai)
    {
        SomController.Tocar(SomController.Som.Error);
        if (GameObject.Find(msgAlerta.ToString()) == null)
        {
            GameObject objAlerta = Instantiate(PnlAlertaRef, ObjPai.transform);
            objAlerta.GetComponentInChildren<Text>().text = msgAlerta;
            objAlerta.name = msgAlerta.ToString();
            objAlerta.GetComponentInChildren<Button>().onClick.AddListener(() => fechar(objAlerta));
            StartCoroutine(Animacoes.Mover(objAlerta, Animacoes.Posicao.Y, -10, 495));
            yield return new WaitForSeconds(2);
            StartCoroutine(Animacoes.Mover(objAlerta, Animacoes.Posicao.Y, 10, 725));
            yield return new WaitForSeconds(0.5f);
            Destroy(objAlerta);
        }
    }

    private void fechar(GameObject objAlerta)
    {
        SomController.Tocar(SomController.Som.Click_Cancel);
        Destroy(objAlerta);
    }
}
