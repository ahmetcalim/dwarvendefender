using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Feedback : MonoBehaviour
{
    public static string FeedbackTxt;
    private void Update()
    {
        GetComponent<Text>().text = FeedbackTxt;
    }
}
