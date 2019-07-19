using System;
using System.Collections.Generic;

namespace APIModel
{
    public class Correio
    {
        public enum TiposAcao
        {
            ConviteGrupo,
            ConviteAmizade,
            Premiacao
        }

        public string cliente;
        public List<Mensagem> correio;

        public class Mensagem
        {
            public string _id;
            public string titulo;
            public string mensagem;
            public string mensagemGrande;
            public bool lida;
            public DateTime dataCriacao;
            public Acao acao;
        }

        public class Acao
        {
            public string tipo;
            public bool executouAcao;
            public string comanda;
            public string cliente;
            public int dinheiro;
            public int exp;
            public int pontos;
        }
    }

}
