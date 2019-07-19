using APIModel;
using UnityEngine;
using UnityEngine.UI;

public class DesafioConcluidoObj : MonoBehaviour
{
    public Text TxtTituloDesafio;
    public Text TxtPremioDesafio;
    public Button BtnResgatarPremio;
    public GameObject PnlDesafioConquistado;

    private Desafio desafio;

    private void Awake()
    {
        BtnResgatarPremio.onClick.AddListener(() => resgatarPremio());
    }
    void OnEnable()
    {
        EasyAudioUtility.Instance.Play(EasyAudioUtility.Som.AchievWin);
    }

    #region PreencherInfo
    public void PreencherInfo(Desafio desafio)
    {
        this.desafio = desafio;
        TxtTituloDesafio.text = desafio.nome;
        TxtPremioDesafio.text = Util.FormatarValores(desafio.premio);
    }
    #endregion

    #region resgatarPremio
    private void resgatarPremio()
    {
        EasyAudioUtility.Instance.Play(EasyAudioUtility.Som.AchievResgate);

        AppManager.Instance.RemoverDesafioDaLista(desafio);
        desafio = null;
        PnlDesafioConquistado.SetActive(false);

        if (AppManager.Instance.ObterTamanhoListaDesafio() > 0)
        {
            AppManager.Instance.ExibirProximoDesafio();
        }

    }
    #endregion
}
