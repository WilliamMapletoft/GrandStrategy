using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSwapper : MonoBehaviour
{

    public GameObject[] unitArray = new GameObject[3];
    private GameObject currentUnit;
    private GameObject canvas;

    public int debugInt;
    private void Awake()
    {
        currentUnit = unitArray[0];
        changeText(currentUnit.name);
        canvas = GameObject.Find("Canvas");
    }
    public void onLeft()
    {
        int tmp = 0;
        int modifier = 0;
        if (!canvas.GetComponent<ResearchManager>().tanksUnlocked)  { modifier++; }
        for (int i = 0; i < unitArray.Length; i++)
        {
            if (unitArray[i] == currentUnit)
            {
                if ((i - 1) < 0)
                {
                    tmp = unitArray.Length - 1 - modifier;
                }
                else
                {
                    tmp = i - 1;
                }
                debugInt = tmp;
            }
        }
        currentUnit = unitArray[tmp];
        changeText(currentUnit.name);
    }

    public void onRight()
    {
        int tmp = 0;
        int modifier = 0;
        if (!canvas.GetComponent<ResearchManager>().tanksUnlocked) { modifier++; }
        for (int i = 0; i < unitArray.Length; i++)
        {
            if (unitArray[i] == currentUnit)
            {
                if ((i + 1) == unitArray.Length - modifier)
                {
                    tmp = 0;
                }
                else
                {
                    tmp = i + 1;
                }
                debugInt = tmp;
            }
        }
        currentUnit = unitArray[tmp];
        changeText(currentUnit.name);
    }

    private void changeText(string newText)
    {
        this.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "Spawn " + newText + " (" + currentUnit.GetComponent<UnitController>().cost +" money)";
    }

    public GameObject passCurrentUnit()
    {
        return currentUnit;
    }
}
