using APIModel;
using UnityEngine;
using UnityEngine.UI;

public class CorreioObj : MonoBehaviour
{
    public Text TxtTitulo;
    public Text TxtMensagem;
    public Button BtnLerMensagem;
    public GameObject ImgNaoVisualizado;

    [HideInInspector]
    public Correio.Mensagem mensagem;

    private void Awake()
    {
        configurarListener();
    }

    #region PreencherInfo
    public void PreencherInfo(Correio.Mensagem mensagem)
    {
        this.mensagem = mensagem;

        TxtTitulo.text = mensagem.titulo;
        TxtMensagem.text = mensagem.mensagem;
        ImgNaoVisualizado.SetActive(!mensagem.lida);
    }
    #endregion

    #region configurarListener
    public void configurarListener()
    {
        BtnLerMensagem.onClick.AddListener(() => abrirPnlMensagem());
    }
    #endregion

    #region abrirPnlMensagem
    private void abrirPnlMensagem()
    {
        Main.Instance.MenuCorreio.AbrirPnlMensagem(mensagem);
    }
    #endregion
}
