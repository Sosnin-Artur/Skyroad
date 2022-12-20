using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpaceshipController : MonoBehaviour
{
    [SerializeField] Renderer _renderer;
    [SerializeField] float rotationSpeed = 3.0f; 
    [SerializeField] float maxRotation = 45.0f;     
        
    
    private float _rotationZ = 0.0f;

    private float _speedModifier = 1.0f;
    private Vector2 _touchLastPos;
    private float _sidewaysSpeed;
    private float _sensitivity = 10.0f;

    private void Start()
    {
        transform.localRotation = Quaternion.identity;
    }

    void Update()
    {
        if (Time.timeScale == 0)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            _touchLastPos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            _sidewaysSpeed = 0;
            _touchLastPos = new Vector2(0, 0);
        }
        else if (Input.GetMouseButton(0))
        {
            Vector2 delta = (Vector2)Camera.main.ScreenToViewportPoint(Input.mousePosition) - _touchLastPos;
            _sidewaysSpeed += delta.x * _sensitivity;

            _touchLastPos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        }

        float hor = _sidewaysSpeed;


        if (Mathf.Approximately(hor, 0))
        {
            _rotationZ = Mathf.Lerp(_rotationZ, 0, rotationSpeed * Time.deltaTime / 4);
        }
        else
        {
            _rotationZ -= hor;
            _rotationZ = Mathf.Clamp(_rotationZ, -maxRotation, maxRotation);
        }        
                        
        transform.localEulerAngles = new Vector3(0, 0, _rotationZ);                                                                                           
    }

    public void ChangeColor(Color color)
    {
        _renderer.material.color = color;
    }

}
