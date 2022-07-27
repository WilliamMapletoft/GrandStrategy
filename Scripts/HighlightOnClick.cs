using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class HighlightOnClick : MonoBehaviour
{
    public bool isHighlighted;
    public Texture2D tex;
    public GameObject outline;
    private GameObject parent;
    private GameObject child;
    private float timer;
    private GameObject UIManager;
    // Start is called before the first frame update

    void Update()
    {
        if (timer > 0.0f)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            if (isHighlighted)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    if (this.GetComponent<UnitSpawner>())
                    {
                        this.GetComponent<UnitSpawner>().buttonActive = false;
                    }
                    isHighlighted = false;
                    GameObject.Destroy(child);
                    this.GetComponent<UnitManager>().deActivateUnits();
                }

                if (Input.GetMouseButtonDown(1))
                {
                    Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    Vector2 mousePosition2D = new Vector2(mousePosition.x, mousePosition.y);

                    RaycastHit2D hit = Physics2D.Raycast(mousePosition2D, Vector2.zero);
                    if (hit.collider != null)
                    {
                        if (this.GetComponent<UnitManager>().containedUnits.Count > 0)
                        {
                            foreach (GameObject unit in this.GetComponent<UnitManager>().containedUnits)
                            {
                                unit.GetComponent<UnitController>().moveTo(hit.collider.gameObject);
                                isHighlighted = false;
                                GameObject.Destroy(child);
                                this.GetComponent<UnitManager>().deActivateUnits();
                            }
                        }
                    }

                }
            }
        }
    }

    void Start()
    {
        parent = this.gameObject.transform.gameObject;
        UIManager = GameObject.Find("Canvas");
    }

    void OnMouseDown()
    {
        if (!isHighlighted && !EventSystem.current.IsPointerOverGameObject() && this.GetComponent<Allegiance>().friendOrFoe == true)
        {
            createChild();
            isHighlighted = true;
            timer += 0.1f;
            this.GetComponent<UnitManager>().activateUnits();


            if (this.GetComponent<UnitSpawner>() != null && this.GetComponent<UnitSpawner>().buttonActive == false)
            {
                this.GetComponent<UnitSpawner>().buttonActive = true;
                UIManager.GetComponent<UIManager>().activateSpawnUnitButton(this.gameObject);
            }
            else
            {
                UIManager.GetComponent<UIManager>().deActivateSpawnUnitButton();
            }
        }
    }

    void createChild()
    {
        child = Instantiate(outline, parent.transform);
        child.transform.localScale = child.transform.localScale * 1.1f;
        child.transform.position = parent.transform.position;
        child.GetComponent<SpriteRenderer>().sprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 10.0f);
        child.GetComponent<SpriteRenderer>().sortingOrder = 1;
        child.GetComponent<SpriteRenderer>().color = Color.yellow;
    }
    
}
