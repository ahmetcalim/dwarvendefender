using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliceHandler : MonoBehaviour
{
    public GameObject sliced;
    public List<Transform> transforms;
    public void InstantiateSliced()
    {
        gameObject.SetActive(false);
        GameObject copy = Instantiate(sliced,transform.position, Quaternion.identity);
        copy.transform.SetParent(FindObjectOfType<MobSpawn>().transform);
        copy.transform.position = GetComponent<EnemySlashHandler>().mob.transform.position;
        copy.SetActive(false);
        for (int i = 0; i < copy.GetComponent<HipsHolder>().transforms.Count; i++)
        {
            copy.GetComponent<HipsHolder>().transforms[i].localPosition = transforms[i].localPosition;
            copy.GetComponent<HipsHolder>().transforms[i].localEulerAngles = transforms[i].localEulerAngles;

        }

        copy.SetActive(true);
    }
}
