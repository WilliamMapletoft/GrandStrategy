using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnHandler : MonoBehaviour
{
    public List<GameObject> allUnits = new List<GameObject>();
    private List<GameObject> units = new List<GameObject>();
    private List<GameObject> toTiles = new List<GameObject>();
    private GameObject ResearchManager;

    private void Start()
    {
        ResearchManager = GameObject.Find("Canvas");
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            onTurnNext();
        }
    }
    public void onTurnNext()
    {
        updateBuffs();
        ResearchManager.GetComponent<ResearchManager>().onTurn();

        foreach(GameObject unit in allUnits){
            if (!units.Contains(unit)){
                unit.GetComponent<UnitController>().emplace();
            }
        }

        for (int i = 0; i < units.Count; i++)
        {
            if (toTiles[i].GetComponent<UnitManager>().containedUnits.Count == 0 || toTiles[i].GetComponent<Allegiance>().friendOrFoe == false)
            {
                moveUnit(units[i], toTiles[i]);
            }
        }
        
        units.Clear();
        toTiles.Clear();

        this.GetComponent<BankManager>().addMoney(2, true);
        this.GetComponent<BankManager>().addMoney(2, false);

        this.transform.gameObject.GetComponent<AIManager>().onTurnEnd();

        for (int i = 0; i < units.Count; i++)
        {
            if (toTiles[i].GetComponent<UnitManager>().containedUnits.Count == 0 || toTiles[i].GetComponent<Allegiance>().friendOrFoe == true)
            {
                //Debug.Log("moving enemy unit");
                moveUnit(units[i], toTiles[i]);
            }
        }


        foreach (GameObject tile in this.GetComponent<SpawnTiles>().tiles)
        {
            if (tile.GetComponent<BattleManager>().inProgress == true)
            {
                tile.GetComponent<BattleManager>().turnCycle();
            }
        }

        units.Clear();
        toTiles.Clear();

        this.gameObject.GetComponent<GameEnder>().nextTurn();
    }

    public void addMove(GameObject from, GameObject to)
    {
        for(int i = 0; i < units.Count; i++)
        {
            if (units[i] == from)
            {
                units.RemoveAt(i);
                toTiles.RemoveAt(i);
            }
        }
        units.Add(from);
        toTiles.Add(to);
    }

    public void moveUnit(GameObject from, GameObject to)
    {
        bool doIMove = true;
        if (from.GetComponent<UnitController>().amIInBattle == false)
        {
            if (to.GetComponent<Allegiance>().friendOrFoe != from.transform.parent.GetComponent<Allegiance>().friendOrFoe && //handling moving into empty enemy territory
            to.GetComponent<UnitManager>().containedUnits.Count == 0)
            {
                try { to.GetComponent<SupplyGenerator>().onCapture(); }
                catch { }
                to.GetComponent<Allegiance>().friendOrFoe = !to.GetComponent<Allegiance>().friendOrFoe;
                from.transform.parent.GetComponent<SupplySpreader>().sendSupply();
                to.GetComponent<SupplySpreader>().updateColour();
            }
            else if (to.GetComponent<Allegiance>().friendOrFoe != from.transform.parent.GetComponent<Allegiance>().friendOrFoe && //handling moving into occupied enemy territory
                to.GetComponent<UnitManager>().containedUnits.Count > 0)
            {
                if (from.transform.parent.GetComponent<Allegiance>().friendOrFoe == true)
                {
                    to.GetComponent<BattleManager>().addAlliedUnitToBattle(from);
                }
                else
                {
                    to.GetComponent<BattleManager>().addAxisUnitToBattle(from);
                }
                doIMove = false;
            }
        }
        else
        {
            doIMove = false;
        }

        if (doIMove)
        { //enter this when a unit should not be entered into a battle during the movement phase
            if (from.transform.parent.gameObject.GetComponent<Allegiance>().friendOrFoe == false)
            {
                this.transform.gameObject.GetComponent<AIManager>().AiUnits.Remove(from.transform.parent.gameObject);
                this.transform.gameObject.GetComponent<AIManager>().AiUnits.Add(to);
            }
            from.transform.parent.GetComponentInParent<UnitManager>().containedUnits.Remove(from);
            from.transform.position = to.transform.position;
            from.transform.parent = to.transform;
            to.GetComponent<UnitManager>().containedUnits.Add(from.gameObject);
            from.GetComponent<UnitController>().unHighLight();
            from.GetComponent<UnitController>().updateSupply();
        }
        from.GetComponent<UnitController>().clearMoveHint();
    }

    public void updateBuffs()
    {
        GameObject temp = GameObject.Find("Canvas");

        int tPower, tBreak, tEmplac, tHealth;

        tPower = temp.GetComponent<ResearchManager>().powerBuff;
        tBreak = temp.GetComponent<ResearchManager>().breakthroughBuff;
        tEmplac = temp.GetComponent<ResearchManager>().emplacementBuff;
        tHealth = temp.GetComponent<ResearchManager>().healthBuff;

        foreach (GameObject unit in allUnits)
        {
            if (unit.transform.parent.gameObject.GetComponent<Allegiance>().friendOrFoe == true)
            {
                unit.GetComponent<UnitController>().applyBuffs(tPower, tBreak, tEmplac, tHealth);
            }
        }
    }
}
