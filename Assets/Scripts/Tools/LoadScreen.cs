using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadScreen : MonoBehaviour
{
    [SerializeField] private RectTransform _loadScreen;
    [SerializeField] private Slider _loadSlider;

    public static LoadScreen instance;

    private void Awake() {
        instance = this;
    }

    public void LoadSceneAsync(string sceneName) {
        _loadScreen.gameObject.SetActive(true);
        _loadScreen.GetComponent<CanvasGroup>().DOFade(1, .2f).OnComplete(() => {
            StartCoroutine(LoadAsync(sceneName));
        });
    }

    public void ReloadSceneAscyn(string sceneName) {
        StartCoroutine(ReloadAsync(sceneName));
    }

    private IEnumerator ReloadAsync(string sceneName) {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        while(!operation.isDone) {
            _loadSlider.value = operation.progress;
            yield return new WaitForEndOfFrame();
        };

        _loadSlider.value = 1.0f;
        yield return new WaitForSeconds(0.5f);

        HideLoadScreen();
    }

    private IEnumerator LoadAsync(string sceneName) {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        while(!operation.isDone) {
            _loadSlider.value = operation.progress;
            yield return new WaitForEndOfFrame();
        };

        _loadSlider.value = 1.0f;
        yield return new WaitForSeconds(0.5f);

        HideLoadScreen();
    }

    private void HideLoadScreen() {
        _loadScreen.GetComponent<CanvasGroup>().DOFade(0, .2f).OnComplete(() => {
            _loadScreen.gameObject.SetActive(false);
        });
    }
}
