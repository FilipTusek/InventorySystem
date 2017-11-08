﻿using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int MaxHealth = 100;
    public int CurrentHealth { get; private set; }

    public int MaxMana = 100;
    public int CurrentMana { get; private set; }

    public Stat Strength;
    public Stat Dexterity;
    public Stat Agility;
    public Stat Inteligence;

    private void Awake ( )
    {
        CurrentHealth = MaxHealth;
        CurrentMana = MaxMana;
    }

    private void Start ( )
    {
        EquipmentManager.instance.OnEquipmentChangedCallback += OnEquipmentChanged;
    }

    public void TakeDamage(int amount)
    {
        CurrentHealth -= amount;
    }

    public void Heal(int amount)
    {
        CurrentHealth += amount;
        CurrentHealth = Mathf.Clamp (CurrentHealth, 0, MaxHealth);
    }

    public void DepleteMana(int amount)
    {
        CurrentMana -= amount;
    }

    public void ReplenishMana(int amount)
    {
        CurrentMana += amount;
        CurrentMana = Mathf.Clamp (CurrentMana, 0, MaxMana);
    }

    private void OnEquipmentChanged (Equipment newItem, Equipment oldItem)
    {
        if (newItem != null)
        {
            Strength.AddModifier (newItem.StrengthModifier);
            Dexterity.AddModifier (newItem.DexterityModifier);
            Agility.AddModifier (newItem.AgilityModifier);
            Inteligence.AddModifier (newItem.InteligenceModifier);
        }

        if(oldItem != null)
        {
            Strength.RemoveModifier (oldItem.StrengthModifier);
            Dexterity.RemoveModifier (oldItem.DexterityModifier);
            Agility.RemoveModifier (oldItem.AgilityModifier);
            Inteligence.RemoveModifier (oldItem.InteligenceModifier);
        }
    }
}