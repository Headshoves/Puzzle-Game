using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class TextBox : MonoBehaviour
{
    [SerializeField] private GameObject _textContent;
    [SerializeField] private TextMeshProUGUI _text;

    [SerializeField] private float _timeToShowChar = 0.1f;

    public async Task PlayText(string text) {

        _text.text = "";
        _textContent.SetActive(true);
        _textContent.GetComponent<RectTransform>().DOScale(1, .2f);
        await Task.Delay(TimeSpan.FromSeconds(.2f));

        for (int i = 0; i < text.Length; i++) {
            _text.text += text[i];
            await Task.Delay(TimeSpan.FromSeconds(_timeToShowChar));
        }

        _textContent.GetComponent<RectTransform>().DOScale(0, .2f).OnComplete(() => {
            _textContent.SetActive(false);
        });

        await Task.Delay(TimeSpan.FromSeconds(.2f));
    }
}
