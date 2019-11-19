using APIModel;
using Network;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class GrupoObj : MonoBehaviour
{
    public Text LblValor;
    public Text LblNome;
    public Button BtnTransferirLideranca;

    public Image ImgJaPagou;
    public Image ImgNaoPagou;

    public Transform PnlAvatar;
    public AvatarObj Avatar;

    [HideInInspector]
    public Comanda.Grupo Integrante;

    private void Awake()
    {
        Avatar = Instantiate(Avatar, PnlAvatar);
    }


    #region PreencherInfo
    public void PreencherInfo(Comanda.Grupo grupo, Cliente.Avatar avatar, bool clienteLogadoEhLider, Action<string, string> transferirLideranca)
    {
        Integrante = grupo;

        LblNome.text = Integrante.cliente.apelido;

        BtnTransferirLideranca.gameObject.SetActive(Integrante.lider);

        if (clienteLogadoEhLider)
        {
            BtnTransferirLideranca.gameObject.SetActive(clienteLogadoEhLider && !Integrante.jaPagou);
            BtnTransferirLideranca.interactable = true;
        }

        ImgJaPagou.gameObject.SetActive(Integrante.jaPagou);
        ImgNaoPagou.gameObject.SetActive(!Integrante.jaPagou);

        if (avatar != null)
            Avatar.PreencherInfo(Integrante.cliente.sexo, avatar);

        BtnTransferirLideranca.onClick.AddListener(() =>
        {
            transferirLideranca(Integrante.cliente._id, Integrante.cliente.apelido);
        });
    }
    #endregion

    #region preencherValorAPagar
    public void preencherValorAPagar(double valor)
    {
        if (Integrante.jaPagou)
        {
            LblValor.text = Integrante.valorPago.ToString("C2", CultureInfo.GetCultureInfo("pt-BR"));
            return;
        }

        LblValor.text = valor.ToString("C2", CultureInfo.GetCultureInfo("pt-BR"));
    }
    #endregion
}
