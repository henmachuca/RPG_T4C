using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic {

    // Calculates experience required to level up
    public static float ExperienceForNextLevel(int currentLevel)
    {
        if (currentLevel == 0) return 0;
        return (currentLevel * currentLevel ) * 10;
    }


    /// <summary>
    /// Calculates the player base attack damage
    /// Base damage equals STR + 3 damage per 10 STR + 1 bonus damage per 3 AGI
    /// </summary>
    /// <returns> The player base attack damage </returns>
    ///  <param name="PlayerController">Player controller</param>
    public static float CalculatePlayerBaseAttackDamage(Player_Attack playerAttack)
    {
        float baseDamage = playerAttack.strength + Mathf.Floor(playerAttack.strength / 10) * 3 + Mathf.Floor(playerAttack.agility / 3);
        return baseDamage;
    }

}
