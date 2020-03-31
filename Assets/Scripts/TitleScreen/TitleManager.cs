using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TitleManager : MonoBehaviour {

	public GameObject windowPaleta;
	public GameObject windowConfiguration;
	public GameObject credits;
	public AuxiliarAnim fadePlane;

	public Image colorAtuto;
	public Image colorBtuto;
	public Image colorCtuto;

	// Use this for initialization
	void Start () {
		Time.timeScale = 1;
		GameSethings.isWindowOpen = false;
		GameSethings.language = "PT_BR";
		GameSethings.player_level = 17;
		attTexts ();
	}
	
	// Update is called once per frame
	void Update () {
		//Debug.Log("Janela aberta: "+GameSethings.isWindowOpen):
		//if(GameSethings.isWindowOpen)
		//	Debug.Log ("Janela aberta!");
	}

	public void openWindow(Window window){
		if (window.gameObject.name.Equals ("creditsPanel")) {
			GameSethings.isWindowOpen = false;
			windowPaleta.SetActive(false);
			windowConfiguration.SetActive(false);
		}
		if (!GameSethings.isWindowOpen) {
			GameSethings.isWindowOpen = true;
			window.gameObject.SetActive(true);
		}
	}

	public void closeWindow(Window window){
		GameSethings.isWindowOpen = false;
		window.gameObject.SetActive (false);
	}

	public void playGame(){
		fadePlane.gameObject.SetActive (true);
		fadePlane.gameObject.GetComponent<SpriteRenderer> ().color = new Color (0,0,0,0);
		fadePlane.fadeTo (1, 0.4f);

		Application.LoadLevelAsync("LevelsScene");
	}

	public void playSurvivorGame(){
		fadePlane.gameObject.SetActive (true);
		fadePlane.gameObject.GetComponent<SpriteRenderer> ().color = new Color (0,0,0,0);
		fadePlane.fadeTo (1, 0.4f);
		
		Toggle[] toggles = FindObjectsOfType<Toggle> ();
		for (int i = 0; i < toggles.Length; i++) {
			if(toggles[i].name.Equals("TesteCombo")){
				GameSethings.testeCombo = toggles[i].isOn;
			}
		}
		
		Application.LoadLevelAsync("GameScene");
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

	public void tutorial(){
		if(!GameSethings.isWindowOpen)
			Application.LoadLevel ("TutorialScene");
	}

	public void exitGame(){
		Application.Quit ();
	}

	public void attTexts(){
		//windowPaleta.transform.FindChild("title").gameObject.GetComponent<Text> ().text = (new GameSethings()).getText(GameSethings.textsGame.SELECT_COLOR);
		windowPaleta.transform.Find("title").gameObject.GetComponent<Text> ().text = GameSethings.getText(GameSethings.textsGame.SELECT_COLOR);
	}

}
