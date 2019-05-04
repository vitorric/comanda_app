using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EstabelecimentoObj : MonoBehaviour {

	public Text TxtNome;
	public Text TxtAtendimento;
	public Text TxtPessoas;
	public Text TxtGold;
	public GameObject PnlBloqueio;

	private Estabelecimento estabelecimento;

	public void PreencherInfo(Estabelecimento estabelecimento, int gold){
		this.estabelecimento = estabelecimento;

		TxtNome.text = estabelecimento.nome;
		TxtAtendimento.text = estabelecimento.horarioAtendimentoInicio + " às " + estabelecimento.horarioAtendimentoFim;
		TxtPessoas.text = estabelecimento.configEstabelecimentoAtual.clientesNoLocal.Count.ToString();
		TxtGold.text = Util.FormatarValorDisponivel(gold);
		if (!estabelecimento.configEstabelecimentoAtual.estaAberta) PnlBloqueio.SetActive(true);
	}

	public void BtnAbrirPnlLoja(){
		SomController.Tocar(SomController.Som.Click_OK);
		AnimacoesTween.AnimarObjeto(EventSystem.current.currentSelectedGameObject, AnimacoesTween.TiposAnimacoes.SubMenu_Click, () => 
		{
			print(estabelecimento._id);
		},
		0.1f);
	}

	public void BtnAbrirPnlInfo(){
		SomController.Tocar(SomController.Som.Click_OK);
		AnimacoesTween.AnimarObjeto(EventSystem.current.currentSelectedGameObject, AnimacoesTween.TiposAnimacoes.Button_Click, () => 
		{
            Main.Instance.MenuEstabelecimento.PreencherInfoEstabelecimento(estabelecimento, true);
		});
	}
}
