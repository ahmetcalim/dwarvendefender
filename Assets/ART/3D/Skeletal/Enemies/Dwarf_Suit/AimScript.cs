using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimScript : MonoBehaviour
{

    public Transform Target;
    public Vector3 Offset;
    private float xMin = 70.0f, xMax = 290.0f;
    float time = 2f;
    bool rotate = true;
    public Transform neck;
    float valToBeLerped  = 0f;
    float tParam = 0f;
    float lastY;


    void LateUpdate()
    {
        Vector3 targetDir = Target.transform.position - neck.transform.position;
        float angle = Vector3.Angle(targetDir, Target.transform.forward);
        Vector3 cross = Vector3.Cross(targetDir,Target.transform.forward);
        
        if (angle<80f)
        {
            rotate = true;
        }
        else
        {
            rotate = false;
        }
        if (rotate)
        {
            neck.LookAt(Target.position);
            neck.rotation = neck.rotation * Quaternion.Euler(Offset);
            lastY = neck.eulerAngles.y;
            tParam = 0;
            valToBeLerped = 0;
            time = 2f;
        }
        if (!rotate && time >= 0)
        {
            tParam += Time.deltaTime * 2f;
            if (cross.y<0)
            {
                valToBeLerped = Mathf.Lerp(lastY, 0f, tParam);
            }
            else
            {
                valToBeLerped = Mathf.Lerp(lastY, 360f, tParam);
            }
            
            neck.eulerAngles = new Vector3(0, valToBeLerped, 0);
            time = time - Time.deltaTime;
        }
    }
}