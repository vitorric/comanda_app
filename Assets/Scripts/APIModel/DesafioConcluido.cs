using System;
using System.Collections.Generic;

namespace APIModel
{
    public class DesafioConcluido
    {

        public List<InfoDesafio> desafios;

        public class InfoDesafio
        {
            public Estabelecimento estabelecimento;
            public string dataConclusao;
            public Desafio desafio;
        }

        public class Desafio
        {
            public string _id;
            public string nome;
            public string icon;
            public string descricao;
            public Premio premio;
            public Objetivo objetivo;
        }

        public class Estabelecimento
        {
            public string _id;
            public string nome;
        }

        public class Premio
        {
            public string tipo;
            public int quantidade;
            public Produto produto;
        }

        public class Objetivo
        {
            public string tipo;
            public int quantidade;
            public Produto produto;
        }

        public class Produto
        {
            public string _id;
            public string icon;
            public string nome;
        }
    }
}