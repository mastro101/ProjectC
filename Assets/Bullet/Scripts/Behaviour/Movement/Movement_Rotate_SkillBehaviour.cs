using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement_Rotate_SkillBehaviour : BaseSkillBehaviour
{
    [SerializeField] float rotationSpeed;

    [SerializeField] bool counterclockwise;

    protected override void Tick()
    {
        base.Tick();
        if (counterclockwise)
            skill.transform.Rotate(0, -rotationSpeed * Time.deltaTime, 0);
        else
            skill.transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
    }
}