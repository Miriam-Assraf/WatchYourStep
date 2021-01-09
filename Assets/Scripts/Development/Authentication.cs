using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class Authentication : MonoBehaviourPunCallbacks
{
    public static string custom_id = string.Empty;
    private string _playFabPlayerIdCache;
    public string userEmail;
    public string userPassword;
    public string username;
    public GameObject menu;
    public GameObject connectToServer;
    public GameObject login;
    public GameObject loginFail;
    public GameObject connecting;
    public Text errorMsg;

    //public UserDataManagment userDataManagment;

    // Start is called before the first frame update
    void Awake()
    {
#if UNITY_ANDROID
        LoginWithIOS();
#endif
    }

    #region PlayFab API calls
    public void LoginWithIOS()
    {
        string deviceID = SystemInfo.deviceUniqueIdentifier;

        var requestAndroid = new LoginWithAndroidDeviceIDRequest { AndroidDeviceId = deviceID, CreateAccount = true };
        PlayFabClientAPI.LoginWithAndroidDeviceID(requestAndroid, RequestPhotonToken, OnLoginFailure);
    }
 
    public void LoginWithCustomID()
    {
        var request = new LoginWithCustomIDRequest
        {
            CustomId = PlayFabSettings.DeviceUniqueIdentifier,
            CreateAccount = true
        };

        PlayFabClientAPI.LoginWithCustomID(request, RequestPhotonToken, OnLoginFailure);
    }

    public void LoginWithEmail()
    {
        var request = new LoginWithEmailAddressRequest
        {
            Email = userEmail,
            Password = userPassword
        };
        PlayFabClientAPI.LoginWithEmailAddress(request, RequestPhotonToken, OnLoginWithEmailFailure);
    }
    #endregion

    #region Connect With Photon
    private void RequestPhotonToken(LoginResult obj)
    {
        _playFabPlayerIdCache = obj.PlayFabId;
        //userDataManagment.GetUserData(_playFabPlayerIdCache);

        PlayFabClientAPI.GetPhotonAuthenticationToken(new GetPhotonAuthenticationTokenRequest()
        {
            PhotonApplicationId = PhotonNetwork.PhotonServerSettings.AppSettings.AppIdRealtime
        }, AuthenticateWithPhoton, OnLoginFailure);

        connectToServer.SetActive(false);
        login.SetActive(false);
        loginFail.SetActive(false);
        connecting.SetActive(true);
    }

    private void AuthenticateWithPhoton(GetPhotonAuthenticationTokenResult obj)
    {
        AuthenticationValues customAuth = new AuthenticationValues(_playFabPlayerIdCache);
        customAuth.AuthType = CustomAuthenticationType.Custom;
        customAuth.AddAuthParameter("username", _playFabPlayerIdCache);    // expected by PlayFab custom auth service
        customAuth.AddAuthParameter("token", obj.PhotonCustomAuthenticationToken);
        PhotonNetwork.AuthValues = customAuth;
        PhotonNetwork.ConnectUsingSettings();
    }
    #endregion

    public void Register()
    {
        var registerRequest = new RegisterPlayFabUserRequest { Email = userEmail, Password = userPassword, Username = username };
        PlayFabClientAPI.RegisterPlayFabUser(registerRequest, OnRegisterSuccess, OnRegisterFailure);
    }

    private void OnLoginFailure(PlayFabError obj)
    {
        Debug.Log(obj.GenerateErrorReport());
    }

    private void OnLoginWithEmailFailure(PlayFabError error)
    {
        login.SetActive(false);
        loginFail.SetActive(true);
        errorMsg.text = "Invalid Email or password.";
    }

    private void OnRegisterSuccess(RegisterPlayFabUserResult result)
    {
        Debug.Log("registered");
        // remember player data to auto login
        PlayerPrefs.SetString("EMAIL", userEmail);
        PlayerPrefs.SetString("PASSWORD", userPassword);
        LoginWithEmail();
    }

    private void OnRegisterFailure(PlayFabError error)
    {
        // show failure and back to home
        Debug.Log(error.ErrorMessage);
    }

    #region UI Methods
    public void GetUserEmail(string emailInput)
    {
        userEmail = emailInput;
    }    

    public void GetUserPassword(string passwordInput)
    {
        userPassword = passwordInput;
    }

    public void GetUsername(string usernameInput)
    {
        username = usernameInput;
    }
    #endregion

    #region Photon Callback Methods
    public override void OnConnected()
    {
        Debug.Log("server available");
    }

    public override void OnConnectedToMaster()
    {
        connecting.SetActive(false);
        menu.SetActive(true);
        //Menu.SetActive(true);
        //Debug.Log("Connected to master server");
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogWarningFormat("PUN Basics Tutorial/Launcher: OnDisconnected() was called by PUN with reason {0}", cause);
    }
    #endregion
}
