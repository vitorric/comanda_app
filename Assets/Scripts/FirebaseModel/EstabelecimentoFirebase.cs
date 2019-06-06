using APIModel;
using Firebase.Database;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            tempoDisponivel = Convert.ToDateTime(ds.Child("tempoDisponivel").Value)
        };

        return itemLoja;
    }
    #endregion

    #endregion


    #region AcoesDesafio

    #region itensLojaAdicionar
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

    #region itensLojaModificar
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

    #region itensLojaRemover
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

    #region tratarSnapshotItemLoja
    private Desafio tratarSnapshotDesafio(DataSnapshot ds)
    {
        DataSnapshot dsObjetivo = ds.Child("objetivo");

        Desafio desafio = new Desafio
        {
            _id = Convert.ToString(ds.Child("_id").Value),
            descricao = Convert.ToString(ds.Child("descricao").Value),
            icon = Convert.ToString(ds.Child("icon").Value),
            nome = Convert.ToString(ds.Child("nome").Value),
            premio = Convert.ToInt32(ds.Child("premio").Value),
            emGrupo = Convert.ToBoolean(ds.Child("emGrupo").Value),
            tempoDuracao = Convert.ToDateTime(ds.Child("tempoDuracao").Value),
            objetivo = new Desafio.Objetivo
            {
                quantidade = Convert.ToInt32(dsObjetivo.Child("quantidade").Value),
                tipo = Convert.ToString(dsObjetivo.Child("tipo").Value),
                produto = Convert.ToString(dsObjetivo.Child("produto").Value)
            }
        };

        return desafio;
    }
    #endregion

    #endregion
}
