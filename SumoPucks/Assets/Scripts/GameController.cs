using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

	public GameObject playerPrefab;
	public GameObject mapPrefab;
	public GameObject[] powerupPrefabs;
	public RuntimeAnimatorController[] animatorControllers;
	public Sprite[] playerSprites;
	public Sprite[] hammerSprites;
	public Color[] characterColors;
	public GameObject players;
	public Canvas HUD;
	public HUDController hudController;

	private GameObject currentMap;

	SortedList joinedPlayers = new SortedList();

	enum Mode {Joining, Running};
	Mode mode;
	
	// Use this for initialization
	void Start () {
		mode = Mode.Joining;
		hudController = GameObject.FindGameObjectWithTag("HUDCanvas").GetComponent<HUDController>();
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
		if (mode == Mode.Running){
			CheckWinState();
		}
	}

	void WaitForJoinPlayers(){
		int joinedPlayerNum = -1;
		int characterNum = -1;
		bool quitting = false;

		for (int i = 0; i <= 4; i++){
			if (Input.GetButtonDown("Player"+i+"A")){
				joinedPlayerNum = i;
				characterNum = 3;
			}
			if (Input.GetButtonDown("Player"+i+"X")){
				joinedPlayerNum = i;
				characterNum = 2;
			}
			if (Input.GetButtonDown("Player"+i+"B")){
				joinedPlayerNum = i;
				characterNum = 1;
			}
			if (Input.GetButtonDown("Player"+i+"Y")){
				joinedPlayerNum = i;
				characterNum = 4;
			}
			if (Input.GetButtonDown("Player"+i+"Back")){
				joinedPlayerNum = i;
				characterNum = 1;
				quitting = true;
			}
		}


		if (joinedPlayerNum >= 0){

			if (!quitting){
				if (!joinedPlayers.Contains(joinedPlayerNum)){
					Debug.Log("Player" + joinedPlayerNum + " Joined");
					joinedPlayers.Add(joinedPlayerNum, characterNum);
					hudController.JoinedPlayer(joinedPlayerNum, characterNum, true);
				}
			}
			else {
				Debug.Log("Player" + joinedPlayerNum + " Quit");
				joinedPlayers.Remove(joinedPlayerNum);
				hudController.JoinedPlayer(joinedPlayerNum, characterNum, false);
			}
		}

		joinedPlayerNum = -1;
		characterNum = -1;
		quitting = false;

		hudController.StartText(joinedPlayers.Count >= 2);
		hudController.JoinedPlayersText(joinedPlayers.Count, true);
	}

	bool WaitForStart(){
		bool pressedStart = false;

		for (int i = 0; i <= 4; i++){
			if (Input.GetButtonDown("Player"+i+"Start")){
				pressedStart = true;
			}
		}

		return pressedStart && (joinedPlayers.Count >= 2);
	}

	void StartNewGame(GameObject mapPrefab){
		hudController.StartText(false);
		hudController.JoinedPlayersText(joinedPlayers.Count, false);
		currentMap = SpawnMap((GameObject)Instantiate(mapPrefab));
		GameObject spawnPoints = currentMap.transform.Find("Spawnpoints").gameObject;
		//Do other new game stuff. Timer? Idk

		for (int i = 0; i < joinedPlayers.Count; i++){
			SpawnPlayer((GameObject)Instantiate(playerPrefab), spawnPoints.transform.GetChild(i), (int)joinedPlayers.GetKey(i), (int)joinedPlayers.GetByIndex(i));
		}

		SpawnPowerup();
		SpawnPowerup();
	}

	public GameObject SpawnMap(GameObject map){
		map.gameObject.name = "Map";
		return map;
	}

	void CheckWinState(){
		GameObject[] remaining = GameObject.FindGameObjectsWithTag("Player");
		Debug.Log("Remaining length: " + remaining.Length);
		if (remaining.Length == 1){
			Debug.Log("Win!");
			hudController.WinMessage(remaining[0].GetComponent<PlayerController>().playerNum);
			StartCoroutine(Reset());
		}

	}

	IEnumerator Reset(){
		yield return new WaitForSeconds(5);
		Scene scene = SceneManager.GetActiveScene();
		SceneManager.LoadScene(scene.name);
		//Destroy(currentMap);
		//joinedPlayers.Clear();
		//hudController.ResetHUD();
		//mode = Mode.Joining;

	}

	public void SpawnPlayer(GameObject player, Transform spawnPoint, int playerNumber, int characterNumber){
		player.transform.Find("Character").GetComponent<Animator>().runtimeAnimatorController = animatorControllers[characterNumber];
		player.GetComponent<SpriteRenderer>().sprite = playerSprites[characterNumber];
		player.transform.Find("hammer").GetComponent<SpriteRenderer>().sprite = hammerSprites[characterNumber];
		player.transform.Find("Aimer").GetComponent<SpriteRenderer>().color = characterColors[characterNumber];
		player.transform.position = spawnPoint.position;
		player.gameObject.name = "Player" + playerNumber;
		player.GetComponent<PlayerController>().playerNum = playerNumber;
		player.GetComponent<PlayerController>().map = spawnPoint.parent.parent.gameObject;
		player.transform.SetParent(players.transform);
	}

	public void SpawnPowerup(){
		GameObject powerup = (GameObject)Instantiate(powerupPrefabs[Random.Range(0,powerupPrefabs.Length)]);

		//Keep guessing random positions in currentMap scale until it is touching the map's collider.
		//do {
		float randX = Random.Range(currentMap.transform.position.x - currentMap.transform.lossyScale.x*2f ,currentMap.transform.position.x + currentMap.transform.lossyScale.x*2f);
		float randY = Random.Range(currentMap.transform.position.y - currentMap.transform.lossyScale.y*2f ,currentMap.transform.position.y + currentMap.transform.lossyScale.y*2f);
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
