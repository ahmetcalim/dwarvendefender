using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealthTracker : MonoBehaviour
{
    public float MaxHealth = 100;
    public float Health;
    public float RegenPerSecond = 5;
    public float RegenDelay = 10;

    private float _regenTracker;
    private Image _panel;
    private float _maxAlphaTracker;
    private float indicator;
    public UnityEvent onPlayerDie = new UnityEvent();
    public MeshRenderer healthBar;
    public void TakeDamage(float dmg)
    {
        if (GetComponent<Animator>())
        {
            GetComponent<Animator>().SetTrigger("TakeDamage");
        }


        Health -= dmg;
        
       
        if (Health <= 0) ZeroHP();
        _regenTracker = RegenDelay;

        var newAlpha = _maxAlphaTracker * (1 - Health / MaxHealth);
        //_panel.color = new Color(_panel.color.r, _panel.color.g, _panel.color.b, newAlpha);
        // Apply things to cam here.
    }

    private void ZeroHP()
    {
        onPlayerDie.Invoke();
        DeadSceneController.restartIndex = SceneManager.GetActiveScene().buildIndex;
        AchievementManager.Instance.gameEvent.GameFinished.Invoke();
        GetComponentInParent<Rigidbody>().constraints = RigidbodyConstraints.None;
    }

    void Start()
    {
        Health = MaxHealth;
        _panel = GetComponentInChildren<Image>();
        //_maxAlphaTracker = _panel.color.a;
        var newAlpha = _maxAlphaTracker * (1 - Health / MaxHealth);
        //_panel.color = new Color(_panel.color.r, _panel.color.g, _panel.color.b, newAlpha);
    }

    // Update is called once per frame
    void Update()
    {
        indicator = MaxHealth - Health;
        if (healthBar)
        {
            healthBar.sharedMaterial.SetFloat("_Slider", Health / MaxHealth);
        }
     
        if (_regenTracker > 0)
        {
            _regenTracker -= Time.deltaTime;
            if (_regenTracker < 0) _regenTracker = 0;
        }
        if(Health < MaxHealth && _regenTracker == 0)
        {
            Health += RegenPerSecond * Time.deltaTime;
            if (Health > MaxHealth) Health = MaxHealth;
            var newAlpha = _maxAlphaTracker * (1 - Health / MaxHealth);
           
            //_panel.color = new Color(_panel.color.r, _panel.color.g, _panel.color.b, newAlpha);
        }
    }
}
