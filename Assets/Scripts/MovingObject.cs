using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovingObject : MonoBehaviour
{
	public float moveTime = 0.001f;
	public LayerMask blockingLayer;

	private BoxCollider2D boxCollider;
	private Rigidbody2D rb2D;
	private float inverseMoveTime;

	// Use this for initialization
	protected virtual void Start()
	{
		boxCollider = GetComponent<BoxCollider2D>();
		rb2D = GetComponent<Rigidbody2D>();
	}

	protected bool Move (int xDir, int yDir, out RaycastHit2D hit)
	{
		Vector2 start = transform.position;
		Vector2 end = start + new Vector2(xDir, yDir);
		Vector3 move = new Vector2(xDir, yDir);

		boxCollider.enabled = false;
		hit = Physics2D.Linecast(start, end, blockingLayer);
		boxCollider.enabled = true;

		if (hit.transform == null)
		{
			transform.position += move;
			return true;
		}
		else if (hit.transform.gameObject.GetComponent<BoxCollider2D>().isTrigger)
		{
			transform.position += move;
			return true;
		}

		return false;
	}

	protected virtual bool AttemptMove(int xDir, int yDir)
	{
		RaycastHit2D hit;
		bool canMove = Move(xDir, yDir, out hit);

		if (hit.transform == null)
		{
			return true;
		}
		else if (hit.transform.gameObject.GetComponent<BoxCollider2D>().isTrigger)
		{
			return true;
		}

		GameObject hitObject = hit.transform.gameObject;
		if (!canMove && hitObject != null)
			OnCantMove(hitObject);
		return false;
	}

	protected abstract void OnCantMove(GameObject hitObject);

}
