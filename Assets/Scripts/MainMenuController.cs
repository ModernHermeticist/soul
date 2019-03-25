using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
	private GameObject controlsMenu;

	bool showingControlsMenu;

	
	void Start ()
	{
		showingControlsMenu = false;
		controlsMenu = GameObject.Find("ControlsMenu");
	}
	

	void Update ()
	{
		if (!showingControlsMenu && Input.GetKeyDown(KeyCode.B))
		{
			controlsMenu.transform.position = transform.position;
			showingControlsMenu = true;
		}

		else if (Input.GetKeyDown(KeyCode.A))
		{
			SceneManager.LoadScene("SampleScene");
		}

		else if (!showingControlsMenu && Input.GetKeyDown(KeyCode.C))
		{
			Application.Quit();
		}

		else if (showingControlsMenu && Input.GetKeyDown(KeyCode.Escape))
		{
			Vector3 shiftVector = new Vector3(0, -100, 0);
			controlsMenu.transform.position = shiftVector;
			showingControlsMenu = false;
		}


	}

}
