using UnityEngine;
using System.Collections;

public class Powerups : MonoBehaviour
{
    public int spike { get; set; }
    public int jump { get; set; }
    public int hammer { get; set; }
    public int save { get; set; }

    Animator anim;
    GameObject hammerObject;

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
		hammerObject = transform.Find("hammer").gameObject;
		attacking = false;
	}

	public void UseHammer() {
		if (hammer >= 1 && !attacking){
			removeHammer();
			StartCoroutine(spinHammer());
		}
	}

	IEnumerator spinHammer(){
		attacking = true;
		int swingSpeed = 10;
		hammerObject.SetActive(true);
		for (int i = 0; i < 360/swingSpeed; i++){
			hammerObject.transform.Rotate(new Vector3(0,0,-swingSpeed));
			yield return new WaitForEndOfFrame();
		}
		hammerObject.SetActive(false);
		attacking = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

}
