using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HUDController : MonoBehaviour {

	GameObject[] playerHUDs;
	GameObject startText;
	GameObject joinedPlayersText;

	public Sprite[] ProfileImages;

	const string waitingToJoin = "Waiting for players to join... ";

	// Use this for initialization
	void Start () {
		playerHUDs = new GameObject[5];
		startText = transform.Find("StartText").gameObject;
		joinedPlayersText = transform.Find("JoinedPlayersText").gameObject;
		joinedPlayersText.GetComponent<Text>().text = waitingToJoin + "0/4";


		for (int i = 1; i < playerHUDs.Length; i++){
			playerHUDs[i] = transform.Find("Player" + i + "HUD").gameObject;
		}
	}

	public void JoinedPlayer(int playerNum, int characterNum, bool joined){
		Debug.Log("playernum " + playerNum);
		Debug.Log("characterNum " + characterNum);
		Debug.Log("joined " + joined);
		playerHUDs[playerNum].transform.Find("JoinText").GetComponent<Image>().enabled = !joined;
		playerHUDs[playerNum].transform.Find("Panel").gameObject.SetActive(joined);
		playerHUDs[playerNum].transform.Find("Panel").Find("ProfilePanel").Find("ProfileImage").gameObject.GetComponent<Image>().sprite = ProfileImages[characterNum];
	}

	public void StartText(bool visible){
		startText.GetComponent<Text>().enabled = visible;
	}

	public void JoinedPlayersText(int numPlayers, bool visible){
		joinedPlayersText.GetComponent<Text>().text = waitingToJoin + numPlayers + "/4";
		joinedPlayersText.GetComponent<Text>().enabled = visible;
	}

	public void Lives(int playerNum, int remaining){
		playerHUDs[playerNum].transform.Find("Panel").Find("ProfilePanel").Find("LifeCount").gameObject.GetComponent<Text>().text = "x" + remaining;
	}

	public void Hammer(int playerNum, int remaining){
		playerHUDs[playerNum].transform.Find("Panel").Find("WeaponsPanel").Find("HammerCount").gameObject.GetComponent<Text>().text = "x" + remaining;
	}

	public void Spikes(int playerNum, int remaining){
		playerHUDs[playerNum].transform.Find("Panel").Find("WeaponsPanel").Find("SpikesCount").gameObject.GetComponent<Text>().text = "x" + remaining;
	}

	public void ResetHUD(){
		for (int i = 0; i < playerHUDs.Length; i++){
			playerHUDs[i].transform.Find("JoinText").GetComponent<Image>().enabled = true;
			playerHUDs[i].transform.Find("Panel").gameObject.SetActive(false);
		}
	}

	public void WinMessage(int playerNum){
		startText.GetComponent<Text>().text = "Player " + playerNum + " Wins!";
		startText.GetComponent<Text>().enabled = true;
	}
}
