using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class WeaponSystem : MonoBehaviour
{
    [SerializeField] private WeaponScriptableObject[] _equipableWeapons;
    [SerializeField] private Transform _attackRadiusTransform;
    [SerializeField] private LayerMask _enemyLayerMask;
    [SerializeField] private Vector2 _attackColliderSize = new Vector2(1f, 0.5f);
    [SerializeField] private PlayableDirector _timeline;
    [SerializeField] private SpriteRenderer _timelineWeaponSpriteRenderer;
    [SerializeField] private Sprite ak47Sprite, rpgSprite, lightsaberSprite, atomicBombSprite;
    public WeaponScriptableObject _currentWeapon;
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
        Instantiate(_currentWeapon.WeaponSettings.WeaponProjectile, _attackRadiusTransform.position, _attackRadiusTransform.rotation);
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
        if (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Space) && GameManager.Instance.IsInputsEnabled && _currentWeapon.IsWeaponEquipped())
        {
            TryAttack();
        }

        switch (GameManager.Instance.GameState)
        {
            case GameManager.GameStates.BOW when _isBowEquipped == false:
                GameManager.Instance.IsInputsEnabled = false;
                _timeline.Play();
                _isBatEquipped = false;
                _isBowEquipped = true;
                _animator.SetBool("isBatEquipped", _isBatEquipped);
                _animator.SetBool("isBowEquipped", _isBowEquipped);
                ChangeWeapon(_equipableWeapons[0]); //BOW
                break;
            case GameManager.GameStates.AK47 when _isAkEquipped == false:
                GameManager.Instance.IsInputsEnabled = false;
                _timelineWeaponSpriteRenderer.sprite = ak47Sprite;
                _timeline.Play();
                _isBowEquipped = false;
                _isAkEquipped = true;
                _animator.SetBool("isBowEquipped", _isBowEquipped);
                _animator.SetBool("isAkEquipped", _isAkEquipped);
                ChangeWeapon(_equipableWeapons[1]);
                break;
            case GameManager.GameStates.RPG when _isRPGEquipped == false:
                GameManager.Instance.IsInputsEnabled = false;
                _timelineWeaponSpriteRenderer.sprite = rpgSprite;
                _timeline.Play();
                _isAkEquipped = false;
                _isRPGEquipped = true;
                _animator.SetBool("isAkEquipped", _isAkEquipped);
                _animator.SetBool("isRpgEquipped", _isRPGEquipped);
                ChangeWeapon(_equipableWeapons[2]);
                break;
            case GameManager.GameStates.LIGHTSABER when _isLightsaberEquipped == false:
                GameManager.Instance.IsInputsEnabled = false;
                _timelineWeaponSpriteRenderer.sprite = lightsaberSprite;
                _timeline.Play();
                _isRPGEquipped = false;
                _isLightsaberEquipped = true;
                _animator.SetBool("isRpgEquipped", _isRPGEquipped);
                _animator.SetBool("isLightsaberEquipped", _isLightsaberEquipped);
                ChangeWeapon(_equipableWeapons[3]);
                break;
            case GameManager.GameStates.ATOMICBOMB when _isAtomicBombEquipped == false:
                GameManager.Instance.IsInputsEnabled = false;
                GameManager.Instance.PlayFinalCutscene();
                break;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(_attackRadiusTransform.position, _attackColliderSize);
    }
}
