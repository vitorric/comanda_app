using Facebook.Unity;
using System;
using System.Collections.Generic;
using UnityEngine;

public class FacebookManager
{
    public string tipoLogin;
    public Action<string> relogar;
    public Action<string, string, string, string> cadastrar;

    public void Init()
    {
        if (!FB.IsInitialized)
        {
            // Initialize the Facebook SDK
            FB.Init(InitCallback, OnHideUnity);
        }
        else
        {
            // Already initialized, signal an app activation App Event
            FB.ActivateApp();
        }
    }

    private void InitCallback()
    {
        if (FB.IsInitialized)
        {
            // Signal an app activation App Event
            FB.ActivateApp();
            // Continue with Facebook SDK

            if (FB.IsLoggedIn)
            {
                // Get data from Facebook to personalize the player's experience
                if (tipoLogin == "facebook")
                {
                    relogar("facebook");
                }
            }
            else
            {
                // Prompt the user to log in, or offer a "guest" experience
                //login nao valido
                if (tipoLogin == "facebook")
                {
                    PlayerPrefs.DeleteAll();
                }
            }
        }
        else
        {
            Debug.Log("Failed to Initialize the Facebook SDK");
        }
    }
    private void OnHideUnity(bool isGameShown)
    {
        if (!isGameShown)
        {
            // Pause the game - we will need to hide
            Time.timeScale = 0;
        }
        else
        {
            // Resume the game - we're getting focus again
            Time.timeScale = 1;
        }
    }

    public void LoginFacebook()
    {
        var perms = new List<string>() { "public_profile", "email" };
        FB.LogInWithReadPermissions(perms, AuthCallback);
    }

    private void AuthCallback(ILoginResult result)
    {
        if (FB.IsLoggedIn)
        {
            // AccessToken class will have session details
            var aToken = AccessToken.CurrentAccessToken;
            // Print current access token's User ID
            Debug.Log(aToken.UserId);
            // Print current access token's granted permissions
            foreach (string perm in aToken.Permissions)
            {
                Debug.Log(perm);
            }

            FB.API("/me?fields=id,name,email", HttpMethod.GET, GetFacebookInfo, new Dictionary<string, string>() { });
        }
        else
        {
            //SetMsg("Falha ao conectar com o Facebook");
            Debug.Log("User cancelled login");
        }
    }

    public void GetFacebookInfo(IResult result)
    {
        if (result.Error == null)
        {
            cadastrar(result.ResultDictionary["id"].ToString(),
                      result.ResultDictionary["name"].ToString(),
                      result.ResultDictionary["email"].ToString(),
                      "facebook");
        }
        else
        {
            Debug.Log(result.Error);
        }
    }
}
