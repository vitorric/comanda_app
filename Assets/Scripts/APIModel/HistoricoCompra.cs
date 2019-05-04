using System;
using UnityEngine;

public class HistoricoCompra : MonoBehaviour
{
    public string _id;
    public int precoItem;
    public DateTime createdAt;
    public Estabelecimento estabelecimento;
    public ItemLoja itemLoja;
    public InfoEntrega infoEntrega;

    public class Estabelecimento
    {
        public string nome;
    }

    public class InfoEntrega
    {
        public bool jaEntregue;
        public DateTime dataEntrega;
    }

    public class ItemLoja
    {
        public string nome;
    }
}
