using System;
using UnityEngine;

namespace APIModel
{
    public class HistoricoCompra : MonoBehaviour
    {
        public string _id;
        public int precoItem;
        public string createdAt;
        public Estabelecimento estabelecimento;
        public ItemLoja itemLoja;
        public Produto produto;
        public string chaveUnica;
        public string modoObtido;
        public int quantidade;
        public InfoEntrega infoEntrega;

        public class Estabelecimento
        {
            public string nome;
        }

        public class InfoEntrega
        {
            public bool jaEntregue;
            public string dataEntrega;
        }

        public class ItemLoja
        {
            public string nome;
            public string icon;
        }
        public class Produto
        {
            public string nome;
            public string icon;
        }
    }
}
