using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public Language Language;
    
    private void Awake() {
        if(FindObjectsOfType<GameManager>().Length > 1) {
            DestroyImmediate(gameObject);
        }
        else {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private async void Start() {
        JSONFiles.CreateJsonFile("teste");

        await Task.Delay(1000);

        JSONFiles.SaveJsonFile("teste", "testando");
    }

    public void ChangeLanguage(Language language) {
        Language = language;

        foreach(TranslateObject item in FindObjectsOfType<TranslateObject>()) {
            item.ChangeLanguageTo(Language);
        }
    }
}

public class UserInfo {
    
}

public class World {
    public string name;
    public List<Phase> phases;
}

public class Phase {

}