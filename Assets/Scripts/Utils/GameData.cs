using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using LitJson;

public class GameData{
	
	private static GameData instance;

	public int highScore;
	public int spritesId;
	public bool music;
	public bool isBuy;

	private string versionId = "save_000110";
	public static GameData Get()
	{
		if (instance == null)
		{
			instance = new GameData();
			instance = instance.Load();
			AppSoundManager.MuteSfx = !instance.music;
			AppSoundManager.MuteMusic = !instance.music;
		}
		return instance;
	}
	
	
	GameData Load ()
	{
		string data = PlayerPrefs.GetString(versionId, null);
		Debug.Log("Load game data:" + data);
		if (data == null || data.Trim() == "")
		{
			reset();
			return this;
		}
		GameData gdata;
		try
		{
			gdata = JsonMapper.ToObject<GameData>(data);
		}
		catch (System.Exception e)
		{
			Debug.Log(e);
			reset();
			return this;
		}
		return gdata;
	}
	
	void reset ()
	{
		isBuy = false;
		music = true;
		highScore = 0;
		spritesId = 0;
	}
	
	public void save ()
	{
		string data = JsonMapper.ToJson(this);
		Debug.Log("Save gamedata as:" + data);
		PlayerPrefs.SetString(versionId, data);
	}
	
}
