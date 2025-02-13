﻿namespace forms.Models.Interfaces
{
    public interface IBaseRepository
    {
        //MESSAGE PACK
        public void CreateMessagePackFile(string filepath);
        public byte[] ReadMessagePackFile(string filepath);
        public T ReadAndConvertMessagepackFileToObject<T>(string filePath);
        public void UpdateMessagePackFile(string filepath, object obj);
        public void CreateAndUpdateMessagePackFile(string filepath, object obj);
        public void DeleteMessagePackFile();
        //TEXT FILE
        public void CreateTextFile(string filepath);
        public string[] ReadTextFile(string filepath);
        public void AppendTextFile(string filepath, string text);
        public void UpdateTextFile(string filepath, string text);
        public void DeleteTextFile();
        public string GetFilePath(string Xpath);
    }
}
