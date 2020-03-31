using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ComboBar : MonoBehaviour {

	public Image fillImage;
	public GameManager gameManager;
	public Image comboValue;

	private Image img;
	private int currentCombo;
	private float timeToFillCombo;
	private float velDown;
	private float timeToAnimComboValue;

	// Use this for initialization
	void Start () {
		img = GetComponent<Image> ();
		img.color = new Color (img.color.r, img.color.g, img.color.b, 0);
		fillImage.color = new Color (fillImage.color.r, fillImage.color.g, fillImage.color.b, 0);
		comboValue.gameObject.SetActive (false);
		currentCombo = 0;
		timeToFillCombo = 2;
		velDown = 2;
		timeToAnimComboValue = -10;
	}
	
	// Update is called once per frame
	void Update () {
		attBar ();

		if (timeToAnimComboValue > 0)
			timeToAnimComboValue -= Time.deltaTime;
		else if (timeToAnimComboValue <= 0 && timeToAnimComboValue > -5) {
			comboValue.transform.localScale = new Vector3(1,1,1);
			comboValue.color = Color.white;
			timeToAnimComboValue = -10;
		}

	}

	public void addTertiary(){
		if (currentCombo <= 0) {
			img.color = new Color(img.color.r, img.color.g, img.color.b, 1);
			fillImage.color = new Color(fillImage.color.r, fillImage.color.g, fillImage.color.b, 1);
			comboValue.gameObject.SetActive (true);

			currentCombo = 1;
			fillImage.fillAmount = 1;
		} else if (currentCombo < 10) {
			currentCombo++;
			fillImage.fillAmount = 1;
			comboValue.transform.localScale = new Vector3(1.2f,1.2f,1);
			comboValue.color = Color.green;
			timeToAnimComboValue = 0.15f;
			//gameManager.createCombo();
		}
		comboValue.transform.Find("comboValue").GetComponent<Text>().text = currentCombo + "";
	}

	public void addSecondary(){
		if (currentCombo > 0)
			fillImage.fillAmount = 1;
	}

	private void attVelDown(){
		if (currentCombo <= 2)
			velDown = 2;
		else if (currentCombo == 3)
			velDown = 1.5f;
		else if (currentCombo == 4)
			velDown = 1;
		else if (currentCombo == 5)
			velDown = 0.8f;
		else if (currentCombo == 6)
			velDown = 0.8f;
		else if (currentCombo == 7)
			velDown = 0.7f;
		else if (currentCombo == 8)
			velDown = 0.7f;
		else if (currentCombo == 9)
			velDown = 0.5f;
		else if (currentCombo == 10)
			velDown = 0.5f;
	}

	private void attBar (){
		fillImage.fillAmount -= Time.deltaTime/velDown;
		if(fillImage.fillAmount <= 0 && img.color.a > 0){
			img.color = new Color(img.color.r, img.color.g, img.color.b, 0);
			fillImage.color = new Color(fillImage.color.r, fillImage.color.g, fillImage.color.b, 0);
			comboValue.gameObject.SetActive (false);
			//if(currentCombo > 1)
				//gameManager.createCombo();
			currentCombo = 0;
		}
	}

	//Getters and setter
	public int getCurrentCombo(){
		return currentCombo;
	}
	public void setCurrentCombo(int value){
		currentCombo = value;
	}

	public float getFillCombo(){
		return timeToFillCombo;
	}
	public void setFillCombo(float value){
		timeToFillCombo = value;
	}

}