using Newtonsoft.Json;
using System.Collections;
using UnityEngine;
using APIModel;
using System.Collections.Generic;

public class AppManager : MonoBehaviour
{
    public static AppManager Instance { get; set; }

    public const float TEMPO_ANIMACAO_ABRIR_MODEL = 0.2f;
    public const float TEMPO_ANIMACAO_FECHAR_MODAL = 0.1f;
    public const float TEMPO_ANIMACAO_ABRIR_CLICK_BOTAO = 0.1f;

    public GameObject Loader;
    public GameObject LevelUp;
    public GameObject PnlDesafioConquistado;
    public DesafioConcluidoObj DesafioConquistado;

    private List<Desafio> lstDesafiosCompletados;

    // Update is called once per frame
    void Awake()
    {
        if (Instance != null)
            Destroy(this);

        DontDestroyOnLoad(gameObject);
        Instance = this;

        lstDesafiosCompletados = new List<Desafio>();
    }

    #region Loader
    public void AtivarLoader()
    {
        Loader.SetActive(true);
    }

    public IEnumerator DesativarLoader()
    {
        yield return new WaitForSeconds(0.5f);
        Loader.SetActive(false);
    }
    public void DesativarLoaderAsync()
    {
        Loader.SetActive(false);
    }
    #endregion

    #region LevelUp
    public void AtivarLevelUp()
    {
        LevelUp.SetActive(true);
    }
    public void DesativarLevelUp()
    {
        LevelUp.SetActive(false);
    }
    #endregion

    #region Desafio
    public void AtivarDesafioCompletado(Desafio desafio)
    {
        if (lstDesafiosCompletados.Count == 0)
        {
            lstDesafiosCompletados.Add(desafio);
            ExibirProximoDesafio();
            return;
        }

        lstDesafiosCompletados.Add(desafio);
    }    

    public void ExibirProximoDesafio()
    {
        PnlDesafioConquistado.SetActive(true);
        DesafioConquistado.PreencherInfo(lstDesafiosCompletados[0]);
    }

    public void RemoverDesafioDaLista(Desafio desafio)
    {
        lstDesafiosCompletados.Remove(desafio);
    }

    public int ObterTamanhoListaDesafio()
    {
        return lstDesafiosCompletados.Count;
    }

    #endregion

    #region Sessao e Token
    public void GravarSession(string session, string _id, string credenciais)
    {
        PlayerPrefs.SetString("session_token_cliente", "Bearer " + session);
        PlayerPrefs.SetString("session_cliente", _id);
        PlayerPrefs.SetString("credenciais_cliente", credenciais);
    }

    public void RefazerToken(string session)
    {
        PlayerPrefs.SetString("session_token_cliente", "Bearer " + session);
    }

    public string ObterToken()
    {
        return PlayerPrefs.GetString("session_token_cliente");
    }

    public string Obter()
    {
        return PlayerPrefs.GetString("session_cliente");
    }

    public Cliente.Credenciais ObterCredenciais()
    {
        return JsonConvert.DeserializeObject<Cliente.Credenciais>(PlayerPrefs.GetString("credenciais_cliente"));
    }

    public bool EstaLogado()
    {
        if (PlayerPrefs.HasKey("session_token_cliente"))
            return true;

        return false;
    }
    #endregion
}
