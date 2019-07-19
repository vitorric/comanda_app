using APIModel;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Network
{
    public class EstabelecimentoAPI : API
    {
        //OK
        #region ListarEstabelecimento
        public static IEnumerator ListarEstabelecimento(
                Dictionary<string, object> properties,
                Action<List<Estabelecimento>, string> doneCallback = null)
        {
            var done = wrapCallback(doneCallback);

            return Post("listar/estabelecimento/cliente",
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
                        Retorno<List<Estabelecimento>> retornoAPI =
                                   JsonConvert.DeserializeObject<Retorno<List<Estabelecimento>>>
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
        #region ObterEstabelecimento
        public static IEnumerator ObterEstabelecimento(
                Dictionary<string, object> properties,
                Action<Estabelecimento, string> doneCallback = null)
        {
            var done = wrapCallback(doneCallback);

            return Post("obter/estabelecimento/cliente",
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
                        Retorno<Estabelecimento> retornoAPI =
                                   JsonConvert.DeserializeObject<Retorno<Estabelecimento>>
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