using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
    Camera camera;
    public float minSizeY = 0.5f;
    public float buffer = 0;

    // Use this for initialization
    void Start()
    {
        camera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        setCameraPos();
        setCameraSize();
    }

    private void setCameraSize()
    {
        
    }

    private void setCameraPos()
    {
        float highY = 0;
        float lowY = 0;
        float highX = 0;
        float lowX = 0;

        GameObject[] player = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject obj in player)
        {
            if (highX > obj.transform.position.x) highX = obj.transform.position.x;
            else if (lowX < obj.transform.position.x) lowX = obj.transform.position.x;
            if (highY > obj.transform.position.y) highY = obj.transform.position.y;
            else if (lowY < obj.transform.position.y) lowY = obj.transform.position.y;

        }

        float mX = (lowX + highX) / 2;
        float mY = (lowY + highY) / 2;

        transform.position = new Vector3(mX, mY, transform.position.z);

        //horizontal size is based on actual screen ratio
        float minSizeX = minSizeY * Screen.width / Screen.height;

        //multiplying by 0.5, because the ortographicSize is actually half the height
        float width = Mathf.Abs(highX - lowX) * 0.5f;
        float height = Mathf.Abs( highY - lowY) * 0.5f;

        //computing the size
        float camSizeX = Mathf.Max(width, minSizeX);
        camera.orthographicSize = Mathf.Max(height + buffer,
            camSizeX * Screen.height / Screen.width, minSizeY);
    }
}