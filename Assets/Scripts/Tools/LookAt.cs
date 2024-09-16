using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : MonoBehaviour
{
    public bool Active = false;

    [SerializeField] private bool _lookToCamera = false;

    [SerializeField] private Transform _objectToLook;

    private void Start() {
        if (_lookToCamera) {
            _objectToLook = FindObjectOfType<Camera>().transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Active) {
            if(_objectToLook != null) {
                transform.LookAt(_objectToLook);
            }
        }
    }
}
