using APIModel;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EstabelecimentoObj : MonoBehaviour
{
    [Header("Botoes")]
    public Button BtnAbrirInfo;
    public Button BtnAbrirInfoLoja;

    [Header("Text")]
    public Text TxtNome;
    public Text TxtAtendimento;
    public Text TxtPessoas;
    public Text TxtGold;

    public RawImage Icon;
    public GameObject PnlBloqueio;

    private Estabelecimento estabelecimento;

    #region PreencherInfo
    public void PreencherInfo(Estabelecimento estabelecimento, int gold)
    {
        try
        {
            this.estabelecimento = estabelecimento;
            TxtNome.text = estabelecimento.nome;
            TxtAtendimento.text = estabelecimento.horarioAtendimentoInicio + " às " + estabelecimento.horarioAtendimentoFim;
            TxtPessoas.text = estabelecimento.configEstabelecimentoAtual.clientesNoLocal.Count.ToString();
            TxtGold.text = Util.FormatarValores(gold);
            if (!estabelecimento.configEstabelecimentoAtual.estaAberta) PnlBloqueio.SetActive(true);
            configurarListener();
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }
    #endregion

    #region configurarListener
    private void configurarListener()
    {
        BtnAbrirInfo.onClick.AddListener(() => Main.Instance.MenuEstabelecimento.PreencherInfoEstabelecimento(estabelecimento));
        BtnAbrirInfoLoja.onClick.AddListener(() => Main.Instance.MenuEstabelecimento.PreencherInfoEstabelecimento(estabelecimento, 2));
    }
    #endregion

    #region PreencherIcone
    public void PreencherIcone(Texture2D icone)
    {
        Icon.texture = icone;
        Icon = Util.ImgResize(Icon, 180, 180);
    }
    #endregion
}
