using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBullet : BulletBase
{
    [SerializeField] ParticleSystem vfx;

    public override void Shoot(Vector3 shootPosition, Vector3 direction, GameObject _shootable)
    {
        base.Shoot(shootPosition, direction, _shootable);
        vfx.Play();
    }

    public override void Return()
    {
        base.Return();
        vfx.Stop();
    }
}
