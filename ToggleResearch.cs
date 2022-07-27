using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleResearch : MonoBehaviour
{
    private GameObject researchPanel;
    private bool active;

    private void Awake()
    {
        researchPanel = GameObject.Find("ResearchPanel");
        researchPanel.SetActive(false);
        active = false;
    }

    public void toggle()
    {
        if (active)
        {
            researchPanel.SetActive(false);
            active = false;
        }
        else {
            researchPanel.SetActive(true);
            active = true;
        }
    }
}
