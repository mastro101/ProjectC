using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Life_DestroyOnDamage_Behaviour : BaseSkillBehaviour
{
    protected override void OnDamage()
    {
        base.OnDamage();
        skill.Return();
    }
}
