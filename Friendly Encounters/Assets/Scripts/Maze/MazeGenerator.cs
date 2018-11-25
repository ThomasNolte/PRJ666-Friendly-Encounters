using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : MonoBehaviour {
    public int mazeWidth, mazeHeight;
    public string mazeSeed;

    public Sprite floor, roof, wall, corner;
    public static MazeGenerator instance;

    public delegate void MazeReadyAction();

    public MazeSprite mazeSpritePrefab;
    System.Random mazeRg;
    Maze maze;

    public Vector3 mazeGoalPosition;


    void Awake() {
        instance = this;
    }

	// Use this for initialization
	void Start () {
        mazeRg = new System.Random(mazeSeed.GetHashCode());
        if (mazeWidth % 2 == 0) {
            mazeWidth++;
        }

        if (mazeHeight % 2 == 0) {
            mazeHeight++;
        }
        maze = new Maze(mazeWidth, mazeHeight, mazeRg);
        maze.Generate();

        mazeGoalPosition = maze.GetGoalPosition();
        DrawMaze();

        GetComponent<MazeDirectives>().StartDirectives();
    }
    void DrawMaze() {
        for (int i = 0; i < mazeWidth; i++) {
            for (int j = 0; j < mazeHeight; j++) {
                Vector3 position = new Vector3(i, j);

                if (maze.Grid[i, j] == true) {
                    CreateMazeSprite(position, floor, transform, 0, mazeRg.Next(0, 3) * 90);
                }
                else {
                    CreateMazeSprite(position, roof, transform, 0, 0);

                    DrawWalls(i, j);
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

        if (top) {
            CreateMazeSprite(position, wall, transform, 1, 0);
        }

        if (left) {
            CreateMazeSprite(position, wall, transform, 1, 90);
        }

        if (bottom) {
            CreateMazeSprite(position, wall, transform, 1, 180);
        }

        if (right) {
            CreateMazeSprite(position, wall, transform, 1, 270);
        }

        if (!left && !top && x > 0 && y < mazeHeight - 1) {
            CreateMazeSprite(position, corner, transform, 2, 0);
        }

        if (!left && !bottom && x > 0 && y > 0) {
            CreateMazeSprite(position, corner, transform, 2, 90);
        }

        if (!right && !bottom && x < mazeWidth - 1 && y > 0) {
            CreateMazeSprite(position, corner, transform, 2, 180);
        }

        if (!right && !top && x < mazeWidth - 1 && y < mazeHeight - 1) {
            CreateMazeSprite(position, corner, transform, 2, 270);
        }
    }
    public bool GetMazeGridCell(int x, int y) {
        return maze.GetCell(x, y);
    }

    public List<Vector3> GetRandomFloorPositions(int count) {
        List<Vector3> positions = new List<Vector3>();

        for (int i = 0; i < count; i++) {
            Vector3 position = Vector3.one;

            do {
                int posX = 0;
                int posY = 0;

                while (!GetMazeGridCell(posX, posY)) {
                    posX = mazeRg.Next(5, mazeWidth);
                    posY = mazeRg.Next(5, mazeHeight);
                }

                position = new Vector3(posX, posY);
            }
            while (positions.Contains(position));

            positions.Add(position);
        }

        return positions;
    }

    void CreateMazeSprite(Vector3 position, Sprite sprite, Transform parent, int sortingOrder, float rotation) {
        MazeSprite mazeSprite = Instantiate(mazeSpritePrefab, position, Quaternion.identity) as MazeSprite;
        mazeSprite.setSprite(sprite, sortingOrder);
        mazeSprite.transform.SetParent(parent);
        mazeSprite.transform.Rotate(0, 0, rotation);
    }



}
