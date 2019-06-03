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
        //OK
        #region ClienteLogin
        public static IEnumerator ClienteLogin(
               Dictionary<string, string> properties,
               Action<Cliente.SessaoCliente, string> doneCallback = null)
        {
            var done = wrapCallback(doneCallback);

            yield return Post("login/cliente",
                properties,
                (request) =>
                {

                    if (request == null ||
                        request.isNetworkError ||
                        request.responseCode != 200)
                    {
                        done(null, requestError(request, properties));
                        return;
                    }

                    try
                    {
                        Retorno<Cliente.SessaoCliente> retornoAPI =
                                   JsonConvert.DeserializeObject<Retorno<Cliente.SessaoCliente>>
                                   (request.downloadHandler.text);

                        if (retornoAPI.sucesso)
                        {
                            done(retornoAPI.retorno, null);
                            return;
                        }

                        done(null, retornoAPI.mensagem);

                    }
                    catch (Exception ex)
                    {
                        Debug.Log(ex.Message);
                        done(null, msgErro);
                    }
                });
        }
        #endregion
        //OK
        #region ClienteRecuperarSenha
        public static IEnumerator ClienteRecuperarSenha(
                Dictionary<string, string> properties,
                Action<string, string> doneCallback = null)
        {
            var done = wrapCallback(doneCallback);

            yield return Post("recuperar_senha/cliente",
                 properties,
                 (request) =>
                 {
                     if (request == null ||
                         request.isNetworkError ||
                         request.responseCode != 200)
                     {
                         done(null, requestError(request, properties));
                         return;
                     }

                     try
                     {
                         Retorno<string> retornoAPI =
                                    JsonConvert.DeserializeObject<Retorno<string>>
                                    (request.downloadHandler.text);

                         if (retornoAPI != null)
                         {
                             done(retornoAPI.mensagem, null);
                         }
                     }
                     catch (Exception ex)
                     {
                         Debug.Log(ex.Message);
                         done(null, msgErro);
                     }
                 });
        }
        #endregion
        //OK
        #region ClienteCadastrar
        public static IEnumerator ClienteCadastrar(
                Dictionary<string, string> properties,
                Action<Cliente.SessaoCliente, string> doneCallback = null)
        {
            var done = wrapCallback(doneCallback);

            yield return Post("cadastrar/cliente",
                properties,
                (request) =>
                {
                    if (request == null ||
                        request.isNetworkError ||
                        request.responseCode != 200)
                    {
                        done(null, requestError(request, properties));
                        return;
                    }

                    try
                    {
                        Retorno<Cliente.SessaoCliente> retornoAPI =
                                   JsonConvert.DeserializeObject<Retorno<Cliente.SessaoCliente>>
                                   (request.downloadHandler.text);

                        if (retornoAPI.sucesso)
                        {
                            done(retornoAPI.retorno, null);
                            return;
                        }

                        done(null, retornoAPI.mensagem);
                    }
                    catch (Exception ex)
                    {
                        Debug.Log(ex.Message);
                        done(null, msgErro);
                    }
                });
        }
        #endregion

        //OK
        #region ClienteAlterarConfigApp
        public static IEnumerator ClienteAlterarConfigApp(
                Dictionary<string, string> properties,
                Action<bool, string> doneCallback = null)
        {
            var done = wrapCallback(doneCallback);

            yield return Post("alterar_config_app/cliente",
                properties,
                (request) =>
                {
                    if (request == null ||
                        request.isNetworkError ||
                        request.responseCode != 200)
                    {
                        done(false, requestError(request, properties));
                        return;
                    }

                    try
                    {
                        Retorno<string> retornoAPI =
                                   JsonConvert.DeserializeObject<Retorno<string>>
                                   (request.downloadHandler.text);

                        if (retornoAPI.sucesso)
                        {
                            done(retornoAPI.sucesso, null);
                            return;
                        }

                        done(false, retornoAPI.mensagem);
                    }
                    catch (Exception ex)
                    {
                        Debug.Log(ex.Message);
                        done(false, msgErro);
                    }
                });
        }
        #endregion

        //OK
        #region ClienteAlterar
        public static IEnumerator ClienteAlterar(
                Dictionary<string, string> properties,
                Action<bool, string> doneCallback = null)
        {
            var done = wrapCallback(doneCallback);

            yield return Post("alterar/cliente",
                properties,
                (request) =>
                {
                    if (request == null ||
                        request.isNetworkError ||
                        request.responseCode != 200)
                    {
                        done(false, requestError(request, properties));
                        return;
                    }

                    try
                    {
                        Retorno<bool> retornoAPI =
                                   JsonConvert.DeserializeObject<Retorno<bool>>
                                   (request.downloadHandler.text);

                        if (retornoAPI.sucesso)
                        {
                            done(retornoAPI.sucesso, null);
                            return;
                        }

                        done(false, retornoAPI.mensagem);
                    }
                    catch (Exception ex)
                    {
                        Debug.Log(ex.Message);
                        done(false, msgErro);
                    }
                });
        }
        #endregion

        #region ClienteComprarItem
        public static IEnumerator ClienteComprarItem(
                Dictionary<string, string> properties,
                Action<string, string> doneCallback = null)
        {
            var done = wrapCallback(doneCallback);

            yield return Post("comprar_item/cliente",
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

                        if (retornoAPI.sucesso)
                        {
                            done(retornoAPI.retorno, null);
                        }

                        done(null, retornoAPI.mensagem);
                    }
                    catch (Exception ex)
                    {
                        Debug.Log(ex.Message);
                        done(null, msgErro);
                    }
                });
        }
        #endregion

        #region EntrarNoEstabelecimento
        public static IEnumerator EntrarNoEstabelecimento(
                Dictionary<string, string> properties,
                Action<string, string> doneCallback = null)
        {
            var done = wrapCallback(doneCallback);

            yield return Post("entrarestabelecimento/cliente",
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
                        done(null, msgErro);
                    }
                });
        }
        #endregion

        #region SairDoEstabelecimento
        public static IEnumerator SairDoEstabelecimento(
                Dictionary<string, string> properties,
                Action<string, string> doneCallback = null)
        {
            var done = wrapCallback(doneCallback);

            yield return Post("sair_estabelecimento/cliente",
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
                        done(null, msgErro);
                    }
                });
        }
        #endregion

        #region RecusarConviteEstabelecimento
        public static IEnumerator RecusarConviteEstabelecimento(
                Dictionary<string, string> properties,
                Action<string, string> doneCallback = null)
        {
            var done = wrapCallback(doneCallback);

            yield return Post("recusar_convite_estabelecimento/cliente",
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
                        done(null, msgErro);
                    }
                });
        }
        #endregion

        #region ListarClienteConquistas
        public static IEnumerator ListarClienteConquistas(
                Dictionary<string, string> properties,
                Action<List<Cliente.Conquista>, string> doneCallback = null)
        {
            var done = wrapCallback(doneCallback);

            yield return Post("listar/cliente/conquistas",
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
                        done(null, msgErro);
                    }
                });
        }
        #endregion

        //OK
        #region ListarHistoricoCompra
        public static IEnumerator ListarHistoricoCompra(
                Action<List<HistoricoCompra>, string> doneCallback = null)
        {
            var done = wrapCallback(doneCallback);

            yield return Post("listar/cliente/historico/compra",
                null,
                (request) =>
                {
                    if (request == null ||
                        request.isNetworkError ||
                        request.responseCode != 200)
                    {
                        done(null, requestError(request, null));
                        return;
                    }

                    try
                    {
                        Retorno<List<HistoricoCompra>> retornoAPI =
                                   JsonConvert.DeserializeObject<Retorno<List<HistoricoCompra>>>
                                   (request.downloadHandler.text);

                        if (retornoAPI.sucesso)
                        {
                            done(retornoAPI.retorno, null);
                            return;
                        }

                        done(null, retornoAPI.mensagem);
                    }
                    catch (Exception ex)
                    {
                        Debug.Log(ex.Message);
                        done(null, msgErro);
                    }
                });
        }
        #endregion
    }
}