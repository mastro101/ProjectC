using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MegaFlare", menuName = "Commands/MegaFlare")]
public class CommandMegaFlare : CommandSequenceBase
{
    [SerializeField] FireBullet flarePrefab;

    public CommandMegaFlare(IShooter controller, params InputData[] inputDatas) : base(controller, inputDatas)
    {

    }

    public override void Execute()
    {
        base.Execute();
        if (skillPrefab)
            BulletPoolManager.instance.TakeBullet(flarePrefab).Shoot(controller.transform.position, controller.aimDirection, controller);
    }
}