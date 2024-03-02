using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dodo : BaseEnemy
{
    void Update()
    {
        if (base.IsPlayerInAttackRange())
        {
            base.AttackToPlayer();
        }
        else
        {
            base.MoveTowardsPlayer();
        }
    }
}
