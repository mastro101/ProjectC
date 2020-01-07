using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBullet : BulletBase
{

    public override void Shoot(Vector3 shootPosition, Vector3 direction, IShooter _shooter)
    {
        base.Shoot(shootPosition, direction, _shooter);
        vfx.Play();
    }

    public override void Return()
    {
        base.Return();
        vfx.Stop();
    }
}
