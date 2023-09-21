using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace _3DUI.scripts.scoreboard
{
    public class ScoreBoardManager : MonoBehaviour
    {
        public static ScoreBoardManager Instance { get; private set; }

        public List<ScoreBoardEntry> scoreBoard; // functionality from IComparable (Add, Sort) 

        private string savePath;
        
        private void Awake()
        {
            savePath = Application.persistentDataPath + "/scoreboard.csv";
            ReadFromFile();
            Instance = this;
        }

        public void OnDestroy()
        {
            SaveToFile();
        }

        private void ReadFromFile()
        {
            if (File.Exists(savePath))
            {
                scoreBoard = new List<ScoreBoardEntry>();
                string[] lines = File.ReadAllLines(savePath);
                foreach (var line in lines)
                {
                    string[] tokens = line.Split(";");
                    //Check for correct token count: ID, name, time
                    if (tokens.Length == 3)
                    {
                        var id = tokens[0];
                        var name = tokens[1];
                        var time = float.Parse(tokens[2]);
                        scoreBoard.Add(new ScoreBoardEntry(id, name, time));
                    }
                }
                scoreBoard.Sort();
            }
            else
            {
                scoreBoard = new List<ScoreBoardEntry>();
            }
        }

        private void SaveToFile()
        {
            StringBuilder fileContent = new StringBuilder();
            foreach (var entry in scoreBoard)
            {
                fileContent.AppendLine(entry.ToString());
            }
            File.WriteAllText(
                savePath,
                fileContent.ToString().TrimEnd(Environment.NewLine.ToCharArray())
                );
        }
        
        public void AddScoreBoardEntry(ScoreBoardEntry entry)
        {
            scoreBoard.Add(entry);
            scoreBoard.Sort();
        }
    }
}