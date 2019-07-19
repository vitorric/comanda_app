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
    public List<GameObject> lstImgGrupos;

    public RawImage IconPremio;
    public Text TxtPremio;

    public GameObject PnlBarraProgresso;
    public Slider BarraProgresso;
    public Text TxtProgresso;

    [Header("Painel Resgatar Premio")]
    public GameObject PnlResgatarPremio;
    public Text TxtResgatarPremio;

    [Header("Painel Tempo Esgotado")]
    public GameObject PnlTempoEsgotado;

    [Header("Painel Conquista Concluida")]
    public GameObject PnlConquistaConcluida;
    public Text TxtDataConclusao;


    //private Estabelecimento.Conquista conquista;
    private bool pararConferenciaTempo = true;

    //private Cliente.Conquista conquistaUsuario;
    public Desafio Desafio;
    public Cliente.Desafio DesafioCliente;

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

    #region rodarRelogio
    void rodarRelogio()
    {
        TimeSpan ts = Desafio.tempoDuracao.ToLocalTime().Subtract((DateTime.Now.ToLocalTime()));

        if (ts.TotalSeconds <= 0)
        {
            pararConferenciaTempo = true;
            configurarPainelAlerta();
        }
        else
        {
            TxtTempoRestante.text = string.Format("{0:00}:{1:00}:{2:00}", ts.Hours + (ts.Days * 24), ts.Minutes, ts.Seconds);
            Invoke("rodarRelogio", 1f);
        }
    }
    #endregion

    #region PreencherInfo
    public void PreencherInfo(Desafio desafio, Cliente.Desafio desafioCliente)
    {
        Desafio = desafio;
        DesafioCliente = desafioCliente;

        TxtTituloConquista.text = desafio.nome;
        TxtDescricaoConquista.text = desafio.descricao;
        TxtPremio.text = desafio.premio.ToString();

        lstImgGrupos.ForEach(x => x.SetActive(desafio.emGrupo));

        int progressoUsuario = 0;

        if (DesafioCliente != null)
        {
            progressoUsuario = DesafioCliente.progresso;

            if (DesafioCliente.concluido)
            {
                esconderTodosPaineis();

                if (DesafioCliente.resgatouPremio)
                {
                    PnlResgatarPremio.SetActive(true);
                    TxtResgatarPremio.text = Util.FormatarValores(desafio.premio);
                }
                else
                {
                    PnlConquistaConcluida.SetActive(true);
                }
            }
            else
            {
                esconderTodosPaineis();
                PnlConquista.SetActive(true);
            }
        }

        BarraProgresso.value = (float)progressoUsuario / (float)desafio.objetivo.quantidade;

        TxtProgresso.text = progressoUsuario + "/" + desafio.objetivo.quantidade;

        rodarRelogio();

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
        //else if (conquistaUsuario != null && conquistaUsuario.concluido)
        //{
        //    PnlConquista.SetActive(false);
        //    PnlConquistaConcluida.SetActive(true);
        //    TxtDataConclusao.text = conquistaUsuario.dataConclusao.ToShortDateString();
        //}
    }
    #endregion

    #region abrirPnlInfoDesafio
    private void abrirPnlInfoDesafio()
    {
        PnlPopUp.AbrirPopUpCanvas(
            Main.Instance.MenuEstabelecimento.CanvasDesafioInfo, 
            Main.Instance.MenuEstabelecimento.DesafioInfo.gameObject, () => {
                Main.Instance.MenuEstabelecimento.DesafioInfo.PreencherInfo(Desafio.premio, Desafio.icon);
            });

    }
    #endregion

    #region esconderTodosPaineis
    private void esconderTodosPaineis()
    {
        PnlConquista.SetActive(false);
        PnlResgatarPremio.SetActive(false);
        PnlConquistaConcluida.SetActive(false);
    }
    #endregion

}
