using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour {
    [BoxGroup("Position")][SerializeField] private Transform _slotPosition;
    [BoxGroup("Position")][SerializeField] private float _yOffset = .75f;
    protected virtual void Start() {
        transform.position = _slotPosition.position + new Vector3(0, _yOffset, 0);
    }
}