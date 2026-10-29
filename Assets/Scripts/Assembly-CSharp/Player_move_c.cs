using System.Collections;
using UnityEngine;

public class Player_move_c : MonoBehaviour
{
	public GUISkin MySkin;

	public Texture2D AimTexture;

	public int AimTextureWidth = 50;

	public int AimTextureHeight = 50;

	public Transform GunFlash;

	public Transform bullet;

	public int BulletForce = 5000;

	public int MaxAmmoCount = 10;

	public int CurAmmoCount = 10;

	public int InventoryAmmoCount = 120;

	private GameObject _leftJoystick;

	private GameObject _rightJoystick;

	private int _curHealth;

	public int MaxHealth;

	private int DifferenceAmmo;

	public int AmmoBoxWidth = 100;

	public int AmmoBoxHeight = 100;

	public int AmmoBoxOffset = 10;

	public int ScoreBoxWidth = 100;

	public int ScoreBoxHeight = 100;

	public int ScoreBoxOffset = 10;

	private float GunFlashLifetime;

	public GUIStyle ScoreBox;

	public GUIStyle AmmoBox;

	public GUIStyle HealthBox;

	public GUIStyle pauseStyle;

	public GUIStyle pauseFonStyle;

	public GUIStyle resumeStyle;

	public GUIStyle menuStyle;

	public GUIStyle soundStyle;

	public GUIStyle buyStyle;

	public GUIStyle enemiesLeftStyle;

	private GameObject _fpc;

	public Texture HeartTexture;

	public GameObject damage;

	private bool damageShown;

	public Texture pauseFon;

	private Pauser _pauser;

	public Texture pauseTitle;

	public GameObject[] weapons;

	public int CurrentWeaponIndex;

	private GameObject _gameController;

	private WeaponSounds _currentWeaponSounds;

	private bool _backWasPressed;

	private ZombieCreator _zombieCreator;

	public int CurHealth
	{
		get
		{
			return _curHealth;
		}
		set
		{
			_curHealth = value;
		}
	}

	public int curHealthProp
	{
		get
		{
			return CurHealth;
		}
		set
		{
			if (CurHealth > value && !damageShown)
			{
				StartCoroutine(FlashWhenHit());
			}
			CurHealth = value;
		}
	}

	private void WalkAnimation()
	{
		weapons[CurrentWeaponIndex].GetComponent<Animation>().CrossFade("Walk");
	}

	private void IdleAnimation()
	{
		weapons[CurrentWeaponIndex].GetComponent<Animation>().CrossFade("Idle");
	}

	public void ChangeWeapon(int newWeaponIndex, bool shouldSetMaxAmmo = true)
	{
		weapons[CurrentWeaponIndex].active = false;
		CurrentWeaponIndex = newWeaponIndex;
		weapons[CurrentWeaponIndex].active = true;
		_rightJoystick.SendMessage("setSeriya", weapons[CurrentWeaponIndex].GetComponent<WeaponSounds>().isSerialShooting);
		MaxAmmoCount = WeaponsManager.ammoInClipForWeapon(CurrentWeaponIndex);
		if (shouldSetMaxAmmo)
		{
			InventoryAmmoCount = WeaponsManager.maxAmmoInClipForWeapon(CurrentWeaponIndex);
		}
		else
		{
			InventoryAmmoCount = WeaponsManager.InitialAmmoInClipForWeapon(CurrentWeaponIndex);
		}
		_rightJoystick.SendMessage("HasAmmo");
		CurAmmoCount = MaxAmmoCount;
		weapons[CurrentWeaponIndex].GetComponent<Animation>()["Reload"].layer = 1;
		weapons[CurrentWeaponIndex].GetComponent<Animation>()["Shoot"].layer = 1;
		_currentWeaponSounds = weapons[CurrentWeaponIndex].GetComponent<WeaponSounds>();
		PlayerPrefs.SetInt(Defs.CurrentWeaponSett, CurrentWeaponIndex);
	}

	public bool NeedAmmo()
	{
		return InventoryAmmoCount < WeaponsManager.maxAmmoInClipForWeapon(CurrentWeaponIndex);
	}

	private void OnEnable()
	{
	}

	private void OnDisable()
	{
	}

	private void Start()
	{
        NewInput.mouseLocked = true;

		weapons[CurrentWeaponIndex].GetComponent<Animation>()["Reload"].layer = 1;
		weapons[CurrentWeaponIndex].GetComponent<Animation>().Stop();
		CurAmmoCount = MaxAmmoCount;
		GunFlash.gameObject.active = false;
		CurHealth = MaxHealth;
		_fpc = GameObject.FindGameObjectWithTag("FirstPersonControl");
		Color color = damage.GetComponent<GUITexture>().color;
		color.a = 0f;
		damage.GetComponent<GUITexture>().color = color;
		_gameController = GameObject.FindGameObjectWithTag("GameController");
		_zombieCreator = _gameController.GetComponent<ZombieCreator>();
		_pauser = GameObject.FindGameObjectWithTag("GameController").GetComponent<Pauser>();
		_leftJoystick = GameObject.Find("LeftTouchPad");
		_rightJoystick = GameObject.Find("RightTouchPad");
		ChangeWeapon(PlayerPrefs.GetInt(Defs.CurrentWeaponSett, 0), false);
	}

