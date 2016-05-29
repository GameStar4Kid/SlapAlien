using UnityEngine;
using System.Collections;

public class Config {

	public static string borderId = "CgkI58r9jOkYEAIQAA";
//	public static int maxColumCount = 4;
//	public static int minFieldSize = 3;
//	public static int maxFieldSize = 5;
//
	public static int minFindItems = 2;
	public static int maxFindItems = 6;
//
//	public static int minSpyItems = 0;
//	public static int maxSpyItems = 4;

	public static int lvlForSpyAppearence = 5;

	public static int scoresForLvl = 750;
	public static int x2ComboPoints = 250;
	public static int x3ComboPoints = 500;
	public static int x4ComboPoints = 1000;
	public static int x5ComboPoints = 1500;

	public static int pointsForTimer = 2000;

	public static float startTime = 4f;
	public static float stepTime = 0.15f;
	public static float endTime = 2f;
	
	public static float timeForFlyingPoints = 0.7f;

	public static Color[] bgColors = 
	{
		new Color(22f/255f, 160f/255f, 133f/255f), 
		new Color(39f/255f,174f/255f,96f/255f),
		new Color(41f/255f,128f/255f,185f/255f),
		new Color(142f/255f,68f/255f,173f/255f),
		new Color(44f/255f,62f/255f,80f/255f),

		new Color(243f/255f,156f/255f,18f/255f),
		new Color(211f/255f,84f/255f,0f/255f),
		new Color(192f/255f,57f/255f,43f/255f),
		new Color(189f/255f,195f/255f,199f/255f),
		new Color(127f/255f,140f/255f,141f/255f)
	};

	public static Level[] levels = 
	{
		new Level(1,3,0,0),
		new Level(2,3,0,1),
		new Level(3,3,1,1),
		new Level(4,3,1,2),
		new Level(5,4,0,0),
		new Level(6,4,0,1),
		new Level(7,4,1,1),
		new Level(8,4,1,2),
		new Level(9,4,1,3),
		new Level(10,3,1,2),
		new Level(11,3,2,2),
		new Level(12,4,2,3),
		new Level(13,4,2,4),
		new Level(14,5,1,1),
		new Level(15,5,1,2),
		new Level(16,5,2,2),
		new Level(17,5,2,3),
		new Level(18,4,2,4),
		new Level(19,5,2,5),
		new Level(20,5,2,4)
	};

	public static Level GetLvlInfo(int l)
	{
		if (l >= levels.Length) 
		{
			return levels [Random.Range(0,levels.Length)];
		}
		else
			return levels [l];
	}

}
