using APIModel;
using Firebase.Database;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FirebaseModel
{
    public class CorreioFirebase
    {
        public enum TipoAcao
        {
            Adicionar,
            Modificar,
            Remover
        }

        public Action<Correio.Mensagem, TipoAcao> AcaoCorreio;

        #region Watch
        public void Watch(string clienteId, bool ehParaAdicionar)
        {
            try
            {
                var comandaGrupo = FirebaseDatabase.DefaultInstance.GetReference("correios/" + clienteId + "/correio");

                if (ehParaAdicionar)
                {
                    comandaGrupo.ChildAdded += correioAdicionar;
                    comandaGrupo.ChildChanged += correioModificar;
                    comandaGrupo.ChildRemoved += correioRemover;
                }

                if (!ehParaAdicionar)
                {
                    comandaGrupo.ChildAdded -= correioAdicionar;
                    comandaGrupo.ChildChanged -= correioModificar;
                    comandaGrupo.ChildRemoved -= correioRemover;
                }
            }
            catch (Exception e)
            {
                Debug.Log("Watch Correio: " + e.Message);
            }
        }
        #endregion

        #region AcaoCorreio

        #region correioAdicionar
        private void correioAdicionar(object sender, ChildChangedEventArgs e)
        {
            if (e.DatabaseError != null)
            {
                Debug.LogError(e.DatabaseError.Message);
                return;
            }

            try
            {
                AcaoCorreio(tratarSnapshot(e.Snapshot), TipoAcao.Adicionar);
            }
            catch (Exception x)
            {
                Debug.LogError(x.Message);
                throw x;
            }
        }
        #endregion

        #region correioModificar
        private void correioModificar(object sender, ChildChangedEventArgs e)
        {
            if (e.DatabaseError != null)
            {
                Debug.LogError(e.DatabaseError.Message);
                return;
            }

            try
            {
                AcaoCorreio(tratarSnapshot(e.Snapshot), TipoAcao.Modificar);
            }
            catch (Exception x)
            {
                Debug.LogError(x.Message);
                throw x;
            }
        }
        #endregion

        #region correioRemover
        private void correioRemover(object sender, ChildChangedEventArgs e)
        {
            if (e.DatabaseError != null)
            {
                Debug.LogError(e.DatabaseError.Message);
                return;
            }

            try
            {
                AcaoCorreio(tratarSnapshot(e.Snapshot), TipoAcao.Remover);
            }
            catch (Exception x)
            {
                Debug.LogError(x.Message);
                throw x;
            }
        }
        #endregion

        #region tratarSnapshot
        private Correio.Mensagem tratarSnapshot(DataSnapshot ds)
        {
            Correio.Mensagem correio = new Correio.Mensagem
            {
                _id = ds.Child("_id").Value.ToString(),
                titulo = ds.Child("titulo").Value.ToString(),
                lida = Convert.ToBoolean(ds.Child("lida").Value),
                mensagem = ds.Child("mensagem").Value.ToString(),
                mensagemGrande = Convert.ToString(ds.Child("mensagemGrande").Value),
                dataCriacao = Util.ConverterDataFB(ds.Child("dataCriacao").Value.ToString()),
                acao = (ds.Child("acao").Exists) ? JsonConvert.DeserializeObject<Correio.Acao>(ds.Child("acao").GetRawJsonValue()) : null
            };

            return correio;
        }
        #endregion

        #endregion
    }
}
