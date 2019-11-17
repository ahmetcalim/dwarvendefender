using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowHitIndicator : MonoBehaviour
{
    public CameraFilterPack_AAA_Blood_Hit hit;

    public enum Direction { LEFT, RIGHT, FORWARD, BACK}
    public Direction direction;
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<ArrowBehaviour>())
        {
            StartCoroutine(HitEffect());
        }
    }
    IEnumerator HitEffect()
    {
        switch (direction)
        {
            case Direction.LEFT:
                hit.Blood_Hit_Left = 1f;
                yield return new WaitForSeconds(1f);
                hit.Blood_Hit_Left = 0f;
                Debug.Log("SOLDAN VURDU");
              
                break;
            case Direction.RIGHT:
                Debug.Log("SAĞDAN VURDU");
                hit.Blood_Hit_Right = 1f;
                yield return new WaitForSeconds(1f);
                hit.Blood_Hit_Right = 0f;
                break;
            case Direction.FORWARD:
                Debug.Log("ÖNLERDEN VURDU");
                hit.Blood_Hit_Up = 1f;
                yield return new WaitForSeconds(1f);
                hit.Blood_Hit_Up = 0f;
                break;
            case Direction.BACK:
                Debug.Log("ARKADAN VURDU");
                hit.Blood_Hit_Down = 1f;
                yield return new WaitForSeconds(1f);
                hit.Blood_Hit_Down = 0f;
                break;
            default:
                break;
        }
    }
}
