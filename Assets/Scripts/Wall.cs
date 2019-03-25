using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
	public bool hasSeen;
	public bool visible;
	public bool wasHit;

	private bool refreshFOV;

	private Color visibleColor;
	private Color hasSeenColor;

	private SpriteRenderer sprite;

	[SerializeField]
	private Sprite visibleSprite;
	[SerializeField]
	private Sprite hasSeenSprite;

	private PlayerController player;
	// Use this for initialization
	void Start ()
	{
		wasHit = false;
		hasSeen = false;
		visible = false;
		refreshFOV = true;
		sprite = GetComponent<SpriteRenderer>();
		player = GameObject.Find("Player").GetComponent<PlayerController>();


		sprite.color = Color.black;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (player.updateFOV)
			updateFOV();
	}

	void updateFOV()
	{
		if (hasSeen)
			//sprite.color = hasSeenColor;
			sprite.sprite = hasSeenSprite;
		if (visible && wasHit)
		{
			sprite.color = Color.white;
			sprite.sprite = visibleSprite;
		}
		else if (visible && !wasHit)
		{
			//sprite.color = hasSeenColor;
			sprite.sprite = hasSeenSprite;
			visible = false;
		}
		wasHit = false;
	}
}
