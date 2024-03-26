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
        ChineseTraditional,
        Other
    }
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
    LanguageState languageState = LanguageState.ChineseSimplified;
    Dictionary<LanguageState, Dictionary<string, string>> LocalizedDic = new Dictionary<LanguageState, Dictionary<string, string>>();
    Dictionary<string, LanguageState> languageOption = new Dictionary<string, LanguageState>();
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
        foreach (var i in lcList)
        {
            i.ChangeLanguage();
        }
    }
    void ReadFromFile()
    {
        LocalizedDic.Clear();
        languageOption.Clear();
        string filePath = Path.Combine(Application.streamingAssetsPath, "LocalizationData.xlsx");
        FileStream stream = File.Open(filePath, FileMode.Open, FileAccess.Read);
        IExcelDataReader excelReader = ExcelReaderFactory.CreateReader(stream);
        DataSet result = excelReader.AsDataSet();
        DataTable dataTable = result.Tables[0];
        for (int i = 1; i < dataTable.Rows.Count; i++)
        {
            for (int j = 0; j < dataTable.Columns.Count; j++)
            {
                SetLocalization(GetLanguageState(dataTable.Rows[0][j].ToString()), dataTable.Rows[i][0].ToString(), dataTable.Rows[i][j].ToString());
            }
        }
        for (int i = 1; i < dataTable.Columns.Count; i++)
        {
            languageOption.Add(dataTable.Rows[0][i].ToString(), GetLanguageState(dataTable.Rows[0][i].ToString()));
        }
        excelReader.Close();
        stream.Close();
    }
    public LanguageState GetLanguageState(string flag)
    {
        LanguageState language = flag switch
        {
            "简体中文" => LanguageState.ChineseSimplified,
            "English" => LanguageState.English,
            "繁體中文" => LanguageState.ChineseTraditional,
            _ => LanguageState.Other,
        };
        return language;
    }
    public void SetLanguageState(LanguageState state)
    {
        CurrentLanguageState = state;
    }
    void SetLocalization(LanguageState language, string flag, string target)
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
        if (LocalizedDic[languageState].ContainsKey(flag))
        {
            return LocalizedDic[languageState][flag];
        }
        Debug.LogError("不存在该文本！");
        return "";
    }
    public Dictionary<string, LanguageState> GetLanguageOptions()
    {
        return languageOption;
    }
}
