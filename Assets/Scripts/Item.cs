using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "NewItem")]
public class Item : ScriptableObject
{
	public int id;
	public string itemName;
	public string dialogueWhenBought;
	public int cost;
	public Sprite icon;
	public Sprite clothing;
}
