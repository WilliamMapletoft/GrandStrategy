using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResearchManager : MonoBehaviour
{
    public bool currentlyBusy;
    private GameObject currentResearch; 
    public GameObject output;
    public int remainingTurns;

    public int powerBuff, breakthroughBuff, emplacementBuff, healthBuff;
    public bool tanksUnlocked;

    private void Start()
    {
        currentlyBusy = false;
    }
    public void onStartResearch(GameObject toStart)
    {
        if (!currentlyBusy && toStart.GetComponent<ResearchPawn>().isAvailable())
        {
            currentlyBusy = true;
            remainingTurns = toStart.GetComponent<ResearchPawn>().researchCost;
            currentResearch = toStart;
            output.GetComponentInChildren<TMP_Text>().text = "Current research : " + toStart.transform.GetChild(0).GetComponent<TMP_Text>().text + ", turns remaining : " + remainingTurns;
        }
    }

    public void onTurn()
    {
        if (currentlyBusy == true)
        {
            if (remainingTurns > 1)
            {
                remainingTurns--;
                output.GetComponentInChildren<TMP_Text>().text = "Current research : " + currentResearch.transform.GetChild(0).GetComponent<TMP_Text>().text + ", turns remaining : " + remainingTurns;
            }
            else if (remainingTurns == 1)
            {
                currentResearch.GetComponent<ResearchPawn>().onComplete();
                currentResearch = null;
                currentlyBusy = false;
                output.GetComponentInChildren<TMP_Text>().text = "Research complete, and on hold";
            }
        }
    }
}
