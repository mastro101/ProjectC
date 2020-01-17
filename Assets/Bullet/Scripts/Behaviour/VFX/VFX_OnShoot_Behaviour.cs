using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFX_OnShoot_Behaviour : BaseSkillBehaviour
{
    [SerializeField] ParticleSystem vfxPrefab;

    ParticleSystem vfx;

    protected override void OnShoot()
    {
        base.OnShoot();
        vfx = Instantiate(vfxPrefab.gameObject, skill.shooter.transform.position, Quaternion.LookRotation(skill.shooter.aimDirection)).GetComponent<ParticleSystem>();
        vfx.Play();
    }

    protected override void OnReturn()
    {
        base.OnReturn();
        Destroy(vfx.gameObject);
    }
}