using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BubbleManager : MonoBehaviour {

	public GameManager gameManager;
	public BubbleController bubblePrefab;

	private BubbleController selectedBubble;
	private BubbleController mouseDownOnBubble;
	private List<BubbleController> bubbles;
	private List<BubbleController> availableBubbles;
	private float timeToNewBubble;
	private float totalTime;
	private int currentColorTertiary;
	private int maxBubbles = 50;

	//RedPurple, RedOrange, BluePurple, YellowOrange, YellowGreen, BlueGreen.
	private bool[] tertiaryColors = {false, false, false, false, false, false};

	// Use this for initialization
	void Start () {
		timeToNewBubble = 2;
		bubbles = new List<BubbleController>();
		bubbles.Add (Instantiate (bubblePrefab,new Vector3(-100,-100,0), Quaternion.identity) as BubbleController);
		availableBubbles = new List<BubbleController>();
		for(int i = 0; i < gameManager.maxBubbles; i++){
			BubbleController newBubble = Instantiate (bubblePrefab,new Vector3(-100,-100,0), Quaternion.identity) as BubbleController;
			newBubble.gameObject.SetActive(false);
			availableBubbles.Add(newBubble);
		}
		totalTime = 0;
		currentColorTertiary = 0;
		maxBubbles = gameManager.maxBubbles;

		//createBubble ("");
	}
	
	// Update is called once per frame
	void Update () {
		timeToNewBubble -= Time.deltaTime;
		totalTime += Time.deltaTime;

		if (timeToNewBubble <= 0) {
			createBubble(currentTertiaryColor ());
			gameManager.attQuantBubbles(bubbles.Count-1); //O -1 serve por causa da gambiarra da primeira bubble.
		}

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

	void FixedUpdate(){
		if (bubbles.Count > 0){

			int addCombo = 0;
			float posXcombo = -100;
			float posYcombo = -100;

			for (int i = bubbles.Count-1; i > 0; i--) {
				BubbleController b = bubbles [i];
				if (b.isExplodeMe ()) {

					if(b.isSecondaryColor()) {
						gameManager.addScore(GameSethings.joinBubbles.EXPLODE_SECONDARY);
						if(addCombo != 3)
							addCombo = 2;
					} else if(b.isTertiaryColor()) {
						gameManager.addScore(GameSethings.joinBubbles.EXPLODE_TERTIARY);
						posXcombo = b.transform.position.x;
						posYcombo = b.transform.position.y;
						addCombo = 3;
					} else 
						gameManager.addScore(GameSethings.joinBubbles.EXPLODE_PRIMATY);

					b.explode ();
				} else if (b.isRemoveMe()){

					if(b.isSecondaryColor())
						gameManager.addScore(GameSethings.joinBubbles.MIX_SECONDARY);
					else 
						gameManager.addScore(GameSethings.joinBubbles.MIX_PRIMATY);

					bubbles.Remove(b);
					availableBubbles.Add(b);

					//Remover boolean de cor terceárea.
					if(b.getColor().Equals("RedPurple"))
						tertiaryColors[0] = false;
					else if(b.getColor().Equals("RedOrange"))
						tertiaryColors[1] = false;
					else if(b.getColor().Equals("BluePurple"))
						tertiaryColors[2] = false;
					else if(b.getColor().Equals("YellowOrange"))
						tertiaryColors[3] = false;
					else if(b.getColor().Equals("YellowGreen"))
						tertiaryColors[4] = false;
					else if(b.getColor().Equals("BlueGreen"))
						tertiaryColors[5] = false;

					b.transform.position = new Vector3(-100,-100,0);
					b.gameObject.SetActive(false);
					gameManager.attQuantBubbles(bubbles.Count-1);
				}
				if (selectedBubble != null && b == selectedBubble)
					continue;
				b.moveMe ();
			}

			if(addCombo == 2){
				gameManager.addSecondaryCombo();
			} else if (addCombo == 3){
				gameManager.addTertiaryCombo(posXcombo, posYcombo);
			}
		}

	}

	public void showBubbles(bool show){
		foreach (BubbleController b in bubbles) {
			if(show){
				b.transform.position = new Vector3(b.transform.position.x, b.transform.position.y, 0);
			} else {
				b.transform.position = new Vector3(b.transform.position.x, b.transform.position.y, -20);
			}
		}
	}

	//Retorna true se são cores misturáveias
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

	private void errorOnMix(){
		//Cria o feedback de erro
	}

	/*Create a new bubble and add on bubbles list*/
	private void createBubble(string bubbleColor){

		if (availableBubbles.Count == 0 || bubbles.Count > maxBubbles) {
			timeToNewBubble = 1f;
			gameManager.endGame();
			return;
		}

		/*Debug.Log("Quanto bubbles "+bubbles.Count);

		if(bubbles.Count < 7)
		for (int i = 0; i < 6; i++) {
			BubbleController newBu = Instantiate (bubblePrefab, new Vector3 (0, 0, 0), Quaternion.identity) as BubbleController;
			newBu.gameObject.SetActive(true);
			if(i == 0)
				newBu.addTertiaryColor("RedPurple");
			if(i == 1)
				newBu.addTertiaryColor("RedOrange");
			if(i == 2)
				newBu.addTertiaryColor("BluePurple");
			if(i == 3)
				newBu.addTertiaryColor("YellowOrange");
			if(i == 4)
				newBu.addTertiaryColor("YellowGreen");
			if(i == 5)
				newBu.addTertiaryColor("BlueGreen");
		
			bubbles.Add (newBu);
			newBu.transform.SetParent(this.transform);
			newBu.transform.localPosition = new Vector3(0,0,0);
		}

		if(true)
			return;*/

		Vector3 bornPosition;

		int rand = Random.Range(1,9);
		if (rand == 1) {
			bornPosition = new Vector3(-8, 5, 0);
		} else if (rand == 2) {
			bornPosition = new Vector3(8, 5, 0);
		} else if (rand == 3) {
			bornPosition = new Vector3(-8, -5, 0);
		} else if (rand == 4) {
			bornPosition = new Vector3(0, 5, 0);
		} else if (rand == 5) {
			bornPosition = new Vector3(0, -5, 0);
		} else if (rand == 6) {
			bornPosition = new Vector3(8, 0, 0);
		} else if (rand == 7) {
			bornPosition = new Vector3(-8, 0, 0);
		} else {
			bornPosition = new Vector3(8, -5, 0);
		}

		rand = Random.Range(0,availableBubbles.Count);
		BubbleController newBubble = availableBubbles[rand];
		newBubble.gameObject.SetActive (true);
		newBubble.transform.position = bornPosition;
		newBubble.initMe();
		newBubble.transform.SetParent (this.transform);
		availableBubbles.Remove (newBubble);
		bubbles.Add (newBubble);

		//Se vier cor terecária colocá-la aqui.
		if (bubbleColor.Equals ("RedPurple")) {
			newBubble.changeColorTo("RedPurple");
			tertiaryColors[0] = true;
		} else if (bubbleColor.Equals ("RedOrange")) {
			newBubble.changeColorTo("RedOrange");
			tertiaryColors[1] = true;
		} else if (bubbleColor.Equals ("BluePurple")) {
			newBubble.changeColorTo("BluePurple");
			tertiaryColors[2] = true;
		} else if (bubbleColor.Equals ("YellowOrange")) {
			newBubble.changeColorTo("YellowOrange");
			tertiaryColors[3] = true;
		} else if (bubbleColor.Equals ("YellowGreen")) {
			newBubble.changeColorTo("YellowGreen");
			tertiaryColors[4] = true;
		} else if (bubbleColor.Equals ("BlueGreen")) {
			newBubble.changeColorTo("BlueGreen");
			tertiaryColors[5] = true;
		}

		if (gameManager.totalTime < 15)
			timeToNewBubble = 2.5f;
		else if (gameManager.totalTime < 30) {
			timeToNewBubble = 2.2f;
		} else if (gameManager.totalTime < 40) {
			timeToNewBubble = 2f;
		} else if (gameManager.totalTime < 50) {
			timeToNewBubble = 1.6f;
		} else if (gameManager.totalTime < 60) {
			timeToNewBubble = 1.4f;
		} else if (gameManager.totalTime < 80) {
			timeToNewBubble = 1.2f;
		} else if (gameManager.totalTime < 100) {
			timeToNewBubble = 0.9f;
		} else if (gameManager.totalTime < 130) {
			timeToNewBubble = 0.6f;
		} else if (gameManager.totalTime < 180) {
			timeToNewBubble = 1.5f;
		} else if (gameManager.totalTime < 200) {
			timeToNewBubble = 1f;
		} else if (gameManager.totalTime < 300) {
			timeToNewBubble = 0.8f;
		} else {
			timeToNewBubble = 0.6f;
		}
	}

	public void clickOnBubble(BubbleController b){
		//Pode selecionar um bubble, misturar, explodir ou errar mistura.
		//Debug.Log ("Bubble "+b.getColor()+" clicked.");

		if(b != null && mouseDownOnBubble == b){
			if(selectedBubble == null || (selectedBubble != b)){
				if(selectedBubble != null){
					bool mixBubbles = verifyMix(selectedBubble.getColor(), b.getColor());
					bool explodeBubbles = verifiExplode(selectedBubble, b);
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

	private bool verifiExplode(BubbleController bA, BubbleController bB) {
		if(bA.getColor().Equals(bB.getColor())){
			if(bA.isSecondaryColor() || bA.isTertiaryColor())
				return true;
		}
		return false;
	}

	private string currentTertiaryColor(){

		if(GameSethings.testeCombo){
			return "RedPurple";
		}

		//RedPurple, RedOrange, BluePurple, YellowOrange, YellowGreen, BlueGreen.

		if(totalTime > 15 && currentColorTertiary < 1){
			currentColorTertiary++;
			if(!tertiaryColors[0])
				return "RedPurple";
		} else if(totalTime > 25 && currentColorTertiary < 2){
			currentColorTertiary++;
			if(!tertiaryColors[1])
				return "RedOrange";
		} else if(totalTime > 35 && currentColorTertiary < 3){
			currentColorTertiary++;
			if(!tertiaryColors[2])
				return "BluePurple";
		} else if(totalTime > 40 && currentColorTertiary < 4){
			currentColorTertiary++;
			if(!tertiaryColors[3])
				return "YellowOrange";
		} else if(totalTime > 45 && currentColorTertiary < 5){
			currentColorTertiary++;
			if(!tertiaryColors[4])
				return "YellowGreen";
		} else if(totalTime > 50 && currentColorTertiary < 6){
			currentColorTertiary++;
			if(!tertiaryColors[5])
				return "BlueGreen";
		}

		//Apertar mais um pouco!
		if(totalTime > 60) {
			if(currentColorTertiary < 6){
				currentColorTertiary++;
				if(!tertiaryColors[5])
					return "BlueGreen";
			} else if(currentColorTertiary < 7){
				currentColorTertiary++;
				if(!tertiaryColors[2])
					return "BluePurple";
			} else if(currentColorTertiary < 8){
				currentColorTertiary++;
				if(!tertiaryColors[0])
					return "RedPurple";
			}
		}

		if(totalTime > 120) {
			if(currentColorTertiary == 8 || currentColorTertiary == 12 || currentColorTertiary == 16){
				currentColorTertiary++;
				if(!tertiaryColors[5])
					return "BlueGreen";
			} else if(currentColorTertiary == 9 || currentColorTertiary == 13 || currentColorTertiary == 17){
				currentColorTertiary++;
				if(!tertiaryColors[2])
					return "BluePurple";
			} else if(currentColorTertiary == 10 || currentColorTertiary == 14 || currentColorTertiary == 18){
				currentColorTertiary++;
				if(!tertiaryColors[0])
					return "RedPurple";
			}
		}

		if (totalTime > 150 && currentColorTertiary < 12) {
			currentColorTertiary = 12;
		}

		if (totalTime > 200 && currentColorTertiary < 16) {
			currentColorTertiary = 16;
		}

		//...


		return "";

	}

	public int totalBubbles(){
		return bubbles.Count;
	}

	public BubbleController getSelectedBubble(){
		return selectedBubble;
	}

	public GameSethings.colorGame colorSelectedBubble(){
		if (selectedBubble.getColor().Equals(GameSethings.colorGame.BLUE))
			return GameSethings.colorGame.BLUE;
		if (selectedBubble.getColor().Equals(GameSethings.colorGame.RED))
			return GameSethings.colorGame.RED;
		if (selectedBubble.getColor().Equals(GameSethings.colorGame.YELLOW))
			return GameSethings.colorGame.YELLOW;
		if (selectedBubble.getColor().Equals(GameSethings.colorGame.GREEN))
			return GameSethings.colorGame.GREEN;
		if (selectedBubble.getColor().Equals(GameSethings.colorGame.ORANGE))
			return GameSethings.colorGame.ORANGE;
		if (selectedBubble.getColor().Equals(GameSethings.colorGame.PURPLE))
			return GameSethings.colorGame.PURPLE;
		if (selectedBubble.getColor().Equals(GameSethings.colorGame.RED_ORANGE))
			return GameSethings.colorGame.RED_ORANGE;
		if (selectedBubble.getColor().Equals(GameSethings.colorGame.RED_PURPLE))
			return GameSethings.colorGame.RED_PURPLE;
		if (selectedBubble.getColor().Equals(GameSethings.colorGame.BLUE_GREEN))
			return GameSethings.colorGame.BLUE_GREEN;
		if (selectedBubble.getColor().Equals(GameSethings.colorGame.BLUE_PURPLE))
			return GameSethings.colorGame.BLUE_PURPLE;
		if (selectedBubble.getColor().Equals(GameSethings.colorGame.YELLOW_GREEN))
			return GameSethings.colorGame.YELLOW_GREEN;
		
		return GameSethings.colorGame.YELLOW_ORANGE;
	}

}
