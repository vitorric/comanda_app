using System;
using System.Collections.Generic;

namespace APIModel
{
    public class Estabelecimento
    {

        public string _id;
        public bool status;
        public string tipo;
        public string nome;
        public string descricao;
        public string emailContato;
        public string telefone;
        public string celular;
        public string horarioAtendimentoInicio;
        public string horarioAtendimentoFim;
        public Endereco endereco;
        public ConfigEstabelecimentoAtual configEstabelecimentoAtual;

        public partial class ConfigEstabelecimentoAtual
        {
            public bool estaAberta;
            public List<string> clientesNoLocal;
        }

        public partial class Endereco
        {
            public string rua;
            public int numero;
            public string bairro;
            public string cidade;
            public string estado;
        }
    }
}
