using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Options : MonoBehaviour {

	public Set[] sets;
	public GameObject buyBtn;
	public GameObject buyAdvertsBtn;

	public Color chooseColor;
	public Color notChooseColor;
	public Color chooseColorText;
	public Color notChooseColorText;

	public Image musicImg;
	public Button musicBtn;
	public Sprite[] positiveSprites;
	public Sprite[] negativeSprites;

	GameData data;

	public void Initialize()
	{
		data = GameData.Get ();

//		for(int i = 0; i < sets.Length; i++)
//		{
//			sets[i].setBgColor(notChooseColor,notChooseColorText);
//			sets[i].deSelect();
//		}


//		sets [data.spritesId].setBgColor (chooseColor, chooseColorText);
//		sets [data.spritesId].choose ();

		setMusicImg ();
	}

	public void setCurrentSpritesKit(int id)
	{
		AppSoundManager.Get ().PlaySfx (Sfx.Type.sfx_btn_click);
		data.spritesId = id;
		data.save ();
	}

	public void  OnMusicTick()
	{
		data.music = !data.music;
		data.save ();

		AppSoundManager.MuteSfx = !data.music;
		AppSoundManager.MuteMusic = !data.music;
		AppSoundManager.Get ().PlaySfx (Sfx.Type.sfx_btn_click);

//		if(data.music)
//			AppSoundManager.Get ().PlayMusic(Music.Type.game);
//		else
//			AppSoundManager.Get ().StopMusic ();

		setMusicImg ();
	}

	public void OnBuyClick()
	{
		AppSoundManager.Get ().PlaySfx (Sfx.Type.sfx_btn_click);

	}

	public void onBuy()
	{
		//sets [1].deSelect ();
		buyAdvertsBtn.SetActive (false);

		data.isBuy = true;
		data.save ();
	}

	private void setMusicImg()
	{
		if(data.music)
		{
			musicImg.sprite = positiveSprites[0];
			SpriteState s = musicBtn.spriteState;
			s.pressedSprite = positiveSprites[1];
			musicBtn.spriteState = s;
		}
		else
		{
			musicImg.sprite = negativeSprites[0];
			SpriteState s = musicBtn.spriteState;
			s.pressedSprite = negativeSprites[1];
			musicBtn.spriteState = s;
		}

	}
}
