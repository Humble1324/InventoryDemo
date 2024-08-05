using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using Excel;
using Random = System.Random;

public class Excel2Script : MonoBehaviour
{
    public enum RowType : byte
    {
        FIELD_NAME = 4,
        FIELD_TYPE = 5,
        DATA_START_ROW = 6
    }

    public static readonly string ExcelPath = Application.dataPath + "/Resources/Excel";
    public static readonly string ScriptPath = Application.dataPath + "/Scripts/Database";
    public static readonly string BytePath = Application.dataPath + "/Resources/Byte";

    private static readonly int[] primeNumbers =
        { 17, 19, 23, 29, 31, 37, 41, 43, 47, 53, 59, 61, 67, 71, 73, 79, 83, 89, 97 };

    [MenuItem("Tools/Excel2Script")]
    private static void Excel2ScriptFunc()
    {
        foreach (string filePath in Directory.EnumerateFiles(ExcelPath, "*.xlsx"))
        {
            string[][] data = LoadExcel(filePath);
            CreateScript(filePath, data);
        }

        AssetDatabase.Refresh();
    }

    [MenuItem("Tools/Excel2Byte")]
    private static void Excel2Byte()
    {
        foreach (string filePath in Directory.EnumerateFiles(ExcelPath, "*.xlsx"))
        {
            string[][] data = LoadExcel(filePath);
            CreateByte(filePath, data);
        }

        AssetDatabase.Refresh();
        Debug.Log("Excel转换成二进制文件完成");
    }

    /// <summary>
    /// 把excel存成string类型的二维数组
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    private static string[][] LoadExcel(string filePath)
    {
        var fileInfo = new FileInfo(filePath);
        var stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
        var dataSet = fileInfo.Extension == ".xls"
            ? ExcelReaderFactory.CreateBinaryReader(stream).AsDataSet()
            : ExcelReaderFactory.CreateOpenXmlReader(stream).AsDataSet();


        // 随机选择初始值和乘数

        DataRowCollection rows = dataSet.Tables[0].Rows;
        string[][] data = new string[rows.Count][];
        for (int i = 0; i < rows.Count; ++i)
        {
            int columnCount = rows[i].ItemArray.Length;
            string[] columnArray = new string[columnCount];
            for (int j = 0; j < columnArray.Length; ++j)
            {
                columnArray[j] = rows[i].ItemArray[j].ToString();
            }

            data[i] = columnArray;
        }

        return data;
    }

    /// <summary>
    /// Excel转脚本文件
    /// </summary>
    /// <param name="filePath"></param>
    /// <param name="data"></param>
    private static void CreateScript(string filePath, string[][] data)
    {
        var scriptStr = new StringBuilder();
        var className = new FileInfo(filePath).Name.Split('.')[0];
        Random random = new Random();
        int initialValue = primeNumbers[random.Next(primeNumbers.Length)];
        int multiplier = primeNumbers[random.Next(primeNumbers.Length)];
        scriptStr.AppendLine("using System.Collections.Generic;");
        scriptStr.AppendLine("using Editor;");
        scriptStr.AppendLine($"public class {className}");
        scriptStr.AppendLine("{");
        string[] filedTypeArray = data[(int)RowType.FIELD_TYPE];
        string[] filedNameArray = data[(int)RowType.FIELD_NAME];

        for (int i = 1; i < filedTypeArray.Length; i++)
        {
            scriptStr.AppendLine($"\tpublic {filedTypeArray[i],-10}\t{filedNameArray[i]};");
        }

        scriptStr.AppendLine();
        scriptStr.AppendLine("\tpublic override int GetHashCode()");
        scriptStr.AppendLine("\t{");
        scriptStr.AppendLine("\t\tunchecked");
        scriptStr.AppendLine("\t\t{");
        scriptStr.AppendLine($"\t\t\tint hash = {initialValue}; // 初始值为{initialValue}");
        for (int i = 1; i < filedNameArray.Length; i++)
        {
            string fieldName = filedNameArray[i];
            if (filedTypeArray[i] == "string")
            {
                scriptStr.AppendLine(
                    $"\t\t\thash = hash * {multiplier} + ({fieldName} != null ? {fieldName}.GetHashCode() : 0);");
            }
            else
            {
                scriptStr.AppendLine($"\t\t\thash = hash * {multiplier} + {fieldName}.GetHashCode();");
            }
        }

        scriptStr.AppendLine("\t\t\treturn hash;");
        scriptStr.AppendLine("\t\t}");
        scriptStr.AppendLine("\t}");

        // 生成 Equals 方法
        scriptStr.AppendLine();
        scriptStr.AppendLine("\tpublic override bool Equals(object obj)");
        scriptStr.AppendLine("\t{");
        scriptStr.AppendLine("\t\tif (obj == null || GetType() != obj.GetType())");
        scriptStr.AppendLine("\t\t{");
        scriptStr.AppendLine("\t\t\treturn false;");
        scriptStr.AppendLine("\t\t}");
        scriptStr.AppendLine($"\t\t{className} other = ({className})obj;");
        scriptStr.AppendLine($"\t\treturn name ==other.name &&id==other.id;");
        scriptStr.AppendLine("\t}");
        scriptStr.AppendLine("}");
        // if (obj == null || GetType() != obj.GetType())
        // {
        //     return false;
        // }
        //
        // Item other = (Item)obj;
        // return name == other.name && id == other.id;
        // scriptStr.AppendLine("}");
        scriptStr.AppendLine($"public class {className}Loader : ExcelDataLoaderBase");
        scriptStr.AppendLine("{");
        scriptStr.AppendLine($"\tprotected override string GetFilePath()");
        scriptStr.AppendLine("\t{");
        scriptStr.AppendLine($"\t\treturn nameof({className});");
        scriptStr.AppendLine("\t}");
        scriptStr.AppendLine($"\tpublic static List<{className}> LoadData()");
        scriptStr.AppendLine("\t{");
        scriptStr.AppendLine($"\t\treturn LoadData<{className}>(new {className}Loader().GetFilePath());");
        scriptStr.AppendLine("\t}");
        scriptStr.AppendLine("}");
        string path = $"{ScriptPath}/{className}.cs";
        File.Delete(path);
        File.WriteAllText(path, scriptStr.ToString());
    }

