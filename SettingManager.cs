using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SettingManager : MonoBehaviour
{
    const string fileName = "UserData.txt";
    public static SettingManager Instance{get; private set;}
    public string[][] _content;

    const string DateTimeFormat = "yyyy/MM/dd/HH:mm:ss";
    void Awake()
    {
        if(Instance)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            _content = new string[][]{
                new string[]{"PlayCount", "0"},
                new string[]{"PlayTime/m", "0"},
                new string[]{"LastPlay", "2000/01/01/00:00:01"}
            };
            DataManager.Instance.LoadCSV(ref _content, fileName);

            float _PlayCount = 0;
            float.TryParse(_content[0][1], out _PlayCount);
            _PlayCount++;
            _content[0][1] = _PlayCount.ToString();
            DateTime _dt = DateTime.Now;
            _content[2] = new string[]{"LastPlay", _dt.ToString(DateTimeFormat)};

            Settings.Load();
            Debug.Log(Settings.Instance.MainVolume);
        }
    }
    
    public void OnApplicationQuit()
    {
        DateTime _dtTarget = DateTime.ParseExact(_content[2][1], DateTimeFormat, null);
        DateTime _dt = DateTime.Now;
        _content[2] = new string[]{"LastPlay", _dt.ToString(DateTimeFormat)};
        TimeSpan _ts = _dt - _dtTarget;
        float _playTime = 0;
        float.TryParse(_content[1][1], out _playTime);
        _content[1] = new string[]{"PlayTime/m", (_playTime + _ts.TotalMinutes).ToString()};
        DataManager.Instance.SaveCSV(_content, fileName);

        Settings.Save();
        Debug.Log("QuitSave");
    }
}

public class Settings
{
    const string fileName = "setting.json";

    public float MainVolume = 0.1f;

    public static Settings Instance;
    
    static public void Save()
    {
        DataManager.Instance.SaveJson<Settings>(Instance, fileName);
    }

    static public void Load()
    {
        Instance = new Settings();
        DataManager.Instance.LoadJson<Settings>(ref Instance, fileName);
    }
}