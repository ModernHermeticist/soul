using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MovingObject
{
	private DungeonMaster dm;
	private PlayerLog eventLog;
	private CharacterSheetController characterSheetController;
	private InventoryController inventoryController;
	private GUIStatusController guiStatusController;
	private Equipment equipment;

	private List<GameObject> itemsInSameSpace;

	public bool updateFOV;
	public bool myTurn;
	public bool atWillFOVRefresh;
	bool showingCharacterSheet;
	bool showingInventory;
	bool showingItemDescription;
	bool showingLevelUpSheet;
	bool hasMoved;

	private GameObject playerCamera;
	private GameObject targetObject;
	private GameObject characterSheet;
	private GameObject inventory;
	private GameObject itemDescription;
	private GameObject levelUpSheet;
	private Vector3 oldPosition;
	private Vector3 moveVector;

	public LayerMask floorLayer;
	public LayerMask itemLayer;

	public int fovRange = 6;

	private bool dead;

	private bool levelUp;

	private int playerLevel;

	public string playerRace;
	public string playerClass;

	public int damageMin;
	public int damageMax;

	private int attributeModifiedDamageMin;
	private int attributeModifiedDamageMax;


	public int playerArmor;

	public int playerBlockChance;

	public int playerParryChance;

	public int playerDodgeChance;

	public int playerStrength;
	public int playerDexterity;
	public int playerStamina;
	public int playerIntelligence;

	private int oldPlayerStrength;
	private int oldPlayerDexterity;
	private int oldPlayerStamina;
	private int oldPlayerIntelligence;

	public int curHealth;
	public int curMana;
	public int curExperience;

	private int attributeModifiedMaxHealth;

	Vector3 direction;

	void init()
	{
		dead = false;
		levelUp = false;

		MinHealth = 0;
		MaxHealth = 100;
		CurHealth = 100;

		MinMana = 0;
		MaxMana = 10;
		CurMana = 10;

		MinExperience = 0;
		MaxExperience = 20;
		CurExperience = 0;


		PlayerName = "Anastasia";
		playerRace = "Human";
		playerClass = "Warrior";
		playerLevel = 1;

		playerStrength = 2;
		playerDexterity = 1;
		playerStamina = 1;
		playerIntelligence = 0;

		oldPlayerStrength = playerStrength;
		oldPlayerDexterity = playerDexterity;
		oldPlayerStamina = playerStamina;
		oldPlayerIntelligence = playerIntelligence;

		damageMin = 0;
		damageMax = 3;

		playerArmor = 0;
		playerBlockChance = 0;
		playerParryChance = 5;
		playerDodgeChance = 3;
		UpdateStatsByAttributeBonus();
	}

	void UpdateStatsByAttributeBonus()
	{
		attributeModifiedDamageMin = damageMin + (int)1.5f * playerStrength;
		attributeModifiedDamageMax = damageMax + (int)1.5f * playerStrength;
		attributeModifiedMaxHealth = MaxHealth + (int)2f * playerStamina;
	}

	void UpdateCharacterSheet()
	{
		characterSheetController.CharacterName = PlayerName;
		characterSheetController.CharacterRace = playerRace;
		characterSheetController.CharacterClass = playerClass;
		characterSheetController.CharacterLevel = playerLevel.ToString();
		characterSheetController.CharacterExperience = CurExperience.ToString();
		characterSheetController.CharacterExperienceMax = MaxExperience.ToString();
		characterSheetController.CharacterDamageMin = attributeModifiedDamageMin.ToString();
		characterSheetController.CharacterDamageMax = attributeModifiedDamageMax.ToString();
		characterSheetController.CharacterArmor = playerArmor.ToString();
		characterSheetController.CharacterBlockChance = playerBlockChance.ToString();
		characterSheetController.CharacterParryChance = playerParryChance.ToString();
		characterSheetController.CharacterDodgeChance = playerDodgeChance.ToString();
		characterSheetController.CharacterStrength = playerStrength.ToString();
		characterSheetController.CharacterDexterity = playerDexterity.ToString();
		characterSheetController.CharacterStamina = playerStamina.ToString();
		characterSheetController.CharacterIntelligence = playerIntelligence.ToString();
	}

	void UpdateGUIStatus()
	{
		guiStatusController.CurHealth = CurHealth.ToString();
		guiStatusController.MaxHealth = attributeModifiedMaxHealth.ToString();
		guiStatusController.CurMana = CurMana.ToString();
		guiStatusController.MaxMana = MaxMana.ToString();
		guiStatusController.CurExperience = CurExperience.ToString();
		guiStatusController.MaxExperience = MaxExperience.ToString();
	}

	public string PlayerName { get; set; }

	public int MinHealth { get; set; }
	public int MaxHealth { get; set; }
	public int CurHealth
	{
		get
		{
			return curHealth;
		}

		set
		{
			curHealth = Mathf.Clamp(value, MinHealth, MaxHealth);
		}
	}

	public int MinMana { get; set; }
	public int MaxMana { get; set; }
	public int CurMana
	{
		get
		{
			return curMana;
		}

		set
		{
			curMana = Mathf.Clamp(value, MinMana, MaxMana);
		}
	}

	public int MinExperience { get; set; }
	public int MaxExperience { get; set; }
	public int CurExperience
	{
		get
		{
			return curExperience;
		}

		set
		{
			curExperience = Mathf.Clamp(value, MinExperience, MaxExperience);
		}
	}

	protected override void Start ()
	{
		itemsInSameSpace = new List<GameObject>();
		dm = GameObject.Find("DungeonMaster").GetComponent<DungeonMaster>();
		playerCamera = GameObject.FindGameObjectWithTag("MainCamera");
		eventLog = GameObject.Find("ConsoleWindow").GetComponent<PlayerLog>();
		characterSheet = GameObject.Find("CharacterSheet");
		characterSheetController = characterSheet.GetComponent<CharacterSheetController>();
		guiStatusController = GameObject.Find("GUI_Status").GetComponent<GUIStatusController>();
		inventory = GameObject.Find("Inventory");
		inventoryController = inventory.GetComponent<InventoryController>();
		itemDescription = GameObject.Find("ItemDescription");
		equipment = GetComponent<Equipment>();
		levelUpSheet = GameObject.Find("LevelUpSheet");
		showingCharacterSheet = false;
		showingInventory = false;
		showingItemDescription = false;
		showingLevelUpSheet = false;
		init();
		UpdateCharacterSheet();
		UpdateGUIStatus();
		characterSheetController.refresh();
		guiStatusController.refresh();
		updateFOV = true;
		hasMoved = true;
		atWillFOVRefresh = true;
		base.Start();
	}
	

	void Update ()
	{
		if (dm.turn == DungeonMaster.turns.PLAYER)
		{
			hasMoved = false;
			updateFOV = false;
			moveVector = new Vector3(0, 0, 0);
			oldPosition = transform.position;
			int horizontal = 0;
			int vertical = 0;


			if (!showingInventory && !showingCharacterSheet && !showingLevelUpSheet && Input.GetKeyDown("w"))
			{
				vertical = 1;
				direction = Vector3.up;
			}
			else if (!showingInventory && !showingCharacterSheet && !showingLevelUpSheet && Input.GetKeyDown("s"))
			{
				vertical = -1;
				direction = Vector3.down;
			}
			else if (!showingInventory && !showingCharacterSheet && !showingLevelUpSheet && Input.GetKeyDown("a"))
			{
				horizontal = -1;
				direction = Vector3.left;
			}
			else if (!showingInventory && !showingCharacterSheet && !showingLevelUpSheet && Input.GetKeyDown("d"))
			{
				horizontal = 1;
				direction = Vector3.right;
			}
			else if (!showingInventory && !showingCharacterSheet && !showingLevelUpSheet && Input.GetKeyDown("e"))
			{
				horizontal = 1;
				vertical = 1;
				direction = Vector3.right + Vector3.up;
			}
			else if (!showingInventory && !showingCharacterSheet && !showingLevelUpSheet && Input.GetKeyDown("q"))
			{
				horizontal = -1;
				vertical = 1;
				direction = Vector3.left + Vector3.up;
			}
			else if (!showingInventory && !showingCharacterSheet && !showingLevelUpSheet && Input.GetKeyDown("z"))
			{
				horizontal = -1;
				vertical = -1;
				direction = Vector3.left + Vector3.down;
			}
			else if (!showingInventory && !showingCharacterSheet && !showingLevelUpSheet && Input.GetKeyDown("c"))
			{
				horizontal = 1;
				vertical = -1;
				direction = Vector3.right + Vector3.down;
			}
			else if (!showingCharacterSheet && !showingCharacterSheet && !showingLevelUpSheet && Input.GetKeyDown("l"))
			{
				showingCharacterSheet = true;
				characterSheet.transform.position = transform.position;
			}
			else if (!showingInventory && !showingCharacterSheet && !showingLevelUpSheet && Input.GetKeyDown("i"))
			{
				inventoryController.UpdateInventoryDisplay();
				showingInventory = true;
				inventory.transform.position = transform.position;
			}
			else if (Input.GetKeyDown("g"))
			{
				pickUpItem();
			}
			else if (showingInventory && Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
			{
				char c = parseKeyPressed();
				if (c != '\n') dropItem(c);
			}
			else if (showingInventory && Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
			{
				char c = parseKeyPressed();
				if (c != '\n') ShowItemDescription(c);
			}
			else if (showingInventory && parseKeyPressed() != '\n')
			{
				char c = parseKeyPressed();
				if (c != '\n')
				{
					EquipOrUnequipItem(c);
					UpdateCharacterSheet();
					characterSheetController.refresh();
				}
			}
			else if (Input.GetKeyDown(KeyCode.Escape))
			{
				if (showingCharacterSheet)
				{
					Vector3 shiftVector = new Vector3(0, -100, 0);
					showingCharacterSheet = false;
					characterSheet.transform.position = transform.position + shiftVector;
				}
				else if (showingInventory && !showingItemDescription)
				{
					Vector3 shiftVector = new Vector3(0, -100, 0);
					showingInventory = false;
					inventory.transform.position = transform.position + shiftVector;
				}
				else if (showingItemDescription)
				{
					Vector3 shiftVector = new Vector3(0, -100, 0);
					showingItemDescription = false;
					itemDescription.transform.position = transform.position + shiftVector;
				}
				else Application.Quit();
			}

			if (horizontal != 0 || vertical != 0)
			{
				AttemptMove(horizontal, vertical);
			}

			moveVector = transform.position;
			moveVector.z = -10;
			playerCamera.transform.position = moveVector;
			if (hasMoved || atWillFOVRefresh)
			{
				updateFOV = true;
				refreshFOV();
				atWillFOVRefresh = false;
				dm.turn = DungeonMaster.turns.ENEMY;
			}
		}
	}

	protected override bool AttemptMove(int xDir, int yDir)
	{
		hasMoved = true;
		if (base.AttemptMove(xDir, yDir))
		{
			checkWhatIsInSameSpace();
			return true;
		}
		return false;
	}

	protected override void OnCantMove(GameObject hitObject)
	{
		if (hitObject.tag == "Wall")
		{
			eventLog.AddEvent(PlayerName + " ran into a wall. It did not move.");
			return;
		}
		else if (hitObject.tag == "Enemy")
		{
			EnemyController hitEnemy = hitObject.GetComponent<EnemyController>();
			int damage = (int) Random.Range(attributeModifiedDamageMin, attributeModifiedDamageMax);
			hitEnemy.damageEnemy(damage);
			if (hitEnemy.Dead)
			{
				eventLog.AddEvent("<color=#FFD700>" + PlayerName + " killed a " + hitEnemy.name + " and gained " +
					hitEnemy.Experience + " points of experience!</color>");
				CurExperience += hitEnemy.Experience;
				if (CurExperience >= MaxExperience)
				{
					showingLevelUpSheet = true;
					playerLevel += 1;
					MaxExperience += playerLevel * 10;
					characterSheetController.CharacterLevel = playerLevel.ToString();
					StartCoroutine(LevelUp());
				}
				characterSheetController.CharacterExperience = CurExperience.ToString();
				guiStatusController.CurExperience = CurExperience.ToString();
				characterSheetController.refresh();
				guiStatusController.refresh();
			}
			return;
		}
	}


	public void refreshFOV()
	{
		BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();
		RaycastHit2D[] hitFloors;
		RaycastHit2D[] hitItems;
		RaycastHit2D hit;
		GameObject hitObject;
		Wall wall;
		Item item;
		EnemyController enemy;

		Vector2 start = transform.position;
		Vector2 end = start;

		boxCollider.enabled = false;
		for (int i = -fovRange; i < fovRange + 1; i++)
		{
			for (int j = -fovRange; j < fovRange + 1; j++)
			{
				end = start + new Vector2(i, j);
				hit = Physics2D.Linecast(start, end, blockingLayer);
				if (hit.transform)
				{
					end = hit.transform.position;
				}
				else end = start + new Vector2(i, j);
				hitFloors = Physics2D.LinecastAll(start, end, floorLayer);
				hitItems = Physics2D.LinecastAll(start, end, itemLayer);
				for (int k = 0; k < hitFloors.Length; k++)
				{
					hitObject = hitFloors[k].transform.gameObject;
					if (hitObject.tag == "Floor")
					{
						wall = hitObject.GetComponent<Wall>();
						wall.wasHit = true;
						wall.visible = true;
						wall.hasSeen = true;
					}
				}
				for (int k = 0; k < hitItems.Length; k++)
				{
					hitObject = hitItems[k].transform.gameObject;
					if (hitObject.tag == "Item")
					{
						item = hitObject.GetComponent<Item>();
						item.wasHit = true;
						item.visible = true;
					}
				}
				if (hit.transform)
				{
					hitObject = hit.transform.gameObject;
					if (hitObject.tag == "Wall")
					{
						wall = hitObject.GetComponent<Wall>();
						wall.wasHit = true;
						wall.visible = true;
						wall.hasSeen = true;
					}

					else if (hitObject.tag == "Enemy")
					{
						enemy = hitObject.GetComponent<EnemyController>();
						enemy.visible = true;
						enemy.wasHit = true;
					}
				}
			}
		}

		boxCollider.enabled = true;
	}

	public void damagePlayer(int loss, string enemyName)
	{
		int a = loss - playerArmor;
		if (a < 0) a = 0;
		CurHealth -= a;
		eventLog.AddEvent("<color=#ff0000ff>A " + enemyName + " hit " + PlayerName +
		" for " + a + " points of damage!</color>");
		if (CurHealth <= 0)
		{
			Destroy(gameObject);
			dead = true;
		}
		guiStatusController.CurHealth = CurHealth.ToString();
		guiStatusController.refresh();
	}

	public void checkWhatIsInSameSpace()
	{
		itemsInSameSpace.Clear();
		RaycastHit2D[] hitItems;

		Vector2 start = transform.position;
		Vector2 end = start;

		hitItems = Physics2D.LinecastAll(start, end, itemLayer);

		foreach(RaycastHit2D hit in hitItems)
		{
			itemsInSameSpace.Add(hit.transform.gameObject);
			eventLog.AddEvent("<color=#9A9A9A>You see a " + hit.transform.gameObject.name + " here.</color>");
		}
	}

	public void pickUpItem()
	{
		GameObject item;
		if (itemsInSameSpace.Count == 0)
			return;
		item = itemsInSameSpace[0];
		inventoryController.AddInventoryItem(itemsInSameSpace[0]);
		itemsInSameSpace.RemoveAt(0);
		item.SetActive(false);
		eventLog.AddEvent("You pick up a " + item.name);
		checkWhatIsInSameSpace();
	}

	public void dropItem(char c)
	{
		GameObject item;
		int index = ConvertAlphaToNum(c);
		if (index == -1) return;
		item = inventoryController.GetItemInInventory(index);
		if (item == null) return;
		inventoryController.RemoveInventoryItem(index);
		item.SetActive(true);
		item.transform.position = transform.position;
		inventoryController.UpdateInventoryDisplay();
		eventLog.AddEvent("You drop a " + item.name);
	}

	public void EquipOrUnequipItem(char c)
	{
		GameObject item;
		int index = ConvertAlphaToNum(c);
		if (index == -1) return;
		item = inventoryController.GetItemInInventory(index);
		if (item == null) return;
		int identifier = equipment.EquipOrUnequipItem(item.GetComponent<Item>());
		if (identifier == 0)
		{
			eventLog.AddEvent("You equip a " + item.name);
			item.name += " (e)";
			inventoryController.UpdateInventoryDisplay();
		}
		else if (identifier == 1)
		{
			eventLog.AddEvent("You unequip a " + item.name);
			item.name = item.name.Remove(item.name.IndexOf(" (e)"));
			inventoryController.UpdateInventoryDisplay();
		}
		UpdateStatsByAttributeBonus();
	}

	public char parseKeyPressed()
	{
		char c = '\n';
		if (Input.anyKeyDown)
		{
			//97 - 122
			string s = "";

			for (KeyCode kc = (KeyCode)97; kc <= (KeyCode)122; kc++)
			{
				if (Input.GetKeyDown(kc))
				{
					s = kc.ToString();
					c = s[0];
				}
			}
		}
		return c;
	}

	public int ConvertAlphaToNum(char c)
	{
		switch (c)
		{
			case 'A': return 0;
			case 'B': return 1;
			case 'C': return 2;
			case 'D': return 3;
			case 'E': return 4;
			case 'F': return 5;
			case 'G': return 6;
			case 'H': return 7;
			case 'I': return 8;
			case 'J': return 9;
			case 'K': return 10;
			case 'L': return 11;
			case 'M': return 12;
			case 'N': return 13;
			case 'O': return 14;
			case 'P': return 15;
			case 'Q': return 16;
			case 'R': return 17;
			case 'S': return 18;
			case 'T': return 19;
			case 'U': return 20;
			case 'V': return 21;
			case 'W': return 22;
			case 'X': return 23;
			case 'Y': return 24;
			case 'Z': return 25;
			default: return -1;
		}
	}

	public void ShowItemDescription(char c)
	{
		GameObject item;
		int index = ConvertAlphaToNum(c);
		if (index == -1) return;
		item = inventoryController.GetItemInInventory(index);
		if (item == null) return;
		showingItemDescription = true;
		itemDescription.transform.position = transform.position;
		item.GetComponent<Item>().ShowDescription();
	}

	public IEnumerator LevelUp()
	{
		Input.ResetInputAxes();
		float timeCount = 0;
		Vector3 shiftVector = new Vector3(0, -100, 0);
		levelUpSheet.transform.position = transform.position;
		GameObject progressionText = levelUpSheet.transform.GetChild(0).GetChild(0).GetChild(1).gameObject;
		GameObject strength = levelUpSheet.transform.GetChild(0).GetChild(0).GetChild(2).gameObject;
		GameObject dexterity = levelUpSheet.transform.GetChild(0).GetChild(0).GetChild(3).gameObject;
		GameObject stamina = levelUpSheet.transform.GetChild(0).GetChild(0).GetChild(4).gameObject;
		GameObject intelligence = levelUpSheet.transform.GetChild(0).GetChild(0).GetChild(5).gameObject;

		progressionText.GetComponent<Text>().text = "You have progressed to level " + playerLevel + "!";
		strength.GetComponent<Text>().text = "(a) Strength: " + playerStrength;
		dexterity.GetComponent<Text>().text = "(b) Dexterity: " + playerDexterity;
		stamina.GetComponent<Text>().text = "(c) Stamina: " + playerStamina;
		intelligence.GetComponent<Text>().text = "(d) Intelligence: " + playerIntelligence;
		while (timeCount < 2)
		{
			timeCount += Time.deltaTime;
			yield return null;
		}
		bool wait = true;
		while (wait)
		{
			if (Input.GetKeyDown(KeyCode.A))
			{
				playerStrength += 1;
				wait = false;
			}
			else if (Input.GetKeyDown(KeyCode.B))
			{
				playerDexterity += 1;
				wait = false;
			}
			else if (Input.GetKeyDown(KeyCode.C))
			{
				playerStamina += 1;
				wait = false;
			}
			else if (Input.GetKeyDown(KeyCode.D))
			{
				playerIntelligence += 1;
				wait = false;
			}
			yield return null;
		}
		UpdateStatsByAttributeBonus();
		UpdateCharacterSheet();
		characterSheetController.refresh();
		UpdateGUIStatus();
		guiStatusController.refresh();
		showingLevelUpSheet = false;
		levelUpSheet.transform.position = shiftVector;
	}
}



