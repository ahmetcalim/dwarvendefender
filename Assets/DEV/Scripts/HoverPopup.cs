using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Valve.VR.InteractionSystem;

public class HoverPopup : MonoBehaviour {

    public string PopupText;
    public Canvas PopupCanvas;
    private Text PopupTextBox;
    public Vector3 OffsetVector;
    public UnityEvent HoverClick;

    // Use this for initialization
    void Start() {
        PopupCanvas.enabled = false;
        PopupTextBox = PopupCanvas.GetComponentInChildren<Text>();
        if(HoverClick == null)
        {
            HoverClick = new UnityEvent();
        }
    }

    // Update is called once per frame
    void Update() {

    }

    public void HoverStart()
    {
        PopupTextBox.text = PopupText;
        PopupCanvas.enabled = true;

        // put canvas above hovered item
        Vector3 targetPos = new Vector3(transform.position.x + OffsetVector.x,
                                        transform.position.y + OffsetVector.y,
                                        transform.position.z + OffsetVector.z);
        PopupCanvas.transform.position = Vector3.MoveTowards(PopupCanvas.transform.position, targetPos, 1000.0f);

        //point canvas towards camera
        //PopupCanvas.transform.LookAt(Camera.current.transform);
        PopupCanvas.transform.rotation = Quaternion.LookRotation(PopupCanvas.transform.position - Camera.main.transform.position);
    }
    public void HoverStop()
    {
        PopupCanvas.enabled = false;
        PopupTextBox.text = "sample text";
    }
    
}

