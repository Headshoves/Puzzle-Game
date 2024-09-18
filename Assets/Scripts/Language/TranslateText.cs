using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TranslateText : TranslateObject {
    
    [InfoBox("English = 0 \n Portuguese = 1", EInfoBoxType.Normal)]
    [SerializeField] private string[] _texts;
    
    private TextMeshProUGUI _textMeshProUGUI;

    private void Start() {
        _textMeshProUGUI = GetComponent<TextMeshProUGUI>();

        ChangeLanguageTo(GameManager.Instance.Language);
    }

    public override void ChangeLanguageTo(Language language) {
        _textMeshProUGUI.text = _texts[(int)language];
    }
}
