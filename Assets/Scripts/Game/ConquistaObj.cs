using APIModel;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConquistaObj : MonoBehaviour {

	public RawImage IconConquista;
	public Text TxtTituloConquista;

	[Header("Painel Comum")]
	public GameObject PnlConquista;
	public Text TxtDescricaoConquista;
	public Text TxtTempoRestante;
	public RawImage IconPremio;
	public Text TxtPremio;
	public Image BarraProgresso;
	public Text TxtProgresso;

	[Header("Painel Tempo Esgotado")]
	public GameObject PnlTempoEsgotado;

	[Header("Painel Conquista Concluida")]
	public GameObject PnlConquistaConcluida;
	public Text TxtDataConclusao;
	public RawImage IconPremioConclusao;
	public Text TxtPremioConclusao;
	private Estabelecimento.Conquista conquista;
	private bool pararConferenciaTempo = false;
	private Cliente.Conquista conquistaUsuario;

	void Update(){
		if (!pararConferenciaTempo){
			pararConferenciaTempo = !rodarRelogio();
			if (pararConferenciaTempo == false){
				TimeSpan data = conquista.tempoDuracao.ToLocalTime().Subtract((DateTime.Now.ToLocalTime()));
				TxtTempoRestante.text = string.Format("{0:00}:{1:00}:{2:00}",data.Hours + (data.Days * 24), data.Minutes, data.Seconds);
			}else{
				pararConferenciaTempo = true;
				configurarPainelAlerta();
			}
		}
	}

	public void PreencherInfo(Estabelecimento.Conquista conquista, Cliente.Conquista conquistaUsuario, string _idEstabelecimento){
		this.conquista = conquista;		
		TxtTituloConquista.text = conquista.nome;		
		TxtDescricaoConquista.text = conquista.descricao;
		TxtPremio.text = conquista.premio.ToString();

		int progressoUsuario = 0;
		
		if (conquistaUsuario != null){
			this.conquistaUsuario = conquistaUsuario;
			progressoUsuario = conquistaUsuario.quantidadeParaObter;
		}

		BarraProgresso.transform.localScale = new Vector3(Mathf.FloorToInt((float) progressoUsuario / (float) conquista.objetivo.quantidade / 100f),1,1);

		TxtProgresso.text = progressoUsuario + "/" + conquista.objetivo.quantidade;

		configurarPainelAlerta();
	}

	public bool rodarRelogio(){
		return (conquista.tempoDuracao.ToLocalTime().Subtract((DateTime.Now.ToLocalTime())).TotalSeconds > 0) ? true : false;
	}

	private void configurarPainelAlerta(){
		if (pararConferenciaTempo) {
			PnlConquista.SetActive(false);
			PnlTempoEsgotado.SetActive(true);
		} else if (conquistaUsuario != null && conquistaUsuario.concluido){
			PnlConquista.SetActive(false);
			PnlConquistaConcluida.SetActive(true);
			TxtDataConclusao.text = conquistaUsuario.dataConclusao.ToShortDateString();
			TxtPremioConclusao.text = conquista.premio.ToString();
		}
	}
}
