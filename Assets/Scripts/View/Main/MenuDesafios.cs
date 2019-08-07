using APIModel;
using FirebaseModel;
using Network;
using System;
using System.Collections;
using System.Collections.Generic;
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
    private List<DesafioObj> lstDesafiosConcluido;

    [HideInInspector]
    public ClienteFirebase ClienteFirebase;

    private void Awake()
    {
        configurarListener();

        lstDesafiosProgresso = new List<DesafioObj>();
        lstDesafiosConcluido = new List<DesafioObj>();

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

        ClienteFirebase.Watch(Cliente.ClienteLogado._id, true);
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
    private void adicionarDesafio(Cliente.Desafio desafio)
    {
        if (desafio != null)
        {
            DesafioObj desafioObj = null;

            if (desafio.concluido)
            {
                desafioObj = Instantiate(DesafioObjRef, SvcDesafioConcluido);
                lstDesafiosConcluido.Add(desafioObj);
                txtDesafioConcluidoVazio.SetActive(false);
            }
            else
            {
                desafioObj = Instantiate(DesafioObjRef, SvcDesafioProgresso);
                lstDesafiosProgresso.Add(desafioObj);
                txtDesafioProgressoVazio.SetActive(false);
            }

            obterDesafio(desafio, desafioObj);
        }
    }
    #endregion

    #region modificarDesafio
    private void modificarDesafio(Cliente.Desafio desafio)
    {
        DesafioObj desafioObj = lstDesafiosProgresso.Find(x => x.DesafioCliente._id == desafio._id);

        if (desafioObj != null)
        {
            if (desafio.concluido)
            {
                lstDesafiosProgresso.Remove(desafioObj);
                lstDesafiosConcluido.Add(desafioObj);
                desafioObj.transform.SetParent(SvcDesafioConcluido);

                if (lstDesafiosProgresso.Count == 0)
                {
                    txtDesafioProgressoVazio.SetActive(true);
                }
                
                if (lstDesafiosConcluido.Count > 0 && txtDesafioConcluidoVazio.activeInHierarchy)
                {
                    txtDesafioConcluidoVazio.SetActive(false);
                }
            }

            obterDesafio(desafio, desafioObj);
            return;
        }

        desafioObj = lstDesafiosConcluido.Find(x => x.DesafioCliente._id == desafio._id);

        if (desafioObj != null)
        {
            obterDesafio(desafio, desafioObj);
        }
    }
    #endregion

    #region removerDesafio
    private void removerDesafio(Cliente.Desafio desafio)
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

        desafioObj = lstDesafiosConcluido.Find(x => x.DesafioCliente._id == desafio._id);

        if (desafioObj != null)
        {
            Destroy(desafioObj.gameObject);
            lstDesafiosConcluido.Remove(desafioObj);


            if (lstDesafiosConcluido.Count == 0)
            {
                txtDesafioConcluidoVazio.SetActive(true);
            }
        }
    }
    #endregion

    #region obterDesafio
    private void obterDesafio(Cliente.Desafio desafio, DesafioObj desafioObj)
    {
        Dictionary<string, object> form = new Dictionary<string, object>
        {
            { "desafioId", desafio._id }
        };

        StartCoroutine(DesafioAPI.ObterDesafio(form,
        (response, error) =>
        {
            if (error != null)
            {
                Debug.Log("obterDesafio: " + error);
                StartCoroutine(AlertaManager.Instance.ChamarAlertaMensagem(error, false));
                return;
            }

            if (desafio.concluido)
                AppManager.Instance.AtivarDesafioCompletado(response);

            obterIcone(response.icon, FileManager.Directories.desafio, (textura) => {
                desafioObj.PreencherIcone(textura);
            });

            desafioObj.PreencherInfo(response, desafio);
        }));
    }
    #endregion

    #region obterIcone
    private void obterIcone(string nomeIcon, FileManager.Directories tipo, Action<Texture2D> callback)
    {
        if (FileManager.Exists(tipo, nomeIcon))
        {
            Texture2D texture2d = FileManager.ConvertToTexture2D(FileManager.LoadFile(tipo, nomeIcon));
            callback(texture2d);
            return;
        }

        StartCoroutine(DownloadAPI.DownloadImage(nomeIcon, tipo.ToString(), (texture, bytes) =>
        {
            FileManager.SaveFile(tipo, nomeIcon, bytes);
            callback(texture);
        }));
    }
    #endregion

    #region PararWatch
    public void PararWatch()
    {
        ClienteFirebase.Watch(Cliente.ClienteLogado._id, false);
    }
    #endregion

    #region BuscarDesafio
    public Cliente.Desafio BuscarDesafio(string _id)
    {
        DesafioObj clienteDesafio = null;

        clienteDesafio = lstDesafiosProgresso.Find(x => x.DesafioCliente._id == _id);

        if (clienteDesafio != null)
        {
            return clienteDesafio.DesafioCliente;
        }

        clienteDesafio = lstDesafiosConcluido.Find(x => x.DesafioCliente._id == _id);

        if (clienteDesafio != null)
        {
            return clienteDesafio.DesafioCliente;
        }

        return null;
    }
    #endregion
}
