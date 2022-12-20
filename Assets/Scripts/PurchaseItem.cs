using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PurchaseItem : MonoBehaviour
{
    public string ID;
    public float cost;
    public int productTypeId;
    public TMP_Text CostText;

    public void Start()
    {
        CostText.text = "$" + cost.ToString();
    }
}
