using System.Collections;
using System.Collections.Generic;
using APIModel;
using Network;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class APITeste : MonoBehaviour {

	public void btnVoltar(){
		PlayerPrefs.DeleteAll();
		SceneManager.LoadScene("Login");
	}

	public void EntrarNoEstabelecimento(){
        Main.Instance.ClienteEstaNoEstabelecimento();

        Dictionary<string, string> data = new Dictionary<string, string>
        {
            { "_idCliente", Cliente.ClienteLogado._id },
            { "_idEstabelecimento", "5bfd45728ca64e29c8fd7a78" } //na web
        };

        StartCoroutine(ClienteAPI.EntrarNoEstabelecimento(data, 
            (response, error) =>
			{
                APIManager.Retorno<string> retornoAPI = 
                        JsonConvert.DeserializeObject<APIManager.Retorno<string>>(response);
	
				if (retornoAPI.sucesso){
                    Main.Instance.ManipularMenus("FecharTodos");	
       
					string[] palavras = retornoAPI.msg.Split(' ');					

					Cliente.ClienteLogado.configClienteAtual.estaEmUmEstabelecimento = true;
					Cliente.ClienteLogado.configClienteAtual.estabelecimento = "5bfd45728ca64e29c8fd7a78";
					Cliente.ClienteLogado.configClienteAtual.nomeEstabelecimento = palavras[palavras.Length -1].Replace("!","");

                    //StartCoroutine(FindObjectOfType<Alerta>().ChamarAlerta(retornoAPI.msg, comunicadorAPI.PnlPrincipal));
                    Main.Instance.ClienteEstaNoEstabelecimento();     
				
				}else{
					EasyAudioUtility.Instance.Play(EasyAudioUtility.Som.Error);
				}
				
				//StartCoroutine(FindObjectOfType<Alerta>().ChamarAlerta(retornoAPI.msg, comunicadorAPI.PnlPrincipal));

			}));
    }
	
}
