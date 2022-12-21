using Newtonsoft.Json.Linq;
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
            _complted = value;
            if (value)
            {
                CostText.text = "sailed";
            }
        }                
    }

    public void Awake()
    {
        if (_complted)
        {
            CostText.text = "sailed";
        }
        else
        {

            CostText.text = "$" + cost.ToString();
        }
    }
}
