using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BulletBase : MonoBehaviour
{
    public int ID;
    public State state { get; set; }
    public bool created { get; set; }
    [SerializeField] float duration;
    //TODO: TEMP
    [SerializeField] int damage;
    //
    float returnTime;
    [HideInInspector] public GameObject shootable;

    public virtual void Shoot(Vector3 shootPosition, Vector3 direction, GameObject _shootable)
    {
        shootable = _shootable;
        transform.position = shootPosition;
        state = State.Shooted;
        returnTime = Time.time + duration;
    }
    protected virtual void Tick()
    {
        if (Time.time > returnTime)
            Return();
    }

    public virtual void Return()
    {
        BulletPoolManager.instance.ReturnBullet(this);
    }

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
                if (damageable.gameObject != shootable)
                    OnDamageableCollide(damageable);
            }
        }
    }
}