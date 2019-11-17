using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShatterCallFissure : MonoBehaviour
{
    // Start is called before the first frame update

    /*
     * AoE, Angle, Force, Damage ++
     * Duration sabit
     * Linger duration sabit
     * Effect duration (charge time) -- 
     */


        
    public Transform FissureLane, SpikeLaneLeft, SpikeLaneRight;
    public List<GameObject> SpikePrefabs;
    public GameObject FissureSlice;
    
    [Tooltip("Time it takes in seconds to fade.")]
    public float FadeTime;
    [Tooltip("The distance between deploys.")]
    public float DeployDistance;

    [Tooltip("The distance that spikes should follow the fissure from.")]
    public float SpikeDelay;
    [Tooltip("The yaw of spikes in degrees. Negative will have the spikes poking towards the fissure.")]
    public float SpikeAngle;

    private float _dist = 0; // Keeps track of distance traversed so far.
    private float _deployDist = 0; // Keeps track of distance since last deploy.
    private float _vel; // Travel speed of instance, units per second.
    private int _state = -1; // -1: Unset, 0: Charging, 1: Going, 2: Fading
    private float _localTimer = 0; // To keep track of charge and fade.
    private List<GameObject> fissures = new List<GameObject>(); // List of fissure instanes.
    private List<GameObject> spikes = new List<GameObject>(); // List of spike instances.

    private float AoE, Duration, EffectDuration, Angle, Force, Damage;

    public void SetSkill(float aoe, float dur, float edur, float angle, float f, float dmg)
    {
        AoE = aoe; Duration = dur; EffectDuration = edur; Angle = angle; Force = f; Damage = dmg; // Pass the variables from the skill manager.
        _vel = AoE / Duration; // Calculate instance velocity.
        _state = 0; // The skill is now ready to start charging.

        // Set starting positions for transforms.
        FissureLane.position = transform.position;
        FissureLane.rotation = transform.rotation;
        
        SpikeLaneLeft.position = transform.position;

        SpikeLaneLeft.rotation = transform.rotation;
        //SpikeLaneLeft.Rotate(0, 0, SpikeAngle);
        SpikeLaneLeft.Rotate(0, angle / 2, 0, Space.World);

        SpikeLaneRight.position = transform.position;
        SpikeLaneRight.rotation = transform.rotation;
        //SpikeLaneRight.Rotate(0, 0, -SpikeAngle);
        SpikeLaneRight.Rotate(0, -angle / 2, 0, Space.World);
    }

    private void DeployPrefabs()
    {
        _deployDist = 0;
    

        if (_dist>SpikeDelay && SpikeDelay > 0)
        {
            //Deploy and position spikes. Keep spike delay in mind.
            GameObject s1 = Instantiate(SpikePrefabs[Random.Range(0, SpikePrefabs.Count)]);
            s1.transform.position = SpikeLaneLeft.position - SpikeDelay * SpikeLaneLeft.forward;
            GameObject s2 = Instantiate(SpikePrefabs[Random.Range(0, SpikePrefabs.Count)]);
            s2.transform.position = SpikeLaneRight.position - SpikeDelay * SpikeLaneRight.forward;

            // Randomize locations a bit.
            float r = 0.5f;
            
            s1.transform.position += new Vector3(Random.Range(-r, r), -.3f, Random.Range(-r, r));
            s2.transform.position += new Vector3(Random.Range(-r, r), -.3f, Random.Range(-r, r));

            // Adjust for terrain.
            float y1 = Terrain.activeTerrain.SampleHeight(s1.transform.position);
            float y2 = Terrain.activeTerrain.SampleHeight(s2.transform.position);
            s1.transform.position = new Vector3(s1.transform.position.x, y1, s1.transform.position.z);
            s2.transform.position = new Vector3(s2.transform.position.x, y2, s2.transform.position.z);

            // Add new spikes to the list.
            spikes.Add(s1);
            spikes.Add(s2);


            s1.GetComponentInChildren<ShatterCallObstacle>().SetSkill(Force, Damage);
            s2.GetComponentInChildren<ShatterCallObstacle>().SetSkill(Force, Damage);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // If state is -1 don't do anything as the skill has not been set.

        if(_state == 0) // Charging.
        {
            // Charge up the skill.
            _localTimer += Time.deltaTime;
            if (_localTimer >= EffectDuration) _state = 1;
        }
        else if(_state == 1) // Charged and going.
        {
            var distAdd = _vel * Time.deltaTime; // Distance the lanes will go forward for this frame.
            if (_dist < AoE) // If the fissure has not reached its total distance yet:
            {
                // Track added distances.
                _dist += distAdd;
                _deployDist += distAdd;
                if (_dist > AoE) _dist = AoE;
                // Move points forward.
                SpikeLaneLeft.position = SpikeLaneLeft.position + SpikeLaneLeft.forward * distAdd;
                SpikeLaneRight.position = SpikeLaneRight.position + SpikeLaneRight.forward * distAdd;
                FissureLane.position = FissureLane.position + FissureLane.forward * distAdd;

                // Enlarge the slice.
                FissureSlice.transform.localScale = new Vector3(Vector3.Distance(SpikeLaneLeft.position, SpikeLaneRight.position), 1, Vector3.Distance(FissureLane.position, transform.position));

            }
            if (_dist == AoE && SpikeDelay > 0) // If the spikes have not reached their total distance yet:
            {
                SpikeDelay -= distAdd;
                if (SpikeDelay < 0) SpikeDelay = 0;
            }
            if (_deployDist >= DeployDistance) DeployPrefabs(); // Deploy if traversed far enough from the last deployment.
            if (_dist == AoE && SpikeDelay == 0) // If everything reached its total distance:
            {
                _state = 2;
                _localTimer = 0;
            }
        }
        else if(_state == 2) // Fading out.
        {
            _localTimer += Time.deltaTime;
            // Fade everything out.
            Color c;
            /*if (FissurePrefab.GetComponent<Renderer>().sharedMaterial)
            {
                for (int i = 0; i < fissures.Count; i++)
                {
                    c = fissures[i].GetComponent<Renderer>().sharedMaterial.color;
                    c.a -= Time.deltaTime / FadeTime;
                    fissures[i].GetComponent<Renderer>().sharedMaterial.color = c;
                }
            }*/
            if (SpikePrefabs[0].GetComponentInChildren<Renderer>().sharedMaterial)
            {
                for (int i = 0; i < spikes.Count; i++)
                {
                   // c = spikes[i].GetComponentInChildren<Renderer>().sharedMaterial.color;
                    //c.a -= Time.deltaTime / FadeTime;
                    //spikes[i].GetComponentInChildren<Renderer>().sharedMaterial.color = c;
                }
            }
            if(_localTimer > FadeTime) // If the fade effect is complete:
            {
                // Destroy everything.
                for (int i = 0; i < fissures.Count; i++) Destroy(fissures[i]);
                for (int i = 0; i < spikes.Count; i++) Destroy(spikes[i]);
                Destroy(gameObject);
            }
        }
    }
    
}
