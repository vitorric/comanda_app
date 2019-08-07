using APIModel;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class APIManager : MonoBehaviour
{
    public static APIManager Instance { get; set; }

    private readonly string urlBase = "http://localhost:3000/api/";
    //private readonly string urlBase = "http://93.188.164.122:3000/api/";


    public enum URLs
    {
        ClienteLogin,
        ClienteRecuperarSenha,
        ClienteCadastrar,
        ClienteAlterarConfigApp,
        ClienteAlterar,
        ClienteComprarItem,
        ListarEstabelecimento,
        ObterEstabelecimento,
        ListarHistoricoCompra,
        AvatarAlterar,
        EntrarNoEstabelecimento,
        SairDoEstabelecimento,
        RecusarConviteEstabelecimento,
        ListarClienteConquistas
    }


    private Dictionary<URLs, string> uRls;
    
    void Awake()
    {
        if (Instance != null)
            Destroy(this);

        DontDestroyOnLoad(gameObject);
        Instance = this;

        preencherURLs();
    }

    private void preencherURLs()
    {
        uRls = new Dictionary<URLs, string>()
        {
            { URLs.ClienteLogin, urlBase + "login/cliente" },
            { URLs.ClienteRecuperarSenha, urlBase + "recuperarSenha/cliente" },
            { URLs.ClienteCadastrar, urlBase + "cadastrar/cliente" },
            { URLs.ClienteAlterarConfigApp, urlBase + "alterarConfigApp/cliente" },
            { URLs.ClienteAlterar, urlBase + "alterar/cliente" },
            { URLs.ClienteComprarItem, urlBase + "compraritem/cliente" },
            { URLs.ListarEstabelecimento, urlBase + "listar/estabelecimento/cliente" },
            { URLs.ObterEstabelecimento, urlBase + "obter/estabelecimento/cliente" },
            { URLs.ListarHistoricoCompra, urlBase + "listar/cliente/historico/compra" },
            { URLs.AvatarAlterar, urlBase + "alterar/cliente/avatar" },
            { URLs.EntrarNoEstabelecimento, urlBase + "entrarestabelecimento/cliente" },
            { URLs.SairDoEstabelecimento, urlBase + "sairestabelecimento/cliente" },
            { URLs.ListarClienteConquistas, urlBase + "listar/cliente/conquistas" },
            { URLs.RecusarConviteEstabelecimento, urlBase + "recusar_convite_estabelecimento/cliente" },
        };
    }

    public partial class Retorno<T>
    {
        public bool sucesso;
        public string msg;
        public T retorno;
    }
}
