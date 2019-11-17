using CrazyMinnow.SALSA;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FirstPlayTrigger : MonoBehaviour
{
    public Rigidbody pBody;
    public BossDwarf boss;
    public GameObject playlist;
    public void InToFirstInteraction()
    {

        boss.GetComponent<Animator>().SetBool("Talk", true);
        playlist.GetComponent<AudioSource>().mute = true;

        StartCoroutine(OutTheFirstInteraction());
    }
    private IEnumerator OutTheFirstInteraction()
    {
        yield return new WaitForSeconds(3f);
        pBody.isKinematic = true;
        yield return new WaitForSeconds(82);
        SceneManager.LoadScene(10);
      
    }
}
