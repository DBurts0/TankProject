﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankMotor : MonoBehaviour
{
    // Variable for movement speed
    public float moveSpeed;
    // Variable for rotation speed
    public float turnSpeed;
    // Variable to store the character controller
    private CharacterController characterController;
    // Start is called before the first frame update
    void Start()
    {
        // Access the tank's character controller
        characterController = gameObject.GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Forwards()
    {
        // Use a Vector3 variable to move the tank forwards based on the value of moveSpeed
        Vector3 direction;
        direction = transform.forward;
        direction *= moveSpeed;
        characterController.SimpleMove(direction);
    }
    public void Backwards()
    {
        // Use a Vector3 variable to move the tank backwards based on the value of moveSpeed
        Vector3 direction;
        direction = transform.forward;
        direction *= (-1 * moveSpeed);
        characterController.SimpleMove(direction);
    }
    public void RotateLeft()
    {
        // Use a Vector3 to rotate the tank counter-clockwise based on the value of turnSpeed
        Vector3 rotationVector;
        rotationVector = new Vector3(0, -1, 0);
        rotationVector *= turnSpeed * Time.deltaTime;
        transform.Rotate(rotationVector, Space.Self);
    }
    public void RotateRight()
    {
        // Use a Vector3 to rotate the tank clockwise based on the value of turnSpeed
        Vector3 rotationVector;
        rotationVector = new Vector3(0, 1, 0);
        rotationVector *= turnSpeed * Time.deltaTime;
        transform.Rotate(rotationVector, Space.Self);
    }
}
