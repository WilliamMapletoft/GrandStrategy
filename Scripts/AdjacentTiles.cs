using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjacentTiles : MonoBehaviour
{
    public GameObject[] adjacency = new GameObject[8];
    private int counter;

    public void addTile(GameObject inputTile)
    {
        adjacency[counter] = inputTile;
        if (counter < 8)
        {
            counter++;
        }
    }
}
