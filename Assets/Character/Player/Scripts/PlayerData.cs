using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerData : CharacterBase
{
    #region serialize
    [Header("Movement")]
    public float speed;
    [Header("Dash")]
    public float dodgeDuration;
    public float dodgeSpeed;
    public float dodgeCooldown;
    [Header("SlowMotion")]
    [SerializeField][Range(0f, 1f)] public float slowMoPercent;
    public float timeForSlowMo;
    [Header("StandardSkill")]
    public BulletBase bullet;
    [Header("Sequence")]
    public float timeForSequence;
    public SetSequencesData[] sequences;
    #endregion

    float _slowMoRemainTime;
    [HideInInspector] public float slowMoRemainTime
    {
        get { return _slowMoRemainTime; }
        set {
            OnSlowMoUse?.Invoke(value);
            _slowMoRemainTime = value;
        }
    }

    public System.Action OnSlowMoStarted;
    public System.Action<float> OnSlowMoUse;
    public System.Action OnRefilled;

    protected override void Awake()
    {
        base.Awake();
        OnDeath += (ctx) => Restart();
    }

    //TODO: TEMP
    private void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    //
}