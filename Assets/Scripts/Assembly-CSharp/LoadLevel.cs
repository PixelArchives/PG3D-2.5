using UnityEngine;

public class LoadLevel : MonoBehaviour
{
	public Texture fon;

	private void Start()
	{
		Application.LoadLevel("Level3");
	}

	private void OnGUI()
	{
		GUI.DrawTexture(new Rect(0f, 0f, 2048f * (float)Screen.height / 1154f, Screen.height), fon, ScaleMode.StretchToFill);
	}

	private void Update()
	{
	}
}
