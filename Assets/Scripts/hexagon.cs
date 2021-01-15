using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hexagon : MonoBehaviour
{
    // Start is called before the first frame update
    private int numOfHits;
    public bool isHexagonTileBroken;
    void Start()
    {
        numOfHits = 0;
        isHexagonTileBroken = false;
    }

    void hit()
    {
        --numOfHits;
        if (numOfHits == 3)
            isHexagonTileBroken = true;
    }
}
