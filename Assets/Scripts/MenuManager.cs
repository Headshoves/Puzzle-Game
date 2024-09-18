using DG.Tweening;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField][BoxGroup("Buttons")] private Button _playButton;
    [SerializeField][BoxGroup("Buttons")] private Button _returnWorldSelectionIntroButton;

    [SerializeField][BoxGroup("Screens")] private RectTransform _introScreen;
    [SerializeField][BoxGroup("Screens")] private RectTransform _worldSelectionScreen;

    [SerializeField][BoxGroup("Contents")] private RectTransform _worldButtonsContent;

    [SerializeField][BoxGroup("Prefabs")] private RectTransform _worldButtonPrefab;

    private void Start() {
        List<World> worlds = GameManager.Instance.GetWorlds();

        for(int i = 0; i < worlds.Count; i++) {
            int index = i;
            RectTransform temp = Instantiate(_worldButtonPrefab, _worldButtonsContent);

            temp.GetChild(0).GetComponent<TextMeshProUGUI>().text = worlds[i].Names[(int)GameManager.Instance.Language];
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
}
