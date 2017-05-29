using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    [SerializeField] GameObject m_gameOverPanel;
    [SerializeField] GameObject m_pausePanel;

    Board m_gameBoard;
    Spawner m_spawner;
    ScoreManager m_scoreManager;
    Tetrimino m_activeShape;
    float m_dropInterval = .3f;
    float m_dropIntervalModded;
    float m_timeToDrop;
    bool m_gameOver;

    public bool IsPaused { get; set; }

    // Use this for initialization
    void Start () {
        m_gameBoard = FindObjectOfType<Board>();
        m_spawner = FindObjectOfType<Spawner>();
        m_scoreManager = FindObjectOfType<ScoreManager>();

        if (!m_gameBoard) {
            Debug.Log("WARNING! There is no game board defined!");
        }

        if (!m_scoreManager) {
            Debug.Log("WARNING! There is no scoreManager defined!");
        }

        if (!m_spawner) {
            Debug.Log("WARNING! There is no spawner defined!");
        } else {
            m_spawner.transform.position = Vectorf.Round(m_spawner.transform.position);
            if (m_activeShape == null) {
                m_activeShape = m_spawner.SpawnShape();
            }
        }

        if (m_gameOverPanel) {
            m_gameOverPanel.SetActive(false);
        }

        if (m_pausePanel) {
            m_pausePanel.SetActive(false);
        }

        m_dropIntervalModded = m_dropInterval;

    }

    // Update is called once per frame
    void Update () {
        CheckInput();
    }


    void CheckInput() {
        if (Input.GetKeyDown(KeyCode.Keypad1)) {
            m_activeShape.MoveLeft();
            if (!m_gameBoard.IsValidPosition(m_activeShape)) {
                m_activeShape.MoveRight();
            }
        } else if (Input.GetKeyDown(KeyCode.Keypad2)) {
            m_activeShape.MoveRight();
            if (!m_gameBoard.IsValidPosition(m_activeShape)) {
                m_activeShape.MoveLeft();
            }
        } else if (Input.GetKey(KeyCode.Keypad3)) {
            m_activeShape.MoveDown();
            if (!m_gameBoard.IsValidPosition(m_activeShape)) {
                m_activeShape.MoveUp();
            }
        } else if (Input.GetKeyDown(KeyCode.Keypad4)) {
            m_activeShape.RotateLeft();
            if (!m_gameBoard.IsValidPosition(m_activeShape)) {
                m_activeShape.RotateRight();
            } 
        } else if (Input.GetKeyDown(KeyCode.Keypad5)) {
            m_activeShape.RotateRight();
            if (!m_gameBoard.IsValidPosition(m_activeShape)) {
                m_activeShape.RotateLeft();
            } 
        } else if(Input.GetKey(KeyCode.Keypad3) || Time.time > m_timeToDrop) {
            m_timeToDrop = Time.time + m_dropIntervalModded;
            m_activeShape.MoveDown();
            if (!m_gameBoard.IsValidPosition(m_activeShape)) {
                if (m_gameBoard.IsOverTheLimit(m_activeShape)) {
                    GameOver();
                } else {
                    LandShape();
                }
            }
        }
    }


    private void LandShape() {
        int curLevel = m_scoreManager.Level;
        m_activeShape.MoveUp();
        m_gameBoard.StoreShapeInGrid(m_activeShape);
        if (m_spawner) {
            m_activeShape = m_spawner.SpawnShape();
        }
        m_gameBoard.ClearAllRows();

        if (m_gameBoard.CompletedRows > 0) {
            m_scoreManager.ScoreLines(m_gameBoard.CompletedRows);
        }
        if (m_scoreManager.Level > curLevel) {
            m_dropIntervalModded = Mathf.Clamp(m_dropInterval - (((float)m_scoreManager.Level - 1) * 0.05f), 0.05f,1f);
        }
    }

    private void GameOver() {
        m_activeShape.MoveUp();
        m_gameOver = true;
        m_gameOverPanel.SetActive(true);
    }

    public void Restart() {
        IsPaused = false;
        m_scoreManager.Reset();
        UpdatePauseState();
        foreach(Transform child in m_spawner.transform) {
            Destroy(child.gameObject);
        }
        m_activeShape = m_spawner.SpawnShape();
        m_gameOver = false;
        m_gameOverPanel.SetActive(false);
    }

    public void TogglePause() {
        if (m_gameOver) {
            return;
        }
        IsPaused = !IsPaused;
        UpdatePauseState();
    }

    private void UpdatePauseState() {
        if (m_pausePanel) {
            m_pausePanel.SetActive(IsPaused);
        }
        Time.timeScale = (IsPaused) ? 0 : 1;
    }
}
