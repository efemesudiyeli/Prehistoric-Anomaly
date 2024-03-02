using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseEnemy : MonoBehaviour, IDamageable
{
    public Player Player;
    [SerializeField] private float _health = 100f;
    [SerializeField] private float _attackDamage = 5f;
    [SerializeField] private float _attackRate = 1f;
    [SerializeField] private float _attackRange = 0.5f;
    [SerializeField] private float _moveSpeed = 0.5f;
    private bool _isAttackOnCooldown = false;
    private IDamageable _damageable;



    private void Start()
    {
        if (Player != null)
        {
            _damageable = Player.GetComponent<IDamageable>();
        }
    }

    public void GetHit(float attackDamage)
    {
        _health -= attackDamage;
        if (_health <= 0)
        {
            Destroy(gameObject);
        }
    }

    protected void AttackToPlayer()
    {
        if (Player == null || _isAttackOnCooldown) return;

        _damageable.GetHit(_attackDamage);
        StartCoroutine(AttackCooldown());
    }

    protected bool IsPlayerInAttackRange()
    {
        if (Player == null) return false;

        return Vector2.Distance(transform.position, Player.transform.position) <= _attackRange;
    }

    protected void MoveTowardsPlayer()
    {
        if (Player == null) return;

        Vector2 direction = Player.transform.position - transform.position;
        //? Maybe remove Mathf.Sign for smoothing.
        direction.x = Mathf.Sign(Mathf.Clamp(direction.x, -1f, 1f));
        direction.y = 0;
        transform.Translate(_moveSpeed * Time.deltaTime * direction);
    }

    protected IEnumerator AttackCooldown()
    {
        _isAttackOnCooldown = true;
        yield return new WaitForSeconds(_attackRate);
        _isAttackOnCooldown = false;
    }
}
