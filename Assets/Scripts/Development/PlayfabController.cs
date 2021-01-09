using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayfabController : MonoBehaviour
{
    public static PlayfabController PFC;

    private void OnEnable()
    {
        // make singleton
        if(PlayfabController.PFC == null)
        {
            PlayfabController.PFC = this;
        }

        else if (PlayfabController.PFC!=this)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
