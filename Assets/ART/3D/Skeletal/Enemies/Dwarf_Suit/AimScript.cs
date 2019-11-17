using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimScript : MonoBehaviour
{

    public Transform Target;
    public Vector3 Offset;
    private float xMin = 70.0f, xMax = 290.0f;
    float time = 2f;
    public bool rotate = true;
    public Transform neck;
    float valToBeLerped  = 0f;
    float tParam = 0f;
    float lastY;



    public Transform target;
    public float damping;
    private Quaternion initialRotation;
    private void Start()
    {
        initialRotation = transform.rotation;
    }
    // Update is called once per frame
    void Update()
    {

        Vector3 lookPos = target.position - transform.position;
        lookPos.y = 0;
        float angle = Vector3.SignedAngle(initialRotation * Vector3.forward, lookPos, Vector3.up);
        Quaternion rotation = initialRotation * Quaternion.AngleAxis(Mathf.Clamp(angle, -85, 85), Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * damping);
    }

    /*
    void Update()
    {
        Vector3 targetDir = Target.transform.position - neck.transform.position;
        float angle = Vector3.Angle(targetDir, -Target.transform.forward);
        Vector3 cross = Vector3.Cross(targetDir,-Target.transform.forward);




       
        if (rotate)
        {
            if (UnityEditor.TransformUtils.GetInspectorRotation(gameObject.transform).y<75)
            {
                neck.LookAt(Target.position);               
            }
            else
            {
                neck.localEulerAngles = new Vector3(0,0,0);              
            }

           
            
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
    */
    
}