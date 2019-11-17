using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Valve.VR.Extras
{
    public class CustomLaserPointer : MonoBehaviour
    {
        public SteamVR_Behaviour_Pose pose;

        //public SteamVR_Action_Boolean interactWithUI = SteamVR_Input.__actions_default_in_InteractUI;
        public SteamVR_Action_Boolean interactWithUI = SteamVR_Input.GetBooleanAction("InteractUI");

        public bool active = true;
        public Color color;
        public float thickness = 0.002f;
        public Color clickColor = Color.green;
        public GameObject holder;
        public GameObject pointer;
        bool isActive = false;
        public bool addRigidBody = false;

        Transform previousContact = null;



        bool flickHolding = false;
        float flickDistance;
        GameObject flickTrigger;
        Vector3 flickStart, flickFinish;
        float flickT;

        bool triggerDown = false;
        bool triggerReleased = false;

        public float MaxDistance;

        private void Start()
        {
            if (pose == null)
                pose = this.GetComponent<SteamVR_Behaviour_Pose>();
            if (pose == null)
                Debug.LogError("No SteamVR_Behaviour_Pose component found on this object");

            if (interactWithUI == null)
                Debug.LogError("No ui interaction action has been set on this component.");


            holder = new GameObject();
            holder.transform.parent = this.transform;
            holder.transform.localPosition = Vector3.zero;
            holder.transform.localRotation = Quaternion.identity;

            pointer = GameObject.CreatePrimitive(PrimitiveType.Cube);
            pointer.transform.parent = holder.transform;
            pointer.transform.localScale = new Vector3(thickness, thickness, 100f);
            pointer.transform.localPosition = new Vector3(0f, 0f, 50f);
            pointer.transform.localRotation = Quaternion.identity;
            BoxCollider collider = pointer.GetComponent<BoxCollider>();
            if (addRigidBody)
            {
                if (collider)
                {
                    collider.isTrigger = true;
                }
                Rigidbody rigidBody = pointer.AddComponent<Rigidbody>();
                rigidBody.isKinematic = true;
            }
            else
            {
                if (collider)
                {
                    Object.Destroy(collider);
                }
            }
            Material newMaterial = new Material(Shader.Find("Unlit/Color"));
            newMaterial.SetColor("_Color", color);
            pointer.GetComponent<MeshRenderer>().material = newMaterial;
        }

        public virtual void OnPointerIn(PointerEventArgs e)
        {
            if(e.target.GetComponent<HoverPopup>() != null)
            {
                e.target.GetComponent<HoverPopup>().HoverStart();
            }
        }

        public virtual void OnPointerClick(PointerEventArgs e)
        {
            if(e.target.GetComponent<HoverPopup>() != null)
            {
                e.target.GetComponent<HoverPopup>().HoverClick.Invoke();
            }
            if(e.target.GetComponent<Button>() != null)
            {
                e.target.GetComponent<Button>().Select();
                e.target.GetComponent<Button>().onClick.Invoke();
            }
            //flick checks
            if (flickHolding)
            {
                flickFinish = transform.position + (transform.forward * flickDistance);
                var diff = flickFinish - flickStart;
                var td = Time.time - flickT;
                diff = diff / td;
                flickTrigger.GetComponent<Flickable>().Flick(diff);
            }
            flickHolding = false;
        }

        public virtual void OnPointerHold(PointerEventArgs e)
        {
            if(e.target.GetComponent<Flickable>())
            {
                flickTrigger = e.target.gameObject;
                flickDistance = e.distance;
                flickStart = transform.position + (transform.forward * flickDistance);
                flickT = Time.time;
                flickHolding = true;
            }
        }

        public virtual void OnPointerOut(PointerEventArgs e)
        {
            if (e.target.GetComponent<HoverPopup>() != null)
            {
                e.target.GetComponent<HoverPopup>().HoverStop();
            }
        }

        public void TriggerPressed() { triggerReleased = true; }

        public void TriggerDown() { triggerDown = true; }

        private void Update()
        {
            if (!isActive)
            {
                isActive = true;
                this.transform.GetChild(0).gameObject.SetActive(true);
            }

            float dist = MaxDistance;

            Ray raycast = new Ray(transform.position, transform.forward);
            RaycastHit hit;
            bool bHit = Physics.Raycast(raycast, out hit, MaxDistance);

            if (previousContact && previousContact != hit.transform)
            {
                PointerEventArgs args = new PointerEventArgs();
                args.fromInputSource = pose.inputSource;
                args.distance = 0f;
                args.flags = 0;
                args.target = previousContact;
                OnPointerOut(args);
                previousContact = null;
            }
            if (bHit && previousContact != hit.transform)
            {
                PointerEventArgs argsIn = new PointerEventArgs();
                argsIn.fromInputSource = pose.inputSource;
                argsIn.distance = hit.distance;
                argsIn.flags = 0;
                argsIn.target = hit.transform;
                OnPointerIn(argsIn);
                previousContact = hit.transform;
            }
            if (!bHit)
            {
                previousContact = null;
            }
            if (bHit && hit.distance < 100f)
            {
                dist = hit.distance;
            }

            if (bHit && triggerReleased)
            {
                triggerReleased = false;
                PointerEventArgs argsClick = new PointerEventArgs();
                argsClick.fromInputSource = pose.inputSource;
                argsClick.distance = hit.distance;
                argsClick.flags = 0;
                argsClick.target = hit.transform;
                OnPointerClick(argsClick);
            }

            if(bHit && triggerDown)
            {
                triggerDown = false;
                PointerEventArgs argsClick = new PointerEventArgs();
                argsClick.fromInputSource = pose.inputSource;
                argsClick.distance = hit.distance;
                argsClick.flags = 0;
                argsClick.target = hit.transform;
                OnPointerHold(argsClick);
            }

            if (interactWithUI != null && interactWithUI.GetState(pose.inputSource))
            {
                pointer.transform.localScale = new Vector3(thickness * 5f, thickness * 5f, dist);
                pointer.GetComponent<MeshRenderer>().material.color = clickColor;
            }
            else
            {
                pointer.transform.localScale = new Vector3(thickness, thickness, dist);
                pointer.GetComponent<MeshRenderer>().material.color = color;
            }
            pointer.transform.localPosition = new Vector3(0f, 0f, dist / 2f);
        }
    }
}