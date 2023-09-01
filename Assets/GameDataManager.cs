using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataManager : MonoBehaviour
{
    // Create a field for the save file.
    string saveFile;

    // Create a GameData field.
    public Stats gameData;

    void Awake()
    {
        gameData = new Stats();
        // Update the path once the persistent path exists.
        saveFile = Application.persistentDataPath + "/gamedata.json";
        Debug.Log("gameDataHealth: " + gameData.health);
    }

    public void readFile()
    {
        // Does the file exist?
        if (File.Exists(saveFile))
        {
            // Read the entire file and save its contents.
            string fileContents = File.ReadAllText(saveFile);

            // Deserialize the JSON data 
            //  into a pattern matching the GameData class.
            gameData = JsonUtility.FromJson<Stats>(fileContents);
        }
    }

    public void writeFile()
    {
        // Serialize the object into JSON and save string.
        string jsonString = JsonUtility.ToJson(gameData);

        // Write JSON to file.
        File.WriteAllText(saveFile, jsonString);
    }

    // moved to PlayerStats
    // public void takeDamage(int damage) {
    //     gameData.health -= damage;
    // }
}