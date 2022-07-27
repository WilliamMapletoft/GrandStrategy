using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalEffector : MonoBehaviour
{
    public bool power, breakthrough, emplacement, health;
    public int pBuff, bBuff, eBuff, hBuff;
    private GameObject researchManager;

    private void Start()
    {
        researchManager = GameObject.Find("Canvas");
    }
    public void onComplete()
    {
        if (power) { researchManager.GetComponent<ResearchManager>().powerBuff += pBuff; }
        if (breakthrough) { researchManager.GetComponent<ResearchManager>().breakthroughBuff += bBuff; }
        if (emplacement) { researchManager.GetComponent<ResearchManager>().emplacementBuff += eBuff; }
        if (health) { researchManager.GetComponent<ResearchManager>().healthBuff += hBuff; }
    }


}

