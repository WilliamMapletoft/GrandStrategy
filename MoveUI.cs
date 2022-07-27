using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveUI : MonoBehaviour
{
    private int offsetX, offsetY;
    public int additionalX, additionalY;
    Vector2 res;

    private void Start()
    {
        res = new Vector2(Screen.width, Screen.height);
        offsetX = Screen.width / 15 + additionalX;
        offsetY = -(Screen.height / 14 + additionalY);
    }

    // Update is called once per frame
    void Update()
    {
        if (res.x != Screen.width || res.y != Screen.height)
        {
            offsetX = Screen.width / 15 + additionalX;
            offsetY = -(Screen.height / 15 + additionalY);
            res.x = Screen.width;
            res.y = Screen.height;
        }
        transform.position = Input.mousePosition + new Vector3(offsetX,offsetY);
    }
}
