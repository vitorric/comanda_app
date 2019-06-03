using APIModel;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DesafioObj : MonoBehaviour
{

    public RawImage IconConquista;
    public Text TxtTituloConquista;

    [Header("Painel Comum")]
    public GameObject PnlConquista;
    public Text TxtDescricaoConquista;
    public Text TxtTempoRestante;
    public Button BtnAbrirDesafioInfo;

    public RawImage IconPremio;
    public Text TxtPremio;

    public GameObject PnlBarraProgresso;
    public Slider BarraProgresso;
    public Text TxtProgresso;

    [Header("Painel Tempo Esgotado")]
    public GameObject PnlTempoEsgotado;

    [Header("Painel Conquista Concluida")]
    public GameObject PnlConquistaConcluida;
    public Text TxtDataConclusao;


    //private Estabelecimento.Conquista conquista;
    private bool pararConferenciaTempo = false;
    private Cliente.Conquista conquistaUsuario;

    private void Awake()
    {
        configurarListener();
    }

    #region configurarListener
    private void configurarListener()
    {
        BtnAbrirDesafioInfo.onClick.AddListener(() => abrirPnlInfoDesafio());
    }
    #endregion

    #region Update
    void Update()
    {
        //if (!pararConferenciaTempo)
        //{
        //    pararConferenciaTempo = !rodarRelogio();
        //    if (pararConferenciaTempo == false)
        //    {
        //        TimeSpan data = conquista.tempoDuracao.ToLocalTime().Subtract((DateTime.Now.ToLocalTime()));
        //        TxtTempoRestante.text = string.Format("{0:00}:{1:00}:{2:00}", data.Hours + (data.Days * 24), data.Minutes, data.Seconds);
        //    }
        //    else
        //    {
        //        pararConferenciaTempo = true;
        //        configurarPainelAlerta();
        //    }
        //}
    }
    #endregion

    #region PreencherInfo
    public void PreencherInfo(Desafio conquista, Cliente.Conquista conquistaUsuario, string _idEstabelecimento)
    {
        //this.conquista = conquista;
        //TxtTituloConquista.text = conquista.nome;
        //TxtDescricaoConquista.text = conquista.descricao;
        //TxtPremio.text = conquista.premio.ToString();

        //int progressoUsuario = 0;

        //if (conquistaUsuario != null)
        //{
        //    this.conquistaUsuario = conquistaUsuario;
        //    progressoUsuario = conquistaUsuario.quantidadeParaObter;
        //}

        //BarraProgresso.value = (float)progressoUsuario / (float)conquista.objetivo.quantidade / 100f;

        //TxtProgresso.text = progressoUsuario + "/" + conquista.objetivo.quantidade;

        //configurarPainelAlerta();
    }
    #endregion

    #region rodarRelogio
    public bool rodarRelogio()
    {
        //return (conquista.tempoDuracao.ToLocalTime().Subtract((DateTime.Now.ToLocalTime())).TotalSeconds > 0) ? true : false;
        return true;
    }
    #endregion

    #region configurarPainelAlerta
    private void configurarPainelAlerta()
    {
        if (pararConferenciaTempo)
        {
            PnlConquista.SetActive(false);
            PnlTempoEsgotado.SetActive(true);
        }
        else if (conquistaUsuario != null && conquistaUsuario.concluido)
        {
            PnlConquista.SetActive(false);
            PnlConquistaConcluida.SetActive(true);
            TxtDataConclusao.text = conquistaUsuario.dataConclusao.ToShortDateString();
        }
    }
    #endregion

    #region abrirPnlInfoDesafio
    private void abrirPnlInfoDesafio()
    {
        PnlPopUp.AbrirPopUp(Main.Instance.MenuEstabelecimento.DesafioInfo.gameObject,
         () =>
         {
             //Main.Instance.MenuEstabelecimento.DesafioInfo.PreencherInfo(conquista.premio, conquista.icon);
             Main.Instance.MenuEstabelecimento.DesafioInfo.PreencherInfo(500, "");
         });
    }
    #endregion

}
