using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PanelConfiguration : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void setMusicVolume(Scrollbar scrollMusic){

	}

	public void setEffectsVolume(Scrollbar scrollEffects){

	}

	public void setLanguage(string newLanguage){
		if (!newLanguage.Equals ("PT_BR") && !newLanguage.Equals ("ENG") && !newLanguage.Equals ("IT"))
			return;
	}

}
