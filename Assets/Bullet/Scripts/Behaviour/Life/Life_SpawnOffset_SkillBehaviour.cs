using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Life_SpawnOffset_SkillBehaviour : BaseSkillBehaviour
{
    [SerializeField]
    [Tooltip("Sposta la posizione di spawn (valore di 0,0,0 equivale alla posizione del player)")]
    Vector3 offsetPositionOnShoot;

    protected override void OnPreShoot()
    {
        base.OnPreShoot();
        skill.transform.position = skill.shooter.transform.position + offsetPositionOnShoot;
    }
}
