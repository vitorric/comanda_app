using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Network
{
    public class GPSAPI : API
    {
        //OK
        #region ClienteLogin
        public static IEnumerator ValidarClienteEstabelecimento(
               Dictionary<string, object> properties,
               Action<bool, string> doneCallback = null)
        {
            var done = wrapCallback(doneCallback);

            yield return Post("validar/gps/cliente",
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
                            done(retornoAPI.retorno, null);
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
    }
}
