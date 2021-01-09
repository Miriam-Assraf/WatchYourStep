using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class MenuManager : MonoBehaviour
{
    public GameObject menu;
    public GameObject connectToServer;

    #region UNITY Methods
    // Start is called before the first frame update
    void Start()
    {
        if (PhotonNetwork.IsConnected)
        {
            connectToServer.SetActive(false);
            menu.SetActive(true);
        }
        Debug.Log("is connected and ready : " + PhotonNetwork.IsConnectedAndReady);
        Debug.Log("is in room : " + PhotonNetwork.InRoom);
    }
    #endregion

    #region UI Callback Methods
    public void QuitGame()
    {
        Application.Quit();
    }

    public void StartGame()
    {
        PhotonNetwork.LoadLevel("MatchMake");
    }
    #endregion
}
