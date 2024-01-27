using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSpawner : MonoBehaviour
{
    [SerializeField]
    protected List<GameObject> m_ToSpawn;

    [SerializeField]
    protected Vector2Int m_Size;

    [SerializeField]
    protected float m_TileSize;

    [SerializeField]
    protected float m_MinTime;

    [SerializeField]
    protected float m_MaxTime;

    // Start is called before the first frame update
    void Start()
    {
        float maxDistance = new Vector3((m_Size.x-1) / 2 * m_TileSize * (1f / 1.15f), 0, (m_Size.y-1) / 2 * m_TileSize).magnitude;

        for (int x = -m_Size.x / 2; x <= m_Size.x/2; x++)
        {
            int howMany = m_Size.y - Mathf.Abs(x);

            for (int z = 0; z < howMany; z++)
            {
                float zOffset = Mathf.Abs((x % 2f) / 2f);

                Vector3 position = new Vector3(x * m_TileSize * (1f/1.15f), 0, (z - (howMany - 1) / 2 - zOffset) * m_TileSize);

                int toSpawn = Random.Range(0, m_ToSpawn.Count);

                GameObject newTile = Instantiate(m_ToSpawn[toSpawn], position, Quaternion.identity);

                newTile.transform.localScale = Vector3.one * 100f * m_TileSize;

                float timeToDrop = Mathf.Lerp(m_MaxTime, m_MinTime, position.magnitude / maxDistance);

                Tile tile = newTile.GetComponent<Tile>();

                if(!tile)
                    tile = newTile.AddComponent<Tile>();

                tile.Drop(timeToDrop);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
