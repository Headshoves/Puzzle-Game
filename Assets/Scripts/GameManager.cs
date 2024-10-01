using NaughtyAttributes;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public Language Language;

    [SerializeField] private UserInfo _user;
    [SerializeField] private List<World> Worlds;

    private TextBox _textBox;
    
    private void Awake() {
        if(FindObjectsOfType<GameManager>().Length > 1) {
            DestroyImmediate(gameObject);
        }
        else {
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start() {

        if (JSONFiles.ContainsJsonFile("user")) {
            _user = JSONFiles.LoadJsonFile<UserInfo>("user");
        }
        else {
            JSONFiles.CreateJsonFile("user");
            _user = new UserInfo(Worlds);
        }

        _textBox = GetComponent<TextBox>();
    }

    public void ChangeLanguage(Language language) {
        Language = language;
        _user.language = language;

        JSONFiles.SaveJsonFile("user", _user);

        foreach(TranslateObject item in FindObjectsOfType<TranslateObject>()) {
            item.ChangeLanguageTo(Language);
        }
    }

    public List<World> GetWorlds() {
        return Worlds;
    }

    #region GAME

    public void PauseGame() {
        foreach(Piece piece in FindObjectsOfType(typeof(Piece))) {
            piece.CanPlay = false;
        }
    }

    public void ResumeGame() {
        foreach (Piece piece in FindObjectsOfType(typeof(Piece))) {
            piece.CanPlay = true;
        }
    }

    public void CompleteLevel(int world, string phase) {
        for(int i = 0; i < _user.WorldData[world].Phases.Count; i++) {
            if (_user.WorldData[world].Phases[i].PhaseId == phase) {
                _user.WorldData[world].Phases[i].Completed = true;
                JSONFiles.SaveJsonFile("user", _user);
            }
        }
    }

    #endregion
}

[Serializable]
public class UserInfo {
    public Language language;

    public List<World> WorldData;

    public UserInfo(List<World> worldData) {
        language = Language.Portuguese;
        WorldData = worldData;
    }
}

[Serializable]
public class World {
    [InfoBox("English = 0 \n Portuguese = 1", EInfoBoxType.Normal)]
    public string[] Names;
    public List<Phase> Phases;
}

[Serializable]
public class Phase {
    public string PhaseId;
    [InfoBox("English = 0 \n Portuguese = 1", EInfoBoxType.Normal)]
    public string[] Names;
    public bool Completed;
}