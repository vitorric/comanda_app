using System.Collections.Generic;
using APIModel;
using Network;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuConfig : MonoBehaviour
{
    [Header("Botoes")]
    public Button BtnSairApp;

    public GameObject PnlConfigApp;
    public GameObject PnlConfigPerfil;

    [Header("Menu Config App")]
    public Slider SliderSomFundo;
    public Slider SliderSomGeral;
    public Text TxtPctSomFundo;
    public Text TxtPctSomGeral;

    private void Awake()
    {
        configurarListener();
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
        //BtnPerfilConfig.onClick.AddListener(() => PnlPopUp.AbrirPopUp(PnlConfigPerfil, null));
        BtnSairApp.onClick.AddListener(() => btnDeslogar());
    }
    #endregion

    #region btnDeslogar
    private void btnDeslogar()
    {
        EasyAudioUtility.Instance.Play(EasyAudioUtility.Som.Click_OK);

        FindObjectOfType<Main>().PararWatch();
        EasyAudioUtility.Instance.AjustarSomSFX(1);
        EasyAudioUtility.Instance.AjustarSomBG(1);
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene("Login");
    }
    #endregion

    #region btnAplicarConfigApp
    private void alterarConfigSom()
    {
        Cliente.ClienteLogado.configApp.somFundo = SliderSomFundo.value;
        Cliente.ClienteLogado.configApp.somGeral = SliderSomGeral.value;

        Dictionary<string, object> form = new Dictionary<string, object>
        {
            { "configApp", JsonConvert.SerializeObject(Cliente.ClienteLogado.configApp) }
        };

        StartCoroutine(ClienteAPI.ClienteAlterarConfigApp(form,
        (response, error) =>
        {
            if (error != null)
            {
                Debug.Log(error);
                return;
            }
        }));
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

    private void OnApplicationQuit()
    {
        alterarConfigSom();
    }
}
