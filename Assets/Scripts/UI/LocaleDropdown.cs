using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class LocaleDropdown : MonoBehaviour
{
    public Dropdown languageDD;
    bool active = false;
    List<string> languages = new List<string> {
        "简体中文","繁體中文","English","jp"
    };

    private void OnEnable()
    {
        languageDD.onValueChanged.AddListener(OnDropdownValueChanged);
    }
    private void Start()
    {
        LoadLanguage();
    }
    private void OnDisable()
    {
        languageDD.onValueChanged.RemoveListener(OnDropdownValueChanged);
    }
    private void OnDropdownValueChanged(int localeID)
    {
        if (active)
            return;
        StartCoroutine(SetLocale(localeID));
    }
    void LoadLanguage()
    {
        languageDD.ClearOptions();
        List<Dropdown.OptionData> options = new List<Dropdown.OptionData>();
        foreach (var item in languages)
        {
            options.Add(new Dropdown.OptionData(item));
        }
        languageDD.options = options;
    }
    IEnumerator SetLocale(int localeID)
    {
        active = true;
        yield return LocalizationSettings.InitializationOperation;
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[localeID];
        active = false;
    }
}
