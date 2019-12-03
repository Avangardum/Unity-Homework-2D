using UnityEngine;
using System;

public class Damagable : MonoBehaviour
{
    [SerializeField] private int _health;
    [SerializeField] private int _maxHealth;

    public int Health
    {
        get => _health;
        private set
        {
            _health = value;
            HealthChanged?.Invoke();
        }
    }

    public int MaxHealth => _maxHealth;

    public event Action HealthChanged;
    public event Action Death;

    private void Start()
    {
        if (Health <= 0)
            GameObject.Destroy(gameObject);
    }

    public void Damage(int damage)
    {
        Health -= damage;
        if (Health <= 0)
        {
            GameObject.Destroy(gameObject);
            Death?.Invoke();
        }
    }

    public void Heal(int healAmount)
    {
        Health += healAmount;
        if (Health > _maxHealth)
            Health = _maxHealth;
    }
}