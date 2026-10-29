using System.Collections;
using UnityEngine;

public class AutoFade : MonoBehaviour
{
	private static AutoFade m_Instance;

	private Material m_Material;

	private string m_LevelName = string.Empty;

	private int m_LevelIndex;

	private bool m_Fading;

	private static AutoFade Instance
	{
		get
		{
			if (m_Instance == null)
			{
				m_Instance = new GameObject("AutoFade").AddComponent<AutoFade>();
			}
			return m_Instance;
		}
	}

	public static bool Fading
	{
		get
		{
			return Instance.m_Fading;
		}
	}

	private void Awake()
	{
		Object.DontDestroyOnLoad(this);
		m_Instance = this;
		m_Material = Resources.Load<Material>("AutoFade");
	}

	private void DrawQuad(Color aColor, float aAlpha)
	{
		aColor.a = aAlpha;
		m_Material.SetPass(0);
		GL.Color(aColor);
        GL.PushMatrix();
        GL.LoadOrtho();
        GL.Begin(GL.QUADS);
        GL.Color(aColor);
        GL.Vertex3(0f, 0f, 0f); // Adjust z to be closer to 0
        GL.Vertex3(0f, 1f, 0f);
        GL.Vertex3(1f, 1f, 0f);
        GL.Vertex3(1f, 0f, 0f);
        GL.End();
        GL.PopMatrix();
    }

	private IEnumerator Fade(float aFadeOutTime, float aFadeInTime, Color aColor)
	{
		float t = 0f;
		while (t < 1f)
		{
			yield return new WaitForEndOfFrame();
			t = Mathf.Clamp01(t + Time.deltaTime / aFadeOutTime);
			DrawQuad(aColor, t);
		}
		if (m_LevelName != string.Empty)
		{
			Application.LoadLevel(m_LevelName);
		}
		else
		{
			Application.LoadLevel(m_LevelIndex);
		}
		while (t > 0f)
		{
			yield return new WaitForEndOfFrame();
			t = Mathf.Clamp01(t - Time.deltaTime / aFadeInTime);
			DrawQuad(aColor, t);
		}
		m_Fading = false;
	}

	private void StartFade(float aFadeOutTime, float aFadeInTime, Color aColor)
	{
		m_Fading = true;
		StartCoroutine(Fade(aFadeOutTime, aFadeInTime, aColor));
	}

	public static void LoadLevel(string aLevelName, float aFadeOutTime, float aFadeInTime, Color aColor)
	{
		if (!Fading)
		{
			Instance.m_LevelName = aLevelName;
			Instance.StartFade(aFadeOutTime, aFadeInTime, aColor);
		}
	}

	public static void LoadLevel(int aLevelIndex, float aFadeOutTime, float aFadeInTime, Color aColor)
	{
		if (!Fading)
		{
			Instance.m_LevelName = string.Empty;
			Instance.m_LevelIndex = aLevelIndex;
			Instance.StartFade(aFadeOutTime, aFadeInTime, aColor);
		}
	}
}
