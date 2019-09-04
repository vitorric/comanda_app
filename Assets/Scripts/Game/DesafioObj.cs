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
    public Text TxtPontos;
    public Text txtNomePremio;


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
            //TxtTempoRestante.text = string.Format("{0:00}:{1:00}:{2:00}", ts.Hours + (ts.Days * 24), ts.Minutes, ts.Seconds);
            if (ts.Days > 0)
            {
                TxtTempoRestante.text = string.Format("{0:0}d {1:0}h", ts.Days, ts.Hours);
            }
            else if (ts.Days == 0 && ts.Hours > 0)
            {
                TxtTempoRestante.text = string.Format("{0:0}h {1:0}m", ts.Hours, ts.Minutes);
            }
            else if (ts.Hours == 0 && ts.Minutes > 0)
            {
                TxtTempoRestante.text = string.Format("{0:0}m", ts.Minutes);
            }
            else if (ts.Minutes == 0)
            {
                TxtTempoRestante.text = string.Format("{0:0}s", ts.Seconds);
            }

            Invoke("rodarRelogio", 1f);
        }
    }
    #endregion

    #region PreencherInfo
    public void PreencherInfo(Desafio desafio, Cliente.Desafio desafioCliente)
    {
        Desafio = desafio;

        TxtTituloConquista.text = desafio.nome;
        TxtDescricaoConquista.text = desafio.descricao;

        lstImgGrupos.ForEach(x => x.SetActive(desafio.emGrupo));
        AtualizarProgresso(desafioCliente);
        rodarRelogio();
        //obterIcone();
    }
    #endregion

    #region PreencherIcone
    public void PreencherIcone(Texture2D icone)
    {
        IconConquista.texture = icone;
        IconConquista = Util.ImgResize(IconConquista, 180, 180);
    }
    #endregion

    #region PreencherInfoConcluido
    public void PreencherInfoConcluido(DesafioConcluido.InfoDesafio desafioConcluido)
    {
        esconderTodosPaineis();
        PnlConquistaConcluida.SetActive(true);

        TxtTituloConquista.text = desafioConcluido.desafio.nome;
        TxtDescricaoConquista.text = desafioConcluido.desafio.descricao;
        txtNomePremio.text =
            (desafioConcluido.desafio.premio.produto == null) ?
                $"{desafioConcluido.desafio.premio.quantidade} x CPGold" :
                $"{desafioConcluido.desafio.premio.quantidade} x {desafioConcluido.desafio.premio.produto.nome}";
        TxtDataConclusao.text = desafioConcluido.dataConclusao;


        Main.Instance.ObterIcones(desafioConcluido.desafio.icon, FileManager.Directories.desafio, (textura) =>
        {
            PreencherIcone(textura);
        });
    }
    #endregion

    #region AtualizarProgresso
    public void AtualizarProgresso(Cliente.Desafio desafioCliente)
    {
        DesafioCliente = desafioCliente;

        int progressoUsuario = 0;
        if (DesafioCliente != null)
        {
            progressoUsuario = DesafioCliente.progresso;

            if (DesafioCliente.concluido)
            {
                esconderTodosPaineis();
                PnlConquista.SetActive(true);
            }
        }

        BarraProgresso.value = (float)progressoUsuario / (float)Desafio.objetivo.quantidade;

        TxtProgresso.text = progressoUsuario + "/" + Desafio.objetivo.quantidade;
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
    }
    #endregion

    #region abrirPnlInfoDesafio
    private void abrirPnlInfoDesafio()
    {
        PnlPopUp.AbrirPopUpCanvas(
            Main.Instance.MenuEstabelecimento.CanvasDesafioInfo,
            Main.Instance.MenuEstabelecimento.DesafioInfo.gameObject, () =>
            {
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
