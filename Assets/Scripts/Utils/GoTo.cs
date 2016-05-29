using UnityEngine;
using System.Collections;

public class GoTo {

	public static string lastScene = "";

	public static void LoadGame()
	{
		lastScene = "game";
		Application.LoadLevel("game");
	}

	public static void LoadMenu()
	{
		lastScene = "menu";
		Application.LoadLevel("menu");
	}

	public static void LoadHelp()
	{
		lastScene = "help";
		Application.LoadLevel("help");
	}

}
