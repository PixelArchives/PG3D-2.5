using System.Collections;
using UnityEngine;

public class SkinsManager : MonoBehaviour
{
	public Hashtable skins = new Hashtable();

	private void OnLevelWasLoaded(int idx)
	{
		Debug.Log("OnLevelWasLoaded");
		if (skins.Count > 0)
		{
			skins.Clear();
			Debug.Log("Clear");
		}
	}

	private void Start()
	{
		Object.DontDestroyOnLoad(base.gameObject);
	}
}
