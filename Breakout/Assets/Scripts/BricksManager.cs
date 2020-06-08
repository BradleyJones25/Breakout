using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BricksManager : MonoBehaviour
{
    #region Singleton

    private static BricksManager _instance;

    public static BricksManager Instance => _instance;

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    #endregion

    private int maxRows = 17;
    private int maxCols = 12;

    public Sprite[] Sprites;

    public List<int[,]> LevelsData { get; set; }

    private void Start()
    {
        this.LevelsData = this.LoadLevelsData();
    }

    private List<int[,]> LoadLevelsData()
    {
        // Get data from the text file
        TextAsset text = Resources.Load("levels") as TextAsset;
        
        // Split the data into rows
        string[] rows = text.text.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
        
        // Create a temporary variable to store the levels data
        List<int[,]> levelsData = new List<int[,]>();
        int[,] currentLevel = new int[maxRows, maxCols];
        int currentRow = 0;

        // Iterate every row
        for(int row = 0; row < rows.Length; row++)
        {
            string line = rows[row];

            // Check if the line contains a dash '--'
            if(line.IndexOf("--") == -1)
            {
                // Split the line by a comma ',' - this will give a collection of all the bricks in this line
                string[] bricks = line.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                // Iterate every column
                for(int col = 0; col < bricks.Length; col++)
                {
                    // Parse the value and assign to current level
                    currentLevel[currentRow, col] = int.Parse(bricks[col]);
                }

                currentRow++;
            }
            else
            {
                // End of current level
                // Add the maxtrix to the last and continue the loop
                currentRow = 0;                           // Reset the current row
                levelsData.Add(currentLevel);             // Add the current level to the whole levels data
                currentLevel = new int[maxRows, maxCols]; // Reset the current level to be empty
            }
        }

        return levelsData;
    }
}
