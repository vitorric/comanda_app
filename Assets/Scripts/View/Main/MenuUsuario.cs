using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using APIModel;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;

public class MenuUsuario : MonoBehaviour
{

    public GameObject BtnPerfil;
    public GameObject BtnAmigos;
    public GameObject BtnConquistas;
    public GameObject BtnLoja;
    public GameObject BtnFechar;
    public Text TxtTitulo;
    
    [Header("Avatar")]
    public GameObject PnlAvatar;

    [Header("Perfil")]
    public GameObject PnlPerfilUsuario;
    public GameObject PnlPerfilGeral;
    public GameObject PnlAvatarInfo;
    public Text LblPerfilApelido;
    public Text LblPerfilLevel;
    public Text LblPerfilExp;
    public Text LblPerfilPontos;
    public Text LblPerfilGold;

    [Header("Perfil Edicao")]
    public GameObject PnlAvatarEdicao;
    public GameObject PnlPerfilEdicao;
    public Toggle BtnPerfilInfo;
    public List<GameObject> PnlAbasEdicao;
    public Toggle AbaEdicao1;
    public InputField TxtEmailInfo;
    public InputField TxtSenhaInfo;
    public InputField TxtSenhaInfoConfirmar;
    public InputField TxtNomeInfo;
    public InputField TxtCPFInfo;
    public InputField TxtIdadeInfo;
    public InputField TxtRuaInfo;
    public InputField TxtNumeroInfo;
    public InputField TxtBairroInfo;
    public InputField TxtCidadeInfo;
    public Dropdown DDLEstadoInfo;
    public InputField TxtCEPInfo;
    public InputField TxtApelidoInfo;
    public Text TxtSexoInfo;
    public Cliente.Avatar AvatarEditado;

    [Header("Amigos")]
    public GameObject PnlAmigos;

    [Header("Historico Compra")]
    public GameObject PnlHistoricoCompra;
    public GameObject ScvHistoricoCompra;
    public GameObject ScvHistoricoCompraContent;
    public GameObject SlotHistoricoCompra;
    public GameObject TxtHistoricoVazio;

    public bool MenuAtivo = false;

    void Start()
    {
    }

    public void BtnAbrirPnlUsuario(bool fecharAutomatico = false)
    {

        MenuAtivo = (fecharAutomatico) ? false : !MenuAtivo;

        Main.Instance.AbrirMenu("btnPerfil", (fecharAutomatico) ? false : MenuAtivo, new List<GameObject>{
            BtnPerfil,
            BtnAmigos,
            BtnConquistas,
            BtnLoja
        }, fecharAutomatico);
    }

    public void PreencherAvatares()
    {
        PnlAvatar.GetComponent<AvatarObj>().PreencherInfo(Cliente.ClienteLogado.sexo, Cliente.ClienteLogado.avatar);
        PnlAvatarInfo.GetComponent<AvatarObj>().PreencherInfo(Cliente.ClienteLogado.sexo, Cliente.ClienteLogado.avatar);
    }

    #region "Perfil Usuario"

    public void BtnAbrirPnlPerfil()
    {
        TxtTitulo.text = "Perfil";
        PnlPerfilEdicao.SetActive(false);
        PnlHistoricoCompra.SetActive(false);
        BtnFechar.SetActive(true);
        PnlPerfilGeral.SetActive(true);

        SomController.Tocar(SomController.Som.Click_OK);
        AnimacoesTween.AnimarObjeto(EventSystem.current.currentSelectedGameObject, AnimacoesTween.TiposAnimacoes.SubMenu_Click, () =>
        {
            Main.Instance.PnlPopUp.SetActive(true);
            PnlPerfilUsuario.SetActive(true);
            configurarPerfil();
            AnimacoesTween.AnimarObjeto(PnlPerfilUsuario, AnimacoesTween.TiposAnimacoes.Scala, null, 0.5f, new Vector2(1, 1));
        },
        0.1f);
    }

