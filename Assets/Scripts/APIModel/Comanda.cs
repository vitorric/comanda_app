using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace APIModel
{
    public class Comanda
    {
        public string _id;
        public string estabelecimento;
        public double valorTotal;

        public class Grupo
        {
            public Cliente cliente;
            public bool jaPagou;
            public bool lider;
            public double valorPago;
            public DateTime avatarAlterado;
        }

        public class Produto
        {
            public InfoProduto infoProduto;
            public double preco;
            public int quantidade;
            public double precoTotal;
        }

        public class Cliente
        {
            public string _id;
            public string apelido;
            public string avatar;
            public string sexo;
        }
        
        public class InfoProduto
        {
            public string _id;
            public string icon;
            public string nome;
        }
    }
}
