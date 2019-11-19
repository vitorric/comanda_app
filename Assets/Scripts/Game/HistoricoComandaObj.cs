using APIModel;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class HistoricoComandaObj : MonoBehaviour
{
    public Text TxtNome;
    public Text TxtData;
    public Text TxtQuant;
    public Text TxtValorTotal;
    public RawImage IconProduto;

    public void PreencherInfo(HistoricoComanda historicoComanda)
    {
        Main.Instance.ObterIcones(historicoComanda.iconProduto, FileManager.Directories.item_Loja, (textura) =>
        {
            if (textura != null)
            {
                IconProduto.texture = textura;
                IconProduto = Util.ImgResize(IconProduto, 180, 180);
            }
        });

        TxtNome.text = historicoComanda.nomeProduto;
        TxtData.text = historicoComanda.createdAt;
        TxtQuant.text = historicoComanda.quantidade.ToString();
        TxtValorTotal.text = historicoComanda.valorTotal.ToString("C2", CultureInfo.GetCultureInfo("pt-BR"));
    }
}
