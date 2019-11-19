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

    public Button BtnTermosUso;

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
        BtnTermosUso.onClick.AddListener(() => Application.OpenURL("http://93.188.164.122/termouso"));
    }
    #endregion

    #region btnDeslogar
    private void btnDeslogar()
    {
        EasyAudioUtility.Instance.Play(EasyAudioUtility.Som.Click_OK);

        deslogar();
    }
    #endregion

    #region deslogar
    private void deslogar()
    {
        Dictionary<string, object> form = new Dictionary<string, object>
        {
            { "deviceId", AppManager.Instance.deviceId }
        };

        alterarConfigSom();

        StartCoroutine(ClienteAPI.Deslogar(form, (response, error) =>
        {
            if (error != null)
            {
                Debug.Log(error);
                AlertaManager.Instance.ChamarAlertaMensagem(error, false);
                return;
            }

            if (response)
            {
                FindObjectOfType<Main>().PararWatch();
                EasyAudioUtility.Instance.AjustarSomSFX(0);
                EasyAudioUtility.Instance.AjustarSomBG(0);
                PlayerPrefs.DeleteAll();
                SceneManager.LoadScene("Login");
            }
        }));
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

        if (!Application.isEditor)
            EasyAudioUtility.Instance.AjustarSomBG(value);
    }
    #endregion

    #region changeValueGeral
    private void changeValueGeral(float value)
    {
        TxtPctSomGeral.text = Mathf.FloorToInt(value * 100) + "%";

        if (!Application.isEditor)
            EasyAudioUtility.Instance.AjustarSomSFX(value);
    }
    #endregion

    private void OnApplicationQuit()
    {
        alterarConfigSom();
    }
}
