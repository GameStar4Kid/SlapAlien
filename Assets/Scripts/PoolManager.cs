using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PoolManager : MonoBehaviour {

	public int count = 20;

	public string[] itemPrefabs;

	private List<GameObject> pool;

	public void initializePool(int id)
	{
		GameObject itemPrefab = Resources.Load(itemPrefabs[id]) as GameObject;
		pool = new List<GameObject> ();
		for(int i = 0 ; i < count; i++)
		{
			GameObject obj = Instantiate(itemPrefab,Vector3.zero,Quaternion.identity) as GameObject;
			obj.transform.SetParent(transform);
			obj.transform.localPosition = Vector3.zero;
			obj.transform.localScale = new Vector3(1f,1f,1f);
			pool.Add(obj);
		}
	}

	public GameObject pull()
	{
		if(pool.Count == 0)
		{
			Debug.LogError("Pool is empty!");
			return null;
		}

		GameObject obj = pool [0];
		pool.Remove (obj);

		return obj;
	}

	public void push(GameObject obj)
	{
		pool.Add (obj);
		obj.transform.SetParent(transform);
		obj.transform.localPosition = Vector3.zero;
	}

}
