using System.Collections.Generic;
using APIModel;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuConfig : MonoBehaviour
{
    [Header("Botoes")]
    public Button BtnMenuConfig;
    public Button BtnAppConfig;
    public Button BtnPerfilConfig;
    public Button BtnSairApp;
    public Button BtnFecharPnlConfigApp;
    public Button BtnSalvarConfigApp;

    public GameObject PnlConfigApp;
    public GameObject PnlConfigPerfil;

    [Header("Menu Config App")]
    public Slider SliderSomFundo;
    public Slider SliderSomGeral;
    public Text TxtPctSomFundo;
    public Text TxtPctSomGeral;

    [HideInInspector]
    public bool MenuAtivo = false;

    private List<GameObject> lstMenu;

    private void Awake()
    {
        configurarListener();
        lstMenu = new List<GameObject>
        {
            BtnPerfilConfig.gameObject,
            BtnAppConfig.gameObject,
            BtnSairApp.gameObject
        };
    }

    void Start()
    {
        SliderSomFundo.onValueChanged.AddListener(changeValueFundo);
        SliderSomGeral.onValueChanged.AddListener(changeValueGeral);

        configurarSom();
    }

    #region configurarListener
    private void configurarListener()
    {
        BtnMenuConfig.onClick.AddListener(() => BtnAbrirConfiguracoes());
        BtnAppConfig.onClick.AddListener(() => PnlPopUp.AbrirPopUp(PnlConfigApp, null));
        BtnPerfilConfig.onClick.AddListener(() => PnlPopUp.AbrirPopUp(PnlConfigPerfil, null));
        BtnSairApp.onClick.AddListener(() => btnDeslogar());

        BtnFecharPnlConfigApp.onClick.AddListener(() => PnlPopUp.FecharPopUp(PnlConfigApp, () =>
        {
            configurarSom();
        }));

        BtnSalvarConfigApp.onClick.AddListener(() => btnAplicarConfigApp());
    }
    #endregion

    #region BtnAbrirConfiguracoes
    public void BtnAbrirConfiguracoes(bool fecharAutomatico = false)
    {
        MenuAtivo = (fecharAutomatico) ? false : !MenuAtivo;

        Main.Instance.AbrirMenu("BtnConfiguracoes", (fecharAutomatico) ? false : MenuAtivo, lstMenu, fecharAutomatico);
    }
    #endregion

    #region btnDeslogar
    private void btnDeslogar()
    {
        EasyAudioUtility.Instance.Play(EasyAudioUtility.Som.Click_OK);
        AnimacoesTween.AnimarObjeto(BtnSairApp.gameObject,
        AnimacoesTween.TiposAnimacoes.SubMenu_Click, () =>
        {
            EasyAudioUtility.Instance.AjustarSomSFX(1);
            EasyAudioUtility.Instance.AjustarSomBG(1);
            PlayerPrefs.DeleteAll();
            SceneManager.LoadScene("Login");
        },
        AppManager.TEMPO_ANIMACAO_ABRIR_CLICK_BOTAO);
    }
    #endregion

    #region btnAplicarConfigApp
    private void btnAplicarConfigApp()
    {
        EasyAudioUtility.Instance.Play(EasyAudioUtility.Som.Click_OK);

        AnimacoesTween.AnimarObjeto(BtnSalvarConfigApp.gameObject,
        AnimacoesTween.TiposAnimacoes.Button_Click, () =>
        {
            Cliente.ClienteLogado.configApp.somFundo = SliderSomFundo.value;
            Cliente.ClienteLogado.configApp.somGeral = SliderSomGeral.value;

            WWWForm form = new WWWForm();
            form.AddField("_id", Cliente.ClienteLogado._id);
            form.AddField("configApp", JsonConvert.SerializeObject(Cliente.ClienteLogado.configApp));

            StartCoroutine(APIManager.Instance.Post(APIManager.URLs.ClienteAlterarConfigApp, form, (response) =>
            {
                APIManager.Retorno<string> retornoAPI =
                        JsonConvert.DeserializeObject<APIManager.Retorno<string>>(response);

                if (retornoAPI.sucesso)
                {
                    //StartCoroutine(comunicadorAPI.Alerta.ChamarAlerta(retornoAPI.msg, comunicadorAPI.PnlPrincipal));

                    Main.Instance.PnlPopUp.SetActive(false);
                    PnlConfigApp.SetActive(false);
                }
                else
                {
                    EasyAudioUtility.Instance.Play(EasyAudioUtility.Som.Error);
                    //StartCoroutine(comunicadorAPI.Alerta.ChamarAlerta(retornoAPI.msg, comunicadorAPI.PnlPrincipal));
                }
            },
            (error) =>
            {
                //TODO: Tratar Error
            }));
        },
        AppManager.TEMPO_ANIMACAO_ABRIR_CLICK_BOTAO);
    }
    #endregion

    #region configurarSom
    private void configurarSom()
    {
        SliderSomFundo.value = Cliente.ClienteLogado.configApp.somFundo;
        SliderSomGeral.value = Cliente.ClienteLogado.configApp.somGeral;
    }
    #endregion

    #region changeValueFundo
    private void changeValueFundo(float value)
    {
        TxtPctSomFundo.text = Mathf.FloorToInt(value * 100) + "%";

        EasyAudioUtility.Instance.AjustarSomBG(value);
    }
    #endregion

    #region changeValueGeral
    private void changeValueGeral(float value)
    {
        TxtPctSomGeral.text = Mathf.FloorToInt(value * 100) + "%";

        EasyAudioUtility.Instance.AjustarSomSFX(value);
    }
    #endregion

}
