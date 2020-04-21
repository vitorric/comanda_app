using APIModel;
using Network;
using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DesafioConcluidoObj : MonoBehaviour
{
    public Text TxtTituloDesafio;
    public Text TxtPremioDesafio;
    public Text TxtGanhador;
    public Text TxtBtnResgatarPremio;
    public Button BtnResgatarPremio;
    public GameObject PnlDesafioConquistado;
    public Text TxtNomePremio;
    public RawImage Icon;
    public Texture2D ImgIconDinheiro;

    private DesafioCliente desafio;
    private bool ganhouPremioProduto = true;

    private void Awake()
    {
        BtnResgatarPremio.onClick.AddListener(() => resgatarPremio());
    }
    void OnEnable()
    {
        EasyAudioUtility.Instance.Play(EasyAudioUtility.Som.AchievWin);
    }

    #region PreencherInfo
    public void PreencherInfo(DesafioCliente desafio)
    {
        ganhouPremioProduto = true;
        TxtGanhador.text = string.Empty;
        TxtBtnResgatarPremio.text = "RESGATAR";
        this.desafio = desafio;
        TxtTituloDesafio.text = desafio.desafio.nome;
        TxtPremioDesafio.text = Util.FormatarValores(desafio.premio.quantidade);

        if (desafio.premio.produto == null)
        {
            TxtNomePremio.text = "CPGold";
            Icon.texture = ImgIconDinheiro;
        }
        else
        {
            TxtNomePremio.text = desafio.premio.produto.nome;
            
            Main.Instance.ObterIcones(desafio.premio.produto.icon, FileManager.Directories.produto, (textura) =>
            {
                Debug.Log(desafio.premio.produto.icon);
                if (textura != null)
                {
                    Icon.texture = textura;
                    Icon = Util.ImgResize(Icon, 180, 180);
                }
            });

            if (Cliente.ClienteLogado._id != desafio.premio.ganhador._id)
            {
                TxtGanhador.text = $"Ganhador: {desafio.premio.ganhador.nome}";
                TxtBtnResgatarPremio.text = "OK";
                ganhouPremioProduto = false;
            }
        }
    }
    #endregion

    #region resgatarPremio
    private void resgatarPremio()
    {
        EasyAudioUtility.Instance.Play(EasyAudioUtility.Som.AchievResgate);

        Dictionary<string, object> form = new Dictionary<string, object>()
        {
            { "desafioId", desafio.desafio._id },
            { "ganhouPremioProduto", ganhouPremioProduto }
        };

        StartCoroutine(DesafioAPI.ResgatarPremioDesafio(form, (response, error) =>
        {
            AppManager.Instance.RemoverDesafioDaLista(desafio);
            desafio = null;
            PnlDesafioConquistado.SetActive(false);

            if (AppManager.Instance.ObterTamanhoListaDesafio() > 0)
            {
                AppManager.Instance.ExibirProximoDesafio();
            }
        }));
    }
    #endregion
}
