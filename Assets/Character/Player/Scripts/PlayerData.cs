﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : CharacterBase
{
    #region serialize
    public float speed;
    public float timeForSequence;
    [SerializeField][Range(0f, 1f)] public float slowMoPercent;
    public float timeForSlowMo;
    public BulletBase bullet;
    public CommandSequenceBase[] sequences;
    #endregion

    [HideInInspector] public float slowMoRemainTime { get { return _slowMoRemainTime; } set { OnSlowMoUsed?.Invoke(value); _slowMoRemainTime = value; } }
    float _slowMoRemainTime;

    public System.Action<float> OnSlowMoUsed;
}