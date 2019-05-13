using APIModel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuComanda : MonoBehaviour
{
    [Header("Botoes")]
    public Button BtnComanda;
    public Button BtnAbrirGrupoComanda;
    public Button BtnFecharGrupoComanda;
    public Button BtnItensComanda;
    public Button BtnFecharItensComanda;
    public Button BtnAbrirHistoricoCompra;
    public Button BtnFecharHistoricoCompra;

    [Header("Grupo")]
    public GameObject PnlAvatar;
    public GameObject PnlGrupo;

    [Header("Itens Comanda")]
    public GameObject PnlItensComanda;
    public GameObject PnlHistoricoComanda;


    [HideInInspector]
    public bool MenuAtivo = false;

    private List<GameObject> lstMenu;

    private void Awake()
    {
        lstMenu = new List<GameObject>
        {
            BtnItensComanda.gameObject,
            BtnAbrirGrupoComanda.gameObject
        };

        configurarListener();
    }

    #region configurarListener
    private void configurarListener()
    {
        BtnComanda.onClick.AddListener(() => BtnAbrirMenuComanda());

        BtnAbrirGrupoComanda.onClick.AddListener(() => btnAbrirGrupoComanda());
        BtnFecharGrupoComanda.onClick.AddListener(() => btnFecharGrupoComanda());

        BtnItensComanda.onClick.AddListener(() => btnAbrirItensComanda());
        BtnFecharItensComanda.onClick.AddListener(() => btnFecharItensComanda());

        BtnAbrirHistoricoCompra.onClick.AddListener(() => btnAbrirHistoriaCompra());
        BtnFecharHistoricoCompra.onClick.AddListener(() => btnFecharHistoricoCompra());
    }
    #endregion

    #region BtnAbrirMenuComanda
    public void BtnAbrirMenuComanda(bool fecharAutomatico = false)
    {
        MenuAtivo = (fecharAutomatico) ? false : !MenuAtivo;

        Main.Instance.AbrirMenu("BtnComanda", (fecharAutomatico) ? false : MenuAtivo, lstMenu, fecharAutomatico);
    }
    #endregion

    #region btnAbrirGrupoComanda
    private void btnAbrirGrupoComanda()
    {
        PnlPopUp.AbrirPopUp(PnlGrupo, () =>
        {
            PnlAvatar.GetComponent<AvatarObj>().PreencherInfo(Cliente.ClienteLogado.sexo, Cliente.ClienteLogado.avatar);
        });
    }
    #endregion

    #region btnFecharGrupoComanda
    private void btnFecharGrupoComanda()
    {
        PnlPopUp.FecharPopUp(PnlGrupo, () =>
        {
            PnlAvatar.GetComponent<AvatarObj>().PreencherInfo(Cliente.ClienteLogado.sexo, Cliente.ClienteLogado.avatar);
        });
    }
    #endregion

    #region btnAbrirItensComanda
    private void btnAbrirItensComanda()
    {

        PnlPopUp.AbrirPopUp(PnlItensComanda, () =>
        {

        });
    }
    #endregion

    #region btnFecharItensComanda
    private void btnFecharItensComanda()
    {

        PnlPopUp.FecharPopUp(PnlItensComanda, () =>
        {

        });
    }
    #endregion

    #region btnAbrirHistoriaCompra
    private void btnAbrirHistoriaCompra()
    {
        PnlPopUp.AbrirPopUp(PnlHistoricoComanda, () =>
        {

        });
    }
    #endregion

    #region btnFecharHistoricoCompra
    private void btnFecharHistoricoCompra()
    {
        PnlPopUp.FecharPopUpSemDesligarPopUP(PnlHistoricoComanda, () =>
        {

        });
    }
    #endregion
}
