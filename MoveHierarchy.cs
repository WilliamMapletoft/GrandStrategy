using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveHierarchy : MonoBehaviour
{
    public GameObject toMove;

    public void moveToBottom()
    {
        toMove.transform.SetSiblingIndex(toMove.transform.parent.transform.childCount);
    }
}
