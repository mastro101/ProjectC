using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BulletBase : MonoBehaviour
{
    public int ID;
    public State state { get; set; }
    public bool created { get; set; }

    [SerializeField] float duration;
    float returnTime;

    public virtual void Shoot(Vector3 shootPosition, Vector3 direction)
    {
        transform.position = shootPosition;
        state = State.Shooted;
        returnTime = Time.time + duration;
    }
    public virtual void Tick()
    {
        if (Time.time > returnTime)
            Return();
    }
    public virtual void Return()
    {
        BulletPoolManager.instance.ReturnBullet(this);
    }

    private void OnEnable()
    {
        if (!created)
            created = true;
    }

    private void Update()
    {
        if (state == State.Shooted)
            Tick();
    }

    public enum State
    {
        Pooled,
        Shooted,
    }
}