using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleManager : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> alliedUnits = new List<GameObject>();
    [SerializeField]
    private List<GameObject> axisUnits = new List<GameObject>();
    public GameObject initialAttacker, initialDefender;
    private bool attackingSide;
    public bool inProgress;

    [SerializeField]
    private GameObject progressMarker;
    private GameObject progressMarkerInstance;

    void start(){
        inProgress = false;
    }

    private void Update()
    {
        foreach (GameObject unit in axisUnits)
        {
            try
            {
                unit.GetComponent<UnitController>().amIInBattle = true;
            }
            catch
            {
                axisUnits.Clear();
                alliedUnits.Clear();
                inProgress = false;
                initialAttacker = null;
                initialDefender = null;
                GameObject.Destroy(progressMarkerInstance);
                progressMarkerInstance = null;
            }
        }
        foreach (GameObject unit in alliedUnits)
        {
            try
            {
                unit.GetComponent<UnitController>().amIInBattle = true;
            }
            catch
            {
                axisUnits.Clear();
                alliedUnits.Clear();
                inProgress = false;
                initialAttacker = null;
                initialDefender = null;
                GameObject.Destroy(progressMarkerInstance);
                progressMarkerInstance = null;
            }
        }
    }

    public void addAlliedUnitToBattle(GameObject unit){
        if(inProgress == true){
            if (!alliedUnits.Contains(unit)){
                alliedUnits.Add(unit);
            }
        }
        else{
            alliedUnits.Add(unit);
            unit.GetComponent<UnitController>().amIInBattle = true;
            axisUnits.Add(this.GetComponent<UnitManager>().containedUnits[0]);
            this.GetComponent<UnitManager>().containedUnits[0].GetComponent<UnitController>().amIInBattle = true;
            initiateBattle(unit);
            attackingSide = true;
        }
    }

    public void addAxisUnitToBattle(GameObject unit){
        if(inProgress == true){
            if (!axisUnits.Contains(unit)){
                axisUnits.Add(unit);
            }
        }
        else{
            axisUnits.Add(unit);
            unit.GetComponent<UnitController>().amIInBattle = true;
            alliedUnits.Add(this.GetComponent<UnitManager>().containedUnits[0]);
            this.GetComponent<UnitManager>().containedUnits[0].GetComponent<UnitController>().amIInBattle = true;
            initiateBattle(unit);
            attackingSide = false;
        }
    }

    private void initiateBattle(GameObject attacker){
        //Debug.Log("Hello!");
        initialAttacker = attacker;
        attacker.GetComponent<UnitController>().amIInBattle = true;
        initialDefender = this.GetComponent<UnitManager>().containedUnits[0];
        initialDefender.GetComponent<UnitController>().amIInBattle = true;
        progressMarkerInstance = Instantiate(progressMarker, this.transform);
        inProgress = true;
    }

    public void turnCycle(){

        if (initialAttacker.GetComponent<UnitController>().health <= 0){
            foreach (GameObject unit in axisUnits)  { unit.GetComponent<UnitController>().amIInBattle = false; }
            foreach (GameObject unit in alliedUnits) { unit.GetComponent<UnitController>().amIInBattle = false; }
            initialAttacker.GetComponent<UnitController>().deleteUnit();
            axisUnits.Clear();
            alliedUnits.Clear();
            inProgress = false;
            initialAttacker = null;
            initialDefender= null;
            GameObject.Destroy(progressMarkerInstance);
            progressMarkerInstance = null;
            return;
        }
        else if (initialDefender.GetComponent<UnitController>().health <= 0){
            foreach (GameObject unit in axisUnits)
            {
                try
                {
                    unit.GetComponent<UnitController>().amIInBattle = false;
                }
                catch
                {
                    axisUnits.Clear();
                    alliedUnits.Clear();
                    inProgress = false;
                    initialAttacker = null;
                    initialDefender = null;
                    GameObject.Destroy(progressMarkerInstance);
                    progressMarkerInstance = null;
                }
            }
            foreach (GameObject unit in alliedUnits) {
                try
                {
                    unit.GetComponent<UnitController>().amIInBattle = false;
                }
                catch
                {
                    axisUnits.Clear();
                    alliedUnits.Clear();
                    inProgress = false;
                    initialAttacker = null;
                    initialDefender = null;
                    GameObject.Destroy(progressMarkerInstance);
                    progressMarkerInstance = null;
                }
            }
            initialDefender.GetComponent<UnitController>().deleteUnit();
            axisUnits.Clear();
            alliedUnits.Clear();
            inProgress = false;
            initialAttacker= null;
            initialDefender= null;
            GameObject.Destroy(progressMarkerInstance);
            progressMarkerInstance = null;
            return;
        }

        float totalAtk = 0;
        float totalDef = 0;

        if (attackingSide == true){
            totalAtk += sumPower(alliedUnits);
            totalDef += sumPower(axisUnits);
        }
        else{
            totalAtk += sumPower(axisUnits);
            totalDef += sumPower(alliedUnits);
        }

        if (totalAtk > totalDef){
            damageCalc(initialDefender, initialAttacker, totalAtk, totalDef);
            progressMarkerInstance.GetComponentInChildren<TMP_Text>().text = "Attacker is winning";
            progressMarkerInstance.transform.GetChild(1).GetComponent<SpriteRenderer>().color = new Color(0.0f, 1.0f, 0.0f, 0.5f);
        }
        else{
            damageCalc(initialAttacker, initialDefender, totalAtk, totalDef);
            progressMarkerInstance.GetComponentInChildren<TMP_Text>().text = "Attacker is losing";
            progressMarkerInstance.transform.GetChild(1).GetComponent<SpriteRenderer>().color = new Color(1.0f, 0.0f, 0.0f, 0.5f);
        }

    }

    private void damageCalc(GameObject damagedUnit, GameObject damagingUnit, float attack, float defense){
        float damageAtk = (attack + damagingUnit.GetComponent<UnitController>().breakthrough); 
        float damageDef = (defense + damagedUnit.GetComponent<UnitController>().emplacement);

        float damage = 0f;

        if (damageAtk > damageDef){
            damage = damageAtk - damageDef;
        }
        else if (damageDef > damageAtk){
            damage = damageDef - damageAtk;
        }

        if (damage < 0.05f){
            damage = 0.1f;
        }

        damagedUnit.GetComponent<UnitController>().health -= damage;
    }

    private float sumPower(List<GameObject> tempUnits){
        float temp = 0;

            foreach (GameObject unit in tempUnits){
                temp += unit.GetComponent<UnitController>().modifiedPower;
            }

        return temp;
    }

}
