using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSheetController : MonoBehaviour
{
	private GameObject characterSheetImage;
	private GameObject characterSheetElement;

	private Text valueText;

	public string CharacterName { get; set; }
	public string CharacterRace { get; set; }
	public string CharacterClass { get; set; }
	public string CharacterLevel { get; set; }
	public string CharacterExperience { get; set; }
	public string CharacterExperienceMax { get; set; }
	public string CharacterDamageMin { get; set; }
	public string CharacterDamageMax { get; set; }
	public string CharacterArmor { get; set; }
	public string CharacterBlockChance { get; set; }
	public string CharacterParryChance { get; set; }
	public string CharacterDodgeChance { get; set; }
	public string CharacterStrength { get; set; }
	public string CharacterDexterity { get; set; }
	public string CharacterStamina { get; set; }
	public string CharacterIntelligence { get; set; }

	// Use this for initialization
	void Start ()
	{
		characterSheetImage = GameObject.Find("CharacterSheetImage");
	}

	// Update is called once per frame
	public void refresh()
	{
		characterSheetElement = GameObject.Find("NameText");
		valueText = characterSheetElement.GetComponent<Text>();

		valueText.text = "Name:  " + CharacterName;

		characterSheetElement = GameObject.Find("RaceText");
		valueText = characterSheetElement.GetComponent<Text>();

		valueText.text = "Race:  " + CharacterRace;

		characterSheetElement = GameObject.Find("ClassText");
		valueText = characterSheetElement.GetComponent<Text>();

		valueText.text = "Class: " + CharacterClass;

		characterSheetElement = GameObject.Find("LevelText");
		valueText = characterSheetElement.GetComponent<Text>();

		valueText.text = "Level: " + CharacterLevel;

		characterSheetElement = GameObject.Find("ExperienceText");
		valueText = characterSheetElement.GetComponent<Text>();

		valueText.text = "Experience: " + CharacterExperience + "/" + CharacterExperienceMax;

		characterSheetElement = GameObject.Find("DamageText");
		valueText = characterSheetElement.GetComponent<Text>();

		valueText.text = "Damage: " + CharacterDamageMin + "/" + CharacterDamageMax;

		characterSheetElement = GameObject.Find("ArmorText");
		valueText = characterSheetElement.GetComponent<Text>();

		valueText.text = "Armor:  " + CharacterArmor;

		characterSheetElement = GameObject.Find("BlockText");
		valueText = characterSheetElement.GetComponent<Text>();

		valueText.text = "Block:  " + CharacterBlockChance + "%";

		characterSheetElement = GameObject.Find("ParryText");
		valueText = characterSheetElement.GetComponent<Text>();

		valueText.text = "Parry:  " + CharacterParryChance + "%";

		characterSheetElement = GameObject.Find("DodgeText");
		valueText = characterSheetElement.GetComponent<Text>();

		valueText.text = "Dodge:  " + CharacterDodgeChance + "%";

		characterSheetElement = GameObject.Find("StrengthText");
		valueText = characterSheetElement.GetComponent<Text>();

		valueText.text = "Str: " + CharacterStrength;

		characterSheetElement = GameObject.Find("DexterityText");
		valueText = characterSheetElement.GetComponent<Text>();

		valueText.text = "Dex: " + CharacterDexterity;

		characterSheetElement = GameObject.Find("StaminaText");
		valueText = characterSheetElement.GetComponent<Text>();

		valueText.text = "Sta: " + CharacterStamina;

		characterSheetElement = GameObject.Find("IntelligenceText");
		valueText = characterSheetElement.GetComponent<Text>();

		valueText.text = "Int: " + CharacterIntelligence;
	}
}
