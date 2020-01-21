using System;
using System.Collections;
using UnityEngine;

public abstract class CharacterBase : MonoBehaviour, IDamageable
{
    const float KNOCKBACK_RATE = 1f;

    #region Serialized
    [Header("Health")]
    [SerializeField] int _currentHealth;
    [SerializeField] int _maxHealth;
    #endregion

    #region Interface_Damageable
    public int currentHealth { get => _currentHealth; set { _currentHealth = value; if (_currentHealth <= 0) OnDeath?.Invoke(this); } }
    public int maxHealth { get => _maxHealth; set => _maxHealth = value; }
    public bool isDead { get; set; }
    public bool invulnerable { get; set; } 
    #endregion

    public Action<int> OnDamage { get; set; }
    public Action<IDamageable> OnDeath { get; set; }

    public Rigidbody myRigidbody { get; private set; }

    public bool knockbackState { get; protected set; }
    public bool canMove;

    public virtual void TakeDamage(int _damage)
    {
        if (!invulnerable)
        {
            currentHealth -= _damage;
            Debug.Log("Ouch");
            OnDamage?.Invoke(_damage);
        }
    }

    public virtual void Stun(float _duration)
    {
        Debug.Log("Stunned");
    }

    IEnumerator knockbackCorutine;
    public virtual void KnockBack(float _force, Vector3 _hitPoint)
    {
        if (!knockbackState)
        {
            if (knockbackCorutine != null)
                StopCoroutine(knockbackCorutine);
            knockbackState = true;
            knockbackCorutine = KnockbackCorutine();
            myRigidbody.AddExplosionForce(_force, _hitPoint, 1f, 0f, ForceMode.Impulse);
            StartCoroutine(knockbackCorutine);
        }
    }

    IEnumerator KnockbackCorutine()
    {
        yield return new WaitForSeconds(KNOCKBACK_RATE);
        knockbackState = false;
    }

    protected virtual void Awake()
    {
        myRigidbody = GetComponent<Rigidbody>();
        invulnerable = false;
    }

    IEnumerator invulnerabilityCorutine;
    public void TempInvulnerability(float _duration)
    {
        if (invulnerabilityCorutine != null)
            StopCoroutine(invulnerabilityCorutine);
        invulnerabilityCorutine = InvulnerabilityCorutine(_duration);
        StartCoroutine(invulnerabilityCorutine);
    }

    IEnumerator InvulnerabilityCorutine(float _duration)
    {
        invulnerable = true;
        yield return new WaitForSeconds(_duration);
        invulnerable = false;
    }
}