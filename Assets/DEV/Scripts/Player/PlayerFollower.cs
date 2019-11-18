using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PlayerFollower : MonoBehaviour
{
    public float yOffset = .1f;
    public bool TopFollower = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(TopFollower)
            transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y + yOffset, Camera.main.transform.position.z);
        else
            transform.position = new Vector3(Camera.main.transform.position.x, GameObject.FindGameObjectWithTag("Player").transform.position.y + yOffset, Camera.main.transform.position.z);
    }
}
