using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SomController : MonoBehaviour {	

	public enum Som {
		Background,
        Click_OK,
        Click_Cancel,
        Error,
        LevelUp,
        LevelUp2,
        Compra_Item,
        Som_Camera
	}

    public static void Tocar(Som som)
    {
        EasyAudioUtility.instance.Play(som.ToString());
    }

    public static void AjustarSomBG(float volume){
        EasyAudioUtility.instance.AjustarSomBG(volume);
    }
    public static void AjustarSomSFX(float volume){
        EasyAudioUtility.instance.AjustarSomSFX(volume);
    }

    private EasyAudioUtility audioUtility;

    void Awake() {
        audioUtility = FindObjectOfType<EasyAudioUtility>();
        audioUtility.helper.ToList().ForEach(x => x.canPlay = true);
    }

    void Start(){
        Tocar(Som.Background);
    }    
    
}
