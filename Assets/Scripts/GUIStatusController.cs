using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIStatusController : MonoBehaviour
{
	[SerializeField]
	private GameObject healthBar;
	[SerializeField]
	private GameObject manaBar;
	[SerializeField]
	private GameObject experienceBar;

	private Text valueText;

	public string CurHealth { get; set; }
	public string MaxHealth { get; set; }

	public string CurMana { get; set; }
	public string MaxMana { get; set; }

	public string CurExperience { get; set; }
	public string MaxExperience { get; set; }

	// Use this for initialization
	void Start ()
	{
	}

	public void refresh()
	{
		valueText = healthBar.GetComponent<Text>();
		valueText.text = "HP:  " + CurHealth + "/" + MaxHealth;

		valueText = manaBar.GetComponent<Text>();
		valueText.text = "MP:  " + CurMana + "/" + MaxMana;

		valueText = experienceBar.GetComponent<Text>();
		valueText.text = "XP:  " + CurExperience + "/" + MaxExperience;
	}
}
