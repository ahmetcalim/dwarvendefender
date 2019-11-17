using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComboFeedBack : MonoBehaviour
{
    public Text feedBackText;
    Transform player;

    private void OnEnable()
    {
        player = Camera.main.transform;
        StartCoroutine(DestroyFeedBack());
    }
    IEnumerator DestroyFeedBack()
    {
        yield return new WaitForSeconds(1);
        Destroy(this.gameObject);
    }
    private void Update()
    {
        Vector3 targetTransform = new Vector3(player.position.x, transform.position.y, player.position.z);
        Vector3 own = transform.position;
       transform.rotation = Quaternion.LookRotation(-(own - targetTransform));
    }
}
