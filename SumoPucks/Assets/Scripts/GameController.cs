using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	public GameObject playerPrefab;
	public GameObject mapPrefab;
	public GameObject[] powerupPrefabs;
	public GameObject players;

	private GameObject currentMap;

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
				mode = Mode.Running;
				StartNewGame(mapPrefab);
				StartCoroutine(SpawnPowerups());
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
		currentMap = SpawnMap((GameObject)Instantiate(mapPrefab));
		GameObject spawnPoints = currentMap.transform.Find("Spawnpoints").gameObject;
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

	public void SpawnPowerup(){
		GameObject powerup = (GameObject)Instantiate(powerupPrefabs[Random.Range(0,powerupPrefabs.Length -1)]);

		//Keep guessing random positions in currentMap scale until it is touching the map's collider.
		//do {
			float randX = Random.Range(currentMap.transform.position.x - currentMap.transform.localScale.x/2f ,currentMap.transform.position.x + currentMap.transform.localScale.x/2f);
			float randY = Random.Range(currentMap.transform.position.y - currentMap.transform.localScale.y/2f ,currentMap.transform.position.y + currentMap.transform.localScale.y/2f);
			powerup.transform.position = new Vector3(randX,randY,0);
			Debug.Log("touching: " + powerup.GetComponent<Collider2D>().IsTouching(currentMap.GetComponent<Collider2D>()));
		//} while(!powerup.GetComponent<Collider2D>().IsTouching(currentMap.GetComponent<Collider2D>()));
	}

	IEnumerator SpawnPowerups(){
		while (mode == Mode.Running){
			yield return new WaitForSeconds(Random.Range(5,10));
			SpawnPowerup();
		}
	}
}
