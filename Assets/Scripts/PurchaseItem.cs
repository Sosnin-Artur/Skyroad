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

    private bool _complted;

    public bool Complted
    {
        get => _complted;
        set
        {
            if (value)
            {
                CostText.text = "sailed";
            }
        }                
    }

    public void Start()
    {
        CostText.text = "$" + cost.ToString();
    }
}
