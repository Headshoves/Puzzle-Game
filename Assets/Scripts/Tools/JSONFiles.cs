using System.IO;
using UnityEngine;

public static class JSONFiles
{
    public static void CreateJsonFile(string filename) {
        CheckJsonFolder();
        if (!File.Exists(Application.persistentDataPath + "/JSON/" + filename)) {
            File.Create(Application.persistentDataPath + "/JSON/" + filename + ".json");
        }
    }

    private static void CheckJsonFolder() {
        Debug.Log("Chamou o método de criar json folder");
        if (!Directory.Exists(Application.persistentDataPath + "/JSON")) {
            Directory.CreateDirectory(Application.persistentDataPath + "/JSON");
        }
    }

    public static async void SaveJsonFile<T>(string filename, T info) {
        string data = JsonUtility.ToJson(info);

        await File.WriteAllTextAsync(Application.persistentDataPath + "/JSON/" + filename + ".json", data);
    }
}
