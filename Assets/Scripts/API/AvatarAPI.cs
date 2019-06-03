using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Network
{
    public class AvatarAPI : API
    {
        //OK
        #region AvatarAlterar
        public static IEnumerator AvatarAlterar(
                Dictionary<string, string> properties,
                Action<bool, string> doneCallback = null)
        {
            var done = wrapCallback(doneCallback);

            yield return Post("alterar/cliente/avatar",
                 properties,
                 (request) =>
                 {
                     try
                     {
                         if (request == null ||
                         request.isNetworkError ||
                         request.responseCode != 200)
                         {
                             done(false, requestError(request, properties));
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
                         Debug.Log(ex.Message);
                     }
                 });

        }
        #endregion

    }
}