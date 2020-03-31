using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class LevelOne_W1_Manager : MonoBehaviour {
	
	public int levelNumber;
	public BubbleLevelManager bubblesManager;
	public BubbleController[] bubblesLevelOne;
	public BubbleController[] fixedBubbles;
	public GameObject endPauseWindow;
	public GameObject loseWindow;
	public GameSethings.colorGame[] colors;
	public GameSethings.colorGame[] colorsFixedColors;
	public GameSethings.levelType levelType;
	public GameSethings.loseType loseType;
	public int maxBubbles;
	public BubblesGenerator bubblesGenerator;
	public float timeToSurvivor;
	public Text timeLeft;
	
	private float timeToEndLevel;
	private float timeToLoseLevel;
	private float totalTime;

	void Awake(){
		if (bubblesGenerator != null)
			bubblesGenerator.typeLevel = this.levelType;
		if (bubblesManager != null)
			bubblesManager.maxBubbles = maxBubbles;
	}

	// Use this for initialization
	void Start () {

		timeToEndLevel = -10;
		timeToLoseLevel = -10;

		if (bubblesLevelOne != null) {
			for (int i = 0; i < bubblesLevelOne.Length; i++) {
				bubblesLevelOne [i].initMe ();
				bubblesLevelOne [i].changeColorTo (GameSethings.getColorName (colors [i]));
				bubblesManager.addBubble (bubblesLevelOne [i]);
			}
		}

		if (fixedBubbles != null) {
			for (int i = 0; i < fixedBubbles.Length; i++) {
				fixedBubbles [i].initMe ();
				fixedBubbles [i].setFixed(true);
				//fixedBubbles[i].transform.FindChild("bubbleBright").gameObject.GetComponent<SpriteRenderer>().material.color = new Color(0.8f,0.8f,0.8f,1);
				//fixedBubbles[i].transform.FindChild("bubbleRing").gameObject.GetComponent<SpriteRenderer>().material.color = new Color(0.8f,0.8f,0.8f,1);
				fixedBubbles[i].transform.Find("bubbleBright").gameObject.GetComponent<SpriteRenderer>().material.color = new Color(1,1,1,0);
				fixedBubbles[i].transform.Find("bubbleRing").gameObject.GetComponent<SpriteRenderer>().material.color = new Color(1,1,1,0);
				fixedBubbles [i].changeColorTo (GameSethings.getColorName (colorsFixedColors [i]));
				bubblesManager.addBubble (fixedBubbles [i]);
			}
		}

		if (levelType.Equals (GameSethings.levelType.LET_PRIMARY) || levelType.Equals (GameSethings.levelType.SURVIVOR)
		    || levelType.Equals (GameSethings.levelType.BAIACU)) {
			Time.timeScale = 0;
			GameSethings.isWindowOpen = true;
		} else {
			Time.timeScale = 1;
			GameSethings.isWindowOpen = false;
		}

		attTexts ();

		Window[] ws = FindObjectsOfType<Window> ();
		foreach (Window w in ws) {
			if(w.gameObject.name.Equals("pauseWindow") || w.gameObject.name.Equals("pauseLose")) {
				w.gameObject.SetActive(false);
				w.transform.Find("bu_home").gameObject.SetActive(true);
				w.transform.Find("bu_retry").gameObject.SetActive(true);
				w.transform.Find("bu_wolrd").gameObject.SetActive(true);
			}
			if(w.gameObject.name.Equals("pauseWindow")){
				if(levelNumber < GameSethings.player_level){
					w.transform.Find("bu_next").gameObject.SetActive(true);
				} else {
					w.transform.Find("bu_next").gameObject.SetActive(false);
				}
			}
		}

	}
	
	// Update is called once per frame
	void Update () {

		//Gamb ___________________________________________________________________________
		if (GameSethings.isWindowOpen && Time.timeScale == 1) {
			if (timeToLoseLevel <= -10 && timeToEndLevel <= -10)
				GameSethings.isWindowOpen = false;
			else
				return;
		}
		//"""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""

		if (levelType.Equals (GameSethings.levelType.REMOVE_ALL)) {
			if (bubblesManager.getBubbles ().Count <= 1 && timeToEndLevel <= -10) {
				timeToEndLevel = 1;
			}
		} else if (levelType.Equals (GameSethings.levelType.LET_PRIMARY) && timeToEndLevel <= -10) {
			if (endLetPrimary()) {
				bubblesManager.setEndLevel(true);
				timeToEndLevel = 1;
			}
		} else if (levelType.Equals (GameSethings.levelType.SURVIVOR)) {

			if(timeToEndLevel <= -10 && timeToLoseLevel <= -10 && !GameSethings.isWindowOpen){
				timeToSurvivor -= Time.deltaTime;
				timeLeft.text = ""+(int)timeToSurvivor;
			}

			if(timeToSurvivor <= 1 && timeToEndLevel <= -10){
				bubblesManager.setEndLevel(true);
				timeToEndLevel = 1;
			}
		} //Else para o próximo tipo de fase

		if (timeToEndLevel > 0)
			timeToEndLevel -= Time.deltaTime;
		if (timeToLoseLevel > 0)
			timeToLoseLevel -= Time.deltaTime;

		if (timeToEndLevel <= 0 && timeToEndLevel >= -5) {
			endPauseWindow.transform.Find("bu_close").gameObject.SetActive(false);
			endPauseWindow.transform.Find ("bu_wolrd").gameObject.SetActive(true);
			endPauseWindow.transform.Find ("bu_next").gameObject.SetActive(true);
			endPauseWindow.SetActive(true);
			GameSethings.isWindowOpen = true;
			attTexts();
			timeToEndLevel = -10;
			if(levelNumber >= GameSethings.player_level)
				GameSethings.player_level++;
		}
		if (timeToLoseLevel <= 0 && timeToLoseLevel >= -5) {
			loseWindow.SetActive(true);
			bubblesManager.setEndLevel(true);
			GameSethings.isWindowOpen = true;
			timeToLoseLevel = -10;
		}

		/*if (levelType.Equals (GameSethings.levelType.REMOVE_ALL)) {
			if (!hasMoviment () && bubblesManager.getBubbles ().Count >= 2 && timeToLoseLevel <= -10) {
				timeToLoseLevel = 2;
			}
		} else if (levelType.Equals (GameSethings.levelType.SURVIVOR)) {
			if (bubblesManager.getBubbles ().Count >= bubblesManager.maxBubbles && timeToLoseLevel <= -10) {
				timeToLoseLevel = 1;
			}
		}*/

		if (loseType.Equals (GameSethings.loseType.NO_MOVES)) {
			//Debug.Log("Has moviment = "+hasMoviment()+". NumBubbmes = "+(bubblesManager.getBubbles().Count-1) + ". Time to lose level = "+timeToLoseLevel+".");
			if (!hasMoviment () && bubblesManager.getBubbles().Count > 1 && timeToLoseLevel <= -10) {
				timeToLoseLevel = 1;
				timeToEndLevel = -10;
			}
		} else if (loseType.Equals (GameSethings.loseType.MANY_BUBBLES)) {
			if (bubblesManager.getBubbles ().Count >= maxBubbles && timeToLoseLevel <= -10) {
				bubblesManager.setEndLevel(true);
				timeToLoseLevel = 1;
				timeToEndLevel = -10;
			}
		}

	}

	private bool endLetPrimary(){

		if (bubblesManager.getBubbles () == null)
			return false;

		foreach(BubbleController b in bubblesManager.getBubbles()){
			if(!b.isActiveAndEnabled || b.transform.position.x <= -20)
				continue;
			//Debug.Log("["+bubblesManager.getBubbles().Count+"] Cor atual: "+b.getColor());
			if(b.getColor().Equals(GameSethings.getColorName(GameSethings.colorGame.RED)))
				continue;
			if(b.getColor().Equals(GameSethings.getColorName(GameSethings.colorGame.YELLOW)))
				continue;
			if(b.getColor().Equals(GameSethings.getColorName(GameSethings.colorGame.BLUE)))
				continue;

			return false;
		}
		return true;
	}

	private bool hasMoviment(){

		//if (bubblesManager.getBubbles ().Count <= 2)
		if (bubblesManager.getBubbles ().Count < 2)
			return false;

		BubbleController b1;
		BubbleController b2;
		//for(int i = 1; i < bubblesManager.getBubbles().Count-1; i++){
		for(int i = 0; i < bubblesManager.getBubbles().Count-1; i++){
			b1 = bubblesManager.getBubbles()[i];
			for(int j = i+1; j < bubblesManager.getBubbles().Count; j++){
				b2 = bubblesManager.getBubbles()[j];
				//Debug.Log("["+i+"]: "+b1.getColor()+". ["+j+"]: "+b2.getColor()+"): De "+bubblesManager.getBubbles().Count+" bubbles.");
				if(verifyMix(b1.getColor(), b2.getColor()) || verifiExplode(b1,b2)){
					//Debug.Log("Tem movimento!");
					return true;
				} //else
					//Debug.Log("NÃO TEM MOVIMENTO!");
			}
		}

		return false;
	}

	private bool verifyMix(string colorA, string colorB){
		if(colorA.Equals("Red") && colorB.Equals("Blue") || colorB.Equals("Red") && colorA.Equals("Blue")){
			return true;
		} else if(colorA.Equals("Red") && colorB.Equals("Yellow") || colorB.Equals("Red") && colorA.Equals("Yellow")){
			return true;
		} else if(colorA.Equals("Blue") && colorB.Equals("Yellow") || colorB.Equals("Blue") && colorA.Equals("Yellow")){
			return true;
		}
		
		//Mix to tertraity
		if(colorA.Equals("Red") && colorB.Equals("Purple") || colorB.Equals("Red") && colorA.Equals("Purple")){
			return true;
		} else if(colorA.Equals("Red") && colorB.Equals("Orange") || colorB.Equals("Red") && colorA.Equals("Orange")){
			return true;
		} else if(colorA.Equals("Blue") && colorB.Equals("Purple") || colorB.Equals("Blue") && colorA.Equals("Purple")){
			return true;
		} else if(colorA.Equals("Yellow") && colorB.Equals("Orange") || colorB.Equals("Yellow") && colorA.Equals("Orange")){
			return true;
		} else if(colorA.Equals("Yellow") && colorB.Equals("Green") || colorB.Equals("Yellow") && colorA.Equals("Green")){
			return true;
		} else if(colorA.Equals("Blue") && colorB.Equals("Green") || colorB.Equals("Blue") && colorA.Equals("Green")){
			return true;
		}
		
		return false;
	}

	private void attTexts () {
		Window[] ws = FindObjectsOfType<Window> ();
		foreach (Window w in ws) {
			if(w.gameObject.name.Equals("pauseWindow")){
				if(timeToEndLevel == -10){
					if(levelType.Equals(GameSethings.levelType.REMOVE_ALL))
						w.transform.Find("message").GetComponent<Text>().text = GameSethings.getText(GameSethings.textsGame.LEVEL_REMOVE_ALL);
					else if(levelType.Equals(GameSethings.levelType.LET_PRIMARY))
						w.transform.Find("message").GetComponent<Text>().text = GameSethings.getText(GameSethings.textsGame.LEVEL_LET_PRYMARY);
					else if(levelType.Equals(GameSethings.levelType.SURVIVOR))
						w.transform.Find("message").GetComponent<Text>().text = GameSethings.getText(GameSethings.textsGame.LEVEL_SURVIVOR) + (int)timeToSurvivor + "s.";
					else if(levelType.Equals(GameSethings.levelType.BAIACU))
						w.transform.Find("message").GetComponent<Text>().text = GameSethings.getText(GameSethings.textsGame.LEVEL_BAIACU);

				} else {
					w.transform.Find("message").GetComponent<Text>().text = GameSethings.getText(GameSethings.textsGame.END_LEVEL);
				}

			} else if(w.gameObject.name.Equals("pauseLose")){

				if(loseType.Equals(GameSethings.loseType.MANY_BUBBLES))
					w.transform.Find("message").GetComponent<Text>().text = GameSethings.getText(GameSethings.textsGame.LOSE_MANY_BUBBLES);
				else if(loseType.Equals(GameSethings.loseType.NO_MOVES))
					w.transform.Find("message").GetComponent<Text>().text = GameSethings.getText(GameSethings.textsGame.LOSE_NO_MOVES);
				else if(loseType.Equals(GameSethings.loseType.NO_BUBBLES))
					w.transform.Find("message").GetComponent<Text>().text = GameSethings.getText(GameSethings.textsGame.LOSE_NO_BUBBLES);

				if(Application.loadedLevelName.Contains("_W0")){
					
				}

			} else if(w.gameObject.name.Equals("initialWindow") && !SceneManager.GetActiveScene().name.Contains("W0")) {
				if(levelType.Equals(GameSethings.levelType.REMOVE_ALL))
					w.transform.Find("message").GetComponent<Text>().text = GameSethings.getText(GameSethings.textsGame.LEVEL_REMOVE_ALL);
				else if(levelType.Equals(GameSethings.levelType.LET_PRIMARY))
					w.transform.Find("message").GetComponent<Text>().text = GameSethings.getText(GameSethings.textsGame.LEVEL_LET_PRYMARY);
				else if(levelType.Equals(GameSethings.levelType.SURVIVOR))
					w.transform.Find("message").GetComponent<Text>().text = GameSethings.getText(GameSethings.textsGame.LEVEL_SURVIVOR) + (int)timeToSurvivor + "s.";
				else if(levelType.Equals(GameSethings.levelType.BAIACU))
					w.transform.Find("message").GetComponent<Text>().text = GameSethings.getText(GameSethings.textsGame.LEVEL_BAIACU);

				if(Application.loadedLevelName.Contains("_W0")){
					
				}

			}

		}
	}

	private bool verifiExplode(BubbleController bA, BubbleController bB) {
		if(bA.getColor().Equals(bB.getColor())){
			if(bA.isSecondaryColor() || bA.isTertiaryColor())
				return true;
		}
		return false;
	}

	public void retry(){
		Application.LoadLevel (Application.loadedLevelName);
	}

	public void home(){
		Application.LoadLevel ("TitleScreen");
	}

	public void worlds(){
		Application.LoadLevel ("LevelsScene");
	}

	public void nextLevel(){
		if(Application.loadedLevelName.Equals("LevelOne_W1"))
			Application.LoadLevel ("LevelTwo_W1");
		else if(Application.loadedLevelName.Equals("LevelTwo_W1"))
			Application.LoadLevel ("LevelThree_W1");
		else if(Application.loadedLevelName.Equals("LevelThree_W1"))
			Application.LoadLevel ("LevelFour_W1");
		else if(Application.loadedLevelName.Equals("LevelFour_W1"))
			Application.LoadLevel ("LevelFive_W1");
		else if(Application.loadedLevelName.Equals("LevelFive_W1"))
			Application.LoadLevel ("LevelSix_W1");
		else if(Application.loadedLevelName.Equals("LevelSix_W1"))
			Application.LoadLevel ("LevelSeven_W1");
		else if(Application.loadedLevelName.Equals("LevelSeven_W1"))
			Application.LoadLevel ("LevelEight_W1");
		else if(Application.loadedLevelName.Equals("LevelEight_W1"))
			Application.LoadLevel ("LevelNine_W1");
		else if(Application.loadedLevelName.Equals("LevelNine_W1"))
			Application.LoadLevel ("LevelTen_W1");
		else if(Application.loadedLevelName.Equals("LevelTen_W1"))
			Application.LoadLevel ("LevelEleven_W1");
		else if(Application.loadedLevelName.Equals("LevelEleven_W1"))
			Application.LoadLevel ("LevelTwelve_W1");
		else if(Application.loadedLevelName.Equals("LevelTwelve_W1"))
			Application.LoadLevel ("LevelThirteen_W1");
		else if(Application.loadedLevelName.Equals("LevelThirteen_W1"))
			Application.LoadLevel ("LevelFourteen_W1");
		else if(Application.loadedLevelName.Equals("LevelFourteen_W1"))
			Application.LoadLevel ("LevelBaiacu_W1");
		else if(Application.loadedLevelName.Equals("LevelBaiacu_W1"))
			Application.LoadLevel ("Level16_W1");
		else if(Application.loadedLevelName.Equals("Level16_W1"))
			Application.LoadLevel ("Level17_W1");
	}

	public void pauseGame(){
		endPauseWindow.gameObject.SetActive (true);
		GameSethings.isWindowOpen = true;
		Time.timeScale = 0;
	}

	public void continueGame(){
		endPauseWindow.gameObject.SetActive (false);
		Time.timeScale = 1;
	}

	public float getTimeToEndLevel(){
		return timeToEndLevel;
	}

	public void setTimeToEndLevel(float time){
		timeToEndLevel = time;
	}

	public float getTimeToLoseLevel(){
		return timeToLoseLevel;
	}
	
	public void setTimeToLoseLevel(float time){
		timeToLoseLevel = time;
	}
}
