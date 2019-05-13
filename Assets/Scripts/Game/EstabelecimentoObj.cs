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
            TxtGold.text = Util.FormatarValorDisponivel(gold);
            if (!estabelecimento.configEstabelecimentoAtual.estaAberta) PnlBloqueio.SetActive(true);
            else configurarListener();
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
        BtnAbrirInfo.onClick.AddListener(() => Main.Instance.MenuEstabelecimento.PreencherInfoEstabelecimento(estabelecimento, true));
        BtnAbrirInfoLoja.onClick.AddListener(() => print(estabelecimento._id));
    }
    #endregion

}
