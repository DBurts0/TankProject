using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shell : MonoBehaviour
{
    // Public variable for how much damage the shell deals
    public int damage;
    // Start is called before the first frame update
    void Start()
    {

    }
     // Update is called once per frame
     void Update()
    {

    }
    // Destroy the shell on collision
    void OnCollisionEnter(Collision col)
    {
        Destroy(gameObject);

    }
}
