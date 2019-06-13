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
        #region Watch - ConfigClienteAtual
        public void ConfigClienteAtual(object sender, ValueChangedEventArgs e)
        {
            if (e.DatabaseError != null)
            {
                Debug.LogError("ERRO EM ----------------------------- ConfigClienteAtual");
                Debug.LogError(e.DatabaseError.Message);
                return;
            }

            try
            {
                Cliente.ClienteLogado.configClienteAtual.estaEmUmEstabelecimento = (e.Snapshot.HasChild("estaEmUmEstabelecimento")) ?
                                                       Convert.ToBoolean(e.Snapshot.Child("estaEmUmEstabelecimento").Value) : false;

                Cliente.ClienteLogado.configClienteAtual.estabelecimento = (e.Snapshot.HasChild("estabelecimento")) ?
                                                       Convert.ToString(e.Snapshot.Child("estabelecimento").Value) : "";

                Cliente.ClienteLogado.configClienteAtual.nomeEstabelecimento = (e.Snapshot.HasChild("nomeEstabelecimento")) ?
                                                       Convert.ToString(e.Snapshot.Child("nomeEstabelecimento").Value) : "";

                Cliente.ClienteLogado.configClienteAtual.conviteEstabPendente = (e.Snapshot.HasChild("conviteEstabPendente")) ?
                                                       Convert.ToBoolean(e.Snapshot.Child("conviteEstabPendente").Value) : false;

                Main.Instance.ClienteEstaNoEstabelecimento();
            }
            catch (Exception x)
            {
                Debug.Log(x.Message);
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