using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class CanvasTargetFollow : MonoBehaviour
{
    [SerializeField] private GameObject _target;
    [SerializeField] private Vector2 offset = new Vector2(0, 0.5f);

    // Update is called once per frame
    void LateUpdate()
    {
        if (_target == null)
            return;
        transform.position = (Vector2)_target.transform.position + offset;
    }
}
