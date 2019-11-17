using System.Linq;
using UnityEditor;
using UnityEngine;

public class UpgradeGenerator : EditorWindow
{
    public string UpgradeName;
    public Object TargetWeapon;
    public Object TargetSkill;

    public int MaxUpgrades;
    public int CurrentUpgrade;
    public bool Unlocked;
    public int[] Costs;

    public int AffectedVariableCount;
    public UpgradeVariables[] AffectedVariables;

    public MagnitudeLine[] Magnitudes;

    Vector2 scrollPos;

    [MenuItem("Window/UpgradeGenerator")]
    public static void ShowWindow()
    {
        //Show existing window instance. If one doesn't exist, make one.
        EditorWindow.GetWindow(typeof(UpgradeGenerator));
    }

    void OnGUI()
    {
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos, false, false);
        EditorGUIUtility.labelWidth = 200;

        UpgradeName = EditorGUILayout.TextField("Upgrade Name", UpgradeName);
        TargetWeapon = EditorGUILayout.ObjectField("Target weapon", TargetWeapon, typeof(Weapon), true);

        EditorGUILayout.Space();

        EditorGUI.BeginChangeCheck();
        AffectedVariableCount = EditorGUILayout.IntField("Affected Variable Count", AffectedVariableCount);
        EditorGUI.EndChangeCheck();
        var varCountChanged = GUI.changed;

        if (varCountChanged && AffectedVariableCount > 0)
        {
            AffectedVariables = new UpgradeVariables[AffectedVariableCount];
            Magnitudes = new MagnitudeLine[AffectedVariableCount];
            foreach(MagnitudeLine m in Magnitudes)
            {
                m.data = new float[MaxUpgrades];
            }
        }

        EditorGUI.BeginChangeCheck();
        for (int i = 0; i < AffectedVariableCount; i++)
        {
            AffectedVariables[i] = (UpgradeVariables)EditorGUILayout.EnumPopup("Affected Variable " + (i+1).ToString(), AffectedVariables[i]);
        }
        EditorGUI.EndChangeCheck();
        var varChanged = GUI.changed;

        EditorGUILayout.Space();

        EditorGUI.BeginChangeCheck();
        MaxUpgrades = EditorGUILayout.IntField("Maximum Upgrade Level", MaxUpgrades);
        EditorGUI.EndChangeCheck();
        var maxUpChanged = GUI.changed;

        if (AffectedVariableCount > 0)
        {
            if (MaxUpgrades > 0)
            {
                if (maxUpChanged || varCountChanged)
                {
                    Magnitudes = new MagnitudeLine[AffectedVariableCount];
                    foreach (MagnitudeLine m in Magnitudes)
                    {
                        m.data = new float[MaxUpgrades];
                    }
                    Costs = new int[MaxUpgrades];
                }

                CurrentUpgrade = EditorGUILayout.IntSlider("Current Upgrade Level", CurrentUpgrade, 0, MaxUpgrades);
                if (CurrentUpgrade > 0) Unlocked = true;
                else Unlocked = false;

                EditorGUILayout.Space();
                EditorGUILayout.Space();


                for (int i = 0; i < MaxUpgrades; i++)
                {
                    Costs[i] = EditorGUILayout.IntField("Level " + (i + 1).ToString() + " Cost", Costs[i]);
                    for(int j = 0; j < AffectedVariableCount; j++)
                    {
                        Magnitudes[j].data[i] = EditorGUILayout.FloatField(AffectedVariables[j].ToString() + " Upgrade #" + (i + 1).ToString() + " Magnitude", Magnitudes[j].data[i]);
                    }
                    EditorGUILayout.Space();

                }
            }
        }
        if (GUILayout.Button("Generate"))
        {
            GameObject o = new GameObject();
            o.AddComponent<Upgrade>();
            o.name = UpgradeName + " Upgrade";

            o.GetComponent<Upgrade>().UpgradeName = UpgradeName;
            
            o.GetComponent<Upgrade>().Unlocked = Unlocked;
            o.GetComponent<Upgrade>().UpgradeLevel = CurrentUpgrade;
            o.GetComponent<Upgrade>().AffectedVariables = AffectedVariables;
            o.GetComponent<Upgrade>().UpgradeMagnitudes = new MagnitudeLine[AffectedVariableCount];
            for(int i = 0; i < AffectedVariableCount; i++)
            {
                o.GetComponent<Upgrade>().UpgradeMagnitudes[i].data = new float[MaxUpgrades];
                for(int j = 0; j < MaxUpgrades; j++)
                {
                    o.GetComponent<Upgrade>().UpgradeMagnitudes[i].data[j] = Magnitudes[i].data[j];
                }
            }
            o.GetComponent<Upgrade>().UpgradeCosts = Costs;
        }
        EditorGUILayout.EndScrollView();
    }

    
}




