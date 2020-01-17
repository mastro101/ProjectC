using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BulletBase))]
public abstract class BaseSkillBehaviour : MonoBehaviour
{
    protected BulletBase skill;

    protected virtual void Awake()
    {
        skill = GetComponentInParent<BulletBase>();
        skill.OnPreShoot += OnPreShoot;
        skill.OnShoot    += OnShoot;
        skill.OnDamage   += OnDamage;
        skill.OnReturn   += OnReturn;
    }

    protected virtual void OnPreShoot()
    {

    }

    protected virtual void OnShoot()
    {
        
    }

    protected virtual void OnDamage()
    {

    }

    protected virtual void OnReturn()
    {

    }

    private void Update()
    {
        if (skill.state == BulletBase.State.Shooted)
        {
            Tick();
        }
    }

    protected virtual void Tick()
    {

    }

    private void OnDisable()
    {
        skill.OnPreShoot -= OnPreShoot;
        skill.OnShoot    -= OnShoot;
        skill.OnDamage   -= OnDamage;
        skill.OnReturn   -= OnReturn;
    }
}
