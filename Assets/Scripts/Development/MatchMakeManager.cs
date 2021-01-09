using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using PlayFab;
using PlayFab.ClientModels;

public class MatchMakeManager : MonoBehaviourPunCallbacks
{
    public static MatchMakeManager matchMake;

    public const string LEVEL_PROP_KEY = "level";
    private LoadBalancingClient loadBalancingClient;
    private int playerLevel;

    #region UNITY Methods
    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        Launch();
    }
    #endregion

    #region UI Callback Methods
    public void JoinRoom()
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            GetStatistics();
            JoinRandomRoom(); // find random room to join
        }
    }
    #endregion

    #region Photon Callback Methods
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        //base.OnJoinRandomFailed(returnCode, message);
        Debug.Log(message);
        if (playerLevel > 1)    // keep searching until reach closest level
        {
            playerLevel--;
            JoinRandomRoom();
        }
        else // only if no match create a room
        {
            CreateAndJoinRoom();
        }
    }

    public override void OnCreatedRoom()
    {
        //base.OnCreateRoom();
        //Debug.Log("A room is created");
    }

    public override void OnJoinedRoom()
    {
        //base.OnJoinRoom();
        //Debug.Log("joined room");
        //PhotonNetwork.LoadLevel("Game");
        Debug.Log("in room : " + PhotonNetwork.InRoom);
        Debug.Log(playerLevel);
        
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("player entered now " + PhotonNetwork.CurrentRoom.PlayerCount + " in room");
        if(PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            PhotonNetwork.LoadLevel("Game");
        }
    }
    #endregion

    #region Private Methods
    public void Launch()
    {
        if (PhotonNetwork.IsConnected)
        {
            JoinRoom();
        }
        else 
        {
            // display not connected, connect and reload
            PhotonNetwork.ConnectUsingSettings();
            
        }
    }

    private void JoinRandomRoom()
    {
        ExitGames.Client.Photon.Hashtable expectedCustomRoomProperties = new ExitGames.Client.Photon.Hashtable { { LEVEL_PROP_KEY, playerLevel } };
        PhotonNetwork.JoinRandomRoom(expectedCustomRoomProperties,2);
    }
    private void CreateAndJoinRoom()
    {
        string[] roomPropsInLobby = { LEVEL_PROP_KEY };
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2;
        // roomOptions.CustomRoomPropertiesForLobby = { LEVEL_PROP_KEY };
        roomOptions.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable { { LEVEL_PROP_KEY, playerLevel } };
        //EnterRoomParams enterRoomParams = new EnterRoomParams();
        //enterRoomParams.RoomOptions = roomOptions;
        //loadBalancingClient.OpCreateRoom(enterRoomParams);

        roomOptions.CustomRoomPropertiesForLobby = roomPropsInLobby;
        string randomRoomName = "Room" + Random.Range(0, 10000);

        PhotonNetwork.CreateRoom(randomRoomName, roomOptions);
    }
    #endregion

    void GetStatistics()
    {
        PlayFabClientAPI.GetPlayerStatistics(
            new GetPlayerStatisticsRequest(),
            OnGetStatistics,
            error => Debug.LogError(error.GenerateErrorReport())
        );
    }

    void OnGetStatistics(GetPlayerStatisticsResult result)
    {
        Debug.Log("Received the following Statistics:");
        foreach (var stat in result.Statistics)
        {
            if (stat.StatisticName.Equals("Level"))
                playerLevel = stat.Value;
        }
    }

    public void ExitMatchMake()
    {
        PhotonNetwork.LeaveRoom();

    }

    public override void OnLeftRoom()
    {
        PhotonNetwork.LoadLevel("Home");
    }
}
