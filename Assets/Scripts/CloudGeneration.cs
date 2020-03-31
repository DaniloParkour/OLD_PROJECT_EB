using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CloudGeneration : MonoBehaviour {

	public GameObject cloud;
	private List<GameObject> clouds;
	private float timeToNewCloud;

	// Use this for initialization
	void Start () {
		timeToNewCloud = Random.Range (0.0f, 2.5f);
		clouds = new List<GameObject> ();
		for (int i = 0; i < 2; i ++) {
			float y = Random.Range (-5.0f, 5.0f);
			GameObject newCloud = Instantiate (cloud, new Vector3(-12,y,0), Quaternion.identity) as GameObject;
			if(i == 0)
				newCloud.transform.position = new Vector3(-7, newCloud.transform.position.y,0);
			if(i == 1)
				newCloud.transform.position = new Vector3(4, newCloud.transform.position.y,0);
			newCloud.transform.localScale = new Vector3(1.5f,1.5f,1);
			clouds.Add (newCloud);
		}
	}
	
	// Update is called once per frame
	void Update () {
		timeToNewCloud -= Time.deltaTime;
		if (timeToNewCloud <= 0)
			createNewCloud();
		moveClouds ();

	}

	void createNewCloud(){
		float y = Random.Range (-5.0f, 5.0f);
		GameObject newCloud = Instantiate (cloud, new Vector3(-12,y,0), Quaternion.identity) as GameObject;
		clouds.Add (newCloud);
		timeToNewCloud = Random.Range (8.0f, 30.0f);

		int rand = Random.Range (1, 6);

		if(rand == 1)
			newCloud.transform.localScale = new Vector3(1.5f,1.5f,1);
		else if(rand == 2)
			newCloud.transform.localScale = new Vector3(1.5f,2,1);
		else if(rand == 3)
			newCloud.transform.localScale = new Vector3(2,2,1);
		else if(rand == 4)
			newCloud.transform.localScale = new Vector3(2,2.5f,1);
		else if(rand == 5)
			newCloud.transform.localScale = new Vector3(2.5f,2.5f,1);

	}

	void moveClouds(){
		if(clouds.Count > 0){
			for(int i = clouds.Count-1; i >= 0; i--){
				clouds[i].transform.Translate(new Vector3(0.5f*Time.deltaTime, 0, 0));
				if(clouds[i].transform.position.x >= 12){
					GameObject removeMe = clouds[i];
					clouds.Remove(removeMe);
					Destroy(removeMe.gameObject);
				}
			}
		}
	}
}
