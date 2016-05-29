using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StreakObject : MonoBehaviour {

	public ParticleEmitter particles;

	private Color[] valueColors = {
				new Color (0f, 167f / 255f, 70f / 255f) ,
				new Color (174 / 255f, 0f, 97f / 255f),
				new Color (148 / 255f, 0f, 138f / 255f),
				new Color (186 / 255f, 27f / 255f, 27f / 255f)
			};

	public void destroySelf()
	{
		GameObject.Destroy (this.gameObject);
	}

	public void setValue(int val)
	{
		val = Mathf.Min (val, 5);
		Text tx = GetComponent<Text> ();
		tx.color = valueColors [val-2];
		tx.text = val.ToString ();
	}
}