    public void BtnEditPerfil()
    {

        SomController.Tocar(SomController.Som.Click_OK);
        AnimacoesTween.AnimarObjeto(EventSystem.current.currentSelectedGameObject, AnimacoesTween.TiposAnimacoes.Button_Click, () =>
        {
            PnlPerfilGeral.SetActive(false);
            BtnFechar.SetActive(false);
            PnlPerfilEdicao.SetActive(true);
            BtnPerfilInfo.isOn = true;
            configurarEdicaoPerfil();
        });
    }

    public void BtnFecharEditPerfil()
    {

        SomController.Tocar(SomController.Som.Click_Cancel);
        AnimacoesTween.AnimarObjeto(EventSystem.current.currentSelectedGameObject, AnimacoesTween.TiposAnimacoes.Button_Click, () =>
        {
            PnlPerfilEdicao.SetActive(false);
            BtnFechar.SetActive(true);
            PnlPerfilGeral.SetActive(true);
        });
    }

    public void BtnAbrirHistoricoCompra()
    {

        SomController.Tocar(SomController.Som.Click_OK);
        TxtHistoricoVazio.SetActive(false);
        AnimacoesTween.AnimarObjeto(EventSystem.current.currentSelectedGameObject, AnimacoesTween.TiposAnimacoes.Button_Click, () =>
        {
            ScvHistoricoCompra.GetComponentsInChildren<HistoricoCompraObj>().ToList().ForEach(x => Destroy(x.gameObject));

            WWWForm form = new WWWForm();
            form.AddField("_idCliente", Cliente.ClienteLogado._id);

            StartCoroutine(APIManager.Instance.Post(APIManager.URLs.ListarHistoricoCompra, form, (response) =>
            {
                APIManager.Retorno<List<HistoricoCompra>> retornoAPI = 
                    JsonConvert.DeserializeObject<APIManager.Retorno<List<HistoricoCompra>>>(response);

                if (retornoAPI.sucesso)
                {

                    TxtTitulo.text = "Histórico de Compras";
                    PnlPerfilGeral.SetActive(false);
                    BtnFechar.SetActive(false);
                    PnlHistoricoCompra.SetActive(true);

                    AnimacoesTween.AnimarObjeto(PnlHistoricoCompra, AnimacoesTween.TiposAnimacoes.Scala, () =>
                    {
                        if (retornoAPI.retorno.Count == 0)
                        {
                            TxtHistoricoVazio.SetActive(true);
                            return;
                        }

                        foreach (HistoricoCompra historico in retornoAPI.retorno)
                        {
                            GameObject objEstab = Instantiate(SlotHistoricoCompra, ScvHistoricoCompra.transform);
                            objEstab.transform.SetParent(ScvHistoricoCompraContent.transform);
                            objEstab.name = "histCompra_id" + historico._id;
                            objEstab.GetComponent<HistoricoCompraObj>().PreencherInfo(historico);
                        }

                    }, 0.1f, new Vector2(1, 1));
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
        },
        0.3f);
    }

    public void BtnFecharHistoricoCompra()
    {

        SomController.Tocar(SomController.Som.Click_Cancel);
        AnimacoesTween.AnimarObjeto(EventSystem.current.currentSelectedGameObject, AnimacoesTween.TiposAnimacoes.Button_Click, () =>
        {
            AnimacoesTween.AnimarObjeto(PnlHistoricoCompra, AnimacoesTween.TiposAnimacoes.Scala, () =>
            {

                TxtTitulo.text = "Perfil";
                PnlHistoricoCompra.SetActive(false);
                BtnFechar.SetActive(true);
                PnlPerfilGeral.SetActive(true);
            }, 0.5f, new Vector2(0, 0));
        });
    }

    private void configurarPerfil()
    {
        try
        {
            LblPerfilApelido.text = Cliente.ClienteLogado.apelido;
            LblPerfilExp.text = Cliente.ClienteLogado.avatar.exp + "/" + Cliente.ClienteLogado.avatar.expProximoLevel;
            LblPerfilLevel.text = Cliente.ClienteLogado.avatar.level.ToString();
            LblPerfilPontos.text = Cliente.ClienteLogado.pontos.ToString();
            LblPerfilGold.text = Cliente.ClienteLogado.RetornarGoldTotal().ToString();
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    private void configurarEdicaoPerfil()
    {
        PnlAvatarEdicao.GetComponent<AvatarObj>().PreencherInfo(Cliente.ClienteLogado.sexo, Cliente.ClienteLogado.avatar);
        AvatarEditado = null;
        TxtEmailInfo.text = Cliente.ClienteLogado.email;
        TxtSenhaInfo.text = string.Empty;
        TxtSenhaInfoConfirmar.text = string.Empty;
        TxtNomeInfo.text = Cliente.ClienteLogado.nome;
        TxtCPFInfo.text = Cliente.ClienteLogado.cpf;
        TxtIdadeInfo.text = Cliente.ClienteLogado.dataNascimento.ToString("dd/MM/yyyy");
        TxtApelidoInfo.text = Cliente.ClienteLogado.apelido;
        TxtSexoInfo.text = Cliente.ClienteLogado.sexo;

        if (Cliente.ClienteLogado.endereco != null)
        {
            TxtRuaInfo.text = Cliente.ClienteLogado.endereco.rua;
            TxtNumeroInfo.text = Cliente.ClienteLogado.endereco.numero.ToString();
            TxtBairroInfo.text = Cliente.ClienteLogado.endereco.bairro;
            TxtCidadeInfo.text = Cliente.ClienteLogado.endereco.cidade;
            TxtCEPInfo.text = Cliente.ClienteLogado.endereco.cep;
            DDLEstadoInfo.value = DDLEstadoInfo.options.FindIndex(x => x.text == Cliente.ClienteLogado.endereco.estado);
            Cliente.ClienteLogado.endereco.estado = DDLEstadoInfo.options[DDLEstadoInfo.value].text;
        }
    }

    public void BtnAbrirEdicaoAvatar()
    {
        SomController.Tocar(SomController.Som.Click_OK);
        AnimacoesTween.AnimarObjeto(EventSystem.current.currentSelectedGameObject, AnimacoesTween.TiposAnimacoes.Button_Click, () =>
        {
            StartCoroutine(new SceneController().CarregarCenaAdditiveAsync("EdicaoChar"));
        });
    }

    public void BtnSalvarEdicaoPerfil()
    {        
        if (string.IsNullOrEmpty(TxtSenhaInfo.text) || string.IsNullOrEmpty(TxtSenhaInfoConfirmar.text))
        {
            //StartCoroutine(comunicadorAPI.Alerta.ChamarAlerta(Alerta.MsgAlerta.PreenchaAsSenhas, comunicadorAPI.PnlPrincipal));
            return;
        }

        if (TxtSenhaInfo.text != TxtSenhaInfoConfirmar.text)
        {
            //StartCoroutine(comunicadorAPI.Alerta.ChamarAlerta(Alerta.MsgAlerta.SenhasNaoConferem, comunicadorAPI.PnlPrincipal));
            return;
        }

        if (TxtIdadeInfo.text == string.Empty)
        {
            //StartCoroutine(comunicadorAPI.Alerta.ChamarAlerta(Alerta.MsgAlerta.PreenchaOsCampos, comunicadorAPI.PnlPrincipal));
            return;
        }

        string[] valoresDatas = TxtIdadeInfo.text.Split('/');

        if (Convert.ToInt32(valoresDatas[0]) > 31 || Convert.ToInt32(valoresDatas[0]) < 1 ||
            Convert.ToInt32(valoresDatas[1]) > 12 || Convert.ToInt32(valoresDatas[1]) < 1)
        {
            //StartCoroutine(comunicadorAPI.Alerta.ChamarAlerta(Alerta.MsgAlerta.DataInvalida, comunicadorAPI.PnlPrincipal));
            return;
        }

        if (DateTime.Now.Year - Convert.ToInt32(valoresDatas[2]) < 18 || (DateTime.Now.Year - Convert.ToInt32(valoresDatas[2])) == 18 && Convert.ToInt32(valoresDatas[1]) > DateTime.Now.Month)
        {
            //StartCoroutine(comunicadorAPI.Alerta.ChamarAlerta(Alerta.MsgAlerta.MenorIdade, comunicadorAPI.PnlPrincipal));
            return;
        }

        salvarPerfil();
    }

    public void AlterarAvatar(Cliente.Avatar avatar)
    {
        AvatarEditado = avatar;

        PnlAvatarEdicao.GetComponent<AvatarObj>().PreencherInfo(TxtSexoInfo.text, AvatarEditado);
    }

    private void salvarPerfil()
    {

        SomController.Tocar(SomController.Som.Click_OK);

        Cliente.ClienteLogado.endereco = new Cliente.Endereco
        {
            rua = TxtRuaInfo.text,
            bairro = TxtBairroInfo.text,
            cidade = TxtCidadeInfo.text,
            cep = TxtCEPInfo.text
        };

        if (!string.IsNullOrEmpty(TxtNumeroInfo.text))
            Cliente.ClienteLogado.endereco.numero = Convert.ToInt32(TxtNumeroInfo.text);

        Cliente.ClienteLogado.endereco.estado = DDLEstadoInfo.options[DDLEstadoInfo.value].text;
        Cliente.ClienteLogado.password = TxtSenhaInfo.text;

        Cliente.ClienteLogado.dataNascimento = Convert.ToDateTime(Util.formatarDataParaAPI(TxtIdadeInfo.text));
        Cliente.ClienteLogado.nome = TxtNomeInfo.text;

        Cliente.ClienteLogado.avatar = (AvatarEditado != null) ? AvatarEditado : Cliente.ClienteLogado.avatar;
        
        WWWForm form = new WWWForm();
        form.AddField("_id", Cliente.ClienteLogado._id);
        form.AddField("password", Cliente.ClienteLogado.password);
        form.AddField("dataNascimento", Util.formatarDataParaAPI(TxtIdadeInfo.text));
        form.AddField("nome", Cliente.ClienteLogado.nome);
        form.AddField("endereco", JsonConvert.SerializeObject(Cliente.ClienteLogado.endereco));
        form.AddField("avatar", JsonConvert.SerializeObject(Cliente.ClienteLogado.avatar));

        StartCoroutine(APIManager.Instance.Post(APIManager.URLs.ClienteAlterar, form, 
        (response) =>
        {
            APIManager.Retorno<string> retornoAPI = new APIManager.Retorno<string>();
            retornoAPI = JsonConvert.DeserializeObject<APIManager.Retorno<string>>(response);

            if (retornoAPI.sucesso)
            {
                PreencherAvatares();
                //StartCoroutine(comunicadorAPI.Alerta.ChamarAlerta(retornoAPI.msg, comunicadorAPI.PnlPrincipal));
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

    public void MudarAba(int numeroAba)
    {
        if (!EventSystem.current.currentSelectedGameObject.name.Contains("AbrirInfo"))
            SomController.Tocar(SomController.Som.Click_OK);

        AnimacoesTween.AnimarObjeto(EventSystem.current.currentSelectedGameObject, AnimacoesTween.TiposAnimacoes.Button_Click, () =>
        {
            PnlAbasEdicao.ForEach(x => x.SetActive(false));
            PnlAbasEdicao[numeroAba].SetActive(true);
        });
    }

    #endregion

    #region "Amigos"

    public void BtnAbrirPnlAmigos()
    {

        SomController.Tocar(SomController.Som.Click_OK);
        AnimacoesTween.AnimarObjeto(EventSystem.current.currentSelectedGameObject, AnimacoesTween.TiposAnimacoes.SubMenu_Click, () =>
        {
            Main.Instance.PnlPopUp.SetActive(true);
            PnlAmigos.SetActive(true);
            AnimacoesTween.AnimarObjeto(PnlAmigos, AnimacoesTween.TiposAnimacoes.Scala, null, 0.5f, new Vector2(1, 1));
        },
        0.1f);
    }
    #endregion
}
