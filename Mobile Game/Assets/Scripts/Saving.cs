using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class Saving : MonoBehaviour
{
    public void Save(int highScore) {
        Save save = new Save(highScore);

        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/save.data";
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, save);
        stream.Close();
    }

    public Save GetSave() {
        string path = Application.persistentDataPath + "/save.data";
        if (File.Exists(path)) {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            Save save = formatter.Deserialize(stream) as Save;
            stream.Close();

            return save;
        } else {
            Debug.Log("save not found");
            return null;
        }
    }
}
