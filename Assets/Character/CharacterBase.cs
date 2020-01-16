using System;
using System.Collections;
using UnityEngine;

public abstract class CharacterBase : MonoBehaviour, IDamageable
{
    #region Serialized
    [SerializeField] int _currentHealth;
    [SerializeField] int _maxHealth;
    #endregion

    public int currentHealth { get => _currentHealth; set { _currentHealth = value; if (_currentHealth <= 0) OnDeath?.Invoke(this); } }
    public int maxHealth     { get => _maxHealth; set => _maxHealth = value; }
    public bool isDead       { get; set; }

    public Action<int> OnDamage { get; set; }
    public Action<IDamageable> OnDeath { get; set; }

    public Rigidbody myRigidbody { get; private set; }
    public bool invulnerable { get; set; }

    public virtual void TakeDamage(int _damage)
    {
        if (!invulnerable)
        {
            currentHealth -= _damage;
            Debug.Log("Ouch");
            OnDamage?.Invoke(_damage);
        }
    }

    public virtual void Stun()
    {
        Debug.Log("Stunned");
    }

    protected virtual void Awake()
    {
        myRigidbody = GetComponent<Rigidbody>();
        invulnerable = false;
    }

    IEnumerator corutine;
    public void TempInvulnerability(float _duration)
    {
        if (corutine != null)
            StopCoroutine(corutine);
        corutine = InvulnerabilityCorutine(_duration);
        StartCoroutine(corutine);
    }

    IEnumerator InvulnerabilityCorutine(float _duration)
    {
        invulnerable = true;
        yield return new WaitForSeconds(_duration);
        invulnerable = false;
    }
}