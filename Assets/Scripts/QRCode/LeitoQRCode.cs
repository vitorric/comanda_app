using System;
using System.Collections;
using System.Collections.Generic;
using APIModel;
using BarcodeScanner;
using BarcodeScanner.Scanner;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LeitoQRCode : MonoBehaviour {

	private IScanner BarcodeScanner;
	public Text TextHeader;
	public RawImage Image;
	public GameObject PnlLoading;
	private float RestartTime;

	// Disable Screen Rotation on that screen
	void Awake()
	{
		Screen.autorotateToPortrait = false;
		Screen.autorotateToPortraitUpsideDown = false;

		// Enable vsync for the samples (avoid running mobile device at 300fps)
		QualitySettings.vSyncCount = 1;
	}

	void Start () {
		// Create a basic scanner
		BarcodeScanner = new Scanner();
		BarcodeScanner.Camera.Play();
		
		// Display the camera texture through a RawImage
		BarcodeScanner.OnReady += (sender, arg) => {
			// Set Orientation & Texture
			Image.transform.localEulerAngles = BarcodeScanner.Camera.GetEulerAngles();
			Image.transform.localScale = BarcodeScanner.Camera.GetScale();
			Image.texture = BarcodeScanner.Camera.Texture;

			// Keep Image Aspect Ratio
			var rect = Image.GetComponent<RectTransform>();
			var newHeight = rect.sizeDelta.x * BarcodeScanner.Camera.Height / BarcodeScanner.Camera.Width;
			rect.sizeDelta = new Vector2(rect.sizeDelta.x, newHeight);

			RestartTime = Time.realtimeSinceStartup;
		};
	}

	/// <summary>
	/// Start a scan and wait for the callback (wait 1s after a scan success to avoid scanning multiple time the same element)
	/// </summary>
	private void StartScanner()
	{
		BarcodeScanner.Scan((barCodeType, barCodeValue) => {
			BarcodeScanner.Stop();

			PnlLoading.SetActive(true);
			
			SomController.Tocar(SomController.Som.Som_Camera);
            
			WWWForm form = new WWWForm();
			form.AddField("_idCliente", Cliente.ClienteLogado._id);
			form.AddField("_idEstabelecimento", barCodeValue);

            StartCoroutine(APIManager.Instance.Post(APIManager.URLs.EntrarNoEstabelecimento,form, 
            (response) =>
			{
                APIManager.Retorno<string> retornoAPI = 
                    JsonConvert.DeserializeObject<APIManager.Retorno<string>>(response);
	
				if (retornoAPI.sucesso){

                    Main.Instance.ManipularMenus("FecharTodos");		

					string[] palavras = retornoAPI.msg.Split(' ');					

					Cliente.ClienteLogado.configClienteAtual.estaEmUmEstabelecimento = true;
					Cliente.ClienteLogado.configClienteAtual.estabelecimento = barCodeValue;
					Cliente.ClienteLogado.configClienteAtual.nomeEstabelecimento = palavras[palavras.Length -1].Replace("!","");

                    //StartCoroutine(FindObjectOfType<Alerta>().ChamarAlerta(retornoAPI.msg, comunicadorAPI.PnlPrincipal));
                    Main.Instance.ClienteEstaNoEstabelecimento();

					StartCoroutine(StopCamera(() => {
						SceneManager.UnloadSceneAsync("LeitorQRCode");
					}));
				}else{
					SomController.Tocar(SomController.Som.Error);
					PnlLoading.SetActive(false);
				}
				
				//StartCoroutine(FindObjectOfType<Alerta>().ChamarAlerta(retornoAPI.msg, comunicadorAPI.PnlPrincipal));                
			},
            (error) =>
            {
                //TODO: Tratar Error
            }));
            
            //TextHeader.text += "Found: " + barCodeType + " / " + barCodeValue + "\n";

            RestartTime += Time.realtimeSinceStartup + 1f;

			// Feedback
			//Audio.Play();

			#if UNITY_ANDROID || UNITY_IOS
			Handheld.Vibrate();
			#endif
		});
	}

	/// <summary>
	/// The Update method from unity need to be propagated
	/// </summary>
	void Update()
	{
		if (BarcodeScanner != null)
		{
			BarcodeScanner.Update();
		}

		// Check if the Scanner need to be started or restarted
		if (RestartTime != 0 && RestartTime < Time.realtimeSinceStartup)
		{
			StartScanner();
			RestartTime = 0;
		}
	}

	#region UI Buttons


	/// <summary>
	/// This coroutine is used because of a bug with unity (http://forum.unity3d.com/threads/closing-scene-with-active-webcamtexture-crashes-on-android-solved.363566/)
	/// Trying to stop the camera in OnDestroy provoke random crash on Android
	/// </summary>
	/// <param name="callback"></param>
	/// <returns></returns>
	public IEnumerator StopCamera(Action callback)
	{
		// Stop Scanning
		Image = null;
		BarcodeScanner.Destroy();
		BarcodeScanner = null;

		// Wait a bit
		yield return new WaitForSeconds(0.1f);

		callback.Invoke();
	}

	public void BtnFecharLeitor(){
		
		SomController.Tocar(SomController.Som.Click_Cancel);
		AnimacoesTween.AnimarObjeto(EventSystem.current.currentSelectedGameObject, AnimacoesTween.TiposAnimacoes.Button_Click, () => 
		{
			StartCoroutine(StopCamera(() => {
				SceneManager.UnloadSceneAsync("LeitorQRCode");
			}));
		});
	}

	#endregion
}
