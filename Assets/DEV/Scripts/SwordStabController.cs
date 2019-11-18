using BzKovSoft.ObjectSlicerSamples;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Valve.VR;
using MagicalFX;
public class SwordStabController : MonoBehaviour
{
    public Vector3 StabThreshold;
    public float StabAngleTolerance;
    

    public Transform SwordTip;
    private Vector3 tipLast;
    private Vector3 tipCur;

    public float Clinginess;
    private FixedJoint clingJoint;

    public bool stabbing = false;
    public GameObject stabbingTarget;
    public float SlashDisableForceMult;
    public List<GameObject> knifes;
    public SteamVR_Input_Sources source;

    private Vector3 tf;
    private float a, m;
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("enemy") || other.gameObject.CompareTag("head"))
        {
            Vector3 tf = transform.TransformVector(StabThreshold);
            Vector3 v = (tipCur - tipLast) / Time.deltaTime;
            float a = Mathf.Abs(Vector3.Angle(tf, v));
            float m = v.magnitude;
            if (a < StabAngleTolerance)
            {

                if (m > tf.magnitude)
                {
                    if (other.transform.CompareTag("enemy"))
                    {
                        foreach (var item in knifes)
                        {
                            item.SetActive(false);
                        }

                        stabbing = true;
                        stabbingTarget = other.gameObject;


                        // stop slashing code here
                    }
                }
            }
        }
       
    }
    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("enemy") || other.gameObject.CompareTag("head"))
        {
            if (stabbing)
            {
                stabbing = false;
            }
        }
    }
    void OnJointBreak(float breakForce)
    {
        if (!stabbingTarget.GetComponentInParent<Mob>().sliced && !stabbingTarget.GetComponentInParent<Mob>().dead)
        {
            stabbingTarget.GetComponentInParent<Mob>().TakeDamage(stabbingTarget.GetComponentInParent<Mob>().hitPoint + 1);
        }
       
    }
    void Start()
    {
        tipCur = SwordTip.position;
        tipLast = tipCur;
    }
    void FixedUpdate()
    {
        CheckStab(out tf, out a, out m);
        if (stabbing)
        {
            if (m < tf.magnitude) SetStab();
            else if (a > StabAngleTolerance) SetStab();
        }

    }

    private void CheckStab(out Vector3 tf, out float a, out float m)
    {
        tipLast = tipCur;
        tipCur = SwordTip.position;
        tf = transform.TransformVector(StabThreshold);
        Vector3 v = (tipCur - tipLast) / Time.deltaTime;
        a = Mathf.Abs(Vector3.Angle(tf, v));
        m = v.magnitude;
       
    }

    public void SetStab()
    {
        Vector3 tf = transform.TransformVector(StabThreshold);
        Vector3 v = (tipCur - tipLast) / Time.deltaTime;
        float a = Mathf.Abs(Vector3.Angle(tf, v));
        float m = v.magnitude;
        Debug.Log("Setting stab at angle: " + a.ToString() + " and magnitude: " + m.ToString());
        if (stabbingTarget.transform.GetComponentInParent<Mob>())
        {
            if (stabbingTarget.transform.GetComponentInParent<Animator>())
            {
                stabbingTarget.transform.GetComponentInParent<Animator>().enabled = false;
                stabbingTarget.transform.GetComponentInParent<NavMeshAgent>().enabled = false;
                StartCoroutine(FindObjectOfType<GenericFunctions>().ExecuteHaptic(source));
            }
        }
        stabbing = false;
        clingJoint = gameObject.AddComponent<FixedJoint>();
        clingJoint.enablePreprocessing = false;
        clingJoint.breakForce = Clinginess;
        clingJoint.breakTorque = Mathf.Infinity;
        clingJoint.connectedBody = stabbingTarget.GetComponent<Rigidbody>();
    }
}
