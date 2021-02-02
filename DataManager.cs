using System.IO;
using System.Text;
using System.Collections.Generic;
using UnityEngine;

// ファイル保存関係。2021年2月2日
public class DataManager
{
    static DataManager instace;
    string filePath = "";

    public static DataManager Instance
    {
        get
        {
            if(instace == null){ instace = new DataManager(); }
            return instace;
        }
    }

    DataManager()
    {
        filePath = Application.dataPath + "/.SaveData/";
        if(!Directory.Exists(filePath))
        {
            Directory.CreateDirectory(filePath);
            Debug.Log("DataManager : .SaveDataフォルダを新規作成しました" + filePath);
        }
    }

    public bool IsFile(string _str)
    {   // ファイル/ディレクトリがが存在するか？
        return File.Exists(filePath + _str);
    }

    public void CreateDirectory(string _str)
    {
        Directory.CreateDirectory(filePath + _str);
    }

    public string[] FileList()
    {   // フォルダ名を取得。
        string[] _strs = Directory.GetFiles(filePath, "*");
        for(int i = 0; i < _strs.Length; i++)
        {
            _strs[i] = Path.GetFileNameWithoutExtension(_strs[i]);
        }
        return _strs;
    }

    // csv
    public bool SaveCSV(string[][] _contens, string _fileName = "temp.csv")
    {
        // if(!IsFile(_fileName)){return false;}
        StreamWriter _steram = new StreamWriter(filePath + _fileName, false, Encoding.GetEncoding("Shift_JIS"));
        for(int i = 0; i < _contens.Length; i++)
        {
            string _str = string.Join(",", _contens[i]);
            _steram.WriteLine(_str);
        }
        _steram.Close();
        return true;
    }

    public bool LoadCSV(ref string[][] _content, string _fileName = "temp.csv")
    {
        if(IsFile(_fileName))
        {
            StreamReader _stream = new StreamReader(filePath + _fileName);
            string[] _dataLines = _stream.ReadToEnd().Replace("\r\n", "\n").Split(new[]{'\n', '\r'});
            _stream.Close();

            _content = new string[_dataLines.Length][];
            for (int i = 0; i < _dataLines.Length; i++)
            {
                _content[i] = _dataLines[i].Split(',');
            }
            return true;
        }
        return false;
    }

    public bool SaveJson<Type>(Type _type, string _fileName)
    {
        string _json = JsonUtility.ToJson(_type);
        StreamWriter _stream = new StreamWriter(filePath + _fileName, false, Encoding.GetEncoding("Shift_JIS"));
        _stream.Write(_json);
        _stream.Flush();
        _stream.Close();
        return true;
    }

    public bool LoadJson<Type>(ref Type _type, string _fileName)
    {
        if(IsFile(_fileName))
        {
            StreamReader _stream = new StreamReader(filePath + _fileName);
            string _data = _stream.ReadToEnd();
            _stream.Close();
            _type = JsonUtility.FromJson<Type>(_data);
            return true;
        }
        return false;
    }
}
