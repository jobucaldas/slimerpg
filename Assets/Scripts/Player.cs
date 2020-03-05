using UnityEngine;
using GameInterface;

namespace GameInterfaces
{
    namespace CharacterInterface
    {
        public class Player : MonoBehaviour, ICharacter<Player>
        {
            /* User Input */
            // Movement
            [SerializeField]
            private float movementSpeed = 0.1F;       // Movement speed
            [SerializeField]
            [Tooltip("This value should be a positive integer")]
            private float distanceFromBackground = 20;
            // Raycast
            [SerializeField]
            private LayerMask collisionMask;
            [SerializeField]
            private float horizontalRaySpacing;
            [SerializeField]
            private float verticalRaySpacing;

            // Character basics
            private IStats<ComplexStats> stats;
            private IMovement<MouseMovement> movement;

            // Animator
            private IAnimation<PlayerAnimation> animate;
            private Animator animator;
            private BoxCollider2D boxCollider2D;

            // Start is called when object is spawned
            public void Start()
            {
                // Creates stats
                stats = new ComplexStats();

                // Sets animator to gameobject's
                animator = gameObject.GetComponent<Animator>();
                animate  = new PlayerAnimator(ref animator);
                animate.Stop();

                // Set movement
                boxCollider2D = gameObject.GetComponent<BoxCollider2D>();
                movement = new MouseMovement(ref boxCollider2D, ref animate, distanceFromBackground, movementSpeed, horizontalRaySpacing, verticalRaySpacing);
            }

            // Update is called every frame (do NOT add everything here)
            public void Update()
            {
                // Move character
                bool moving = this.movement.MoveTransform(this.mousePosition.Get());
                Debug.Log("Moving? " + moving);
                animate.Move(moving);

                stats.Recover();
            }

            // Triggered
            public void OnTriggerEnter(Collider other)
            {
                // If collides with enemy, life goes down by damage from enemy
                if (other.gameObject.tag == "enemy")
                {
                    ReceiveDMG(other.gameObject);
                }
            }

            public void AddEXP(int exp)
            {
                // Does EXP math and saves it
                stats.AddEXP(exp);

                // LVL UP handler
                if(stats.GetEXP()>=stats.GetLVLCap())
                {
                    /* To Do */
                    // Show gained exp on temp child label
                }
            }

            public void ReceiveDMG(ref GameObject enemy)
            {
                enemy = enemy.gameObject.GetComponent<Enemy>();
                stats.ReceiveDMG(ref enemy.gameObject);

                if(stats.GetHP()<=0){
                    // Animate death
                    animate.Die();

                    // Add exp to enemy
                    enemy.AddEXP(stats.GetEXP());

                    // Destroy instance
                    Destroy(gameObject);
                }
            }
        }
    }
}
