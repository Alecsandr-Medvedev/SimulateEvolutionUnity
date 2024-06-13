using System;
using System.Collections;
using System.Collections.Generic;

public class Updater : UnityEngine.MonoBehaviour
{
    private List<Organism> organisms_= new List<Organism> { };
    private List<Zone> zones_ = new List<Zone> { };

    private bool run = true;
    private bool pause = true;
    private ulong countUpdates = 0;
    private int lastFiftyUpdates = 0;
    private float lastTimeUpdate = 0;
    private List<ulong> freeIds = new List<ulong> { };
    private ulong startId = 0;
    private bool mouseLock = false;
/*    private double lastRedraw;*/
    Random random = new Random();
    public UnityEngine.Camera camera_;

    public UnityEngine.GameObject OrganismPrefab;
    public UnityEngine.GameObject ZonePrefab;

    public SaveSerial gameSaver;
    public PythonIntegration graphEnergy;
    public PythonIntegration graphSize;
    public PythonIntegration graphGens;
    public TMPro.TMP_InputField secretText;

    private ulong selectOrganism = 0;
    private Organism selectOrganismO;
    private MyQuere numsEnergy;
    private MyQuere numsSize;
    private MyQuere numsGenPr;
    private MyQuere numsGenPh;
    private MyQuere numsGenSp;
    private MyQuere numsGenGr;
    private int timeRedraw = 0;
    float startTime = 0;

    [UnityEngine.SerializeField]
    private UnityEngine.Canvas canvas;
    [UnityEngine.SerializeField]
    private UnityEngine.GameObject viewSavedOrganisms;
    [UnityEngine.SerializeField]
    private UnityEngine.GameObject prefabSavedOrganism;

    [UnityEngine.SerializeField]
    private TMPro.TextMeshProUGUI timeText;
    [UnityEngine.SerializeField]
    private TMPro.TextMeshProUGUI speedText;
    [UnityEngine.SerializeField]
    private UnityEngine.GameObject organismInfo;
    [UnityEngine.SerializeField]
    private UnityEngine.UI.Image colorOrganismInfo;
    [UnityEngine.SerializeField]
    private TMPro.TextMeshProUGUI gen1;
    [UnityEngine.SerializeField]
    private TMPro.TextMeshProUGUI gen2;
    [UnityEngine.SerializeField]
    private TMPro.TextMeshProUGUI gen3;
    [UnityEngine.SerializeField]
    private TMPro.TextMeshProUGUI gen4;

    private void Awake()
    {
        gameSaver.LoadGame();
        AddSavedObjectToView();
        numsEnergy = new MyQuere(Settings.lenGraph);
        numsSize = new MyQuere(Settings.lenGraph);
        numsGenPr = new MyQuere(Settings.lenGraph);
        numsGenPh = new MyQuere(Settings.lenGraph);
        numsGenSp = new MyQuere(Settings.lenGraph);
        numsGenGr = new MyQuere(Settings.lenGraph);
    }

    public void SaveGame()
    {
        gameSaver.SaveGame();
    }

    private void GenerateWorld()
    {
        GenerateOrganisms(Settings.CKG, Settings.COG);
        GenerateZones(Settings.CZG, Settings.sizeZones, Settings.spreadZones);
        camera_ = UnityEngine.Camera.main;
        camera_.transform.position = new UnityEngine.Vector3(Settings.WIDTH / 200, Settings.HEIGHT / 200, -10);
    }

    private void GenerateOrganisms(InfoBorn bornData)
    {
       Organism org = new Organism(bornData.getRect(), getFreeId(), bornData.getGens(), bornData.getNeuralChoice(), bornData.getNeuralGo());
       UnityEngine.GameObject obj = Instantiate(OrganismPrefab);
       obj.GetComponent<UnityObject>().Initialize(org);
       organisms_.Add(org);
       Settings.field.addOrganism(org);
    }

