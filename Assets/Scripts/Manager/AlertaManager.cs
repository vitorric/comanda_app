using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlertaManager : MonoBehaviour
{

    public static AlertaManager Instance { get; set; }

    public Text TxtMensagem;
    public Button BtnFechar;
    public Animator animator;

    [Header("Response")]
    public Animator animatorResponse;
    public RawImage ImgSucesso;
    public RawImage ImgErro;

    public enum MsgAlerta
    {
        PreenchaOsCampos,
        SenhasNaoConferem,
        CPGoldInsuficiente,
        SairEstabComandaAberta,
        EmailNaoValido,
        NomeSobreNome,
        ApelidoMenor3Char,
        SenhaMenor6Char,
        PreenchaCPFValido,
        Menor18Anos,
        DataInvalida,
        EhNecessarioAceitarOsTermos,
        GPSError
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
            { MsgAlerta.SenhasNaoConferem, "Senhas não conferem!"},
            { MsgAlerta.SairEstabComandaAberta, "Não é possível sair de um estabelecimento tendo uma comanda aberta!"},
            { MsgAlerta.CPGoldInsuficiente, "CPGold insuficiente!"},
            { MsgAlerta.NomeSobreNome, "Digite o nome e o sobrenome!"},
            { MsgAlerta.EmailNaoValido, "Digite um email válido!"},
            { MsgAlerta.ApelidoMenor3Char, "O apelido tem que ter no mínimo 3 caracteres!" },
            { MsgAlerta.SenhaMenor6Char, "A senha tem que ter no mínimo 6 caracteres!"  },
            { MsgAlerta.PreenchaCPFValido, "Preencha um CPF válido!"  },
            { MsgAlerta.DataInvalida, "Digite uma data válida!"},
            { MsgAlerta.Menor18Anos, "Desculpe, você precisa ter mais de 18 anos!" },
            { MsgAlerta.EhNecessarioAceitarOsTermos, "É necessário aceitar os termos para finalizar o cadastro!" },
            { MsgAlerta.GPSError, "GPS desabilitado ou fora de cobertura!" }
        };
    }

    public void IniciarAlerta(bool sucesso)
    {
        ChamarAlertaResponse(sucesso);
    }

    public void ChamarAlertaResponse(bool sucesso)
    {
        ImgSucesso.gameObject.SetActive(false);
        ImgErro.gameObject.SetActive(false);

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

        animatorResponse.SetTrigger("show");
    }

    public void ChamarAlertaMensagem(string mensagem, bool sucesso)
    {
        if (sucesso)
        {
            EasyAudioUtility.Instance.Play(EasyAudioUtility.Som.Success);
        }
        else
        {
            EasyAudioUtility.Instance.Play(EasyAudioUtility.Som.Error);
        }

        animator.SetTrigger("show");
        TxtMensagem.text = mensagem;
    }

    public void ChamarAlertaMensagem(MsgAlerta msgAlerta, bool sucesso)
    {
        if (sucesso)
        {
            EasyAudioUtility.Instance.Play(EasyAudioUtility.Som.Success);
        }
        else
        {
            EasyAudioUtility.Instance.Play(EasyAudioUtility.Som.Error);
        }

        TxtMensagem.text = msgsApp[msgAlerta];
        animator.SetTrigger("show");
    }

    public void ChamarAlertaNotificacao(string mensagem)
    {
        EasyAudioUtility.Instance.Play(EasyAudioUtility.Som.MailSound);

        TxtMensagem.text = mensagem;
        animator.SetTrigger("show");
    }

    private void fechar()
    {
        EasyAudioUtility.Instance.Play(EasyAudioUtility.Som.Click_Cancel);
        animator.SetTrigger("hide");
    }
}
