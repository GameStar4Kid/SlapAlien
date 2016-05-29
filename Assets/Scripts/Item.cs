using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class Item : MonoBehaviour, IPointerClickHandler {

	public Image overlapImg;
	public Image eyesImg;

	public Color eyesColor;
	public Color eyesColorBad;

	public Sprites sprites;

	public Animator refreshAnimator;

	[HideInInspector]
	public int goodTypeID = -1;
	[HideInInspector]
	public Type type;
	[HideInInspector]
	public Game gm;

	private bool isShow = false;
	private float timeForPlay;
	private float currentTime;
	private float phase;
	private float timeForAnim;
	private RectTransform rectTransform;

	public enum Type
	{
		good,
		bad,
		bad_spy
	}

	void Start()
	{
		phase = UnityEngine.Random.Range (-180, 180);
		timeForAnim = UnityEngine.Random.Range (0.1f, 0.5f);
		rectTransform = GetComponent<RectTransform> ();
	}
	
	void Update () 
	{
		if(isShow && type != Type.bad)
		{
			currentTime += Time.deltaTime;
			currentTime = Math.Min(currentTime,timeForPlay);

			float cof = (timeForPlay - currentTime)/timeForPlay;
			overlapImg.fillAmount = cof;
			if(cof <= 0f)
			{
				isShow = false;
				gm.loseGame(true);
			}
			else if(cof <= 1f && type == Type.good)
			{
				eyesImg.color = Color.Lerp(Color.white,eyesColor,(Mathf.Sin((Time.time/0.1f)*2f) + 1.0f) / 2.0f);
			}
		}

		Vector3 angle = Vector3.Lerp (new Vector3(0f,0f,-3f), new Vector3(0f,0f,3f), (Mathf.Sin((Time.time/timeForAnim)*2f + Mathf.Deg2Rad*phase ) + 1.0f) / 2.0f);
		rectTransform.localRotation = Quaternion.Euler (angle);
	}

	public void hideItem()
	{
		fadeOut ();
		isShow = false;
	}

	public void setView(float time)
	{
		GetComponent<Animator> ().enabled = false;
		Image img = GetComponent<Image> ();
		Color c  = img.color;
		c.a = 1f;
		img.color = c;

		if(type == Type.bad_spy)
			c = eyesColorBad;
		else
			c = Color.white;
		c.a = 1f;
		eyesImg.color = c;
		
		c = overlapImg.color;
		c.a = 0.75f;
		overlapImg.color = c;

		if(type == Type.bad)
		{
			overlapImg.enabled = false;
			int idx = UnityEngine.Random.Range (0, sprites.badCharacters.Length);
			img.sprite =sprites.badCharacters[idx];
			eyesImg.sprite = sprites.eyesBad[idx];
		}
		else
		{
			overlapImg.enabled = true;
			overlapImg.fillAmount  =1f;
			if(type == Type.good)
			{
				goodTypeID = UnityEngine.Random.Range (0, sprites.goodCharacters.Length);
				img.sprite =sprites.goodCharacters[goodTypeID];
				overlapImg.sprite = sprites.masks[goodTypeID];
				eyesImg.sprite = sprites.eyesGod[goodTypeID];
			}
			else if(type == Type.bad_spy)
			{
				img.sprite =sprites.badSpyCharacters[UnityEngine.Random.Range (0, sprites.badSpyCharacters.Length)];
				overlapImg.sprite = sprites.spyMask;
				eyesImg.sprite = sprites.spyEyes;
				if(sprites.showAnim)
				{
					GetComponent<Animator> ().enabled = true;
					GetComponent<Animator> ().Play ("BossAnim");
				}
			}
		}
		timeForPlay = time;
		currentTime = 0f;
		isShow = true;
	}

	#region IPointerClickHandler implementation

	public void OnPointerClick (PointerEventData eventData)
	{
		if(!isShow)
			return;

		isShow = false;
		gm.itemClick (this);
	}

	#endregion

	public void fadeOut()
	{
		Color c  = GetComponent<Image> ().color;
		c.a = 0f;
		GetComponent<Image> ().color = c;

		c = eyesImg.color;
		c.a = 0f;
		eyesImg.color = c;

		c = overlapImg.color;
		c.a = 0f;
		overlapImg.color = c;
	}

	public void resetTime()
	{
		if(type != Type.bad && isShow)
		{
			refreshAnimator.Play ("Refresh", 0, 0f);
			if(type == Type.good)
				eyesImg.color = Color.white;
			else if(type == Type.bad_spy)
				eyesImg.color = eyesColorBad;

		}
		currentTime = 0f;
	}

}
