using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PnlEdicaoChar : MonoBehaviour {

	public GameObject PnlEdicao;

	public GameObject AvatarHomem;
	public GameObject AvatarMulher;

	// Use this for initialization
	void Start () {
		
	}
	
	public void BtnFecharPnl(){

		SomController.Tocar(SomController.Som.Click_Cancel);
		AnimacoesTween.AnimarObjeto(EventSystem.current.currentSelectedGameObject, AnimacoesTween.TiposAnimacoes.Button_Click, () => 
		{
			AnimacoesTween.AnimarObjeto(PnlEdicao, AnimacoesTween.TiposAnimacoes.Scala, () => {
				PnlEdicao.SetActive(false);
			}, 0.5f, new Vector2(0,0));
		},
		0.1f);
	}

	public void AbrirPnlEdicao(bool avatarHomem){
		if (avatarHomem){
			AvatarHomem.SetActive(true);
			AvatarMulher.SetActive(false);
		}
		else{
			AvatarMulher.SetActive(true);
			AvatarHomem.SetActive(false);
		}
		SomController.Tocar(SomController.Som.Click_OK);
		AnimacoesTween.AnimarObjeto(EventSystem.current.currentSelectedGameObject, AnimacoesTween.TiposAnimacoes.Button_Click, () => 
		{
			PnlEdicao.SetActive(true);
			AnimacoesTween.AnimarObjeto(PnlEdicao, AnimacoesTween.TiposAnimacoes.Scala, () => {
				
			}, 0.5f, new Vector2(1,1));
		},
		0.1f);
	}
}
