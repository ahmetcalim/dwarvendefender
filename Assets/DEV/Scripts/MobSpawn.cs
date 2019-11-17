using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class MobSpawn : MonoBehaviour
{
    public static int objectCount;

    public int spawnedObject;
    public List<Transform> spawnPoints;
    private bool rmInitialized = false;
    public static int killCount;
    public List<GameObject> mobPrefabs;
    private Vector3 currentOccuredPos;
    public Text currentObjectCount;
    public Mob[] mobsAlive;
    public Mob archerPrefab;
    public static int onSpire;
    public bool gameFinished;
    public static int onPlayer;
    public GameObject orcPrefab;
    int mobc;
    public GameObject spireDead;
    public List<GameObject> defaultMobList = new List<GameObject>(4);
    public GameObject spireWin;
    public void FinishTheGame()
    {
        gameFinished = true;
        
        StartCoroutine(GoDeadScene());
    }
    public void KillAllEnemies()
    {
        foreach (var item in mobsAlive)
        {
            if (item)
            {
                if (item.puppet)
                {
                    item.puppet.Kill();
                }
            }
        }
    }
    private IEnumerator GoDeadScene()
    {
        Destroy(spireWin);
        spireDead.SetActive(true);
        yield return new WaitForSeconds(13f);
        SceneManager.LoadScene("DeadScene");
    }
    private void Start()
    {
        onPlayer = 0;
           onSpire = 0;
        killCount = 0;
        objectCount = 0;
        Time.timeScale = 1.0f;
        defaultMobList = mobPrefabs;
        StartCoroutine(BeginningOfGame());
  
    }
    private IEnumerator BeginningOfGame()
    {
        yield return new WaitForSeconds(1);
        GameObject.FindGameObjectWithTag("Horn").GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(5);
        StartCoroutine(RecursiveSpawn());
    }
    public IEnumerator RecursiveSpawn()
    {
        if (gameFinished) yield return null;
            yield return new WaitForSeconds(2f);
        if (MobOnSceneAvailable())
        {
            SpawnMob(1);
        }
        StartCoroutine(RecursiveSpawn());
    }
    void SpawnMob(int spawnSize)
    {
        for (int i = 0; i < spawnSize; i++)
        {
            Spawn();
        }
       
    }
    public bool MobOnSceneAvailable()
    {
        mobsAlive = GetComponentsInChildren<Mob>();

        spawnedObject = mobsAlive.Where(t => !t.dead && !t.sliced).ToList().Count;
        onSpire = mobsAlive.Where(t => !t.dead && !t.sliced && t.target.CompareTag("SpireAttackPoint")).ToList().Count;
        onPlayer = mobsAlive.Where(t => !t.dead && !t.sliced && t.target.CompareTag("PlayerAttackPoint")).ToList().Count;
        if (spawnedObject < GetComponent<SpawnRules>().currentMaxOnScene)
        {
            if (onPlayer < 5)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
     
    }
    private void Spawn()
    {
    
        Vector3 sourcePostion = GetComponent<SpawnPoint>().GetRandomPoint().position;//The position you want to place your agent
        NavMeshHit closestHit;
        if (NavMesh.SamplePosition(sourcePostion, out closestHit, 500, 1))
        {

            
           
            GameObject spawn = Instantiate(mobPrefabs[Random.Range(0, mobc)], closestHit.position, Quaternion.identity).gameObject;
            spawn.transform.SetParent(transform);
            spawn.SetActive(false);

            spawn.SetActive(true);
           
        }
        else
        {
            Debug.Log("...");
        }
    }

    private bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        Vector3 randomPoint = center + Random.insideUnitSphere * range;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, range, NavMesh.AllAreas))
        {
            result = hit.position;
            return true;
        }
        result = Vector3.zero;
        return false;
    }
}

