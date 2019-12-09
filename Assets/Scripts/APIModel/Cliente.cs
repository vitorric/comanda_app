using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace APIModel
{
    public class Cliente
    {
        public static Dados ClienteLogado;

        public class SessaoCliente
        {
            public string token;
            public string _id;
        }
        public class Credenciais
        {
            public string email;
            public string password;
            public string tipoLogin;
        }

        public partial class Dados
        {
            public string _id;
            public string email;
            public string nome;
            public string apelido;
            public string password;
            public string chaveAmigavel;
            public DateTime? dataNascimento;
            public string cpf;
            public string sexo;
            public int goldGeral;
            public int pontos;
            public bool concluiuTutorialGeral;
            public bool concluiuTutorialProfile;
            public bool concluiuTutorialCorreio;
            public bool concluiuTutorialDesafios;
            public List<GoldPorEstabelecimento> goldPorEstabelecimento;
            public Endereco endereco;
            public Avatar avatar;
            public ConfigApp configApp;
            public ConfigClienteAtual configClienteAtual;

            public int RetornarPctExp()
            {
                return Mathf.FloorToInt(((float)this.avatar.info.exp / (float)this.avatar.info.expProximoLevel) * 100f);
            }

            public int RetornarGoldTotal()
            {
                int goldTotal = this.goldGeral;

                if (this.goldPorEstabelecimento != null)
                    this.goldPorEstabelecimento.ForEach(x => goldTotal += x.gold);

                return goldTotal;
            }

            public int RetornoGoldEstabelecimento(string idEstabelecimento)
            {
                if (this.goldPorEstabelecimento != null)
                {
                    GoldPorEstabelecimento goldEstabelecimento = this.goldPorEstabelecimento.Find(x => x.estabelecimento == idEstabelecimento);

                    return (goldEstabelecimento != null) ? goldEstabelecimento.gold : 0;
                }

                return 0;
            }

            //public int AlterarGoldEstabelecimento(string idEstabelecimento, int quantidadeGold, bool adicionar)
            //{
            //    if (adicionar)
            //        this.goldPorEstabelecimento.Find(x => x.estabelecimento == idEstabelecimento).gold += quantidadeGold;
            //    else
            //        this.goldPorEstabelecimento.Find(x => x.estabelecimento == idEstabelecimento).gold -= quantidadeGold;

            //    return RetornoGoldEstabelecimento(idEstabelecimento);
            //}
        }

        public partial class Avatar
        {
            public string _id;
            public AvatarInfo info;
            public string corpo;
            public string cabeca;
            public string nariz;
            public string olhos;
            public string boca;
            public string roupa;
            public string cabeloTraseiro;
            public string cabeloFrontal;
            public string barba;
            public string sombrancelhas;
            public string orelha;
            public string corPele;
            public string corCabelo;
            public string corBarba;
        }

        public partial class AvatarInfo
        {
            public int level;
            public double exp;
            public double expProximoLevel;
        }

        public partial class Endereco
        {
            public string rua;
            public int numero;
            public string bairro;
            public string cidade;
            public string cep;
            public string estado;
        }

        public partial class ConfigApp
        {
            public float somFundo;
            public float somGeral;
        }

        public partial class GoldPorEstabelecimento
        {
            public string estabelecimento;
            public int gold;
        }

        public partial class ConfigClienteAtual
        {
            public bool estaEmUmEstabelecimento;
            public string estabelecimento;
            public string nomeEstabelecimento;
            public bool conviteEstabPendente;
            public string comanda;
        }
    }
}