using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatBar : MonoBehaviour
{
	private float curVal;
	private float oldVal;
	[SerializeField]
	private GameObject bar;
	[SerializeField]
	private Text valueText;

	private Vector3 newPos;

	public float Max { get; set; }

	public float Min { get; set; }

	public float CurVal
	{
		get
		{
			return curVal;
		}

		set
		{
			curVal = Mathf.Clamp(value, 0, Max);
		}
	}

	// Use this for initialization
	void Start ()
	{
		string[] tmp = valueText.text.Split(':');
		valueText.text = tmp[0] + ": " + CurVal + "/" + Max;
	}
	
	// Update is called once per frame
	void Update ()
	{
		string[] tmp = valueText.text.Split(':');
		valueText.text = tmp[0] + ": " + CurVal + "/" + Max;
	}
}
