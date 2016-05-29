using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SocialPlatforms;
using System;
using UnionAssets.FLE;
public class Game : MonoBehaviour {

	//public int seed;
	public GridLayoutGroup table;
	public PoolManager objPool;

	public Text finalScoresView;
	public Text bestScoreView;
	public Text currentScoresView;
	public Text currentLvlView;
	public Text timeBreakerView;

	public GameObject startGamePopup;
	public GameObject preGameView;
	public GameObject gameOverViewTime;
	public GameObject gameOverViewClick;

	public GameObject streakPrefab;
	public GameObject flyingPointsPrefab;
	public GameObject flashPrefab;

	public RawImage bg;
	public RawImage bgPreGame;
	public RawImage bgPopup;
	public RawImage bgBetweenLvl;

	public Image timerImg;

	public Animator timeBreakerAnimator;
	public Animator betweenLvlvAnimator;

	public Image[] objRecolor;

	private int currentScores = 0;
	private int currentLvl = 0;
	private int currentTimeBreakers = 0;
	private int itemsForFind;
	private int foundItems = 0;
	private int bgID = 0;
	private int dirIndx = 0;
	
	private List<int> tmpSctreaks = new List<int>();
	private int streak = 1;
	private int preItemID = -1;
	private int streakInGame = 0;

	private Vector2 tableSize;

	private float time;

	private List<GameObject> items;

	private bool mStartedGameplay = false;

	private GameData data;

	private string leaderBoardId =  "bestScore";
	
	private Vector3[] directionsForPoints = {new Vector3 (0.5f, 0.5f, 0f), new Vector3 (-0.5f, 0.5f, 0f)};

	void Start () 
	{
		data = GameData.Get ();
		items = new List<GameObject>();
		//Random.seed = seed;
		objPool.initializePool (data.spritesId);
		showTimeBreakers ();
		onPlayCLick (false);
		setBgColor ();
	}

	void showTimeBreakers(bool showAnim = false)
	{
		if(currentTimeBreakers > 0)
		{
			timeBreakerView.text ="x " + currentTimeBreakers.ToString ();
			Color c  = timerImg.color;
			c.a = 1f;
			timerImg.color = c;
			if(showAnim)
				timeBreakerAnimator.Play("gotTimeBreakerPoint",0,0f);
		}
		else
		{
			timeBreakerView.text ="";
			Color c  = timerImg.color;
			c.a = 0.5f;
			timerImg.color = c;
		}
	}

	public void onPlayCLick(bool isTick = true)
	{
		if(isTick)
			AppSoundManager.Get().PlaySfx(Sfx.Type.sfx_btn_click);
		AppSoundManager.Get ().StopMusic ();

		startGamePopup.SetActive (false);
		preGameView.SetActive (true);
		GetComponent<Animator> ().Play ("PreGameAnim",0,0f);
	}

	public void runGame()
	{
		preGameView.SetActive (false);
		AppSoundManager.Get ().PlayMusic (Music.Type.game);
		//Random.seed = seed;
		currentLvl = 0;
		currentScores = 0;
		currentTimeBreakers = 0;
		addPoints (0);
		restartGame ();
	}

	void fillTable ()
	{
		Level lvl = Config.GetLvlInfo (currentLvl);

		itemsForFind = UnityEngine.Random.Range (Config.minFindItems, Config.maxFindItems + 1);
		int spyCount = 0;
		int max  = Mathf.Min(lvl.maxBadGuy,itemsForFind - 1);
		spyCount = UnityEngine.Random.Range(lvl.minBadGuy,max+1);
		tableSize.x = lvl.sizeByWidth;
		tableSize.y = UnityEngine.Random.Range(lvl.sizeByWidth,lvl.sizeByWidth + 2);
		table.constraintCount =(int) tableSize.x;

		if(spyCount+itemsForFind > tableSize.x * tableSize.y)
			spyCount = ((int)(tableSize.x * tableSize.y) - itemsForFind);

		int findItm = itemsForFind;

		time = Mathf.Max (Config.endTime, Config.startTime - (Config.stepTime * currentLvl));

		int size =(int) (tableSize.x * tableSize.y);

		for(int i = 0; i < size; i++ )
		{
			GameObject item = objPool.pull();
	
			Item itm = item.GetComponent<Item>();
			itm.gm = this;
			if(findItm > 0)
			{
				itm.type = Item.Type.good;
				findItm -= 1;
			}
			else if(spyCount > 0)
			{
				itm.type = Item.Type.bad_spy;
				spyCount -=1;
			}
			else
				itm.type = Item.Type.bad;
			itm.setView(time);
			items.Add(item);
		}

		List<GameObject> itemsPlacement = new List<GameObject> ();
		for(int k = 0; k < items.Count; k++ )
			itemsPlacement.Add(items[k]);

		int count = itemsPlacement.Count;
		for(int j = 0; j < count; j++)
		{
			GameObject obj = itemsPlacement[UnityEngine.Random.Range(0, itemsPlacement.Count)];
			obj.transform.SetParent(table.transform);
			obj.transform.localScale = new Vector3(1f,1f,1f);
			itemsPlacement.Remove(obj);
		}
		adjustTableSize ();
	}

