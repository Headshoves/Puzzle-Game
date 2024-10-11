using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Piece : MonoBehaviour {
    [BoxGroup("Position")][SerializeField] private Transform _slotPosition;
    [BoxGroup("Position")][SerializeField] private float _yOffset = .75f;
    [BoxGroup("Position")][SerializeField] protected Angles _initialAngle;


    [BoxGroup("Move Limits")][SerializeField] protected bool _hasLimitMoves = false;
    [BoxGroup("Move Limits")][ShowIf("_hasLimitMoves")][SerializeField] protected int _moveCount = 5;
    [BoxGroup("Move Limits")][ShowIf("_hasLimitMoves")][SerializeField] protected TextMeshProUGUI _moveText;

    [BoxGroup("Color")][SerializeField] protected MeshRenderer[] _objectsToChangeColor;
    [BoxGroup("Color")][SerializeField] protected bool _hasColor = false;
    [BoxGroup("Color")][ShowIf("_hasColor")] public Color ColorReference = Color.red;

    public bool CanPlay = true;

    private void Awake() {
        if (_hasColor) { SetColor(ColorReference); } else { SetColor(Color.gray); }
    }

    protected virtual void Start() {
        transform.eulerAngles = new Vector3(0, (int)_initialAngle, 0);

        if (_slotPosition != null) {
            transform.position = _slotPosition.position + new Vector3(0, _yOffset, 0);
        }
        else {
            Slot closeSlot = null;
            foreach(Slot slot in FindObjectsOfType<Slot>()) {
                if(closeSlot == null) {
                    closeSlot = slot;
                }
                else {
                    if (Vector3.Distance(transform.position, slot.transform.position) < Vector3.Distance(transform.position, closeSlot.transform.position)) {
                        closeSlot = slot;
                    }
                }
            }
            _slotPosition = closeSlot.transform;
            transform.position = _slotPosition.position + new Vector3(0, _yOffset, 0);
        }

        if( _hasLimitMoves) {
            _moveText = transform.GetComponentInChildren<TextMeshProUGUI>();
            _moveText.gameObject.SetActive(true);
            _moveText.text = _moveCount.ToString();
        }
        else {
            _moveText = transform.GetComponentInChildren<TextMeshProUGUI>();
            _moveText.gameObject.SetActive(false);
        }

    }
    public void SetColor(Color color) {
        foreach (var obj in _objectsToChangeColor) {
            obj.material.color = color;
        }
    }
}


[Serializable]
public enum Angles {zero = 0, thirty = 30, ninety = 90, oneHundredFifty = 150, twoHundredTen = 210, twoHundredSeventy = 270, threeHundredThirty = 330 }