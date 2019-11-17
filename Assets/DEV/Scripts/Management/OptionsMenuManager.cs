using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenuManager : MonoBehaviour
{
    public Slider MusicSlider;
    public Slider SoundSlider;

    private bool _particles = false;
    public Button ParticleToggle;
    public Sprite ToggleOn;
    public Sprite ToggleOff;

    public Image[] qualityButton;
    public Sprite[] qualityPassive;
    public Sprite[] qualityActive;

    private void Start()
    {
        if (!PlayerPrefs.HasKey("SoundVolume"))
        {
            PlayerPrefs.SetFloat("SoundVolume", 1);
        }
        if (!PlayerPrefs.HasKey("MusicVolume"))
        {
            PlayerPrefs.SetFloat("MusicVolume", 1);
        }
        if (!PlayerPrefs.HasKey("QualityValue"))
        {
            PlayerPrefs.SetInt("QualityValue", 3);
        }
        if (!PlayerPrefs.HasKey("ParticleValue"))
        {
            PlayerPrefs.SetInt("ParticleValue", 0);
        }
        SoundSlider.value = PlayerPrefs.GetFloat("SoundVolume");
        MusicSlider.value = PlayerPrefs.GetFloat("MusicVolume");
        SetQuality(PlayerPrefs.GetInt("QualityValue"));

        if (PlayerPrefs.GetInt("ParticleValue") == 0)
        {
            _particles = false;
            ParticleToggle.GetComponent<Image>().sprite = null;
            ParticleToggle.GetComponent<Image>().sprite = ToggleOff;
        }
        else
        {
            _particles = true;
            ParticleToggle.GetComponent<Image>().sprite = null;
            ParticleToggle.GetComponent<Image>().sprite = ToggleOn;
        }
    }


    public void SetQuality(int level)
    {
        QualitySettings.SetQualityLevel(level, true);
        for (int i = 0; i < qualityButton.Length; i++)
            qualityButton[i].sprite = qualityPassive[i];

        qualityButton[level - 1].sprite = qualityActive[level - 1];
        PlayerPrefs.SetInt("QualityValue", level);
    }

    public void SetSoundLevel(float value)
    {
        SoundSlider.value += value;
        PlayerPrefs.SetFloat("SoundVolume", SoundSlider.value);
        //TÜm audio sourcelara etki et
    }

    public void SetMusicLevel(float value)
    {
        MusicSlider.value += value;
        PlayerPrefs.SetFloat("MusicVolume", MusicSlider.value);
        //TÜm audio sourcelara etki et
    }

    public void ToggleParticles()
    {
        if (_particles)
        {
            _particles = false;
            ParticleToggle.GetComponent<Image>().sprite = null;
            ParticleToggle.GetComponent<Image>().sprite = ToggleOff;
            PlayerPrefs.SetInt("ParticleValue", 0);
            //Particle ları kapat
        }
        else
        {
            _particles = true;
            ParticleToggle.GetComponent<Image>().sprite = null;
            ParticleToggle.GetComponent<Image>().sprite = ToggleOn;
            PlayerPrefs.SetInt("ParticleValue", 1);
            //Particle ları kapat
        }
    }

    public void Reset()
    {
        MusicSlider.value = 1;
        PlayerPrefs.SetFloat("MusicVolume", MusicSlider.value);

        SoundSlider.value = 1;
        PlayerPrefs.SetFloat("SoundVolume", SoundSlider.value);

        _particles = false;
        ParticleToggle.GetComponent<Image>().sprite = null;
        ParticleToggle.GetComponent<Image>().sprite = ToggleOff;
        PlayerPrefs.SetInt("ParticleValue", 0);

        SetQuality(3);
    }
}
