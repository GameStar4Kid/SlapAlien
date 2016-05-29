using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Flash : MonoBehaviour {


	public ParticleEmitter particles;

	public void showParticles()
	{
		particles.emit = true;
		GetComponent<Image> ().enabled = false;
		StartCoroutine (destroySelf ());
	}

	IEnumerator destroySelf()
	{
		yield return new WaitForSeconds (0.5f);
		GameObject.Destroy (this.gameObject);
		yield return null;
	}
}
