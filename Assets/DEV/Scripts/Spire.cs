using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Spire : MonoBehaviour
{
    public float baseTime;
    public float maxHp;
    public float baseHp;
    public float hp;
    public UnityEvent growFinished = new UnityEvent();
    public UnityEvent onSpireDie = new UnityEvent();
    public Image healthDisplayer;
    public Animator healthDisplayerAnimator;

    public float realtimeOnLevel;
    public float maxScale;
    public float scale;
    public GameObject spireobj;
    public Text hpDisplayer;
    public float interval;
    private float hpref;
    private float damageAmount;
    public List<Animator> spireAnimators;
    private float timer = 1f;
    public static float spireHp;
    public List<float> spirePieceScale;
    public float pieceScale = 0f;
    public int pieceIndex;
    public static float timePlayed;
    private bool orcSpawned;
    private bool spireDead;
    public MeshRenderer hpBar;
    private void Start()
    {
        BossDwarf.Win = false;
        AchievementManager.Instance.gameEvent.ResetGameEvents();
        
    }
    private void Update()
    {
        realtimeOnLevel = Time.timeSinceLevelLoad;
        timePlayed = Time.timeSinceLevelLoad;
        hpBar.sharedMaterial.SetFloat("_Slider", hp / maxHp);
        if (hpDisplayer)
        {
            hpDisplayer.text = hp.ToString("F0") + "/" + maxHp;
        }
        if (hp < 0)
        {
            if (!spireDead)
            {

                onSpireDie.Invoke();
                AchievementManager.Instance.gameEvent.GameFinished.Invoke();
                spireDead = true;
            }
        }
        if (scale < 1)
        {
            hpref = (baseHp + ((maxHp - baseHp) / baseTime) * realtimeOnLevel) - damageAmount;
            hp = hpref;
            scale = (maxScale / baseTime) * realtimeOnLevel * Mathf.Pow((hp / maxHp), .5f);
            if (scale > .7f && !orcSpawned)
            {
                for (int i = 0; i < FindObjectOfType<MobSpawn>().mobPrefabs.Count; i++)
                {
                    FindObjectOfType<MobSpawn>().mobPrefabs[i] = FindObjectOfType<MobSpawn>().orcPrefab;
                }
                orcSpawned = true;
            }
            if(orcSpawned)
            {
                FindObjectOfType<MobSpawn>().mobPrefabs = FindObjectOfType<MobSpawn>().defaultMobList;

            }
            if (pieceIndex < spirePieceScale.Count)
            {
                if (pieceScale < 1f)
                {
                    if (pieceScale < 0)
                    {
                        pieceIndex--;
                    }
                    pieceScale = (scale * 10f) - pieceIndex;
                    spirePieceScale[pieceIndex] = pieceScale;
                    spireAnimators[pieceIndex].SetFloat("Blend", spirePieceScale[pieceIndex]);

                }
                else
                {
                    pieceIndex++;
                    pieceScale = 0f;
                }
            }
          
            if (healthDisplayer)
            {
                healthDisplayer.fillAmount = hp / maxHp;
            }
           
        }
        else
        {
            if (hp > 0)
            {
                foreach (var item in spireAnimators)
                {
                    if (item.GetFloat("Blend") > 0)
                    {
                        //item.SetFloat("Blend", timer);
                        timer -= Time.deltaTime / 4f;
                    }
                    else
                    {
                        timer = 1f;
                    }
                }
                spireHp = hp;
                growFinished.Invoke();
                BossDwarf.Win = true;
                if (AchievementManager.Instance.gameEvent.GameFinished != null)
                {
                    AchievementManager.Instance.gameEvent.GameFinished.Invoke();

                }
            }


        }
      
    }
    
    public static float GetSpireCurrentHealth()
    {
        return spireHp;
    }
    public void TakeDamage(float damage)
    {
        Debug.Log("SPİRE HASAR ALIYOR");
        damageAmount += damage;
        if (healthDisplayer)
        {
            healthDisplayerAnimator.SetTrigger("spirehealthgg");
        }
      
    }
}
