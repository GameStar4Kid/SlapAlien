using UnityEngine;
using System.Collections;

public class ItemMainMenu : MonoBehaviour {

	public int phase;
	float timeForAnim = 0.3f;
	private RectTransform rectTransform;

	void Start()
	{
		rectTransform = GetComponent<RectTransform> ();
	}

	void Update () 
	{
		Vector3 angle = Vector3.Lerp (new Vector3(0f,0f,-5f), new Vector3(0f,0f,5f), (Mathf.Sin((Time.time/timeForAnim)*2f + Mathf.Deg2Rad*phase ) + 1.0f) / 2.0f);
		rectTransform.localRotation = Quaternion.Euler (angle);
	}
}
