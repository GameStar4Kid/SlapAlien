using UnityEngine;
using System.Collections;

public class Sfx 
{
    public enum Type
    {
		sfx_btn_click,
		sfx_item_click,
		sfx_monster_click,
		sfx_streak,
		sfx_time_over,
		sfx_refresh_click,
		sfx_refresh_add
    };


    public static string[] sfxFiles = 
    {
		"btn_click",
		"item_click",
		"monster_click",
		"streak_music",
		"time_over",
		"refresh_click",
		"refresh_add"
    };

}

public class Music
{
    public enum Type
    {
        game
    }

    public static string[] musicFiles =
    {
		"game_music"
    };
}

