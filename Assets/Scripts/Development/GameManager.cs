using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class GameManager : MonoBehaviourPunCallbacks
{
    public GameObject Game;
    public GameObject Disconnected;

    // Start is called before the first frame update
    void Start()
    {
        Disconnected.SetActive(false);      
    }

    // Update is called once per frame
    void Update()
    {
        // if still in room check there are 2 players in room
        if (PhotonNetwork.InRoom)
        {
            if (PhotonNetwork.CurrentRoom.PlayerCount < 2)  // if there are not 2 players in room
            {
                // display disconnected canvas
                Game.SetActive(false);
                Disconnected.SetActive(true);
            }
        }
    }

    public void BackToMenu()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        // after left room go back to home page
        PhotonNetwork.LoadLevel("Home");
    }
}
