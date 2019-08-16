using APIModel;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace Network
{
    public abstract class API
    {
        public const string CONTENT_TYPE_JSON = "application/json";
        //private const string urlBase = "http://localhost:3000/api/";
        //private const string urlBaseDownloadIcon = "http://localhost:3000/";
        private const string urlBaseDownloadIcon = "http://93.188.164.122:3000/";
        private const string urlBase = "http://93.188.164.122:3000/api/";
        internal const string msgErro = "Solicitação inválida, tente novamente!";

        public partial class Retorno<T>
        {
            public bool sucesso;
            public string mensagem;
            public T retorno;
        }

        internal static string requestError(UnityWebRequest request)
        {

            string responseBody = string.Empty;
            string bug = string.Empty;

            if (request != null)
            {
                Debug.LogError(request.url);

                if (request.downloadHandler != null)
                {
                    responseBody = request.downloadHandler.text;
                }

                Debug.Log(string.Format(
                    "[api#error] request status code: {0}, data: ======= response: {1}, error: {2} =======",
                    request.responseCode, responseBody, request.error));
            }

            if (request.isHttpError)
            {
                bug = "Solicitação inválida, tente novamente!";
            }

            if (request.responseCode == 401)
            {
                bug = "Usuário ou senha inválido!";
            }

            if (request.isNetworkError)
            {
                bug = "Verifique sua conexão com a Internet!";
            }

            return bug;
        }

        internal static byte[] requestResponseDownload(UnityWebRequest request)
        {
            try
            {
                var responseData = request.downloadHandler.data;
                return responseData;
            }
            catch (Exception ex)
            {
                Debug.Log(ex.Message);
                return null;
            }
        }

        internal static Action<T1, T2> wrapCallback<T1, T2>(Action<T1, T2> doneCallback)
        {
            return doneCallback ?? ((_arg1, _arg2) => { });
        }

        #region Post
        internal static IEnumerator Post(string url,
                            Dictionary<string, object> data,
                            Action<UnityWebRequest> doneCallback = null)
        {
            string urlPost = urlBase + url;
            string dataSerialize = (data != null) ? JsonConvert.SerializeObject(data) : string.Empty;

            using (UnityWebRequest request = UnityWebRequest.Post(urlPost, dataSerialize))
            {
                request.method = UnityWebRequest.kHttpVerbPOST;
                request.downloadHandler = new DownloadHandlerBuffer();
                if (!string.IsNullOrEmpty(dataSerialize))
                    request.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(dataSerialize));

                request.SetRequestHeader("Content-Type", CONTENT_TYPE_JSON);
                request.SetRequestHeader("Accept", CONTENT_TYPE_JSON);
                request.SetRequestHeader("Authorization", AppManager.Instance.ObterToken());

                yield return request.SendWebRequest();

                doneCallback(request);
            }
        }
        #endregion

        #region DownloadImage
        internal static IEnumerator DownloadImage(string url,
                            Action<UnityWebRequest> doneCallback = null)
        {
            string urlBase = urlBaseDownloadIcon + url;
            UnityWebRequest request = UnityWebRequestTexture.GetTexture(urlBase);

            yield return request.SendWebRequest();

            if (request.isNetworkError)
            {
                Debug.Log("Error: " + request.error);
            }
            else
            {
                yield return new WaitUntil(() => request.downloadHandler.isDone);
                doneCallback(request);
            }
        }
        #endregion

        #region isOnline
        public static IEnumerator isOnline(Action<bool> online)
        {
            bool sucesso = true;
            using (UnityWebRequest webRequest = UnityWebRequest.Get("http://google.com.br"))
            {
                yield return webRequest.SendWebRequest();

                try
                {
                    if (webRequest == null ||
                        webRequest.isHttpError ||
                        webRequest.isNetworkError ||
                        !string.IsNullOrEmpty(webRequest.error))
                    {
                        sucesso = false;
                    }
                }
                catch (Exception e)
                {
                    Debug.Log(Util.GetExceptionDetails(e));
                    sucesso = false;
                }
            }

            Debug.Log("Conexao Internet: " + sucesso);

            online(sucesso);
        }
        #endregion

    }
}
