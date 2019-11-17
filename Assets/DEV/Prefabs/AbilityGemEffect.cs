using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityGemEffect : MonoBehaviour
{
    public List<Transform> gemTransforms;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private IEnumerator RecursiveEffect()
    {
        for (int i = 0; i < gemTransforms.Count; i++)
        {
            yield return new WaitForSeconds(.1f);
        }
        

    }
}
