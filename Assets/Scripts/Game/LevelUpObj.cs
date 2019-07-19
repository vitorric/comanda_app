using APIModel;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpObj : MonoBehaviour
{
    public Text TxtLevelUp;
    public Button BtnFechar;

    private void Awake()
    {
        BtnFechar.onClick.AddListener(() => BtnPnlFechar());
    }

    void OnEnable()
    {
        EasyAudioUtility.Instance.Play(EasyAudioUtility.Som.LevelUp);
        TxtLevelUp.text = Cliente.ClienteLogado.avatar.info.level.ToString();
    }

    public void BtnPnlFechar()
    {
        EasyAudioUtility.Instance.Play(EasyAudioUtility.Som.Click_Cancel);

        AppManager.Instance.DesativarLevelUp();
    }
}
