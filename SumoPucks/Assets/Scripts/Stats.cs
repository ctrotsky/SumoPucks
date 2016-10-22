using UnityEngine;
using System.Collections;

public class Stats : MonoBehaviour {
    public int weight { get; set; }
    public int friction { get; set; }
    public int charge { get; set; }

    public void addWeight(int w)
    {
        weight += w;
    }
    public void addFriction(int f)
    {
        friction += f;
    }
    public void addCharge(int c)
    {
        charge += c;
    }

    public void removeWeight(int w)
    {
        weight -= w;
    }
    public void removeFriction(int f)
    {
        friction -= f;
    }
    public void removeCharge(int c)
    {
        charge -= c;
    }
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
