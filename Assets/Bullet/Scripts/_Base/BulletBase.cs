using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBase : MonoBehaviour
{
    public int ID;
    [SerializeField] protected ParticleSystem vfx;
    [SerializeField] float duration;
    //TODO: Da considerare temporaneo fino ulteriori informazioni
    [SerializeField] int damage;
    //

    public State state { get; set; }
    public bool created { get; set; }
    float returnTime;
    [HideInInspector] public IShooter shooter;
    public Vector3 newWorldPosition { private get; set; }
    public Vector3 newLocalPosition { private get; set; }

    public Action OnShoot;
    public Action OnReturn;

    protected virtual void Tick()
    {
        if (Time.time > returnTime)
            Return();
    }

    #region API
    public virtual void Shoot(Vector3 shootPosition, Vector3 direction, IShooter _shooter)
    {
        shooter = _shooter;
        transform.position = shootPosition;
        state = State.Shooted;
        returnTime = Time.time + duration;
        OnShoot?.Invoke();
        if (vfx != null)
            vfx.Play();
    }

    public virtual void Return()
    {
        if (vfx != null)
        {
            vfx.Stop();
            vfx.Clear();
        }
        BulletPoolManager.instance.ReturnBullet(this);
        OnReturn?.Invoke();
    }
    #endregion

    public virtual void OnDamageableCollide(IDamageable damageable)
    {
        damageable.TakeDamage(damage);
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


    private void OnTriggerEnter(Collider other)
    {
        if (state == State.Shooted)
        {
            IDamageable damageable = other.GetComponentInParent<IDamageable>();
            if (damageable != null)
            {
                if (damageable.gameObject != shooter.gameObject)
                    OnDamageableCollide(damageable);
            }
        }
    }
}