	void addPoints(int points)
	{
		int pre = 0;
		int after = 0;
		pre = currentScores / Config.pointsForTimer;

		currentScores += points;

		after = currentScores / Config.pointsForTimer;
		if(pre != after)
		{
			AppSoundManager.Get().PlaySfx(Sfx.Type.sfx_refresh_add);
			currentTimeBreakers++;
			showTimeBreakers(true);
		}
		currentScoresView.text = currentScores.ToString ();

		if(points > 0)
		{
			GameObject obj = Instantiate (flyingPointsPrefab, currentScoresView.gameObject.GetComponent<RectTransform> ().position, Quaternion.identity) as GameObject;
			obj.transform.SetParent(transform);
			obj.GetComponent<RectTransform> ().localScale = new Vector3 (1f, 1f, 1f);
			obj.GetComponent<FlyingPoints> ().View (points, directionsForPoints[dirIndx]);

			dirIndx++;
			if(dirIndx >= directionsForPoints.Length)
				dirIndx = 0;
		}
	}

	void restartGame ()
	{
		setBgColor ();

		pushAllObjects ();

		fillTable ();

		currentLvl++;
		if(currentLvl != 1)
			addPoints(Config.scoresForLvl);

		currentLvlView.text = currentLvl.ToString ();

		foundItems = 0;
		preItemID = -1;
		streak = 1;
		streakInGame = 0;

		showTimeBreakers();
		mStartedGameplay = true;
	}

	void setBgColor ()
	{
		bg.color = Config.bgColors[bgID];
		bgPreGame.color = Config.bgColors[bgID];
		bgPopup.color = Config.bgColors [bgID];
		bgBetweenLvl.color = Config.bgColors [bgID];

//		for(int i = 0; i < objRecolor.Length; i++)
//		{
//			objRecolor[i].color = Config.bgColors [bgID];;
//		}

		bgID++;
		if(bgID>= Config.bgColors.Length)
			bgID = 0;
	}

	void adjustTableSize()
	{
		float marginX = 50f;
		float marginY = 50f;

		float with = table.GetComponent<RectTransform> ().rect.width;
		with -= ((tableSize.x + 2) * marginX);
		float size1 = with / tableSize.x;

		float height = table.GetComponent<RectTransform> ().rect.height;
		height -= ((tableSize.y + 2) * marginY);
		float size2 = height / tableSize.y;

		float size = Mathf.Min (size1, size2);

		table.cellSize = new Vector2 (size, size);

		if(size1>size2)
		{
			float dif = (with - (tableSize.x * size))/tableSize.x;
			marginX += dif;
		}
		else
		{
			float dif = (height - (tableSize.y * size))/tableSize.y;
			marginY += dif;
		}
	
		table.spacing = new Vector2 (marginX, marginY);

	}

	public void itemClick(Item itm)
	{
		if(!mStartedGameplay)
			return;

		if(itm.type == Item.Type.bad_spy)
		{
			Handheld.Vibrate();
			AppSoundManager.Get().PlaySfx(Sfx.Type.sfx_monster_click);
			loseGame(false,itm.gameObject);
		}
		else if(itm.type == Item.Type.good)
		{
			AppSoundManager.Get().PlaySfx(Sfx.Type.sfx_item_click);
			GameObject obj = Instantiate(flashPrefab,itm.gameObject.GetComponent<RectTransform>().position,Quaternion.identity) as GameObject;
			obj.transform.SetParent(transform);
			obj.GetComponent<RectTransform>().localScale = new Vector3(1f,1f,1f);

			if(preItemID == itm.goodTypeID)
			{
				streak++;
				if(streak >=2)
					showStreak(itm,streak);
			}
			else 
			{
				if(streak>=2)
				{
					addPoints(calculatePointForStreak());
				}
				streak = 1;
			}

			preItemID = itm.goodTypeID;

			itm.fadeOut ();
			foundItems += 1;
			if(foundItems >= itemsForFind)
			{
				if(streak>=2)
				{
					addPoints(calculatePointForStreak());
					streak = 1;
				}
				StartCoroutine(winGame());
			}
		}
	}

	int calculatePointForStreak()
	{
		int p = 0;
		switch(streak)
		{
			case 2: p = Config.x2ComboPoints;break;
			case 3: p = Config.x3ComboPoints;break;
			case 4: p = Config.x4ComboPoints;break;
			default: p = Config.x5ComboPoints;break;
		}
		return p;
	}
	
	void showStreak (Item itm, int streak)
	{
		streakInGame += streak;
		GameObject tip = Instantiate (streakPrefab, itm.transform.position, Quaternion.identity) as GameObject;
		tip.GetComponent<StreakObject> ().setValue (streak);
		tip.transform.SetParent(transform);
		tip.transform.localScale = new Vector3 (1f, 1f, 1f);
	}

