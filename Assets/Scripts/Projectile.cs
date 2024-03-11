using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Rigidbody2D _rigidbody2D;
    private void Start() {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    Invoke(nameof(DestroyProjectile), 5f);
    }
    private void Update() {
        _rigidbody2D.velocity = transform.right * 500f * Time.deltaTime;
    }

    private void DestroyProjectile() {
        Destroy(gameObject);
    }

      private void OnTriggerEnter2D(Collider2D other) {
        if (other.TryGetComponent(out BaseEnemy enemy))
        {
            enemy.GetHit(10f);
            DestroyProjectile();
        } 
      }

}
