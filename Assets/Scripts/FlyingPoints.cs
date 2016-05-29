using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FlyingPoints : MonoBehaviour {

	private Text text;
	private RectTransform rectTransform;
	private Vector3 direction;
	private const float speed = 3f;
	private bool isShow = false;

	// Update is called once per frame
	void Update () 
	{
		if(isShow)
		{
			Vector3 pos = rectTransform.position;
			pos += direction * Time.deltaTime * speed;
			rectTransform.position = pos;

			Color c = text.color;
			c.a = Mathf.Max(0.5f,c.a - Time.deltaTime);
			text.color = c;
		}
	}

	public void View(int points, Vector3 dir)
	{
		text = GetComponent<Text> ();
		rectTransform = GetComponent<RectTransform> ();

		text.text = points.ToString ();
		direction = dir;
		isShow = true;
		StartCoroutine (deleteSelf ());
	}

	IEnumerator deleteSelf()
	{
		yield return new WaitForSeconds (Config.timeForFlyingPoints);
		GameObject.Destroy (this.gameObject);
		yield return null;
	}

}
