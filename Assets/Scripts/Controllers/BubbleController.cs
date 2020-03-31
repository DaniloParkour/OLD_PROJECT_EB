using UnityEngine;
using System.Collections;

public class BubbleController : MonoBehaviour {

	private bool selected;
	private string color;
	private SpriteRenderer sr;
	private SpriteRenderer sr_bright;
	private AuxiliarAnim anim_bright;
	private Vector2 forceDirection;
	private bool fixedBubble;
	private Vector3 initialPosition;

	//private float forceMove;
	private Rigidbody2D rigidbody;
	private AuxiliarAnim anim;
	private bool movingTo;
	private float timeToMovingTo;
	private bool explodeMe;
	private bool removeMe;

	private BubbleController bubble_b;
	private float timeToRemoveMe;
	private float timeToNewBright;

	// Use this for initialization
	void Awake () {
		sr = GetComponent<SpriteRenderer> ();
		rigidbody = GetComponent<Rigidbody2D> ();
		anim = GetComponent<AuxiliarAnim> ();
		fixedBubble = false;
	}

	void Start () {
		initialPosition = transform.localPosition;
	}
	
	// Update is called once per frame
	void Update () {
		if (timeToMovingTo >= 0)
			timeToMovingTo -= Time.deltaTime;
		else if (movingTo)
			movingTo = false;
	}

	void FixedUpdate() {
		if (timeToRemoveMe > 0)
			timeToRemoveMe -= Time.deltaTime;
		if (timeToRemoveMe <= 0 && timeToRemoveMe > -5 && !removeMe) {
			removeMe = true;
			//Debug.Log("RemoveMe agora é TRUE em bubble "+color+".");
			timeToRemoveMe = -10;
		}

		//if(removeMe)
		//	Debug.Log("RemoveMe está sendo TRUE em bubble "+color+".");
		
		//Debug.Log("Time to remove "+timeToRemoveMe);

		if (!(transform.position.x <= -20)) {
			timeToNewBright -= Time.deltaTime;
				if(sr_bright.color.a <= 0 && timeToNewBright <= 0){
					initBright();
				}
		}

		if (fixedBubble && !transform.localPosition.Equals (initialPosition)) {
			transform.localPosition = initialPosition;
			//Debug.Log("AjustPosition!");
		}
	}

	void OnCollisionEnter2D(Collision2D coll) {
		//if (!movingTo || (coll != null && coll.gameObject != bubble_b.gameObject))
		if (!movingTo || (bubble_b != null && coll.gameObject != bubble_b.gameObject))
			forceDirection = new Vector2 (Random.Range (-2f, 2f), Random.Range (-2f, 2f));

		//Verify hit on Baiacu
		Baiacu baiacu = coll.gameObject.GetComponent<Baiacu> ();
		Spark spark = coll.gameObject.GetComponent<Spark> ();
		if(baiacu != null){
			baiacu.hitMe(this);
			explodeMe = true;
		} else if(spark != null){

			//Baiacu is the grandfather of spark. The Spark is son of the Spark Side Group.
			if(spark.transform.parent != null) //Verifica se ele não é um espinho arrancado.
				spark.transform.parent.parent.gameObject.GetComponent<Baiacu>().hitMe(this);
			//""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""""

			explodeMe = true;
		}
	}

