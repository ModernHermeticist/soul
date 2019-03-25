using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : MonoBehaviour
{
	private PlayerController player;

	private Item mainHand;
	private Item offHand;
	private Item chest;

	public bool mainHandEquipped;
	public bool offHandEquipped;
	public bool chestEquipped;


	private void Start()
	{
		player = GameObject.Find("Player").GetComponent<PlayerController>();
		mainHandEquipped = false;
		offHandEquipped = false;
		chestEquipped = false;
	}


	public int EquipOrUnequipItem(Item item)
	{
		string s = item.slot;
		int identifier = 0;
		if (s == null) return -1;
		switch (s)
		{
			case "Main Hand":
				{
					if (mainHand == null)
					{
						mainHand = item;
						player.damageMin += mainHand.minDamage;
						player.damageMax += mainHand.maxDamage;
						player.playerArmor += mainHand.armor;
						player.playerBlockChance += mainHand.blockChance;
						player.playerParryChance += mainHand.parryChance;
						player.playerDodgeChance += mainHand.dodgeChance;
						mainHandEquipped = true;
					}
					else
					{
						player.damageMin -= mainHand.minDamage;
						player.damageMax -= mainHand.maxDamage;
						player.playerArmor -= mainHand.armor;
						player.playerBlockChance -= mainHand.blockChance;
						player.playerParryChance -= mainHand.parryChance;
						player.playerDodgeChance -= mainHand.dodgeChance;
						mainHand = null;
						mainHandEquipped = false;
						identifier = 1;
					}
					break;
				}
			case "Off Hand":
				{
					if (offHand == null)
					{
						offHand = item;
						player.damageMin += offHand.minDamage;
						player.damageMax += offHand.maxDamage;
						player.playerArmor += offHand.armor;
						player.playerBlockChance += offHand.blockChance;
						player.playerParryChance += offHand.parryChance;
						player.playerDodgeChance += offHand.dodgeChance;
						offHandEquipped = true;
					}
					else
					{
						player.damageMin -= offHand.minDamage;
						player.damageMax -= offHand.maxDamage;
						player.playerArmor -= offHand.armor;
						player.playerBlockChance -= offHand.blockChance;
						player.playerParryChance -= offHand.parryChance;
						player.playerDodgeChance -= offHand.dodgeChance;
						offHand = null;
						offHandEquipped = false;
						identifier = 1;
					}
					break;
				}
			case "Chest":
				{
					if (chest == null)
					{
						chest = item;
						player.damageMin += chest.minDamage;
						player.damageMax += chest.maxDamage;
						player.playerArmor += chest.armor;
						player.playerBlockChance += chest.blockChance;
						player.playerParryChance += chest.parryChance;
						player.playerDodgeChance += chest.dodgeChance;
						chestEquipped = true;
					}
					else
					{
						player.damageMin -= chest.minDamage;
						player.damageMax -= chest.maxDamage;
						player.playerArmor -= chest.armor;
						player.playerBlockChance -= chest.blockChance;
						player.playerParryChance -= chest.parryChance;
						player.playerDodgeChance -= chest.dodgeChance;
						chest = null;
						chestEquipped = false;
						identifier = 1;
					}
					break;
				}
			default: return -1;
		}

		return identifier;
	}

	public Item GetMainHand()
	{
		return mainHand;
	}

	public Item GetOffHand()
	{
		return offHand;
	}

	public Item GetChest()
	{
		return chest;
	}
	
}
