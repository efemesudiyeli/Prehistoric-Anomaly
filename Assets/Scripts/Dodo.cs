using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dodo : BaseEnemy
{
    void Update()
    {
        if (base.IsPlayerInAttackRange())
        {
            base.Invoke(nameof(AttackToPlayer), 0.5f);
        }
        else
        {
            base.MoveTowardsPlayer();
        }
    }
}
