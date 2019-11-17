using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundFXController : MonoBehaviour
{
    public List<AudioClip> hitClips;
    public List<AudioClip> death;
    public List<AudioClip> talks;
    public List<AudioClip> attacks;
    public AudioSource source;
    public void PlayHitClip()
    {
        if (!GetComponent<Mob>().puppet.isAlive) return;
      
        if (hitClips.Count > 0)
        {
            if (source.isPlaying) return;
            source.PlayOneShot(hitClips[Random.Range(0, hitClips.Count)]);
        }
    }
    public void PlayDEathClip()
    {
        if (!GetComponent<Mob>().puppet.isAlive) return;
        if (death.Count > 0)
        {
            source.PlayOneShot(death[Random.Range(0, death.Count)]);
        }
    }
    public void PlayTalksClip()
    {
        if (!GetComponent<Mob>().puppet.isAlive) return;
        if (talks.Count > 0)
        {
            if (source.isPlaying) return;
          
            source.PlayOneShot(talks[Random.Range(0, talks.Count)]);
        }
    }
    public void PlayAttackClip()
    {
        if (!GetComponent<Mob>().puppet.isAlive) return;
        if (attacks.Count > 0)
        {
            if (source.isPlaying) return;
            source.PlayOneShot(attacks[Random.Range(0, attacks.Count)]);
        }
    }
}
