using System;
using UnityEngine;

// Class
public class Character : MonoBehaviour
{
    // Stats
    private struct Stats
    {
        int hp;
        int sp;
        int exp;
        int dmg;
    };
    private Stats BaseStats;
    private Stats CurrentStats;

    protected Animate animate;

    protected void CreateStats(int hp = 100, int sp = 100, int exp = 100, int currentExp = 1, int dmg = 10){
        BaseStats        = SetStats(hp, sp, exp, dmg);
        CurrentStats     = BaseStats;
        CurrentStats.exp = currentExp;
    }

    private void ResetCurrentStats()
    {
        CurrentStats.hp = BaseStats.hp;
        CurrentStats.sp = BaseStats.sp;
    }

    private Stats SetStats(int hp = 100, int sp = 100, int exp = 100, int dmg = 10)
    {
        Stats stats;
        stats.hp  = hp;
        stats.sp  = sp;
        stats.exp = exp;
        stats.dmg = dmg;

        return stats;
    }

    // Recovers stats without passing tresshold
    protected void Recover(int hpRecover, int spRecover)
    {
        if (CurrentStats.hp < BaseStats.hp)
        {
            if (CurrentStats.hp + hpRecover > BaseStats.hp)
            {
                CurrentStats.hp = BaseStats.hp;
            }
            else
            {
                CurrentStats.hp += hpRecover;
            }
        }
        if (CurrentStats.sp < BaseStats.sp)
        {
            if (CurrentStats.sp + spRecover > BaseStats.sp)
            {
                CurrentStats.sp = BaseStats.sp;
            }
            else
            {
                CurrentStats.sp += spRecover;
            }
        }
    }

    protected void ReceiveDMG(GameObject enemy)
    {
        CurrentStats.hp -= enemy.GetComponent<Character>().GetDMG();

        if(CurrentStats.hp<=0){
            // Animate death
            animate.Die();

            // Add exp to enemy
            enemy.GetComponent<Character>().AddEXP(BaseStats.exp);

            // Destroy instance
            Destroy(gameObject);
        }
    }

    // Add EXP to this instance
    public void AddEXP(int exp)
    {
        // Should show gained exp on screen
        /**** Not Implemented ****/

        // Add exp to this instance
        CurrentStats.exp += exp;

        // If LVL UP
        if (CurrentStats.exp >= BaseStats.exp)
        {
            // Base param
            BaseStats.hp += (int)Math.Ceiling(BaseStats.hp*0.2);
            BaseStats.sp += (int)Math.Ceiling(BaseStats.sp*0.2);

            // Recover params
            ResetCurrentStats();

            // New lvlup requirement
            BaseStats.exp += (int)Math.Ceiling(BaseStats.exp * 0.4);
        }
    }

    // Retrieve HP
    public int GetHP()
    {
        return CurrentStats.hp;
    }

    // Retrieve SP
    public int GetSP()
    {
        return CurrentStats.sp;
    }

    // Retrieve EXP
    public int GetEXP()
    {
        return CurrentStats.exp;
    }

    // Retrieve dmg
    public int GetDMG()
    {
        return Math.Random(CurrentStats.dmg, CurrentStats.dmg * 1.4);
    }
}