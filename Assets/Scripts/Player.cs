using UnityEngine;
using GameInterfaces;

namespace GameInterfaces
{
    namespace CharacterInterface
    {
        public class Player : MonoBehaviour, ICharacter
        {
            /* User Input */
            // Player data
            [SerializeField] private int exp = 1;             // Current exp
            [SerializeField] private float multiplier = 1.3F; // Stat multiplier

            // Movement
            [SerializeField]
            private float movementSpeed = 0.1F;        // Movement speed
            [SerializeField] [Tooltip("This value should be a positive integer")]
            private float distanceFromBackground = 20; // Distance setter

            // Raycast
            [SerializeField] private LayerMask collisionMask;
            [SerializeField] private float horizontalRaySpacing;
            [SerializeField] private float verticalRaySpacing;

            /* Class specific */
            // Character basics
            public IMovement movement           { get; set; } // Movement handler
            public IStats stats                 { get; set; } // Object's stats
            public MousePoint mousePoint        { get; set; } // Gets mouse point (anytime)

            // Animator
            public IAnimation animate           { get; set; } // Animation Handler
            public Animator animator            { get; set; } // Actual animator in unity
            public BoxCollider2D collisionBody  { get; set; } // Collider

            /* Functions */
            // Start is called when object is spawned
            public void Start()
            {
                // Creates stats
                stats = new ComplexStats(exp, multiplier);

                // Sets animator to gameobject's
                animator = gameObject.GetComponent<Animator>();
                animate  = new PlayerAnimation(animator);
                animate.Stop(); // Starts in a stopped animation

                // Set movement
                collisionBody = gameObject.GetComponent<BoxCollider2D>();
                mousePoint    = new MousePoint(transform.position);
                movement      = new MouseMovement(collisionBody, collisionMask,              // Collision
                                                  animate,                                   // Animation
                                                  distanceFromBackground, movementSpeed,     // Movement settings
                                                  horizontalRaySpacing, verticalRaySpacing); // Raycast settings
            }

            // Update is called every frame (do NOT add everything here)
            public void Update()
            {
                // Move character
                movement.MoveTo(mousePoint.Get());

                // Update stats
                stats.Update();
            }

            // Collision triggered
            public void OnTriggerEnter(Collider other)
            {
                // If collides with enemy, life goes down by damage from enemy
                if (other.gameObject.tag == "enemy")
                {
                    ReceiveDMG(other.gameObject.GetComponent<ICharacter>());
                }
            }

            public void AddEXP(int exp)
            {
                int lvl = stats.GetLVL();

                // Does EXP math and saves it
                stats.AddEXP(exp);

                // LVL UP handler
                if(stats.GetLVL()>lvl) // Actually means: 'if current level higher than stored one, than'
                {
                    /* To Do */
                    // Show gained exp on temp child label
                }
            }

            public void ReceiveDMG(ICharacter enemy)
            {
                stats.ReceiveDMG(enemy);

                if(stats.hp <= 0){
                    // Animate death
                    animate.Die();

                    // Destroy instance
                    Destroy(gameObject);
                }
            }
        }
    }
}
