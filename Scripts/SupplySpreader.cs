using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupplySpreader : MonoBehaviour
{
    public int alliedSupply;
    public int axisSupply;
    public GameObject root;

    public void receiveAllySupply(int inputAllySupply, GameObject newRoot)
    {
        alliedSupply = inputAllySupply;
        root = newRoot;
        updateColour();
        if (this.GetComponent<UnitManager>().containedUnits.Count > 0 ){
            foreach(GameObject unit in this.GetComponent<UnitManager>().containedUnits){
                unit.GetComponent<UnitController>().updateSupply();
            }
        }
        sendSupply();
    }

    public void receiveAxisSupply(int inputAxisSupply, GameObject newRoot)
    {
        axisSupply = inputAxisSupply;
        root = newRoot;
        updateColour();
        if (this.GetComponent<UnitManager>().containedUnits.Count > 0 ){
            foreach(GameObject unit in this.GetComponent<UnitManager>().containedUnits){
                unit.GetComponent<UnitController>().updateSupply();
            }
        }
        sendSupply();
    }

    public void sendSupply()
    {
        foreach (GameObject a in this.GetComponent<AdjacentTiles>().adjacency)
        {
            if (a != null)
            {
                if (a.GetComponent<Allegiance>().friendOrFoe == true)
                {
                    if (alliedSupply - 2 > a.GetComponent<SupplySpreader>().alliedSupply)
                    {
                        a.GetComponent<SupplySpreader>().receiveAllySupply(alliedSupply - 2, root.gameObject);
                        //Debug.Log("Updating supply");
                    }
                }
                else
                {
                    if (axisSupply - 2 > a.GetComponent<SupplySpreader>().axisSupply)
                    {
                        a.GetComponent<SupplySpreader>().receiveAxisSupply(axisSupply - 2, root.gameObject);
                        //Debug.Log("Updating supply");
                    }
                }
            }

        }
    }

    public void updateColour() // Probably gonna remove this eventually, this exists to visually represent the supply a tile has. Perhaps could turn this into a toggle map view?
    {
        this.GetComponent<SpriteRenderer>().color = Color.white;
        int currentSupply;
        if (this.GetComponent<Allegiance>().friendOrFoe == true)
        {
            currentSupply = alliedSupply;
        }
        else
        {
            currentSupply = axisSupply;
        }

        for (int i = 0; i < currentSupply; i++)
        {
            this.GetComponent<SpriteRenderer>().color += new Color(-0.02f, 0.00f, -0.02f); // adjust r and b values to change supply display contrast.
        }
        this.GetComponent<Allegiance>().SetColours();
    }

    private void Update()
    {
        if (axisSupply > 0 && this.GetComponent<Allegiance>().friendOrFoe == true)
        {
            axisSupply = 0;
        }
        else if(alliedSupply > 0 && this.GetComponent<Allegiance>().friendOrFoe == false)
        {
            alliedSupply = 0;
        }
        if (root != null)
        {
            if (root.GetComponent<Allegiance>().friendOrFoe != this.GetComponent<Allegiance>().friendOrFoe)
            {
                alliedSupply = 0;
                axisSupply = 0;
                updateColour();

                foreach (GameObject a in this.GetComponent<AdjacentTiles>().adjacency)
                {
                    if (a != null)
                    {
                        a.GetComponent<SupplySpreader>().sendSupply();
                    }
                }

            }
        }
    }
}
