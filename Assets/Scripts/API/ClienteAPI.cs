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
               Dictionary<string, object> properties,
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
                        done(null, requestError(request));
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
        #region ClienteLogin
        public static IEnumerator Deslogar(
               Dictionary<string, object> properties,
               Action<bool, string> doneCallback = null)
        {
            var done = wrapCallback(doneCallback);

            yield return Post("deslogar/cliente",
                properties,
                (request) =>
                {

                    if (request == null ||
                        request.isNetworkError ||
                        request.responseCode != 200)
                    {
                        done(false, requestError(request));
                        return;
                    }

                    try
                    {
                        Retorno<Cliente.SessaoCliente> retornoAPI =
                                   JsonConvert.DeserializeObject<Retorno<Cliente.SessaoCliente>>
                                   (request.downloadHandler.text);

                        if (retornoAPI.sucesso)
                        {
                            done(true, null);
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
        #region ClienteLoginFacebook
        public static IEnumerator ClienteLoginFacebook(
               Dictionary<string, object> properties,
               Action<Cliente.SessaoCliente, string> doneCallback = null)
        {
            var done = wrapCallback(doneCallback);

            yield return Post("login/facebook",
                properties,
                (request) =>
                {

                    if (request == null ||
                        request.isNetworkError ||
                        request.responseCode != 200)
                    {
                        done(null, requestError(request));
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
                Dictionary<string, object> properties,
                Action<string, string> doneCallback = null)
        {
            var done = wrapCallback(doneCallback);

            yield return Post("solicitar/recuperar_senha",
                 properties,
                 (request) =>
                 {
                     if (request == null ||
                         request.isNetworkError ||
                         request.responseCode != 200)
                     {
                         done(null, requestError(request));
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
                Dictionary<string, object> properties,
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
                        done(null, requestError(request));
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
        #region ClienteCadastrarValidarCPF
        public static IEnumerator ClienteCadastrarValidarCPF(
                Dictionary<string, object> properties,
                Action<bool, string> doneCallback = null)
        {
            var done = wrapCallback(doneCallback);

            yield return Post("validar/cadastrar/cliente/cpf",
                properties,
                (request) =>
                {
                    if (request == null ||
                        request.isNetworkError ||
                        request.responseCode != 200)
                    {
                        done(false, requestError(request));
                        return;
                    }

                    try
                    {
                        Retorno<bool> retornoAPI =
                                   JsonConvert.DeserializeObject<Retorno<bool>>
                                   (request.downloadHandler.text);

                        if (retornoAPI.sucesso)
                        {
                            done(true, null);
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
        #region ClienteCadastrarValidarEmail
        public static IEnumerator ClienteCadastrarValidarEmail(
                Dictionary<string, object> properties,
                Action<bool, string> doneCallback = null)
        {
            var done = wrapCallback(doneCallback);

            yield return Post("validar/cadastrar/cliente/email",
                properties,
                (request) =>
                {
                    if (request == null ||
                        request.isNetworkError ||
                        request.responseCode != 200)
                    {
                        done(false, requestError(request));
                        return;
                    }

                    try
                    {
                        Retorno<bool> retornoAPI =
                                   JsonConvert.DeserializeObject<Retorno<bool>>
                                   (request.downloadHandler.text);

                        if (retornoAPI.sucesso)
                        {
                            done(true, null);
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
        #region ClienteCadastrarValidarApelido
        public static IEnumerator ClienteCadastrarValidarApelido(
                Dictionary<string, object> properties,
                Action<bool, string> doneCallback = null)
        {
            var done = wrapCallback(doneCallback);

            yield return Post("validar/cadastrar/cliente/apelido",
                properties,
                (request) =>
                {
                    if (request == null ||
                        request.isNetworkError ||
                        request.responseCode != 200)
                    {
                        done(false, requestError(request));
                        return;
                    }

                    try
                    {
                        Retorno<bool> retornoAPI =
                                   JsonConvert.DeserializeObject<Retorno<bool>>
                                   (request.downloadHandler.text);

                        if (retornoAPI.sucesso)
                        {
                            done(true, null);
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
        #region ClienteAlterarConfigApp
        public static IEnumerator ClienteAlterarConfigApp(
                Dictionary<string, object> properties,
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
                        done(false, requestError(request));
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
                Dictionary<string, object> properties,
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
                        done(false, requestError(request));
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
                Dictionary<string, object> properties,
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
                        done(null, requestError(request));

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

        //OK
        #region EntrarNoEstabelecimento
        public static IEnumerator EntrarNoEstabelecimento(
                Dictionary<string, object> properties,
                Action<bool, string> doneCallback = null)
        {
            var done = wrapCallback(doneCallback);

            yield return Post("entrar_estabelecimento/cliente",
                properties,
                (request) =>
                {
                    if (request == null ||
                        request.isNetworkError ||
                        request.responseCode != 200)
                    {
                        done(false, requestError(request));
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

        #region SairDoEstabelecimento
        public static IEnumerator SairDoEstabelecimento(
                Action<bool, string> doneCallback = null)
        {
            var done = wrapCallback(doneCallback);

            yield return Post("sair_estabelecimento/cliente",
                null,
                (request) =>
                {
                    if (request == null ||
                        request.isNetworkError ||
                        request.responseCode != 200)
                    {
                        done(false, requestError(request));
                        return;
                    }

                    try
                    {
                        Retorno<string> retornoAPI =
                                   JsonConvert.DeserializeObject<Retorno<string>>
                                   (request.downloadHandler.text);

                        if (retornoAPI.sucesso)
                        {
                            done(true, null);
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
        #region RecusarConviteEstabelecimento
        public static IEnumerator RecusarConviteEstabelecimento(
                Action<bool, string> doneCallback = null)
        {
            var done = wrapCallback(doneCallback);

            yield return Post("recusar_convite_estabelecimento/cliente",
                null,
                (request) =>
                {
                    if (request == null ||
                        request.isNetworkError ||
                        request.responseCode != 200)
                    {
                        done(false, requestError(request));
                        return;
                    }

                    try
                    {
                        Retorno<string> retornoAPI =
                                   JsonConvert.DeserializeObject<Retorno<string>>
                                   (request.downloadHandler.text);

                        if (retornoAPI.sucesso)
                        {
                            done(true, null);
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
                        done(null, requestError(request));
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