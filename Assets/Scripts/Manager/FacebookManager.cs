using Facebook.Unity;
using System;
using System.Collections.Generic;
using UnityEngine;

public class FacebookManager
{
    public string tipoLogin;
    public Action<string, string> relogar;
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
                    var aToken = AccessToken.CurrentAccessToken;

                    relogar("facebook", aToken.UserId);
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
        AppManager.Instance.AtivarLoader();
        var perms = new List<string>() { "public_profile", "email" };
        FB.LogInWithReadPermissions(perms, AuthCallback);
    }

    public void LogoutFacebook()
    {
        FB.LogOut();
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
            AppManager.Instance.DesativarLoaderAsync();
            //SetMsg("Falha ao conectar com o Facebook");
            Debug.Log("User cancelled login");
        }
    }

    public void GetFacebookInfo(IResult result)
    {
        if (result.Error == null)
        {
            string email = result.ResultDictionary.ContainsKey("email") ? result.ResultDictionary["email"].ToString() : null;

            cadastrar(result.ResultDictionary["id"].ToString(),
                      result.ResultDictionary["name"].ToString(),
                      email,
                      "facebook");
        }
        else
        {
            Debug.Log(result.Error);
        }
    }
}
