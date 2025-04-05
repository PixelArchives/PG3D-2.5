using UnityEngine;

public class Switcher : MonoBehaviour
{
	public Texture fon;

	public Texture fonLevel1;

	public Texture fonLevel2;

	public Texture fonLevel3;

	public Texture fonLevel4;

	public Texture fonLevel5;

	public Texture fonLevel6;

	public bool NoWait;

	public bool isGameOver;

	private Texture fonToDraw;

	public GameObject skinsManagerPrefab;

	private void Start()
	{
		if (GlobalGameController.currentLevel >= GlobalGameController.NumOfLevels)
		{
			GlobalGameController.currentLevel = 0;
			GlobalGameController.AllLevelsCompleted++;
		}
		switch (GlobalGameController.currentLevel)
		{
		case -1:
			fonToDraw = fon;
			break;
		case 0:
			fonToDraw = fonLevel1;
			break;
		case 1:
			fonToDraw = fonLevel2;
			break;
		case 2:
			fonToDraw = fonLevel3;
			break;
		case 3:
			fonToDraw = fonLevel4;
			break;
		case 4:
			fonToDraw = fonLevel5;
			break;
		case 5:
			fonToDraw = fonLevel6;
			break;
		default:
			fonToDraw = fon;
			break;
		}
		if (NoWait)
		{
			LoadMenu();
		}
		else
		{
			Invoke("LoadMenu", 2.5f);
		}
		if (!GameObject.FindGameObjectWithTag("SkinsManager") && (bool)skinsManagerPrefab)
		{
			Object.Instantiate(skinsManagerPrefab, Vector3.zero, Quaternion.identity);
		}
	}

	private void Method()
	{
	}

	private void OnGUI()
	{
		Rect position = new Rect(((float)Screen.width - 2048f * (float)Screen.height / 1154f) / 2f, 0f, 2048f * (float)Screen.height / 1154f, Screen.height);
		GUI.DrawTexture(position, fonToDraw, ScaleMode.StretchToFill);
	}

	private void LoadMenu()
	{
		if (isGameOver)
		{
			Application.LoadLevel("Restart");
			return;
		}
		string text;
		switch (GlobalGameController.currentLevel)
		{
		case -1:
			text = "Restart";
			break;
		case 0:
			text = "Level1_cementery";
			break;
		case 1:
			text = "Level2_maze";
			break;
		case 2:
			text = "Level3_city";
			break;
		case 3:
			text = "level4";
			break;
		case 4:
			text = "level6";
			break;
		case 5:
			text = "level1_prison";
			break;
		default:
			text = "Restart";
			break;
		}
		GlobalGameController.currentLevel++;
		Application.LoadLevel(text);
	}
}
