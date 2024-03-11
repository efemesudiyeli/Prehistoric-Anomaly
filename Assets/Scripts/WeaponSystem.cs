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
    private bool _isBowEquipped = false;
    private bool _isAkEquipped = false;
    private bool _isRPGEquipped = false;
    private bool _isLightsaberEquipped = false;
    private bool _isAtomicBombEquipped = false;

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

    private void RangedAttack()
    {
       Instantiate( _currentWeapon.WeaponSettings.WeaponProjectile, _attackRadiusTransform.position, _attackRadiusTransform.rotation);
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


        if (GameManager.Instance.GameState == GameManager.GameStates.BOW && _isBowEquipped == false)
        {
            _isBatEquipped = false;
            _isBowEquipped = true;
            _animator.SetBool("isBatEquipped", _isBatEquipped);
            _animator.SetBool("isBowEquipped", _isBowEquipped);
            ChangeWeapon(_equipableWeapons[0]); //BOW
        }

        if (GameManager.Instance.GameState == GameManager.GameStates.AK47 && _isAkEquipped == false)
        {
            _isBowEquipped = false;
            _isAkEquipped = true;
            _animator.SetBool("isBowEquipped", _isBowEquipped);
            _animator.SetBool("isAkEquipped", _isAkEquipped);
            ChangeWeapon(_equipableWeapons[1]);
        }
        if (GameManager.Instance.GameState == GameManager.GameStates.RPG && _isRPGEquipped == false)
        {
            _isAkEquipped = false;
            _isRPGEquipped = true;
            _animator.SetBool("isBowEquipped", _isBowEquipped);
            _animator.SetBool("isAkEquipped", _isRPGEquipped);
            ChangeWeapon(_equipableWeapons[2]);
        }
        if (GameManager.Instance.GameState == GameManager.GameStates.LIGHTSABER && _isLightsaberEquipped == false)
        {
            _isRPGEquipped = false;
            _isLightsaberEquipped = true;
            _animator.SetBool("isBowEquipped", _isBowEquipped);
            _animator.SetBool("isAkEquipped", _isLightsaberEquipped);
            ChangeWeapon(_equipableWeapons[3]);
        }
        if (GameManager.Instance.GameState == GameManager.GameStates.ATOMICBOMB && _isAtomicBombEquipped == false)
        {
            _isLightsaberEquipped = false;
            _isAtomicBombEquipped = true;
            _animator.SetBool("isBowEquipped", _isBowEquipped);
            _animator.SetBool("isAkEquipped", _isAtomicBombEquipped);
            ChangeWeapon(_equipableWeapons[4]);
        }

       

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(_attackRadiusTransform.position, _attackColliderSize);
    }
}
