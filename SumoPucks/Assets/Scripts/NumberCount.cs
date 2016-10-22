using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NumberCount : MonoBehaviour {

    public Text spike;
    public Text hammer;
    public Text jump;
    public Text save;
    public Powerups power;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    if(power != null)
        {
            spike.text = power.spike.ToString();
            hammer.text = power.hammer.ToString();
            jump.text = power.jump.ToString();
            save.text = power.save.ToString();
        }
	}

    public void setStats(Powerups power)
    {
        this.power = power;
    }
}
