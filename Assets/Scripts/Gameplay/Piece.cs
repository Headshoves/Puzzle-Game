using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Piece : MonoBehaviour {
    [BoxGroup("Position")][SerializeField] private Transform _slotPosition;
    [BoxGroup("Position")][SerializeField] private float _yOffset = .75f;


    [BoxGroup("Move Limits")][SerializeField] protected bool _hasLimitMoves = false;
    [BoxGroup("Move Limits")][ShowIf("_hasLimitMoves")][SerializeField] protected int _moveCount = 5;
    [BoxGroup("Move Limits")][ShowIf("_hasLimitMoves")][SerializeField] protected TextMeshProUGUI _moveText;

    [BoxGroup("Color")][SerializeField] protected MeshRenderer[] _objectsToChangeColor;
    [BoxGroup("Color")][SerializeField] protected bool _hasColor = false;
    [BoxGroup("Color")][ShowIf("_hasColor")] public Color ColorReference = Color.red;

    public bool CanPlay = true;

    protected virtual void Start() {
        if(_slotPosition != null) {
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

        if(_hasColor) { SetColor(ColorReference); }
    }
    public void SetColor(Color color) {
        foreach (var obj in _objectsToChangeColor) {
            obj.material.color = color;
        }
    }
}