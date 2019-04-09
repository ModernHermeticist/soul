using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
	private SpriteRenderer sprite;

	public bool visible;
	public bool wasHit;

	public string slot;

	public int minDamage;
	public int maxDamage;

	public int armor;
	public int dodgeChance;
	public int parryChance;
	public int blockChance;

	public string description;


	void Start ()
	{
		sprite = GetComponent<SpriteRenderer>();
		visible = false;
		wasHit = false;
	}

	public void doTurn()
	{
		updateFOV();
	}


	void updateFOV()
	{
		if (visible && wasHit)
		{
			visible = true;
			sprite.enabled = true;
		}
		else if (visible && !wasHit)
		{
			visible = false;
			sprite.enabled = false;
		}
		wasHit = false;
	}

	public void ShowDescription()
	{
		GameObject itemLabel = GameObject.Find("ItemDescriptionLabel");
		GameObject itemDescriptionText = GameObject.Find("ItemDescriptionText");
		GameObject itemStats_1 = GameObject.Find("ItemStats_1");
		
		itemLabel.GetComponent<Text>().text = name;
		itemDescriptionText.GetComponent<Text>().text = description;
		Text stats = itemStats_1.GetComponent<Text>();
		stats.text = "Damage: " + minDamage + "/" + maxDamage + "\n";
		stats.text += "Armor:  " + armor + "\n";
		stats.text += "Block:  " + blockChance + "% \n";
		stats.text += "Dodge:  " + dodgeChance + "% \n";
		stats.text += "Parry:  " + parryChance + "% \n";
	}

}
