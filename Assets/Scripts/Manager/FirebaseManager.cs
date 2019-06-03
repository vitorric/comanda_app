using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using Network;
using UnityEngine;

public class FirebaseManager : MonoBehaviour
{
    public static FirebaseManager Instance { get; set; }
    public bool isReady { get; set; }
    public bool ConexaoOK = true;

    private readonly string uRLFirebase = "https://comanda-3c059.firebaseio.com/";

    void Awake()
    {
        if (Instance != null)
            Destroy(this);

        DontDestroyOnLoad(gameObject);
        Instance = this;

        StartCoroutine(API.isOnline((conexaoOK) => {

            if (conexaoOK)
            {
                FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
                {
                    DependencyStatus dependencyStatus = task.Result;
                    if (dependencyStatus == DependencyStatus.Available)
                    {
                        Debug.Log("Firebase pronto pra uso");

                        FirebaseApp app = FirebaseApp.DefaultInstance;
                        app.SetEditorDatabaseUrl(uRLFirebase);
                        if (app.Options.DatabaseUrl != null)
                            app.SetEditorDatabaseUrl(app.Options.DatabaseUrl);

                        isReady = true;
                    }
                    else
                    {
                        Debug.LogError("Could not resolve all Firebase dependencies: " + dependencyStatus);
                    }
                });

                return;
            }

            ConexaoOK = conexaoOK;
        }));
    }

    private void OnDestroy()
    {        
        FirebaseDatabase.DefaultInstance.App.Dispose();        
    }    
}
