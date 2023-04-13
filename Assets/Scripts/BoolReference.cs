using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableReferences/BoolReference", fileName = "BoolVar")]
public class BoolReference : ScriptableObject
{
	public bool toggle;
	public bool reset;

	[Header("0 = false, 1 = true")]
	public int defValue = 0;

	private void OnEnable()
	{
		hideFlags = HideFlags.DontUnloadUnusedAsset;
		if (reset && defValue == 0)
		{
			toggle = false;
		}
		else if (reset && defValue == 1)
		{
			toggle = true;
		}
	}

	public void Toggle()
	{
		toggle = !toggle;
	}
}
