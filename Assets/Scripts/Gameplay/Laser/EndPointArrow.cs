using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class EndPointArrow : MonoBehaviour
{

    [SerializeField] private Arrow _laser;
    [SerializeField] private float _speed;

    private bool _shoot;
    private bool _hitSomething;


    private void OnTriggerEnter(Collider other) {
        _hitSomething = true;
        if(other.TryGetComponent(out Target target)) {
            _laser.HitSomething(target);
        }

        if (other.TryGetComponent(out Mirror mirror)) {
            _laser.HitSomething(mirror);
        }

        if (other.TryGetComponent(out Wall wall)) {
            _laser.HitSomething(wall);
        }
    }

    private void Update() {
        if(_shoot) {
            
        }
    }

    public void Shoot() {
        _shoot = true;

        StartCoroutine(ShootCor());
    }

    public void Stop() {
        _shoot=false;
    }

    private IEnumerator ShootCor() {
        while (_shoot) {
            transform.position = transform.position + transform.forward * (_speed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }

        if (!_hitSomething) { _laser.HitNothing(); }
    }

    public void RotateLaser(float degree, Mirror mirror) {
        _shoot = false;
        transform.eulerAngles = new Vector3(0, degree, 0);
        transform.position = new Vector3(mirror.transform.position.x, transform.position.y, mirror.transform.position.z);

        _shoot = true;
        StartCoroutine(ShootCor());
    }
}
