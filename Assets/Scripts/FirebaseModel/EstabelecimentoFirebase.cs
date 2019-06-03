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
        Modificar
    }

    public Action<List<ItemLoja>, TipoAcao> AcaoItemLoja;

    public void Watch_TelaEstabelecimento(string estabelecimentoId, bool ehParaAdicionar)
    {
        var estabelecimentos = FirebaseDatabase.DefaultInstance.GetReference("estabelecimentos/" + estabelecimentoId);

        if (ehParaAdicionar)
        {
            estabelecimentos.ChildAdded += itensLojaAdicionar;
            estabelecimentos.ChildChanged += itensLojaModificar;
            return;
        }

        if (!ehParaAdicionar)
        {
            estabelecimentos.ChildAdded -= itensLojaAdicionar;
            estabelecimentos.ChildChanged -= itensLojaModificar;
            AcaoItemLoja = null;
            return;
        }
    }
    #region Watch - configClienteAtual

    private void itensLojaAdicionar(object sender, ChildChangedEventArgs e)
    {
        if (e.DatabaseError != null)
        {
            Debug.LogError(e.DatabaseError.Message);
            return;
        }

        try
        {
            AcaoItemLoja(tratarSnapshot(e.Snapshot.Children), TipoAcao.Adicionar);
        }
        catch (Exception x)
        {
            Debug.LogError(x.Message);
            throw x;
        }
    }
    private void itensLojaModificar(object sender, ChildChangedEventArgs e)
    {
        if (e.DatabaseError != null)
        {
            Debug.LogError(e.DatabaseError.Message);
            return;
        }

        try
        {
            AcaoItemLoja(tratarSnapshot(e.Snapshot.Children), TipoAcao.Modificar);
        }
        catch (Exception x)
        {
            Debug.LogError(x.Message);
            throw x;
        }
    }

    private List<ItemLoja> tratarSnapshot(IEnumerable<DataSnapshot> dataSnapshots)
    {
        List<ItemLoja> lstItensLoja = new List<ItemLoja>();

        foreach (DataSnapshot ds in dataSnapshots)
        {
            lstItensLoja.Add(
                new ItemLoja
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
                }
            );
        }

        return lstItensLoja;
    }

    #endregion

}
