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

    public void LoadSceneAsync(string sceneName, string previousScene) {
        _loadScreen.gameObject.SetActive(true);
        _loadScreen.GetComponent<CanvasGroup>().DOFade(1, .2f).OnComplete(() => {
            StartCoroutine(LoadAsync(sceneName, previousScene));
        });
    }

    public void ReloadSceneAscyn(string sceneName) {
        StartCoroutine(ReloadAsync(sceneName));
    }

    private IEnumerator ReloadAsync(string sceneName) {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        while(!operation.isDone) {
            yield return new WaitForEndOfFrame();
        };

        yield return new WaitForSeconds(0.5f);

        HideLoadScreen();
    }

    private IEnumerator LoadAsync(string sceneName, string previousScene) {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        while(!operation.isDone) {
            yield return new WaitForEndOfFrame();
        };

        yield return new WaitForSeconds(0.5f);

        HideLoadScreen();
    }

    private void HideLoadScreen() {
        _loadScreen.GetComponent<CanvasGroup>().DOFade(0, .2f).OnComplete(() => {
            _loadScreen.gameObject.SetActive(false);
        });
    }
}
