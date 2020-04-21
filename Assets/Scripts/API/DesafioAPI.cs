using APIModel;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Network
{
    public class DesafioAPI : API
    {

        //OK
        #region ObterDesafio
        public static IEnumerator ObterDesafio(
                Dictionary<string, object> properties,
                Action<Desafio, string> doneCallback = null)
        {
            var done = wrapCallback(doneCallback);

            return Post("obter/desafio/cliente",
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
                        Retorno<Desafio> retornoAPI =
                                   JsonConvert.DeserializeObject<Retorno<Desafio>>
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
                        Debug.Log(ex.StackTrace);
                        //done(null, ex.Message);
                    }
                });
        }
        #endregion

        //OK
        #region ResgatarPremioDesafio
        public static IEnumerator ResgatarPremioDesafio(
                Dictionary<string, object> properties,
                Action<bool, string> doneCallback = null)
        {
            var done = wrapCallback(doneCallback);

            return Post("resgatar/recompensa/desafio",
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
                        done(false, ex.Message);
                    }
                });
        }
        #endregion

        //OK
        #region ListarDesafiosConcluidos
        public static IEnumerator ListarDesafiosConcluidos(
                Action<DesafioConcluido, string> doneCallback = null)
        {
            var done = wrapCallback(doneCallback);

            return Post("listar/cliente/desafios/concluido",
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
                        Retorno<DesafioConcluido> retornoAPI =
                                   JsonConvert.DeserializeObject<Retorno<DesafioConcluido>>
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
                        done(null, ex.Message);
                    }
                });
        }
        #endregion


        //OK
        #region ObterDesafioConcluido
        public static IEnumerator ObterDesafioConcluido(
                Dictionary<string, object> properties,
                Action<DesafioConcluido.InfoDesafio, string> doneCallback = null)
        {
            var done = wrapCallback(doneCallback);

            return Post("obter/cliente/desafios/concluido",
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
                        Retorno<DesafioConcluido.InfoDesafio> retornoAPI =
                                   JsonConvert.DeserializeObject<Retorno<DesafioConcluido.InfoDesafio>>
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
                        done(null, ex.Message);
                    }
                });
        }
        #endregion
    }
}
