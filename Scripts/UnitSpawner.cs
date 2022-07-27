using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSpawner : MonoBehaviour
{
    public GameObject sceneManager;
    public bool buttonActive;
    public GameObject temp;
    public Sprite producerTex;


    private void Awake()
    {
        buttonActive = false;
        sceneManager = GameObject.Find("SceneManager");
        var temp = Resources.Load<Sprite>("Sprites/ProducerTile");
        producerTex = temp;
        this.gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/ProducerTile");
    }

    public void spawnUnit()
    {
        temp = GameObject.Find("SpawnButton");

        GameObject toSpawn = temp.GetComponent<UnitSwapper>().passCurrentUnit();

        if (sceneManager.GetComponent<BankManager>().alliedMoney >= toSpawn.GetComponent<UnitController>().cost)
        {
            if (this.GetComponent<UnitManager>().containedUnits.Count == 0)
            {
                this.GetComponent<UnitManager>().addUnitToTile(toSpawn);
                int cost = (int)toSpawn.GetComponent<UnitController>().cost;
                sceneManager.GetComponent<BankManager>().takeMoney(cost, true);
            }
        }
    }
}
