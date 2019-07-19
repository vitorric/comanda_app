using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Network
{
    public class CorreioAPI : API
    {
        #region MarcarMensagemComoLida
        public static IEnumerator MarcarMensagemComoLida(
                Dictionary<string, object> properties,
                Action<bool, string> doneCallback = null)
        {
            var done = wrapCallback(doneCallback);

            yield return Post("correio/mensagem_lida",
                 properties,
                 (request) =>
                 {
                     try
                     {
                         if (request == null ||
                         request.isNetworkError ||
                         request.responseCode != 200)
                         {
                             done(false, requestError(request));
                             return;
                         }

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
                         done(false, msgErro);
                         Debug.Log(ex.Message);
                     }
                 });
        }
        #endregion
    }
}
