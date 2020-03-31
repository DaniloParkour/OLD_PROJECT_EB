using UnityEngine;
using System.Collections;

public class BonusController : MonoBehaviour {

	private AuxiliarAnim anim;
	private SpriteRenderer sr;
	private float timeToWait;
	private Vector3 goToPosition;
	private bool actived;
	private Vector3 initialPosition;

	// Use this for initialization
	void Start () {
		anim = GetComponent<AuxiliarAnim> ();
		sr = GetComponent<SpriteRenderer> ();
		timeToWait = -10;
		initialPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		if (timeToWait > 0)
			timeToWait -= Time.deltaTime;
		if(timeToWait <= 0 && timeToWait > -5){
			anim.moveTo(goToPosition, 0.2f);
			anim.fadeTo(0,0.3f);
			timeToWait = -10;
		}

		if (actived && sr.color.a <= 0.001f) {
			resetMe();
		}
	}

	public void initMe(Vector3 pos, Vector3 goTo){
		Debug.Log ("InitMe "+name+". To ("+goTo.x+", "+goTo.y+").");
		goToPosition = goTo;
		transform.position = pos;
		anim.moveTo (new Vector3(pos.x, pos.y+0.5f, pos.z),0.7f);
		timeToWait = 0.7f;
		actived = true;

		//Init rand color
		int rand = Random.Range (0,12);
		if(rand == 0){
			sr.color = GameSethings.getColor(GameSethings.colorGame.BLUE);
		} else if(rand == 1){
			sr.color = GameSethings.getColor(GameSethings.colorGame.YELLOW);
		} else if(rand == 2){
			sr.color = GameSethings.getColor(GameSethings.colorGame.RED);
		} else if(rand == 3){
			sr.color = GameSethings.getColor(GameSethings.colorGame.ORANGE);
		} else if(rand == 4){
			sr.color = GameSethings.getColor(GameSethings.colorGame.PURPLE);
		} else if(rand == 5){
			sr.color = GameSethings.getColor(GameSethings.colorGame.GREEN);
		} else if(rand == 6){
			sr.color = GameSethings.getColor(GameSethings.colorGame.BLUE_GREEN);
		} else if(rand == 7){
			sr.color = GameSethings.getColor(GameSethings.colorGame.BLUE_PURPLE);
		} else if(rand == 8){
			sr.color = GameSethings.getColor(GameSethings.colorGame.YELLOW_GREEN);
		} else if(rand == 9){
			sr.color = GameSethings.getColor(GameSethings.colorGame.YELLOW_ORANGE);
		} else if(rand == 10){
			sr.color = GameSethings.getColor(GameSethings.colorGame.RED_ORANGE);
		} else if(rand == 11){
			sr.color = GameSethings.getColor(GameSethings.colorGame.RED_PURPLE);
		} 
		//sr.color = new Color (sr.color.r, sr.color.g, sr.color.b, 0);
	}

	private void resetMe(){
		actived = false;
		transform.position = initialPosition;
		sr.color = new Color (sr.color.r, sr.color.g, sr.color.b, 1);
	}
}
