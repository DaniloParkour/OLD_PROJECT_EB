using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TutorialController : MonoBehaviour {

	public GameObject toExplode;
	public GameObject toMix;
	public BubbleController[] bubblesExplode;
	public BubbleController[] bubblesMix;
	public GameObject[] windows;
	public GameObject janelaDeDicas;

	public Image colorAtuto;
	public Image colorBtuto;
	public Image colorCtuto;

	private int currentSituation;
	private int quantBubblesExplode = 6;
	private int quantBubblesMix = 4;
	private float timeToWait;

	//BubbleManager
	private BubbleController mouseDownOnBubble;
	private BubbleController selectedBubble;

	void Awake(){
		toExplode.SetActive (true);
		toMix.SetActive (true);
		initBubbles ();
		toExplode.SetActive (false);
		toMix.SetActive (false);
	}

	// Use this for initialization
	void Start () {
		currentSituation = 0;
		GameSethings.language = "PT_BR";
		GameSethings.isWindowOpen = true;

		attTexts ();

		//timeToWait = 0.5f;
		toExplode.SetActive (true);
		initBubblesExplode ();
		//toMix.SetActive (true);
		//initBubblesMix ();

		janelaDeDicas.transform.Find("bu_no").gameObject.SetActive(false);
		janelaDeDicas.transform.Find("bu_closeOnNo").gameObject.SetActive(true);


	}
	
	// Update is called once per frame
	void Update () {

		//Coisas abaixo esperam pelo timeToWait
		/*if (timeToWait >= 0) {
			timeToWait -= Time.deltaTime;
			return;
		}*/

		if (currentSituation == 0) {
			windows[0].SetActive(true);
			currentSituation++; // [1]: Mostrando janela 1.
		} else if (currentSituation == 2) {
			windows[1].SetActive(true);
			currentSituation++; // [3]: Mostrando janela 2.
		} else if (currentSituation == 4) {
			windows[2].SetActive(true);
			currentSituation++; // [5]: Mostrando janela 3.
		} else if (currentSituation == 6) {
			GameSethings.isWindowOpen = false;
			currentSituation++; //[7]: Primeira prática do tutorial.
		} else if (currentSituation == 8) {
			timeToWait = 1;
			currentSituation++;
		} else if (currentSituation == 9 && timeToWait <= 0){
			GameSethings.isWindowOpen = true;
			toExplode.SetActive(false);
			toMix.SetActive (true);
			initBubblesMix ();
			windows[3].SetActive(true);
			currentSituation++; // [10]: Mostrar janela 4.
		} else if (currentSituation == 11) {
			windows[4].SetActive(true);
			currentSituation++; //[12]: Mostrar janela 5.
		} else if (currentSituation == 13) { //O player deverá juntar as bolhas e remover todas da tela.
			GameSethings.isWindowOpen = false;
			currentSituation++; // [14]: Segunda prática do tutorial.
		} else if (currentSituation == 15) {
			timeToWait = 1;
			currentSituation++;
		} else if (currentSituation == 16  && timeToWait <= 0) {
			GameSethings.isWindowOpen = true;
			toExplode.SetActive(false);
			toMix.SetActive (false);
			windows[5].SetActive(true);
			currentSituation++;
		} else if (currentSituation == 18) {
			janelaDeDicas.SetActive(true);
			janelaDeDicas.transform.Find("bu_no").gameObject.SetActive(true);
			janelaDeDicas.transform.Find("bu_closeOnNo").gameObject.SetActive(false);
			currentSituation++;
		}

		if (timeToWait > 0)
			timeToWait -= Time.deltaTime;

		if (currentSituation == 7 && quantBubblesExplode == 0) {
			currentSituation++;
		}

		if (currentSituation == 14 && quantBubblesMix == 0) {
			currentSituation++;
		}

		verifyClick ();
		verifyBubbles ();
	}

	void FixedUpdate(){
		for (int i = 0; i < bubblesExplode.Length; i++) {
			if (selectedBubble != null && bubblesExplode[i] == selectedBubble)
				continue;
			bubblesExplode [i].moveMe ();
		}
		for (int i = 0; i < bubblesMix.Length; i++) {
			if (selectedBubble != null && bubblesMix[i] == selectedBubble)
				continue;
			bubblesMix [i].moveMe ();
		}
	}

	private void verifyClick(){
		if (!GameSethings.isWindowOpen) {
			if (Input.GetMouseButtonDown (0)) {
				RaycastHit2D hit = Physics2D.Raycast (Camera.main.ScreenToWorldPoint (Input.mousePosition), Vector2.zero, Mathf.Infinity, 1 << 8); //The layer 8 is the Bubbles layer.
				if (hit.collider != null) {
					mouseDownOnBubble = hit.transform.gameObject.GetComponent<BubbleController> ();
				}
			} else if (Input.GetMouseButtonUp (0)) {
				RaycastHit2D hit = Physics2D.Raycast (Camera.main.ScreenToWorldPoint (Input.mousePosition), Vector2.zero, Mathf.Infinity, 1 << 8); //The layer 8 is the Bubbles layer.
				if (hit.collider != null && mouseDownOnBubble != null) {
					BubbleController bc_aux = hit.transform.gameObject.GetComponent<BubbleController> ();
					clickOnBubble (bc_aux);
				} else if (selectedBubble != null) {
					selectedBubble.removeSelect ();
					selectedBubble = null;
				}
				mouseDownOnBubble = null;
			}
		}
	}

	private void verifyBubbles(){
		if (toExplode.activeSelf) {
			for (int i = bubblesExplode.Length-1; i > 0; i--) {
				BubbleController b = bubblesExplode [i];
				if (b.isExplodeMe ()) {
					b.explode ();
					if (currentSituation == 7)
						quantBubblesExplode--;
				} else if (b.isRemoveMe ()) {
					b.transform.position = new Vector3 (-100, -100, 0);
				}
				if (selectedBubble != null && b == selectedBubble)
					continue;
				b.moveMe ();
			}
		}

		if (toMix.activeSelf) {
			for (int i = bubblesMix.Length-1; i > 0; i--) {
				BubbleController b = bubblesMix [i];
				if (b.isExplodeMe ()) {
					b.explode ();
					if (currentSituation == 14)
						quantBubblesMix--;
				} else if (b.isRemoveMe ()) {
					b.transform.position = new Vector3 (-100, -100, 0);
				}
				if (selectedBubble != null && b == selectedBubble)
					continue;
				b.moveMe ();
			}
		}
	}

	public void clickOnBubble(BubbleController b){
		//Pode selecionar um bubble, misturar, explodir ou errar mistura.
		//Debug.Log ("Bubble "+b.getColor()+" clicked.");
		
		if(b != null && mouseDownOnBubble == b){
			if(selectedBubble == null || (selectedBubble != b)){
				if(selectedBubble != null){
					bool mixBubbles = verifyMix(selectedBubble.getColor(), b.getColor());
					bool explodeBubbles = verifyExplode(selectedBubble, b);
					if(mixBubbles || explodeBubbles){
						selectedBubble.goToPosition(new Vector2(b.transform.position.x, b.transform.position.y), b);
					}
					selectedBubble.removeSelect();
					selectedBubble = null;
				} else {
					b.selectMe();
					selectedBubble = b;
				}
			} else {
				b.removeSelect();
				selectedBubble = null;
			}
		}
	}

	private bool verifyExplode(BubbleController bA, BubbleController bB) {
		if(bA.getColor().Equals(bB.getColor())){
			if(bA.isSecondaryColor() || bA.isTertiaryColor())
				return true;
		}
		return false;
	}

	//Retorna true se são cores misturáveias
	private bool verifyMix(string colorA, string colorB){

		//bool openDialog = true;

		/*if(colorA.Equals("Red") && colorB.Equals("Blue") || colorB.Equals("Red") && colorA.Equals("Blue")){
			return true;
		} else if(colorA.Equals("Red") && colorB.Equals("Yellow") || colorB.Equals("Red") && colorA.Equals("Yellow")){
			return true;
		} else */

		if(colorA.Equals("Blue") && colorB.Equals("Yellow") || colorB.Equals("Blue") && colorA.Equals("Yellow")){
			return true;
		}

		/*
		//Mix to tertraity
		if(colorA.Equals("Red") && colorB.Equals("Purple") || colorB.Equals("Red") && colorA.Equals("Purple")){
			return true;
		} else if(colorA.Equals("Red") && colorB.Equals("Orange") || colorB.Equals("Red") && colorA.Equals("Orange")){
			return true;
		} else if(colorA.Equals("Blue") && colorB.Equals("Purple") || colorB.Equals("Blue") && colorA.Equals("Purple")){
			return true;
		} else if(colorA.Equals("Yellow") && colorB.Equals("Orange") || colorB.Equals("Yellow") && colorA.Equals("Orange")){
			return true;
		} else */

		if(colorA.Equals("Yellow") && colorB.Equals("Green") || colorB.Equals("Yellow") && colorA.Equals("Green")){
			return true;
		} else if(colorA.Equals("Blue") && colorB.Equals("Green") || colorB.Equals("Blue") && colorA.Equals("Green")){
			return true;
		}

		return false;
	}

	private void attTexts(){
		//Att windows
		/*windows [0].transform.FindChild ("text").GetComponent<Text> ().text = (new GameSethings()).getText(GameSethings.textsGame.TUTORIAL_ONE);
		windows [1].transform.FindChild ("text").GetComponent<Text> ().text = (new GameSethings()).getText(GameSethings.textsGame.TUTORIAL_TWO);
		windows [2].transform.FindChild ("text").GetComponent<Text> ().text = (new GameSethings()).getText(GameSethings.textsGame.TUTORIAL_THREE);
		windows [3].transform.FindChild ("text").GetComponent<Text> ().text = (new GameSethings()).getText(GameSethings.textsGame.TUTORIAL_FOUR);
		windows [4].transform.FindChild ("text").GetComponent<Text> ().text = (new GameSethings()).getText(GameSethings.textsGame.TUTORIAL_FIVE);
		windows [5].transform.FindChild ("text").GetComponent<Text> ().text = (new GameSethings()).getText(GameSethings.textsGame.TUTORIAL_SIX);*/

		windows [0].transform.Find ("text").GetComponent<Text> ().text = GameSethings.getText(GameSethings.textsGame.TUTORIAL_ONE);
		windows [1].transform.Find ("text").GetComponent<Text> ().text = GameSethings.getText(GameSethings.textsGame.TUTORIAL_TWO);
		windows [2].transform.Find ("text").GetComponent<Text> ().text = GameSethings.getText(GameSethings.textsGame.TUTORIAL_THREE);
		windows [3].transform.Find ("text").GetComponent<Text> ().text = GameSethings.getText(GameSethings.textsGame.TUTORIAL_FOUR);
		windows [4].transform.Find ("text").GetComponent<Text> ().text = GameSethings.getText(GameSethings.textsGame.TUTORIAL_FIVE);
		windows [5].transform.Find ("text").GetComponent<Text> ().text = GameSethings.getText(GameSethings.textsGame.TUTORIAL_SIX);

		if(currentSituation == 18)
			janelaDeDicas.transform.Find ("text").GetComponent<Text> ().text = GameSethings.getText(GameSethings.textsGame.TUTORIAL_END);
		else
			janelaDeDicas.transform.Find ("text").GetComponent<Text> ().text = GameSethings.getText(GameSethings.textsGame.TUTORIAL_SKIP);
		janelaDeDicas.transform.Find ("bu_no").Find ("text").GetComponent<Text>().text = (new GameSethings()).getGUIText(GameSethings.textsGUI.BU_NO);
		janelaDeDicas.transform.Find ("bu_closeOnNo").Find ("text").GetComponent<Text>().text = (new GameSethings()).getGUIText(GameSethings.textsGUI.BU_NO);
		janelaDeDicas.transform.Find ("bu_yes").Find ("text").GetComponent<Text>().text = (new GameSethings()).getGUIText(GameSethings.textsGUI.BU_YES);
	}

	private void initBubbles() {
		for (int i = 0; i < bubblesExplode.Length; i++)
			bubblesExplode [i].initMe ();
		for (int i = 0; i < bubblesMix.Length; i++)
			bubblesMix [i].initMe ();
	}

	private void initBubblesMix(){
		
		bubblesMix [0].changeColorTo ("Yellow");
		bubblesMix [1].changeColorTo ("Yellow");
		bubblesMix [2].changeColorTo ("YellowGreen");
		bubblesMix [3].changeColorTo ("Green");
		bubblesMix [4].changeColorTo ("RedPurple");
		bubblesMix [5].changeColorTo ("RedPurple");
		
	}

	private void initBubblesExplode(){

		bubblesExplode [0].changeColorTo ("Yellow");
		bubblesExplode [1].changeColorTo ("Purple");
		bubblesExplode [2].changeColorTo ("Yellow");
		bubblesExplode [3].changeColorTo ("Yellow");
		bubblesExplode [4].changeColorTo ("Purple");
		bubblesExplode [5].changeColorTo ("YellowOrange");
		bubblesExplode [6].changeColorTo ("Yellow");
		bubblesExplode [7].changeColorTo ("RedPurple");
		bubblesExplode [8].changeColorTo ("RedPurple");
		bubblesExplode [9].changeColorTo ("YellowOrange");

	}

	public void changeColorTuto(string cor){
		if(cor.Equals("Red")){
			colorAtuto.color = new Color(1,1,1,0);
			colorBtuto.color = new Color(1,1,1,0);
			colorCtuto.color = (GameSethings.getColor(GameSethings.colorGame.RED));
		} else if(cor.Equals("Blue")){
			colorAtuto.color = new Color(1,1,1,0);
			colorBtuto.color = new Color(1,1,1,0);
			colorCtuto.color = GameSethings.getColor(GameSethings.colorGame.BLUE);
		} else if(cor.Equals("Yellow")){
			colorAtuto.color = new Color(1,1,1,0);
			colorBtuto.color = new Color(1,1,1,0);
			colorCtuto.color = (GameSethings.getColor(GameSethings.colorGame.YELLOW));
		} else if(cor.Equals("Green")){
			colorAtuto.color = (GameSethings.getColor(GameSethings.colorGame.BLUE));
			colorBtuto.color = (GameSethings.getColor(GameSethings.colorGame.YELLOW));
			colorCtuto.color = (GameSethings.getColor(GameSethings.colorGame.GREEN));
		} else if(cor.Equals("Orange")){
			colorAtuto.color = (GameSethings.getColor(GameSethings.colorGame.YELLOW));
			colorBtuto.color = (GameSethings.getColor(GameSethings.colorGame.RED));
			colorCtuto.color = (GameSethings.getColor(GameSethings.colorGame.ORANGE));
		} else if(cor.Equals("Purple")){
			colorAtuto.color = (GameSethings.getColor(GameSethings.colorGame.RED));
			colorBtuto.color = (GameSethings.getColor(GameSethings.colorGame.BLUE));
			colorCtuto.color = (GameSethings.getColor(GameSethings.colorGame.PURPLE));
		} else if(cor.Equals("RedPurple")){
			colorAtuto.color = (GameSethings.getColor(GameSethings.colorGame.RED));
			colorBtuto.color = (GameSethings.getColor(GameSethings.colorGame.PURPLE));
			colorCtuto.color = (GameSethings.getColor(GameSethings.colorGame.RED_PURPLE));
		} else if(cor.Equals("RedOrange")){
			colorAtuto.color = (GameSethings.getColor(GameSethings.colorGame.ORANGE));
			colorBtuto.color = (GameSethings.getColor(GameSethings.colorGame.RED));
			colorCtuto.color = (GameSethings.getColor(GameSethings.colorGame.RED_ORANGE));
		} else if(cor.Equals("BluePurple")){
			colorAtuto.color = (GameSethings.getColor(GameSethings.colorGame.PURPLE));
			colorBtuto.color = (GameSethings.getColor(GameSethings.colorGame.BLUE));
			colorCtuto.color = (GameSethings.getColor(GameSethings.colorGame.BLUE_PURPLE));
		} else if(cor.Equals("YellowOrange")){
			colorAtuto.color = (GameSethings.getColor(GameSethings.colorGame.YELLOW));
			colorBtuto.color = (GameSethings.getColor(GameSethings.colorGame.ORANGE));
			colorCtuto.color = (GameSethings.getColor(GameSethings.colorGame.YELLOW_ORANGE));
		} else if(cor.Equals("YellowGreen")){
			colorAtuto.color = (GameSethings.getColor(GameSethings.colorGame.GREEN));
			colorBtuto.color = (GameSethings.getColor(GameSethings.colorGame.YELLOW));
			colorCtuto.color = (GameSethings.getColor(GameSethings.colorGame.YELLOW_GREEN));
		} else if(cor.Equals("BlueGreen")){
			colorAtuto.color = (GameSethings.getColor(GameSethings.colorGame.BLUE));
			colorBtuto.color = (GameSethings.getColor(GameSethings.colorGame.GREEN));
			colorCtuto.color = (GameSethings.getColor(GameSethings.colorGame.BLUE_GREEN));
		}
	}

	public void windowNextStep(GameObject go){
		go.SetActive (false);
		currentSituation++;
	}

	public void closeMe(GameObject go){
		go.SetActive (false);
	}

	public void openMe(GameObject go){
		/*if (go.name.Equals ("janelaDeDicas")) {
			if (currentSituation == 18) {
				go.transform.FindChild ("text").GetComponent<Text> ().text = (new GameSethings ()).getText (GameSethings.textsGame.TUTORIAL_END);
				go.transform.FindChild("bu_no").gameObject.SetActive(true);
				go.transform.FindChild("bu_closeOnNo").gameObject.SetActive(false);
			} else {
				go.transform.FindChild ("text").GetComponent<Text> ().text = (new GameSethings ()).getText (GameSethings.textsGame.TUTORIAL_SKIP);
				go.transform.FindChild("bu_no").gameObject.SetActive(false);
				go.transform.FindChild("bu_closeOnNo").gameObject.SetActive(true);
			}
		}*/
		go.SetActive (true);
	}

	public void titleScreen(){
		Application.LoadLevelAsync("TitleScreen");
	}

	public void initTutorial(){
		Application.LoadLevel ("TutorialScene");
	}
}
