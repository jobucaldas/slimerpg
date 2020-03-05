using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Player : Character
{
    // User Input
    [SerializeField]
    private float movementSpeed = 0.1F;       // Movement speed
    [SerializeField]
    [Tooltip("This value should be a positive integer")]
    private float distanceFromBackground = 20;

    // Variables
    private Movement movement;
    private MousePosition mousePosition;

    // Start is called when object is spawned
    private void Start()
    {
        CreateStats();

        // Initialize objects
        Animator animator = gameObject.GetComponent<Animator>();
        animate = new Animate(ref animator);
        movement = new Movement(ref animate, distanceFromBackground, movementSpeed);

        CreateStats();
    }

    // Update is called every frame (do NOT add everything here)
    private void Update()
    {
        // Move character
        Bool moving = movement.MoveTransform(mousePosition.Get());
        animate.Move(moving);

        Recover();
        GetMousePosition();
    }
    
    // Triggered
    void OnTriggerEnter(Collider other)
    {
        // If collides with enemy, life goes down by damage from enemy
        if (other.gameObject.tag == "enemy")
        {
            ReceiveDMG(other.gameObject);
        }
    }
}