	public void onTimeClick()
	{
		if(currentTimeBreakers <=0 ) return;

		AppSoundManager.Get ().PlaySfx (Sfx.Type.sfx_refresh_click);
		currentTimeBreakers -= 1;
		int count = items.Count;
		for(int i = 0; i < count; i++)
		{
			items[i].GetComponent<Item>().resetTime();
		}

		showTimeBreakers ();
		timeBreakerAnimator.Play ("useTimeBreakerPoints", 0, 0f);
	}

	public void loseGame(bool isTime = false, GameObject obj = null)
	{
		if(!mStartedGameplay)
			return;

		AppSoundManager.Get ().StopMusic ();
		if(data.highScore < currentScores)
		{
			data.highScore = currentScores;
			data.save();
			UM_GameServiceManager.instance.SubmitScore(leaderBoardId, (int)currentScores);
		}
		if(currentScores>5000)
		{
			UM_AdManager.instance.StartInterstitialAd();
		}
		mStartedGameplay = false;

		if(isTime)
		{
			AppSoundManager.Get().PlaySfx(Sfx.Type.sfx_time_over);
			gameOverViewTime.SetActive (true);

			for(int i = 0; i < items.Count;i++)
			{
				if(items[i].GetComponent<Item>().type != Item.Type.good)
				{
					//objPool.push(items[i]);
					items[i].GetComponent<Item>().hideItem();
				}
			}
			
			StartCoroutine (onGameOver());
		}
		else
		{
			for(int i = 0; i < items.Count;i++)
			{
				if(items[i]!= obj)
				{
					//objPool.push(items[i]);
					items[i].GetComponent<Item>().hideItem();
				}
			}
			StartCoroutine(gameOverByClick());
		}


	}

	public void showLeaderBoard()
	{
		AppSoundManager.Get().PlaySfx(Sfx.Type.sfx_btn_click);
		#if UNITY_IPHONE || UNITY_ANDROID
		UM_GameServiceManager.instance.ShowLeaderBoardsUI();
		#else
		
		#endif
	}

	IEnumerator gameOverByClick()
	{
		yield return new WaitForSeconds (0.5f);
		gameOverViewClick.SetActive (true);
		StartCoroutine (onGameOver ());
		yield return null;
	}

	public void openCurtain()
	{
		StartCoroutine (curtainOpen ());
	}

	IEnumerator onGameOver()
	{
		yield return new WaitForSeconds (1.8f);
		GetComponent<Animator> ().Play ("CurtainAnimation",0,0f);
		yield return null;
	}

	IEnumerator curtainOpen()
	{

		yield return new WaitForSeconds (1f);

		gameOverViewTime.SetActive (false);
		gameOverViewClick.SetActive (false);
		finalScoresView.text = currentScores.ToString ();
		bestScoreView.text = data.highScore.ToString ();
		pushAllObjects ();
		startGamePopup.SetActive (true);

		GetComponent<Animator> ().Play ("CurtainAnimationOpen",0,0f);
		//AppSoundManager.Get ().PlayMusic (Music.Type.game);
		yield return null;
	}

	public void pushAllObjects()
	{
		int count = items.Count;
		
		for(int i = 0; i < count; i++)
		{
			objPool.push(items[i]);
			items[i].GetComponent<Item>().hideItem();
		}
		items.RemoveRange (0, items.Count);
	}

	public void onMenuClick()
	{
		AppSoundManager.Get().PlaySfx(Sfx.Type.sfx_btn_click);

		GoTo.LoadMenu ();
	}
	
	public void OnShareClick()
	{
		shareClick ();
	}
	public void shareClick() {
		
		StartCoroutine(TakeScreenshot());
	}
	private IEnumerator TakeScreenshot()
	{
		yield return new WaitForEndOfFrame();
		Texture2D snap = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
		//screenShotTexture.mainTexture = snap;
		
		snap.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
		//chụp màn hình lưu thành texture2D
		snap.Apply();
		UM_ShareUtility.FacebookShare("Pikachu",snap);
		
	}
	void share(string subject, string text, string title)
	{
//		AndroidJavaClass jc = new  AndroidJavaClass ("com.unity3d.player.UnityPlayer");
//		AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
//		jo.Call ("shareText", subject, text, title);
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

	public void showScreenBetweenLvl()
	{
		if (streakInGame < 7)
						return;

		betweenLvlvAnimator.gameObject.SetActive (true);
	
		AppSoundManager.Get ().PlaySfx (Sfx.Type.sfx_streak);
		if(streakInGame >= 7 && streakInGame <9)
			betweenLvlvAnimator.Play("good",0,0f);
		else if(streakInGame >=9 && streakInGame < 11)
			betweenLvlvAnimator.Play("perfect",0,0f);
		else if(streakInGame>=11)
			betweenLvlvAnimator.Play("exelent",0,0f);
	}
	IEnumerator winGame()
	{
		mStartedGameplay = false;
		showScreenBetweenLvl ();
		if(betweenLvlvAnimator.gameObject.activeSelf)
		{
			yield return new WaitForSeconds (1f);
			betweenLvlvAnimator.gameObject.SetActive (false);
		}
		else
			yield return new WaitForSeconds (0.1f);
		restartGame ();
		yield return null;
	}
}
