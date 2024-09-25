using DG.Tweening;
using NaughtyAttributes;
using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private List<Combination> _combinations = new List<Combination>();

    [BoxGroup("Tutorial")][SerializeField] private bool _hasTutorial;
    [BoxGroup("Tutorial")][ShowIf("_hasTutorial")][TextArea(1,5)][SerializeField] private string[] _textTutorial;

    [BoxGroup("Level Settings")][SerializeField] private GameObject _winScreen;
    [BoxGroup("Level Settings")][SerializeField] private GameObject _loseScreen;
    
    public static LevelManager instance;

    private bool _canLaunch = true;

    private void Awake() {
        instance = this;
    }

    private void Start() {
        foreach(Tower tower in FindObjectsOfType<Tower>()) {
            _combinations.Add(new Combination(tower, tower.HasTarget() ? tower.GetTarget() : null));
        }

        if(_hasTutorial) { TextBox.Instance.ShowTextList(_textTutorial); }
    }

    public void RegisterLaserCollision(Tower tower,Target target) {
        for(int i = 0; i < _combinations.Count; i++) {
            if(tower == _combinations[i].Tower) {
                tower.Complete = true;
                if (tower.HasTarget()) {
                    if (tower.GetTarget() ==  target) {
                        tower.RightTarget = true;
                    }
                }
                else{
                    tower.RightTarget = true;
                }
            }
        }

        CheckEndGame();
    }

    public void RegisterLaserFoundNothing(Tower tower) {
        for (int i = 0; i < _combinations.Count; i++) {
            if (tower == _combinations[i].Tower) {
                tower.RightTarget = false;
                tower.Complete = true;
            }
        }

        CheckEndGame();
    }

    private void CheckEndGame() {
        bool allTowersComplete = true;
        bool allTowersRight = true;
        for(int i = 0; i < _combinations.Count; i ++) {
            if (!_combinations[i].Tower.Complete) {
                allTowersComplete = false;
                break;
            }
            else {
                if (!_combinations[i].Tower.RightTarget) {
                    allTowersRight = false;
                    break;
                }
            }
        }

        if (allTowersComplete) {
            if(allTowersRight) {
                _winScreen.SetActive(true);
                _winScreen.GetComponent<CanvasGroup>().DOFade(1, .3f);
            }
            else {
                _loseScreen.SetActive(true);
                _loseScreen.GetComponent<CanvasGroup>().DOFade(1, .3f);
            }
        }
    }

    public void Launch() {
        if (_canLaunch) {
            for (int i = 0; i < _combinations.Count; i++) {
                _combinations[i].Tower.Launch();
            }

            _canLaunch = false;
        }
        
    }
}

[Serializable]
public class Combination {
    public Tower Tower;
    public Target Target;

    public Combination(Tower tower, Target target = null) {
        Tower = tower;
        Target = target;
    }
}
