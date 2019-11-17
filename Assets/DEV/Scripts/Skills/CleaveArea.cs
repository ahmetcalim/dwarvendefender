using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CleaveArea : MonoBehaviour
{
    [Tooltip("Arbitrary decay control variable. Multiplies with time.")]
    public float DecayMult = 1.25f;

    private float Duration = 0.1f, Damage, AoE = 3; // Variables taken from the skill generator.

    private float _decayTime; // Variable derived from Duration * DecayMult
    private float _time = 0; // Timer to track the skill state.
    private Mob _initialEnemy;
    private float _initialAlpha;

    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = new Vector3(0, transform.localScale.y, 0); // Start from zero.
        Color c = GetComponent<MeshRenderer>().material.color;
        _initialAlpha = c.a;
    }

    public void SetSkill(float dur, float dmg, float aoe, Mob init)
    {
        Duration = dur; Damage = dmg; AoE = aoe;
        _initialEnemy = init;
        transform.Rotate(Random.Range(-5, 5), 0, Random.Range(-5, 5)); // Random rotation
        _decayTime = DecayMult * Duration;
    }

    // Update is called once per frame
    void Update()
    {
        // size increase
        _time += Time.deltaTime;
        var add = AoE * Time.deltaTime / Duration;
        transform.localScale += new Vector3(add, 0, add);

        // state control
        if (_time > _decayTime) Destroy(gameObject);
        if (_time > Duration) GetComponent<Collider>().enabled = false;

        // fade out
        Color c = GetComponent<MeshRenderer>().material.color;
        c.a = (1 - (_time / _decayTime)) * _initialAlpha;
        GetComponent<MeshRenderer>().material.color = c;
    }

    private void OnTriggerEnter(Collider other)
    {
        Mob e;
        if(e = other.GetComponentInParent<Mob>())
        {
            if (e == _initialEnemy) return;

           // e.TakeDamage(Damage);
            // Apply damage here.
        }
    }
}
