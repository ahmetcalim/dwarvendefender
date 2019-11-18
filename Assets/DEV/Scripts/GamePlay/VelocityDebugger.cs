using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class VelocityDebugger : MonoBehaviour
{
    public static List<string> velocities = new List<string>();
    // Start is called before the first frame update
    public void SetVelocitiesToJson()
    {
        Velocity vel = new Velocity();
        vel.velocities = velocities.ToArray();
        string json = JsonUtility.ToJson(vel);
        File.WriteAllText(Application.dataPath + "/" + "Velocities" + ".json", json);
    }
    
    
}
