using APIModel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HistoricoCompraObj : MonoBehaviour {

	public Text TxtNomeItem;
    public Text txtQuantidade;
	public Text TxtNomeEstabelecimento;
	public Text TxtDataCompra;
    public Text txtModoObtido;
    public Text txtChaveUnica;    
	public Text TxtStatusEntrega;

    public RawImage ImgIcon;
    public Text TxtTipoHistorico;
    public GameObject PnlCustoItem;
    public Text TxtCusto;

    public void PreencherInfo(HistoricoCompra historico)
    {
        bool ehItemLoja = historico.itemLoja != null ? true : false;

        txtQuantidade.text = $"<color=orange>{historico.quantidade}x</color>";
		TxtNomeEstabelecimento.text = $"Estabelecimento: {historico.estabelecimento.nome}";
		TxtDataCompra.text = $"Data: {historico.createdAt}";
        txtModoObtido.text = $"Modo Obtido: <color=cyan>{historico.modoObtido}</color>";
        txtChaveUnica.text = $"Chave Resgate: <color=green>{historico.chaveUnica}</color>";
        TxtStatusEntrega.text = (historico.infoEntrega.jaEntregue) ? "<color=green>Sim</color>" : "<color=red>Não</color>";

        if (ehItemLoja)
        {
            PnlCustoItem.SetActive(true);
            TxtNomeItem.text = historico.itemLoja.nome;
            TxtCusto.text = "- " + historico.precoItem;
            TxtTipoHistorico.text = "<color=lightblue>Item da Loja</color>";
            Main.Instance.ObterIcones(historico.itemLoja.icon, FileManager.Directories.item_Loja, (textura) =>
            {
                ImgIcon.texture = textura;
                ImgIcon = Util.ImgResize(ImgIcon, 180, 180);
            });
        }
        else
        {
            TxtNomeItem.text = historico.produto.nome;
            TxtTipoHistorico.text = "<color=lightblue>Produto</color>";
            Main.Instance.ObterIcones(historico.produto.icon, FileManager.Directories.produto, (textura) =>
            {
                ImgIcon.texture = textura;
                ImgIcon = Util.ImgResize(ImgIcon, 180, 180);
            });
        }

    }
}
