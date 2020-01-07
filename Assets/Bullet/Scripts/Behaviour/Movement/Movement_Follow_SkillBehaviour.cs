using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement_Follow_SkillBehaviour : BaseSkillBehaviour
{
    Vector3 oldPos;
    Vector3 newPos;

    protected override void OnShoot()
    {
        base.OnShoot();
        oldPos = skill.shooter.transform.position;
    }

    protected override void Tick()
    {
        base.Tick();
        newPos = skill.shooter.transform.position;
        skill.transform.position += newPos - oldPos;
        oldPos = newPos;
    }
}