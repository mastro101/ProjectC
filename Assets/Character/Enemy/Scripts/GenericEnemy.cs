﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericEnemy : CharacterBase , IShooter
{
    [SerializeField] BulletBase bullet;
    [SerializeField] float viewRadious;
    [SerializeField] float fireRate;

    Transform targetTransform;
    float timer;

    Collider[] colliders;

    public Vector3 aimDirection { get; set; }

    public Action OnDestroy { get; set; }

    protected override void Awake()
    {
        base.Awake();
        OnDeath += Death;
    }

    private void Update()
    {
        colliders = Physics.OverlapSphere(transform.position, viewRadious);

        foreach (var item in colliders)
        {
            PlayerData player = item.GetComponentInParent<PlayerData>();
            if (player != null)
            {
                aimDirection = player.transform.position - transform.position;
                timer += Time.deltaTime;
                if (timer >= fireRate)
                    Attack(player.transform);
            }
        }
    }

    void Attack(Transform target)
    {
        timer = 0f;
        BulletPoolManager.instance.Shoot(bullet, transform.position, aimDirection, this);
    }

    void Death(IDamageable _damageable)
    {
        OnDeath -= Death;
        Destroy(gameObject);
    }

    public override void TakeDamage(int _damage)
    {
        base.TakeDamage(_damage);
        timer = 0;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, viewRadious);
    }
}