using UnityEngine;

namespace GameInterfaces
{
    // Stats with level ups and rng damage
    public class ComplexStats : SimpleStats, IStats
    {
        public Vector3 baseStats { get; set; }

        // Initializer
        public ComplexStats(int exp = 1, float multiplier = 1.3F)
        {
            // Sets stats to be used
            SetStats(exp, multiplier);

            // Makes a stats base
            baseStats = new Vector3(hp, mp, GetLVL());
        }

        public override void Update(){
            Recover();
        }

        // Recovers stats without passing tresshold
        public void Recover()
        {
            if (hp < baseStats.x)
            {
                if (hp + (hp * multiplier) > baseStats.x)
                {
                    hp = (int)baseStats.x;
                }
                else
                {
                    hp += (int)(hp * multiplier);
                }
            }
            if (mp < baseStats.y)
            {
                if (mp + (mp * multiplier) > baseStats.y)
                {
                    mp = (int)baseStats.y;
                }
                else
                {
                    mp += (int)(mp * multiplier);
                }
            }
        }

        // This is used on phase change, so everything is on max again
        public void ResetCurrentStats()
        {
            // Make base higher
            baseStats = baseStats + new Vector3((int)(baseStats.x * multiplier),     //Add to health
                                                (int)(baseStats.y * multiplier), 0); // Add to mana

            // Max out life again
            hp       = (int)baseStats.x;
            mp       = (int)baseStats.y;
        }

        // Add EXP to this instance
        public override void AddEXP(int exp = 10)
        {
            // Add exp to current
            base.AddEXP(exp);

            // If LVL UP
            if (this.exp >= FindNextLVLUp())
            {
                // Recover params
                ResetCurrentStats();

                // Actual lvlup
                baseStats += new Vector3(0, 0, 1);
            }
        }
    }
}