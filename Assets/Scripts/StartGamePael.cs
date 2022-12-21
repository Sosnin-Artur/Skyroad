using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class StartGamePael : MonoBehaviour, IPointerDownHandler
{
    [SerializeField]
    private GameManager _manager;

    public void OnPointerDown(PointerEventData eventData)
    {
      
        GameManager.instance.StartGame();
    }

}
