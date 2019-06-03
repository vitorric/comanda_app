using APIModel;
using Firebase.Database;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using UnityEngine;

namespace FirebaseModel
{
    public class ClienteFirebase
    {
        #region ObterUsuario
        public async Task<Cliente.Dados> ObterUsuario(string _id)
        {
            Cliente.Dados cliente = null;

            await FirebaseDatabase.DefaultInstance.GetReference("clientes")
                                                  .Child(_id)//nome da collection
                                                  .GetValueAsync() //obtem os dados
                                                  .ContinueWith(task =>
                                                  {
                                                      cliente = JsonConvert.DeserializeObject<Cliente.Dados>(task.Result.GetRawJsonValue());

                                                      //if (usuario != null)
                                                      //{
                                                      //    iniciarWatch(usuarioResult);
                                                      //}

                                                  });

            return cliente;
        }

        #endregion

        #region IniciarWatch

        public async void IniciarWatch(string clienteId)
        {
            await FirebaseDatabase.DefaultInstance.GetReference("clientes")
                                                     .Child(clienteId)//nome da collection
                                                     .GetValueAsync() //obtem os dados
                                                     .ContinueWith(task =>
                                                     {
                                                         task.Result.Child("configClienteAtual")
                                                                      .Reference
                                                                      .ValueChanged += configClienteAtual;

                                                     });

        }

        #endregion

        #region Watch - configClienteAtual

        private void configClienteAtual(object sender, ValueChangedEventArgs e)
        {
            if (e.DatabaseError != null)
            {
                Debug.LogError(e.DatabaseError.Message);
                return;
            }

            try
            {
                Cliente.ClienteLogado.configClienteAtual = JsonConvert.DeserializeObject<Cliente.ConfigClienteAtual>(e.Snapshot.GetRawJsonValue());
                Main.Instance.ClienteEstaNoEstabelecimento();
            }
            catch (Exception x)
            {
                throw x;
            }
        }

        #endregion

        #region Watch - Exp/Level
        #endregion

        #region Watch - Status
        #endregion
        
    }
}