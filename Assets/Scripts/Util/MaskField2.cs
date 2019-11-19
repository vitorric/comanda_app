using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MaskField2 : MonoBehaviour
{

    public enum tiposFormatacao
    {
        CPF,
        Dinheiro,
        DataNascimento,
        CEP
    }

    public bool aplicarNoEndEdit;
    public TMP_InputField inputField;
    public tiposFormatacao TiposFormatacao;

    private void Awake()
    {
        if (aplicarNoEndEdit)
        {
            inputField.onEndEdit.AddListener(OnValueChanged);
            inputField.onValueChanged.AddListener(clearEditChange);
        }
        else
            inputField.onValueChanged.AddListener(OnValueChanged);
    }

    private void clearEditChange(string input)
    {
        Debug.Log(inputField.isFocused);
        if (input.Contains("/") && inputField.isFocused)
        {
            inputField.text = string.Empty;
            inputField.caretPosition = 0;
        }
    }


    private void OnValueChanged(string input)
    {
        string novaFormatacao = string.Empty;

        if (TiposFormatacao == tiposFormatacao.DataNascimento)
        {
            novaFormatacao = input.Replace("/", "");

            if (novaFormatacao.Length > 2)
                novaFormatacao = novaFormatacao.Insert(2, "/");

            if (novaFormatacao.Length > 5)
                novaFormatacao = novaFormatacao.Insert(5, "/");
        }

        if (TiposFormatacao == tiposFormatacao.CPF)
        {
            novaFormatacao = input.Replace(".", "").Replace("-", "");

            if (novaFormatacao.Length > 3)
                novaFormatacao = novaFormatacao.Insert(3, ".");

            if (novaFormatacao.Length > 7)
                novaFormatacao = novaFormatacao.Insert(7, ".");

            if (novaFormatacao.Length > 11)
                novaFormatacao = novaFormatacao.Insert(11, "-");
        }

        inputField.text = novaFormatacao;
        inputField.caretPosition = novaFormatacao.Length;
    }
}
