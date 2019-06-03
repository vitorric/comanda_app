using APIModel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HistoricoCompraObj : MonoBehaviour {

	public Text TxtNomeItem;
	public Text TxtNomeEstabelecimento;
	public Text TxtDataCompra;
	public Text TxtCusto;
	public Text TxtStatusEntrega;
	public List<Color> corStatus;

	public void PreencherInfo(HistoricoCompra historico){
		TxtNomeItem.text = historico.itemLoja.nome;
		TxtNomeEstabelecimento.text = historico.estabelecimento.nome;
		TxtDataCompra.text = historico.createdAt.ToString("dd/MM/yyyy hh:mm");
		TxtCusto.text = "- " + historico.precoItem;

		if (historico.infoEntrega.jaEntregue){
			TxtStatusEntrega.text = "Entregue";
			TxtStatusEntrega.color = corStatus[0];
		}
		else
		{
			TxtStatusEntrega.text = "Pendente";
			TxtStatusEntrega.color = corStatus[1];
		}
		
	}
}
