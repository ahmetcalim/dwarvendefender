using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class UpgradeBookHover : MonoBehaviour
{
    public int index;
    public List<Image> buttonImages;
    private Color col = new Color();
    private void Start()
    {
        col = GetComponent<Image>().color;
    }
    // Start is called before the first frame update
    public void Hover()
    {
        Debug.Log("HOVER ÇALIŞTI ?");
       

        foreach (var item in buttonImages)
        {
            if (item.GetComponent<UpgradeBookHover>().index != index)
            {
                col.a = .5f;
                item.color = col;
            }
            else
            {
                col.a = 1f;
                item.color = col;
            }
        }
    }
}
