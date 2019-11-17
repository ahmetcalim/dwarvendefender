using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIndicator : MonoBehaviour
{
    public GameObject redArrowPrefab;
    Mob mob;
    GameObject player;
    GameObject redArrow;
    IndicatorFollower iFollower;
    float distance;

    private void OnEnable()

    {
        mob = GetComponent<MobParent>().mob;
        player = GameObject.FindGameObjectWithTag("MainCamera");
        GameObject g = GameObject.FindGameObjectWithTag("IndicatorTrigger");
        iFollower = g.GetComponent<IndicatorFollower>();

    }

    bool CheckAngle()
    {
        Vector3 targetDir = mob.transform.position - player.transform.position;
        float angle = Vector3.Angle(targetDir, player.transform.forward);

        if (angle < 60)
            return true;
        else
            return false;
    }

    private void Update()
    {
        distance = Vector3.Distance(mob.transform.position, player.transform.position);

        if (distance <= 10 && !mob.dead && !CheckAngle() && FindObjectOfType<Spire>().scale <=1f)
        { 
            if (redArrow == null && mob.target.gameObject.layer == 15)
            {
                redArrow = iFollower.AddIndicator(redArrowPrefab);
                redArrow.transform.localPosition = Vector3.zero;
                redArrow.GetComponent<LookAtEnemy>().target = mob.transform;
            }

            if (redArrow != null)
            {
                if (distance <= 10 && distance > 6)
                {
                    redArrow.GetComponentInChildren<SpriteRenderer>().color = Color.white;
                    redArrow.GetComponentInChildren<Animator>().enabled = false;
                }
                else if (distance <= 6 && distance > 2)
                {
                    redArrow.GetComponentInChildren<SpriteRenderer>().color = Color.yellow;
                    redArrow.GetComponentInChildren<Animator>().enabled = false;
                }
                else if (distance < 3)
                {
                    redArrow.GetComponentInChildren<SpriteRenderer>().color = Color.red;
                    redArrow.GetComponentInChildren<Animator>().enabled = true;
                }
            }

        }
        else
        {
            DeleteIndicator();
        }

        if (MobSpawn.onPlayer<1)
        {
            if (redArrow != null)
            {
                iFollower.DeleteIndicator(redArrow);
                Destroy(redArrow);
            }
        }
    }
    public void DeleteIndicator()
    {

        if (redArrow != null)
        {
            iFollower.DeleteIndicator(redArrow);
            Destroy(redArrow);
        }
    }

}
