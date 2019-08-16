using APIModel;
using Firebase.Database;
using System;
using UnityEngine;

namespace FirebaseModel
{
    public class EstabelecimentoFirebase
    {

        public enum TipoAcao
        {
            Adicionar,
            Modificar,
            Remover
        }

        public Action<ItemLoja, TipoAcao> AcaoItemLoja;
        public Action<Desafio, TipoAcao> AcaoDesafio;

        public void Watch_TelaEstabelecimento(string estabelecimentoId, bool ehParaAdicionar)
        {
            var estabelecimentosLoja = FirebaseDatabase.DefaultInstance.GetReference("estabelecimentos/" + estabelecimentoId + "/itensLoja");
            var estabelecimentosDesafios = FirebaseDatabase.DefaultInstance.GetReference("estabelecimentos/" + estabelecimentoId + "/desafios");

            if (ehParaAdicionar)
            {
                estabelecimentosLoja.ChildAdded += itensLojaAdicionar;
                estabelecimentosLoja.ChildChanged += itensLojaModificar;
                estabelecimentosLoja.ChildRemoved += itensLojaRemover;

                estabelecimentosDesafios.ChildAdded += desafioAdicionar;
                estabelecimentosDesafios.ChildChanged += desafioModificar;
                estabelecimentosDesafios.ChildRemoved += desafioRemover;

                return;
            }

            if (!ehParaAdicionar)
            {
                estabelecimentosLoja.ChildAdded -= itensLojaAdicionar;
                estabelecimentosLoja.ChildChanged -= itensLojaModificar;
                estabelecimentosLoja.ChildChanged -= itensLojaRemover;

                estabelecimentosDesafios.ChildAdded -= desafioAdicionar;
                estabelecimentosDesafios.ChildChanged -= desafioModificar;
                estabelecimentosDesafios.ChildRemoved -= desafioRemover;
                AcaoItemLoja = null;
                AcaoDesafio = null;
                return;
            }
        }

        #region AcoesItemLoja

        #region itensLojaAdicionar
        private void itensLojaAdicionar(object sender, ChildChangedEventArgs e)
        {
            if (e.DatabaseError != null)
            {
                Debug.LogError(e.DatabaseError.Message);
                return;
            }

            try
            {
                AcaoItemLoja(tratarSnapshotItemLoja(e.Snapshot), TipoAcao.Adicionar);
            }
            catch (Exception x)
            {
                Debug.LogError(x.Message);
                throw x;
            }
        }
        #endregion

        #region itensLojaModificar
        private void itensLojaModificar(object sender, ChildChangedEventArgs e)
        {
            if (e.DatabaseError != null)
            {
                Debug.LogError(e.DatabaseError.Message);
                return;
            }

            try
            {
                AcaoItemLoja(tratarSnapshotItemLoja(e.Snapshot), TipoAcao.Modificar);
            }
            catch (Exception x)
            {
                Debug.LogError(x.Message);
                throw x;
            }
        }
        #endregion

        #region itensLojaRemover
        private void itensLojaRemover(object sender, ChildChangedEventArgs e)
        {
            if (e.DatabaseError != null)
            {
                Debug.LogError(e.DatabaseError.Message);
                return;
            }

            try
            {
                AcaoItemLoja(tratarSnapshotItemLoja(e.Snapshot), TipoAcao.Remover);
            }
            catch (Exception x)
            {
                Debug.LogError(x.Message);
                throw x;
            }
        }
        #endregion

        #region tratarSnapshotItemLoja
        private ItemLoja tratarSnapshotItemLoja(DataSnapshot ds)
        {
            ItemLoja itemLoja = new ItemLoja
            {
                _id = Convert.ToString(ds.Child("_id").Value),
                descricao = Convert.ToString(ds.Child("descricao").Value),
                icon = Convert.ToString(ds.Child("icon").Value),
                nome = Convert.ToString(ds.Child("nome").Value),
                hotSale = Convert.ToBoolean(ds.Child("hotSale").Value),
                preco = Convert.ToDouble(ds.Child("preco").Value),
                quantidadeDisponivel = Convert.ToInt32(ds.Child("quantidadeDisponivel").Value),
                quantidadeVendida = Convert.ToInt32(ds.Child("quantidadeVendida").Value),
                tempoDisponivel = Util.ConverterDataFB(ds.Child("tempoDisponivel").Value.ToString())
            };

            return itemLoja;
        }
        #endregion

        #endregion

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
        private Desafio tratarSnapshotDesafio(DataSnapshot ds)
        {
            DataSnapshot dsObjetivo = ds.Child("objetivo");
            DataSnapshot dsPremio = ds.Child("premio");

            Desafio desafio = new Desafio
            {
                _id = Convert.ToString(ds.Child("_id").Value),
                descricao = Convert.ToString(ds.Child("descricao").Value),
                icon = Convert.ToString(ds.Child("icon").Value),
                nome = Convert.ToString(ds.Child("nome").Value),
                pontos = (ds.Child("pontos").Exists) ? Convert.ToInt32(ds.Child("pontos").Value) : 0,
                emGrupo = Convert.ToBoolean(ds.Child("emGrupo").Value),
                tempoDuracao = Util.ConverterDataFB(ds.Child("tempoDuracao").Value.ToString()),
                objetivo = new Desafio.Objetivo
                {
                    quantidade = Convert.ToInt32(dsObjetivo.Child("quantidade").Value),
                    tipo = Convert.ToString(dsObjetivo.Child("tipo").Value),
                    produto = Convert.ToString(dsObjetivo.Child("produto").Exists ? dsObjetivo.Child("produto").Value : null)
                },
                premio = new Desafio.Premio
                {
                    quantidade = Convert.ToInt32(dsPremio.Child("quantidade").Value),
                    tipo = Convert.ToString(dsPremio.Child("tipo").Value),
                    produto = Convert.ToString(dsPremio.Child("produto").Exists ? dsPremio.Child("produto").Value : null)
                }
            };

            return desafio;
        }
        #endregion

        #endregion
    }
}