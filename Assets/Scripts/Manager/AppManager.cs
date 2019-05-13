using UnityEngine;

public class AppManager : MonoBehaviour
{
    public static AppManager Instance { get; set; }

    public const float TEMPO_ANIMACAO_ABRIR_MODEL = 0.2f;
    public const float TEMPO_ANIMACAO_FECHAR_MODAL = 0.1f;
    public const float TEMPO_ANIMACAO_ABRIR_CLICK_BOTAO = 0.1f;


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
