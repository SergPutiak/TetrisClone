using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    [SerializeField] Tetrimino[] m_allTetriminos;
    [SerializeField] Transform m_nextTetriminoSpawnPoint;

    Tetrimino m_nextTetrimino;

    private void Awake() {
        NextTetrimino();
    }

    void NextTetrimino() {
        int i = Random.Range(0, m_allTetriminos.Length);
        if (m_allTetriminos[i]) {
            m_nextTetrimino = Instantiate(m_allTetriminos[i], m_nextTetriminoSpawnPoint.position, Quaternion.identity, m_nextTetriminoSpawnPoint);
        } else {
            Debug.Log("Invalid Tetrimino!");
        }
    }

    public Tetrimino SpawnShape() {
        Tetrimino tetrimino = m_nextTetrimino;
        tetrimino.transform.position = this.transform.position;
        tetrimino.transform.SetParent(this.transform);
        NextTetrimino();
        if (tetrimino) {
            return tetrimino;
        } else {
            Debug.Log("Invalid Tetrimino!");
            return null;
        }
    }
}

