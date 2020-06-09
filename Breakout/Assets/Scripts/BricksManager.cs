using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

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
    private GameObject bricksContainer;
    private float initialBrickSpawnPositionX = -1.96f;  // Get the intial spawn position for x   
    private float initialBrickSpawnPositionY = 3.325f;  // Get the intial spawn position for y
    private float shiftAmount = 0.365f;

    public Brick brickPrefab;

    public Sprite[] Sprites;

    public Color[] BrickColours;

    public List<Brick> RemainingBricks { get; set; }

    public List<int[,]> LevelsData { get; set; }

    public int InitialBricksCount { get; set; }

    public int currentLevel;

    private void Start()
    {
        this.bricksContainer = new GameObject("BricksContainer");
        this.RemainingBricks = new List<Brick>();
        this.LevelsData = this.LoadLevelsData();
        this.GenerateBricks();
    }

    private void GenerateBricks()
    {
        // Get the current level
        int[,] currentLevelData = this.LevelsData[this.currentLevel];
        // Iterate over the level and instantiate bricks 
        float currentSpawnX = initialBrickSpawnPositionX;   // Set the current spawn position for x 
        float currentSpawnY = initialBrickSpawnPositionY;   // Set the current spawn position for y
        float zShift = 0;

        // Iterate over the rows
        for(int row = 0; row < this.maxRows; row++)
        {
            // Iterate over the columns
            for(int col = 0; col < this.maxCols; col++)
            {
                int brickType = currentLevelData[row, col];

                if(brickType > 0)
                {
                    // Instantiate the brick
                    Brick newBrick = Instantiate(brickPrefab, new Vector3(currentSpawnX, currentSpawnY, 0.0f - zShift), Quaternion.identity) as Brick;
                    newBrick.Init(bricksContainer.transform, this.Sprites[brickType - 1], this.BrickColours[brickType], brickType);
                    // Add remaining bricks collection
                    this.RemainingBricks.Add(newBrick);
                    zShift += 0.0001f;
                }

                // Shift the brick x position for the next iteration
                currentSpawnX += shiftAmount;
                // Check if we have reached the end of the columns
                if(col + 1 == this.maxCols)
                {
                    // Reset the current spawn position for x
                    currentSpawnX = initialBrickSpawnPositionX;
                }
            }

            // Shift the brick y position for the next iteration
            currentSpawnY -= shiftAmount;
        }

        this.InitialBricksCount = this.RemainingBricks.Count;
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
