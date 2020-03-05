using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    // Start is called before the first frame update
    private void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        // If collides with enemy, life goes down by damage from enemy
        if (other.gameObject.tag == "player" || other.gameObject.tag == "skill")
        {
            // Does not make much sense
            ReceiveDMG(other.gameObject);
        }
    }
}
