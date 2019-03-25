using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonMaster : MonoBehaviour
{
	[SerializeField]
	private Item[] items;
	[SerializeField]
	private EnemyController[] enemies;
	private GameObject player;

	public enum turns { PLAYER, ENEMY, ITEMS };
	public turns turn;

	// Use this for initialization
	void Start ()
	{
		player = GameObject.Find("Player");
		turn = turns.PLAYER;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (turn == turns.ENEMY)
		{
			foreach(EnemyController enemy in enemies)
			{
				if (enemy != null)
					enemy.doTurn();
			}
			foreach (Item item in items)
			{
				if (item != null)
				{
					item.doTurn();
				}
			}
			turn = turns.PLAYER;
		}
	}

	public void changeTurn()
	{
		if (turn == turns.PLAYER) turn = turns.ENEMY;
		else if (turn == turns.ENEMY) turn = turns.PLAYER;
	}

	public void updateFOV()
	{

	}
}
