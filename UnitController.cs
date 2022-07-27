using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using TMPro;

public class UnitController : MonoBehaviour
{

    [SerializeField]
    private GameObject SceneManager;

    public float power, modifiedPower, breakthrough, emplacement, maxEmplacement, health, maxHealth, cost;
    private float initPower, initModifiedPower, initBreakthrough, initEmplacement, initMaxEmplacement, initHealth, initMaxHealth, initCost;

    public GameObject unitHighlight;
    public Texture2D tex;
    private GameObject child;
    private GameObject moveHint;
    public bool isActive;
    public List<GameObject> validMoves = new List<GameObject>();
    public bool amIInBattle;
    // Update is called once per frame
    private void Start()
    {
        initPower = power;
        initModifiedPower = modifiedPower;
        initBreakthrough = breakthrough;
        initEmplacement = emplacement;
        initMaxEmplacement = maxEmplacement;
        initHealth = health;
        initMaxHealth = maxHealth;
        initCost = cost;


        SceneManager = GameObject.Find("SceneManager");
        maxHealth = health;
        this.GetComponentInChildren<TMP_Text>().text = modifiedPower.ToString();
    }

    public bool moveTo(GameObject dest)
    {
        if (!amIInBattle)
        {
            validMoves.Clear();
            getValidTiles(this.transform.parent.gameObject, 2);
            unHighLight();
            if (validMoves.Contains(dest))
            {
                clearMoveHint();
                SceneManager.GetComponent<TurnHandler>().addMove(this.gameObject, dest);
                Debug.Log("Adding move");
                if (this.transform.parent.GetComponent<Allegiance>().friendOrFoe == true)
                {
                    highlightDest(dest);
                }
                return true;
            }
        }
        return false;
    }

    public void emplace(){
        updateSupply();
        if (emplacement < maxEmplacement){
            emplacement += 0.5f;
        }
    }

    public void onMovement()
    {
        updateSupply();
        emplacement = 0;
    }

    public void updateSupply(){
        if (this.transform.parent.GetComponent<Allegiance>().friendOrFoe == true){
            modifiedPower = power - ((25f - this.transform.parent.GetComponent<SupplySpreader>().alliedSupply) / 6f);
        }
        else
        {
            modifiedPower = power - ((25f - this.transform.parent.GetComponent<SupplySpreader>().axisSupply) / 6f);
        }
        this.GetComponentInChildren<TMP_Text>().text = modifiedPower.ToString();
    }

    
    public void createChild()
    {
        if( isActive != true)
        {
            child = Instantiate(unitHighlight, this.transform);
            child.transform.localScale = this.transform.localScale * 0.15f;
            child.transform.position = this.transform.position;
            child.GetComponent<SpriteRenderer>().sprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 10.0f);
            child.GetComponent<SpriteRenderer>().sortingOrder = 2;
            child.GetComponent<SpriteRenderer>().color = Color.yellow;
        }
        isActive = true;
    }

    private void highlightDest(GameObject dest)
    {
        moveHint = Instantiate(unitHighlight, this.transform);
        moveHint.transform.localScale = this.transform.localScale * 0.15f;
        moveHint.transform.position = dest.transform.position;
        moveHint.GetComponent<SpriteRenderer>().sprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 10.0f);
        moveHint.GetComponent<SpriteRenderer>().sortingOrder = 2;
        moveHint.GetComponent<SpriteRenderer>().color = Color.blue;
    }

    public void clearMoveHint()
    {
        GameObject.Destroy(moveHint);
    }

    public void unHighLight()
    {
        isActive = false;
        GameObject.Destroy(child);
    }

    public void getValidTiles(GameObject startTile, int movePoints)
    {
        if (!validMoves.Contains(startTile))
        {
            validMoves.Add(startTile);
        }

        foreach (GameObject adjacent in startTile.GetComponent<AdjacentTiles>().adjacency) //Check all adjacent tiles to currently focused tile
        {
            if (adjacent != null) //Checking for edge-tiles or random issues that may arise.
            {
                int nextMovePts = movePoints - 1; //Subtract the cost of the just completed potential movement
                if (adjacent.GetComponent<Allegiance>().friendOrFoe != this.transform.parent.GetComponent<Allegiance>().friendOrFoe) //Attempting to move through enemy territory should allow just one more movement
                {
                    nextMovePts = movePoints - 2;
                }
                if (nextMovePts >= 0 && adjacent.GetComponent<UnitManager>().containedUnits.Count == 0)
                {
                    getValidTiles(adjacent, nextMovePts);
                }
                else if (nextMovePts >= 0 && adjacent.GetComponent<UnitManager>().containedUnits.Count > 0 &&
                    adjacent.GetComponent<Allegiance>().friendOrFoe != this.transform.parent.GetComponent<Allegiance>().friendOrFoe)
                {
                    getValidTiles(adjacent, nextMovePts);
                }
            }
        }
    }

    public void deleteUnit(){
        this.transform.parent.GetComponent<UnitManager>().containedUnits.Clear();
        SceneManager.GetComponent<TurnHandler>().allUnits.Remove(this.transform.gameObject);
        SceneManager.GetComponent<AIManager>().AiUnits.Remove(this.transform.parent.gameObject);
        GameObject.Destroy(this.transform.gameObject);
    }

    public void applyBuffs(int powerBuff, int breakThroughBuff, int emplacementBuff, int healthBuff)
    {
        power = initPower + powerBuff;
        breakthrough = initBreakthrough + breakThroughBuff;
        maxEmplacement = initMaxEmplacement + emplacementBuff;
        maxHealth = initMaxHealth + healthBuff;
    }
}

//public bool moveTo(GameObject dest)
    //{
    //    validMoves.Clear();
    //    getValidTiles(this.transform.parent.gameObject, 2);

    //    if (validMoves.Contains(dest))
    //    {

    //        if (dest.GetComponent<Allegiance>().friendOrFoe != this.transform.parent.GetComponent<Allegiance>().friendOrFoe)
    //        {
    //            try { dest.GetComponent<SupplyGenerator>().onCapture();  }
    //            catch { }
    //            dest.GetComponent<Allegiance>().friendOrFoe = !dest.GetComponent<Allegiance>().friendOrFoe;
    //            this.transform.parent.GetComponent<SupplySpreader>().sendSupply();
    //            dest.GetComponent<SupplySpreader>().updateColour();
    //        }
    //        this.transform.position = dest.transform.position;
    //        this.transform.parent = dest.transform;
    //        dest.GetComponent<UnitManager>().containedUnits.Add(this.gameObject);
    //        unHighLight();

    //        return true;
    //    }
    //    return false;
    //}