using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResearchPawn : MonoBehaviour
{

    public bool isComplete;
    public bool hasPreRequisite;
    public GameObject preRequisite;
    public int researchCost;

    public bool globalBuff, unlock;

    void Start()
    {
        isComplete = false;
    }

    public void onComplete()
    {
        Debug.Log("Research Complete");
        isComplete = true;
        if (globalBuff)
        {
            this.gameObject.GetComponent<GlobalEffector>().onComplete();
        }
        if (unlock)
        {
            this.gameObject.GetComponent<Unlocker>().OnComplete();
        }

        this.gameObject.GetComponent<Button>().interactable = false;
    }

    public bool isAvailable()
    {
        if (isComplete) { return false; }
        if (!hasPreRequisite) { return true; }
        if (preRequisite.GetComponent<ResearchPawn>().isComplete) {  return true; }
        return false;
    }


}
