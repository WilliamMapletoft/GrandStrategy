using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTiles : MonoBehaviour
{
    public GameObject prefabTile;
    private GameObject tileManager;
    public int mapW, mapH, supplyTiles;
    public GameObject[,] tiles;
    public List<GameObject> friendlyProducers;
    public List<GameObject> enemyProducers;
    public Sprite producerTex;
    // Start is called before the first frame update
    void Start()
    {
        tileManager = GameObject.Find("Tiles"); // Finding the parent object to add all of the tiles to. This allows me to keep tabs on which tiles are where without adding loads of data to the tiles themselves.

        tiles = new GameObject[mapW , mapH];

        for (int i = 0; i < mapW ; i++)
        {
            for (int j = 0; j < mapH; j++)
            {
                GameObject childObject = Instantiate(prefabTile, new Vector3((i - mapW/2)+0.5f, (j - mapH/2)+0.5f), Quaternion.identity);
                tiles[i, j] = childObject;

                if (i < (mapW / 2)) { childObject.GetComponent<Allegiance>().friendOrFoe = true; } // True = player, false = AI. Left side of the map is player.
                else { childObject.GetComponent<Allegiance>().friendOrFoe = false; }

                if (isSupply()) // adding the supplygenerator script to the tiles that spawn as generators. these produce the supply which will spread across the board, and are useful strategic tools.
                {
                    childObject.AddComponent<SupplyGenerator>();
                }

                if (i == 20) { childObject.GetComponent<UnitManager>().addDefaultUnitToTile(); } // Spawning infantry units (default) in all frontline tiles for thhe player.
                else if (i == 21)
                {
                    childObject.GetComponent<UnitManager>().addDefaultUnitToTile(true); // Similar to spawning player units, but these are added to a list elsewhere for ease of access later.
                    this.transform.gameObject.GetComponent<AIManager>().AiUnits.Add(childObject);
                }
                else if (i == 0) { 
                    childObject.AddComponent<UnitSpawner>(); // Setting the tiles at the edge of the map to be able to spawn units.
                    friendlyProducers.Add(childObject);
                }
                else if (i == mapW - 1)
                {
                    enemyProducers.Add(childObject); // Enemy producer tiles do not need to be actually able to spawn units, and so the unitspawner script is not added. Instead, the texture is merely modified.
                    childObject.GetComponent<SpriteRenderer>().sprite = producerTex; // this is because AI spawn units differently to the players units (In the AI script itself).
                }

                childObject.GetComponent<Allegiance>().SetColours(); // Colour in tiles with the colour of the side they are on. Blue = player, yellow = AI
                childObject.transform.SetParent(tileManager.transform); // Setting all of the tiles to be children of an empty gameobject for ease of debugging.
            }
        }

        addAdjacency();
        
    }
    bool isSupply()
    {
        if (Random.Range(0, 25) == 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    void addAdjacency() 
    {
        //int i = 41;
        //int j = 0;
        //tiles[i, j].GetComponent<SpriteRenderer>().color = Color.black;
        for (int i = 0; i < mapW; i++)
        {
            for (int j = 0; j < mapH; j++)
            {
                //run through every tile, needs to be done on a second pass, because doing it as the tiles spawn results in null values being passed, and errors being thrown. :(

                for (int y = 0; y < 3; y++)
                {
                    for (int x = 0; x < 3; x++) //checking every tile within -1 and 1 of the centre, should return all tiles within a square 3 wide and 3 tall centred around tiles[i,j]
                    {
                        int tX = x - 1;
                        int tY = y - 1;

                        if (i + tX < (mapW) && i + tX > -1)
                        {
                            if (j + tY < (mapH) && j + tY > -1)
                            {
                                if (tX != 0 || tY != 0) //could combine into one if statement later, currently designed for ease of testing. Checking no null values will be sent, and only the adjacent tiles, not the currently selected tile.
                                {
                                    //Debug.Log("Sending X = " + tX + " Y = " + tY);
                                    tiles[i, j].GetComponent<AdjacentTiles>().addTile(tiles[(i + tX), (j + tY)]);
                                }
                                else
                                {
                                    //Debug.Log("NOT sending X = " + tX + " Y = " + tY);
                                }
                            }
                        }
                    }
                }



            }
        }
    }
}
