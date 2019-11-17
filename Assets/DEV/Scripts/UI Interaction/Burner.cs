using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Burner : MonoBehaviour
{
    public float StartSize, SizeIncrement, MaxSize, SizeDecrementPerSecond, DecreaseDelay;
    public Vector3 DefaultScale;
    private float _delay, _currentSize;
    public GameObject[] TargetObjects;

    // Start is called before the first frame update
    void Start()
    {
        _delay = 0;
        _currentSize = StartSize;
    }

    // Update is called once per frame
    void Update()
    {
        if(_currentSize > StartSize)
        {
            if(_delay < DecreaseDelay)
            {
                _delay += Time.deltaTime;
            }
            else
            {
                _currentSize -= SizeDecrementPerSecond * Time.deltaTime;
                if (_currentSize < StartSize) _currentSize = StartSize;
                ApplySizeChange();
            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.tag == "fuel")
        {
            Destroy(collision.transform.gameObject);
            _currentSize += SizeIncrement;
            if (_currentSize > MaxSize) _currentSize = MaxSize;
            ApplySizeChange();
            _delay = 0;
        }
    }

    private void ApplySizeChange()
    {
        foreach(GameObject g in TargetObjects)
        {
            g.transform.localScale = DefaultScale * _currentSize;
        }
    }
}
