using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{    
    [SerializeField] float horizontalForce = 1.0f;    
    [SerializeField] float verticalSpeed = 1.0f;    
    [SerializeField] float verticalSpeedBoost = 2.0f;    

    [SerializeField] Rigidbody rb;
    [SerializeField] GameManager gameManager;

    private float _speedModifier = 1.0f;
    private Vector2 _touchLastPos;
    private float _sidewaysSpeed;
    private float _sensitivity = 10.0f;

    private void Update()
    {
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
        //float angleY = Mathf.Atan2(_sidewaysSpeed, forwardSpeed) * Mathf.Rad2Deg;
        //  head.rotation = Quaternion.Euler(0, angleY, 0);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _speedModifier = verticalSpeedBoost;
            gameManager.Boost(verticalSpeedBoost);
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            _speedModifier = 1.0f;
            gameManager.Boost(1.0f);
        }

        rb.velocity = new Vector3(0, 0, verticalSpeed * _speedModifier);

        float hor = _sidewaysSpeed;
        Vector3 move = new Vector3(hor * horizontalForce, 0, 0);
        rb.AddForce(move, ForceMode.Impulse);
    }    

    private void OnCollisionEnter(Collision other) 
    {
        if (other.gameObject.CompareTag("Obstacle"))
        {
            gameManager.AddLives(-1);
        }    
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (other.CompareTag("ScoreLine"))
        {   
            gameManager.AddScore(5);         
            gameManager.AddAsteroid(1);
        }
        else if (other.CompareTag("Crystal"))
        {
            _speedModifier = verticalSpeedBoost;
            StartCoroutine(BoostMode());
        }
    }

    private IEnumerator BoostMode()
    {
        yield return new WaitForSeconds(2);
        _speedModifier = 1.0f;
    }
}
