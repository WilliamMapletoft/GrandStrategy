using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupplyGenerator : MonoBehaviour
{
    public int alliedSupply;
    public int axisSupply;
    private int counter;
    // Start is called before the first frame update
    void Awake()
    {
        int supplyQuantity;
        supplyQuantity = Random.Range(5, 25);
        if (this.GetComponent<Allegiance>().friendOrFoe == true)
        {
            alliedSupply = supplyQuantity;
        }
        else if (this.GetComponent<Allegiance>().friendOrFoe == false)
        {
            axisSupply = supplyQuantity;
        }

        this.GetComponent<SupplySpreader>().alliedSupply = alliedSupply;
        this.GetComponent<SupplySpreader>().axisSupply = axisSupply;
        this.GetComponent<SupplySpreader>().root = this.gameObject;

        for (int i = 0; i < supplyQuantity; i++)
        {
            this.GetComponent<SpriteRenderer>().color += new Color(-0.02f, 0.00f, -0.02f);
            //Debug.Log(this.GetComponent<SpriteRenderer>().color);
        }
    }

    private void Update()
    {
        for (int i = 0; i < 8; i++)
        {
            GameObject tmp = this.gameObject.GetComponent<AdjacentTiles>().adjacency[i];
            if (tmp != null)
            {
                if (tmp.GetComponent<Allegiance>().friendOrFoe == true)
                {
                    if (alliedSupply - 2 > tmp.GetComponent<SupplySpreader>().alliedSupply)
                    {
                        tmp.GetComponent<SupplySpreader>().receiveAllySupply(alliedSupply - 2, this.gameObject);
                        //Debug.Log("Updating supply");
                    }
                }
                else
                {
                    if (axisSupply - 2 > tmp.GetComponent<SupplySpreader>().axisSupply)
                    {
                        tmp.GetComponent<SupplySpreader>().receiveAxisSupply(axisSupply - 2, this.gameObject);
                        //Debug.Log("Updating supply");
                    }
                }
            }
        }

    }


    public void onCapture()
    {
        if (this.GetComponent<Allegiance>().friendOrFoe == true)
        {
            axisSupply = alliedSupply;
            alliedSupply = 0;
            this.GetComponent<SupplySpreader>().alliedSupply = 0;
        }
        else
        {
            alliedSupply = axisSupply;
            axisSupply = 0;
            this.GetComponent<SupplySpreader>().axisSupply = 0;
        }

    }
}
