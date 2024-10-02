using DG.Tweening;
using NaughtyAttributes;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private List<Combination> _combinations = new List<Combination>();

    [BoxGroup("Tutorial")][SerializeField] private bool _hasTutorial;
    [BoxGroup("Tutorial")][ShowIf("_hasTutorial")][TextArea(1,5)][SerializeField] private string[] _textTutorial;

    [BoxGroup("Level Settings")][SerializeField] private GameObject _winScreen;
    [BoxGroup("Level Settings")][SerializeField] private GameObject _loseScreen;
    [BoxGroup("Level Settings")][SerializeField] private string _nextSceneName;
    [BoxGroup("Level Settings")][SerializeField] private string _currentSceneName;
    
    public static LevelManager instance;

    private bool _canLaunch = true;

    private void Awake() {
        instance = this;
    }

    private void Start() {
        foreach(Tower tower in FindObjectsOfType<Tower>()) {
            _combinations.Add(new Combination(tower, tower.HasTarget() ? tower.GetTarget() : null));
        }

        if(_hasTutorial) { FindObjectOfType<TextBox>().ShowTextList(_textTutorial); }
    }

    public void RegisterLaserCollision(Tower tower,Target target) {
        for(int i = 0; i < _combinations.Count; i++) {
            if(tower == _combinations[i].Tower) {
                tower.Complete = true;
                if (tower.HasTarget()) {
                    if (tower.GetTarget() ==  target) {
                        tower.RightTarget = true;
                        target.Complete = true;
                        target.RightTower = true;
                    }
                }
                else{
                    tower.RightTarget = true;
                    target.Complete = true;
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

    private async void CheckEndGame() {
        if (CheckAllTowersComplete()) {
            bool complete = false;

            if (CheckAllTowersRight() && CheckAllTargets()) {
                complete = true;
            }

            await Task.Delay(1000);

            if (complete) {
                _winScreen.SetActive(true);
                _winScreen.GetComponent<CanvasGroup>().DOFade(1, .3f);
            }
            else {
                _loseScreen.SetActive(true);
                _loseScreen.GetComponent<CanvasGroup>().DOFade(1, .3f);
            }
        }
    }

    private bool CheckAllTowersComplete() {

        for (int i = 0; i < _combinations.Count; i++) {
            if (!_combinations[i].Tower.Complete) {
                return false;
            }
        }
        return true;
    }

    private bool CheckAllTowersRight() {
        for (int i = 0; i < _combinations.Count; i++) {
            if (!_combinations[i].Tower.RightTarget) {
                return false;
            }
        }
        return true;
    }

    private bool CheckAllTargets() {
        Target[] targets = FindObjectsOfType<Target>();

        for(int i = 0; i < targets.Length; i++) {
            if (!targets[i].Complete) {
                return false;
            }
            else {
                if (targets[i].HasTower && !targets[i].RightTower) {
                    return false;
                }
            }
        }

        return true;
    }

    public void Launch() {
        if (_canLaunch) {
            for (int i = 0; i < _combinations.Count; i++) {
                _combinations[i].Tower.Launch();
            }

            _canLaunch = false;
        }
        
    }

    public void NextLevel() {
        FindObjectOfType<LoadScreen>().LoadSceneAsync(_nextSceneName, _currentSceneName);
    } 

    public void RestartLevel() {
        FindObjectOfType<LoadScreen>().ReloadSceneAscyn(_currentSceneName);
    }

    public void GoToMenu() {
        FindObjectOfType<LoadScreen>().LoadSceneAsync("Menu", _currentSceneName);
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
