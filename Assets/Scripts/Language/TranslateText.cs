using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TranslateText : TranslateObject {

    [SerializeField] private string _englishText;
    [SerializeField] private string _portugueseText;
    [SerializeField] private string _spanishText;
    
    private TextMeshProUGUI _textMeshProUGUI;

    private void Start() {
        _textMeshProUGUI = GetComponent<TextMeshProUGUI>();
    }

    public override void ChangeLanguageTo(Language language) {
        switch(language) {
            case Language.English:
                _textMeshProUGUI.text = _englishText;
                break;
            case Language.Portuguese:
                _textMeshProUGUI.text = _portugueseText;
                break;
            case Language.Spanish:
                _textMeshProUGUI.text = _spanishText;
                break;
        }
    }
}
