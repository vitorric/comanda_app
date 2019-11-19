using APIModel;
using Network;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DesafioInfo : MonoBehaviour
{
    public Button BtnFechar;
    public GameObject PnlInfo;
    public Text TxtValorPremio;
    public Text TxtNomePremio;
    public Texture2D ImgIconDinheiro;
    public RawImage IconPremio;

    private void Awake()
    {
        configurarListener();
    }

    #region configurarListener
    private void configurarListener()
    {
        BtnFechar.onClick.AddListener(() => PnlPopUp.FecharPopUpSemDesligarPopUP(PnlInfo, () => this.gameObject.SetActive(false)));
    }
    #endregion

    #region PreencherInfo
    public void PreencherInfo(Desafio.Premio premio)
    {
        this.gameObject.SetActive(true);
        PnlInfo.SetActive(true);
        TxtValorPremio.text = Util.FormatarValores(premio.quantidade);

        if (string.IsNullOrEmpty(premio.produto))
        {
            TxtNomePremio.text = "CPGold";
            IconPremio.texture = ImgIconDinheiro;

            animarPnlInfo();
        }
        else
        {
            Dictionary<string, object> form = new Dictionary<string, object>()
            {
                { "produtoId", premio.produto }
            };

            StartCoroutine(ProdutoAPI.ObterProdutoCliente(form, 
            (response, error) =>
            {
                TxtNomePremio.text = response.nome;

                Main.Instance.ObterIcones(response.icon, FileManager.Directories.produto, (textura) =>
                {
                    if (textura = null)
                    {
                        IconPremio.texture = textura;
                        IconPremio = Util.ImgResize(IconPremio, 180, 180);
                    }

                    animarPnlInfo();
                });
            }));
        }
    }
    #endregion

    #region animarPnlInfo
    private void animarPnlInfo()
    {

        AnimacoesTween.AnimarObjeto(PnlInfo,
                    AnimacoesTween.TiposAnimacoes.Scala,
                    null,
                    AppManager.TEMPO_ANIMACAO_ABRIR_MODEL,
                    Vector2.one);
    }
    #endregion
}
