using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSystem : MonoBehaviour
{
    [SerializeField] private WeaponScriptableObject _currentWeapon;
    [SerializeField] private WeaponScriptableObject[] _equipableWeapons;
    [SerializeField] private Transform _attackRadiusTransform;
    [SerializeField] private LayerMask _enemyLayerMask;

    private bool _isAttackOnCooldown = false;
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        ChangeWeapon(_currentWeapon);
    }

    private void ChangeWeapon(WeaponScriptableObject newWeapon)
    {
        _currentWeapon = newWeapon;
        _animator.runtimeAnimatorController = _currentWeapon.WeaponSettings.AnimatorController;
    }

    private void Attack()
    {
        if (_isAttackOnCooldown) return;

        _animator.SetTrigger("attackTrigger");
        Collider2D[] _hitEnemies = Physics2D.OverlapBoxAll(_attackRadiusTransform.position, new Vector3(0.6f, 0.5f, 0), 0, _enemyLayerMask);
        StartCoroutine(AttackCooldown());
        if (_hitEnemies.Length == 0) return;
        foreach (Collider2D _hit in _hitEnemies)
        {
            _hit.TryGetComponent(out IDamageable damageable);
            Debug.Log(damageable);
            damageable.GetHit(_currentWeapon.WeaponSettings.WeaponAttackDamage);
        }

    }

    private IEnumerator AttackCooldown()
    {
        _isAttackOnCooldown = true;
        yield return new WaitForSeconds(_currentWeapon.WeaponSettings.WeaponAttackSpeed);
        _isAttackOnCooldown = false;
    }

    void Update()
    {
        // Attack Input
        if (Input.GetKeyDown(KeyCode.Mouse0) && _currentWeapon.IsWeaponEquipped())
        {
            Attack();
        }

        // Change Weapon Inputs
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ChangeWeapon(_equipableWeapons[0]);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ChangeWeapon(_equipableWeapons[1]);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(_attackRadiusTransform.position, new Vector3(0.6f, 0.5f, 0));
    }
}
