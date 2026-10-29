using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class NewInput
{
	public static bool isMobile = true;
	public static event Action onInputChange;

	public static bool mouseLocked
	{
		get
		{
			return Cursor.lockState == CursorLockMode.Locked;
		}
		set
		{
			Cursor.lockState = value ? CursorLockMode.Locked : CursorLockMode.None;
		}
	}

	public static Vector2 mouseDelta
	{
		get
		{
			return new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
		}
	}

	public static Vector3 movement
	{
		get
		{
			return new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
		}
	}

	public static Vector2 movementAbs
	{
		get
		{
			return new Vector2(Mathf.Abs(Input.GetAxisRaw("Horizontal")), Mathf.Abs(Input.GetAxisRaw("Vertical")));
		}
	}

	public static bool pause
	{
		get
		{
#if UNITY_EDITOR || UNITY_WEBGL
			return Input.GetKeyDown(KeyCode.Tab); // esc acts weird in editor, doent lock mouse properly
#else
			return Input.GetKeyDown(KeyCode.Escape);
#endif
		}
	}

	public static bool Shooting(int curAmmo)
	{
		if (curAmmo > 0) 
			return Input.GetMouseButton(0); // to prevent empty animation and sound playing every frame

		else return Input.GetMouseButtonDown(0);
	}

	public static bool reload
	{
		get
		{
			return Input.GetKeyDown(KeyCode.R);
		}
	}

	public static bool jump
	{
		get
		{
			return Input.GetKey(KeyCode.Space);
		}
	}

	public static bool muteMusic
	{
		get
		{
			return Input.GetKeyDown(KeyCode.M);
		}
	}

	public static float volumeAdd
	{
		get
		{
			if (Input.GetKey(KeyCode.Minus)) return -1f * Time.unscaledDeltaTime;
			if (Input.GetKey(KeyCode.Equals)) return 1f * Time.unscaledDeltaTime;

			return 0f;
		}
	}

	public static void Start()
	{
		isMobile = Application.isMobilePlatform;
	}

	public static void Update()
	{
		bool old = isMobile;

		if (!isMobile)
		{
			if (Input.touchCount > 0) isMobile = true;
		}
		else
		{
			if (movement != Vector3.zero) isMobile = false;
		}

		if (old != isMobile) onInputChange.Invoke();
	}
}

public class NewUtils : MonoBehaviour
{
	public static float soundVolume = 1f;
	public static bool enableAudio;
	private static bool enableAudioPrev;

	private bool musicMuted;

	[RuntimeInitializeOnLoadMethod]
	private static void Init()
	{
		DontDestroyOnLoad(new GameObject("[NewUtils]", typeof(NewUtils)));
		NewInput.Start();
	}

	private void Start()
	{
		SceneManager.activeSceneChanged += SceneManager_activeSceneChanged;
	}

	private void SceneManager_activeSceneChanged(Scene arg0, Scene arg1)
	{
		CheckMusic();
	}

	private void Update()
	{
		NewInput.Update();

		if (enableAudio != enableAudioPrev)
		{
			AudioListener.volume = enableAudio ? soundVolume : 0f;
			enableAudioPrev = enableAudio;
			PlayerPrefsX.SetBool(PlayerPrefsX.SndSetting, enableAudio);
			PlayerPrefs.Save();
		}

		float vAdd = NewInput.volumeAdd;

		if (vAdd != 0f)
		{
			soundVolume = Mathf.Clamp(soundVolume + vAdd, 0f, 1f);
			AudioListener.volume = enableAudio ? soundVolume : 0f;
		}

		if (NewInput.muteMusic)
		{
			musicMuted = !musicMuted;
			CheckMusic();
		}
	}

	private void CheckMusic()
	{
		var obj = GameObject.Find("BackgroundMusic");
		if (!obj) return;

		var a = obj.GetComponent<AudioSource>();
		if (!a) return;

		a.volume = musicMuted ? 0f : 1f;
	}
}