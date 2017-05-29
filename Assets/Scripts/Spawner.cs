using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    [SerializeField] Tetrimino[] m_allTetriminos;

    Tetrimino GetRandomShape() {
        int i = Random.Range(0, m_allTetriminos.Length);
        if (m_allTetriminos[i]) {
            return m_allTetriminos[i];
        } else {
            Debug.Log("Invalid Tetrimino!");
            return null;
        }
    }

    public Tetrimino SpawnShape() {
        Tetrimino tetrimino = Instantiate(GetRandomShape(), transform.position, Quaternion.identity, transform);
        if (tetrimino) {
            return tetrimino;
        } else {
            Debug.Log("Invalid Tetrimino!");
            return null;
        }
    }
}

