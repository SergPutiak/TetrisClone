using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour {

    [SerializeField] Transform m_emptySprite;
    [SerializeField] int m_height = 25;
    [SerializeField] int m_width = 10;
    [SerializeField] int m_topSpacing = 5;

    Transform[,] m_grid;

    public int CompletedRows { get; set; }

    void Awake() {
        m_grid = new Transform[m_width, m_height];    
    }

    // Use this for initialization
    void Start () {
        DrawEmptyCells();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void DrawEmptyCells() {
        if (m_emptySprite) {
            for (int y = 0; y < m_height - m_topSpacing; y++) {
                for(int x = 0; x < m_width; x++) {
                    Transform clone = Instantiate(m_emptySprite, new Vector3(x, y, 0), Quaternion.identity);
                    clone.name = string.Format("Board (x = {0}, y = {1})", x, y);
                    clone.transform.parent = transform;
                }
            }
        } else {
            Debug.Log("Assign the emptySprite!");
        }
    }

    bool IsOnBoard(int x, int y) {
        return (x >= 0 && x < m_width && y >= 0);
    }

    public bool IsValidPosition(Tetrimino tetrimino) {
        foreach(Transform child in tetrimino.transform) {
            Vector2 position = Vectorf.Round(child.position);
            if(!IsOnBoard((int)position.x, (int)position.y)) {
                return false;
            }

            if (IsOccupied((int)position.x, (int)position.y, tetrimino)) {
                return false;
            }
        }
        return true;
    }

    bool IsOccupied(int x, int y, Tetrimino tetrimino) {
        return (m_grid[x, y] != null && m_grid[x, y].parent != tetrimino.transform);
    }

    public void StoreShapeInGrid(Tetrimino tetrimino) {
        if(tetrimino == null) {
            return;
        }

        foreach (Transform child in tetrimino.transform) {
            Vector2 position = Vectorf.Round(child.position);
            m_grid[(int)position.x, (int)position.y] = child;
        }
    }

    bool IsComplete(int y) {
        for(int x = 0; x < m_width; x++) {
            if(m_grid[x,y] == null) {
                return false;
            }
        }
        return true;
    }

    void ClearRow(int y) {
        for(int x = 0; x < m_width; x++) {
            if (m_grid[x, y] != null) {
                Destroy(m_grid[x, y].gameObject);
            }
            m_grid[x, y] = null;
        }
    }

    void ShiftOneRowDown(int y) {
        for(int x = 0; x < m_width; x++) {
            if (m_grid[x, y] != null) {
                m_grid[x, y - 1] = m_grid[x, y];
                m_grid[x, y] = null;
                m_grid[x, y - 1].position += new Vector3(0, -1, 0);
            }
            
        }
    }

    void ShiftAllRowsDown(int startY) {
        for(int i =startY; i< m_height; i++) {
            ShiftOneRowDown(i);
        }
    }

    public void ClearAllRows() {
        CompletedRows = 0;
        
        for(int y =0; y < m_height; y++) {
            if (IsComplete(y)) {
                CompletedRows++;
                ClearRow(y);
                ShiftAllRowsDown(y + 1);
                y--;
            }
        }
    }

    public bool IsOverTheLimit(Tetrimino shape) {
        foreach(Transform child in shape.transform) {
            if(child.transform.position.y>=(m_height - m_topSpacing - 1)) {
                return true;
            }
        }
        return false;
    }

}
