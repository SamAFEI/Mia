using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Script.Save_and_Load
{
    public class FileDataHandler
    {
        private string dataDirPath = "";
        private string dataFileName = "";

        public FileDataHandler(string _dataDirPath,string _dataFileName) 
        {
            this.dataDirPath = _dataDirPath;
            this.dataFileName = _dataFileName;
        }

        public void Save(GameData _data)
        {
            string fullPath = Path.Combine(dataDirPath, dataFileName);

            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
                string dataToStore = JsonUtility.ToJson(_data, true);
                using(FileStream stream = new FileStream(fullPath, FileMode.Create))
                {
                    using(StreamWriter writer = new StreamWriter(stream))
                    {
                        writer.Write(dataToStore);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError(fullPath + "\n" + e);
            }
        }

        public GameData Load()
        {
            string fullPath = Path.Combine(dataDirPath , dataFileName);
            GameData loadData = null;

            if (File.Exists(fullPath))
            {
                try
                {
                    string dataToLoad = "";
                    using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                    {
                        using(StreamReader reader = new StreamReader(stream))
                        {
                            dataToLoad = reader.ReadToEnd();
                        }
                    }
                    loadData = JsonUtility.FromJson<GameData>(dataToLoad);
                }
                catch(Exception e)
                {
                    Debug.LogError(e);
                }
            }
            return loadData;
        }

        public void Delete()
        {
            string fullPath = Path.Combine(dataDirPath, dataFileName); ;
            if (File.Exists(fullPath))
            { File.Delete(fullPath); }
        }
    }
}
