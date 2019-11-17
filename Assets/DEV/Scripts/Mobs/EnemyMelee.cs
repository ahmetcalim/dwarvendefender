using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMelee : Weapon
{
    public AudioSource hitAudioSource;
    public List<AudioClip> hitClips;

    private Vector3 dir;
    private float vel;
    private Vector3 _lastPos;

    private GameObject hitTarget;
    private Rigidbody hitRigidbody;
    private float _hitTimer = 0;
    public float HitCooldown = 0.75f;

    public float Force = 0;

    void Start()
    {
        // EnemyWeapon = true; // Tentative. Could be checked in the inspector or in script.
        _lastPos = transform.position; // Getting rid of those pesky first-frame null exceptions.
    }
    private void Update()
    {
        #region Vector Control
        dir = (transform.position - _lastPos).normalized; // Direction of the weapon.
        vel = Vector3.Magnitude(transform.position - _lastPos) * Time.deltaTime; // Velocity of the weapon.
        _lastPos = transform.position; // Tracking last position.
        #endregion

        // Check velocity, set damage level index.
        #region Damage Level Index Control
        if (vel >= 9f)
        {
            DamageLevelIndex = 4;
        }
        else if(vel >= 7f)
        {
            DamageLevelIndex = 3;
        }
        else if(vel >= 3.5f)
        {
            DamageLevelIndex = 2;
        }
        else if(vel >= 1.5f)
        {
            DamageLevelIndex = 1;
        }
        else
        {
            DamageLevelIndex = 0;
        }
        #endregion

        // Hit cooldown control.
        if(_hitTimer > 0)
        {
            _hitTimer -= Time.deltaTime;
            if (_hitTimer < 0) _hitTimer = 0;
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if (GetComponentInParent<Mob>().dead || GetComponentInParent<Mob>().sliced) return;
        if (!GetComponentInParent<Mob>().attacking) return;
        if (_hitTimer > 0) return; // If hit is on cooldown, return.
        hitTarget = null;
        hitRigidbody = null;

        if (col.gameObject.GetComponent<Weapon>()) // If the weapon struck a weapon:
        {
            hitTarget = col.gameObject;
            if (hitTarget.GetComponent<Weapon>().EnemyWeapon) return; // If it is an enemy weapon, return.
            if (GetComponentInParent<Mob>()) GetComponentInParent<Mob>().Stagger(); // If it is not an enemy weapon, stagger.
            return;
        }
        if (col.gameObject.GetComponentInChildren<PlayerHealthTracker>()) // If the target is a player:
        {
            hitTarget = col.gameObject.GetComponentInChildren<PlayerHealthTracker>().gameObject; // Tracking last hit target just in case.
            hitTarget.GetComponent<PlayerHealthTracker>().TakeDamage(Damage); // Player takes damage.
            hitRigidbody = col.gameObject.GetComponent<Rigidbody>(); // Set hitRigidbody whenever a target can be affected by physics.
            _hitTimer = HitCooldown;
        }
        if (col.gameObject.GetComponent<Spire>()) // If the target is the spire:
        {
            hitTarget = col.gameObject; // Tracking last hit target just in case.
            hitTarget.GetComponent<Spire>().TakeDamage(Damage); // Spire takes damage.
            _hitTimer = HitCooldown;
        }
    }
}
