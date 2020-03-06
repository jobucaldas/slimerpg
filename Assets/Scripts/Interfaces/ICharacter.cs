using UnityEngine;

namespace GameInterfaces
{
    namespace CharacterInterface
    {
        // Interface Declaration
        public interface ICharacter
        {
            // Variables
            // Character basics
            IMovement movement           { get; set; } // Character movement
            BoxCollider2D collisionBody  { get; set; } // Object's collider
            Animator animator            { get; set; } // Objects animator object
            IAnimation animate           { get; set; } // Animation setter
            IStats stats                 { get; set; } // Stats handler            

            // Unity things
            void Start();
            void Update();
            void OnTriggerEnter(Collider other);

            // Game systems
            void ReceiveDMG(ICharacter enemy);
            void AddEXP(int exp);
        }
    }
}