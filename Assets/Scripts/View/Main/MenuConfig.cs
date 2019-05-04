using System.Collections;
using System.Collections.Generic;
using APIModel;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuConfig : MonoBehaviour
{
    public GameObject BtnAppSetting;
    public GameObject BtnPerfilSetting;
    public GameObject BtnSairSetting;

    public GameObject PnlConfigApp;
    public GameObject PnlConfigPerfil;

    [Header("Menu Config App")]
    public Slider SliderSomFundo;
    public Slider SliderSomGeral;
    public Text TxtPctSomFundo;
    public Text TxtPctSomGeral;

    [HideInInspector]
    public bool MenuAtivo = false;

    void Start()
    {
        SliderSomFundo.onValueChanged.AddListener(changeValueFundo);
        SliderSomGeral.onValueChanged.AddListener(changeValueGeral);

        configurarSom();
    }

    #region "ManipularMenus"

    public void BtnAbrirConfiguracoes(bool fecharAutomatico = false)
    {

        MenuAtivo = (fecharAutomatico) ? false : !MenuAtivo;

        Main.Instance.AbrirMenu("btnConfiguracoes", (fecharAutomatico) ? false : MenuAtivo, new List<GameObject>{
                BtnPerfilSetting,
                BtnAppSetting,
                BtnSairSetting
            }, fecharAutomatico);
    }

    public void BtnAbrirPnlAppConfig()
    {
        SomController.Tocar(SomController.Som.Click_OK);
        AnimacoesTween.AnimarObjeto(EventSystem.current.currentSelectedGameObject, AnimacoesTween.TiposAnimacoes.SubMenu_Click, () =>
        {
            Main.Instance.PnlPopUp.SetActive(true);
            PnlConfigApp.SetActive(true);
            AnimacoesTween.AnimarObjeto(PnlConfigApp, AnimacoesTween.TiposAnimacoes.Scala, null, 0.5f, new Vector2(1, 1));
        },
        0.1f);
    }

    public void BtnAbrirPnlPerfilConfig()
    {
        SomController.Tocar(SomController.Som.Click_OK);
        AnimacoesTween.AnimarObjeto(EventSystem.current.currentSelectedGameObject, AnimacoesTween.TiposAnimacoes.SubMenu_Click, () =>
        {
            Main.Instance.PnlPopUp.SetActive(true);
            PnlConfigPerfil.SetActive(true);
            AnimacoesTween.AnimarObjeto(PnlConfigPerfil, AnimacoesTween.TiposAnimacoes.Scala, null, 0.5f, new Vector2(1, 1));
        },
        0.1f);
    }

    public void BtnDeslogar()
    {
        SomController.Tocar(SomController.Som.Click_OK);
        AnimacoesTween.AnimarObjeto(EventSystem.current.currentSelectedGameObject, AnimacoesTween.TiposAnimacoes.SubMenu_Click, () =>
        {
            SomController.AjustarSomSFX(1);
            SomController.AjustarSomBG(1);
            PlayerPrefs.DeleteAll();
            SceneManager.LoadScene("Login");
        },
        0.1f);
    }
    #endregion

    #region "Painel Config App"
    private void changeValueFundo(float value)
    {
        TxtPctSomFundo.text = Mathf.FloorToInt(value * 100) + "%";

        SomController.AjustarSomBG(value);
    }
    private void changeValueGeral(float value)
    {
        TxtPctSomGeral.text = Mathf.FloorToInt(value * 100) + "%";

        SomController.AjustarSomSFX(value);
    }

    private void configurarSom()
    {
        SliderSomFundo.value = Cliente.ClienteLogado.configApp.somFundo;
        SliderSomGeral.value = Cliente.ClienteLogado.configApp.somGeral;
    }

    public void BtnAplicarConfigApp()
    {

        SomController.Tocar(SomController.Som.Click_OK);

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
                SomController.Tocar(SomController.Som.Error);
                //StartCoroutine(comunicadorAPI.Alerta.ChamarAlerta(retornoAPI.msg, comunicadorAPI.PnlPrincipal));
            }
        },
        (error) =>
        {
            //TODO: Tratar Error
        }));
    }

    public void BtnFecharPnlConfigApp()
    {
        SomController.Tocar(SomController.Som.Click_Cancel);
        AnimacoesTween.AnimarObjeto(EventSystem.current.currentSelectedGameObject, AnimacoesTween.TiposAnimacoes.Button_Click, () =>
        {
            configurarSom();
            Main.Instance.PnlPopUp.SetActive(false);
            PnlConfigApp.SetActive(false);
        });
    }

    #endregion
}
