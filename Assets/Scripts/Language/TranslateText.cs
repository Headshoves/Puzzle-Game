using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TranslateText : TranslateObject {

    [InfoBox("English = 0 \n Portuguese = 1", EInfoBoxType.Normal)]
    [SerializeField] public List<string> Texts = new List<string>();
    
    private TextMeshProUGUI _textMeshProUGUI;

    private void Start() {
        _textMeshProUGUI = GetComponent<TextMeshProUGUI>();

        ChangeLanguageTo(FindObjectOfType<GameManager>().Language);
    }

    public override void ChangeLanguageTo(Language language) {
        _textMeshProUGUI.text = Texts[(int)language];
    }
}
