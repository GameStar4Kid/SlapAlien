using UnityEngine;
using System.Collections;

public class Startup : MonoBehaviour {

	// Use this for initialization
	void Start () 
	{
		GoTo.LoadMenu ();
		GoTo.lastScene = "";
	}
}
