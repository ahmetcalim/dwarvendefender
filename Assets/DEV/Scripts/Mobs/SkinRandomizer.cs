using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinRandomizer : MonoBehaviour
{
    public List<Material> skinMats;
    public SkinnedMeshRenderer body;
    public List<GameObject> armors;
    public bool slicedModel;
    // Start is called before the first frame update
    void Start()
    {
        if (!slicedModel)
        {
            body.sharedMaterials[1] = skinMats[Random.Range(0, skinMats.Count)];

            foreach (var item in armors)
            {
                if (Random.Range(0, 2) == 0)
                {
                    item.SetActive(true);
                }
                else
                {
                    item.SetActive(false);
                }
            }
        }
    
    }
    public void SetSlicedModel(List<GameObject> _armors, SkinnedMeshRenderer _body)
    {
        for (int i = 0; i < armors.Count; i++)
        {
            armors[i].SetActive(_armors[i].activeInHierarchy);
        }
        body.sharedMaterials[1] = _body.sharedMaterials[1];
    }
    
}
