using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HUDController : MonoBehaviour {

	GameObject[] playerHUDs;
	GameObject startText;

	public Sprite[] ProfileImages;

	// Use this for initialization
	void Start () {
		playerHUDs = new GameObject[5];
		startText = transform.Find("StartText").gameObject;

		for (int i = 1; i < playerHUDs.Length; i++){
			playerHUDs[i] = transform.Find("Player" + i + "HUD").gameObject;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void JoinedPlayer(int playerNum, int characterNum, bool joined){
		playerHUDs[playerNum].transform.Find("JoinText").GetComponent<Image>().enabled = !joined;
		playerHUDs[playerNum].transform.Find("Panel").gameObject.SetActive(joined);
		playerHUDs[playerNum].transform.Find("Panel").Find("ProfilePanel").Find("ProfileImage").gameObject.GetComponent<Image>().sprite = ProfileImages[characterNum];

	}

	public void StartText(bool visible){
		startText.GetComponent<Text>().enabled = visible;
	}

	public void Lives(int playerNum, int lives){
		playerHUDs[playerNum].transform.Find("Panel").Find("ProfilePanel").Find("LifeCount").gameObject.GetComponent<Text>().text = "x" + lives;
	}
}
