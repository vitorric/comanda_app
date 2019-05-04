using APIModel;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Network
{
    public class ClienteAPI : API
    {

        #region ClienteLogin
        public static IEnumerator ClienteLogin(
               Dictionary<string, string> properties,
               Action<Cliente.SessaoCliente, string> doneCallback = null)
        {
            var done = wrapCallback(doneCallback);

            try
            {
                return Post("login/cliente",
                    properties,
                    (request) =>
                    {
                        if (request == null ||
                            request.isNetworkError ||
                            request.responseCode != 200)
                            done(null, requestError(request, properties));

                        try
                        {
                            Retorno<Cliente.SessaoCliente> retornoAPI =
                                       JsonConvert.DeserializeObject<Retorno<Cliente.SessaoCliente>>
                                       (request.downloadHandler.text);

                            if (retornoAPI != null)
                            {
                                done(retornoAPI.retorno, null);
                            }
                        }
                        catch (Exception ex)
                        {
                            Debug.Log(ex.Message);
                            done(null, ex.Message);
                        }
                    });
            }
            catch (Exception ex)
            {
                Debug.Log(ex.Message);
                done(null, ex.Message);
            }

            return null;
        }
        #endregion

        #region ClienteRecuperarSenha
        public static IEnumerator ClienteRecuperarSenha(
                Dictionary<string, string> properties,
                Action<string, string> doneCallback = null)
        {
            var done = wrapCallback(doneCallback);

            try
            {
                return Post("recuperarSenha/cliente",
                    properties,
                    (request) =>
                    {
                        if (request == null ||
                            request.isNetworkError ||
                            request.responseCode != 200)
                            done(null, requestError(request, properties));

                        try
                        {
                            Retorno<string> retornoAPI =
                                       JsonConvert.DeserializeObject<Retorno<string>>
                                       (request.downloadHandler.text);

                            if (retornoAPI != null)
                            {
                                done(retornoAPI.retorno, null);
                            }
                        }
                        catch (Exception ex)
                        {
                            Debug.Log(ex.Message);
                            done(null, ex.Message);
                        }
                    });
            }
            catch (Exception ex)
            {
                Debug.Log(ex.Message);
                done(null, ex.Message);
            }

            return null;
        }
        #endregion

        #region ClienteCadastrar
        public static IEnumerator ClienteCadastrar(
                Dictionary<string, string> properties,
                Action<Cliente.SessaoCliente, string> doneCallback = null)
        {
            var done = wrapCallback(doneCallback);

            try
            {
                return Post("cadastrar/cliente",
                    properties,
                    (request) =>
                    {
                        if (request == null ||
                            request.isNetworkError ||
                            request.responseCode != 200)
                            done(null, requestError(request, properties));

                        try
                        {
                            Retorno<Cliente.SessaoCliente> retornoAPI =
                                       JsonConvert.DeserializeObject<Retorno<Cliente.SessaoCliente>>
                                       (request.downloadHandler.text);

                            if (retornoAPI != null)
                            {
                                done(retornoAPI.retorno, null);
                            }
                        }
                        catch (Exception ex)
                        {
                            Debug.Log(ex.Message);
                            done(null, ex.Message);
                        }
                    });
            }
            catch (Exception ex)
            {
                Debug.Log(ex.Message);
                done(null, ex.Message);
            }

            return null;
        }
        #endregion

        #region ClienteAlterarConfigApp
        public static IEnumerator ClienteAlterarConfigApp(
                Dictionary<string, string> properties,
                Action<string, string> doneCallback = null)
        {
            var done = wrapCallback(doneCallback);

            try
            {
                return Post("alterarConfigApp/cliente",
                    properties,
                    (request) =>
                    {
                        if (request == null ||
                            request.isNetworkError ||
                            request.responseCode != 200)
                            done(null, requestError(request, properties));

                        try
                        {
                            Retorno<string> retornoAPI =
                                       JsonConvert.DeserializeObject<Retorno<string>>
                                       (request.downloadHandler.text);

                            if (retornoAPI != null)
                            {
                                done(retornoAPI.retorno, null);
                            }
                        }
                        catch (Exception ex)
                        {
                            Debug.Log(ex.Message);
                            done(null, ex.Message);
                        }
                    });
            }
            catch (Exception ex)
            {
                Debug.Log(ex.Message);
                done(null, ex.Message);
            }

            return null;
        }
        #endregion

        #region ClienteAlterar
        public static IEnumerator ClienteAlterar(
                Dictionary<string, string> properties,
                Action<string, string> doneCallback = null)
        {
            var done = wrapCallback(doneCallback);

            try
            {
                return Post("alterar/cliente",
                    properties,
                    (request) =>
                    {
                        if (request == null ||
                            request.isNetworkError ||
                            request.responseCode != 200)
                            done(null, requestError(request, properties));

                        try
                        {
                            Retorno<string> retornoAPI =
                                       JsonConvert.DeserializeObject<Retorno<string>>
                                       (request.downloadHandler.text);

                            if (retornoAPI != null)
                            {
                                done(retornoAPI.retorno, null);
                            }
                        }
                        catch (Exception ex)
                        {
                            Debug.Log(ex.Message);
                            done(null, ex.Message);
                        }
                    });
            }
            catch (Exception ex)
            {
                Debug.Log(ex.Message);
                done(null, ex.Message);
            }

            return null;
        }
        #endregion

        #region ClienteComprarItem
        public static IEnumerator ClienteComprarItem(
                Dictionary<string, string> properties,
                Action<string, string> doneCallback = null)
        {
            var done = wrapCallback(doneCallback);

            try
            {
                return Post("compraritem/cliente",
                    properties,
                    (request) =>
                    {
                        if (request == null ||
                            request.isNetworkError ||
                            request.responseCode != 200)
                            done(null, requestError(request, properties));

                        try
                        {
                            Retorno<string> retornoAPI =
                                       JsonConvert.DeserializeObject<Retorno<string>>
                                       (request.downloadHandler.text);

                            if (retornoAPI != null)
                            {
                                done(retornoAPI.retorno, null);
                            }
                        }
                        catch (Exception ex)
                        {
                            Debug.Log(ex.Message);
                            done(null, ex.Message);
                        }
                    });
            }
            catch (Exception ex)
            {
                Debug.Log(ex.Message);
                done(null, ex.Message);
            }

            return null;
        }
        #endregion

        #region EntrarNoEstabelecimento
        public static IEnumerator EntrarNoEstabelecimento(
                Dictionary<string, string> properties,
                Action<string, string> doneCallback = null)
        {
            var done = wrapCallback(doneCallback);

            try
            {
                return Post("entrarestabelecimento/cliente",
                    properties,
                    (request) =>
                    {
                        if (request == null ||
                            request.isNetworkError ||
                            request.responseCode != 200)
                            done(null, requestError(request, properties));

                        try
                        {
                            Retorno<string> retornoAPI =
                                       JsonConvert.DeserializeObject<Retorno<string>>
                                       (request.downloadHandler.text);

                            if (retornoAPI != null)
                            {
                                done(retornoAPI.retorno, null);
                            }
                        }
                        catch (Exception ex)
                        {
                            Debug.Log(ex.Message);
                            done(null, ex.Message);
                        }
                    });
            }
            catch (Exception ex)
            {
                Debug.Log(ex.Message);
                done(null, ex.Message);
            }

            return null;
        }
        #endregion

        #region SairDoEstabelecimento
        public static IEnumerator SairDoEstabelecimento(
                Dictionary<string, string> properties,
                Action<string, string> doneCallback = null)
        {
            var done = wrapCallback(doneCallback);

            try
            {
                return Post("sairestabelecimento/cliente",
                    properties,
                    (request) =>
                    {
                        if (request == null ||
                            request.isNetworkError ||
                            request.responseCode != 200)
                            done(null, requestError(request, properties));

                        try
                        {
                            Retorno<string> retornoAPI =
                                       JsonConvert.DeserializeObject<Retorno<string>>
                                       (request.downloadHandler.text);

                            if (retornoAPI != null)
                            {
                                done(retornoAPI.retorno, null);
                            }
                        }
                        catch (Exception ex)
                        {
                            Debug.Log(ex.Message);
                            done(null, ex.Message);
                        }
                    });
            }
            catch (Exception ex)
            {
                Debug.Log(ex.Message);
                done(null, ex.Message);
            }

            return null;
        }
        #endregion

        #region ListarClienteConquistas
        public static IEnumerator ListarClienteConquistas(
                Dictionary<string, string> properties,
                Action<List<Cliente.Conquista>, string> doneCallback = null)
        {
            var done = wrapCallback(doneCallback);

            try
            {
                return Post("listar/cliente/conquistas",
                    properties,
                    (request) =>
                    {
                        if (request == null ||
                            request.isNetworkError ||
                            request.responseCode != 200)
                            done(null, requestError(request, properties));

                        try
                        {
                            Retorno<List<Cliente.Conquista>> retornoAPI =
                                       JsonConvert.DeserializeObject<Retorno<List<Cliente.Conquista>>>
                                       (request.downloadHandler.text);

                            if (retornoAPI != null)
                            {
                                done(retornoAPI.retorno, null);
                            }
                        }
                        catch (Exception ex)
                        {
                            Debug.Log(ex.Message);
                            done(null, ex.Message);
                        }
                    });
            }
            catch (Exception ex)
            {
                Debug.Log(ex.Message);
                done(null, ex.Message);
            }

            return null;
        }
        #endregion

        #region ListarHistoricoCompra
        public static IEnumerator ListarHistoricoCompra(
                Dictionary<string, string> properties,
                Action<List<HistoricoCompra>, string> doneCallback = null)
        {
            var done = wrapCallback(doneCallback);

            try
            {
                return Post("listar/cliente/historico/compra",
                    properties,
                    (request) =>
                    {
                        if (request == null ||
                            request.isNetworkError ||
                            request.responseCode != 200)
                            done(null, requestError(request, properties));

                        try
                        {
                            Retorno<List<HistoricoCompra>> retornoAPI =
                                       JsonConvert.DeserializeObject<Retorno<List<HistoricoCompra>>>
                                       (request.downloadHandler.text);

                            if (retornoAPI != null)
                            {
                                done(retornoAPI.retorno, null);
                            }
                        }
                        catch (Exception ex)
                        {
                            Debug.Log(ex.Message);
                            done(null, ex.Message);
                        }
                    });
            }
            catch (Exception ex)
            {
                Debug.Log(ex.Message);
                done(null, ex.Message);
            }

            return null;
        }
        #endregion
    }
}