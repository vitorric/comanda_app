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
        public Action<Cliente.GoldPorEstabelecimento, TipoAcao> AcaoGoldPorEstabelecimento;

        public void WatchGoldPorEstab(string clienteId, bool ehParaAdicionar)
        {

            var goldPorEstabelecimento = FirebaseDatabase.DefaultInstance.GetReference("clientes/" + clienteId + "/goldPorEstabelecimento");

            if (ehParaAdicionar)
            {
                goldPorEstabelecimento.ChildAdded += goldPorEstabelecimentoAdicionar;
                goldPorEstabelecimento.ChildChanged += goldPorEstabelecimentoModificar;
                goldPorEstabelecimento.ChildRemoved += goldPorEstabelecimentoRemover;
                return;
            }

            if (!ehParaAdicionar)
            {
                goldPorEstabelecimento.ChildAdded -= goldPorEstabelecimentoAdicionar;
                goldPorEstabelecimento.ChildChanged -= goldPorEstabelecimentoModificar;
                goldPorEstabelecimento.ChildRemoved -= goldPorEstabelecimentoRemover;
            }
        }

        public void WatchDesafios(string clienteId, bool ehParaAdicionar)
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
                AcaoDesafio(tratarSnapshotDesafio(e.Snapshot), TipoAcao.Adicionar);
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
                AcaoDesafio(tratarSnapshotDesafio(e.Snapshot), TipoAcao.Modificar);
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
                AcaoDesafio(tratarSnapshotDesafio(e.Snapshot), TipoAcao.Remover);
            }
            catch (Exception x)
            {
                Debug.LogError(x.Message);
                throw x;
            }
        }
        #endregion

        #region tratarSnapshotDesafio
        private Cliente.Desafio tratarSnapshotDesafio(DataSnapshot ds)
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

        #region AcoesGoldPorEstabelecimento

        #region goldPorEstabelecimentoAdicionar
        private void goldPorEstabelecimentoAdicionar(object sender, ChildChangedEventArgs e)
        {
            if (e.DatabaseError != null)
            {
                Debug.LogError(e.DatabaseError.Message);
                return;
            }

            try
            {
                AcaoGoldPorEstabelecimento(tratarSnapshotGoldPorEstabelecimento(e.Snapshot), TipoAcao.Adicionar);
            }
            catch (Exception x)
            {
                Debug.LogError(x.Message);
                throw x;
            }
        }
        #endregion

        #region goldPorEstabelecimentoModificar
        private void goldPorEstabelecimentoModificar(object sender, ChildChangedEventArgs e)
        {
            if (e.DatabaseError != null)
            {
                Debug.LogError(e.DatabaseError.Message);
                return;
            }

            try
            {
                AcaoGoldPorEstabelecimento(tratarSnapshotGoldPorEstabelecimento(e.Snapshot), TipoAcao.Modificar);
            }
            catch (Exception x)
            {
                Debug.LogError(x.Message);
                throw x;
            }
        }
        #endregion

        #region goldPorEstabelecimentoRemover
        private void goldPorEstabelecimentoRemover(object sender, ChildChangedEventArgs e)
        {
            if (e.DatabaseError != null)
            {
                Debug.LogError(e.DatabaseError.Message);
                return;
            }

            try
            {
                AcaoGoldPorEstabelecimento(tratarSnapshotGoldPorEstabelecimento(e.Snapshot), TipoAcao.Remover);
            }
            catch (Exception x)
            {
                Debug.LogError(x.Message);
                throw x;
            }
        }
        #endregion

        #region tratarSnapshotDesafio
        private Cliente.GoldPorEstabelecimento tratarSnapshotGoldPorEstabelecimento(DataSnapshot ds)
        {
            Cliente.GoldPorEstabelecimento goldPorEstabelecimento = new Cliente.GoldPorEstabelecimento
            {
                gold = Convert.ToInt32(ds.Child("gold").Value),
                estabelecimento = Convert.ToString(ds.Child("estabelecimento").Value)
            };

            return goldPorEstabelecimento;
        }
        #endregion

        #endregion
    }
}