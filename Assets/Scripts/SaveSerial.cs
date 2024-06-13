using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class SaveSerial : UnityEngine.MonoBehaviour
{ 

    public void SaveGame()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(UnityEngine.Application.persistentDataPath
          + "/MySaveData.dat");
        SaveData sd = new SaveData();
        sd.FPS = Settings.FPS ;
        sd.TPS = Settings.TPS ;
        sd.kBornEnergy = Settings.kBornEnergy;
        sd.kEnergyGrow = Settings.kEnergyGrow;
        sd.kGrow = Settings.kGrow;
        sd.kFind = Settings.kFind;
        sd.kEnemy = Settings.kEnemy;
        sd.kMaxNeighbour = Settings.kMaxNeighbour;
/*        sd.kMultiply = Settings.kMultiply;
*/        sd.kDeltMutate = Settings.kDeltMutate;
        sd.kPhotosintes = Settings.kPhotosintes;
        sd.kPredator = Settings.kPredator;
        sd.kGo = Settings.kGo;
        sd.camSpeed = Settings.camSpeed;
        sd.minSizeOrganism = Settings.minSizeOrganism;
        sd.maxSizeOrgainsm = Settings.maxSizeOrgainsm;
        sd.COG = Settings.COG;
        sd.CKG = Settings.CKG;
        sd.CZG = Settings.CZG;
        sd.sizeZones = Settings.sizeZones;
        sd.spreadZones = Settings.spreadZones;
        sd.minZoom = Settings.minZoom;
        sd.maxZoom = Settings.maxZoom;
        sd.speedZoom = Settings.speedZoom;
        sd.saveOrganisms = Settings.saveOrganisms;


    bf.Serialize(file, sd);
        file.Close();
    }

    public void LoadGame()
    {
        try
        {
            if (File.Exists(UnityEngine.Application.persistentDataPath
          + "/MySaveData.dat"))
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file =
                  File.Open(UnityEngine.Application.persistentDataPath
                  + "/MySaveData.dat", FileMode.Open);
                SaveData sd = (SaveData)bf.Deserialize(file);
                file.Close();
                Settings.FPS = sd.FPS;
                Settings.TPS = sd.TPS;
                Settings.kBornEnergy = sd.kBornEnergy;
                Settings.kEnergyGrow = sd.kEnergyGrow;
                Settings.kGrow = sd.kGrow;
                Settings.kFind = sd.kFind;
                Settings.kEnemy = sd.kEnemy;
                Settings.kMaxNeighbour = sd.kMaxNeighbour;
/*                Settings.kMultiply = sd.kMultiply;*/
                Settings.kDeltMutate = sd.kDeltMutate;
                Settings.kPhotosintes = sd.kPhotosintes;
                Settings.kPredator = sd.kPredator;
                Settings.kGo = sd.kGo;
                Settings.camSpeed = sd.camSpeed;
                Settings.minSizeOrganism = sd.minSizeOrganism;
                Settings.maxSizeOrgainsm = sd.maxSizeOrgainsm;
                Settings.COG = sd.COG;
                Settings.CKG = sd.CKG;
                Settings.CZG = sd.CZG;
                Settings.sizeZones = sd.sizeZones;
                Settings.spreadZones = sd.spreadZones;
                Settings.minZoom = sd.minZoom;
                Settings.maxZoom = sd.maxZoom;
                Settings.speedZoom = sd.speedZoom;
                if (sd.saveOrganisms != null)
                {
                    Settings.saveOrganisms = sd.saveOrganisms;
                }
                else
                {
                    Settings.saveOrganisms = new List<SaveOrganism>();
                }
            }
        }
        catch
        {

        }
            
    }

}


[Serializable]
class SaveData
{
    public int FPS;
    public int TPS;
    public float kBornEnergy;
    public float kEnergyGrow;
    public float kGrow;
    public float kFind;
    public float kEnemy;
    public float kMaxNeighbour;
/*    public float kMultiply;*/
    public float kDeltMutate;
    public float kPhotosintes;
    public float kPredator;
    public float kGo;
    public float camSpeed;
    public int minSizeOrganism;
    public int maxSizeOrgainsm;
    public int COG;
    public int CKG;
    public int CZG;
    public int sizeZones;
    public int spreadZones;
    public float minZoom;
    public float maxZoom;
    public float speedZoom;
    public List<SaveOrganism> saveOrganisms;
}
