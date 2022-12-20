using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{    
    [SerializeField] private Transform target;        
    
    private Vector3 _offset;      

    private void Start()
    {
        _offset = transform.position - target.position;
    }

    private void Update()
    {        
        // Follow the target at a starting offset.
        transform.position = new Vector3
            (transform.position.x, transform.position.y, target.position.z + _offset.z);    
    }
}
