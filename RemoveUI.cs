using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveUI : MonoBehaviour
{
    public GameObject parent;
    public void delete()
    {
        GameObject.Destroy(parent);
    }
}
