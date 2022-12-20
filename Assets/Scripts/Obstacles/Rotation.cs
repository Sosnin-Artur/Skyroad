﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour
{
    [SerializeField] float rotationSpeed = 3.0f; 
            
    void Update()
    {
        transform.Rotate(0, rotationSpeed, 0);
    }
}
