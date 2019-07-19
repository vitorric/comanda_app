using APIModel;
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
        #region AlterarAvatar
        public static IEnumerator AlterarAvatar(
                Dictionary<string, object> properties,
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

        //OK
        #region ObterAvatar
        public static IEnumerator ObterAvatar(
                Dictionary<string, object> properties,
                Action<Cliente.Avatar, string> doneCallback = null)
        {
            var done = wrapCallback(doneCallback);

            yield return Post("obter/cliente/avatar",
                 properties,
                 (request) =>
                 {
                     try
                     {
                         if (request == null ||
                         request.isNetworkError ||
                         request.responseCode != 200)
                         {
                             done(null, requestError(request));
                             return;
                         }

                         Retorno<Cliente.Avatar> retornoAPI =
                                    JsonConvert.DeserializeObject<Retorno<Cliente.Avatar>>
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
                         done(null, msgErro);
                         Debug.Log(ex.Message);
                     }
                 });

        }
        #endregion

    }
}