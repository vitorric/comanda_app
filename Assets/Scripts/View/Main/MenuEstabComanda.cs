using System.Collections.Generic;
using System.Linq;
using APIModel;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuEstabComanda : MonoBehaviour
{
    public GameObject BtnEstabInfoComanda;
    public GameObject BtnConquistaComanda;
    public GameObject BtnSairEstabelecimento;
    [Header("Party")]
    public GameObject PnlAvatar;
    public GameObject PnlGroupParty;
    [Header("Itens Comanda")]
    public GameObject PnlItensComanda;
    public GameObject PnlHistoricoComanda;
    public bool MenuAtivo = false;

    #region Controle Estabelecimento

    public void BtnAbrirMenuEstabelecimentoComanda(bool fecharAutomatico = false)
    {

        MenuAtivo = (fecharAutomatico) ? false : !MenuAtivo;

        Main.Instance.AbrirMenu("btnEstabelecimentoComanda", (fecharAutomatico) ? false : MenuAtivo, new List<GameObject>{
            BtnEstabInfoComanda,
            BtnConquistaComanda,
            BtnSairEstabelecimento
        }, fecharAutomatico);
    }

    public void BtnAbrirEstabelecimento(int aba)
    {
        SomController.Tocar(SomController.Som.Click_OK);

        AnimacoesTween.AnimarObjeto(EventSystem.current.currentSelectedGameObject, AnimacoesTween.TiposAnimacoes.Button_Click, () =>
        {            
            WWWForm form = new WWWForm();
            form.AddField("_idEstabelecimento", Cliente.ClienteLogado.configClienteAtual.estabelecimento);

            StartCoroutine(APIManager.Instance.Post(APIManager.URLs.ObterEstabelecimento, form, (response) =>
            {
                APIManager.Retorno<Estabelecimento> retornoAPI =
                           JsonConvert.DeserializeObject<APIManager.Retorno<Estabelecimento>>(response);

                if (retornoAPI.sucesso)
                {
                    Main.Instance.MenuEstabelecimento.PreencherInfoEstabelecimento(retornoAPI.retorno, false);
                }
                else
                {
                    SomController.Tocar(SomController.Som.Error);
                    //StartCoroutine(FindObjectOfType<Alerta>().ChamarAlerta(retornoAPI.msg, comunicadorAPI.PnlPrincipal));
                }
            },
            (error) =>
            {
                //TODO: Tratar Error
            }));
        });
    }

    public void FecharPnlEstabInfo()
    {
        SomController.Tocar(SomController.Som.Click_Cancel);
        AnimacoesTween.AnimarObjeto(EventSystem.current.currentSelectedGameObject, AnimacoesTween.TiposAnimacoes.Button_Click, () =>
        {
            Main.Instance.PnlPopUp.SetActive(false);
            AnimacoesTween.AnimarObjeto(Main.Instance.MenuEstabelecimento.PnlEstabInfo, AnimacoesTween.TiposAnimacoes.Scala, () =>
            {
                Main.Instance.MenuEstabelecimento.PnlEstabInfo.SetActive(false);
            }, 0.1f, new Vector2(0, 0));
        },
        0.1f);

        Main.Instance.MenuEstabelecimento.ScvEstabelecimentoShopContent.GetComponentsInChildren<ItemObj>().ToList().ForEach(x => Destroy(x.gameObject));
    }

    public void BtnSairDoEstabelecimento()
    {
        SomController.Tocar(SomController.Som.Click_Cancel);

        AnimacoesTween.AnimarObjeto(EventSystem.current.currentSelectedGameObject, AnimacoesTween.TiposAnimacoes.Button_Click, () =>
        {
            WWWForm form = new WWWForm();
            form.AddField("_idCliente", Cliente.ClienteLogado._id);
            form.AddField("_idEstabelecimento", Cliente.ClienteLogado.configClienteAtual.estabelecimento);

            StartCoroutine(APIManager.Instance.Post(APIManager.URLs.SairDoEstabelecimento, form,
            (response) =>
            {
                APIManager.Retorno<string> retornoAPI = JsonConvert.DeserializeObject<APIManager.Retorno<string>>(response);

                if (retornoAPI.sucesso)
                {
                    Main.Instance.ManipularMenus("FecharTodos");
                    Cliente.ClienteLogado.configClienteAtual.estaEmUmEstabelecimento = false;
                    Cliente.ClienteLogado.configClienteAtual.estabelecimento = null;
                    Cliente.ClienteLogado.configClienteAtual.nomeEstabelecimento = null;
                    Main.Instance.ClienteEstaNoEstabelecimento();
                }
                else
                {
                    SomController.Tocar(SomController.Som.Error);
                    //StartCoroutine(FindObjectOfType<Alerta>().ChamarAlerta(retornoAPI.msg, comunicadorAPI.PnlPrincipal));
                }
            },
            (error) =>
            {
                //TODO: Tratar Error
            }));
        });

    }
    #endregion

    #region "Group Party"
    public void BtnAbrirParty()
    {
        SomController.Tocar(SomController.Som.Click_OK);

        AnimacoesTween.AnimarObjeto(EventSystem.current.currentSelectedGameObject, AnimacoesTween.TiposAnimacoes.SubMenu_Click, () =>
        {
            Main.Instance.PnlPopUp.SetActive(true);
            PnlGroupParty.SetActive(true);
            Cliente.Dados usuario = Cliente.ClienteLogado;
            PnlAvatar.GetComponent<AvatarObj>().PreencherInfo(usuario.sexo, usuario.avatar);
            AnimacoesTween.AnimarObjeto(PnlGroupParty, AnimacoesTween.TiposAnimacoes.Scala, () =>
            {
            }, 0.5f, new Vector2(1, 1));
        });
    }
    #endregion

    #region "Itens na Comanda"
    public void BtnAbrirItensComanda()
    {
        SomController.Tocar(SomController.Som.Click_OK);

        AnimacoesTween.AnimarObjeto(EventSystem.current.currentSelectedGameObject, AnimacoesTween.TiposAnimacoes.SubMenu_Click, () =>
        {
            Main.Instance.PnlPopUp.SetActive(true);
            PnlItensComanda.SetActive(true);
            AnimacoesTween.AnimarObjeto(PnlItensComanda, AnimacoesTween.TiposAnimacoes.Scala, () =>
            {

            }, 0.5f, new Vector2(1, 1));
        });
    }

    public void BtnAbrirHistoriaCompra()
    {
        SomController.Tocar(SomController.Som.Click_OK);

        AnimacoesTween.AnimarObjeto(EventSystem.current.currentSelectedGameObject, AnimacoesTween.TiposAnimacoes.SubMenu_Click, () =>
        {
            Main.Instance.PnlPopUp.SetActive(true);
            PnlHistoricoComanda.SetActive(true);
            AnimacoesTween.AnimarObjeto(PnlHistoricoComanda, AnimacoesTween.TiposAnimacoes.Scala, () =>
            {

            }, 0.5f, new Vector2(1, 1));
        });
    }

    public void BtnFecharHistoricoCompra()
    {
        SomController.Tocar(SomController.Som.Click_Cancel);
        AnimacoesTween.AnimarObjeto(EventSystem.current.currentSelectedGameObject, AnimacoesTween.TiposAnimacoes.Button_Click, () =>
        {
            AnimacoesTween.AnimarObjeto(PnlHistoricoComanda, AnimacoesTween.TiposAnimacoes.Scala, () =>
            {
            }, 0.5f, new Vector2(0, 0));
        },
        0.1f);
    }
    #endregion
}
