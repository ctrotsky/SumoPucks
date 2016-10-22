using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	public GameObject playerPrefab;
	public GameObject map;
	public GameObject players;
	
	// Use this for initialization
	void Start () {
		StartNewGame(map, 3);
	}
	
	// Update is called once per frame
	void Update () {

	}

	void WaitForPlayerConnect(){

	}

	void StartNewGame(GameObject map, int numPlayers){
		GameObject spawnPoints = map.transform.Find("Spawnpoints").gameObject;

		for (int i = 0; i < numPlayers; i++){
			SpawnPlayer((GameObject)Instantiate(playerPrefab), spawnPoints.transform.GetChild(i), i);
		}
	}

	public void SpawnPlayer(GameObject player, Transform spawnPoint, int playerNumber){
		player.transform.position = spawnPoint.position;
		player.gameObject.name = "Player" + playerNumber;
		player.GetComponent<PlayerController>().playerNum = playerNumber;
		player.GetComponent<PlayerController>().map = spawnPoint.parent.parent.gameObject;
		player.transform.SetParent(players.transform);
	}
}
