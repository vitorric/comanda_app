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
                        Debug.Log(ex.Message);
                        done(null, ex.Message);
                    }
                });
        }
        #endregion
    }
}
