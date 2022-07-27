using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    private GameObject sceneManager;
    public List<GameObject> containedUnits = new List<GameObject>();
    private bool isActive = false;
    public GameObject unitPrefab;

    private void Awake()
    {
        sceneManager = GameObject.Find("SceneManager");
    }

    public void addDefaultUnitToTile()
    {
        GameObject newUnit = Instantiate(unitPrefab, this.transform);
        sceneManager.GetComponent<TurnHandler>().allUnits.Add(newUnit);
        newUnit.GetComponent<UnitController>().updateSupply();
        containedUnits.Add(newUnit);
    }

    public void addDefaultUnitToTile(bool Evil)
    {
        GameObject newUnit = Instantiate(unitPrefab, this.transform);
        newUnit.GetComponent<SpriteRenderer>().color = Color.red;
        sceneManager.GetComponent<TurnHandler>().allUnits.Add(newUnit);
        newUnit.GetComponent<UnitController>().updateSupply();
        containedUnits.Add(newUnit);
    }

    public void addUnitToTile(GameObject toSpawn)
    {
        GameObject newUnit = Instantiate(toSpawn, this.transform);
        sceneManager.GetComponent<TurnHandler>().allUnits.Add(newUnit);
        newUnit.GetComponent<UnitController>().updateSupply();
        containedUnits.Add(newUnit);
    }

    public void activateUnits()
    {
        isActive = true;
        foreach (GameObject unit in containedUnits)
        {
            unit.GetComponent<UnitController>().createChild();
        }
    }

    public void deActivateUnits()
    {
        isActive = false;
        foreach (GameObject unit in containedUnits)
        {
            unit.GetComponent<UnitController>().unHighLight();
        }
    }
}
