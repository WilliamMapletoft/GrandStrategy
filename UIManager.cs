using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{

    private bool enableUI;
    private GameObject tileManager;
    private GameObject dropDown;
    private GameObject child;
    public GameObject spawnUnitButton;
    private int outputSupply;

    void Start()
    {
        tileManager = GameObject.Find("SceneManager");
        dropDown = GameObject.Find("SelectionDropDown");
    }

    void Update()
    {
        enableUI = false;
        outputSupply = 0;

        if (!EventSystem.current.IsPointerOverGameObject())
        {
            foreach (GameObject tile in tileManager.GetComponent<SpawnTiles>().tiles)
            {
                if (tile.GetComponent<HighlightOnClick>().isHighlighted == true)
                {
                    enableUI = true;
                    outputSupply += tile.GetComponent<SupplySpreader>().alliedSupply;


                }
            }

            if (enableUI == true)
            {
                dropDown.GetComponent<CanvasRenderer>().SetColor(new Color(1, 1, 1, 1));
                dropDown.SetActive(true);

                for (int i = 0; i < dropDown.transform.childCount; i++)
                {
                    dropDown.transform.GetChild(i).gameObject.SetActive(true);
                }

                dropDown.transform.GetChild(0).GetComponent<TMP_Text>().SetText("Cumulative supply : " + outputSupply);
            }
            else if (enableUI == false)
            {
                for (int i = 0; i < dropDown.transform.childCount; i++)
                {
                    dropDown.transform.GetChild(i).gameObject.SetActive(false);
                }
                dropDown.GetComponent<CanvasRenderer>().SetColor(new Color(0, 0, 0, 0));
                dropDown.SetActive(false);
            }
        }
    }

    public void activateSpawnUnitButton(GameObject calledFrom)
    {
        if (child != null)
        {
            GameObject.Destroy(child);
        }
        child = Instantiate(spawnUnitButton);
        child.transform.parent = dropDown.transform;
        child.transform.localScale = dropDown.transform.localScale;

        child.GetComponentInChildren<Button>().onClick.AddListener(calledFrom.GetComponent<UnitSpawner>().spawnUnit);
    }


    public void deActivateSpawnUnitButton()
    {
        GameObject.Destroy(child);
    }

}