    private void GenerateOrganisms(int countKinds, int countOrganisms)
    {
        for (int i = 0; i < countKinds; i++)
        {
            int width10 = (int)(Settings.WIDTH * 0.1f);
            int height10 = (int)(Settings.HEIGHT * 0.1f);

            int x = random.Next(width10, Settings.WIDTH - width10);
            int y = random.Next(height10, Settings.HEIGHT - height10);
            int size = random.Next(Settings.minSizeOrganism, Settings.maxSizeOrgainsm);
            int pr = random.Next(0, 100);
            int ph = 100 - pr;
            float gR = random.Next(0, 100) / 100.0f;
            float mS = random.Next(0, 200) / 100.0f;


            for (int j = 0; j < countOrganisms; j++)
            {
                Organism org = new Organism(x + (float)(random.NextDouble() * 2 - 1) * size , y + (float)(random.NextDouble() * 2 - 1) * size, size, getFreeId(),
                    new float[] { pr, ph, gR, mS });
                organisms_.Add(org);
                Settings.field.addOrganism(org);
                UnityEngine.GameObject obj = Instantiate(OrganismPrefab);
                obj.GetComponent<UnityObject>().Initialize(org);
            }
        }
    }

    private ulong getFreeId()
    {
        if (freeIds.Count != 0)
        {
            int lastIndex = freeIds.Count - 1;
            ulong freeId = freeIds[lastIndex];
            freeIds.RemoveAt(lastIndex);
            return freeId;
        }
        else
        {
            startId++;
            return startId - 1;
        }
    }

    private void GenerateZones(int count, int size, int spread)
    {
        for (int i = 0; i < count; i++)
        {
            Rect rect = new Rect(random.Next(0, Settings.WIDTH), random.Next(0, Settings.HEIGHT), random.Next(size - spread, size + spread), random.Next(size - spread, size + spread));
            Zone zone = new Zone(rect, (float)random.NextDouble(), (float)random.NextDouble(), (float)random.NextDouble(), getFreeId(), Settings.showZone);
            Settings.field.addZone(zone);
            zones_.Add(zone);
            UnityEngine.GameObject obj = Instantiate(ZonePrefab);
            obj.GetComponent<UnityObject>().Initialize(zone);
        }
        
    }

    public void changeShowZone()
    {
        if (Settings.showZone)
        {
            Settings.showZone = false;
        
        }
        else
        {
            Settings.showZone = true;
        }
        for (int i = 0; i < zones_.Count; i++)
        {
            zones_[i].setShow(Settings.showZone);
        }
    }

    public void Start()
    {
        UnityEngine.Application.targetFrameRate = Settings.FPS;
        StartCoroutine(UpdateOrganisms());
    }

