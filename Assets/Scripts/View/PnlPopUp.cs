using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PnlPopUp : MonoBehaviour {

	public void FecharPopUp(GameObject pnl){
		
		SomController.Tocar(SomController.Som.Click_Cancel);
		AnimacoesTween.AnimarObjeto(EventSystem.current.currentSelectedGameObject, AnimacoesTween.TiposAnimacoes.Button_Click, () => 
		{
			AnimacoesTween.AnimarObjeto(pnl, AnimacoesTween.TiposAnimacoes.Scala, () => {
                Main.Instance.PnlPopUp.SetActive(false);
				pnl.SetActive(false);
			}, 0.5f, new Vector2(0,0));
		},
		0.1f);
	}
}
