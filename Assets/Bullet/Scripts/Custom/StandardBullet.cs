using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandardBullet : BulletBase
{
    [SerializeField] float speed;

    public override void Shoot(Vector3 shootPosition, Vector3 direction, GameObject _shootable)
    {
        base.Shoot(shootPosition, direction, _shootable);
        transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
    }

    public override void Tick()
    {
        base.Tick();
        transform.Translate(Vector3.forward * speed * Time.deltaTime, Space.Self);
    }
}