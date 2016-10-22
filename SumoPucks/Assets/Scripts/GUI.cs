using UnityEngine;
using System.Collections;

public class GUI : MonoBehaviour {

    public GameObject playerGUI;

	// Use this for initialization
	void Start () {
        Camera camera = GetComponent<Camera>();
        GameObject[] player = GameObject.FindGameObjectsWithTag("Player");
        //camera.ViewportToScreenPoint();
        //player.Length;
        foreach (GameObject obj in player)
        {

        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
