using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlertaManager : MonoBehaviour
{

    public static AlertaManager Instance { get; set; }


    public GameObject PnlPai;
    public GameObject ObjAlertaMensagem;
    public Text TxtMensagem;
    public Button BtnFechar;

    public GameObject ObjAlertaResponse;
    public RawImage ImgSucesso;
    public RawImage ImgErro;


    private bool animando = false;

    public enum MsgAlerta
    {
        PreenchaOsCampos,
        SenhasNaoConferem
    }

    private Dictionary<MsgAlerta, string> msgsApp;

    void Awake()
    {
        if (Instance != null)
            Destroy(this);

        DontDestroyOnLoad(gameObject);
        Instance = this;

        BtnFechar.onClick.AddListener(() => fechar());

        msgsApp = new Dictionary<MsgAlerta, string>
        {
            { MsgAlerta.PreenchaOsCampos, "Preencha todos os campos!"},
            { MsgAlerta.SenhasNaoConferem, "Senhas não conferem!"}
        };
    }

    public IEnumerator ChamarAlertaResponse(bool sucesso)
    {
        ObjAlertaResponse.SetActive(true);


        if (sucesso)
        {
            EasyAudioUtility.Instance.Play(EasyAudioUtility.Som.Success);
            ImgSucesso.gameObject.SetActive(true);
        }
        else
        {
            EasyAudioUtility.Instance.Play(EasyAudioUtility.Som.Error);
            ImgErro.gameObject.SetActive(true);
        }

        if (!animando)
        {
            animando = true;

            PnlPai.SetActive(true);
            StartCoroutine(Animacoes.Mover(ObjAlertaResponse, Animacoes.Posicao.Y, -10, 545));
            yield return new WaitForSeconds(1.5f);
            StartCoroutine(Animacoes.Mover(ObjAlertaResponse, Animacoes.Posicao.Y, 10, 765));
            yield return new WaitForSeconds(0.6f);
            PnlPai.SetActive(false);
            animando = false;
            ObjAlertaResponse.SetActive(false);
            ImgSucesso.gameObject.SetActive(false);
            ImgErro.gameObject.SetActive(false);
        }
    }

    public IEnumerator ChamarAlertaMensagem(string mensagem, bool sucesso)
    {
        ObjAlertaMensagem.SetActive(true);

        if (sucesso)
        {
            EasyAudioUtility.Instance.Play(EasyAudioUtility.Som.Success);
        }
        else
        {
            EasyAudioUtility.Instance.Play(EasyAudioUtility.Som.Error);
        }

        if (!animando)
        {
            animando = true;

            PnlPai.SetActive(true);
            TxtMensagem.text = mensagem;
            StartCoroutine(Animacoes.Mover(ObjAlertaMensagem, Animacoes.Posicao.Y, -10, 545));
            yield return new WaitForSeconds(2);
            StartCoroutine(Animacoes.Mover(ObjAlertaMensagem, Animacoes.Posicao.Y, 10, 765));
            yield return new WaitForSeconds(0.6f);
            PnlPai.SetActive(false);
            animando = false;
            ObjAlertaMensagem.SetActive(false);
        }
    }

    public IEnumerator ChamarAlertaMensagem(MsgAlerta msgAlerta, bool sucesso)
    {
        ObjAlertaMensagem.SetActive(true);

        if (sucesso)
        {
            EasyAudioUtility.Instance.Play(EasyAudioUtility.Som.Success);
        }
        else
        {
            EasyAudioUtility.Instance.Play(EasyAudioUtility.Som.Error);
        }

        if (!animando)
        {
            animando = true;

            PnlPai.SetActive(true);
            TxtMensagem.text = msgsApp[msgAlerta];
            StartCoroutine(Animacoes.Mover(ObjAlertaMensagem, Animacoes.Posicao.Y, -10, 545));
            yield return new WaitForSeconds(2);
            StartCoroutine(Animacoes.Mover(ObjAlertaMensagem, Animacoes.Posicao.Y, 10, 765));
            yield return new WaitForSeconds(0.6f);
            PnlPai.SetActive(false);
            animando = false;
            ObjAlertaMensagem.SetActive(false);
        }
    }

    private void fechar()
    {
        EasyAudioUtility.Instance.Play(EasyAudioUtility.Som.Click_Cancel);
        PnlPai.SetActive(false);
    }
}
