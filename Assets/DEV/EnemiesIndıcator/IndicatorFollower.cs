using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class IndicatorFollower : MonoBehaviour
{
    public List<IndicatorPoint> indicatorPoints;
    public float yOffset = .1f;
    Vector3 currentAngle;

    void Start()
    {
        currentAngle = transform.eulerAngles;
    }

    GameObject g;

    public GameObject AddIndicator(GameObject prefab)
    {
        for (int i = 0; i < 4; i++)
        {
            if (!indicatorPoints[i].isFull)
            {
                g = Instantiate(prefab, indicatorPoints[i].transform);
                indicatorPoints[i].isFull = true;
                break;
            }
        }
        return g;
    }
    public void DeleteIndicator(GameObject g)
    {
        g.transform.parent.GetComponent<IndicatorPoint>().isFull = false;
    }

    void Update()
    {
        transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y + yOffset, Camera.main.transform.position.z);
        currentAngle = new Vector3(0, Mathf.LerpAngle(currentAngle.y, Camera.main.transform.eulerAngles.y, Time.deltaTime * 3), 0);
        transform.eulerAngles = currentAngle;
    }
}
