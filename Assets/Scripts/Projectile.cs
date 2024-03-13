using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private WeaponScriptableObject _weaponSystem;
    [SerializeField] private float _projectileSpeed = 1000f;
    private Rigidbody2D _rigidbody2D;

    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        Invoke(nameof(DestroyProjectile), 5f);
    }
    private void FixedUpdate()
    {
        _rigidbody2D.velocity = _projectileSpeed * Time.deltaTime * transform.right;
    }

    private void DestroyProjectile()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out BaseEnemy enemy))
        {
            enemy.GetHit(_weaponSystem.WeaponSettings.WeaponAttackDamage);
            DestroyProjectile();
        }
    }

}
