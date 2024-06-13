using System.Collections;
using System.Collections.Generic;


public static class Settings
{

    public static int FPS = 60;
    public static int TPS = 100;
    /*    FPS_1S = 1 / FPS*/
    public static float TPS_1S = 1 / (TPS + 0.00001f);
    public static int[] SSN_choice = new int[5] { 8, 8, 8, 8, 4 };
    public static int[] SSN_go = new int[5] { 9, 8, 8, 8, 2 };
    public static int WIDTH = 1000;
    public static int HEIGHT = 1000;
    public static float[] kBaseSettings = new float[3] { 0.5f, 0.5f, 0.5f };
    public static float kBornEnergy = 15;
    public static float kEnergyGrow = 0.05f;
    public static float kGrow = 0.005f;
    public static float kFind = 1.1f;
    public static float kEnemy = 20;
    public static float kMaxNeighbour = 5;
    public static float kMultiply = 1;
    public static float kDeltMutate = 3;
    public static float kPhotosintes = 0.001f;
    public static float kPredator = 1;
    public static float kGo = 0.001f;
    public static float camSpeed = 15;
    public static Field field = new Field(kBaseSettings);
    public static int minSizeOrganism = 10;
    public static int maxSizeOrgainsm = 30;
    public static int COG = 5;
    public static int CKG = 200;
    public static int CZG = 20;
    public static int sizeZones = 200;
    public static int spreadZones = 100;
    public static float minZoom = 1.0f;
    public static float maxZoom = 5.0f;
    public static float speedZoom = 1.0f;
    public static bool showZone = false;
    public static int speedUpdateGraphes = 50;
    public static int criticalUpdateTime = 10;
    public static int lenGraph = 1000;
    public static int passGraph = 10;
    public static string pathToExe = System.IO.Path.Combine(UnityEngine.Application.dataPath, "map.exe");
    public static List<SaveOrganism> saveOrganisms = new List<SaveOrganism>();



    public static void setElement(string name, string var)
    {
        
        if (name == "FPS")
        {
            int.TryParse(var, out FPS);
            UnityEngine.Application.targetFrameRate = FPS;
        }
        else if (name == "TPS")
        {
            int.TryParse(var, out TPS);
            TPS_1S = 1 / (TPS + 0.00001f);
        }
        else if (name == "kBornEnergy")
        {
            float.TryParse(var, out kBornEnergy);
        }
        else if (name == "kEnergyGrow")
        {
            float.TryParse(var, out kEnergyGrow);
        }
        else if (name == "kGrow")
        {
            float.TryParse(var, out kGrow);
        }
        else if (name == "kFind")
        {
            float.TryParse(var, out kFind);
        }
        else if (name == "kEnemy")
        {
            float.TryParse(var, out kEnemy);
        }
        else if (name == "kMaxNeighbour")
        {
            float.TryParse(var, out kMaxNeighbour);
        }
/*        else if (name == "kMultiply")
        {
            float.TryParse(var, out kMultiply);
        }*/
        else if (name == "kDeltMutate")
        {
            float.TryParse(var, out kDeltMutate);
        }
        else if (name == "kPhotosintes")
        {
            float.TryParse(var, out kPhotosintes);
        }
        else if (name == "kPredator")
        {
            float.TryParse(var, out kPredator);
        }
        else if (name == "kGo")
        {
            float.TryParse(var, out kGo);
        }
        else if (name == "camSpeed")
        {
            float.TryParse(var, out camSpeed);
        }
        else if (name == "minZoom")
        {
            float.TryParse(var, out minZoom);
        }
        else if (name == "maxZoom")
        {
            float.TryParse(var, out maxZoom);
        }
        else if (name == "speedZoom")
        {
            float.TryParse(var, out speedZoom);
        }
        else if (name == "minSizeOrganism")
        {
            int.TryParse(var, out minSizeOrganism);
        }
        else if (name == "maxSizeOrgainsm")
        {
            int.TryParse(var, out maxSizeOrgainsm);
        }
        else if (name == "COG")
        {
            int.TryParse(var, out COG);
        }
        else if (name == "CKG")
        {
            int.TryParse(var, out CKG);
        }
        else if (name == "CZG")
        {
            int.TryParse(var, out CZG);
        }
        else if (name == "sizeZones")
        {
            int.TryParse(var, out sizeZones);
        }
        else if (name == "spreadZones")
        {
            int.TryParse(var, out spreadZones);
        }
    }

    public static float[] getGensToColor(float[] gens)
    {
        float[] color = new float[4];
        color[0] = gens[0] / 100;
        color[1] = gens[1] / 100;
        color[2] = gens[2] / 2;
        color[3] = gens[3] / 2 + 0.5f;
        return color;
    }
}
