using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	public GameObject playerPrefab;
	public GameObject map;
	public GameObject players;

	ArrayList joinedPlayers = new ArrayList();

	enum Mode {Joining, Running};
	Mode mode;
	
	// Use this for initialization
	void Start () {
		mode = Mode.Joining;
	}
	
	// Update is called once per frame
	void Update () {
		if (mode == Mode.Joining){
			WaitForJoinPlayers();
			if (WaitForStart()){
				StartNewGame(map);
				mode = Mode.Running;
			}
		}
	}

	void WaitForJoinPlayers(){
		int joinedPlayerNum = -1;
		if (Input.GetButtonDown("Player0A")){
			joinedPlayerNum = 0;
		}
		if (Input.GetButtonDown("Player1A")){
			joinedPlayerNum = 1;
		}
		if (Input.GetButtonDown("Player2A")){
			joinedPlayerNum = 2;
		}
		if (Input.GetButtonDown("Player3A")){
			joinedPlayerNum = 3;
		}
		if (Input.GetButtonDown("Player4A")){
			joinedPlayerNum = 4;
		}

		if (joinedPlayerNum >= 0){
			if (!joinedPlayers.Contains(joinedPlayerNum)){
				Debug.Log("Player" + joinedPlayerNum + " Joined");
				joinedPlayers.Add(joinedPlayerNum);
			} else {
				Debug.Log("Player" + joinedPlayerNum + " Quit");
				joinedPlayers.Remove(joinedPlayerNum);
			}
			joinedPlayerNum = -1;
		}
	}

	bool WaitForStart(){
		bool pressedStart = false;

		for (int i = 0; i <= 4; i++){
			if (Input.GetButtonDown("Player"+i+"Start")){
				pressedStart = true;
			}
		}

		return pressedStart;
	}

	void StartNewGame(GameObject map){
		GameObject spawnPoints = map.transform.Find("Spawnpoints").gameObject;
		//Do other new game stuff. Timer? Idk

		for (int i = 0; i < joinedPlayers.Count; i++){
			SpawnPlayer((GameObject)Instantiate(playerPrefab), spawnPoints.transform.GetChild(i), (int)joinedPlayers[i]);
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
