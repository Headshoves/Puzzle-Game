using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCreator : MonoBehaviour
{
    [SerializeField] private GameObject _hexagonPrefab;

    [SerializeField] private int _mapWidth;
    [SerializeField] private int _mapHeight;

    private void Start() {
        for (int h = 0; h < _mapHeight; h++) {
            for (int w = 0; w < _mapWidth; w++) {
                GameObject temp = Instantiate(_hexagonPrefab, transform);

                float x = temp.GetComponent<MeshFilter>().mesh.bounds.size.x;
                float z = temp.GetComponent<MeshFilter>().mesh.bounds.size.y;

                temp.transform.position = new Vector3((x * w) - (h%2 == 0 ? 0 : x/2), 0, (z * h) - ((z/4) * h));
            }
        }

    }
}
