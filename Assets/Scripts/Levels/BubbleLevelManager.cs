using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class BubbleLevelManager : MonoBehaviour {

	public BubbleController bubblePrefab;
	public int maxBubbles;
	public Image quantBubbles;

	private List<BubbleController> bubbles;
	private BubbleController mouseDownOnBubble;
	private BubbleController selectedBubble;
	private bool endLevel;
	private GameObject initialWindow;
	private Button bu_pause;

	// Use this for initialization
	void Awake () {
		bubbles = new List<BubbleController> ();
		/*bubbles.Add (Instantiate (bubblePrefab,new Vector3(-100,-100,0), Quaternion.identity) as BubbleController);
		bubbles [0].transform.parent = this.transform;
		endLevel = false;*/
	}

	void Start(){
		if(maxBubbles > 0)
			quantBubbles.fillAmount = (bubbles.Count)/maxBubbles;
		else
			quantBubbles.fillAmount = 0;

		initialWindow = quantBubbles.transform.parent.Find("initialWindow").gameObject;
		bu_pause = quantBubbles.transform.Find ("PauseButton").gameObject.GetComponent<Button>();

		if (initialWindow.activeSelf)
			bu_pause.enabled = false;
		else
			bu_pause.enabled = true;
	}
	
	// Update is called once per frame
	void Update () {

		//Gamb _________________________________________________________
		if (!initialWindow.activeSelf && !bu_pause.enabled && !endLevel)
			bu_pause.enabled = true;
		//""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""

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
		for(int i = bubbles.Count-1; i >= 0; i--){
			if(bubbles[i].transform.position.x > -50)
				bubbles[i].moveMe ();
			if(bubbles[i].isExplodeMe())
				bubbles[i].explode();
			if(bubbles[i].isRemoveMe()){
				BubbleController b = bubbles[i];
				bubbles.Remove(b);
				Destroy(b.gameObject);
			}
		}
	}

	public void addBubble(BubbleController b){
		if (!endLevel) {
			bubbles.Add (b);
		}
		if(maxBubbles > 0)
			quantBubbles.fillAmount = (bubbles.Count)/(float)maxBubbles;
	}

	public List<BubbleController> getBubbles(){
		return bubbles;
	}

	public void setEndLevel(bool value){
		quantBubbles.transform.Find ("PauseButton").gameObject.GetComponent<Button>().enabled = false;
		endLevel = value;
	}

	public bool isEndLevel(){
		return endLevel;
	}

	private void clickOnBubble(BubbleController b){
		if(b != null && mouseDownOnBubble == b){
			if(selectedBubble == null || (selectedBubble != b)){
				if(selectedBubble != null){
					//OBS: Colocar um IF (bubble não é FIXA).
					selectedBubble.goToPosition(new Vector2(b.transform.position.x, b.transform.position.y), b);
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

	public BubbleController getSelectedBubble(){
		return selectedBubble;
	}

	public GameSethings.colorGame getColorSelectedBubble(){
		if (selectedBubble.getColor().Equals(GameSethings.getColorName(GameSethings.colorGame.BLUE)))
			return GameSethings.colorGame.BLUE;
		if (selectedBubble.getColor().Equals(GameSethings.getColorName(GameSethings.colorGame.RED)))
			return GameSethings.colorGame.RED;
		if (selectedBubble.getColor().Equals(GameSethings.getColorName(GameSethings.colorGame.YELLOW)))
			return GameSethings.colorGame.YELLOW;
		if (selectedBubble.getColor().Equals(GameSethings.getColorName(GameSethings.colorGame.GREEN)))
			return GameSethings.colorGame.GREEN;
		if (selectedBubble.getColor().Equals(GameSethings.getColorName(GameSethings.colorGame.ORANGE)))
			return GameSethings.colorGame.ORANGE;
		if (selectedBubble.getColor().Equals(GameSethings.getColorName(GameSethings.colorGame.PURPLE)))
			return GameSethings.colorGame.PURPLE;
		if (selectedBubble.getColor().Equals(GameSethings.getColorName(GameSethings.colorGame.RED_ORANGE)))
			return GameSethings.colorGame.RED_ORANGE;
		if (selectedBubble.getColor().Equals(GameSethings.getColorName(GameSethings.colorGame.RED_PURPLE)))
			return GameSethings.colorGame.RED_PURPLE;
		if (selectedBubble.getColor().Equals(GameSethings.getColorName(GameSethings.colorGame.BLUE_GREEN)))
			return GameSethings.colorGame.BLUE_GREEN;
		if (selectedBubble.getColor().Equals(GameSethings.getColorName(GameSethings.colorGame.BLUE_PURPLE)))
			return GameSethings.colorGame.BLUE_PURPLE;
		if (selectedBubble.getColor().Equals(GameSethings.getColorName(GameSethings.colorGame.YELLOW_GREEN)))
			return GameSethings.colorGame.YELLOW_GREEN;

		return GameSethings.colorGame.YELLOW_ORANGE;
	}

}