	private void ReloadPressed()
	{
		DifferenceAmmo = MaxAmmoCount - CurAmmoCount;
		if (InventoryAmmoCount > 0 && CurAmmoCount != MaxAmmoCount)
		{
			weapons[CurrentWeaponIndex].GetComponent<Animation>().Stop("Empty");
			weapons[CurrentWeaponIndex].GetComponent<Animation>().CrossFade("Shoot");
			weapons[CurrentWeaponIndex].GetComponent<Animation>().Play("Reload");
			if (InventoryAmmoCount >= DifferenceAmmo)
			{
				CurAmmoCount += DifferenceAmmo;
				InventoryAmmoCount -= DifferenceAmmo;
			}
			else
			{
				CurAmmoCount += InventoryAmmoCount;
				InventoryAmmoCount = 0;
			}
			base.GetComponent<AudioSource>().PlayOneShot(_currentWeaponSounds.reload);
			_rightJoystick.SendMessage("HasAmmo");
		}
	}

	private void ShotPressed()
	{
		if (weapons[CurrentWeaponIndex].GetComponent<Animation>().IsPlaying("Shoot") || weapons[CurrentWeaponIndex].GetComponent<Animation>().IsPlaying("Reload"))
		{
			return;
		}
		weapons[CurrentWeaponIndex].GetComponent<Animation>().Stop();
		if (CurAmmoCount > 0)
		{
			weapons[CurrentWeaponIndex].GetComponent<Animation>().Play("Shoot");
			_fpc.SendMessage("shootS");
			CurAmmoCount--;
			if (CurAmmoCount == 0)
			{
				_rightJoystick.SendMessage("NoAmmo");
			}
			base.GetComponent<AudioSource>().PlayOneShot(_currentWeaponSounds.shoot);
            GunFlash.gameObject.active = true;
			GunFlashLifetime = 0.1f;
		}
		else
		{
			weapons[CurrentWeaponIndex].GetComponent<Animation>().Play("Empty");
			base.GetComponent<AudioSource>().PlayOneShot(_currentWeaponSounds.empty);
		}
	}

	private IEnumerator Fade(float start, float end, float length, GameObject currentObject)
	{
		for (float i = 0f; i < 1f; i += Time.deltaTime / length)
		{
			Color rgba = currentObject.GetComponent<GUITexture>().color;
			rgba.a = Mathf.Lerp(start, end, i);
			currentObject.GetComponent<GUITexture>().color = rgba;
			yield return 0;
			Color rgba_ = currentObject.GetComponent<GUITexture>().color;
			rgba_.a = end;
			currentObject.GetComponent<GUITexture>().color = rgba_;
		}
	}

	private IEnumerator FlashWhenHit()
	{
		damageShown = true;
		Color rgba = damage.GetComponent<GUITexture>().color;
		rgba.a = 0f;
		damage.GetComponent<GUITexture>().color = rgba;
		float danageTime = 0.15f;
		yield return StartCoroutine(Fade(0f, 1f, danageTime, damage));
		yield return new WaitForSeconds(0.01f);
		yield return StartCoroutine(Fade(1f, 0f, danageTime, damage));
		damageShown = false;
	}

	private void Update()
	{
        if (NewInput.pause) SetPause();

        if (NewInput.mouseLocked)
        {
            if (NewInput.reload) ReloadPressed();
            if (NewInput.Shooting(CurAmmoCount)) ShotPressed();
        }

		if (GunFlashLifetime > 0f)
		{
			GunFlashLifetime -= Time.deltaTime;
		}
		if (GunFlashLifetime <= 0f)
		{
            GunFlash.gameObject.active = false;
		}
		if (CurHealth <= 0)
		{
			if (GlobalGameController.Score > PlayerPrefs.GetInt(Defs.BestScoreSett, 0))
			{
				PlayerPrefs.SetInt(Defs.BestScoreSett, GlobalGameController.Score);
				PlayerPrefs.Save();
			}
			Application.LoadLevel("GameOver");
		}
	}

