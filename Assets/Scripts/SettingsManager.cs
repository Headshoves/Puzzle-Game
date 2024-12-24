using System;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] private GameObject _settingsPanel;
    [SerializeField] private GameObject _returnMenuButton;
    [SerializeField] private GameObject _exitButton;
    [SerializeField] private GameObject _settingsButton;
    [SerializeField] private GameObject _closeSettingsButton;

    private void Start()
    {
        _returnMenuButton.GetComponent<Button>().onClick.RemoveAllListeners();
        _returnMenuButton.GetComponent<Button>().onClick.AddListener(GoToMenu);
        
        _exitButton.GetComponent<Button>().onClick.RemoveAllListeners();
        _exitButton.GetComponent<Button>().onClick.AddListener(Application.Quit);
        
        _settingsButton.GetComponent<Button>().onClick.RemoveAllListeners();
        _settingsButton.GetComponent<Button>().onClick.AddListener(ShowSettings);
        
        _closeSettingsButton.GetComponent<Button>().onClick.RemoveAllListeners();
        _closeSettingsButton.GetComponent<Button>().onClick.AddListener(HideSettings);
    }
    
    private async void GoToMenu(){
        LoadScreen.instance.LoadSceneAsync("Menu");
        await Task.Delay(TimeSpan.FromSeconds(0.2f));
        _settingsPanel.SetActive(false);
        _settingsPanel.GetComponent<CanvasGroup>().alpha = 0;
    }

    private void ShowSettings(){
        _settingsPanel.SetActive(true);
        _settingsPanel.GetComponent<CanvasGroup>().DOFade(1, 0.2f);
    }

    private void HideSettings(){
        _settingsPanel.GetComponent<CanvasGroup>().DOFade(0, 0.2f).OnComplete(() => {
            _settingsPanel.SetActive(false);
        });
    }
}
