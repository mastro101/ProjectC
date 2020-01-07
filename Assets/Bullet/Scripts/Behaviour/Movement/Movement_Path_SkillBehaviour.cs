using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Movement_Path_SkillBehaviour : BaseSkillBehaviour
{
    [SerializeField] Vector3[] path;
    [SerializeField] float durationPath;
    [SerializeField] [Tooltip("-1 for infinite")]int loops;
    [SerializeField] LoopType loopType;

    Tween tween;

    protected override void OnShoot()
    {
        base.OnShoot();
        tween = skill.transform.DOPath(path, durationPath).SetRelative().SetLoops(loops, loopType).Play().SetEase(Ease.Linear);
    }

    protected override void OnReturn()
    {
        base.OnReturn();
        tween.Pause();
        tween.Kill();
    }
}
