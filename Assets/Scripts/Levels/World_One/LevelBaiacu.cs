using UnityEngine;
using System.Collections;

public class LevelBaiacu : MonoBehaviour {

	public LevelOne_W1_Manager gameManager;
	public Baiacu boss;
	public GameObject pauseEndWindow;
	public GameObject pauseLoseWindow;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		//Se Baiacu derrotado:
		if (boss.hp () <= 0 && gameManager.getTimeToEndLevel () == -10) {
			gameManager.setTimeToEndLevel (2);
			boss.animaOnLose ();
			//Parar mix e explode das bolhas.
		}

		if(gameManager.bubblesManager.getBubbles().Count <= 0 && gameManager.getTimeToLoseLevel() == -10){
			gameManager.setTimeToLoseLevel(1);
		}

		if (gameManager.getTimeToEndLevel () > 0) {
			gameManager.setTimeToEndLevel(gameManager.getTimeToEndLevel () - Time.deltaTime);
		}

		if (gameManager.getTimeToLoseLevel () > 0) {
			gameManager.setTimeToLoseLevel(gameManager.getTimeToLoseLevel () - Time.deltaTime);
		}

	}
}
