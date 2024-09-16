using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Laser : MonoBehaviour
{
    [SerializeField] private EndPointLaser _endPoint;

    [BoxGroup("Tower")][SerializeField] private Tower _tower; 

    private LineRenderer _lineRenderer;

    private void Start() {
        _lineRenderer = GetComponent<LineRenderer>();
    }

    public void StartTrail() {
        _tower.LockRotate(true);
        _lineRenderer.positionCount = 2;
        _lineRenderer.SetPosition(0, transform.position);
        _endPoint.Shoot(_lineRenderer);
    }

    public void HitNothing() {
        LevelManager.instance.RegisterLaserFoundNothing(_tower);
    }

    public void HitSomething(Target target) {
        _endPoint.Stop();
        LevelManager.instance.RegisterLaserCollision(_tower, target);
    }

    public void HitSomething(Mirror mirror) {
        if((int)mirror.RotationAngle == 90 || (int)mirror.RotationAngle == -90) {
            switch ((int)_tower.Angle) {
                case 330:
                    _endPoint.RotateLaser(30, mirror);
                    break;
                case 270:
                    _endPoint.RotateLaser(90, mirror);
                    break;
                case 210:
                    _endPoint.RotateLaser(150, mirror);
                    break;
                case 150:
                    _endPoint.RotateLaser(210, mirror);
                    break;
                case 90:
                    _endPoint.RotateLaser(270, mirror);
                    break;
                case 30:
                    _endPoint.RotateLaser(330, mirror);
                    break;
            }
        }
        else if (mirror.RotationAngle == 210 || mirror.RotationAngle == 30) {
            switch ((int)_tower.Angle) {
                case 330:
                    _endPoint.RotateLaser(270, mirror);
                    break;
                case 270:
                    _endPoint.RotateLaser(330, mirror);
                    break;
                case 210:
                    _endPoint.RotateLaser(30, mirror);
                    break;
                case 150:
                    _endPoint.RotateLaser(90, mirror);
                    break;
                case 90:
                    _endPoint.RotateLaser(150, mirror);
                    break;
                case 30:
                    _endPoint.RotateLaser(210, mirror);
                    break;
            }
        }
        else if (mirror.RotationAngle == 150 || mirror.RotationAngle == -30) {
            switch ((int)_tower.Angle) {
                case 330:
                    _endPoint.RotateLaser(150, mirror);
                    break;
                case 270:
                    _endPoint.RotateLaser(210, mirror);
                    break;
                case 210:
                    _endPoint.RotateLaser(270, mirror);
                    break;
                case 150:
                    _endPoint.RotateLaser(330, mirror);
                    break;
                case 90:
                    _endPoint.RotateLaser(30, mirror);
                    break;
                case 30:
                    _endPoint.RotateLaser(90, mirror);
                    break;
            }
        }
    }

    public void HitSomething(Wall wall) {
        _endPoint.Stop();
        LevelManager.instance.RegisterLaserFoundNothing(_tower);
    }
}
