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

        Dictionary<string, object> data = new Dictionary<string, object>
            {
                { "estabelecimentoId", "5cdcc0885d47c43790919e7c" }
            };

        StartCoroutine(ClienteAPI.EntrarNoEstabelecimento(data,
        (response, error) =>
        {
            if (error != null)
            {
                Debug.Log(error);
                AlertaManager.Instance.ChamarAlertaMensagem(error, false);
                return;
            }

            if (response)
            {
                return;
            }

            if (!response)
            {
                EasyAudioUtility.Instance.Play(EasyAudioUtility.Som.Error);
            }
        }));
    }
	
}
