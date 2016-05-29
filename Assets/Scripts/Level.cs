using UnityEngine;
using System.Collections;

public class Level
{
	public int lvl;
	public int sizeByWidth;
	public int minBadGuy;
	public int maxBadGuy;
	
	public Level (int l, int s, int minB, int maxB)
	{
		lvl = l;
		sizeByWidth = s;
		minBadGuy = minB;
		maxBadGuy = maxB;
	}
}
