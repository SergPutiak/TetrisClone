using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tetrimino : MonoBehaviour {

    [SerializeField] bool m_canRotate = true;

    public void MoveLeft() {
        transform.Translate(Vector3.left, Space.World);
    }

    public void MoveRight() {
        transform.Translate(Vector3.right, Space.World);
    }

    public void MoveDown() {
        transform.Translate(Vector3.down, Space.World);
    }

    public void MoveUp() {
        transform.Translate(Vector3.up, Space.World);
    }

    public void RotateRight() {
        if (m_canRotate) {
            transform.Rotate(0, 0, -90);
        }
    }

    public void RotateLeft() {
        if (m_canRotate) {
            transform.Rotate(0, 0, 90);
        }
    }

}
