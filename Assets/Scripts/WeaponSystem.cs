using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSystem : MonoBehaviour
{
    [SerializeField] private WeaponScriptableObject _currentWeapon;
    [SerializeField] private WeaponScriptableObject[] _equipableWeapons;
    [SerializeField] private Transform _attackRadiusTransform;

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
        _animator.SetTrigger("attackTrigger");
        Collider2D[] _hitEnemies = Physics2D.OverlapCapsuleAll(_attackRadiusTransform.position, new Vector2(0.3f, 0.54f), CapsuleDirection2D.Vertical, 0);
        if (_hitEnemies.Length == 0) return;
        foreach (Collider2D _hit in _hitEnemies)
        {
            _hit.TryGetComponent(out IDamageable damageable);
            Debug.Log(damageable);
            damageable.GetHit(_currentWeapon.WeaponSettings.WeaponAttackDamage);
        }
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
}
