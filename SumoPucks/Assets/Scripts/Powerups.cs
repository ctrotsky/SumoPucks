using UnityEngine;
using System.Collections;

public class Powerups : MonoBehaviour
{
    public int spike { get; set; }
    public int jump { get; set; }
    public int hammer { get; set; }
    public int save { get; set; }

    public float spikesDuration;

    Animator anim;
    GameObject hammerObject;
	GameObject spikesObject;
    PlayerController pc;

    bool attacking;

    public void setPowerups(int sp, int j, int h, int s)
    {
        spike = sp;
        jump = j;
        hammer = h;
        save = s;
    }
    public void addSpike()
    {
        spike += 1;
    }
    public void addJump()
    {
        jump += 1;
    }
    public void addHammer()
    {
        hammer += 1;
    }
    public void addSave()
    {
        save += 1;
    }
    public void removeSpike()
    {
        spike -= 1;
    }
    public void removeJump()
    {
        jump -= 1;
    }
    public void removeHammer()
    {
        hammer -= 1;
    }
    public void removeSave()
    {
        save -= 1;
    }
    // Use this for initialization
    void Start () {
		anim = GetComponent<Animator>();
		pc = GetComponent<PlayerController>();
		hammerObject = transform.Find("hammer").gameObject;
		spikesObject = transform.Find("Spikes").gameObject;
		attacking = false;
	}

	public void UseHammer() {
		Debug.Log("use hammer");
		if (hammer >= 1 && !attacking){
			removeHammer();
			AnimateHammer();
			//StartCoroutine(spinHammer());
		}
	}

	public void UseSpikes() {
		Debug.Log("use spikes");
		if (spike >= 1 && !attacking){
			StartCoroutine(SpikesFriction());
			StartCoroutine(AnimateSpikes());
		}
	}

	void AnimateHammer(){
		anim.SetTrigger("HammAtk");
	}

	IEnumerator SpikesFriction(){
		pc.friction = pc.friction + 4;
		yield return new WaitForSeconds(spikesDuration);
		pc.friction = pc.friction - 4;
	}

	IEnumerator AnimateSpikes(){
		attacking = true;
		for (int i = 0; i < 10; i++){
			spikesObject.transform.localScale += new Vector3(0.05f,0.05f,0.0f);
			yield return new WaitForEndOfFrame();
		}
		yield return new WaitForSeconds(spikesDuration);
		for (int i = 0; i < 10; i++){
			spikesObject.transform.localScale -= new Vector3(0.05f,0.05f,0.0f);
			yield return new WaitForEndOfFrame();
		}
		attacking = false;
	}

	/*IEnumerator spinHammer(){
		Debug.Log("spin hammer");
		attacking = true;
		int swingSpeed = 10;
		hammerObject.SetActive(true);
		for (int i = 0; i < 360/swingSpeed; i++){
			Debug.Log("spinnnnnn");
			hammerObject.transform.Rotate(new Vector3(0,0,-swingSpeed));
			yield return new WaitForEndOfFrame();
		}
		hammerObject.SetActive(false);
		attacking = false;
	}*/
	
	// Update is called once per frame
	void Update () {
	
	}

}
