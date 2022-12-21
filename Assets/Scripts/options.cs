using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class options : MonoBehaviour
{
    [SerializeField]
    private AudioSource _aidio;

    private void Start()
    {
        _aidio.enabled = PlayerPrefs.GetInt("music") > 0;
    }

    public void Toggle(bool value)
    {
        _aidio.enabled = value;

        PlayerPrefs.SetInt("music", value ? 1 : 0);
    }
}
