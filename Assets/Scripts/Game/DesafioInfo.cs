using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DesafioInfo : MonoBehaviour
{
    public Button BtnFechar;
    public GameObject PnlInfo;
    public Text TxtValorPremio;
    public RawImage IconPremio;

    private void Awake()
    {
        configurarListener();
    }

    #region configurarListener
    private void configurarListener()
    {
        BtnFechar.onClick.AddListener(() => PnlPopUp.FecharPopUpSemDesligarPopUP(PnlInfo, () => this.gameObject.SetActive(false)));
    }
    #endregion

    #region PreencherInfo
    public void PreencherInfo(float valor, string icon)
    {
        this.gameObject.SetActive(true);
        PnlInfo.SetActive(true);
        TxtValorPremio.text = Util.FormatarValores(valor);

        AnimacoesTween.AnimarObjeto(PnlInfo,
                    AnimacoesTween.TiposAnimacoes.Scala,
                    null,
                    AppManager.TEMPO_ANIMACAO_ABRIR_MODEL,
                    Vector2.one);
    }
    #endregion
}
