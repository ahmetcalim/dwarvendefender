using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;

public class PointyFingerController : MonoBehaviour
{
    public SteamVR_Input_Sources source;
    public SteamVR_Action_Vibration vibration;

    private float _flickmult = 0.7f;
    private Vector3 _pos, _lastpos;
    private bool hapticFeedback;

    private bool _pressing = false;
    private float _pressTimer = 0;
    public float PressTime = 2.0f;
    public float DefaultPressTime = 2.0f;
    public GameObject LoadFeedback;
    private Transform _target;
    public Image loadBar;
    private float HoverDistance = 0.03f;
    public float upgradePressTime;
    IEnumerator ExecuteHaptic(float t)
    {
        hapticFeedback = true;
        yield return new WaitForSeconds(t);
        hapticFeedback = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        LoadFeedback.transform.SetParent(null);
        hapticFeedback = false;
        _pos = transform.position;
        _lastpos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (hapticFeedback)
        {
            vibration.Execute(0, 0.1f, 160, 0.25f, source);
        }
        _lastpos = _pos;
        _pos = transform.position;
        if (_pressing)
        {
            if (!_target.gameObject.activeInHierarchy)
            {
                ResetLoadFeedback();
            }
          
            _pressTimer += Time.deltaTime * ( 1f / Time.timeScale) ;
            loadBar.fillAmount = _pressTimer / PressTime;
            if (_target)
            {
                LoadFeedback.transform.position = _target.position;
                LoadFeedback.transform.position = Vector3.MoveTowards(LoadFeedback.transform.position, Camera.main.transform.position, HoverDistance);
                LoadFeedback.transform.rotation = Quaternion.LookRotation(Camera.main.transform.position - LoadFeedback.transform.position);
            }
        }
            
    }

    private void SetLoadFeedback(Transform t)
    {
        _pressing = true;
        _pressTimer = 0;
        if (LoadFeedback)
        {
            LoadFeedback.SetActive(true);
            _target = t;
        }
    }

    private void ResetLoadFeedback()
    {
        _pressing = false;
        _pressTimer = 0;
        if (LoadFeedback)
        {
            LoadFeedback.SetActive(false);
            _target = null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Menu"))
        {
            SetLoadFeedback(other.transform);

        }
        else if (other.CompareTag("Upgrade"))
        {
            SetLoadFeedback(other.transform);
            PressTime = upgradePressTime;
        }
        else if (other.CompareTag("Options"))
        {
            SetLoadFeedback(other.transform);
            PressTime = upgradePressTime;
        }
        else
        {
            if (other.GetComponent<Button>())
            {
                if (!other.GetComponent<Button>().interactable) return;
                if (other.GetComponent<CampaignDataDisplayer>())
                {
                    other.GetComponent<CampaignDataDisplayer>().PopupAt(other.transform);
                }
                SetLoadFeedback(other.transform);
            }
        }
        if (other.CompareTag("LevelUI"))
        {
            GetComponentInParent<Animator>().SetTrigger("pointer");
        }
       
    }

    private void OnTriggerExit(Collider other)
    {
        PressTime = DefaultPressTime;
        if (other.GetComponent<Flickable>())
        {
            var f = (_pos - _lastpos) * Time.deltaTime * _flickmult;
            other.GetComponent<Flickable>().Flick(f);
        }

        else if (other.GetComponent<Button>())
        {
            ResetLoadFeedback();
            StartCoroutine(ButonEventAfterSeconds(other.gameObject));
        }
        else if (other.CompareTag("Menu"))
        {
            other.GetComponent<ObjectInteractionBehaviour>().hoverText.SetActive(false);
            ResetLoadFeedback();
        }
        if (other.CompareTag("LevelUI"))
        {
            GetComponentInParent<Animator>().SetTrigger("idle");
        }
    }
    
    IEnumerator ButonEventAfterSeconds(GameObject other)
    {
        yield return new WaitForSeconds(.2f);
        if (other.GetComponent<CampaignDataDisplayer>())
        {
            if (other.GetComponent<CampaignDataDisplayer>().PopupObject.activeSelf)
            {
                other.GetComponent<CampaignDataDisplayer>().PopupDisable();
            }
           
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Menu"))
        {
            other.GetComponent<ObjectInteractionBehaviour>().hoverText.SetActive(true);
            if (_pressing && _pressTimer >= PressTime)
            {

                if (other.GetComponent<ObjectInteractionBehaviour>())
                {
                    other.GetComponent<ObjectInteractionBehaviour>().DoEventByBool();
                }
                ExecuteHaptic(1);
                ResetLoadFeedback();
            }
        }
        else
        {
            if (!other.GetComponent<Button>()) return;

            if (_pressing && _pressTimer >= PressTime)
            {
                other.GetComponent<Button>().Select();
                other.GetComponent<Button>().onClick.Invoke();
                ExecuteHaptic(1);
                ResetLoadFeedback();
            }
        }
    }
}
