using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hexagon : MonoBehaviour
{
    // Start is called before the first frame update
    private int numOfHits;
    public bool isHexTileBroken;
    public bool isHexOccupied;
    void Start()
    {
        numOfHits = 0;
        isHexTileBroken = false;
    }

    void hit()
    {
        ++numOfHits;

        // deactive hexagon after damage

        if (numOfHits == 3)
            isHexTileBroken = true;

        // active next hexagon after damage 
    }
}
