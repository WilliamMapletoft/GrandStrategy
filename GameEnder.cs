using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameEnder : MonoBehaviour
{
    public List<GameObject> infoDumps;
    public GameObject timer;
    private GameObject canvas;
    private int currentTurn;
    float timeRem;
    int minRem;
    int secRem;

    private void Start()
    {
        canvas = GameObject.Find("Canvas");
        timeRem = 600;
    }
    private void Update()
    {
        timeRem -= Time.deltaTime;
        minRem = (int)timeRem / 60;
        secRem = (int)timeRem % 60;
        timer.GetComponent<TMP_Text>().text = "Time Remaining: " + minRem + ";" + secRem;

        if (timeRem <= 0.0f)
        {
            onGameEnd();
        }

        bool doIend = true;
        foreach (GameObject producer in this.gameObject.GetComponent<SpawnTiles>().enemyProducers)
        {
            if (producer.GetComponent<Allegiance>().friendOrFoe == false)
            {
                doIend = false;
            }
        }
        if (doIend)
        {
            onGameEnd();
        }

        doIend = true;
        foreach (GameObject producer in this.gameObject.GetComponent<SpawnTiles>().friendlyProducers)
        {
            if (producer.GetComponent<Allegiance>().friendOrFoe == true)
            {
                doIend = false;
            }
        }
        if (doIend)
        {
            onGameEnd();
        }
    }

    public void onGameEnd()
    {
        SceneManager.LoadScene("EndScene", LoadSceneMode.Single);
    }

    public void nextTurn()
    {
        currentTurn++;
        GameObject temp;
        switch (currentTurn)
        {
            case 5:
                temp = Instantiate(infoDumps[0]);
                temp.transform.SetParent(canvas.transform, false);
                break;
            case 10:
                temp = Instantiate(infoDumps[1]);
                temp.transform.SetParent(canvas.transform, false);
                break;
        }
    }

    public void onTank()
    {
        GameObject temp;
        temp = Instantiate(infoDumps[2]);
        temp.transform.SetParent(canvas.transform, false);
    }
}
