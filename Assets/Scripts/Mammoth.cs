using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mammoth : BaseEnemy
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
