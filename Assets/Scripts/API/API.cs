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
        private const string urlBase = "http://localhost:3000/api/";

        public partial class Retorno<T>
        {
            public bool sucesso;
            public string msg;
            public T retorno;
        }

        internal static string requestError(UnityWebRequest request,
            Dictionary<string, string> propriedades)
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

            if (propriedades != null)
                Debug.LogError(JsonConvert.SerializeObject(propriedades));

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
                            Dictionary<string, string> data,
                            Action<UnityWebRequest> doneCallback = null)
        {
            bool sucesso = false;
            string response = string.Empty;
            string urlPost = urlBase + url;
            string dataSerialize = JsonConvert.SerializeObject(data);


            for (int i = 0; i < 3; i++)
            {
                using (UnityWebRequest request = UnityWebRequest.Post(urlPost, dataSerialize))
                {
                    request.method = UnityWebRequest.kHttpVerbPOST;
                    request.downloadHandler = new DownloadHandlerBuffer();
                    request.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(dataSerialize));

                    request.SetRequestHeader("Content-Type", CONTENT_TYPE_JSON);
                    request.SetRequestHeader("Accept", CONTENT_TYPE_JSON);
                    request.SetRequestHeader("Authorization", Cliente.ObterToken());

                    yield return request.SendWebRequest();
                    try
                    {
                        if (request.isNetworkError || request.isHttpError)
                        {
                            response = "Erro ao acessar o servidor!";

                            if (request.responseCode == 401)
                            {
                                response = "Usuário ou senha inválido!";
                                break;
                            }
                        }

                        if (request.downloadHandler != null &&
                            !string.IsNullOrEmpty(request.downloadHandler.text))
                        {
                            sucesso = true;
                            response = request.downloadHandler.text;
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.Log(Util.GetExceptionDetails(e));
                        doneCallback(request);
                        break;
                    }

                    if (sucesso)
                    {
                        doneCallback(request);
                        break;
                    }
                }
            }

            if (!sucesso)
                doneCallback(null);
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
