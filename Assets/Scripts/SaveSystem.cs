using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void saveGame(PlayerMovement playerMovement, ItemManager itemManager)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string savePath = Application.dataPath + "/gameData.bin";
        FileStream stream = new FileStream(savePath, FileMode.Create);
        gameData data = new gameData(playerMovement, itemManager);
        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static gameData loadGame()
    {   
        string savePath = Application.dataPath + "/gameData.bin";
        if(File.Exists(savePath))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(savePath, FileMode.Open);
            gameData data = formatter.Deserialize(stream) as gameData;
            stream.Close();
            return data;
        }
        else
        {
            Debug.Log("Error: Save file not found at " + savePath);
            return null;
        }
    }

    public static void deleteGame()
    {
        string savePath = Application.dataPath + "/gameData.bin";
        if(File.Exists(savePath))
        {
            File.Delete(savePath);
        }
        else
        {
            Debug.Log("Error: File already deleted at " + savePath);
            return;
        }
    }
}
