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
        _loadScreen.GetComponent<CanvasGroup>().DOFade(1, .2f).OnComplete(async () => {
            StartCoroutine(LoadAsync(sceneName, previousScene));
        });
    }

    private IEnumerator LoadAsync(string sceneName, string previousScene) {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

        float percentage = 0;

        while(!operation.isDone) {
            DOTween.To(()=> percentage, x=> percentage = x, operation.progress, 0.1f).OnUpdate(() => {
                _loadSlider.value = percentage;
            });
            yield return null;
        }
        DOTween.To(() => percentage, x => percentage = x, operation.progress, 0.1f).OnUpdate(() => {
            _loadSlider.value = percentage;
        });

        SceneManager.UnloadSceneAsync(previousScene);

        yield return new WaitForSeconds(0.1f);

        HideLoadScreen();
    }

    private void HideLoadScreen() {
        _loadScreen.GetComponent<CanvasGroup>().DOFade(0, .2f).OnComplete(() => {
            _loadScreen.gameObject.SetActive(false);
        });
    }
}
