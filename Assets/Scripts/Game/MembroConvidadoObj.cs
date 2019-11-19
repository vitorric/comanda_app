using APIModel;
using Network;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MembroConvidadoObj : MonoBehaviour
{
    public Text TxtNome;
    public Text TxtChaveAmigavel;
    public Button BtnDesconvidar;
    public Transform PnlAvatar;
    public AvatarObj Avatar;

    private ConvitesComanda conviteComanda;
    private void Awake()
    {
        configurarListener();
    }

    #region configurarListener
    private void configurarListener()
    {
        BtnDesconvidar.onClick.AddListener(() => btnDesconvidar());
    }
    #endregion

    #region PreencherInfo
    public void PreencherInfo(ConvitesComanda convite)
    {
        conviteComanda = convite;

        TxtNome.text = convite.apelido;
        TxtChaveAmigavel.text = convite.chaveAmigavel;

        Instantiate(Avatar, PnlAvatar).PreencherInfo(convite.sexo, convite.avatar);
    }
    #endregion

    #region btnDesconvidar
    private void btnDesconvidar()
    {
        Dictionary<string, object> form = new Dictionary<string, object>
        {
            { "chaveAmigavel", conviteComanda.chaveAmigavel }
        };

        StartCoroutine(ComandaAPI.CancelarConviteMembroGrupo(form,
        (response, error) =>
        {

            if (error != null)
            {
                Debug.Log("btnDesconvidar: " + error);
                AlertaManager.Instance.ChamarAlertaMensagem(error, false);
                return;
            }

            AlertaManager.Instance.IniciarAlerta(true);

            Destroy(gameObject);
        }));
    }
    #endregion
}
