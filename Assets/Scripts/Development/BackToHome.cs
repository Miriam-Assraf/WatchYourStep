using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BackToHome : MonoBehaviour
{
    public void BackToMenu()
    {
        PhotonNetwork.LoadLevel("Home");
    }
}