	void OnCollisionStay2D(Collision2D coll) {

		if (coll == null || bubble_b == null)
			return;

		if(coll.gameObject == bubble_b.gameObject){
			if(bubble_b.getColor().Equals(color) && (isSecondaryColor() || isTertiaryColor())){
				explodeMe = true;
				forceDirection = Vector2.zero;
				rigidbody.velocity = forceDirection;
				bubble_b.setExplodeMe(true);
				bubble_b.gameObject.GetComponent<CircleCollider2D>().enabled = false;
				GetComponent<CircleCollider2D>().enabled = false;
			} else {
				if(color.Equals("Red") && bubble_b.getColor().Equals("Blue") || color.Equals("Blue") && bubble_b.getColor().Equals("Red")){
					changeColorTo("Purple");
					bubble_b.removeMeFromScene();
					bubble_b.gameObject.GetComponent<CircleCollider2D>().enabled = false;
				} else if(color.Equals("Red") && bubble_b.getColor().Equals("Yellow") || color.Equals("Yellow") && bubble_b.getColor().Equals("Red")){
					changeColorTo("Orange");
					bubble_b.removeMeFromScene();
					bubble_b.gameObject.GetComponent<CircleCollider2D>().enabled = false;
				} else if(color.Equals("Blue") && bubble_b.getColor().Equals("Yellow") || color.Equals("Yellow") && bubble_b.getColor().Equals("Blue")){
					changeColorTo("Green");
					bubble_b.removeMeFromScene();
					bubble_b.gameObject.GetComponent<CircleCollider2D>().enabled = false;
				} 
				//RedPurple, RedOrange, BluePurple, YellowOrange, YellowGreen, BlueGreen.
				else if(color.Equals("Red") && bubble_b.getColor().Equals("Purple") || color.Equals("Purple") && bubble_b.getColor().Equals("Red")){
					changeColorTo("RedPurple");
					bubble_b.removeMeFromScene();
					bubble_b.gameObject.GetComponent<CircleCollider2D>().enabled = false;
				} else if(color.Equals("Red") && bubble_b.getColor().Equals("Orange") || color.Equals("Orange") && bubble_b.getColor().Equals("Red")){
					changeColorTo("RedOrange");
					bubble_b.removeMeFromScene();
					bubble_b.gameObject.GetComponent<CircleCollider2D>().enabled = false;
				} else if(color.Equals("Blue") && bubble_b.getColor().Equals("Purple") || color.Equals("Purple") && bubble_b.getColor().Equals("Blue")){
					changeColorTo("BluePurple");
					bubble_b.removeMeFromScene();
					bubble_b.gameObject.GetComponent<CircleCollider2D>().enabled = false;
				} else if(color.Equals("Yellow") && bubble_b.getColor().Equals("Orange") || color.Equals("Orange") && bubble_b.getColor().Equals("Yellow")){
					changeColorTo("YellowOrange");
					bubble_b.removeMeFromScene();
					bubble_b.gameObject.GetComponent<CircleCollider2D>().enabled = false;
				} else if(color.Equals("Green") && bubble_b.getColor().Equals("Yellow") || color.Equals("Yellow") && bubble_b.getColor().Equals("Green")){
					changeColorTo("YellowGreen");
					bubble_b.removeMeFromScene();
					bubble_b.gameObject.GetComponent<CircleCollider2D>().enabled = false;
				} else if(color.Equals("Blue") && bubble_b.getColor().Equals("Green") || color.Equals("Green") && bubble_b.getColor().Equals("Blue")){
					changeColorTo("BlueGreen");
					bubble_b.removeMeFromScene();
					bubble_b.gameObject.GetComponent<CircleCollider2D>().enabled = false;
				}
				forceDirection = Vector2.zero;
				rigidbody.velocity = forceDirection;
			}
			bubble_b = null;
		}
	}

	/*NAO MOVER SE FOR FIXO XD*/
	public void moveMe(){
		if (!selected && !fixedBubble) {
			rigidbody.AddForce (forceDirection);
		} 
	}

	public void selectMe(){
		selected = true;
		sr.color = new Color (sr.color.r, sr.color.g, sr.color.b, 1);
		//transform.GetChild (1).gameObject.GetComponent<SpriteRenderer> ().color = new Color (sr.color.r, sr.color.g, sr.color.b, 1);
		transform.localScale = new Vector3 (1.1f,1.1f, 1);
		rigidbody.velocity = new Vector2 (0, 0);
	}

	public void removeSelect(){
		selected = false;
		sr.color = new Color (sr.color.r, sr.color.g, sr.color.b, 1);
		sr_bright.color = new Color (sr.color.r, sr.color.g, sr.color.b, 0);
		transform.localScale = new Vector3 (1,1, 1);
	}

	public void goToPosition(Vector2 pos, BubbleController b){
		if (fixedBubble)
			return;
		anim.moveTo (new Vector3 (pos.x, pos.y, 0), 0.5f);
		movingTo = true;
		timeToMovingTo = 0.5f;
		rigidbody.velocity = new Vector2 (0, 0);
		bubble_b = b;
	}

	public void removeMeFromScene() {
		if (timeToRemoveMe != -10)
			return;
		sr.color = new Color (1,1,1,0);
		sr_bright.color = new Color (sr.color.r, sr.color.g, sr.color.b, 0);
		transform.GetChild (0).GetComponent<SpriteRenderer> ().color = new Color (1, 1, 1, 0);
		timeToRemoveMe = 0.3f;
		forceDirection = Vector2.zero;
		rigidbody.velocity = forceDirection;
	}

