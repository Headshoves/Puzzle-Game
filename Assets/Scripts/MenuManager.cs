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

    [SerializeField][BoxGroup("Buttons Sprites")] private Sprite _completedSprite;
    [SerializeField][BoxGroup("Buttons Sprites")] private Sprite _unlockSprite;
    [SerializeField][BoxGroup("Buttons Sprites")] private Sprite _lockSprite;

    [SerializeField] [BoxGroup("Texts")] private TextMeshProUGUI _infoText;

    private List<World> worlds;


    private void Start() {
        worlds = GameManager.Instance.GetWorlds();
        
        for(int i = 0; i < worlds.Count; i++) {
            int index = i;
            RectTransform temp = Instantiate(_worldButtonPrefab, _worldButtonsContent);
            temp.GetChild(0).GetComponent<TextMeshProUGUI>().text = worlds[index].Name;
            
            temp.GetComponent<Button>().onClick.RemoveAllListeners();
            temp.GetComponent<Button>().onClick.AddListener(() =>
            {
                ShowWorldPhases(worlds[index].Phases);
                GameManager.Instance.SetWorld(index);
            });

            temp.gameObject.SetActive(true);
        }

        _playButton.onClick.RemoveAllListeners();
        _playButton.onClick.AddListener(() => { ShowWorldSelectionScreen(); });

        _returnWorldSelectionIntroButton.onClick.RemoveAllListeners();
        _returnWorldSelectionIntroButton.onClick.AddListener(() => { ReturnWorldSelectionToIntro(); });
    }

    private async void ShowWorldSelectionScreen(){
        _infoText.text = "Escolha um mundo para jogar";
        
        await Task.Delay(TimeSpan.FromSeconds(0.3f));
        _introScreen.DOLocalMoveY(-1100, 1f).SetEase(Ease.InSine).OnComplete(() => {
            _worldSelectionScreen.DOLocalMoveY(0, 1f);
        });
    }

    private void ReturnWorldSelectionToIntro() {
        _worldSelectionScreen.DOLocalMoveY(1100, 1f).OnComplete(() => {
             _introScreen.DOLocalMoveY(0, 1f);
        });
    }

    private async void ShowWorldPhases(List<Phase> phases) {
        
        _infoText.text = "Escolha a fase que deseja jogar";
        
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
                GameManager.Instance.PhaseIndex = index;
            });

            if (index == 0){
                button.GetComponent<Button>().interactable = true;

                button.GetComponent<Image>().sprite = phases[index].Completed ? _completedSprite : _unlockSprite;
            }
            else if (index > 0){
                button.GetComponent<Button>().interactable = phases[index-1].Completed;
                
                if(button.GetComponent<Button>().interactable)
                    button.GetComponent<Image>().sprite = phases[index].Completed ? _completedSprite : _unlockSprite;
                else
                    button.GetComponent<Image>().sprite = _lockSprite;
            } 

            button.gameObject.SetActive(true);
            button.DOScale(1, 0.1f);
            await Task.Delay(TimeSpan.FromSeconds(0.15f));
        }
    }

    private void GoToPhase(string phaseId) {
        FindObjectOfType<LoadScreen>().LoadSceneAsync(phaseId);
    }
}
