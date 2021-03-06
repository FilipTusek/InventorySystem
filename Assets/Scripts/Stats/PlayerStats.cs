﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats instance;

    public Image HealthBar;
    public Image ManaBar;

    public List<Image> Buffs;

    public float MaxHealth = 100.0f;
    public float CurrentHealth { get; private set; }

    public float MaxMana = 100.0f;
    public float CurrentMana { get; private set; }

    public float Fill;

    public Stat Strength;
    public Stat Dexterity;
    public Stat Agility;
    public Stat Inteligence;

    private AttributesUI _attributesUI;

    private int _buffSlot;

    private void Awake ( )
    {
        if(instance == null)
        {
            instance = this;
        }

        CurrentHealth = MaxHealth;
        CurrentMana = MaxMana;
    }

    private void Start ( )
    {
        _attributesUI = AttributesUI.instance;        

        EquipmentManager.instance.OnEquipmentChangedCallback += OnEquipmentChanged;
    }    

    public void TakeDamage(float amount)
    {
        CurrentHealth -= amount;

        HealthBar.fillAmount = CurrentHealth / MaxHealth;
    }

    public void Heal(float amount)
    {
        CurrentHealth += amount;
        CurrentHealth = Mathf.Clamp (CurrentHealth, 0, MaxHealth);

        HealthBar.fillAmount = CurrentHealth / MaxHealth;
    }

    public void DepleteMana(int amount)
    {
        CurrentMana -= amount;

        ManaBar.fillAmount = CurrentMana / MaxMana;
    }

    public void ReplenishMana(int amount)
    {
        CurrentMana += amount;
        CurrentMana = Mathf.Clamp (CurrentMana, 0, MaxMana);

        ManaBar.fillAmount = CurrentMana / MaxMana;
    }
    
    public IEnumerator BuffWithConstantValue(string stat, float buffPercent, float time, Sprite sprite)
    {
        float modifier;
        float buffTime = time;      

        if (stat == "Strength")
        {
            modifier = Strength.GetValue () * (buffPercent / 100);
            Strength.AddModifier ((int) modifier);

            _attributesUI.UpdateAttributes ();

            FindFreeBuffIcon ();

            int slot = _buffSlot;

            Buffs [slot].gameObject.SetActive (true);
            Buffs [slot].sprite = sprite;            
            
            while (buffTime > 0.0f)
            {
                buffTime -= Time.deltaTime;

                Buffs [slot].fillAmount = buffTime / time;
                yield return 0;
            }
            Strength.RemoveModifier ((int) modifier);

            _attributesUI.UpdateAttributes ();

            Buffs [slot].sprite = null;
            Buffs [slot].gameObject.SetActive (false);
        }
    }

    public IEnumerator BuffGradualyAndHoldValue(string stat, float amount, int changeTime, float duration, Sprite sprite)
    {
        float time = 0;      

        float changeAmount = amount / changeTime;
        float currentChange = 0;

        FindFreeBuffIcon ();

        int slot = _buffSlot;

        Buffs [slot].gameObject.SetActive (true);

        Buffs [slot].sprite = sprite;

        if (stat == "Dexterity")
        {
            while(time <= changeTime)
            {
                if(currentChange < 1.0f)
                {
                    currentChange += changeAmount * Time.deltaTime;                    
                }
                else
                {
                    Dexterity.AddModifier (1);
                    _attributesUI.UpdateAttributes ();
                    currentChange -= 1;
                    currentChange += changeAmount * Time.deltaTime;
                }

                Buffs [slot].fillAmount = (duration + changeTime - time) / (duration + changeTime);

                time += Time.deltaTime;                
                yield return 0;
            }
            Dexterity.AddModifier (1);
            _attributesUI.UpdateAttributes ();

            while (time > changeTime && time < duration + changeTime)
            {
                Buffs [slot].fillAmount = (duration + changeTime - time) / (duration + changeTime);
                time += Time.deltaTime;               
                yield return 0;
            }

            Buffs [slot].sprite = null;
            Buffs [slot].gameObject.SetActive (false);

            for (int i = 0; i < amount; i++)
            {
                Dexterity.RemoveModifier (1);                
                yield return 0;
            }
            _attributesUI.UpdateAttributes ();            
        }        
    }

    public IEnumerator HealOverTime ( float amount, float duration, Sprite sprite )
    {
        float tickAmount = amount / duration;

        float buffTime = duration;

        FindFreeBuffIcon ();

        int slot = _buffSlot;

        Buffs [slot].gameObject.SetActive (true);
        Buffs [slot].sprite = sprite;

        while (buffTime > 0)
        {
            Heal (tickAmount * Time.deltaTime);          

            buffTime -= Time.deltaTime;

            Buffs [slot].fillAmount = buffTime / duration;

            yield return 0;
        }

        Buffs [slot].sprite = null;
        Buffs [slot].gameObject.SetActive (false);        
    }

    private void FindFreeBuffIcon()
    {
        bool buffActive = false;

        for (int i = 0; i < Buffs.Count; i++)
        {
            if (!buffActive)
            {
                if (Buffs [i].sprite == null)
                {
                    buffActive = true;
                    _buffSlot = i;                    
                }
            }
        }
    }

    private void OnEquipmentChanged (Equipment newItem, Equipment oldItem)
    {
        if (newItem != null)
        {
            Strength.AddModifier (newItem.StrengthModifier);
            Dexterity.AddModifier (newItem.DexterityModifier);
            Agility.AddModifier (newItem.AgilityModifier);
            Inteligence.AddModifier (newItem.InteligenceModifier);

            _attributesUI.UpdateAttributes ();
        }

        if(oldItem != null)
        {
            Strength.RemoveModifier (oldItem.StrengthModifier);
            Dexterity.RemoveModifier (oldItem.DexterityModifier);
            Agility.RemoveModifier (oldItem.AgilityModifier);
            Inteligence.RemoveModifier (oldItem.InteligenceModifier);

            _attributesUI.UpdateAttributes ();
        }
    }
}
