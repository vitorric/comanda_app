using APIModel;
using Firebase;
using Firebase.Database;
using Firebase.Messaging;
using Firebase.Unity.Editor;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class FirebaseManager : MonoBehaviour
{
    //public static FirebaseManager Instance { get; set; }
    public bool isReady { get; set; }

    private readonly string uRLFirebase = "https://comanda-3c059.firebaseio.com/";

    void Awake()
    {
        //if (Instance != null)
        //    Destroy(this);

        //DontDestroyOnLoad(gameObject);
        //Instance = this;

        isReady = false;

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

                FirebaseMessaging.TokenReceived += OnTokenReceived;
                FirebaseMessaging.MessageReceived += OnMessageReceived;
            }
            else
            {
                Debug.LogError("Could not resolve all Firebase dependencies: " + dependencyStatus);
            }
        });
    }

    public void OnTokenReceived(object sender, TokenReceivedEventArgs token)
    {
        Debug.Log("Received Registration Token: " + token.Token);
        AppManager.Instance.tokenFirebase = token.Token;
    }
    public void OnMessageReceived(object sender, MessageReceivedEventArgs e)
    {
        foreach (KeyValuePair<string, string> item in e.Message.Data)
        {
            //Debug.Log("--------------------------" + e.Message.Notification.Body);
            //Debug.Log("--------------------------" + e.Message.RawData);
            //Debug.Log("--------------------------" + JsonConvert.SerializeObject(e.Message.Data));
            //Debug.LogFormat($"{item.Key}: {item.Value}");

            //AlertManager.Instance.ShowMessage(e.Message.Notification.Body);
        }
    }


    #region ObterUsuario
    public async Task<Cliente.Dados> ObterUsuario(string clienteId)
    {
        Cliente.Dados cliente = null;

        try
        {
            await FirebaseDatabase.DefaultInstance.GetReference("clientes")
                                                  .Child(clienteId)
                                                  .GetValueAsync() //obtem os dados
                                                  .ContinueWith(task =>
                                                  {
                                                      cliente = new Cliente.Dados
                                                      {
                                                          _id = Convert.ToString(task.Result.Child("_id").Value),
                                                          email = Convert.ToString(task.Result.Child("email").Value),
                                                          apelido = Convert.ToString(task.Result.Child("apelido").Value),
                                                          cpf = (task.Result.HasChild("cpf")) ? Convert.ToString(task.Result.Child("cpf").Value) : "",
                                                          dataNascimento = (task.Result.HasChild("dataNascimento")) ? Convert.ToDateTime(task.Result.Child("dataNascimento").Value) : DateTime.MinValue,
                                                          goldGeral = Convert.ToInt32(task.Result.Child("goldGeral").Value),
                                                          nome = Convert.ToString(task.Result.Child("nome").Value),
                                                          sexo = Convert.ToString(task.Result.Child("sexo").Value),
                                                          pontos = Convert.ToInt32(task.Result.Child("pontos").Value),
                                                          chaveAmigavel = Convert.ToString(task.Result.Child("chaveAmigavel").Value),
                                                          configApp = new Cliente.ConfigApp
                                                          {
                                                              somFundo = float.Parse(Convert.ToString(task.Result.Child("configApp").Child("somFundo").Value)),
                                                              somGeral = float.Parse(Convert.ToString(task.Result.Child("configApp").Child("somGeral").Value))
                                                          },
                                                          avatar = new Cliente.Avatar
                                                          {
                                                              _id = Convert.ToString(task.Result.Child("avatar").Child("_id").Value),
                                                              info = new Cliente.AvatarInfo
                                                              {
                                                                  exp = Convert.ToInt32(task.Result.Child("avatar").Child("info").Child("exp").Value),
                                                                  expProximoLevel = Convert.ToInt32(task.Result.Child("avatar").Child("info").Child("expProximoLevel").Value),
                                                                  level = Convert.ToInt32(task.Result.Child("avatar").Child("info").Child("level").Value),
                                                              },
                                                              barba = Convert.ToString(task.Result.Child("avatar").Child("barba").Value),
                                                              boca = Convert.ToString(task.Result.Child("avatar").Child("boca").Value),
                                                              cabeca = Convert.ToString(task.Result.Child("avatar").Child("cabeca").Value),
                                                              cabeloFrontal = Convert.ToString(task.Result.Child("avatar").Child("cabeloFrontal").Value),
                                                              cabeloTraseiro = Convert.ToString(task.Result.Child("avatar").Child("cabeloTraseiro").Value),
                                                              corBarba = Convert.ToString(task.Result.Child("avatar").Child("corBarba").Value),
                                                              corCabelo = Convert.ToString(task.Result.Child("avatar").Child("corCabelo").Value),
                                                              corPele = Convert.ToString(task.Result.Child("avatar").Child("corPele").Value),
                                                              corpo = Convert.ToString(task.Result.Child("avatar").Child("corpo").Value),
                                                              nariz = Convert.ToString(task.Result.Child("avatar").Child("nariz").Value),
                                                              olhos = Convert.ToString(task.Result.Child("avatar").Child("olhos").Value),
                                                              orelha = Convert.ToString(task.Result.Child("avatar").Child("orelha").Value),
                                                              roupa = Convert.ToString(task.Result.Child("avatar").Child("roupa").Value),
                                                              sombrancelhas = Convert.ToString(task.Result.Child("avatar").Child("sombrancelhas").Value)
                                                          },
                                                          configClienteAtual = new Cliente.ConfigClienteAtual(),
                                                          goldPorEstabelecimento = new System.Collections.Generic.List<Cliente.GoldPorEstabelecimento>()
                                                      };
                                                  });
        }
        catch (Exception e)
        {
            Debug.Log("---------------------------------------------------------------------------------------");
            Debug.Log("ObterUsuario: " + e.Message);
        }

        

        return cliente;
    }
    #endregion

    //private void OnApplicationQuit()
    //{
    //    FirebaseDatabase.DefaultInstance.App.Dispose();   
    //}
}
