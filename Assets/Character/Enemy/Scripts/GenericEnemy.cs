using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericEnemy : CharacterBase
{
    [SerializeField] BulletBase bullet;

    protected override void Awake()
    {
        base.Awake();
        OnDeath += Death;
    }



    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            Attack();
    }

    void Attack()
    {
        //if (bullet != null)
        //{
        //    BulletPoolManager.instance.TakeBullet(bullet).Shoot(transform.position + Vector3.up * 0.5f, Vector3.right, this.gameObject);
        //    BulletPoolManager.instance.TakeBullet(bullet).Shoot(transform.position + Vector3.up * 0.5f, Vector3.forward, this.gameObject);
        //    BulletPoolManager.instance.TakeBullet(bullet).Shoot(transform.position + Vector3.up * 0.5f, Vector3.back, this.gameObject);
        //    BulletPoolManager.instance.TakeBullet(bullet).Shoot(transform.position + Vector3.up * 0.5f, Vector3.left, this.gameObject);
        //}
    }

    void Death(IDamageable _damageable)
    {
        OnDeath -= Death;
        Destroy(gameObject);
    }
}