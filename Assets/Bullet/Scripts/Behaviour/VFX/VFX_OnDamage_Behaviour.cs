using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFX_OnDamage_Behaviour : BaseSkillBehaviour
{
    [SerializeField] ParticleSystem vfxPrefab;

    ParticleSystem vfx;

    protected override void OnDamage()
    {
        base.OnShoot();
        vfx = Instantiate(vfxPrefab.gameObject, skill.transform.position, Quaternion.LookRotation(skill.shooter.aimDirection)).GetComponent<ParticleSystem>();
        vfx.Play();
        StartCoroutine(Corutine());
    }

    IEnumerator Corutine()
    {
        yield return new WaitForSeconds(vfx.main.duration);
        Destroy(vfx.gameObject);
    }
}