using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Network
{
    public class AvatarAPI : API
    {

        #region AvatarAlterar
        public static IEnumerator AvatarAlterar(
                Dictionary<string, string> properties,
                Action<string, string> doneCallback = null)
        {
            var done = wrapCallback(doneCallback);

            try
            {
                return Post("alterar/cliente/avatar",
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