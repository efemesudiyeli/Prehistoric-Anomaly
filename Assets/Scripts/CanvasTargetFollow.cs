using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class CanvasTargetFollow : MonoBehaviour
{
    [SerializeField] private GameObject _target;
    [SerializeField] private Vector2 offset = new Vector2(0, 0.5f);
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.position = (Vector2)_target.transform.position + offset;
    }
}
