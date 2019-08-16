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
        public int pontos;
        public int status;
        public int statusFirebase;
        public string nome;
        public string descricao;
        public DateTime tempoDuracao;
        public string estabelecimento;
        public bool emGrupo;
        public Objetivo objetivo;
        public Premio premio;

        public partial class Premio
        {
            public string tipo;
            public int quantidade;
            public string produto;
        }

        public partial class Objetivo
        {
            public string tipo;
            public int quantidade;
            public string produto;
        }
    }
}