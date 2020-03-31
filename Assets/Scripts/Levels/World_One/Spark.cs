using UnityEngine;
using System.Collections;

public class Spark : MonoBehaviour {

	private SpriteRenderer sr;
	private float timeToRemoveMe;
	private bool removeMe;
	private GameSethings.colorGame cor;

	void Awake () {
		sr = GetComponent<SpriteRenderer> ();
	}

	// Use this for initialization
	void Start () {
		timeToRemoveMe = -10;
		removeMe = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (timeToRemoveMe > 0) {
			transform.Translate(0.25f*Time.deltaTime,2*Time.deltaTime,0);
			sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, sr.color.a - Time.deltaTime*2);
			timeToRemoveMe -= Time.deltaTime;
		} else if (timeToRemoveMe > -5) {
			removeMe = true;
		}
	}

	public void toRemove(){
		transform.parent = null;
		timeToRemoveMe = 0.5f;
	}

	public void destroyMe(){
		removeMe = false;
		Destroy (this.gameObject);
	}

	public void setColorTo(GameSethings.colorGame color){
		sr.color = GameSethings.getColor (color);
		cor = color;
	}

	public GameSethings.colorGame getColor(){
		return cor;
	}
	
	public bool isRemoveMe(){
		return removeMe;
	}

	public void setRendererPosition(int pos){
		sr.sortingOrder = pos;
	}

}
