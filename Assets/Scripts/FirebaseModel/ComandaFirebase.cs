using APIModel;
using Firebase.Database;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FirebaseModel
{
    public class ComandaFirebase
    {
        public enum TipoAcao
        {
            Adicionar,
            Modificar,
            Remover
        }

        public Action<Comanda.Grupo, TipoAcao> AcaoGrupo;
        public Action<Comanda.Produto, TipoAcao> AcaoProdutos;

        #region Watch
        public void Watch(string comandaId, bool ehParaAdicionar)
        {
            try
            {
                var comandaGrupo = FirebaseDatabase.DefaultInstance.GetReference("comandas/" + comandaId + "/grupo");
                var comandaProdutos = FirebaseDatabase.DefaultInstance.GetReference("comandas/" + comandaId + "/produtos");

                if (ehParaAdicionar)
                {
                    comandaGrupo.ChildAdded += grupoAdicionar;
                    comandaGrupo.ChildChanged += grupoModificar;
                    comandaGrupo.ChildRemoved += grupoRemover;

                    comandaProdutos.ChildAdded += produtoAdicionar;
                    comandaProdutos.ChildChanged += produtoModificar;
                    comandaProdutos.ChildRemoved += produtoRemover;
                }

                if (!ehParaAdicionar)
                {
                    comandaGrupo.ChildAdded -= grupoAdicionar;
                    comandaGrupo.ChildChanged -= grupoModificar;
                    comandaGrupo.ChildRemoved -= grupoRemover;

                    comandaProdutos.ChildAdded -= produtoAdicionar;
                    comandaProdutos.ChildChanged -= produtoModificar;
                    comandaProdutos.ChildRemoved -= produtoRemover;
                }
            }
            catch (Exception e)
            {
                Debug.Log("Watch Comanda: " + e.Message);
            }
        }
        #endregion

        #region AcaoGrupo

        #region grupoAdicionar
        private void grupoAdicionar(object sender, ChildChangedEventArgs e)
        {
            if (e.DatabaseError != null)
            {
                Debug.LogError(e.DatabaseError.Message);
                return;
            }

            try
            {
                AcaoGrupo(tratarSnapshotGrupo(e.Snapshot), TipoAcao.Adicionar);
            }
            catch (Exception x)
            {
                Debug.LogError(x.Message);
                throw x;
            }
        }
        #endregion

        #region grupoModificar
        private void grupoModificar(object sender, ChildChangedEventArgs e)
        {
            if (e.DatabaseError != null)
            {
                Debug.LogError(e.DatabaseError.Message);
                return;
            }

            try
            {
                AcaoGrupo(tratarSnapshotGrupo(e.Snapshot), TipoAcao.Modificar);
            }
            catch (Exception x)
            {
                Debug.LogError(x.Message);
                throw x;
            }
        }
        #endregion

        #region grupoRemover
        private void grupoRemover(object sender, ChildChangedEventArgs e)
        {
            if (e.DatabaseError != null)
            {
                Debug.LogError(e.DatabaseError.Message);
                return;
            }

            try
            {
                AcaoGrupo(tratarSnapshotGrupo(e.Snapshot), TipoAcao.Remover);
            }
            catch (Exception x)
            {
                Debug.LogError(x.Message);
                throw x;
            }
        }
        #endregion

        #region tratarSnapshotGrupo
        private Comanda.Grupo tratarSnapshotGrupo(DataSnapshot ds)
        { 
            Comanda.Grupo grupo = new Comanda.Grupo
            {
                cliente = JsonConvert.DeserializeObject<Comanda.Cliente>(ds.Child("cliente").GetRawJsonValue()),
                lider = Convert.ToBoolean(ds.Child("lider").Value),
                valorPago = Convert.ToDouble(ds.Child("valorPago").Value),
                jaPagou = Convert.ToBoolean(ds.Child("jaPagou").Value),
                avatarAlterado = Util.ConverterDataFB(ds.Child("avatarAlterado").Value.ToString())
            };

            return grupo;
        }
        #endregion

        #endregion

        #region AcaoProduto

        #region produtoAdicionar
        private void produtoAdicionar(object sender, ChildChangedEventArgs e)
        {
            if (e.DatabaseError != null)
            {
                Debug.LogError(e.DatabaseError.Message);
                return;
            }

            try
            {
                AcaoProdutos(tratarSnapshotProduto(e.Snapshot), TipoAcao.Adicionar);
            }
            catch (Exception x)
            {
                Debug.LogError(x.Message);
                throw x;
            }
        }
        #endregion

        #region produtoModificar
        private void produtoModificar(object sender, ChildChangedEventArgs e)
        {
            if (e.DatabaseError != null)
            {
                Debug.LogError(e.DatabaseError.Message);
                return;
            }

            try
            {
                AcaoProdutos(tratarSnapshotProduto(e.Snapshot), TipoAcao.Modificar);
            }
            catch (Exception x)
            {
                Debug.LogError(x.Message);
                throw x;
            }
        }
        #endregion

        #region produtoRemover
        private void produtoRemover(object sender, ChildChangedEventArgs e)
        {
            if (e.DatabaseError != null)
            {
                Debug.LogError(e.DatabaseError.Message);
                return;
            }

            try
            {
                AcaoProdutos(tratarSnapshotProduto(e.Snapshot), TipoAcao.Remover);
            }
            catch (Exception x)
            {
                Debug.LogError(x.Message);
                throw x;
            }
        }
        #endregion

        #region tratarSnapshotProduto
        private Comanda.Produto tratarSnapshotProduto(DataSnapshot ds)
        {
            Comanda.Produto produto = new Comanda.Produto
            {
                infoProduto = JsonConvert.DeserializeObject<Comanda.InfoProduto>(ds.Child("produto").GetRawJsonValue()),
                preco = Convert.ToDouble(ds.Child("preco").Value),
                precoTotal = Convert.ToDouble(ds.Child("precoTotal").Value),
                quantidade = Convert.ToInt32(ds.Child("quantidade").Value)
            };

            return produto;
        }
        #endregion

        #endregion

    }
}
