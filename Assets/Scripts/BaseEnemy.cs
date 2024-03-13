using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public abstract class BaseEnemy : MonoBehaviour, IDamageable
{
    public Player Player;
    public HitReceiveFlashEffect HitReceiver;
    [SerializeField] private float _health = 100f;
    [SerializeField] private float _attackDamage = 5f;
    [SerializeField] private float _attackRate = 1f;
    [SerializeField] private float _attackRange = 0.5f;
    [SerializeField] private float _moveSpeed = 0.5f;
    [SerializeField] private Animator _animator;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    private bool _isAttackOnCooldown = false;
    private IDamageable _damageable;
    private Rigidbody2D _rigidbody2d;
    private WeaponSystem _weaponSystem;

    public event Action OnDie;

    private AudioSource _audioSource;
    [SerializeField] private AudioClip _hitSound;


    private void Awake()
    {
        _rigidbody2d = GetComponent<Rigidbody2D>();
        _audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        if (Player != null)
        {
            _weaponSystem = Player.GetComponent<WeaponSystem>();
            _damageable = Player.GetComponent<IDamageable>();
        }
    }

    public void GetHit(float attackDamage)
    {
        _health -= attackDamage;
        HitReceiver.GetFlashEffect(_spriteRenderer);
        if (_health <= 0)
        {
            Die();
        }
        else
        {
            GetKnockback();
            _audioSource.pitch = UnityEngine.Random.Range(0.9f, 1.1f);
            _audioSource.PlayOneShot(_hitSound);
        }
    }

    private void Die()
    {
        OnDie?.Invoke();
        Destroy(this.gameObject, 0.1f);
    }

    private void GetKnockback()
    {
        Vector2 direction = new Vector2(Player.transform.position.x - transform.position.x, -1);
        _rigidbody2d.AddForce(-direction * _weaponSystem._currentWeapon.WeaponSettings.WeaponKnockbackPower, ForceMode2D.Impulse);
    }

    protected void AttackToPlayer()
    {
        if (Player == null || _isAttackOnCooldown) return;

        _animator.SetTrigger("attackTrigger");

        if (Vector2.Distance(transform.position, Player.transform.position) <= _attackRange)
        {
            _damageable.GetHit(_attackDamage);
        }
        else
        {
            Debug.Log("Player dodged");
        }
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
        FlipSpriteToDirection(direction.x);
        transform.Translate(_moveSpeed * Time.deltaTime * direction, Space.World);
    }

    protected IEnumerator AttackCooldown()
    {
        _isAttackOnCooldown = true;
        yield return new WaitForSeconds(_attackRate);
        _isAttackOnCooldown = false;
    }

    private void FlipSpriteToDirection(float horizontalDirection)
    {
        if (horizontalDirection > 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (horizontalDirection < 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }
}
