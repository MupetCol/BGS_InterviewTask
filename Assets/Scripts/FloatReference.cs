using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableReferences/FloatReference", fileName = "FloatVar")]
public class FloatReference : ScriptableObject
{
	public float value;
	public float defaultValue;
	public bool reset;

	private void OnEnable()
	{
		hideFlags = HideFlags.DontUnloadUnusedAsset;
		if (reset)
			value = defaultValue;
	}

	public void ResetValues()
	{
		value = defaultValue;
	}
}
