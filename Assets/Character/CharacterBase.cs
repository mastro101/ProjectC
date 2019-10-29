using System;
using UnityEngine;

public abstract class CharacterBase : MonoBehaviour, IDamageable
{
    #region Serialized
    [SerializeField] int _currentHealth;
    [SerializeField] int _maxHealth; 
    #endregion

    public int currentHealth { get => _currentHealth; set => _currentHealth = value; }
    public int maxHealth     { get => _maxHealth; set => _maxHealth = value; }
    public bool isDead       { get; set; }

    public Action<int> OnDamage { get; set; }

    public virtual void TakeDamage(int _damage)
    {
        currentHealth -= _damage;
        OnDamage?.Invoke(_damage);
    }
}