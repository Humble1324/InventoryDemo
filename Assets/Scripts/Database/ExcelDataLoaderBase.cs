using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace Editor
{
    public abstract class ExcelDataLoaderBase
    {
        public static readonly string ByteBasePath = Application.dataPath + "/Resources/Byte";
        protected string FilePath { get; private set; }
        

        protected abstract string GetFilePath();
        /// <summary>
        /// 二进制文件读取
        /// </summary>
        /// <param name="fileName"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<T> LoadData<T>(string fileName) where T : new()
        {
            string path = $"{ByteBasePath}/{fileName}";
            if (!File.Exists(path))
            {
                Debug.LogError($"File not found: {path}");
                return new List<T>(); // 返回空的列表
            }

            var result = new List<T>();

            using (var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
            using (var binaryReader = new BinaryReader(fileStream))
            {
                while (binaryReader.BaseStream.Position < binaryReader.BaseStream.Length)
                {
                    T obj = new T();
                    var objType = obj.GetType();
                    var fields = objType.GetFields();

                    foreach (var field in fields)
                    {
                        var fieldType = field.FieldType;
                        object value = ReadField(binaryReader, fieldType);
                        field.SetValue(obj, value);
                    }

                    result.Add(obj);
                }
            }

            return result;
        }


        private static object ReadField(BinaryReader reader, Type type)
        {
            if (type == typeof(int))
            {
                int intValue = reader.ReadInt32();

                return intValue;
            }
            else if (type == typeof(float))
            {
                float floatValue = reader.ReadSingle();

                return floatValue;
            }
            else if (type == typeof(string))
            {
                int length = reader.ReadInt32();
                if (length < 0)
                {
                    throw new ArgumentOutOfRangeException("length", "Non-negative number required.");
                }

                byte[] bytes = reader.ReadBytes(length);
                string stringValue = Encoding.Default.GetString(bytes);

                return stringValue;
            }
            else if (IsListType(type))
            {
                int length = reader.ReadInt32();
                if (length < 0)
                {
                    throw new ArgumentOutOfRangeException("length", "Non-negative number required.");
                }

                Debug.Log($"List length: {length}");
                Type itemType = type.GetGenericArguments()[0];
                var list = (IList)Activator.CreateInstance(type);

                for (int i = 0; i < length; i++)
                {
                    list.Add(ReadField(reader, itemType));
                }

                return list;
            }

            throw new Exception($"Unsupported field type: {type}");
        }

        private static bool IsListType(Type type)
        {
            return type.IsGenericType && (type.GetGenericTypeDefinition() == typeof(List<>));
        }
    }
}