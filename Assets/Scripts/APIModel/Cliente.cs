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
        }

        public partial class Dados
        {
            public string _id;
            public string email;
            public string nome;
            public string apelido;
            public string password;
            public DateTime dataNascimento;
            public string cpf;
            public string sexo;
            public int goldGeral;
            public int pontos;
            public List<GoldPorEstabelecimento> goldPorEstabelecimento;
            public Endereco endereco;
            public Avatar avatar;
            public ConfigApp configApp;
            public ConfigClienteAtual configClienteAtual;
            public List<Conquista> conquistas;

            public bool AdicionarExp(int exp)
            {
                this.avatar.exp += exp;
                return subiuLevel();
            }

            private bool subiuLevel()
            {
                if (this.avatar.exp >= this.avatar.expProximoLevel)
                {
                    this.avatar.level += 1;
                    this.avatar.exp = this.avatar.exp - this.avatar.expProximoLevel;
                    ConfigurarExpProLevel();
                    return true;
                }

                return false;
            }

            public void ConfigurarExpProLevel()
            {
                Configuracoes configApp = new Configuracoes();
                this.avatar.expProximoLevel = configApp.levelSystem.ExpProximoLevel(this.avatar.level);
            }

            public int AtualizarPctExp()
            {
                return Mathf.FloorToInt(((float)this.avatar.exp / (float)this.avatar.expProximoLevel) * 100f);
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
                    Debug.Log(goldEstabelecimento);
                    return (goldEstabelecimento != null) ? goldEstabelecimento.gold : 0;
                }

                return 0;
            }

            public int AlterarGoldEstabelecimento(string idEstabelecimento, int quantidadeGold, bool adicionar)
            {
                if (adicionar)
                    this.goldPorEstabelecimento.Find(x => x.estabelecimento == idEstabelecimento).gold += quantidadeGold;
                else
                    this.goldPorEstabelecimento.Find(x => x.estabelecimento == idEstabelecimento).gold -= quantidadeGold;

                return RetornoGoldEstabelecimento(idEstabelecimento);
            }
        }

        public partial class Avatar
        {
            public string _id;
            public int level;
            public int exp;
            public int expProximoLevel;
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
        }

        public partial class Conquista
        {
            public string conquista;
            public string estabelecimento;
            public int quantidadeParaObter;
            public bool concluido;
            public DateTime dataConclusao;

        }

        public static void GravarSession(string session, string _id, string credenciais)
        {
            PlayerPrefs.SetString("session_token_cliente", session);
            PlayerPrefs.SetString("session_cliente", _id);
            PlayerPrefs.SetString("credenciais_cliente", credenciais);
        }

        public static void RefazerToken(string session)
        {
            PlayerPrefs.SetString("session_token_cliente", session);
        }

        public static string ObterToken()
        {
            return PlayerPrefs.GetString("session_token_cliente");
        }

        public static string Obter()
        {
            return PlayerPrefs.GetString("session_cliente");
        }

        public static Credenciais ObterCredenciais()
        {
            return JsonConvert.DeserializeObject<Credenciais>(PlayerPrefs.GetString("credenciais_cliente"));
        }

        public static bool EstaLogado()
        {
            if (PlayerPrefs.HasKey("session_token_cliente"))
                return true;

            return false;
        }
    }
}