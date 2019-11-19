using APIModel;
using FirebaseModel;
using Network;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MenuDesafios : MonoBehaviour
{
    [Header("Button Aba Config")]
    public ButtonControl buttonControl;

    [Header("Paineis")]
    public List<GameObject> PnlAbasDesafio;

    [Header("Desafio")]
    public DesafioObj DesafioObjRef;
    public Transform SvcDesafioProgresso;
    public Transform SvcDesafioConcluido;
    public GameObject txtDesafioProgressoVazio;
    public GameObject txtDesafioConcluidoVazio;

    private List<DesafioObj> lstDesafiosProgresso;
    private List<string> lstDesafiosConcluido;

    [HideInInspector]
    public ClienteFirebase ClienteFirebase;

    private void Awake()
    {
        configurarListener();

        lstDesafiosProgresso = new List<DesafioObj>();
        lstDesafiosConcluido = new List<string>();

        ClienteFirebase = new ClienteFirebase()
        {
            AcaoDesafio = (desafio, tipoAcao) =>
            {
                if (tipoAcao == ClienteFirebase.TipoAcao.Adicionar)
                {
                    adicionarDesafio(desafio);
                    return;
                }

                if (tipoAcao == ClienteFirebase.TipoAcao.Modificar)
                {
                    modificarDesafio(desafio);
                    return;
                }

                if (tipoAcao == ClienteFirebase.TipoAcao.Remover)
                {
                    removerDesafio(desafio);
                    return;
                }
            }
        };

        ClienteFirebase.WatchDesafios(Cliente.ClienteLogado._id, true);

        //listarDesafiosConcluidos();
    }

    #region configurarListener
    private void configurarListener()
    {
        buttonControl.BtnAbas[0].onClick.AddListener(() => mudarAba(0, true));
        buttonControl.BtnAbas[1].onClick.AddListener(() => mudarAba(1, true));
    }
    #endregion

    #region mudarAba
    private void mudarAba(int numeroAba, bool tocarSom = false)
    {
        EasyAudioUtility.Instance.Play(EasyAudioUtility.Som.Click_OK);

        PnlAbasDesafio.ForEach(x => x.SetActive(false));
        PnlAbasDesafio[numeroAba].SetActive(true);
        buttonControl.TrocarAba(numeroAba);
    }
    #endregion

    #region adicionarDesafio
    private void adicionarDesafio(DesafioCliente desafio)
    {
        if (desafio != null)
        {
            DesafioObj desafioObj = null;

            //os objetos do desafio sao criado dentro do response, somente quando for adicionar que eh feito isso
            obterDesafio(desafio, desafioObj, true);
        }
    }
    #endregion

    #region modificarDesafio
    private void modificarDesafio(DesafioCliente desafio)
    {
        DesafioObj desafioObj = lstDesafiosProgresso.Find(x => x.DesafioCliente._id == desafio._id);

        if (desafioObj != null)
        {
            if (desafio.concluido)
            {
                lstDesafiosProgresso.Remove(desafioObj);
                lstDesafiosConcluido.Add(desafio._id);

                if (lstDesafiosProgresso.Count == 0)
                {
                    txtDesafioProgressoVazio.SetActive(true);
                }

                //listarDesafiosConcluidos();

                obterDesafioConcluido(desafio.desafio._id);
            }

            obterDesafio(desafio, desafioObj, false);
        }
    }
    #endregion

    #region removerDesafio
    private void removerDesafio(DesafioCliente desafio)
    {
        DesafioObj desafioObj = lstDesafiosProgresso.Find(x => x.DesafioCliente._id == desafio._id);

        if (desafioObj != null)
        {
            Destroy(desafioObj.gameObject);
            lstDesafiosProgresso.Remove(desafioObj);

            if (lstDesafiosProgresso.Count == 0)
            {
                txtDesafioProgressoVazio.SetActive(true);
            }

            return;
        }

        //desafioObj = lstDesafiosConcluido.Find(x => x.DesafioCliente._id == desafio._id);

        //if (desafioObj != null)
        //{
        //    Destroy(desafioObj.gameObject);
        //    lstDesafiosConcluido.Remove(desafioObj);


        //    if (lstDesafiosConcluido.Count == 0)
        //    {
        //        txtDesafioConcluidoVazio.SetActive(true);
        //    }
        //}
    }
    #endregion

    #region obterDesafio
    private void obterDesafio(DesafioCliente desafio, DesafioObj desafioObj, bool ehAdicao)
    {
        Dictionary<string, object> form = new Dictionary<string, object>
        {
            { "desafioId", desafio.desafio._id }
        };


        StartCoroutine(DesafioAPI.ObterDesafio(form,
        (response, error) =>
        {
            if (error != null)
            {
                Debug.Log("obterDesafio: " + error);
                AlertaManager.Instance.ChamarAlertaMensagem(error, false);
                return;
            }

            if (ehAdicao)
            {
                TimeSpan ts = response.tempoDuracao.ToLocalTime().Subtract((DateTime.Now.ToLocalTime()));

                if (ts.TotalSeconds <= 0 && !desafio.concluido)
                    return;

                if (desafio.concluido)
                {
                    lstDesafiosConcluido.Add(desafio._id);

                    if (!desafio.resgatouPremio)
                        AppManager.Instance.AtivarDesafioCompletado(desafio);

                    obterDesafioConcluido(desafio.desafio._id);
                    return;
                }

                desafioObj = Instantiate(DesafioObjRef, SvcDesafioProgresso);
                lstDesafiosProgresso.Add(desafioObj);
                txtDesafioProgressoVazio.SetActive(false);
            }


            if (desafio.concluido && !desafio.resgatouPremio)
                AppManager.Instance.AtivarDesafioCompletado(desafio);

            if (!ehAdicao)
            {
                if (desafio.concluido)
                {
                    Main.Instance.MenuEstabelecimento.DeletarDesafioCompletado(desafioObj.Desafio);
                    Destroy(desafioObj.gameObject);
                    return;
                }
            }

            Main.Instance.ObterIcones(response.icon, FileManager.Directories.desafio, (textura) =>
            {
                if (textura != null)
                    desafioObj.PreencherIcone(textura);
            });

            desafioObj.PreencherInfo(response, desafio);
            Main.Instance.MenuEstabelecimento.AlterarProgressoDesafio(desafio);
        }));
    }
    #endregion

    #region PararWatch
    public void PararWatch()
    {
        ClienteFirebase.WatchDesafios(Cliente.ClienteLogado._id, false);
    }
    #endregion

    #region BuscarDesafio
    public DesafioCliente BuscarDesafio(string _id)
    {
        DesafioObj clienteDesafio = null;

        clienteDesafio = lstDesafiosProgresso.Find(x => x.DesafioCliente._id == _id);

        if (clienteDesafio != null)
        {
            return clienteDesafio.DesafioCliente;
        }

        return null;
    }
    #endregion

    #region ConferirDesfioConcluido
    public bool ConferirDesafioConcluido(string desafioId)
    {
        if (lstDesafiosConcluido.Find(x => x == desafioId) != null)
            return true;

        return false;
    }
    #endregion

    #region listarDesafiosConcluidos
    private void listarDesafiosConcluidos()
    {
        SvcDesafioConcluido.GetComponentsInChildren<DesafioObj>().ToList().ForEach(x => Destroy(x.gameObject));

        StartCoroutine(DesafioAPI.ListarDesafiosConcluidos((response, error) =>
        {
            if (response != null && response.desafios.Count > 0)
            {
                txtDesafioConcluidoVazio.SetActive(false);

                for (int i = 0; i < response.desafios.Count; i++)
                {
                    DesafioObj desafioObj = Instantiate(DesafioObjRef, SvcDesafioConcluido);
                    desafioObj.PreencherInfoConcluido(response.desafios[i]);
                }
            }
        }));
    }
    #endregion

    #region obterDesafioConcluido
    private void obterDesafioConcluido(string desafioId)
    {
        Dictionary<string, object> form = new Dictionary<string, object>()
        {
            { "desafioId", desafioId }
        };

        StartCoroutine(DesafioAPI.ObterDesafioConcluido(form, (response, error) =>
        {
            if (response != null)
            {
                txtDesafioConcluidoVazio.SetActive(false);

                DesafioObj desafioObj = Instantiate(DesafioObjRef, SvcDesafioConcluido);
                desafioObj.PreencherInfoConcluido(response);
            }
        }));
    }
    #endregion
}
