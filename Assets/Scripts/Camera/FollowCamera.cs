using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] private float maxZoom = 2.0f;    
    [SerializeField] private float damping = 1;

    [SerializeField] private GameObject target;    
    
    private Vector3 _offset;
    private float _curZoom = 1.0f;
    private bool _isZoom = false;

    private void Start() 
    {
        _offset = target.transform.position - transform.position;
    }
     
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _isZoom = true;            
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            _isZoom = false;            
        }

        if (_isZoom)
        {
            _curZoom = Mathf.Lerp(_curZoom, maxZoom, Time.deltaTime * damping);
        }
        else
        {
            _curZoom = Mathf.Lerp(_curZoom, 1, Time.deltaTime * damping);
        }
    }

    private void LateUpdate() 
    {
        float currentAngle = transform.eulerAngles.y;
        float desiredAngle = target.transform.eulerAngles.y;
        float angle = Mathf.LerpAngle(currentAngle, desiredAngle, Time.deltaTime * damping);
         
        Quaternion rotation = Quaternion.Euler(0, angle, 0);
        transform.position = target.transform.position - (rotation * _offset * _curZoom);
         
        transform.LookAt(target.transform);
    }    
}
