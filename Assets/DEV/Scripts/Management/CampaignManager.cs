using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR;
using Valve.VR;

public class CampaignManager : MonoBehaviour
{
    /*
     * Switching scenes with style.
     * Keeping instance data.
     * Keeping instance connections.
     * Handling instance win/loss.
     * Keeping weapons to pass.
     */
     
    public SteamVR_LoadLevel loadLevel;

    public GameObject mapHighlight;
    public List<GameObject> weaponHighlights;
    public GameObject mapGuide;
    public IEnumerator SeekControllers()
    {
        bool _found = true;
        GameObject g = GameObject.Find("Player/Controller (Left)");
        if (g)
        {
            g.GetComponent<WeaponHandler>().ChangeWeapon(_activeWeapons[0]);
        }
        else _found = false;
        g = GameObject.Find("Player/Controller (Right)");
        if (g)
            g.GetComponent<WeaponHandler>().ChangeWeapon(_activeWeapons[1]);
        else _found = false;
        if (!_found){
            yield return new WaitForSeconds(.25f);
            StartCoroutine(SeekControllers());
        }
    }

    public static CampaignManager campaignManager; // Highlander setup

    [Tooltip("The scenes the instances will switch to.")]
    public int[] InstanceScenes;
    [Tooltip("Is the instance unlocked?")]
    public bool[] InstanceUnlocked;
    public bool[] InstanceWon;
    private List<WaveInstance> _instances = new List<WaveInstance>();
    [Tooltip("Pairs that will unlock each other. X: From, Y: To")]
    public Vector2Int[] UnlockPairs; // x: from, y: to

    [Tooltip("Journal entries.")]
    public string[] JournalEntries;
    public int UnlockedEntries = 0;
    public Button[] buttons = new Button[8];
    private int _activeInstance = 1; // Keeping track of the active instance for unlocks.
    public int[] _activeWeapons = new int[2];

    // Start is called before the first frame update
    void Start()
    {
        if (mapHighlight && BossDwarf.highlightActive)
        {
            mapHighlight.GetComponent<Outline>().enabled = true;
            mapHighlight.GetComponent<Animator>().enabled = true;
        }
        else
        {
            mapHighlight.GetComponent<Outline>().enabled = false;
            mapHighlight.GetComponent<Animator>().enabled = false;
        }
            // Highlander.
            if (!campaignManager)
        {
            XRSettings.eyeTextureResolutionScale = 1f;
            campaignManager = this;
            SceneManager.sceneLoaded += EnterScene;
            DontDestroyOnLoad(gameObject);
            SaveData save = SaveData.ReadFromFile("mysave");
            if (save != null)
            {
                InstanceUnlocked = save.InstancesUnlocked;
                InstanceWon = save.InstancesWon;
                UnlockedEntries = save.UnlockedEntries;
            }
            EnterScene(SceneManager.GetActiveScene(), LoadSceneMode.Single);
        }
        if (campaignManager != this) Destroy(gameObject);

        _instances.AddRange(GetComponentsInChildren<WaveInstance>());
        _instances.Sort((i1, i2) => i1.name.CompareTo(i2.name));
    }
    

    public void LoadInstance(int id)
    {
        SaveData mySave = new SaveData();
        mySave.GrabAndWrite("mysave");
        if (!InstanceUnlocked[id]) return;
        _activeInstance = id;
        ChangeScene(InstanceScenes[id]);
    }
    public void ReloadInstance()
    {
        LoadInstance(_activeInstance);
    }
    public void UnlockFromInstance(int id)
    {
        bool freshUnlock = false;
        foreach(Vector2Int v in UnlockPairs)
        {
            if (v.x == id)
            {
                if (!InstanceUnlocked[v.y]) freshUnlock = true;
                InstanceUnlocked[v.y] = true;
            }
        }
        if (freshUnlock) UnlockedEntries++;
    }

    public void ChangeScene(int id)
    {
        // Fade out here.
        Time.timeScale = 1;
       // loadLevel.Begin(id);
    }
    public void EnterScene(Scene current, LoadSceneMode mode)
    {
        if(current.buildIndex == 0)
        {
            SaveData mySave = new SaveData();
            mySave.GrabAndWrite("mysave");
            
            GameObject mapObject = GameObject.Find("MapObject");
          
            buttons = mapObject.GetComponentsInChildren<Button>();

            for (int i = 0; i < buttons.Length; i++)
            {
                Debug.Log(InstanceUnlocked[i]);
                if (InstanceUnlocked[i]) buttons[i].interactable = true;
                else buttons[i].interactable = false;
                var j = i;
                buttons[i].onClick.AddListener(delegate { SelectScene(j); });
            }

        }
        else
        {
            int i = 0;
            SteamObjectContainer.leftHandler.ChangeWeapon(_activeWeapons[0]);
            SteamObjectContainer.rigtHandler.ChangeWeapon(_activeWeapons[1]);
            if(_instances != null && _activeInstance < _instances.Count)
                PassInstanceToSpawner(_instances[_activeInstance]);
        }

        // Fade in here.
    }
    public void SetWeaponHighlights(bool yey)
    {
        foreach (var item in weaponHighlights)
        {
            item.GetComponent<Outline>().enabled = yey;
            item.GetComponent<Animator>().enabled = yey;
        }
    }
    public void SelectScene(int a)
    {
        if (mapHighlight && BossDwarf.highlightActive)
        {
            mapHighlight.GetComponent<Outline>().enabled = false;
            mapHighlight.GetComponent<Animator>().enabled = false;
            SetWeaponHighlights(true);

            mapGuide.SetActive(false);
        }
   
     
        SaveData mySave = new SaveData();
        mySave.GrabAndWrite("mysave");
        if (!InstanceUnlocked[a]) return;
        _activeInstance = a;
        FindObjectOfType<MenuController>().levelSelected = true;
        //mapHighlight.SetBool("isHighlighting", false);
        MenuDoor.selectedScene = a;
    }
    public void LoseInstance()
    {
        // do losing things here.
        //ChangeScene(0);
    }
    public void WinInstance()
    {
        if(!InstanceWon[_activeInstance])

            if (ResourceManager.resourceManager)
            {
                ResourceManager.resourceManager.ResourceAmounts[1] += ResourceManager.resourceManager.AmountsEarned[1] += 1;
                
                SaveData mySave = new SaveData();
                mySave.GrabAndWrite("mysave");
            }
            else
            {
                Debug.Log("Resource manager yok");
            }
         
        InstanceWon[_activeInstance] = true;
        UnlockFromInstance(_activeInstance);

    }

    public void PickWeaponIndex(GameObject wep, int index)
    {
        if (wep.GetComponent<Weapon>())
        {
            // 0 is left 1 is right
            _activeWeapons[index] = wep.GetComponent<Weapon>().index; // YOU CAN CHECK THESE WITH if(_activeWeapons[0] is Hammer) OR _hammer = _activeWeapons[0] as Hammer AND SUCH

            if (mapHighlight && BossDwarf.highlightActive)
            {
                SetWeaponHighlights(false);
            }
           
        }
        if (_activeWeapons[0] == _activeWeapons[1])
        {
            _activeWeapons[1] = 2;
        }
        
    }
    
    private void PassInstanceToSpawner(WaveInstance i)
    {
        List<Wave> waves = new List<Wave>();
        waves.AddRange(i.GetComponentsInChildren<Wave>());
        waves.Sort((w1, w2) => w1.id.CompareTo(w2.id)); // sort waves by id
    }
}
