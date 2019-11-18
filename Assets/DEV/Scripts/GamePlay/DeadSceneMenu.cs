using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class DeadSceneMenu : MonoBehaviour
{
    public int sceneIndexNormal;
    public static int sceneIndex;
    public bool restart;
    private void Start()
    {
        if (restart)
        {
            sceneIndex = DeadSceneController.restartIndex;
        }
        else
        {
            sceneIndex = sceneIndexNormal;
        }
      
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene(sceneIndex);
        }
    }
}
