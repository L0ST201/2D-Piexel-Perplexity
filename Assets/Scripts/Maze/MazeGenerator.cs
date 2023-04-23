using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : MonoBehaviour {

    // Singleton instance
    public static MazeGenerator instance;

    public int mazeWidth;
    public int mazeHeight;
    public string mazeSeed;

    public Sprite floorSprite;
    public Sprite roofSprite;
    public Sprite wallSprite;
    public Sprite wallCornerSprite;

    public MazeSprite mazeSpritePrefab;

    System.Random mazeRG;

    Maze maze;

    public Vector3 mazeGoalPosition;

    public delegate void MazeReadyAction();
    public static event MazeReadyAction OnMazeReady;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        mazeRG = new System.Random(mazeSeed.GetHashCode());

        if (mazeWidth % 2 == 0)
            mazeWidth++;

        if (mazeHeight % 2 == 0)
        {
            mazeHeight++;
        }

        int startX = 1;
        int startY = 1;

        maze = new Maze(mazeWidth, mazeHeight, startX, startY, mazeRG);
        maze.Generate();

        mazeGoalPosition = maze.GetGoalPosition();

        DrawMaze();

        if(OnMazeReady != null) {
            OnMazeReady();
        }
    }

    void DrawMaze() {
        for(int x = 0; x < mazeWidth; x++) {
            for(int y = 0; y < mazeHeight; y++) {
                Vector3 position = new Vector3(x, y);

                if(maze.Grid[x,y] == true) {
                    CreateMazeSprite(position, floorSprite, transform, 0, mazeRG.Next(0, 3) * 90);
                }else {
                    CreateMazeSprite(position, roofSprite, transform, 0, 0);

                    DrawWalls(x, y);
                }
            }
        }
    }

    void DrawWalls(int x, int y) {
        bool top = GetMazeGridCell(x, y + 1);
        bool bottom = GetMazeGridCell(x, y - 1);
        bool right = GetMazeGridCell(x + 1, y);
        bool left = GetMazeGridCell(x - 1, y);

        Vector3 position = new Vector3(x, y);

        if(top) {
            CreateMazeSprite(position, wallSprite, transform, 1, 0);
        }

        if(left) {
            CreateMazeSprite(position, wallSprite, transform, 1, 90);
        }

        if(bottom) {
            CreateMazeSprite(position, wallSprite, transform, 1, 180);
        }

        if(right) {
            CreateMazeSprite(position, wallSprite, transform, 1, 270);
        }

        if(!left && !top && x > 0 && y < mazeHeight - 1) {
            CreateMazeSprite(position, wallCornerSprite, transform, 2, 0);
        }

        if(!left && !bottom && x > 0 && y > 0) {
            CreateMazeSprite(position, wallCornerSprite, transform, 2, 90);
        }

        if(!right && !bottom && x < mazeWidth - 1 && y > 0) {
            CreateMazeSprite(position, wallCornerSprite, transform, 2, 180);
        }

        if(!right && !top && x < mazeWidth - 1 && y < mazeHeight - 1) {
            CreateMazeSprite(position, wallCornerSprite, transform, 2, 270);
        }
    }

    public bool GetMazeGridCell(int x, int y) {
        if(x >= mazeWidth || x < 0 || y >= mazeHeight || y <= 0) {
            return false;
        }

        return maze.Grid[x, y];
    }

     public List<Vector3> GetRandomFloorPositions(int count) {
        List<Vector3> positions = new List<Vector3>();

        for(int i = 0; i < count; i++) {
            Vector3 position = Vector3.one;

            do {
                int posX = 0;
                int posY = 0;

                while(!GetMazeGridCell(posX, posY)) {
                    posX = mazeRG.Next(3, mazeWidth);
                    posY = mazeRG.Next(3, mazeHeight);
                }

                position = new Vector3(posX, posY);
            } while(positions.Contains(position));

            positions.Add(position);
        }

        return positions;
    }

    void CreateMazeSprite(Vector3 position, Sprite sprite, Transform parent, int sortingOrder, float rotation) {
        MazeSprite mazeSprite = Instantiate(mazeSpritePrefab, position, Quaternion.identity) as MazeSprite;
        mazeSprite.SetSprite(sprite, sortingOrder);
        mazeSprite.transform.SetParent(parent);
        mazeSprite.transform.Rotate(0, 0, rotation);
    }

    public IEnumerator GenerateNewMaze()
    {
        // Destroy the existing maze first
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        // Then generate a new maze
        maze = new Maze(mazeWidth, mazeHeight, 1, 1, mazeRG);
        maze.Generate();
        mazeGoalPosition = maze.GetGoalPosition();
        DrawMaze();

        if (OnMazeReady != null)
        {
            OnMazeReady();
        }

        yield return null;
    }


}