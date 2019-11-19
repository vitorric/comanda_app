using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;

public class GPSManager : MonoBehaviour
{
    public static GPSManager Instance { get; set; }

    private float desiredAccuracy = 0.01f;
    private float updateDistance = 0.01f;

    public void Awake()
    {
        if (Instance != null)
            Destroy(this);

        Instance = this;
    }

    public IEnumerator IniciarGPS(Action<float, float, bool> callback)
    {

#if PLATFORM_ANDROID
        if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            Permission.RequestUserPermission(Permission.FineLocation);
        }

        yield return new WaitUntil(() => Permission.HasUserAuthorizedPermission(Permission.FineLocation));
#endif

        if (!Input.location.isEnabledByUser)
        {
            callback(0, 0, true);
            Debug.Log("GPS Desabilitado");
            yield break;
        }

        Input.location.Start(desiredAccuracy, updateDistance);
        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;

        }
        if (maxWait <= 0)
        {
            callback(0, 0, false);
            Debug.Log("Time out");
            yield break;
        }

        if (Input.location.status == LocationServiceStatus.Failed)
        {
            callback(0, 0, false);
            Debug.Log("Falha ao iniciar o GPS");

        }

        LocationInfo location = Input.location.lastData;
        Debug.Log(location.latitude);
        Debug.Log(location.longitude);
        callback(location.latitude, location.longitude, true);
        Input.location.Stop();
    }

    private void OnApplicationQuit()
    {
        Input.location.Stop();
    }
}
