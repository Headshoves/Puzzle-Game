using NaughtyAttributes;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private UserInfo _user;
    [SerializeField] private List<World> Worlds;

    private TextBox _textBox;

    public int PhaseIndex = 0;
    public World World;
    
    private void Awake() {
        if(FindObjectsOfType<GameManager>().Length > 1) {
            DestroyImmediate(gameObject);
        }
        else {
            DontDestroyOnLoad(gameObject);
            Instance = this;
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

    public List<World> GetWorlds() {
        return Worlds;
    }
    
    public string GetNextPhaseId(){ return World.Phases[PhaseIndex+1].PhaseId; }

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

    public void CompleteLevel() {
        for(int i = 0; i < _user.WorldData.Count; i++) {
            if (_user.WorldData[i] == World) {
                _user.WorldData[i].Phases[PhaseIndex].Completed = true;
                PhaseIndex++;
                JSONFiles.SaveJsonFile("user", _user);
                break;
            }
        }
    }

    public bool LastLevel(){ return PhaseIndex >= World.Phases.Count - 1;}

    #endregion
}

[Serializable]
public class UserInfo {
    public List<World> WorldData;

    public UserInfo(List<World> worldData) {
        WorldData = worldData;
    }
}

[Serializable]
public class World {
    public string Name;
    public List<Phase> Phases;
}

[Serializable]
public class Phase {
    public string PhaseId;
    public string Name;
    public bool Completed;
}