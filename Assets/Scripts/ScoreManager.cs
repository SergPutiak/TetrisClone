using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour {

    int m_score;
    int m_linesTolevelUp;
    public int Level { get; set; }

    [SerializeField] int m_linesPerLevel = 30;
    [SerializeField] Text m_linesToLevelUpText;
    [SerializeField] Text m_levelText;
    [SerializeField] Text m_scoreText;


	// Use this for initialization
	void Start () {
        Reset();
        UpdateUIText();
	}
	
    public void ScoreLines(int n) {
        switch (n) {
            case 0:
                m_score += 5 * Level;
                break;
            case 1:
                m_score += 20 * Level;
                break;
            case 2:
                m_score += 50 * Level;
                break;
            case 3:
                m_score += 125 * Level;
                break;
            case 4:
                m_score += 400 * Level;
                break;
        }
        m_linesTolevelUp -= n;
        if (m_linesTolevelUp <= 0) {
            LevelUp();
        }
        UpdateUIText();
    }

    public void Reset() {
        Level = 1;
        m_linesTolevelUp = m_linesPerLevel * Level;
    }

    void UpdateUIText() {
        if (m_linesToLevelUpText) {
            m_linesToLevelUpText.text = m_linesTolevelUp.ToString();
        }
        if (m_scoreText) {
            m_scoreText.text = m_score.ToString();
        }
        if (m_levelText) {
            m_levelText.text = Level.ToString();
        }
    }

    public void LevelUp() {
        Level++;
        m_linesTolevelUp = m_linesPerLevel + m_linesTolevelUp;
    }
}
