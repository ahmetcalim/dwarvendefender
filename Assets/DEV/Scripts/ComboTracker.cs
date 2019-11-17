using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboTracker : MonoBehaviour
{
    public static ComboTracker comboTracker;

    public int MaxCombo = 6;
    public float[] Timers = {12, 11, 10, 9, 8, 7, 6};
    public int[] KillGates = { 3, 7, 12, 16, 21, 30 };
    public ComboFeedBack feedBack;
    public float _comboTimer = 0;
    private float _killCounter = 0;
    public int comboCounter = 0;

    // Start is called before the first frame update
    void Start()
    {
        if (!comboTracker)
        {
            comboTracker = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(_comboTimer > 0)
        {
            _comboTimer -= Time.deltaTime;
            if (_comboTimer < 0) _comboTimer = 0;
            if (_comboTimer == 0)
            {
                _killCounter = 0;
                comboCounter = 0;               
            }
        }
        
    }

    public void IncrementCombo(Mob mob)
    {
        _killCounter += 1;

        if(comboCounter < MaxCombo && KillGates[comboCounter] <= _killCounter)
        {
            comboCounter += 1;

            switch (comboCounter)
            {
                case 1:
                    feedBack.feedBackText.text = "Combo 1x";
                    Instantiate(feedBack.gameObject,mob.transform.position+new Vector3(0,2,0),Quaternion.identity);
                    break;
                case 2:
                    feedBack.feedBackText.text = "Combo 2x";
                    Instantiate(feedBack.gameObject, mob.transform.position + new Vector3(0, 2, 0), Quaternion.identity);
                    break;
                case 3:
                    feedBack.feedBackText.text = "Combo 3x";
                    Instantiate(feedBack.gameObject, mob.transform.position + new Vector3(0, 2, 0), Quaternion.identity);
                    break;
                case 4:
                    feedBack.feedBackText.text = "Combo 4x";
                    Instantiate(feedBack.gameObject, mob.transform.position + new Vector3(0, 2, 0), Quaternion.identity);
                    break;
                case 5:
                    feedBack.feedBackText.text = "Combo 5x";
                    Instantiate(feedBack.gameObject, mob.transform.position + new Vector3(0, 2, 0), Quaternion.identity);
                    break;
                case 6:
                    feedBack.feedBackText.text = "Combo 6x";
                    Instantiate(feedBack.gameObject, mob.transform.position + new Vector3(0, 2, 0), Quaternion.identity);
                    break;


                default:
                    break;
            }
        }
        _comboTimer = Timers[comboCounter];
    }
}