	public void explode(){

		if (timeToRemoveMe != -10 || !explodeMe)
			return;

		GameObject bubbleBright = transform.GetChild (0).gameObject;
		bubbleBright.GetComponent<SpriteRenderer> ().color = new Color (1, 1, 1, 0);

		CircleCollider2D col = GetComponent<CircleCollider2D> ();
		col.enabled = false;
		anim.scaleTo (new Vector3(1.8f,1.8f,1), 0.3f);
		anim.fadeTo (0, 0.3f);
		timeToRemoveMe = 0.5f;

		explodeMe = false;

	}

	public void changeColorTo(string newColor){

		if (sr == null)
			sr = GetComponent<SpriteRenderer> ();

		//primary colors
		if (newColor.Equals("Yellow")) {
			sr.color = GameSethings.getColor(GameSethings.colorGame.YELLOW);
			color = "Yellow";
		} else if (newColor.Equals("Blue")) {
			sr.color = GameSethings.getColor(GameSethings.colorGame.BLUE);
			color = "Blue";
		} else if (newColor.Equals("Red")) {
			sr.color = GameSethings.getColor(GameSethings.colorGame.RED);
			color = "Red";
		}

		//Others colors
		else if (newColor.Equals("Green")) {
			//sr.color = new Color(0.4f,0.69f,0.2f,1);
			sr.color = GameSethings.getColor(GameSethings.colorGame.GREEN);
			color = "Green";
		} else if (newColor.Equals("Orange")) {
			//sr.color = new Color (1, 0.4f, 0, 1);
			sr.color = GameSethings.getColor(GameSethings.colorGame.ORANGE);
			color = "Orange";
		} else if (newColor.Equals("Purple")) {
			//sr.color = new Color (0.52f, 0, 0.67f, 1);
			sr.color = GameSethings.getColor(GameSethings.colorGame.PURPLE);
			color = "Purple";
		} else if (newColor.Equals("RedPurple")) {
			//sr.color = new Color (0.65f, 0.09f, 0.29f, 1);
			sr.color = GameSethings.getColor(GameSethings.colorGame.RED_PURPLE);
			color = "RedPurple";
		} else if (newColor.Equals("RedOrange")) {
			//sr.color = new Color (0.99f, 0.33f, 0.03f, 1);
			sr.color = GameSethings.getColor(GameSethings.colorGame.RED_ORANGE);
			color = "RedOrange";
		} else if (newColor.Equals("BluePurple")) {
			//sr.color = new Color (0.24f, 0f, 0.64f, 1);
			sr.color = GameSethings.getColor(GameSethings.colorGame.BLUE_PURPLE);
			color = "BluePurple";
		} else if (newColor.Equals("YellowOrange")) {
			//sr.color = new Color (1, 0.74f, 0, 1);
			sr.color = GameSethings.getColor(GameSethings.colorGame.YELLOW_ORANGE);
			color = "YellowOrange";
		} else if (newColor.Equals("YellowGreen")) {
			//sr.color = new Color (0.72f, 0.99f, 0.17f, 1);
			sr.color = GameSethings.getColor(GameSethings.colorGame.YELLOW_GREEN);
			color = "YellowGreen";
		} else if (newColor.Equals("BlueGreen")) {
			//sr.color = new Color (0.012f, 0.57f, 0.81f, 1);
			sr.color = GameSethings.getColor(GameSethings.colorGame.BLUE_GREEN);
			color = "BlueGreen";
		}
		
		transform.GetChild (0).gameObject.GetComponent<SpriteRenderer> ().color = new Color(1,1,1,1);
		sr_bright.color = new Color (sr.color.r, sr.color.g, sr.color.b, 0);
	}

