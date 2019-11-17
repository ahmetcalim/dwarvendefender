using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;
public class EndGameCondition : MonoBehaviour
{
    public GameObject spireDiedExplosion;
    public GameObject spireWinExplosion;
    public Rigidbody playerBody;
    public GameObject winConditionGate;
    public MobSpawn mobSpawnSystem;


    public void SpireCompleted()
    {
        //TO DO Patlamayı gerçekleştir.
        spireWinExplosion.SetActive(true);
        //TO DO Mobları öldür.


        //TO DO Portalı aç

        StartCoroutine(KillAllEnemiesOnScene());
    }
    public void PlayerDied()
    {
        //TO DO Player ı yık
        playerBody.constraints = RigidbodyConstraints.None;

        //TO DO Ölüm sahnesine yönlendir.
        StartCoroutine(ThrowPlayerToDeadScene(4f));
        if (mobSpawnSystem)
        {
            mobSpawnSystem.KillAllEnemies();
        }
        else
        {
            mobSpawnSystem = FindObjectOfType<MobSpawn>();
            mobSpawnSystem.KillAllEnemies();
            mobSpawnSystem.enabled = false;
        }

    }
    public void SpireDied()
    {
        //TO DO Patlamayı gerçekleştir.
        spireDiedExplosion.SetActive(true);
        //TO DO Spire içine kaçsın
        DeadSceneController.restartIndex = SceneManager.GetActiveScene().buildIndex;
        AchievementManager.Instance.gameEvent.GameFinished.Invoke();

        //TO DO Ölüm sahnesine yönlendir
        StartCoroutine(ThrowPlayerToDeadScene(10f));
        if (mobSpawnSystem)
        {
            mobSpawnSystem.KillAllEnemies();
        }
        else
        {
            mobSpawnSystem = FindObjectOfType<MobSpawn>();
            mobSpawnSystem.KillAllEnemies();
            mobSpawnSystem.enabled = false;
        }
    }
    public IEnumerator ThrowPlayerToDeadScene(float after)
    {
        yield return new WaitForSeconds(after);
        SceneManager.LoadScene("DeadScene");
    }
    private IEnumerator KillAllEnemiesOnScene()
    {
        yield return new WaitForSeconds(8f);
        if (mobSpawnSystem)
        {
            mobSpawnSystem.KillAllEnemies();
        }
        else
        {
            mobSpawnSystem = FindObjectOfType<MobSpawn>();
            mobSpawnSystem.KillAllEnemies();
            mobSpawnSystem.enabled = false;
        }
        winConditionGate.SetActive(true);
    }
}
