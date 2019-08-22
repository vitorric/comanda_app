using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MaskField : MonoBehaviour {

    public enum tiposFormatacao{
        CPF,
		Dinheiro,
        DataNascimento,
        CEP
	}

 	public TMP_InputField inputField;

    public tiposFormatacao TiposFormatacao;

    private void Awake()
    {
        inputField.onValueChanged.AddListener(OnValueChanged);
    }

    private void OnValueChanged(string input)
    {
        List<char> novosCaracteres;
        List<char> caracteres;
        List<char> formatacao;
        string novaFormatacao = string.Empty;
        string format = string.Empty;
        int index = 0;
        int indexCaracteres = 0;

        switch(TiposFormatacao){
			case tiposFormatacao.Dinheiro:
			    caracteres = input.ToCharArray().ToList();
                formatacao = "###.##.##,##".ToCharArray().ToList();

                caracteres.Remove(',');
                caracteres.RemoveAll(x => x == '.');

                novosCaracteres = new List<char>(caracteres.Count);

                index = formatacao.Count - 1;
                indexCaracteres = caracteres.Count - 1;

                for (int i = 0; i < caracteres.Count; i++)
                {
                    if (caracteres.Count == 1)
                    {
                        novosCaracteres.Add(caracteres[i]);
                    }
                    else
                    {
                        if (formatacao[index] != '#')
                        {
                            novosCaracteres.Add(formatacao[index]);
                        }

                        novosCaracteres.Add(caracteres[indexCaracteres]);

                        index--;
                        indexCaracteres--;
                    }
                }

                novosCaracteres.Reverse();
                novosCaracteres.ForEach(x => novaFormatacao += x);                
			break;
            case tiposFormatacao.DataNascimento:
                format = "##/#/####";
                
                if (validarSomenteNumeros() == false){                 
                    return;
                }

                if (input.Length >= format.Length +2){
                    removerUltimoCaracter();
                    return;
                }

                caracteres = input.ToCharArray().ToList();
                formatacao = format.ToCharArray().ToList();

                caracteres.RemoveAll(x => x == '/');

                novosCaracteres = new List<char>(caracteres.Count);

                index = 0;
                indexCaracteres = 0;

                for (int i = 0; i < caracteres.Count; i++)
                {
                    if (caracteres.Count == 1)
                    {
                        novosCaracteres.Add(caracteres[i]);
                    }
                    else
                    {
                        if (formatacao[index] != '#')
                        {
                            novosCaracteres.Add(formatacao[index]);
                        }

                        novosCaracteres.Add(caracteres[indexCaracteres]);

                        index++;
                        indexCaracteres++;
                    }
                }
                
                novosCaracteres.ForEach(x => novaFormatacao += x);   
            break;
            case tiposFormatacao.CPF:
                format = "###.##.##-##";
                
                if (input.Length >= format.Length +3){
                    removerUltimoCaracter();
                    return;
                }

                caracteres = input.ToCharArray().ToList();
                formatacao = format.ToCharArray().ToList();

                caracteres.Remove('-');
                caracteres.RemoveAll(x => x == '.');

                novosCaracteres = new List<char>(caracteres.Count);

                index = 0;
                indexCaracteres = 0;

                for (int i = 0; i < caracteres.Count; i++)
                {
                    if (caracteres.Count == 1)
                    {
                        novosCaracteres.Add(caracteres[i]);
                    }
                    else
                    {
                        if (formatacao[index] != '#')
                        {
                            novosCaracteres.Add(formatacao[index]);
                        }

                        novosCaracteres.Add(caracteres[indexCaracteres]);

                        index++;
                        indexCaracteres++;
                    }
                }
                
                novosCaracteres.ForEach(x => novaFormatacao += x);   
            break;
            case tiposFormatacao.CEP:
                format = "#####-###";
                
                if (input.Length >= format.Length +1){
                    removerUltimoCaracter();
                    return;
                }

                caracteres = input.ToCharArray().ToList();
                formatacao = format.ToCharArray().ToList();

                caracteres.Remove('-');

                novosCaracteres = new List<char>(caracteres.Count);

                index = 0;
                indexCaracteres = 0;

                for (int i = 0; i < caracteres.Count; i++)
                {
                    if (caracteres.Count == 1)
                    {
                        novosCaracteres.Add(caracteres[i]);
                    }
                    else
                    {
                        if (formatacao[index] != '#')
                        {
                            novosCaracteres.Add(formatacao[index]);
                        }

                        novosCaracteres.Add(caracteres[indexCaracteres]);

                        index++;
                        indexCaracteres++;
                    }
                }
                
                novosCaracteres.ForEach(x => novaFormatacao += x);   
            break;
		}

        inputField.text = novaFormatacao;
        inputField.caretPosition = novaFormatacao.Length;
    }

    private bool validarSomenteNumeros(){
        List<string> numeros = new List<string>{"0", "1","2","3", "4","5","6", "7","8","9"};

        try{
            string ultimoCaracter = inputField.text.Substring(inputField.text.Length-1, 1);
        
            if (numeros.Find(x => x == ultimoCaracter) != null)
                return true;

            removerUltimoCaracter();
        }
        catch(Exception e)
        {
            print(e.Message);
        }
        
        return false;
    }

    private void removerUltimoCaracter(){
        inputField.text = inputField.text.Substring(0, inputField.text.Length-1);
    }
}
