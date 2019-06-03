using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace APIModel
{
    public class Desafio
    {
        public string _id;
        public string icon;
        public int premio;
        public int status;
        public string nome;
        public string descricao;
        public DateTime tempoDuracao;
        public string estabelecimento;
        public Objetivo objetivo;

        public partial class Objetivo
        {
            public string tipo;
            public int quantidade;
            public string produto;
        }
    }
}