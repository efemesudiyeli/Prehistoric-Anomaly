using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour, IDamageable
{
    [SerializeField] private float _health = 100;
    [SerializeField] private float _moveSpeed = 2f;
    [SerializeField] private UIController _uiController;
    [SerializeField] private HitReceiveFlashEffect _hitReceiver;
    private AudioSource _audioSource;
    [SerializeField] private AudioClip _walkSound;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    private bool _isWalking = false;
    private bool _isWalkSoundPlaying = false;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (GameManager.Instance.IsInputsEnabled)
        {
            Move();
        }
    }

    public void GetHit(float attackDamage)
    {
        _health -= attackDamage;
        _uiController.SetHealth(_health);
        _hitReceiver.GetFlashEffect(_spriteRenderer);
        if (_health <= 0)
        {
            GameManager.Instance.LoadMainMenu();
            Destroy(gameObject);
        }
    }

    private void Move()
    {
        Vector2 direction = new Vector2(Input.GetAxis("Horizontal"), 0);
        transform.Translate(_moveSpeed * Time.deltaTime * direction, Space.World);

        SetWalkingAnimation(direction);
        FlipCharacter(direction);
    }

    private void SetWalkingAnimation(Vector2 direction)
    {
        if (direction != Vector2.zero)
        {
            _animator.SetBool("isWalking", true);
            _isWalking = true;
            StartCoroutine(WalkSound());
        }
        else
        {
            _animator.SetBool("isWalking", false);
            _isWalking = false;
        }
    }

    private void FlipCharacter(Vector2 direction)
    {
        if (direction.x > 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else if (direction.x < 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    private IEnumerator WalkSound()
    {
        if (_isWalkSoundPlaying) yield break;
        _isWalkSoundPlaying = true;


        while (_isWalking)
        {
            _audioSource.pitch = UnityEngine.Random.Range(0.9f, 1.3f);
            _audioSource.PlayOneShot(_walkSound, 0.1f);
            yield return new WaitForSeconds(0.5f);
        }

        _isWalkSoundPlaying = false;

    }
}
