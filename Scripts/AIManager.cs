using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManager : MonoBehaviour
{
    public List<GameObject> AiUnits;
    public List<GameObject> tmpValidTiles = new List<GameObject>();
    private List<GameObject> unitsToMove = new List<GameObject>();
    private List<GameObject> tilesToMove = new List<GameObject>();

    public void onTurnEnd()
    {
        int counter = 0;
        unitsToMove.Clear();
        tilesToMove.Clear();
        foreach(GameObject unit in AiUnits)
        {
            tmpValidTiles.Clear();
            unit.transform.GetChild(0).GetComponent<UnitController>().getValidTiles(unit, 2);
            tmpValidTiles = unit.transform.GetChild(0).GetComponent<UnitController>().validMoves;

            if (Random.Range(0, 2) == 0)
            {
                tmpValidTiles.Reverse();
            }

            bool noEnemies = true;
            foreach (GameObject tile in tmpValidTiles)
            {
                if (tile.GetComponent<UnitManager>().containedUnits.Count == 0 && tile.GetComponent<Allegiance>().friendOrFoe == true)
                {
                    unitsToMove.Add(unit);
                    tilesToMove.Add(tile);
                    noEnemies = false;
                }
                else if (tile.GetComponent<UnitManager>().containedUnits.Count == 1 && tile.GetComponent<UnitManager>().containedUnits[0].GetComponent<UnitController>().modifiedPower < unit.transform.GetChild(0).GetComponent<UnitController>().modifiedPower)
                {
                    if (Random.Range(0, 3) == 1)
                    {
                        unitsToMove.Add(unit);
                        tilesToMove.Add(tile);
                        noEnemies = false;
                    }
                }
            }
            if (noEnemies)
            {
                try
                {

                    if(unit.GetComponent<AdjacentTiles>().adjacency[3].GetComponent<UnitManager>().containedUnits.Count == 0)
                    {
                    unitsToMove.Add(unit);
                    try
                    {
                        tilesToMove.Add(unit.GetComponent<AdjacentTiles>().adjacency[3]);
                    }
                    catch
                    {

                    }
                    }
                }
                catch
                {

                }
            }
        }

        if (AiUnits.Count < 0.5 * this.gameObject.GetComponent<TurnHandler>().allUnits.Count) 
        {
            foreach(GameObject tile in this.gameObject.GetComponent<SpawnTiles>().enemyProducers)
            {
                if (Random.Range(0, 7) == 1)
                {
                    if (tile.GetComponent<Allegiance>().friendOrFoe == false)
                    {
                        if (tile.GetComponent<UnitManager>().containedUnits.Count == 0)
                        {
                            tile.GetComponent<UnitManager>().addDefaultUnitToTile(true);
                            AiUnits.Add(tile);
                        }
                    }
                }
            }
        }

        counter = 0;
        foreach(GameObject unit in unitsToMove)
        {
            if (Random.Range(0,2) == 1)
            {
                unit.transform.GetChild(0).gameObject.GetComponent<UnitController>().moveTo(tilesToMove[counter]);
                counter++;
            }
        }
    }
}
