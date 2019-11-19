using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIModel
{
    public class DesafioCliente
    {
        public string _id;
        public string cliente;
        public int progresso;
        public bool concluido;
        public bool resgatouPremio;
        public string estabelecimento;
        public DateTime dataConclusao;
        public Desafio desafio;
        public Premio premio;

        public partial class Desafio
        {
            public string _id;
            public string nome;
            public string icon;
            public DateTime tempoDuracao;
        }

        public partial class Premio
        {
            public string tipo;
            public int quantidade;
            public Produto produto;
            public Ganhador ganhador;
        }

        public partial class Ganhador
        {
            public string _id;
            public string nome;
        }

        public partial class Produto
        {
            public string _id;
            public string nome;
            public string icon;
        }
    }
}
