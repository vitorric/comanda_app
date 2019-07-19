using APIModel;
using Firebase.Database;
using System;
using UnityEngine;

namespace FirebaseModel
{
    public class ClienteFirebase
    {
        public enum TipoAcao
        {
            Adicionar,
            Modificar,
            Remover
        }

        public Action<Cliente.Desafio, TipoAcao> AcaoDesafio;

        public void Watch(string clienteId, bool ehParaAdicionar)
        {
            var desafiosCliente = FirebaseDatabase.DefaultInstance.GetReference("desafios/" + clienteId);

            if (ehParaAdicionar)
            {
                desafiosCliente.ChildAdded += desafioAdicionar;
                desafiosCliente.ChildChanged += desafioModificar;
                desafiosCliente.ChildRemoved += desafioRemover;
                return;
            }

            if (!ehParaAdicionar)
            {
                desafiosCliente.ChildAdded -= desafioAdicionar;
                desafiosCliente.ChildChanged -= desafioModificar;
                desafiosCliente.ChildChanged -= desafioRemover;
                return;
            }
        }


        #region AcoesDesafio

        #region desafioAdicionar
        private void desafioAdicionar(object sender, ChildChangedEventArgs e)
        {
            if (e.DatabaseError != null)
            {
                Debug.LogError(e.DatabaseError.Message);
                return;
            }

            try
            {
                AcaoDesafio(tratarSnapshotItemLoja(e.Snapshot), TipoAcao.Adicionar);
            }
            catch (Exception x)
            {
                Debug.LogError(x.Message);
                throw x;
            }
        }
        #endregion

        #region desafioModificar
        private void desafioModificar(object sender, ChildChangedEventArgs e)
        {
            if (e.DatabaseError != null)
            {
                Debug.LogError(e.DatabaseError.Message);
                return;
            }

            try
            {
                AcaoDesafio(tratarSnapshotItemLoja(e.Snapshot), TipoAcao.Modificar);
            }
            catch (Exception x)
            {
                Debug.LogError(x.Message);
                throw x;
            }
        }
        #endregion

        #region desafioRemover
        private void desafioRemover(object sender, ChildChangedEventArgs e)
        {
            if (e.DatabaseError != null)
            {
                Debug.LogError(e.DatabaseError.Message);
                return;
            }

            try
            {
                AcaoDesafio(tratarSnapshotItemLoja(e.Snapshot), TipoAcao.Remover);
            }
            catch (Exception x)
            {
                Debug.LogError(x.Message);
                throw x;
            }
        }
        #endregion

        #region tratarSnapshotItemLoja
        private Cliente.Desafio tratarSnapshotItemLoja(DataSnapshot ds)
        {
            Cliente.Desafio desafio = new Cliente.Desafio
            {
                _id = Convert.ToString(ds.Child("_id").Value),
                concluido = Convert.ToBoolean(ds.Child("concluido").Value),
                resgatouPremio = Convert.ToBoolean(ds.Child("resgatouPremio").Value),
                progresso = Convert.ToInt32(ds.Child("progresso").Value),
                estabelecimento = Convert.ToString(ds.Child("estabelecimento").Value)
            };

            return desafio;
        }
        #endregion

        #endregion
    }
}