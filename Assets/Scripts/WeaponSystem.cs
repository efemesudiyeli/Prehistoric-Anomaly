using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSystem : MonoBehaviour
{
    [SerializeField] public WeaponScriptableObject _currentWeapon;
    [SerializeField] private WeaponScriptableObject[] _equipableWeapons;
    [SerializeField] private Transform _attackRadiusTransform;
    [SerializeField] private LayerMask _enemyLayerMask;
    [SerializeField] private Vector2 _attackColliderSize = new Vector2(1f, 0.5f);

    private bool _isAttackOnCooldown = false;
    private Animator _animator;
    private bool _isBatEquipped = true;
    private bool _isLightsaberEquipped = false;

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
    }

    private void TryAttack()
    {
        if (_isAttackOnCooldown) return;
        _animator.SetTrigger("attackTrigger");
        StartCoroutine(AttackCooldown());
    }

    private void Attack()
    {
        Collider2D[] _hitEnemies = Physics2D.OverlapBoxAll(_attackRadiusTransform.position, _attackColliderSize, 0, _enemyLayerMask);

        if (_hitEnemies.Length == 0) return;
        foreach (Collider2D _hit in _hitEnemies)
        {
            _hit.TryGetComponent(out IDamageable damageable);
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
        if (Input.GetKeyDown(KeyCode.Mouse0) && GameManager.Instance.IsInputsEnabled && _currentWeapon.IsWeaponEquipped())
        {
            TryAttack();
        }

        if (GameManager.Instance.GameState == GameManager.GameStates.GAMEPLAY && _isLightsaberEquipped == false)
        {
            _isBatEquipped = false;
            _isLightsaberEquipped = true;
            _animator.SetBool("isBatEquipped", _isBatEquipped);
            _animator.SetBool("isLightsaberEquipped", _isLightsaberEquipped);
            ChangeWeapon(_equipableWeapons[1]);
        }

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(_attackRadiusTransform.position, _attackColliderSize);
    }
}
