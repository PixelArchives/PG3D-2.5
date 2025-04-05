using UnityEngine;

public class MainMenu : MonoBehaviour
{
	public Texture fon;

	public Texture plashkaPodScore;

	public GUIStyle playStyle;

	public GUIStyle soundStyle;

	public GUIStyle bestScoreStyle;

	public GUIStyle facebookStyle;

	public GUIStyle gamecenterStyle;

	public GUIStyle labelStyle;

	private bool showMessagFacebook;

	public static float iOSVersion
	{
		get
		{
			float result = -1f;
			string text = SystemInfo.operatingSystem.Replace("iPhone OS ", string.Empty);
			float.TryParse(text.Substring(0, 1), out result);
			return result;
		}
	}

	private void Start()
	{
		GlobalGameController.ResetParameters();
		GlobalGameController.Score = 0;
	}

	private void Update()
	{
		if (Application.platform == RuntimePlatform.Android && Input.GetKey(KeyCode.Escape))
		{
			Application.Quit();
		}
	}

	private void OnGUI()
	{
		GUI.DrawTexture(new Rect(((float)Screen.width - 2048f * (float)Screen.height / 1154f) / 2f, 0f, 2048f * (float)Screen.height / 1154f, Screen.height), fon, ScaleMode.StretchToFill);
		if (GUI.Button(new Rect((float)(Screen.width / 2) - (float)Screen.height * 0.238f, (float)Screen.height * 0.8475f - (float)Screen.height * 0.17f, (float)Screen.height * 0.476f, (float)Screen.height * 0.17f), string.Empty, playStyle))
		{
			PlayerPrefs.SetInt(Defs.CurrentWeaponSett, 0);
			Application.LoadLevel("LoadingNoWait");
		}
		bool @bool = PlayerPrefsX.GetBool(PlayerPrefsX.SndSetting, true);
		@bool = GUI.Toggle(new Rect((float)Screen.width * 0.05f, (float)Screen.height * 0.923f - (float)Screen.height * 0.0525f, (float)Screen.height * 0.105f, (float)Screen.height * 0.105f), @bool, string.Empty, soundStyle);
		AudioListener.volume = (@bool ? 1 : 0);
		PlayerPrefsX.SetBool(PlayerPrefsX.SndSetting, @bool);
		PlayerPrefs.Save();
		bestScoreStyle.fontSize = Mathf.RoundToInt((float)Screen.height * 0.04f);
		GUI.DrawTexture(new Rect((float)(Screen.width / 2) - (float)Screen.height * 0.317f, (float)Screen.height * 0.923f - (float)Screen.height * 0.0525f, (float)Screen.height * 0.63525f, (float)Screen.height * 0.105f), plashkaPodScore);
		GUI.Label(new Rect((float)(Screen.width / 2) - (float)Screen.height * 0.272f, (float)Screen.height * 0.923f - (float)Screen.height * 0.045f, (float)Screen.height * 0.5445f, (float)Screen.height * 0.09f), "BEST SCORE " + PlayerPrefs.GetInt(Defs.BestScoreSett, 0), bestScoreStyle);
		if (showMessagFacebook)
		{
			labelStyle.fontSize = Mathf.RoundToInt((float)Screen.height * 0.03f);
			GUI.Label(new Rect((float)(Screen.width / 2) - (float)Screen.height * 0.5f, (float)Screen.height * 0.5f - (float)Screen.height * 0.0525f, Screen.height, (float)Screen.height * 0.105f), "Your best score was sent to Facebook", labelStyle);
		}
	}

	private void hideMessag()
	{
		showMessagFacebook = false;
	}
}
