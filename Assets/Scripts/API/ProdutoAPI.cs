using APIModel;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Network
{
    public class ProdutoAPI : API
    {
        //OK
        #region ObterProdutoCliente
        public static IEnumerator ObterProdutoCliente(
                Dictionary<string, object> properties,
                Action<Produto, string> doneCallback = null)
        {
            var done = wrapCallback(doneCallback);

            return Post("obter/produto/cliente",
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
                        Retorno<Produto> retornoAPI =
                                   JsonConvert.DeserializeObject<Retorno<Produto>>
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