	public void initMe(){

		sr = GetComponent<SpriteRenderer> ();
		rigidbody = GetComponent<Rigidbody2D> ();
		anim = GetComponent<AuxiliarAnim> ();
		sr_bright = transform.GetChild (1).gameObject.GetComponent<SpriteRenderer> ();
		anim_bright = transform.GetChild (1).gameObject.GetComponent<AuxiliarAnim> ();

		int rand = Random.Range (1, 19);
		forceDirection = new Vector2 (Random.Range(-2.0f, 2.0f), Random.Range(-2.0f, 2.0f));
		movingTo = false;
		explodeMe = false;
		removeMe = false;
		timeToRemoveMe = -10;
		timeToNewBright = 0.5f;

		transform.localScale = new Vector3 (1, 1, 1);
		GetComponent<CircleCollider2D> ().enabled = true;
		
		if (rand <= 5) {
			//sr.color = Color.blue;
			sr.color = GameSethings.getColor(GameSethings.colorGame.BLUE);
			color = "Blue";
		} else if (rand <= 10) {
			//sr.color = Color.yellow;
			sr.color = GameSethings.getColor(GameSethings.colorGame.YELLOW);
			color = "Yellow";
		} else if (rand <= 15) {
			//sr.color = Color.red;
			sr.color = GameSethings.getColor(GameSethings.colorGame.RED);
			color = "Red";
		} else if (rand == 16) {
			changeColorTo("Green");
			color = "Green";
		} else if (rand == 17) {
			//sr.color = new Color (0.98f, 0.6f, 0, 1);
			changeColorTo("Orange");
			color = "Orange";
		} else if (rand == 18) {
			//sr.color = new Color (0.52f, 0, 0.67f, 1);
			changeColorTo("Purple");
			color = "Purple";
		}

		transform.GetChild (0).gameObject.GetComponent<SpriteRenderer> ().color = Color.white;
		sr_bright.color = new Color (sr.color.r, sr.color.g, sr.color.b, 0);
		
	}

	/*public void addTertiaryColor(string colorName){

		if (sr == null)
			sr = GetComponent<SpriteRenderer> ();

		if (colorName.Equals("RedPurple")) {
			sr.color = new Color (0.65f, 0.09f, 0.29f, 1);
			color = "RedPurple";
		} else if (colorName.Equals("RedOrange")) {
			sr.color = new Color (0.99f, 0.33f, 0.03f, 1); 
			color = "RedOrange";
		} else if (colorName.Equals("BluePurple")) {
			sr.color = new Color (0.24f, 0f, 0.64f, 1);
			color = "BluePurple";
		} else if (colorName.Equals("YellowOrange")) {
			sr.color = new Color (1, 0.74f, 0, 1);
			color = "YellowOrange";
		} else if (colorName.Equals("YellowGreen")) {
			sr.color = new Color (0.72f, 0.99f, 0.17f, 1);
			color = "YellowGreen";
		} else if (colorName.Equals("BlueGreen")) {
			sr.color = new Color (0.012f, 0.57f, 0.81f, 1);
			color = "BlueGreen";
		}

		transform.GetChild (0).gameObject.GetComponent<SpriteRenderer> ().color = new Color(1,1,1,1);
		sr_bright.color = new Color (sr.color.r, sr.color.g, sr.color.b, 0);
	}*/

	public bool isSecondaryColor(){
		if(color.Equals("Purple") || color.Equals("Orange") || color.Equals("Green"))
			return true;
		return false;
	}

	public bool isTertiaryColor(){
		if(color.Equals("RedPurple") || color.Equals("RedOrange") || color.Equals("BluePurple") || color.Equals("YellowOrange")
		   || color.Equals("YellowGreen") || color.Equals("BlueGreen"))
			return true;
		return false;
	}

	private void initBright(){
		float timeAnim = 0;
		float initialFade = 0;
		float toScale = 0;

		if (!isSecondaryColor () && !isTertiaryColor ())
			return;

		if (isSecondaryColor ()) {
			timeAnim = 1;
			initialFade = 0.5f;
			toScale = 1.2f;
			timeToNewBright = 1.2f;
			//Debug.Log("Secondary Bright");
		} else if (isTertiaryColor ()) {
			timeAnim = 0.5f;
			//initialFade = 1;
			initialFade = 0.5f;
			toScale = 1.5f;
			timeToNewBright = 0.8f;
			//Debug.Log("Tertiary Bright");
		}

		anim_bright.gameObject.transform.localScale = new Vector3 (1.06f, 1.06f, 1);
		sr_bright.color = new Color (sr_bright.color.r, sr_bright.color.g, sr_bright.color.b, initialFade);

		anim_bright.scaleTo (new Vector3(toScale, toScale, 1), timeAnim);
		anim_bright.fadeTo (0, timeAnim);

	}

	//Getters and setters
	public string getColor(){
		return color;
	}

	public bool isExplodeMe(){
		return explodeMe;
	}
	public void setExplodeMe(bool explode){
		forceDirection = Vector2.zero;
		rigidbody.velocity = forceDirection;
		explodeMe = explode;
	}
	public bool isRemoveMe(){
		return removeMe;
	}
	public bool isFixed(){
		return fixedBubble;
	}
	public void setFixed(bool value){
		fixedBubble = value;
	}
	public bool isMovingTo(){
		return movingTo;
	}
	public BubbleController get_bubble_b(){
		return bubble_b;
	}
}
