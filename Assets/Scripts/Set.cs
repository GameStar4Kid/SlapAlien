using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Set : MonoBehaviour {

	public Image bg;
	public GameObject chooseView;
	public GameObject priceView;
	public Text lbl;
	public void setBgColor(Color c,Color text)
	{
		bg.color = c;
		lbl.color = text;
	}

	public void choose()
	{
		chooseView.SetActive (true);
		priceView.SetActive (false);
	}

	public void setPriceView()
	{
		priceView.SetActive (true);
		chooseView.SetActive (false);
	}

	public void deSelect()
	{
		priceView.SetActive (false);
		chooseView.SetActive (false);
	}
}
