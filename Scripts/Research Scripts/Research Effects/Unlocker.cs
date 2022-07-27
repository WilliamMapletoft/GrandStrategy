using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unlocker : MonoBehaviour
{
    public bool tank;
    private GameObject canvas;
    private GameObject sceneManager;

    private void Start()
    {
        canvas = GameObject.Find("Canvas");
        sceneManager = GameObject.Find("SceneManager");
    }

    public void OnComplete()
    {
        if (tank)
        {
            canvas.GetComponent<ResearchManager>().tanksUnlocked = true;
            sceneManager.GetComponent<GameEnder>().onTank();
        }
    }
}
