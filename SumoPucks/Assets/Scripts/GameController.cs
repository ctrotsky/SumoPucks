using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	public GameObject playerPrefab;
	public GameObject mapPrefab;
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
				StartNewGame(mapPrefab);
				mode = Mode.Running;
			}
		}
	}

	void WaitForJoinPlayers(){
		int joinedPlayerNum = -1;
		for (int i = 0; i <= 4; i++){
			if (Input.GetButtonDown("Player"+i+"A")){
				joinedPlayerNum = i;
			}
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

		return pressedStart && (joinedPlayers.Count > 0);
	}

	void StartNewGame(GameObject mapPrefab){

		GameObject map = SpawnMap((GameObject)Instantiate(mapPrefab));
		GameObject spawnPoints = map.transform.Find("Spawnpoints").gameObject;
		//Do other new game stuff. Timer? Idk

		for (int i = 0; i < joinedPlayers.Count; i++){
			SpawnPlayer((GameObject)Instantiate(playerPrefab), spawnPoints.transform.GetChild(i), (int)joinedPlayers[i]);
		}
	}

	public GameObject SpawnMap(GameObject map){
		map.gameObject.name = "Map";
		return map;
	}

	public void SpawnPlayer(GameObject player, Transform spawnPoint, int playerNumber){
		player.transform.position = spawnPoint.position;
		player.gameObject.name = "Player" + playerNumber;
		player.GetComponent<PlayerController>().playerNum = playerNumber;
		player.GetComponent<PlayerController>().map = spawnPoint.parent.parent.gameObject;
		player.transform.SetParent(players.transform);
	}
}
