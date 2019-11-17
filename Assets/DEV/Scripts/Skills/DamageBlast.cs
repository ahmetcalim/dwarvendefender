using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageBlast : MonoBehaviour
{
    [Tooltip("Duration multiplier that changes visible duration, compared to active duration.")]
    public float DecayMult = 1.25f;

    private float Damage = 1, Duration = 5, AoE = .5f;
    private float _decayTime;
    private float _time = 0;

    public void SetSkill(float dmg, float dur, float aoe)
    {
        Damage = dmg; Duration = dur; AoE = aoe;
        _decayTime = Duration * DecayMult;
    }

    // Update is called once per frame
    void Update()
    {
        _time += Time.deltaTime; // Increment timer.
        var _add = AoE * Time.deltaTime / Duration; // Calculate the addition to the size.
        transform.localScale += new Vector3(_add, _add, _add); // Increase size.

        // Fade away.
        Color c = GetComponent<MeshRenderer>().material.color;
        c.a = 1 - (_time / _decayTime);
        GetComponent<MeshRenderer>().material.color = c;

        if (_time > _decayTime) Destroy(gameObject);
        if (_time > Duration) GetComponent<Collider>().enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponentInParent<Mob>())
        {
            other.GetComponentInParent<Mob>().TakeDamage(Damage);
        }
    }
}
