using APIModel;
using Network;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DesafioConcluidoObj : MonoBehaviour
{
    public Text TxtTituloDesafio;
    public Text TxtPremioDesafio;
    public Button BtnResgatarPremio;
    public GameObject PnlDesafioConquistado;
    public Text TxtNomePremio;
    public RawImage Icon;
    public Texture2D ImgIconDinheiro;

    private Desafio desafio;

    private void Awake()
    {
        BtnResgatarPremio.onClick.AddListener(() => resgatarPremio());
    }
    void OnEnable()
    {
        EasyAudioUtility.Instance.Play(EasyAudioUtility.Som.AchievWin);
    }

    #region PreencherInfo
    public void PreencherInfo(Desafio desafio)
    {
        this.desafio = desafio;
        TxtTituloDesafio.text = desafio.nome;
        TxtPremioDesafio.text = Util.FormatarValores(desafio.premio.quantidade);

        if (string.IsNullOrEmpty(desafio.premio.produto))
        {
            TxtNomePremio.text = "CPGold";
            Icon.texture = ImgIconDinheiro;
        }
        else
        {
            Dictionary<string, object> form = new Dictionary<string, object>()
            {
                { "produtoId", desafio.premio.produto }
            };

            StartCoroutine(ProdutoAPI.ObterProdutoCliente(form,
            (response, error) =>
            {
                TxtNomePremio.text = response.nome;

                Main.Instance.ObterIcones(response.icon, FileManager.Directories.produto, (textura) =>
                {
                    Icon.texture = textura;
                    Icon = Util.ImgResize(Icon, 180, 180);
                });
            }));
        }
    }
    #endregion

    #region resgatarPremio
    private void resgatarPremio()
    {
        EasyAudioUtility.Instance.Play(EasyAudioUtility.Som.AchievResgate);

        Dictionary<string, object> form = new Dictionary<string, object>()
        {
            { "desafioId", desafio._id }
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
