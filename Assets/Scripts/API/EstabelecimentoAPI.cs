using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Network
{
    public class EstabelecimentoAPI : API
    {

        #region ListarEstabelecimento
        public static IEnumerator ListarEstabelecimento(
                Dictionary<string, string> properties,
                Action<List<Estabelecimento>, string> doneCallback = null)
        {
            var done = wrapCallback(doneCallback);

            try
            {
                return Post("listar/estabelecimento/cliente",
                    properties,
                    (request) =>
                    {
                        if (request == null ||
                            request.isNetworkError ||
                            request.responseCode != 200)
                            done(null, requestError(request, properties));

                        try
                        {
                            Retorno<List<Estabelecimento>> retornoAPI =
                                       JsonConvert.DeserializeObject<Retorno<List<Estabelecimento>>>
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


        #region ObterEstabelecimento
        public static IEnumerator ObterEstabelecimento(
                Dictionary<string, string> properties,
                Action<Estabelecimento, string> doneCallback = null)
        {
            var done = wrapCallback(doneCallback);

            try
            {
                return Post("obter/estabelecimento/cliente",
                    properties,
                    (request) =>
                    {
                        if (request == null ||
                            request.isNetworkError ||
                            request.responseCode != 200)
                            done(null, requestError(request, properties));

                        try
                        {
                            Retorno<Estabelecimento> retornoAPI =
                                       JsonConvert.DeserializeObject<Retorno<Estabelecimento>>
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