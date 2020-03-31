using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class BubblesGenerator : MonoBehaviour {

	public string colors;

	/*Valor de X é o tempo para a próxima bolha e valor de Y é o tempo para mudar para o próximo X*/
	public List<Vector2> timeToBubbles;
	public BubbleController bubblePrefab;
	public Vector3[] bornPositions;
	public GameSethings.levelType typeLevel;

	private BubbleLevelManager bubbleManager;
	private int currentBubble;
	private string[] cores;
	private float timeToCreateBubble;
	// Use this for initialization
	void Start () {

		/*
		cores = colors.Split (' ');
		bubbles = new List<BubbleController> ();
		bubbles.Add (Instantiate (bubblePrefab, new Vector3 (-100, -100, 0), Quaternion.identity) as BubbleController);
		currentBubble = 0;
		timeToCreateBubble = timesToBubbles [0];
		currentTime = 1;
		if (timesToBubbles.Length > 0)
			changeTime = cores.Length / timesToBubbles.Length;
		*/

		bubbleManager = GetComponent<BubbleLevelManager> ();
		if (typeLevel.Equals (GameSethings.levelType.LET_PRIMARY)) {
			timeToCreateBubble = timeToBubbles[0].x;
			timeToBubbles[0] = new Vector2(timeToBubbles[0].x, timeToBubbles[0].y-timeToBubbles[0].x);

		} else if (typeLevel.Equals(GameSethings.levelType.SURVIVOR) || typeLevel.Equals(GameSethings.levelType.BAIACU)){
			timeToCreateBubble = timeToBubbles[0].x;
			timeToBubbles[0] = new Vector2(timeToBubbles[0].x, timeToBubbles[0].y-timeToBubbles[0].x);
		}

	}
	
	// Update is called once per frame
	void Update () {

		/*if (timeToCreateBubble <= 0 && currentBubble < colors.Length) {
			addBubble();
			timeToCreateBubble = timesToBubbles[currentTime];
			//changeTime = colors.Length / timesToBubbles.Length;
		}*/

		//quantBubbles.fillAmount = ;

		if (timeToCreateBubble > 0)
			timeToCreateBubble -= Time.deltaTime;



		if (typeLevel.Equals (GameSethings.levelType.LET_PRIMARY) || typeLevel.Equals (GameSethings.levelType.SURVIVOR)
		    || typeLevel.Equals (GameSethings.levelType.BAIACU)) {
			if (timeToBubbles.Count > 1 && timeToBubbles [0].y < 0) {
				timeToBubbles.Remove (timeToBubbles [0]);
			}
			if (timeToCreateBubble <= 0) {
				if(typeLevel.Equals (GameSethings.levelType.LET_PRIMARY))
					createLetPrimaryBubble ();
				else if(typeLevel.Equals (GameSethings.levelType.SURVIVOR))
					createRandBubble ();
				else if(typeLevel.Equals (GameSethings.levelType.BAIACU))
					createRandPrimary ();
				timeToCreateBubble = timeToBubbles [0].x;
				if (timeToBubbles.Count > 1)
					timeToBubbles [0] = new Vector2 (timeToBubbles [0].x, timeToBubbles [0].y - timeToBubbles [0].x);
			}
		}

	}

	public void createRandPrimary(){
		if (bubbleManager.isEndLevel ())
			return;

		createRandBubble ();

		int rand = Random.Range (0, 3);
		if(rand == 0)
			changeColorTo (bubbleManager.getBubbles()[bubbleManager.getBubbles().Count-1], "R");
		else if(rand == 1)
			changeColorTo (bubbleManager.getBubbles()[bubbleManager.getBubbles().Count-1], "Y");
		else if(rand == 2)
			changeColorTo (bubbleManager.getBubbles()[bubbleManager.getBubbles().Count-1], "B");
	}

	public void createRandBubble(){
		/*
		BubbleController newBubble = Instantiate (bubblePrefab,pos, Quaternion.identity) as BubbleController;
		newBubble.gameObject.SetActive (true);
		newBubble.initMe();
		newBubble.transform.SetParent (this.transform);
		bubbleManager.addBubble (newBubble);
		changeColorTo (newBubble, color);
		*/

		if (bubbleManager.isEndLevel ())
			return;
		
		int pos = Random.Range(0,bornPositions.Length);
		BubbleController newBubble = Instantiate (bubblePrefab, bornPositions[pos], Quaternion.identity) as BubbleController;
		
		newBubble.gameObject.SetActive (true);
		newBubble.initMe();
		newBubble.transform.SetParent (this.transform);
		bubbleManager.addBubble (newBubble);
		
		int color = Random.Range (0, 90);
		
		if(color < 10)
			changeColorTo (newBubble, "R");
		else if(color < 20)
			changeColorTo (newBubble, "B");
		else if(color < 30)
			changeColorTo (newBubble, "Y");
		else if(color < 40)
			changeColorTo (newBubble, "G");
		else if(color < 50)
			changeColorTo (newBubble, "P");
		else if(color < 60)
			changeColorTo (newBubble, "O");
		else if(color < 65)
			changeColorTo (newBubble, "YO");
		else if(color < 70)
			changeColorTo (newBubble, "YG");
		else if(color < 75)
			changeColorTo (newBubble, "RO");
		else if(color < 80)
			changeColorTo (newBubble, "RP");
		else if(color < 85)
			changeColorTo (newBubble, "BG");
		else if(color < 90)
			changeColorTo (newBubble, "BP");

	}

	private void createLetPrimaryBubble() {

		if (bubbleManager.isEndLevel ())
			return;

		int pos = Random.Range(0,bornPositions.Length);
		BubbleController newBubble = Instantiate (bubblePrefab, bornPositions[pos], Quaternion.identity) as BubbleController;

		newBubble.gameObject.SetActive (true);
		newBubble.initMe();
		newBubble.transform.SetParent (this.transform);
		bubbleManager.addBubble (newBubble);

		int color = Random.Range (0, 31);

		if(color < 8)
			changeColorTo (newBubble, "R");
		else if(color < 16)
			changeColorTo (newBubble, "B");
		else if(color < 24)
			changeColorTo (newBubble, "Y");
		else if(color < 26)
			changeColorTo (newBubble, "G");
		else if(color < 28)
			changeColorTo (newBubble, "P");
		else if(color < 30)
			changeColorTo (newBubble, "O");
	}
	
	private void addBubble () {
		/*
		if (currentBubble >= colors.Length)
			return;

		int pos = Random.Range(0,bornPositions.Length);

		BubbleController newBubble = Instantiate (bubblePrefab,bornPositions[pos], Quaternion.identity) as BubbleController;
		newBubble.gameObject.SetActive (true);
		newBubble.initMe();
		newBubble.transform.SetParent (this.transform);
		changeColorTo (newBubble, colors[currentBubble]+"");
		currentBubble++;
		*/
	}

	private void changeColorTo(BubbleController b, string color) {
		if(color.Equals("R")){
			b.changeColorTo("Red");
		} else if(color.Equals("B")){
			b.changeColorTo("Blue");
		} else if(color.Equals("Y")){
			b.changeColorTo("Yellow");
		} else if(color.Equals("G")){
			b.changeColorTo("Green");
		} else if(color.Equals("P")){
			b.changeColorTo("Purple");
		} else if(color.Equals("O")){
			b.changeColorTo("Orange");
		} else if(color.Equals("RO")){
			b.changeColorTo("RedOrange");
		} else if(color.Equals("RP")){
			b.changeColorTo("RedPurple");
		} else if(color.Equals("BG")){
			b.changeColorTo("BlueGreen");
		} else if(color.Equals("BP")){
			b.changeColorTo("BluePurple");
		} else if(color.Equals("YG")){
			b.changeColorTo("YellowGreen");
		} else if(color.Equals("YO")){
			b.changeColorTo("YellowOrange");
		} else {
			//Se der algum erro fica mais fácil de ver
			b.transform.localScale = new Vector3(0.3f, 0.3f, 1);
		}
	}
}
