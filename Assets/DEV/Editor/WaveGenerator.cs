using System.Linq;
using UnityEditor;
using UnityEngine;

public class WaveGenerator : EditorWindow
{
    int id;
    int nMobSum;
    int nSpawnSize;
    float spawnTick;
    int mobOnScene;
    string objectName;
    WaveInstance instance;
    // Add menu item named "My Window" to the Window menu
    [MenuItem("Window/WaveGenerator")]
    public static void ShowWindow()
    {
        //Show existing window instance. If one doesn't exist, make one.
        EditorWindow.GetWindow(typeof(WaveGenerator));
    }

    void OnGUI()
    {
        GUILayout.Label("Base Settings", EditorStyles.boldLabel);
        id = EditorGUILayout.IntField("ID", id);

        //groupEnabled = EditorGUILayout.BeginToggleGroup("Optional Settings", groupEnabled);
       // myBool = EditorGUILayout.Toggle("Toggle", myBool);
        spawnTick = EditorGUILayout.Slider("Spawn Tick", spawnTick, 0, 1);
        objectName = EditorGUILayout.TextField("Name", objectName);
        nMobSum = EditorGUILayout.IntSlider("nMob Sum", nMobSum, 0, 1000);
        nSpawnSize = EditorGUILayout.IntField("Spawn Size", nSpawnSize);
        mobOnScene = EditorGUILayout.IntSlider("Mob On Scene", mobOnScene, 0, 50);
        instance = EditorGUILayout.ObjectField(instance, typeof(WaveInstance), true) as WaveInstance;
        
        if (GUILayout.Button("Create"))
        {
           
            if (!instance)
            {
                Debug.LogError("Create or assign a Wave Instance before creating wave.");
            }
            else
            {
                GameObject waveCopy = new GameObject();
                waveCopy.name = objectName + "_" + id;
                waveCopy.transform.SetParent(instance.transform);
                waveCopy.AddComponent<Wave>();
                waveCopy.GetComponent<Wave>().id = id;
                waveCopy.GetComponent<Wave>().nMobSum = nMobSum;
                waveCopy.GetComponent<Wave>().nSpawnSize = nSpawnSize;
                waveCopy.GetComponent<Wave>().spawnTick = spawnTick;

            }
            
         
            
        }
        if (GUILayout.Button("Create New Instance"))
        {
            
            GameObject instanceCopy = new GameObject();
            instanceCopy.AddComponent<WaveInstance>();
            instanceCopy.name = "NewInstance";
        }
        if (GUILayout.Button("Order Waves by ID in assigned Instance"))
        {
            
            if (instance.GetComponentsInChildren<Wave>().Length < 1)
            {
                Debug.LogError("No Wave created on this Instance", instance);
            }
            else if(instance.GetComponentsInChildren<Wave>().Length > 1)
            {
                Debug.Log(instance.GetComponentsInChildren<Wave>().Length);
                Wave[] waves = new Wave[instance.GetComponentsInChildren<Wave>().Length];
                waves = instance.GetComponentsInChildren<Wave>();
                waves = waves.OrderBy(t => t.id).ToArray();
                foreach (var item in waves)
                {
                    item.gameObject.transform.SetSiblingIndex(item.id);
                }
            }
            
        }
        if (GUILayout.Button("Reset"))
        {
            spawnTick = 0;
            nMobSum = 0;
            nSpawnSize = 0;
            mobOnScene = 0;
            objectName = "";
        }
    }
}