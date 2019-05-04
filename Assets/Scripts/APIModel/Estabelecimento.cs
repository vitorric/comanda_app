using System;
using System.Collections.Generic;

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
    public List<ItensLoja> itensLoja;
    public List<Conquista> conquistas;

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

    public partial class ItensLoja
    {
        public string _id;
        public int quantidadeDisponivel;
        public int quantidadeVendida;
        public bool hotSale;
        public DateTime tempoDisponivel;
        public Item item;
    }

    public partial class Item
    {
        public string _id;
        public string nome;
        public string descricao;
        public string icon;
        public double preco;
    }

    public partial class Conquista
    {
        public string _id;
        public string icon;
        public int premio;
        public int status;
        public string nome;
        public string descricao;
        public DateTime tempoDuracao;
        public string estabelecimento;
        public ConquistaObjetivo objetivo;
    }

    public partial class ConquistaObjetivo
    {
        public string tipo;
        public int quantidade;
        public string produto;
    }
}
