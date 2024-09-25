using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextBox : MonoBehaviour
{
    public static TextBox Instance;

    [SerializeField] private GameObject _textContent;
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private Button _nextButton;

    [SerializeField] private float _timeToShowChar = 0.1f;

    int _textCount = 0;

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        _nextButton.onClick.RemoveAllListeners();
    }

    private async void PlayText(string text) {

        _text.text = "";
        _textContent.SetActive(true);
        _nextButton.gameObject.SetActive(false);
        _textContent.GetComponent<RectTransform>().DOScale(1, .2f);

        await Task.Delay(TimeSpan.FromSeconds(.2f));

        for (int i = 0; i < text.Length; i++) {
            _text.text += text[i];
            await Task.Delay(TimeSpan.FromSeconds(_timeToShowChar));
        }

        _nextButton.gameObject.SetActive(true);
        _nextButton.interactable = true;
        await Task.Delay(TimeSpan.FromSeconds(.2f));
    }

    public void ShowTextList(string[] text) {

        GameManager.Instance.PauseGame();

        _textCount = 0;

        _nextButton.onClick.AddListener(() => NextText(text));

        PlayText(text[0]);
    }

    private void NextText(string[] text) {
        _textCount++;
        _nextButton.interactable = false;

        if (_textCount < text.Length) {
            PlayText(text[_textCount]);
        }
        else {
            _textContent.GetComponent<RectTransform>().DOScale(0, .2f).OnComplete(() => {
                _textContent.SetActive(false);

                GameManager.Instance.ResumeGame();

                _nextButton.gameObject.SetActive(false);
            });
        }
    }
}
