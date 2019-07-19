using APIModel;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Network
{
    public class ComandaAPI : API
    {
        //OK
        #region ConvidarMembroGrupo
        public static IEnumerator ConvidarMembroGrupo(
                Dictionary<string, object> properties,
                Action<string, string> doneCallback = null)
        {
            var done = wrapCallback(doneCallback);

            yield return Post("convidar/membro_grupo/comanda",
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

                         Retorno<string> retornoAPI =
                                    JsonConvert.DeserializeObject<Retorno<string>>
                                    (request.downloadHandler.text);

                         if (retornoAPI.sucesso)
                         {
                             done(retornoAPI.mensagem, null);
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

        //OK
        #region CancelarConviteMembroGrupo
        public static IEnumerator CancelarConviteMembroGrupo(
                Dictionary<string, object> properties,
                Action<string, string> doneCallback = null)
        {
            var done = wrapCallback(doneCallback);

            yield return Post("cancelar_convite/membro_grupo/comanda",
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

                         Retorno<string> retornoAPI =
                                    JsonConvert.DeserializeObject<Retorno<string>>
                                    (request.downloadHandler.text);

                         if (retornoAPI.sucesso)
                         {
                             done(retornoAPI.mensagem, null);
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

        //OK
        #region ConvitesEnviadoGrupo
        public static IEnumerator ConvitesEnviadoGrupo(
                Dictionary<string, object> properties,
                Action<List<ConvitesComanda>, string> doneCallback = null)
        {
            var done = wrapCallback(doneCallback);

            yield return Post("listar/convites/enviados",
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

                         Retorno<List<ConvitesComanda>> retornoAPI =
                                    JsonConvert.DeserializeObject<Retorno<List<ConvitesComanda>>>
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

        //OK
        #region RespostaConviteGrupo
        public static IEnumerator RespostaConviteGrupo(
                Dictionary<string, object> properties,
                Action<bool, string> doneCallback = null)
        {
            var done = wrapCallback(doneCallback);

            yield return Post("resposta_convite/membro_grupo/comanda",
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
                             done(true, null);
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

        #region TransferirLiderancaGrupo
        public static IEnumerator TransferirLiderancaGrupo(
                Dictionary<string, object> properties,
                Action<bool, string> doneCallback = null)
        {
            var done = wrapCallback(doneCallback);

            yield return Post("transferir/lideranca/comanda",
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
                             done(true, null);
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