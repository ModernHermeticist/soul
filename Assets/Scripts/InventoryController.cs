using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryController : MonoBehaviour
{
	private GameObject inventoryList;
	private Text inventoryText;

	private List<GameObject> inventoryItems;

	// Use this for initialization
	void Start ()
	{
		inventoryList = GameObject.Find("InventoryItemList");
		inventoryText = inventoryList.GetComponent<Text>();
		inventoryItems = new List<GameObject>();
	}

	public GameObject GetItemInInventory(int index)
	{
		if (inventoryItems.Count < (index - 1)) return null;
		return inventoryItems[index];
	}

	public void AddInventoryItem(GameObject item)
	{
		inventoryItems.Add(item);
	}

	public void RemoveInventoryItem(int item)
	{
		inventoryItems.RemoveAt(item);
	}

	public void UpdateInventoryDisplay()
	{
		inventoryText.text = "";
		char c = 'a';
		foreach (GameObject item in inventoryItems)
		{
			inventoryText.text += "(" + c + ") " + item.name + "\n";
			c++;
		}
	}


}
