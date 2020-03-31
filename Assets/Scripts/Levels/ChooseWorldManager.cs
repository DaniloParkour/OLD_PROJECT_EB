using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ChooseWorldManager : MonoBehaviour {

	public GameObject canvasWorldSpace;
	public CloudGeneration cloudGeneration;

	private int currentWorld;
	private List<Image> buttonsLevelOne;
	private List<Image> buttonsWorldZero;

	// Use this for initialization
	void Start () {

		//DELETAR DEPOIS
		GameSethings.player_level = 15;

		buttonsLevelOne = new List<Image>();
		buttonsWorldZero = new List<Image> ();

		Transform t_w0 = canvasWorldSpace.transform.Find ("worldZero");
		for(int i = 0; i < t_w0.childCount; i++) {
			if(t_w0.GetChild(i).gameObject.name.Contains("bu_"))
				buttonsWorldZero.Add(t_w0.GetChild(i).gameObject.GetComponent<Image>());
		}

		Transform t_w1 = canvasWorldSpace.transform.Find ("worldOne");
		for(int i = 0; i < t_w1.childCount; i++) {
			if(t_w1.GetChild(i).gameObject.name.Contains("bu_"))
				buttonsLevelOne.Add(t_w1.GetChild(i).gameObject.GetComponent<Image>());
		}

		int cont = 1;
		foreach(Image im in buttonsWorldZero) {
			if(cont > GameSethings.player_level) {
				im.color = new Color(0.7f, 0.7f, 0.7f);
				im.gameObject.GetComponent<Button>().enabled = false;
			} else {
				if(cont+1 <= GameSethings.player_level)
					im.color = new Color(0.7f, 1f, 0.7f);
				im.gameObject.GetComponent<Button>().enabled = true;
			}
			
			cont++;
		}

		cont = 13;
		foreach(Image im in buttonsLevelOne){
			if(cont > GameSethings.player_level){
				if(!im.gameObject.name.Equals("bu_level_fiveteen"))
					im.color = new Color(0.7f, 0.7f, 0.7f);
				im.gameObject.GetComponent<Button>().enabled = false;
			} else {
				if(!im.gameObject.name.Equals("bu_level_fiveteen")) {
					im.color = new Color(1f, 1f, 1f);
					if(cont+1 <= GameSethings.player_level)
						im.color = new Color(0.7f, 1f, 0.7f);
				}
				if(im.gameObject.name.Equals("bu_level_fiveteen") && (cont+1 <= GameSethings.player_level))
					im.transform.Find("sparks").gameObject.SetActive(false);

				im.gameObject.GetComponent<Button>().enabled = true;
			}

			cont++;
		}

		if(GameSethings.player_level <= 12){
			currentWorld = 0; //0
		} else if(GameSethings.player_level <= 42) {
			currentWorld = 1; //600
		} else if(GameSethings.player_level <= 72) {
			currentWorld = 2; //1200
		} else if(GameSethings.player_level <= 102) {
			currentWorld = 3; //1800
		}

		//DELETAR DEPOIS
		currentWorld = 0;

		Vector3 v = canvasWorldSpace.transform.position;
		canvasWorldSpace.transform.position = new Vector3 (0 - 18*currentWorld, v.y, v.z);

		if (currentWorld != 1)
			cloudGeneration.gameObject.SetActive(false);

	}
	
	// Update is called once per frame
	void Update () {

	}

	public void home(){
		Application.LoadLevel ("TitleScreen");
	}

	public void worldOneOpenLevel(int level){

		//WorldZero
		if (level == 1)
			Application.LoadLevel ("Level01_W0");
		else if (level == 2)
			Application.LoadLevel ("Level02_W0");
		else if (level == 3)
			Application.LoadLevel ("Level03_W0");
		else if (level == 4)
			Application.LoadLevel ("Level04_W0");
		else if (level == 5)
			Application.LoadLevel ("Level05_W0");
		else if (level == 6)
			Application.LoadLevel ("Level06_W0");
		else if (level == 7)
			Application.LoadLevel ("Level07_W0");
		else if (level == 8)
			Application.LoadLevel ("Level08_W0");
		else if (level == 9)
			Application.LoadLevel ("Level09_W0");
		else if (level == 10)
			Application.LoadLevel ("Level10_W0");
		else if (level == 11)
			Application.LoadLevel ("Level11_W0");
		else if (level == 12)
			Application.LoadLevel ("Level12_W0");

		//WorldOne
		else if (level == 13)
			Application.LoadLevel ("LevelOne_W1");
		else if (level == 14)
			Application.LoadLevel ("LevelTwo_W1");
		else if (level == 15)
			Application.LoadLevel ("LevelThree_W1");
		else if (level == 16)
			Application.LoadLevel ("LevelFour_W1");
		else if (level == 17)
			Application.LoadLevel ("LevelFive_W1");
		else if (level == 18)
			Application.LoadLevel ("LevelSix_W1");
		else if (level == 19)
			Application.LoadLevel ("LevelSeven_W1");
		else if (level == 20)
			Application.LoadLevel ("LevelEight_W1");
		else if (level == 21)
			Application.LoadLevel ("LevelNine_W1");
		else if (level == 22)
			Application.LoadLevel ("LevelTen_W1");
		else if (level == 23)
			Application.LoadLevel ("LevelEleven_W1");
		else if (level == 24)
			Application.LoadLevel ("LevelTwelve_W1");
		else if (level == 25)
			Application.LoadLevel ("LevelThirteen_W1");
		else if (level == 26)
			Application.LoadLevel ("LevelFourteen_W1");
		else if (level == 27)
			Application.LoadLevel ("LevelBaiacu_W1");
		else if (level == 28)
			Application.LoadLevel ("Level16_W1");
		else if (level == 29)
			Application.LoadLevel ("Level17_W1");
	}

	public void nextWorld() {

		Vector3 v = canvasWorldSpace.transform.position;
		if (currentWorld < GameSethings.getMaxWorld ()) {
			canvasWorldSpace.transform.position = new Vector3 (v.x - 18, v.y, v.z);
			currentWorld++;
		} else {
			canvasWorldSpace.transform.position = new Vector3 (0, v.y, v.z);
			currentWorld = 0;
		}

		if (currentWorld == 1)
			cloudGeneration.gameObject.SetActive(true);
		else
			cloudGeneration.gameObject.SetActive(false);
	}

	public void previewWorld() {
		Vector3 v = canvasWorldSpace.transform.position;
		if (currentWorld > 0) {
			canvasWorldSpace.transform.position = new Vector3 (v.x + 18, v.y, v.z);
			currentWorld--;
		} else {
			canvasWorldSpace.transform.position = new Vector3 (0 - 18*GameSethings.getMaxWorld (), v.y, v.z);
			currentWorld = GameSethings.getMaxWorld ();
		}

		if (currentWorld == 1)
			cloudGeneration.gameObject.SetActive(true);
		else
			cloudGeneration.gameObject.SetActive(false);
	}

}
