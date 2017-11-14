using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "Consumable Item", menuName = "Inventory/Stackable Item/ConsumableItem")]
public class UsableItem : StackableItem
{
	public enum UsableItemType { HealthPotion, ManaPotion, PoisonPotion, DrainPotion, StewOfStrength, DexyFish}
    public UsableItemType TypeOfUsableItem;

    public int EffectAmount = 0;

    public override void Use ( )
    {
        base.Use ();

        switch (TypeOfUsableItem)
        {
            case UsableItemType.HealthPotion:
                PlayerStats.instance.Heal (EffectAmount);
                break;

            case UsableItemType.ManaPotion:
                PlayerStats.instance.ReplenishMana (EffectAmount);
                break;

            case UsableItemType.PoisonPotion:
                PlayerStats.instance.TakeDamage (EffectAmount);
                break;

            case UsableItemType.DrainPotion:
                PlayerStats.instance.DepleteMana (EffectAmount);
                break;

            case UsableItemType.StewOfStrength:
                PlayerStats.instance.StartCoroutine (PlayerStats.instance.BuffWithConstantValue ("Strength", EffectAmount, 4.0f, Icon));                
                break;

            case UsableItemType.DexyFish:
                PlayerStats.instance.StartCoroutine (PlayerStats.instance.BuffGradualyAndHoldValue ("Dexterity", EffectAmount, 2, 5, Icon));
                break;
        }        
    }
}
