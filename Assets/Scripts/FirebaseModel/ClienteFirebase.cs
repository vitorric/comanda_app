using APIModel;
using Firebase.Database;
using Newtonsoft.Json;
using System;
using System.Globalization;
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
                                                      DataSnapshot dsConfigApp = task.Result.Child("configApp");

                                                      cliente = new Cliente.Dados
                                                      {
                                                          email = Convert.ToString(task.Result.Child("email").Value),
                                                          apelido = Convert.ToString(task.Result.Child("apelido").Value),
                                                          cpf = Convert.ToString(task.Result.Child("cpf").Value),
                                                          dataNascimento = Convert.ToDateTime(task.Result.Child("dataNascimento").Value),
                                                          goldGeral = Convert.ToInt32(task.Result.Child("goldGeral").Value),
                                                          nome = Convert.ToString(task.Result.Child("nome").Value),
                                                          sexo = Convert.ToString(task.Result.Child("sexo").Value),
                                                          pontos = Convert.ToInt32(task.Result.Child("pontos").Value),
                                                          chaveAmigavel = Convert.ToString(task.Result.Child("chaveAmigavel").Value),
                                                          configApp = new Cliente.ConfigApp
                                                          {
                                                              somFundo = float.Parse(Convert.ToString(task.Result.Child("configApp").Child("somFundo").Value), CultureInfo.InvariantCulture),
                                                              somGeral = float.Parse(Convert.ToString(task.Result.Child("configApp").Child("somGeral").Value), CultureInfo.InvariantCulture)
                                                          },
                                                          avatar = new Cliente.Avatar
                                                          {
                                                              exp = Convert.ToInt32(task.Result.Child("avatar").Child("exp").Value),
                                                              level = Convert.ToInt32(task.Result.Child("avatar").Child("level").Value),
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
                                                          conquistas = new System.Collections.Generic.List<Cliente.Conquista>(),
                                                          goldPorEstabelecimento = new System.Collections.Generic.List<Cliente.GoldPorEstabelecimento>()
                                                      };
                                                  });



            //,
            return cliente;
        }

        #endregion

        #region IniciarWatch

        public async void IniciarWatch(string clienteId)
        {
            //await FirebaseDatabase.DefaultInstance.GetReference("clientes")
            //                                         .Child(clienteId)//nome da collection
            //                                         .Child("configClienteAtual")
            //                                         .Reference
            //                                         .ValueChanged += configClienteAtual;

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