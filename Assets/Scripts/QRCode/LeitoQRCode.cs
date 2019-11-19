using System;
using System.Collections;
using System.Collections.Generic;
using APIModel;
using BarcodeScanner;
using BarcodeScanner.Scanner;
using Network;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LeitoQRCode : MonoBehaviour
{

    private IScanner BarcodeScanner;
    public Text TextHeader;
    public RawImage Image;
    private float RestartTime;

    // Disable Screen Rotation on that screen
    void Awake()
    {
        Screen.autorotateToPortrait = false;
        Screen.autorotateToPortraitUpsideDown = false;

        // Enable vsync for the samples (avoid running mobile device at 300fps)
        QualitySettings.vSyncCount = 1;
    }

    void Start()
    {
        // Create a basic scanner
        BarcodeScanner = new Scanner();
        BarcodeScanner.Camera.Play();

        // Display the camera texture through a RawImage
        BarcodeScanner.OnReady += (sender, arg) =>
        {
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
        try
        {
            BarcodeScanner.Scan((barCodeType, barCodeValue) =>
            {
                BarcodeScanner.Stop();


                StartCoroutine(StopCamera(() =>
                {
                    Main.Instance.EntrarNoEstab(barCodeValue);
                    SceneManager.UnloadSceneAsync("LeitorQRCode");
                }));
                 

                //StartCoroutine(StopCamera(() =>
                //{
                //    entrarEstab(barCodeValue);
                //}));

                RestartTime += Time.realtimeSinceStartup + 1f;

#if UNITY_ANDROID || UNITY_IOS
                Handheld.Vibrate();
#endif
            });
        }
        catch (Exception e)
        {
            Debug.Log("Error: " + e.Message);
        }
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

    private void entrarEstab(string barCodeValue)
    {
        EasyAudioUtility.Instance.Play(EasyAudioUtility.Som.Som_Camera);

        Dictionary<string, string> coordenadas = new Dictionary<string, string>();

        StartCoroutine(GPSManager.Instance.IniciarGPS((lat, longi, sucesso) =>
        {
            if (sucesso)
            {
                coordenadas.Add("lat", lat.ToString());
                coordenadas.Add("long", longi.ToString());

                Dictionary<string, object> data = new Dictionary<string, object>
                        {
                            { "estabelecimentoId", barCodeValue },
                            { "coordenadas", coordenadas }
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
                        StartCoroutine(StopCamera(() =>
                        {
                            SceneManager.UnloadSceneAsync("LeitorQRCode");
                        }));

                        return;
                    }

                    if (!response)
                    {
                        EasyAudioUtility.Instance.Play(EasyAudioUtility.Som.Error);
                    }
                }));
                return;
            }
            AlertaManager.Instance.ChamarAlertaMensagem(AlertaManager.MsgAlerta.GPSError, false);
        }));

    }

    public void BtnFecharLeitor()
    {

        EasyAudioUtility.Instance.Play(EasyAudioUtility.Som.Click_Cancel);

        StartCoroutine(StopCamera(() =>
        {
            SceneManager.UnloadSceneAsync("LeitorQRCode");
        }));
    }

    #endregion
}
