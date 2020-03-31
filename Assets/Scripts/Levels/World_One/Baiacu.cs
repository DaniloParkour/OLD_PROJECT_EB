using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Baiacu : MonoBehaviour {

	public bool justTertiary;
	public BubbleLevelManager bubbleManager;
	public float maxValueIddle;
	public float velocityIddle;
	public float velocicy;
	public Vector2 verticalLimits;
	public Vector2 horizontalLimits;

	//0- Iddle		2- Livremente normal	4- Ir para um local e fazer L
	//1- Up/Down	3- Livremente rapido	5- Ir para um local e fazer Z
	public List<Vector2> myMoviment;

	private Transform sideFin;
	private Transform behindFin;
	private Transform face;
	private List<Spark> sparks_left;
	private List<Spark> sparks_right;
	private SpriteRenderer sr;
	private AuxiliarAnim anim;
	private float timeToActions;
	private float timeToHit;
	private Vector2 moveDirection;
	private bool upOnIddle;
	private float valueUpMe;
	private float timeLeftOnMove;
	private float timeToAnimaHit;
	private int quantLetfOnAnimaHit;
	private BubbleController selectedBubble;
	private bool bossLose;
	private float timeToDestroyMe;
	private int finVelocity;
	private bool flipFinUp;
	private float valueAccFinRotation;

	void Awake(){
		sr = GetComponent<SpriteRenderer> ();
	}

	// Use this for initialization
	void Start () {

		sideFin = transform.Find ("pivotSiderFin");
		behindFin = transform.Find ("pivotBehinderFin");
		face = transform.Find ("face");

		//InitSparks
		Transform all_left_sparks = transform.Find ("leftSparks");
		Transform all_right_sparks = transform.Find ("rightSparks");
		sparks_left = new List<Spark> ();
		sparks_right = new List<Spark> ();

		for (int i = 0; i < all_left_sparks.childCount; i++)
			sparks_left.Add (all_left_sparks.GetChild(i).GetComponent<Spark>());
		for (int i = 0; i < all_right_sparks.childCount; i++)
			sparks_right.Add (all_right_sparks.GetChild(i).GetComponent<Spark>());

		setSparkColors ();

		upOnIddle = true;
		valueUpMe = maxValueIddle / 2;

		anim = GetComponent<AuxiliarAnim> ();

		quantLetfOnAnimaHit = 3;
		timeToAnimaHit = -10;

		bossLose = false;
		timeToDestroyMe = -10;
		finVelocity = 1;
		valueAccFinRotation = 0;
	}
	
	// Update is called once per frame
	void Update () {

		if (bossLose) {
			timeToDestroyMe -= Time.deltaTime;
			if(timeToDestroyMe <= 0)
				Destroy (this.gameObject);
			return;
		}

		if (myMoviment [0].x == 0)
			iddleAnimation();
		else if (myMoviment [0].x == 1 && timeLeftOnMove <= 0)
			upOrDown ();
		else if (myMoviment [0].x == 2 && timeLeftOnMove <= 0)
			freeMove(false);
		else if (myMoviment [0].x == 3 && timeLeftOnMove <= 0)
			freeMove(true);

		if (myMoviment.Count > 1) {
			if(myMoviment[0].x != 2 && myMoviment[0].x != 3)
				myMoviment[0] = new Vector2(myMoviment[0].x, myMoviment[0].y - Time.deltaTime);
			if(myMoviment[0].y < 0)
				myMoviment.Remove(myMoviment[0]);
		}

		if (timeLeftOnMove > 0)
			timeLeftOnMove -= Time.deltaTime;

		if (timeToAnimaHit > 0)
			timeToAnimaHit -= Time.deltaTime;

		animaHit ();

		for (int i = sparks_left.Count-1; i >= 0; i--) {
			if(sparks_left[i].isRemoveMe()){
				Spark remove = sparks_left[i];
				sparks_left.Remove(remove);
				remove.destroyMe();
			}
		}
		for (int i = sparks_right.Count-1; i >= 0; i--) {
			if(sparks_right[i].isRemoveMe()){
				Spark remove = sparks_right[i];
				sparks_right.Remove(remove);
				remove.destroyMe();
			}
		}

		verifyClicks ();

		animaFins ();
	
	}

	public void animaOnLose(){
		if (hp () > 0 || bossLose == true)
			return;

		bossLose = true;
		if(transform.localScale.x > 0)
			anim.scaleTo (new Vector3(2,2,1), 0.5f);
		else
			anim.scaleTo (new Vector3(-2,2,1), 0.5f);
		anim.fadeTo (0, 0.5f);
		timeToDestroyMe = 0.6f;
	}

	private void animaHit(){

		if (quantLetfOnAnimaHit <= 0) {
			timeToAnimaHit = -10;
			quantLetfOnAnimaHit = 3;
			sr.material.color = new Color(sr.material.color.r, sr.material.color.g, sr.material.color.b, 1);
		}
		
		if (timeToAnimaHit <= 0 && timeToAnimaHit > -5) {
			if(sr.material.color.a == 1){
				sr.material.color = new Color(sr.material.color.r, sr.material.color.g, sr.material.color.b, 1.5f);
				timeToAnimaHit = 0.15f;
				quantLetfOnAnimaHit--;
			} else {
				sr.material.color = new Color(sr.material.color.r, sr.material.color.g, sr.material.color.b, 1);
				timeToAnimaHit = 0.2f;
			}
		}
	}

	private void verifyClicks(){
		if (Input.GetMouseButtonDown (0)) {
			RaycastHit2D hit = Physics2D.Raycast (Camera.main.ScreenToWorldPoint (Input.mousePosition), Vector2.zero, Mathf.Infinity, 1 << 9); //The layer 8 is the Enemy layer.
			if (hit.collider != null && hit.collider.gameObject == this.gameObject) {
				selectedBubble = bubbleManager.getSelectedBubble();
			}
		} else if (Input.GetMouseButtonUp(0)) {
			RaycastHit2D hit = Physics2D.Raycast (Camera.main.ScreenToWorldPoint (Input.mousePosition), Vector2.zero, Mathf.Infinity, 1 << 9); //The layer 8 is the Bubbles layer.
			if (hit.collider != null  && hit.collider.gameObject == this.gameObject) {
				if(selectedBubble != null)
					selectedBubble.goToPosition(new Vector2(transform.position.x, transform.position.y), null);
			}
			selectedBubble = null;
		}
	}

	private void iddleAnimation() {
		if (valueUpMe > maxValueIddle) {
			valueUpMe = 0;
			upOnIddle = !upOnIddle;
		}
		if (upOnIddle)
			transform.Translate (0, velocityIddle * Time.deltaTime, 0);
		else
			transform.Translate (0, -velocityIddle * Time.deltaTime, 0);

		valueUpMe += velocityIddle * Time.deltaTime;
	}

	private void upOrDown() {

		Vector3 pos = transform.position;
		
		//Deslocamentos de em X e Y de (7 e 3.5)
		int rand = Random.Range (0, 2);
		if(rand == 0)
			anim.moveTo(new Vector3(pos.x, 3.5f, pos.z), myMoviment[0].y);
		else
			anim.moveTo(new Vector3(pos.x, -3.5f, pos.z), myMoviment[0].y);

		timeLeftOnMove = myMoviment [0].y;
		
		myMoviment[0] = new Vector2(myMoviment[0].x, -1);
	}

	private void freeMove(bool fast) {

		Vector3 pos = new Vector3(Random.Range (horizontalLimits.x, horizontalLimits.y), Random.Range (verticalLimits.x, verticalLimits.y), 0);
		if(!fast)
			anim.moveTo(new Vector3(pos.x, pos.y, pos.z), velocicy);
		else
			anim.moveTo(new Vector3(pos.x, pos.y, pos.z), velocicy/2);

		if (transform.position.x >= pos.x) {
			transform.localScale = new Vector3 (Mathf.Abs(transform.localScale.x), transform.localScale.y, 1);
			foreach(Spark s in sparks_left)
				s.setRendererPosition(15);
			foreach(Spark s in sparks_right)
				s.setRendererPosition(5);
		} else {
			transform.localScale = new Vector3 (-Mathf.Abs(transform.localScale.x), transform.localScale.y, 1);
			//invert = false;
			foreach(Spark s in sparks_left)
				s.setRendererPosition(5);
			foreach(Spark s in sparks_right)
				s.setRendererPosition(15);
		}

		timeLeftOnMove = velocicy;
		
		if(myMoviment.Count > 1)
			myMoviment[0] = new Vector2(myMoviment[0].x, myMoviment[0].y - timeLeftOnMove);

	}

	private void animaFins() {

		//x -0.25 point axis angle
		Vector3 v = behindFin.transform.position;

		if (flipFinUp) {
			behindFin.transform.Rotate(0,5 * finVelocity,0);
			sideFin.transform.Rotate(5*finVelocity,0,0);
			valueAccFinRotation += 5 * finVelocity;
		} else {
			behindFin.transform.Rotate(0,-5 * finVelocity,0);
			sideFin.transform.Rotate(-5*finVelocity,0,0);
			valueAccFinRotation -= 5 * finVelocity;
		}

		if (valueAccFinRotation > 40/finVelocity && flipFinUp)
			flipFinUp = false;
		else if (valueAccFinRotation < -40/finVelocity && !flipFinUp)
			flipFinUp = true;


	}

	public int hp(){
		return (sparks_left.Count + sparks_right.Count);
	}

	public void hitMe(BubbleController bubble){
		bool broken = false;

		if (bubble.isMovingTo () && bubble.get_bubble_b() == null) {
			foreach (Spark s in sparks_left) {
				if (GameSethings.getColorName (s.getColor ()).Equals (bubble.getColor ())) {
					s.toRemove ();
					broken = true;
					break;
				}
			}

			if (!broken) {
				foreach (Spark s in sparks_right) {
					if (GameSethings.getColorName (s.getColor ()).Equals (bubble.getColor ())) {
						s.toRemove ();
						broken = true;
						break;
					}
				}
			}
		}

		if (broken)
			timeToAnimaHit = 0.1f;

		bubble.explode ();

	}

	public void setSparkColors(){
		int rand = 0;
		int initialValue = 0;
		if (justTertiary)
			initialValue = 3;
		for (int i = 0; i < sparks_left.Count; i++) {
			rand = Random.Range (initialValue, 15);

			//Secondary
			if(rand == 0)
				sparks_left[i].setColorTo(GameSethings.colorGame.ORANGE);
			else if(rand == 1)
				sparks_left[i].setColorTo(GameSethings.colorGame.GREEN);
			else if(rand == 2)
				sparks_left[i].setColorTo(GameSethings.colorGame.PURPLE);

			//Tertiary
			else if(rand == 3 || rand == 4)
				sparks_left[i].setColorTo(GameSethings.colorGame.RED_ORANGE);
			else if(rand == 5 || rand == 6)
				sparks_left[i].setColorTo(GameSethings.colorGame.RED_PURPLE);
			else if(rand == 7 || rand == 8)
				sparks_left[i].setColorTo(GameSethings.colorGame.YELLOW_GREEN);
			else if(rand == 9 || rand == 10)
				sparks_left[i].setColorTo(GameSethings.colorGame.YELLOW_ORANGE);
			else if(rand == 11 || rand == 12)
				sparks_left[i].setColorTo(GameSethings.colorGame.BLUE_GREEN);
			else if(rand == 13 || rand == 14)
				sparks_left[i].setColorTo(GameSethings.colorGame.BLUE_PURPLE);
		}

		for (int i = 0; i < sparks_right.Count; i++) {
			rand = Random.Range (initialValue, 15);
			
			//Secondary
			if(rand == 0)
				sparks_right[i].setColorTo(GameSethings.colorGame.ORANGE);
			else if(rand == 1)
				sparks_right[i].setColorTo(GameSethings.colorGame.GREEN);
			else if(rand == 2)
				sparks_right[i].setColorTo(GameSethings.colorGame.PURPLE);
			
			//Tertiary
			else if(rand == 3 || rand == 4)
				sparks_right[i].setColorTo(GameSethings.colorGame.RED_ORANGE);
			else if(rand == 5 || rand == 6)
				sparks_right[i].setColorTo(GameSethings.colorGame.RED_PURPLE);
			else if(rand == 7 || rand == 8)
				sparks_right[i].setColorTo(GameSethings.colorGame.YELLOW_GREEN);
			else if(rand == 9 || rand == 10)
				sparks_right[i].setColorTo(GameSethings.colorGame.YELLOW_ORANGE);
			else if(rand == 11 || rand == 12)
				sparks_right[i].setColorTo(GameSethings.colorGame.BLUE_GREEN);
			else if(rand == 13 || rand == 14)
				sparks_right[i].setColorTo(GameSethings.colorGame.BLUE_PURPLE);
		}

	}

}
