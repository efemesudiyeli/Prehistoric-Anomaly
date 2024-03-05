using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IDamageable
{
    [SerializeField] private float _health = 100;
    [SerializeField] private float _moveSpeed = 2f;
    private Animator _animator;
    [SerializeField] private UIController _uiController;


    private void Awake()
    {
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
        if (_health <= 0)
        {
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
        }
        else
        {
            _animator.SetBool("isWalking", false);
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
}
