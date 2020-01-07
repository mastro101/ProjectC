using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement_Forward_SkillBehaviour : BaseSkillBehaviour
{
    [SerializeField] float speed;

    Vector3 direction;

    protected override void OnShoot()
    {
        base.OnShoot();
        direction = skill.shooter.aimDirection;
    }

    protected override void Tick()
    {
        base.Tick();
        skill.transform.position += direction * Time.deltaTime * speed;
    }
}