using UnityEngine;
using System.Collections;

public class Window : MonoBehaviour {

	public bool pauseGameOnOpen;
	public bool continueGameOnClose;

	// Use this for initialization
	void Start () {
		if (gameObject.name.Equals ("initialWindow") && !GameSethings.enableTips) {
			Time.timeScale = 1;
			this.gameObject.SetActive(false);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void openMe(){
		if(pauseGameOnOpen)
			Time.timeScale = 0;
		gameObject.SetActive (true);
	}

	public void closeMe(){
		if(continueGameOnClose)
			Time.timeScale = 1;
		gameObject.SetActive (false);
	}
}
