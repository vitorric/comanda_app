﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace APIModel
{
    public class ItemLoja
    {
        public string _id;
        public string descricao;
        public bool hotSale;
        public string icon;
        public string nome;
        public double preco;
        public int quantidadeDisponivel;
        public int quantidadeVendida;
        public DateTime tempoDisponivel;
    }
}