using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour {

	public static IEnumerator CarregarCena(string nomeCena, GameObject pnlLoading)
    {
        pnlLoading.SetActive(true);
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(nomeCena);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        if (asyncLoad.isDone)
        {
            //
        }
    }

    public IEnumerator CarregarCenaAdditiveAsync(string nomeCena){
 
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(nomeCena, LoadSceneMode.Additive);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        if (asyncLoad.isDone)
        {

        }
    }

    public void CarregarCenaAdditive(string nomeCena)
    {
        SceneManager.LoadSceneAsync(nomeCena, LoadSceneMode.Additive);
    }

    public IEnumerator DescarregarCenaAdditive(string nomeCena)
    {
        AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(nomeCena);

        while (!asyncUnload.isDone)
        {
            yield return null;
        }

        if (asyncUnload.isDone)
        {

        }
    }

    public static string NomeCenaAtiva()
    {
        return SceneManager.GetActiveScene().name;
    }
}
