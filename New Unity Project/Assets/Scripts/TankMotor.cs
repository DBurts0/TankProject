using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankMotor : MonoBehaviour
{
    public float moveSpeed;
    public float turnSpeed;
    private CharacterController characterController;
    // Start is called before the first frame update
    void Start()
    {
        characterController = gameObject.GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Forwards()
    {
        Vector3 direction;
        direction = transform.forward;
        direction *= moveSpeed;
        characterController.SimpleMove(direction);
    }
    public void Backwards()
    {
        Vector3 direction;
        direction = transform.forward;
        direction *= (-1 * moveSpeed);
        characterController.SimpleMove(direction);
    }
    public void RotateLeft()
    {
        Vector3 rotationVector;
        rotationVector = new Vector3(0, -1, 0);
        rotationVector *= turnSpeed * Time.deltaTime;
        transform.Rotate(rotationVector, Space.Self);
    }
    public void RotateRight()
    {
        Vector3 rotationVector;
        rotationVector = new Vector3(0, 1, 0);
        rotationVector *= turnSpeed * Time.deltaTime;
        transform.Rotate(rotationVector, Space.Self);
    }
}
