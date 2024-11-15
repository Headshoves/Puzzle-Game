using DG.Tweening;
using NaughtyAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField][BoxGroup("Buttons")] private Button _playButton;
    [SerializeField][BoxGroup("Buttons")] private Button _returnWorldSelectionIntroButton;

    [SerializeField][BoxGroup("Screens")] private RectTransform _introScreen;
    [SerializeField][BoxGroup("Screens")] private RectTransform _worldSelectionScreen;

    [SerializeField][BoxGroup("Contents")] private RectTransform _worldButtonsContent;
    [SerializeField][BoxGroup("Contents")] private RectTransform _phasesButtonsContent;

    [SerializeField][BoxGroup("Prefabs")] private RectTransform _worldButtonPrefab;
    [SerializeField][BoxGroup("Prefabs")] private RectTransform _phaseButtonPrefab;

    private List<World> worlds;


    private void Start() {
        worlds = FindObjectOfType<GameManager>().GetWorlds();

        for(int i = 0; i < worlds.Count; i++) {
            int index = i;
            RectTransform temp = Instantiate(_worldButtonPrefab, _worldButtonsContent);
            temp.GetChild(0).GetComponent<TextMeshProUGUI>().text = worlds[index].Name;
            
            temp.GetComponent<Button>().onClick.RemoveAllListeners();
            temp.GetComponent<Button>().onClick.AddListener(() =>
            {
                ShowWorldPhases(worlds[index].Phases);
                GameManager.Instance.World = worlds[index];
            });

            temp.gameObject.SetActive(true);
        }

        _playButton.onClick.RemoveAllListeners();
        _playButton.onClick.AddListener(() => { ShowWorldSelectionScreen(); });

        _returnWorldSelectionIntroButton.onClick.RemoveAllListeners();
        _returnWorldSelectionIntroButton.onClick.AddListener(() => { ReturnWorldSelectionToIntro(); });
    }

    private void ShowWorldSelectionScreen() {
        _introScreen.DOLocalMoveY(-1100, .3f).OnComplete(() => {
            _worldSelectionScreen.DOLocalMoveY(0, .3f);
        });
    }

    private void ReturnWorldSelectionToIntro() {
        _worldSelectionScreen.DOLocalMoveY(1100, .3f).OnComplete(() => {
             _introScreen.DOLocalMoveY(0, .3f);
        });
    }

    private async void ShowWorldPhases(List<Phase> phases) {
        _worldButtonsContent.DOScale(0, 0.1f);

        await Task.Delay(TimeSpan.FromSeconds(0.05f));

        if (_phasesButtonsContent.childCount > 0) {
            for (int i = 0; i < _phasesButtonsContent.childCount; i++) {
                DestroyImmediate(_phasesButtonsContent.GetChild(0));
            }
        }

        await Task.Delay(TimeSpan.FromSeconds(0.05f));

        for (int i = 0; i < phases.Count; i++) {
            int index = i;

            RectTransform button = Instantiate(_phaseButtonPrefab, _phasesButtonsContent);
            button.GetChild(0).GetComponent<TextMeshProUGUI>().text = phases[index].Name;

            button.GetComponent<Button>().onClick.RemoveAllListeners();
            button.GetComponent<Button>().onClick.AddListener(() =>
            {
                GoToPhase(phases[index].PhaseId);
            });

            if (index == 0){
                button.GetComponent<Button>().interactable = true;

                button.GetComponent<Image>().color = phases[index].Completed ? Color.green : Color.yellow;
            }
            else if (index > 0){
                button.GetComponent<Button>().interactable = phases[index-1].Completed;
                
                if(button.GetComponent<Button>().interactable)
                    button.GetComponent<Image>().color = phases[index].Completed ? Color.green : Color.yellow;
                else
                    button.GetComponent<Image>().color = Color.red;
            } 

            button.gameObject.SetActive(true);
            button.DOScale(1, 0.1f);
            await Task.Delay(TimeSpan.FromSeconds(0.15f));
        }
    }

    private void GoToPhase(string phaseId) {
        FindObjectOfType<LoadScreen>().LoadSceneAsync(phaseId, "Menu");
    }
}
