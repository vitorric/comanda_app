using Network;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class DownloadAPI : API
{
    //OK
    #region DownloadImage
    public static IEnumerator DownloadImage(
            string nomeIcon,
            string tipo,
            Action<Texture2D, byte[]> doneCallback = null)
    {
        var done = wrapCallback(doneCallback);

        return DownloadImage("files/" + tipo + "/icon/" + nomeIcon,
            (request) =>
            {
                if (request == null ||
                    request.isNetworkError ||
                    request.responseCode != 200)
                {
                    done(null, null);
                    return;
                }

                try
                {
                    done(((DownloadHandlerTexture)request.downloadHandler).texture, request.downloadHandler.data);
                }
                catch (Exception ex)
                {
                    Debug.Log(ex.Message);
                    done(null, null);
                }
            });
    }
    #endregion
}
