using UnityEngine;
using GameInterfaces.CharacterInterface;

namespace GameInterfaces
{
    public interface IStats
    {
        /* Variables */
        // Stats
        int hp            { get; set; }           // Health points
        int mp            { get; set; }           // Magic points
        int exp           { get; set; }           // Experience points
        int dmg           { get; set; }           // Damage dealt
        float multiplier  { get; set; }           // Time between hp recovers

        /* Functions */
        // Game systems
        void SetStats(int exp, float multiplier); // Sets everything at once
        void ReceiveDMG(ICharacter enemy);        // Receives damage
        void AddEXP(int exp);                     // Adds over current exp, does not make override current
        int FindNextLVLUp();                      // Find when to level up
        int GetLVL();                             // Get current level based on exp
        void Update();                            // Everything to update
    }

    // Simple stats
    public class SimpleStats : MonoBehaviour, IStats
    {
        // Stats
        public int hp            { get; set; } // Health points
        public int mp            { get; set; } // Magic points
        public int exp           { get; set; } // Experience points
        public int dmg           { get; set; } // Damage dealt
        public float multiplier  { get; set; } // Time between hp recovers

        // Initializer
        public SimpleStats(int exp = 1, float multiplier = 1.3F)
        {
            // Sets stats to be used
            SetStats(exp, multiplier);
        }

        public virtual void Update()
        {

        }

        // Set all stats (receives enum for type of stat)
        public void SetStats(int exp = 1, float multiplier = 1.3F)
        {
            this.hp         = (int)(100*GetLVL()*1.3F);
            this.mp         = (int)(100*GetLVL()*1.3F);
            this.exp        = exp;
            this.dmg        = (int)(10*GetLVL()*1.3F);
            this.multiplier = multiplier;
        }

        // Add EXP to this instance
        public virtual void AddEXP(int exp)
        {
            this.exp += exp;
        }

        // Calculate current level
        public int GetLVL()
        {
            return (int)Mathf.Ceil((exp) / (100 * multiplier)); 
        }

        // Find when too level up
        public int FindNextLVLUp()
        {
            return (int)(100 * exp * multiplier); 
        }

        // Receives DMG (only does the math)
        public void ReceiveDMG(ICharacter enemy)
        {
            hp -= enemy.stats.dmg;
        }
    }
}
