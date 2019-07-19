using Firebase.Database;
using Newtonsoft.Json;
using System;
using UnityEngine;

namespace FirebaseModel
{
    public class GenericFirebase<T>
    {
        private DatabaseReference reference;

        public Action<T> Callback;

        public GenericFirebase(string path)
        {
            reference = FirebaseDatabase.
                         DefaultInstance.
                         GetReference(path);
        }

        public void Watch(bool ehParaAdicionar)
        {
            try
            {
                if (ehParaAdicionar)
                {
                    reference.ValueChanged += callback;
                    return;
                }

                if (!ehParaAdicionar)
                {
                    reference.ValueChanged -= callback;
                    //Callback = null;
                }
            }
            catch (Exception e)
            {
                Debug.Log("Watch GenericFirebase");
                Debug.LogError(e.Message);
            }
        }
        private void callback(object sender, ValueChangedEventArgs e)
        {
            try
            {
                if (e.DatabaseError != null)
                {
                    Debug.LogError(e.DatabaseError.Message);
                    return;
                }

                if (e.Snapshot.Exists)
                {
                    Callback(JsonConvert.DeserializeObject<T>(e.Snapshot.GetRawJsonValue()));
                }
            }
            catch (Exception ex)
            {
                Debug.Log("callback GenericFirebase");
                Debug.LogError(ex.Message);
            }
        }
    }
}
