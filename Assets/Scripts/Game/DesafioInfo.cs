using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DesafioInfo : MonoBehaviour
{
    public Button BtnFechar;
    public Text TxtValorPremio;
    public RawImage IconPremio;

    private void Awake()
    {
        configurarListener();
    }

    #region configurarListener
    private void configurarListener()
    {
        BtnFechar.onClick.AddListener(() => PnlPopUp.FecharPopUpSemDesligarPopUP(this.gameObject, null));
    }
    #endregion

    #region PreencherInfo
    public void PreencherInfo(float valor, string icon)
    {
        this.gameObject.SetActive(true);
        TxtValorPremio.text = Util.FormatarValorDisponivel(valor);
    }
    #endregion
}
