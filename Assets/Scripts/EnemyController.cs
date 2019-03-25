using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MovingObject
{
	[SerializeField]
	private float health;

	private SpriteRenderer sprite;
	[SerializeField]
	private Sprite deathSprite;
	private PlayerController player;
	private DungeonMaster dm;
	private PlayerLog eventLog;

	[SerializeField]
	private int originalChaseTurns;
	private int chaseTurns;

	private Vector2 lastSeenLocation;

	private Color spriteColor;

	public bool myTurn;

	public bool visible;
	public bool wasHit;

	[SerializeField]
	private int experience;
	public int damageMin;
	public int damageMax;

	public int armor;

	public int Experience
	{
		get
		{
			return experience;
		}
		set
		{
			experience = value;
		}
	}

	public bool Dead { get; set; }

	// Use this for initialization
	protected override void Start ()
	{
		sprite = GetComponent<SpriteRenderer>();
		player = GameObject.Find("Player").GetComponent<PlayerController>();
		eventLog = GameObject.Find("ConsoleWindow").GetComponent<PlayerLog>();
		dm = GameObject.Find("DungeonMaster").GetComponent<DungeonMaster>();
		spriteColor = sprite.color;
		sprite.enabled = false;
		Dead = false;
		chaseTurns = originalChaseTurns;
		
		visible = false;
		wasHit = false;
		base.Start();
	}
	
	// Update is called once per frame
	void Update ()
	{
	}

	public void doTurn()
	{
		if (player.updateFOV)
			refreshFOV();
		if (visible && !Dead)
		{
			approachPlayer();
		}
		else if (!visible && !Dead && chaseTurns > 0)
		{
			approachLastSeenLocation();
		}
	}

	public void damageEnemy(float loss)
	{
		float a = loss - armor;
		if (a < 0) a = 0;
		health -= a;
		eventLog.AddEvent("<color=#ff0000ff>" + player.PlayerName + " hit a " + name +
		" for " + a + " points of damage!</color>");
		if (health <= 0)
		{
			gameObject.GetComponent<BoxCollider2D>().enabled = false;
			sprite.sprite = deathSprite;
			sprite.color = Color.red;
			Dead = true;
		}
	}

	public void refreshFOV()
	{
		if (!Dead && visible && wasHit)
		{
			chaseTurns = originalChaseTurns;
			sprite.enabled = true;
			wasHit = false;
		}
		else if (!Dead && visible && !wasHit)
		{
			sprite.enabled = false;
			visible = false;
		}
		if (Dead && Vector2.Distance(transform.position, player.gameObject.transform.position) <= player.fovRange)
		{
			RaycastHit2D hit = Physics2D.Linecast(transform.position, player.gameObject.transform.position, blockingLayer);
			if (hit.transform && hit.transform.gameObject.tag == "Player")
			{
				sprite.enabled = true;
				visible = true;
			}
			else
			{
				sprite.enabled = false;
				visible = false;
			}
		}
		else if (Dead)
		{
			sprite.enabled = false;
			visible = false;
		}
	}

	public void approachPlayer()
	{
		chaseTurns = originalChaseTurns;
		Vector2 owner = transform.position;
		Vector2 target = player.transform.position;
		Vector3 dir = new Vector3(0, 0, 0);
		lastSeenLocation = target;
		int xDir = 0;
		int yDir = 0;

		if (Mathf.Abs(target.x - owner.x) < float.Epsilon)
		{
			if (target.y > owner.y)
				yDir = 1;
			else yDir = -1;
		}
		else if (Mathf.Abs(target.y - owner.y) < float.Epsilon)
		{
			if (target.x > owner.x)
				xDir = 1;
			else xDir = -1;
		}
		else
		{
			if (target.x > owner.x)
				xDir = 1;
			else xDir = -1;
			if (target.y > owner.y)
				yDir = 1;
			else yDir = -1;
		}

		AttemptMove(xDir, yDir);
	}

	public void approachLastSeenLocation()
	{
		Vector2 owner = transform.position;
		Vector2 target = lastSeenLocation;
		int xDir = 0;
		int yDir = 0;

		if (Mathf.Abs(target.x - owner.x) < float.Epsilon)
		{
			if (target.y > owner.y)
				yDir = 1;
			else yDir = -1;
		}
		else if (Mathf.Abs(target.y - owner.y) < float.Epsilon)
		{
			if (target.x > owner.x)
				xDir = 1;
			else xDir = -1;
		}
		else
		{
			if (target.x > owner.x)
				xDir = 1;
			else xDir = -1;
			if (target.y > owner.y)
				yDir = 1;
			else yDir = -1;
		}

		AttemptMove(xDir, yDir);
		chaseTurns -= 1;
	}

	protected override bool AttemptMove(int xDir, int yDir)
	{
		if (base.AttemptMove(xDir, yDir))
		{
			return true;
		}
		return false;
	}

	protected override void OnCantMove(GameObject hitObject)
	{
		if (hitObject.tag == "Player")
		{
			PlayerController hitPlayer = hitObject.GetComponent<PlayerController>();
			int blockCheck = Random.Range(0, 100);
			int parryCheck = Random.Range(0, 100);
			int dodgeCheck = Random.Range(0, 100);
			if (blockCheck < hitPlayer.playerBlockChance)
			{
				eventLog.AddEvent("<color=#7FFF00>" + hitPlayer.PlayerName + " Blocked a " + name + "</color>");
				return;
			}
			if (parryCheck < hitPlayer.playerParryChance)
			{
				eventLog.AddEvent("<color=#7FFF00>" + hitPlayer.PlayerName + " Parried a " + name + "</color>");
				return;
			}
			if (dodgeCheck < hitPlayer.playerParryChance)
			{
				eventLog.AddEvent("<color=#7FFF00>" + hitPlayer.PlayerName + " Dodged a " + name + "</color>");
				return;
			}
			int damage = Random.Range(damageMin, damageMax);
			hitPlayer.damagePlayer(damage, name);
			return;
		}
	}
}
