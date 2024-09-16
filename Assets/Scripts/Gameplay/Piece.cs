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

    protected virtual void Start() {
        transform.position = _slotPosition.position + new Vector3(0, _yOffset, 0);

        if( _hasLimitMoves) {
            _moveText = transform.GetComponentInChildren<TextMeshProUGUI>();
            _moveText.gameObject.SetActive(true);
            _moveText.text = _moveCount.ToString();
        }
    }
}