using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BankManager : MonoBehaviour
{
    public int alliedMoney, axisMoney;
    public GameObject alliedBalance;

    public void addMoney(int input, bool allegiance) //Pass income, and allegiance (true for allied, false for axis)
    {
        if (allegiance == true)
        {
            alliedMoney += input;
            updateBalance(true);
        }
        else
        {
            axisMoney += input;
        }
    }

    public void takeMoney(int input, bool allegiance) //Pass cost of unit, and allegiance (true for allied, false for axis)
    {
        if (allegiance == true)
        {
            alliedMoney -= input;
            updateBalance(true);
        }
        else
        {
            axisMoney -= input;
        }
    }

    void updateBalance(bool allegiance) // Display current balance on UI elements.
    {
        alliedBalance.GetComponent<TMPro.TextMeshProUGUI>().text = "£" + alliedMoney.ToString();
    }
}
