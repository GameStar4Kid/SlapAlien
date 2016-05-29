using UnityEngine;
using System.Collections;
using UnityEngine.UI;

using UnityEngine.SocialPlatforms;

public class Menu : MonoBehaviour {
	private const bool DEBUG = false;
	public RawImage bg;
	public Animator curtain;

	public GameObject helpObj;
	public GameObject mainMenu;
	public GameObject options;
	public GameObject ownAdb;
	public Image[] objRecolor;
	public static bool isLogged =false;
	private static int bannerId1 = 0;
	void Awake()
	{
		GameData.Get ();
		Color c = Config.bgColors [Random.Range (0, Config.bgColors.Length)];
		//bg.color = c;

		for(int i = 0; i < objRecolor.Length; i++)
		{
			objRecolor[i].color = c;
		}
	}

	void Start()
	{		
		UM_GameServiceManager.instance.Connect();
		UM_AdManager.instance.Init();
		if (DEBUG == true) {
			UM_ExampleStatusBar.text = "Connecting To Game Service";
		}
		
		UM_GameServiceManager.OnPlayerConnected += OnPlayerConnected;
		UM_GameServiceManager.OnPlayerDisconnected += OnPlayerDisconnected;
		if (bannerId1 != 0) {
			if(UM_AdManager.instance.IsBannerLoaded(bannerId1) && !UM_AdManager.instance.IsBannerOnScreen(bannerId1)) {
				UM_AdManager.instance.ShowBanner (bannerId1);
			}
		} else {
			bannerId1=UM_AdManager.instance.CreateAdBanner(TextAnchor.UpperCenter);
		}

		
		if(UM_GameServiceManager.instance.ConnectionSate == UM_ConnectionState.CONNECTED) {
			OnPlayerConnected();
		}
		if(curtain != null && GoTo.lastScene == "")
			curtain.Play("CurtainAnim");



		Social.localUser.Authenticate((bool success) => {
			isLogged = success;
		});
	}


	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape) && mainMenu.activeSelf)
		{
			if(ownAdb.activeSelf)
				OnExitClick();
			else
				ownAdb.SetActive(true);
		}
	}

	public void OnMoreGamesClick()
	{
		#if UNITY_IPHONE
		Application.OpenURL ("https://itunes.apple.com/us/developer/nguyen-tran/id647021292");
		#else
		Application.OpenURL ("https://play.google.com/store/apps/developer?id=Game+Star+Co.+Ltd");
		#endif

	}

	public void OnPlayClick()
	{
		AppSoundManager.Get ().PlaySfx (Sfx.Type.sfx_btn_click);
		UM_AdManager.instance.HideBanner (bannerId1);
		GoTo.LoadGame ();
	}

	public void OnHighScoreClick()
	{
		AppSoundManager.Get ().PlaySfx (Sfx.Type.sfx_btn_click);
		#if UNITY_IPHONE || UNITY_ANDROID
		UM_GameServiceManager.instance.ShowLeaderBoardsUI();
		#else

		#endif
	}

	public void OnHelpClick()
	{
		AppSoundManager.Get ().PlaySfx (Sfx.Type.sfx_btn_click);
		mainMenu.SetActive (false);
		options.SetActive (false);
		helpObj.SetActive (true);
		//GoTo.LoadHelp ();
	}

	public void OnRateClick()
	{
		AppSoundManager.Get ().PlaySfx (Sfx.Type.sfx_btn_click);
		#if UNITY_IPHONE
		Application.OpenURL ("https://itunes.apple.com/us/app/alien-the-hardest-game/id1049621532?ls=1&mt=8");
		#else
		Application.OpenURL ("https://play.google.com/store/apps/developer?id=Game+Star+Co.+Ltd");
		#endif
	}

	public void OnMainMenuClick()
	{
		AppSoundManager.Get ().PlaySfx (Sfx.Type.sfx_btn_click);
		mainMenu.SetActive (true);
		options.SetActive (false);
		helpObj.SetActive (false);
	}

	public void OnOptionsClick()
	{
		options.GetComponent<Options> ().Initialize ();
		AppSoundManager.Get ().PlaySfx (Sfx.Type.sfx_btn_click);
		mainMenu.SetActive (false);
		options.SetActive (true);
		helpObj.SetActive (false);
	}

	public void OnExitClick()
	{
		AppSoundManager.Get ().PlaySfx (Sfx.Type.sfx_btn_click);
		Application.Quit ();
		Debug.Log("exit");
	}

	public void GoToOwnAdb()
	{

	}
	private void OnPlayerConnected() {
		if (DEBUG == true) {
			UM_ExampleStatusBar.text = "Player Connected";
		}
	}
	
	
	private void OnPlayerDisconnected() {
		if (DEBUG == true) {
			UM_ExampleStatusBar.text = "Player Disconnected";
		}
	}
}
