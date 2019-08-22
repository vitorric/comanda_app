using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GPSManager : MonoBehaviour
{
    public static GPSManager Instance { get; set; }

    private float desiredAccuracy = 0.01f;
    private float updateDistance = 0.01f;

    private bool GPSDesabilitado = false;

    public void Awake()
    {
        if (Instance != null)
            Destroy(this);

        Instance = this;

        StartCoroutine(iniciarGPS());
    }

    private IEnumerator iniciarGPS()
    {
        if (!Input.location.isEnabledByUser)
        {
            GPSDesabilitado = true;
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
            GPSDesabilitado = true;
            Debug.Log("Time out");
            yield break;
        }

        if (Input.location.status == LocationServiceStatus.Failed)
        {
            GPSDesabilitado = true;
            Debug.Log("Falha ao iniciar o GPS");

        }

        LocationInfo location = Input.location.lastData;
        Debug.Log(location.latitude);
        Debug.Log(location.longitude);
    }
    //    latitude.text = location.latitude.ToString();
    //    altitude.text = location.altitude.ToString();
    //    horizontalAcccuracy.text = location.horizontalAccuracy.ToString();
    //    verticalAcccuracy.text = location.verticalAccuracy.ToString();
    //    lastUpdated.text = new DateTime(1970, 1, 1).AddSeconds(location.timestamp).ToString();
    //status.text = Input.location.status.ToString();
    //    isEnabled.text = Input.location.isEnabledByUser.ToString();

    private void OnApplicationQuit()
    {
        Input.location.Stop();
    }
}
