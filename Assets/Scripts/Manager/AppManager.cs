using UnityEngine;

public class AppManager : MonoBehaviour
{
    public static AppManager Instance { get; set; }

    public GameObject Loader;

    // Update is called once per frame
    void Awake()
    {
        if (Instance != null)
            Destroy(this);

        DontDestroyOnLoad(gameObject);
        Instance = this;
    }    

    public void AtivarLoader()
    {
        Loader.SetActive(true);
    }

    public void DesativarLoader()
    {
        Loader.SetActive(false);
    }
}