	private void OnGUI()
	{
		GUI.skin = MySkin;
		GUI.DrawTexture(new Rect((float)(Screen.width / 2) - (float)Screen.height * 0.023f, (float)(Screen.height / 2) - (float)Screen.height * 0.023f, (float)Screen.height * 0.046f, (float)Screen.height * 0.046f), AimTexture);
		Rect position = new Rect((float)Screen.width - (float)Screen.width * 0.208f, 0f, (float)Screen.width * 0.208f, (float)Screen.height * 0.078f);
		AmmoBox.fontSize = Mathf.RoundToInt((float)Screen.height * 0.035f);
		GUI.Box(position, CurAmmoCount + "/" + InventoryAmmoCount, AmmoBox);
		Rect position2 = new Rect(0f, 0f, (float)Screen.width * 0.271f, (float)Screen.height * 0.078f);
		ScoreBox.fontSize = Mathf.RoundToInt((float)Screen.height * 0.035f);
		GUI.Box(position2, "SCORE " + GlobalGameController.Score, ScoreBox);
		bool flag = true;
		float num = (float)Screen.width * 0.271f;
		float width = (float)Screen.width * 0.521f;
		float num2 = (float)Screen.height * 0.078f;
		GUI.Box(new Rect(num, 0f, width, num2), string.Empty, HealthBox);
		float num3 = 45.4f - (float)HeartTexture.width;
		float top = (float)Screen.height * 0.068f - (float)Screen.width * 0.521f * 0.07f;
		for (int i = 0; i < CurHealth; i++)
		{
			GUI.DrawTexture(new Rect(num + (float)Screen.width * 0.521f * 0.015f + (float)i * ((float)Screen.width * 0.521f * 0.09f), top, (float)Screen.width * 0.521f * 0.07f, (float)Screen.width * 0.521f * 0.07f), HeartTexture);
		}
		bool flag2 = false;
		if (Application.platform == RuntimePlatform.Android)
		{
			if (Input.GetKey(KeyCode.Escape))
			{
				_backWasPressed = true;
				Debug.Log("407 androidBackPressed " + flag2);
			}
			else
			{
				if (_backWasPressed)
				{
					flag2 = true;
				}
				_backWasPressed = false;
			}
		}
		if (NewInput.isMobile && (GUI.Button(new Rect(0f, (float)Screen.height * 0.078f, (float)Screen.height * 0.078f, (float)Screen.height * 0.078f), string.Empty, pauseStyle) || (flag2 && !_pauser.paused)) && !_pauser.paused)
		{
			flag2 = false;
			if (CurHealth > 0)
			{
				SetPause();
			}
		}
		int num4 = GlobalGameController.EnemiesToKill - _zombieCreator.NumOfDeadZombies;
		string text = ((num4 <= 0) ? "Enter the Portal..." : ("Enemies left: " + num4));
		enemiesLeftStyle.fontSize = Mathf.RoundToInt((float)Screen.height * 0.035f);
		GUI.Box(new Rect(num, num2, width, num2), text, enemiesLeftStyle);
		if ((bool)_pauser && _pauser.paused)
		{
			Rect position3 = new Rect(((float)Screen.width - 2048f * (float)Screen.height / 1154f) / 2f, 0f, 2048f * (float)Screen.height / 1154f, Screen.height);
			GUI.DrawTexture(position3, pauseFon, ScaleMode.StretchToFill);
			float num5 = 15f;
			Vector2 vector = new Vector2(176f, 150f - num5);
			GUI.DrawTexture(new Rect((float)Screen.width * 0.57f, (float)Screen.height * 0.2f, (float)Screen.height * 0.4781f, (float)Screen.height * 0.1343f), pauseTitle);
			if (GUI.Button(new Rect((float)Screen.width * 0.57f + (float)Screen.height * 0.129f, (float)Screen.height * 0.38f, (float)Screen.height * 0.22f, (float)Screen.height * 0.078f), string.Empty, resumeStyle) || flag2)
			{
				SetPause();
			}
			if (GUI.Button(new Rect((float)Screen.width * 0.57f + (float)Screen.height * 0.129f, (float)Screen.height * 0.5f, (float)Screen.height * 0.22f, (float)Screen.height * 0.078f), string.Empty, menuStyle))
			{
				Time.timeScale = 1f;
				Application.LoadLevel("Restart");
			}
			float num6 = 15f;
			bool @bool = PlayerPrefsX.GetBool(PlayerPrefsX.SndSetting, true);
			@bool = GUI.Toggle(new Rect((float)Screen.width * 0.05f, (float)Screen.height * 0.923f - (float)Screen.height * 0.0525f, (float)Screen.height * 0.105f, (float)Screen.height * 0.105f), @bool, string.Empty, soundStyle);
			NewUtils.enableAudio = @bool;
		}
	}

	private void SetPause()
	{
		_pauser.paused = !_pauser.paused;
		if (_pauser.paused)
		{
			Time.timeScale = 0f;
		}
		else
		{
			Time.timeScale = 1f;
		}

		NewInput.mouseLocked = !_pauser.paused;
	}

	void OnDestroy()
	{
        NewInput.mouseLocked = false;
	}
}
