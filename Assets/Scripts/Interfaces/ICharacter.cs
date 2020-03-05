using UnityEngine;

namespace GameInterfaces
{
    namespace CharacterInterface
    {
        // Interface Declaration
        public interface ICharacter<T>
        {
            // Unity things
            void Start();
            void Update();
            void OnTriggerEnter(Collider other);

            // Game systems
            void ReceiveDMG(ref GameObject enemy);
            void AddEXP(int exp);
        }
    }
}