    public void Update()
    {
        if (!mouseLock)
        {
            if (UnityEngine.Input.GetMouseButton(0))
            {
                PanCameraWithMouse();
            }
/*            if (UnityEngine.Input.GetMouseButtonDown(1)) // 1 означает правую кнопку мыши
            {
                // Проверяем, что позиция нажатия находится над спрайтом
                UnityEngine.Vector2 mousePosition = UnityEngine.Camera.main.ScreenToWorldPoint(UnityEngine.Input.mousePosition);
                UnityEngine.RaycastHit2D hit = UnityEngine.Physics2D.Raycast(mousePosition, UnityEngine.Vector2.zero);

                if (hit.collider != null && hit.collider.tag == "organism")
                {
                    if (selectOrganismO != null)
                    {
                        selectOrganismO.setSelect(false);
                    }
                    
                    UnityEngine.RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as UnityEngine.RectTransform, UnityEngine.Input.mousePosition, canvas.worldCamera, out UnityEngine.Vector2 localPoint);
                    selectOrganism = hit.collider.gameObject.GetComponent<UnityObject>().Collide();
                    UnityEngine.Debug.Log(selectOrganism);
                    for (int i = 0; i < organisms_.Count; i++)
                    {
                        if (organisms_[i].getId() == selectOrganism)
                        {
                            selectOrganismO = organisms_[i];
                        }
                    }

                    selectOrganismO.setSelect(true);


                    *//*setSelectOrganismInfo();*/
                    /*CreateButton("Сохранить", localPoint.x + 10, localPoint.y + 10, 100, 30, SaveOrganism);*//*
                }
            }*/
            CameraZoom();
        }
        if (UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.Space))
        {
            changePause();
        }
        
    }
    void CameraZoom()
    {
        float zoomDelta = UnityEngine.Input.GetAxis("Mouse ScrollWheel") * Settings.speedZoom;

        UnityEngine.Camera.main.orthographicSize = UnityEngine.Mathf.Clamp(UnityEngine.Camera.main.orthographicSize - zoomDelta, Settings.minZoom, Settings.maxZoom);
    }
    void PanCameraWithMouse()
    {
        float horizontal = -UnityEngine.Input.GetAxis("Mouse X");
        float vertical = -UnityEngine.Input.GetAxis("Mouse Y");

        UnityEngine.Vector3 moveDirection = new UnityEngine.Vector3(horizontal, vertical, 0f);

        moveDirection.Normalize();

        camera_.transform.Translate(moveDirection * Settings.camSpeed * (UnityEngine.Camera.main.orthographicSize / Settings.maxZoom) * 0.015f, UnityEngine.Space.World);

    }

    public void changeMouseLock()
    {
        if (mouseLock)
        {
            mouseLock = false;
        }
        else
        {
            mouseLock = true;
        }
    }

    public void changePause()
    {
        if (pause)
        {
            pause = false;
        }
        else
        {
            pause = true;
        }
    }

    private IEnumerator UpdateOrganisms()
    {
        startTime += UnityEngine.Time.time;
        lastTimeUpdate = startTime;
        DrawGraphes();
        while (run){
            

            if (! pause)
            {
                
                lastFiftyUpdates++;
                if (lastFiftyUpdates == 50)
                {
                    countUpdates += 50;
                    float speedUpdates = UnityEngine.Mathf.Round(500 / (UnityEngine.Time.time - lastTimeUpdate)) / 10;
                    lastTimeUpdate = UnityEngine.Time.time;
                    lastFiftyUpdates = 0;
                    float tTimeUpdates = UnityEngine.Mathf.Round(countUpdates / 100) / 10;

                    timeText.text = $"Время {tTimeUpdates} тыс обн";
                    speedText.text = $"Скорость {speedUpdates} обн / c";
                }

                int countOrganisms = 0;
                float averageEnergy = 0;
                float averageSize = 0;
                float averagePrGen = 0;
                float averagePhGen = 0;
                float averageSpGen = 0;
                float averageGrGen = 0;
                int i = 0;
                while (i < organisms_.Count)
                {
                    Organism org = organisms_[i];
                    org.Update();
                    if (org.IsAlive())
                    {
                        averageEnergy += org.getEnergy();
                        averageSize += org.getSize();
                        countOrganisms++;
                        float[] gs = org.getGens();
                        averagePrGen += gs[0];
                        averagePhGen += gs[1];
                        averageSpGen += gs[2];
                        averageGrGen += gs[3];
                        i += 1;
                    }
                    else
                    {

                        Settings.field.delOrganism(org.getId());
                        organisms_.RemoveAt(i);

                    }
                }
                if (countOrganisms > 0)
                {
                    averageEnergy /= countOrganisms;
                    averageSize /= countOrganisms;
                    averagePrGen /= countOrganisms;
                    averagePhGen /= countOrganisms;
                    averageSpGen /= countOrganisms;
                    averageGrGen /= countOrganisms;
                    averageSpGen *= 50;
                    averageGrGen *= 100;
                    timeRedraw++;
                    numsEnergy.Add(averageEnergy);
                    numsSize.Add(averageSize);
                    numsGenPr.Add(averagePrGen);
                    numsGenPh.Add(averagePhGen);
                    numsGenSp.Add(averageSpGen);
                    numsGenGr.Add(averageGrGen);
                }
                    if (timeRedraw > Settings.speedUpdateGraphes && UnityEngine.Time.time - startTime > Settings.criticalUpdateTime)
                    {
                        startTime = 0 + UnityEngine.Time.time;
                        timeRedraw = 0;
                    DrawGraphes();
                    }
                
               
               

                
                

                for (int j = 0; j < organisms_.Count; j++)
                {
                    InfoBorn bornData = organisms_[j].GetBorn();
                    if (bornData.getBorn())
                    {

                        GenerateOrganisms(bornData);
                    }
                }
            }

            if (Settings.TPS > 0)
            {
                yield return new UnityEngine.WaitForSeconds(Settings.TPS_1S);
            }
            else
            {
                yield return null;
            }


        }

    }

    private void DrawGraphes()
    {
        graphEnergy.SendDataToPython(numsEnergy.GetStringNums(), "orange", "Средняя_энергия", "энергия");
        graphEnergy.LoadImageFromFile();
        graphSize.SendDataToPython(numsSize.GetStringNums(), "green", "Средний_рост", "рост");
        graphSize.LoadImageFromFile();
        graphGens.SendDataToPython(numsGenPr.GetStringNums() + ";" + numsGenPh.GetStringNums() + ";" + numsGenSp.GetStringNums() + ";" + numsGenGr.GetStringNums(), "red;green;blue;black", "Гены", "Хищничество;Фотосинтез;Скорость;Скорость_роста");
        graphGens.LoadImageFromFile();
    }
    public void QuitGame()
    {
        SaveGame();
#if UNITY_EDITOR
        // Если игра выполняется в режиме редактора Unity, останавливаем игровой режим
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // Если игра выполняется вне режима редактора, выходим из приложения
        UnityEngine.Application.Quit();
#endif
    }

    public void ResetGame()
    {
        for (int i = 0; i < zones_.Count; i++)
        {
            zones_[i].setAlive(false);
            Settings.field.delZone(zones_[i].getId());

        }
        zones_.Clear();
        for (int i = 0; i < organisms_.Count; i++)
        {
            organisms_[i].setAlive(false);
            Settings.field.delOrganism(organisms_[i].getId());
        }
        organisms_.Clear();
        GenerateWorld();
        countUpdates = 0;
        lastFiftyUpdates = 0;
        pause = false;
        timeText.text = $"Время ... тыс обн";
        speedText.text = $"Скорость ... обн / c";
        numsEnergy.Clear();
        numsSize.Clear();
        numsGenPr.Clear();
        numsGenPh.Clear();
        numsGenSp.Clear();
        numsGenGr.Clear();
    }

    public void Secret()
    {
        secretText.text = Settings.pathToExe;
    }

    private void AddSavedObjectToView()
    {
        for (int i = 0; i < Settings.saveOrganisms.Count; i++)
        {
            UnityEngine.GameObject sO = Instantiate(prefabSavedOrganism, viewSavedOrganisms.transform);
            sO.GetComponent<OrganismCard>().Init(Settings.saveOrganisms[i].getName(), Settings.saveOrganisms[i].getGens());

            UnityEngine.UI.LayoutRebuilder.ForceRebuildLayoutImmediate(viewSavedOrganisms.GetComponent<UnityEngine.RectTransform>());
        }
       
    }

   /* public void CreateButton(string text, float x, float y, int width, int height, UnityEngine.Events.UnityAction func)

    {
        UnityEngine.Debug.Log($"{x}, {y}");
        UnityEngine.GameObject buttonGameObject = new UnityEngine.GameObject("Button");

        // Добавляем компонент кнопки
        UnityEngine.UI.Button buttonComponent = buttonGameObject.AddComponent<UnityEngine.UI.Button>();
        UnityEngine.RectTransform rectTransform = buttonGameObject.AddComponent<UnityEngine.RectTransform >();


        // Устанавливаем родительский объект (может быть Canvas или другой объект с компонентом RectTransform)
        buttonGameObject.transform.SetParent(canvas.transform, false);

        // Задаем положение и размеры кнопки
        rectTransform.sizeDelta = new UnityEngine.Vector2(width, height); // Размеры кнопки (ширина, высота)
        rectTransform.localPosition = new UnityEngine.Vector3(x, y, 0); // Положение кнопки относительно родительского объекта
        UnityEngine.UI.Image imageComponent = buttonGameObject.AddComponent<UnityEngine.UI.Image>();
        imageComponent.color = UnityEngine.Color.white;

        // Добавляем текст на кнопку
        UnityEngine.GameObject textGameObject = new UnityEngine.GameObject("Text");
        textGameObject.transform.SetParent(buttonGameObject.transform);
        UnityEngine.UI.Text textComponent = textGameObject.AddComponent<UnityEngine.UI.Text>();
        textComponent.text = text; // Текст на кнопке
        textComponent.font = UnityEngine.Resources.GetBuiltinResource<UnityEngine.Font>("Arial.ttf"); // Шрифт текста
        textComponent.alignment = UnityEngine.TextAnchor.MiddleCenter; // Выравнивание текста по центру
        UnityEngine.RectTransform textRectTransform = textGameObject.GetComponent<UnityEngine.RectTransform>();
        textRectTransform.sizeDelta = new UnityEngine.Vector2(width , height); // Размеры текста (ширина, высота)

        // Добавляем функцию, которая будет вызываться при нажатии на кнопку
        buttonComponent.onClick.AddListener(func);
    }*/
    public void SaveOrganism()
    {
        for (int i = 0; i < organisms_.Count; i++)
        {
            if (organisms_[i].getId() == selectOrganism)
            {
                /*float[] color = organisms_[i].getColor();
                float[] gens = organisms_[i].getGens();
                SaveOrganism sO = new SaveOrganism($"{selectOrganism}", organisms_)
                break;*/
            }
        }
        
    }
    public void setSelectOrganismInfo()
    {
        organismInfo.SetActive(true);

        for (int i = 0; i < organisms_.Count; i++)
        {
            if (organisms_[i].getId() == selectOrganism)
            {
                float[] color = organisms_[i].getColor();
                float[] gens = organisms_[i].getGens();
                colorOrganismInfo.color = new UnityEngine.Color(color[0], color[1], color[2], color[4]);
                gen1.text = $"П: {gens[0]}";
                gen2.text = $"Ф: {gens[1]}";
                gen3.text = $"С: {gens[2]}";
                gen4.text = $"Р: {gens[3]}";
                /*selectOrganismO = new Organism();*/
                break;
            }
        }
        
    }
}

public struct MyQuere
{
    int[] _quere;
    int _len;
    int pass;
    public MyQuere(int len)
    {
        _quere = new int[len];
        pass = 0;
        _len = len;
        for (int i = 0; i < len; i++)
        {
            _quere[i] = 0;
        }
    }

    public int[] GetNums()
    {
        return _quere;
    }

    public void Add(float num)
    {
        pass += 1;
        if (pass == Settings.passGraph)
        {
            for (int i = 0; i < _len - 1; i++)
            {
                _quere[i] = _quere[i + 1];
            }
            _quere[_len - 1] = (int)num;
            pass = 0;
        }
        
    }

    public string GetStringNums()
    {
        string ans = "";
        for (int i = 0; i <_len; i++)
        {
            ans += $",{_quere[i]}";
        }
        ans = ans.Substring(1, ans.Length - 1);
        return ans;
    }

    public void Clear()
    {
        for(int i = 0; i < _len - 1; i++)
            {
            _quere[i] = 0;
        }
    }
}


