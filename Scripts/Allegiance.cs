using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Allegiance : MonoBehaviour
{
    public bool friendOrFoe;

    public void SetColours()
    {
        if (friendOrFoe == false)
        {
            this.GetComponent<SpriteRenderer>().color += new Color(0.5f, 0.0f, 0.0f);
        }
        else
        {
            this.GetComponent<SpriteRenderer>().color += new Color(0.0f, 0.0f, 0.5f);
        }
    }
}
