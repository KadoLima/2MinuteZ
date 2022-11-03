using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Facebook.Unity;
using PlayFab;
using UnityEngine.SceneManagement;

public class PlayfabAuthentication : MonoBehaviour
{
    public string titleID;


    // Start is called before the first frame update
    void Start()
    {
        if (FB.IsInitialized)
            return;

        FB.Init(() => FB.ActivateApp());
    }

    public void LoginWithFacebook()
    {
        FB.LogInWithReadPermissions(new List<string> { "public profile", "email" },Res =>
         {
             LoginWithPlayfab(); 
         });
    }
    
    public void LoginWithPlayfab()
    {
        Debug.Log("1111111");
        PlayFabClientAPI.LoginWithFacebook(new PlayFab.ClientModels.LoginWithFacebookRequest
        {
            TitleId = titleID,
            AccessToken = AccessToken.CurrentAccessToken.TokenString,
            CreateAccount = true

        }, PlayfabLoginSuccess, PlayfabLoginFailed);
    }

    public void PlayfabLoginSuccess(PlayFab.ClientModels.LoginResult result)
    {
        Debug.Log("LOGIN SUCCESS!");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void PlayfabLoginFailed(PlayFabError error)
    {
        Debug.Log("LOGIN FAILED!");
    }
}
