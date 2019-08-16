using APIModel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemComandaObj : MonoBehaviour
{

    public Text LblNome;
    public Text LblQuantidade;
    public Text LblValor;
    public RawImage Icon;

    public Comanda.Produto Produto;

    public void PreencherInfo(Comanda.Produto produto)
    {
        Produto = produto;

        LblNome.text = produto.infoProduto.nome;
        LblQuantidade.text = produto.quantidade + "x";
        LblValor.text = produto.precoTotal.ToString("C2");
    }

    #region PreencherIcone
    public void PreencherIcone(Texture2D icone)
    {
        Icon.texture = icone;
        Icon = Util.ImgResize(Icon, 180, 180);
    }
    #endregion
}
