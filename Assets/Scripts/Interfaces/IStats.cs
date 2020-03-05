using UnityEngine;

namespace GameInterfaces
{
    public struct Stats
    {
        public int hp;
        public int mp;
        public int exp;
        public int dmg;
    }

    public interface IStats<T>
    {
        // Stat things
        void ResetCurrentStats();    // For level up | For enemy phase change
        void Recover();              // Recovers life after some time (implementation depends on user)

        // Game systems
        void ReceiveDMG(ref GameObject enemy);
        void AddEXP(int exp);

        // Setters
        // void SetStats();

        // Getters
        int GetHP();
        int GetMP();
        int GetEXP();
        int GetDMG();
        int GetRecoverTime();
    }

    // Stats with level ups and rng damage
    public class ComplexStats : MonoBehaviour, IStats<ComplexStats> 
    {
        // Knows type of stats
        private enum StatType
        {
            Current,
            Base
        }

        // Stats
        protected Stats baseStats;
        protected Stats currentStats; // Actually does not need dmg value, but it helps to use the same struct
        private int recoveryTime;     // Time until recovers some amount of live (so it doesn't happen every frame)

        // Initializer
        public ComplexStats(int hp = 100, int mp = 100, int exp = 1, int dmg = 10, int lvlUp = 100, int recoveryTime = 3)
        {
            // Sets stats to be used
            SetStats(StatType.Base, hp, mp, lvlUp, dmg);
            SetStats(StatType.Current, baseStats.hp, baseStats.mp, baseStats.exp, baseStats.dmg);

            // Wait time for recovery (which defaults to 3 seconds)
            this.recoveryTime = recoveryTime;
        }

        // Recovers stats without passing tresshold
        public void Recover()
        {
            if (currentStats.hp < baseStats.hp)
            {
                if (currentStats.hp + recoveryTime > baseStats.hp)
                {
                    currentStats.hp = baseStats.hp;
                }
                else
                {
                    currentStats.hp += GetRecoverTime();
                }
            }
            if (currentStats.mp < baseStats.mp)
            {
                if (currentStats.mp + GetRecoverTime() > baseStats.mp)
                {
                    currentStats.mp = baseStats.mp;
                }
                else
                {
                    currentStats.mp += recoveryTime;
                }
            }
        }

        // This is used on level up, so everything is on max again
        public void ResetCurrentStats()
        {
            currentStats.hp = baseStats.hp;
            currentStats.mp = baseStats.mp;
        }

        // Add EXP to this instance (only does the math)
        public void AddEXP(int exp = 10)
        {
            // Add exp to current
            currentStats.exp += exp;

            // If LVL UP
            if (currentStats.exp >= baseStats.exp)
            {
                // Base param
                baseStats.hp += (int)Mathf.Ceil(baseStats.hp * 0.2F);
                baseStats.mp += (int)Mathf.Ceil(baseStats.mp * 0.2F);

                // Recover params
                ResetCurrentStats();

                // New lvlup requirement
                baseStats.exp += (int)Mathf.Ceil(baseStats.exp * 0.4F);
            }
        }

        // Receives DMG (only does the math)
        public void ReceiveDMG(ref GameObject enemy)
        {
            currentStats.hp -= enemy.GetDMG();
        }

        /* Getters */
        // Retrieve HP
        public int GetHP()
        {
            return currentStats.hp;
        }

        // Retrieve MP
        public int GetMP()
        {
            return currentStats.mp;
        }

        // Retrieve EXP
        public int GetEXP()
        {
            return currentStats.exp;
        }

        // Retrieve dmg
        public int GetDMG()
        {
            int damage = (int)Random.Range(currentStats.dmg, currentStats.dmg * 3); // Gets a random dmg value (for rng) between dmg and 3*dmg
            return damage;
        }

        // Retrieves when level up will occur
        public int GetLVLCap()
        {
            return baseStats.exp;
        }

        // Retrieves wait time until next recover (so it doesn't happen every frame)
        public int GetRecoverTime()
        {
            return recoveryTime;
        }

        // Set all stats (receives enum for type of stat)
        private void SetStats(StatType type, int hp = 100, int mp = 100, int exp = 100, int dmg = 10)
        {
            Stats stat;

            stat.hp  = hp;
            stat.mp  = mp;
            stat.exp = exp;
            stat.dmg = dmg;

            if(type == StatType.Current)
            {
                currentStats = stat;
            }
            else
            {
                baseStats    = stat;
            }
        }
    }

    public class SimpleStats : MonoBehaviour, IStats<SimpleStats>
    {
        // Stats
        protected Stats stats;        // All stats
        protected Vector2 baseStats;  // Base stats
        private int recoveryTime;     // So that it is not a valid strategy to hide and recover your life, as simpler mobs also recover

        // Set all stats (receives enum for type of stat)
        private void SetStats(int hp = 100, int mp = 100, int exp = 100, int dmg = 10)
        {
            Stats stat;

            stat.hp  = hp;
            stat.mp  = mp;
            stat.exp = exp;
            stat.dmg = dmg;

            stats = stat;
        }

        public SimpleStats(int hp = 100, int mp = 100, int exp = 1, int dmg = 10, int recoveryTime = 3)
        {
            // Sets stats to be used
            stats         = SetStats(hp, mp, exp, dmg);


            // Wait time for recovery (which defaults to 3 seconds)
            this.recoveryTime = recoveryTime;
        }

        // This is used on phase change, so everything is on max again
        public void ResetCurrentStats()
        {
            // Make base higher
            baseStats.x *= 1.2;
            baseStats.y *= 1.2;

            // Max out life again
            stats.hp = baseStats.x;
            stats.mp = baseStats.y;
        }

        // Add EXP to this instance (placeholder)
        public void AddEXP(int exp)
        {

        }

        // Recovers stats without passing tresshold
        public void Recover()
        {
            if (stats.hp < baseStats.x)
            {
                if (stats.hp + recoveryTime > baseStats.x)
                {
                    stats.hp = baseStats.x;
                }
                else
                {
                    stats.hp += baseStats.x * 0.1;
                }
            }
            if (stats.mp < baseStats.y)
            {
                if (stats.mp + GetRecoverTime() > baseStats.y)
                {
                    stats.mp = baseStats.y;
                }
                else
                {
                    stats.mp += baseStats.y * 0.1;
                }
            }
        }


        // Receives DMG (only does the math)
        public void ReceiveDMG(ref GameObject enemy)
        {
            stats.hp -= enemy.GetComponent<ICharacter>.GetDMG();
        }

        /* Getters */
        // Retrieve HP
        public int GetHP()
        {
            return stats.hp;
        }

        // Retrieve MP
        public int GetMP()
        {
            return stats.mp;
        }

        // Retrieve EXP
        public int GetEXP()
        {
            return stats.exp;
        }

        // Retrieve dmg
        public int GetDMG()
        {
            return stats.dmg;
        }

        // Retrieves wait time until next recover (so it doesn't happen every frame)
        public int GetRecoverTime()
        {
            return recoveryTime;
        }
    }
}
