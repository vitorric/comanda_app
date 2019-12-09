using APIModel;
using Network;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public enum TiposTutorial
    {
        Geral = 2,
        Profile = 0,
        Correio = 1,
        Desafio = 3
    }

    [Header("Geral")]
    public CanvasGroup CanvasGroup;
    public List<Transform> LstPosicoesQuadro;
    public MenuMain MenuMain;
    public GameObject EstabExemplo;

    [Header("Passo Historico Compra")]
    public GameObject TxtMsgHistoricoVazio;
    public List<GameObject> LstHistCompraExemplo;

    [Header("Passo Correio")]
    public Canvas PnlMensagemCorreio;

    [Header("Componentes Do Tutorial")]
    public GameObject PnlTutorial;
    public Transform TransIcon;
    public RectTransform PainelDestaque;
    public Text TxtDescricao;
    public GameObject Quadro;
    public Vector2 TamanhoQuadroNormal;
    public Vector2 TamanhoQuadroMaior;
    public Button BtnProximo;

    [Header("Tutorial Geral")]
    public Tutorial TutorialGeral;

    [Header("Tutorial Profile")]
    public Tutorial TutorialProfile;

    [Header("Tutorial Correio")]
    public Tutorial TutorialCorreio;

    [Header("Tutorial Desafio")]
    public Tutorial TutorialDesafio;

    private TiposTutorial tipoTutorial;
    private Tutorial.Passo passoAtual;
    private int etapaAtual = 1;
    private bool executandoTutorial = false;

    private List<string> lstVariaveis;

    private void Awake()
    {
        lstVariaveis = new List<string>
        {
            "{NOME}"
        };

        BtnProximo.onClick.AddListener(() =>
        {
            EasyAudioUtility.Instance.Play(EasyAudioUtility.Som.Click_OK);
            ProximaEtapa();
        });

    }

    public void ChecarTutorial(int tutorial, float tempo)
    {
        CanvasGroup.alpha = 0;
        bool iniciarTutorial = false;

        if (tutorial == (int)TiposTutorial.Geral && !Cliente.ClienteLogado.concluiuTutorialGeral ||
            tutorial == (int)TiposTutorial.Profile && !Cliente.ClienteLogado.concluiuTutorialProfile ||
            tutorial == (int)TiposTutorial.Correio && !Cliente.ClienteLogado.concluiuTutorialCorreio ||
            tutorial == (int)TiposTutorial.Desafio && !Cliente.ClienteLogado.concluiuTutorialDesafios)
        {
            iniciarTutorial = true;
            tipoTutorial = (TiposTutorial)tutorial;
        }

        if (iniciarTutorial && !executandoTutorial)
        {
            executandoTutorial = true;
            etapaAtual = 1;
            PnlTutorial.SetActive(true);

            StartCoroutine(carregarEtapa(0));
            return;
        }

        if (!executandoTutorial)
            PnlTutorial.SetActive(false);
    }

    public void IniciarTutorial(int tutorial)
    {
        if (!executandoTutorial)
        {
            tipoTutorial = (TiposTutorial)tutorial;
            executandoTutorial = true;
            etapaAtual = 1;
            PnlTutorial.SetActive(true);

            StartCoroutine(carregarEtapa(0));
        }
    }

    public void ProximaEtapa()
    {
        BtnProximo.interactable = false;

        if (passoAtual != null && passoAtual.PainelAlvo != null)
        {
            //reseta os componentes anteriores
            if (passoAtual.ObjetoAlvoInteracao != null)
                passoAtual.ObjetoAlvoInteracao.GetComponent<Button>().onClick.RemoveListener(ProximaEtapa);

            if (passoAtual.PainelAlvo != null)
            {
                passoAtual.PainelAlvo.gameObject.GetComponentsInChildren<Button>().ToList().ForEach(x => x.interactable = true);
                passoAtual.PainelAlvo.SetParent(passoAtual.PainelAlvoPai);
            }
        }

        if (tipoTutorial == TiposTutorial.Geral && etapaAtual == TutorialGeral.Passos.Count)
        {
            //Concluiu
            StartCoroutine(concluiuTutorial());
            return;
        }

        if (tipoTutorial == TiposTutorial.Profile && etapaAtual == TutorialProfile.Passos.Count)
        {
            //Concluiu
            StartCoroutine(concluiuTutorial());
            return;
        }

        if (tipoTutorial == TiposTutorial.Correio && etapaAtual == TutorialCorreio.Passos.Count)
        {
            //Concluiu
            StartCoroutine(concluiuTutorial());
            return;
        }

        if (tipoTutorial == TiposTutorial.Desafio && etapaAtual == TutorialDesafio.Passos.Count)
        {
            //Concluiu
            StartCoroutine(concluiuTutorial());
            return;
        }

        if (passoAtual.AcaoProximo != null)
            passoAtual.AcaoProximo.Invoke();

        etapaAtual++;
        StartCoroutine(carregarEtapa(passoAtual.tempoTransicaoTela));
    }

    private IEnumerator carregarEtapa(float tempoEspera)
    {
        yield return new WaitForSeconds(tempoEspera);

        if (tipoTutorial == TiposTutorial.Geral)
            passoAtual = TutorialGeral.Passos.Find(x => x.Ordem == etapaAtual);

        if (tipoTutorial == TiposTutorial.Profile)
            passoAtual = TutorialProfile.Passos.Find(x => x.Ordem == etapaAtual);

        if (tipoTutorial == TiposTutorial.Correio)
            passoAtual = TutorialCorreio.Passos.Find(x => x.Ordem == etapaAtual);

        if (tipoTutorial == TiposTutorial.Desafio)
            passoAtual = TutorialDesafio.Passos.Find(x => x.Ordem == etapaAtual);

        //encontra o passo
        //passoAtual = Tutorial.Passos.Find(x => x.Ordem == etapaAtual);

        if (passoAtual.AcaoAntes != null)
            passoAtual.AcaoAntes.Invoke();

        configurarPainelDestaque();
        configurarQuadroDescricao();
        configurarIcone();
        CanvasGroup.alpha = 1;
        BtnProximo.interactable = true;
    }

    private void configurarIcone()
    {
        if (passoAtual.PosAlvo != null)
        {
            TransIcon.gameObject.SetActive(true);

            //muda posicao do icon
            TransIcon.position = passoAtual.PosAlvo.position;

            rotacionarIcon(passoAtual);

            return;
        }

        TransIcon.gameObject.SetActive(false);
    }

    private void configurarQuadroDescricao()
    {
        Quadro.transform.SetParent(LstPosicoesQuadro[(int)passoAtual.PosicaoQuadro]);
        Quadro.transform.localPosition = Vector3.zero;

        bool contemVariaveis = false;

        for (int i = 0; i < lstVariaveis.Count; i++)
        {
            if (passoAtual.Descricao.Contains(lstVariaveis[i]))
            {
                contemVariaveis = true;
                break;
            }
        }

        if (!contemVariaveis)
            TxtDescricao.text = passoAtual.Descricao;

        BtnProximo.gameObject.SetActive(passoAtual.ExibirBotaoDescricao);
    }

    private void configurarPainelDestaque()
    {
        if (passoAtual.PainelAlvo != null)
        {
            PainelDestaque.gameObject.SetActive(true);
            //muda posicoes do painel de destaque
            PainelDestaque.position = passoAtual.PainelAlvo.position;
            PainelDestaque.sizeDelta = new Vector2(passoAtual.PainelAlvo.sizeDelta.x, passoAtual.PainelAlvo.sizeDelta.y);

            passoAtual.PainelAlvo.SetParent(PainelDestaque);
            passoAtual.PainelAlvo.localScale = Vector3.one;

            //if (passoAtual.PainelAlvo.GetComponent<Button>() != null)
            //    passoAtual.PainelAlvo.GetComponent<Button>().onClick.AddListener(ProximaEtapa);

            passoAtual.PainelAlvo.gameObject.GetComponentsInChildren<Button>().ToList().ForEach(x => x.interactable = false);

            if (passoAtual.ObjetoAlvoInteracao != null)
            {
                passoAtual.ObjetoAlvoInteracao.interactable = true;
                passoAtual.ObjetoAlvoInteracao.onClick.AddListener(ProximaEtapa);
            }

            //passoAtual.PainelAlvo.gameObject.AddComponent<CanvasGroup>().interactable = passoAtual.HabilitarInteracaoAlvo;

            return;
        }

        PainelDestaque.gameObject.SetActive(false);
    }

    private void rotacionarIcon(Tutorial.Passo passo)
    {
        TransIcon.localRotation = new Quaternion(0, 0, 0, 0);

        if (passo.RotacaoIcon == Tutorial.rotacaoIcon.Esquerda)
            TransIcon.rotation = new Quaternion(0, 0, 0, 0);
        if (passo.RotacaoIcon == Tutorial.rotacaoIcon.Direita)
            TransIcon.rotation = new Quaternion(0, 90, 0, 0);
        if (passo.RotacaoIcon == Tutorial.rotacaoIcon.Baixo)
            TransIcon.Rotate(Vector3.forward * -90);
        if (passo.RotacaoIcon == Tutorial.rotacaoIcon.Cima)
            TransIcon.Rotate(Vector3.forward * 90);
    }


    #region Passo1_Antes
    public void Passo1_Antes()
    {
        DeixarQuadroMaior();
        TxtDescricao.text = passoAtual.Descricao.Replace("{NOME}", Cliente.ClienteLogado.apelido);
    }
    #endregion

    #region DeixarQuadroMaior
    public void DeixarQuadroMaior()
    {
        RectTransform rect = Quadro.GetComponent<RectTransform>();
        rect.sizeDelta = TamanhoQuadroMaior;
        Quadro.transform.localPosition = Vector3.zero;
    }
    #endregion

    #region DeixarQuadroPequeno
    public void DeixarQuadroPequeno()
    {
        RectTransform rect = Quadro.GetComponent<RectTransform>();
        rect.sizeDelta = TamanhoQuadroNormal;
        Quadro.transform.localPosition = Vector3.zero;
    }
    #endregion

    #region ExibirEstabExemplo
    public void ExibirEstabExemplo()
    {
        EstabExemplo.SetActive(true);
    }
    #endregion

    #region ExcluirEstabExemplo
    public void ExcluirEstabExemplo()
    {
        Destroy(EstabExemplo);
    }
    #endregion

    #region ExibirHistoricoExemplo
    public void ExibirHistoricoExemplo()
    {
        TxtMsgHistoricoVazio.SetActive(false);
        LstHistCompraExemplo.ForEach(x => x.SetActive(true));
    }
    #endregion

    #region ExcluirHistoricoExemplo
    public void ExcluirHistoricoExemplo()
    {
        LstHistCompraExemplo.ForEach(x => Destroy(x));
        LstHistCompraExemplo.Clear();
        TxtMsgHistoricoVazio.SetActive(true);
    }
    #endregion

    #region HabilitarQuadroMensagem
    public void HabilitarQuadroMensagem()
    {
        PnlMensagemCorreio.enabled = true;
    }
    #endregion

    #region concluiuTutorial
    public IEnumerator concluiuTutorial()
    {
        bool enviarConclusaoTuto = false;
        executandoTutorial = false;
        PnlTutorial.SetActive(false);
        string tutorialConcluido = string.Empty;
        passoAtual = null;
        etapaAtual = 1;
        DeixarQuadroPequeno();

        if (tipoTutorial == TiposTutorial.Geral && !Cliente.ClienteLogado.concluiuTutorialGeral)
        {
            tutorialConcluido = "concluiuTutorialGeral";
            Cliente.ClienteLogado.concluiuTutorialGeral = true;
            enviarConclusaoTuto = true;
        }

        if (tipoTutorial == TiposTutorial.Profile && !Cliente.ClienteLogado.concluiuTutorialProfile)
        {
            tutorialConcluido = "concluiuTutorialProfile";
            Cliente.ClienteLogado.concluiuTutorialProfile = true;
            enviarConclusaoTuto = true;
        }

        if (tipoTutorial == TiposTutorial.Correio && !Cliente.ClienteLogado.concluiuTutorialCorreio)
        {
            tutorialConcluido = "concluiuTutorialCorreio";
            Cliente.ClienteLogado.concluiuTutorialCorreio = true;
            enviarConclusaoTuto = true;
        }

        if (tipoTutorial == TiposTutorial.Desafio && !Cliente.ClienteLogado.concluiuTutorialDesafios)
        {
            tutorialConcluido = "concluiuTutorialDesafios";
            Cliente.ClienteLogado.concluiuTutorialDesafios = true;
            enviarConclusaoTuto = true;
        }

        if (enviarConclusaoTuto)
        {
            AppManager.Instance.AtivarLoader();

            Dictionary<string, object> form = new Dictionary<string, object>
            {
                { "tipoTutorial", tutorialConcluido }
            };

            yield return StartCoroutine(ClienteAPI.ClienteConcluiuTutorial(form, (response, error) =>
            {
                if (error != null)
                {
                    AppManager.Instance.DesativarLoaderAsync();
                    Debug.Log(error);
                    AlertaManager.Instance.ChamarAlertaMensagem(error, false);
                    return;
                }
            }));

            AppManager.Instance.DesativarLoaderAsync();
        }
    }
    #endregion

    private void voltarBtnMenuNormal()
    {
        RectTransform rect = passoAtual.PainelAlvo.GetComponent<RectTransform>();
        rect.offsetMax = new Vector2(0, 0);
        rect.offsetMin = new Vector2(0, 0);
    }
}
