using System;
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

    public virtual void TakeDamage(int _damage)
    {
        currentHealth -= _damage;
        OnDamage?.Invoke(_damage);
    }

    public virtual void Stun()
    {
        Debug.Log("Stunned");
    }

    protected virtual void Awake()
    {
        myRigidbody = GetComponent<Rigidbody>();
    }
}