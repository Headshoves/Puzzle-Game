using DG.Tweening;
using NaughtyAttributes;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private List<Combination> _combinations = new List<Combination>();

    [BoxGroup("Tutorial")][SerializeField] private bool _hasTutorial;
    [BoxGroup("Tutorial")][ShowIf("_hasTutorial")][TextArea(1,5)][SerializeField] private string[] _textTutorial;

    [BoxGroup("Level Settings")][SerializeField] private GameObject _winScreen;
    [BoxGroup("Level Settings")][SerializeField] private TextMeshProUGUI _winText;
    [BoxGroup("Level Settings")][SerializeField] private Button _nextLevelButton;
    [BoxGroup("Level Settings")][SerializeField] private GameObject _loseScreen;
    private string _currentSceneName;
    
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

        _currentSceneName = SceneManager.GetActiveScene().name;
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
            bool complete = CheckAllTowersRight() && CheckAllTargets();

            await Task.Delay(1000);

            if (complete) {
                GameManager.Instance.CompleteLevel();
                
                _nextLevelButton.interactable = true;
                _winScreen.SetActive(true);
                _winScreen.GetComponent<CanvasGroup>().DOFade(1, .3f);
                if (FindObjectOfType<GameManager>().LastLevel()){
                    _winText.text = "Muito Bem! \n\n você terminou o mundo!";
                    _nextLevelButton.onClick.RemoveAllListeners();
                    _nextLevelButton.onClick.AddListener(GoToMenu);
                    _nextLevelButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Voltar para o Menu";
                }
                else {
                    _winText.text = "Muito Bem! \n\n vá para a próxima fase!";
                    _nextLevelButton.onClick.RemoveAllListeners();
                    _nextLevelButton.onClick.AddListener(NextLevel);
                    _nextLevelButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Próxima Fase";
                }
                
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
        Debug.Log("Chamou próxima fase");
        GameManager.Instance.NextLevel();
        _nextLevelButton.interactable = false;
    } 

    public void RestartLevel() {
        LoadScreen.instance.ReloadSceneAscyn(_currentSceneName);
        _nextLevelButton.interactable = false;
    }

    public void GoToMenu() {
        LoadScreen.instance.LoadSceneAsync("Menu");
        _nextLevelButton.interactable = false;
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
