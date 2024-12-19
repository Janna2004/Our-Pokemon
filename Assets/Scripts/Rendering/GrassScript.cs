using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RandomMapGenerator : MonoBehaviour
{
    public Tilemap tilemap; // 需要绑定的 Tilemap
    public Tile groundTile; // 地面 Tile
    public Tile waterTile; // 水 Tile

    private int mapWidth = 24; // 地图宽
    private int mapHeight = 24; // 地图高

    private byte[,] mapData; // 存储地图数据

    void Start()
    {
        GenerateMapData(); // 生成地图数据
        RenderMap(); // 根据数据绘制地图
    }

    // 生成地图数据
    void GenerateMapData()
    {
        mapData = new byte[mapWidth, mapHeight];

        // 初始化地图为全地面
        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                mapData[x, y] = 0; // 0 表示地面
            }
        }

        // 随机生成水块
        int waterCount = 30; // 水块数量
        for (int i = 0; i < waterCount; i++)
        {
            int randomX = UnityEngine.Random.Range(0, mapWidth);
            int randomY = UnityEngine.Random.Range(0, mapHeight);

            mapData[randomX, randomY] = 1; // 1 表示水
        }
    }

    // 渲染地图
    void RenderMap()
    {
        tilemap.ClearAllTiles(); // 清除 Tilemap 中的现有 Tile

        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                // 根据 mapData 的值决定放置哪种 Tile
                if (mapData[x, y] == 0)
                {
                    tilemap.SetTile(new Vector3Int(x, y, 0), groundTile); // 地面
                }
                else if (mapData[x, y] == 1)
                {
                    tilemap.SetTile(new Vector3Int(x, y, 0), waterTile); // 水
                }
            }
        }
    }
}