    /// <summary>
    /// 根据excel数据生成二进制文件
    /// </summary>
    /// <param name="filePath"></param>
    /// <param name="data"></param>
    private static void CreateByte(string filePath, string[][] data)
    {
        string className = new FileInfo(filePath).Name.Split('.')[0];
        string path = $"{BytePath}/{className}";
        File.Delete(path);
        using (var fileStream = new FileStream(path, FileMode.Create))
        {
            var types = GetTypeByFieldType(data);
            using (var binaryWriter = new BinaryWriter(fileStream))
            {
                for (int i = (int)RowType.DATA_START_ROW; i < data.Length; i++)
                {
                    for (int j = 0; j < types.Count; j++)
                    {
                        var bytes = GetField(types[j], data[i][j + 1]);
                        binaryWriter.Write(bytes);
                    }
                }
            }
        }
    }

    private static byte[] GetField(Type type, string data)
    {
        if (IsListType(type))
        {
            string[] dataArray = data.Split('|');
            List<byte> byteList = BitConverter.GetBytes(dataArray.Length).ToList();
            for (int i = 0; i < dataArray.Length; i++)
            {
                byteList.AddRange(GetBasicField(type.GenericTypeArguments[0], dataArray[i]).ToList());
            }

            return byteList.ToArray();
        }

        return GetBasicField(type, data);
    }

    private static byte[] GetBasicField(Type type, string data)
    {
        print(type + "---" + data);
        byte[] bytes = null;
        if (type == typeof(int))
        {
            bytes = BitConverter.GetBytes(int.Parse(data));
        }
        else if (type == typeof(float))
        {
            bytes = BitConverter.GetBytes(float.Parse(data));
        }
        else if (type == typeof(string))
        {
            List<byte> dataBytes = Encoding.Default.GetBytes(data).ToList();
            List<byte> lengthBytes = BitConverter.GetBytes(dataBytes.Count).ToList();
            lengthBytes.AddRange(dataBytes);
            bytes = lengthBytes.ToArray();
        }

        if (bytes == null)
            throw new Exception($"{nameof(name)}.GetBasicField: 其类型未配置或不是基础类型 Type:{type} Data:{data}");
        return bytes;
    }

    private static List<Type> GetTypeByFieldType(string[][] data)
    {
        List<Type> types = new List<Type>();
        string[] temp = data[(int)RowType.FIELD_TYPE];
        for (int i = 1; i < temp.Length; ++i)
        {
            if (temp[i] == "int") types.Add(typeof(int));
            else if (temp[i] == "float") types.Add(typeof(float));
            else if (temp[i] == "string") types.Add(typeof(string));
            else if (temp[i] == "List<int>") types.Add(typeof(List<int>));
            else if (temp[i] == "List<float>") types.Add(typeof(List<float>));
        }

        return types;
    }

    /// <summary>
    /// 判断是否合理的List类型
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    private static bool IsListType(Type type)
    {
        if (type == typeof(List<int>)) return true;
        if (type == typeof(List<float>)) return true;
        if (type == typeof(List<string>)) return true;
        return false;
    }
}