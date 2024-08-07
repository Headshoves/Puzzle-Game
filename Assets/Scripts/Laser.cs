using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [BoxGroup("Positions")][SerializeField] private Transform _startPos;
    [BoxGroup("Positions")][SerializeField] private Transform _endPos;

    [BoxGroup("Tower")][SerializeField] private Tower _tower; 

    [SerializeField] private float _speed;

    private bool _isPlaying = false;

    private LineRenderer _lineRenderer;

    private void Start() {
        _lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.W)) { StartTrail(); }
    }

    public async void StartTrail() {
        _isPlaying = true;
        _tower.LockRotate(true);
        StartCoroutine(Trail());
        await Task.Delay(TimeSpan.FromSeconds(5f));
        _isPlaying = false;

    }

    private IEnumerator Trail() {
        while(_isPlaying) {
            float front = _endPos.localPosition.z + (_speed * Time.deltaTime);

            _endPos.localPosition = new Vector3(_endPos.localPosition.x, _endPos.localPosition.y, front);

            _lineRenderer.SetPosition(0, _startPos.position);
            _lineRenderer.SetPosition(1, _endPos.position);

            yield return new WaitForEndOfFrame();
        }
    }
}
