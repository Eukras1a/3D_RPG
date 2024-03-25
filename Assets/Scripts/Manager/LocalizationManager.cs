using ExcelDataReader;
using System.Collections.Generic;
using System.Data;
using System.IO;
using UnityEngine;

public class LocalizationManager : Singleton<LocalizationManager>
{
    public enum LanguageState
    {
        ChineseSimplified,
        English,
    }
    LanguageState languageState = LanguageState.ChineseSimplified;
    SystemLanguage mainLanguage = SystemLanguage.ChineseSimplified;
    public LanguageState CurrentLanguageState
    {
        get
        {
            return languageState;
        }
        set
        {
            if (languageState != value)
            {
                languageState = value;
                OnLanguageChanged();
            }
        }
    }
    public Dictionary<SystemLanguage, Dictionary<string, string>> LocalizedDic = new Dictionary<SystemLanguage, Dictionary<string, string>>();
    List<ILocalizationController> lcList = new List<ILocalizationController>();
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }
    private void Start()
    {
        ReadFromFile();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Debug.Log(GetLocalization("start_game"));
            
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            CurrentLanguageState = LanguageState.English;
        }
    }
    public void AddLocalizationController(ILocalizationController lc)
    {
        lcList.Add(lc);
    }
    public void RemoveLocalizationController(ILocalizationController lc)
    {
        lcList.Remove(lc);
    }
    void OnLanguageChanged()
    {
        mainLanguage = languageState switch
        {
            LanguageState.ChineseSimplified => SystemLanguage.ChineseSimplified,
            LanguageState.English => SystemLanguage.English,
            _ => SystemLanguage.Unknown
        };
        foreach (var i in lcList)
        {
            i.ChangeLanguage();
        }
    }
    void ReadFromFile()
    {
        string filePath = Application.dataPath + "/Game Data/LocalizationData.xlsx";
        FileStream stream = File.Open(filePath, FileMode.Open, FileAccess.Read);
        IExcelDataReader excelReader = ExcelReaderFactory.CreateReader(stream);
        DataSet result = excelReader.AsDataSet();
        DataTable dataTable = result.Tables[0];
        for (int i = 1; i < dataTable.Rows.Count; i++)
        {
            for (int j = 0; j < dataTable.Columns.Count; j++)
            {
                SetLocalization(GetLanguage(dataTable.Rows[0][j].ToString()), dataTable.Rows[i][0].ToString(), dataTable.Rows[i][j].ToString());
            }
        }
        excelReader.Close();
        stream.Close();
        Debug.Log("加载成功");
    }
    SystemLanguage GetLanguage(string flag)
    {
        SystemLanguage language = flag switch
        {
            "EN" => SystemLanguage.English,
            "CN" => SystemLanguage.ChineseSimplified,
            _ => SystemLanguage.Unknown,
        };
        return language;
    }
    void SetLocalization(SystemLanguage language, string flag, string target)
    {
        if (!LocalizedDic.ContainsKey(language))
        {
            LocalizedDic[language] = new Dictionary<string, string>();
        }
        if (!LocalizedDic[language].ContainsKey(flag))
        {
            LocalizedDic[language].Add(flag, target);
        }
    }
    public string GetLocalization(string flag)
    {
        if (LocalizedDic[mainLanguage].ContainsKey(flag))
        {
            return LocalizedDic[mainLanguage][flag];
        }
        Debug.LogError("不存在该文本！");
        return "";
    }
}
