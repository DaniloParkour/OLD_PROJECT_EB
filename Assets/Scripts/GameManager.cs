using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour {

	public BubbleManager bubbleManager;
	public float totalTime;
	public Image quantBubbles;
	public Text score;
	public GameObject pauseEndWindow;
	public ComboBar comboBar;
	public int maxBubbles = 50;
	public GameObject bonus;

	private int currentScore;
	private int addToScore;
	private float timeToAddScore;

	//Deletar depois
	public Text totalTimeText;

	// Use this for initialization
	void Start () {
		totalTime = 0;
		quantBubbles.fillAmount = 0;
		score.text = "0";
		currentScore = 0;
		Time.timeScale = 1;
		GameSethings.isWindowOpen = false;
		timeToAddScore = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (!GameSethings.pauseGame)
			totalTime += Time.deltaTime;
		attGameScore ();
		totalTimeText.text = (int)totalTime + "";
	}

	public void pauseGame(){
		pauseEndWindow.gameObject.SetActive (true);
		GameSethings.isWindowOpen = true;
		pauseEndWindow.transform.Find ("totalScore").GetComponent<Text> ().text = "";
		bubbleManager.showBubbles (false);
		Time.timeScale = 0;
	}

	public void retry(){
		Application.LoadLevel ("GameScene");
	}

	public void home(){
		Application.LoadLevel ("TitleScreen");
	}

	public void titleScreen(){

	}

	public void attTexts(){

	}

	public void addScore(GameSethings.joinBubbles type){
		if(type == GameSethings.joinBubbles.EXPLODE_PRIMATY){
			addToScore += 10;
		} else if(type == GameSethings.joinBubbles.EXPLODE_SECONDARY){
			addToScore += 20;
		} else if(type == GameSethings.joinBubbles.EXPLODE_TERTIARY){
			addToScore += 50;
		} else if(type == GameSethings.joinBubbles.MIX_PRIMATY){
			addToScore += 5;
		} else if(type == GameSethings.joinBubbles.MIX_SECONDARY){
			addToScore += 10;
		}

		timeToAddScore = 0;
		score.color = Color.green;
	}

	//Atualiza a barra de indicação de quantidade de bubbles na screen.
	public void attQuantBubbles(int quant){
		quantBubbles.fillAmount = (quant/(float)maxBubbles);
	}

	public void endGame(){
		if (bubbleManager.totalBubbles () > maxBubbles) {
			pauseEndWindow.transform.Find("bu_close").gameObject.SetActive(false);
			pauseEndWindow.gameObject.SetActive (true);
			GameSethings.isWindowOpen = true;
			//pauseEndWindow.transform.FindChild("totalScore").GetComponent<Text>().text = (new GameSethings().getText(GameSethings.textsGame.SCORE)+": "+currentScore);
			pauseEndWindow.transform.Find("totalScore").GetComponent<Text>().text = GameSethings.getText(GameSethings.textsGame.SCORE)+": "+currentScore;
			Time.timeScale = 0;
		}
	}

	public void closeWindow(Window window){
		if (window.gameObject.name.Equals ("pauseWindow")) {
			Time.timeScale = 1;
			bubbleManager.showBubbles (true);
		}
		GameSethings.isWindowOpen = false;
		window.gameObject.SetActive (false);
	}

	public void saveGame(){

	}

	public void createCombo(float posX, float posY){
		int addToCurrentScore = 0;

		if (comboBar.getCurrentCombo () == 2) {
			addToCurrentScore += 60;
			bonus.transform.Find("Bonus_Two").GetComponent<BonusController>().initMe(new Vector3(posX, posY, 0), bonus.transform.position);
		} else if (comboBar.getCurrentCombo () == 3) {
			addToCurrentScore += 120;
			bonus.transform.Find("Bonus_Three").GetComponent<BonusController>().initMe(new Vector3(posX, posY, 0), bonus.transform.position);
		} else if (comboBar.getCurrentCombo () == 4) {
			addToCurrentScore += 200;
			bonus.transform.Find("Bonus_Four").GetComponent<BonusController>().initMe(new Vector3(posX, posY, 0), bonus.transform.position);
		} else if (comboBar.getCurrentCombo () == 5) {
			addToCurrentScore += 300;
			bonus.transform.Find("Bonus_Five").GetComponent<BonusController>().initMe(new Vector3(posX, posY, 0), bonus.transform.position);
		} else if (comboBar.getCurrentCombo () == 6) {
			addToCurrentScore += 400;
			bonus.transform.Find("Bonus_Six").GetComponent<BonusController>().initMe(new Vector3(posX, posY, 0), bonus.transform.position);
		} else if (comboBar.getCurrentCombo () == 7) {
			addToCurrentScore += 550;
			bonus.transform.Find("Bonus_Seven").GetComponent<BonusController>().initMe(new Vector3(posX, posY, 0), bonus.transform.position);
		} else if (comboBar.getCurrentCombo () == 8) {
			addToCurrentScore += 800;
			bonus.transform.Find("Bonus_Eight").GetComponent<BonusController>().initMe(new Vector3(posX, posY, 0), bonus.transform.position);
		} else if (comboBar.getCurrentCombo () == 9) {
			addToCurrentScore += 1000;
			bonus.transform.Find("Bonus_Nine").GetComponent<BonusController>().initMe(new Vector3(posX, posY, 0), bonus.transform.position);
		} else if (comboBar.getCurrentCombo () == 10) {
			addToCurrentScore += 2000;
			bonus.transform.Find("Bonus_Ten").GetComponent<BonusController>().initMe(new Vector3(posX, posY, 0), bonus.transform.position);
		}

		currentScore += addToCurrentScore;
	}

	public void addSecondaryCombo(){
		comboBar.addSecondary ();
	}

	public void addTertiaryCombo(float posX, float posY){
		comboBar.addTertiary ();
		createCombo (posX, posY);
	}

	//Private methods
	private void addExplodeSecondary(){
		Debug.Log ("Implementar secondary explosion!");
	}
	
	private void addExplodeTertiary(){
		Debug.Log ("Implementar tertiary explosion!");
	}

	private void attGameScore(){
		if (timeToAddScore > 0)
			timeToAddScore -= Time.deltaTime;
		if (addToScore > 0 && timeToAddScore <= 0) {
			currentScore++;
			addToScore--;
			timeToAddScore = 0.01f;
			score.text = "" + currentScore;
			if(addToScore == 0)
				score.color = Color.black;
		}
	}

}
