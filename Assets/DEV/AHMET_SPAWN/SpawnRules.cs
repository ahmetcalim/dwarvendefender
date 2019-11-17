using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnRules : MonoBehaviour
{
    public static int maxOnScene = 10;
    public static int maxOnSpire = 6;
    public static int maxOnPlayer = 4;
    public static int maxOnSpireCurrent;
    public int maxOnSceneBase;
    public float mapDifficulty;
    public int currentMaxOnScene;
    public Spire spire;
 
    private void Start()
    {
        maxOnSpireCurrent = 0;
    }
    private void Update()
    {
      
        if (currentMaxOnScene < maxOnScene)
        {
            currentMaxOnScene = (int)(maxOnSceneBase * (spire.hp / spire.maxHp + 1) * Mathf.Pow((spire.scale / spire.maxScale + 1), 2) * mapDifficulty);

           
        }
    }
}
