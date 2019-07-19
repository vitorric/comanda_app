using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class MenuMain : MonoBehaviour
{
    public Button BtnHome;
    public Button BtnChat;
    public Button BtnEstabelecimentos;
    public Button BtnDesafios;
    public Button BtnUltimo;
    public Button BtnPerfil;
    public List<RectTransform> LstMenus;
    public List<RectTransform> LstIconMenus;
    public List<RectTransform> LstTxtMenu;

    public HorizontalScrollSnap HorizontalScrollSnap;

    private Vector2 tamanhoPadraoBotao;
    private Vector2 tamanhoBotaoSelecionado;
    private Vector2 scalaIconBotaoSelecionado;

    bool tocarSom = false;

    private void Awake()
    {
        tamanhoPadraoBotao = new Vector2(200, 200);
        tamanhoBotaoSelecionado = new Vector2(255, 255);
        scalaIconBotaoSelecionado = new Vector2(1.3f, 1.3f);

        adicionarListener();

        trocarPainel(2);

        HorizontalScrollSnap.OnSelectionPageChangedEvent.AddListener(trocarPainel);
        tocarSom = true;
    }

    #region adicionarListener
    private void adicionarListener()
    {
        BtnHome.onClick.AddListener(() => btnTrocarPainel(0));
        BtnChat.onClick.AddListener(() => btnTrocarPainel(1));
        BtnEstabelecimentos.onClick.AddListener(() => btnTrocarPainel(2));
        BtnDesafios.onClick.AddListener(() => btnTrocarPainel(3));
        BtnUltimo.onClick.AddListener(() => btnTrocarPainel(4));

        BtnPerfil.onClick.AddListener(() => btnTrocarPainel(0));
    }
    #endregion

    private void trocarPainel(int indexPainel)
    {
        if (tocarSom)
            EasyAudioUtility.Instance.Play(EasyAudioUtility.Som.Click_OK);

        LstMenus.ForEach(x => x.sizeDelta = tamanhoPadraoBotao);
        LstIconMenus.ForEach(x =>
        {
            x.localScale = Vector2.one;
            x.localPosition = new Vector2(x.localPosition.x, 15);
        });
        LstTxtMenu.ForEach(x => x.gameObject.SetActive(false));

        LstMenus[indexPainel].sizeDelta = tamanhoBotaoSelecionado;
        LstIconMenus[indexPainel].localScale = scalaIconBotaoSelecionado;
        LstIconMenus[indexPainel].localPosition = new Vector2(LstIconMenus[indexPainel].localPosition.x, 40);
        LstTxtMenu[indexPainel].gameObject.SetActive(true);
        LstTxtMenu[indexPainel].localPosition = new Vector2(LstTxtMenu[indexPainel].localPosition.x, -60);
    }

    private void btnTrocarPainel(int indexPainel)
    {

        HorizontalScrollSnap.GoToScreen(indexPainel);
    }

}
