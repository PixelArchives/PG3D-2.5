using UnityEngine;

public class crossHair : MonoBehaviour
{
	public Texture2D crossHairTexture;

	private Rect crossHairPosition;

	private void Start()
	{
		crossHairPosition = new Rect((Screen.width - crossHairTexture.width * Screen.height / 640) / 2, (Screen.height - crossHairTexture.height * Screen.height / 640) / 2, crossHairTexture.width * Screen.height / 640, crossHairTexture.height * Screen.height / 640);
	}

	private void OnGUI()
	{
		GUI.DrawTexture(crossHairPosition, crossHairTexture);
	}
}
