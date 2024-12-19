using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RandomMapGenerator : MonoBehaviour
{
    public Tilemap tilemap; // Tilemap 组件
    public Tile groundTile; // 草地 Tile
    public Tile waterTile; // 水 Tile
    public Tile[] treeTiles; // 树 Tile 数组（四种树）
    public Tile flowerTile; // 小花 Tile

    private int mapWidth = 16 + 2; // 地图宽度
    private int mapHeight = 16 + 2; // 地图高度
    private int groupSize = 2; // 每组的大小

    private byte[,] mapData; // 存储地图数据

    void Start()
    {
        GenerateMapData(); // 生成地图数据
        RenderMap(); // 绘制地图
    }

    // 生成地图数据
    void GenerateMapData()
    {
        mapData = new byte[mapWidth, mapHeight];

        // 初始化地图为全草地
        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                mapData[x, y] = 0; // 草地：0
            }
        }

        // 强制在边缘放置随机树
        PlaceEdgeTrees();

        // 减少水分组和树分组的总数量，同时空出最外圈
        int totalGroupsX = (mapWidth - 2) / groupSize; // 不包括边缘
        int totalGroupsY = (mapHeight - 2) / groupSize;

        int waterGroups = UnityEngine.Random.Range(1, 3); // 总水分组数量
        int treeGroups = UnityEngine.Random.Range(1, 3); // 总树分组数量

        // 随机生成水分组
        for (int i = 0; i < waterGroups; i++)
        {
            int groupX = UnityEngine.Random.Range(1, totalGroupsX) * groupSize; // 从第1组开始
            int groupY = UnityEngine.Random.Range(1, totalGroupsY) * groupSize; // 从第1组开始
            GenerateWaterGroup(groupX, groupY);
        }

        // 随机生成树分组
        for (int i = 0; i < treeGroups; i++)
        {
            int groupX = UnityEngine.Random.Range(1, totalGroupsX) * groupSize; // 从第1组开始
            int groupY = UnityEngine.Random.Range(1, totalGroupsY) * groupSize; // 从第1组开始
            GenerateTreeGroup(groupX, groupY);
        }
    }

    // 在边缘放置随机树
    void PlaceEdgeTrees()
    {
        for (int x = 0; x < mapWidth; x++)
        {
            // 上边缘
            mapData[x, mapHeight - 1] = 2; // 树：2
            // 下边缘
            mapData[x, 0] = 2;
        }

        for (int y = 0; y < mapHeight; y++)
        {
            // 左边缘
            mapData[0, y] = 2;
            // 右边缘
            mapData[mapWidth - 1, y] = 2;
        }
    }

    // 在2*2组内随机生成水
    void GenerateWaterGroup(int startX, int startY)
    {
        int waterCount = UnityEngine.Random.Range(2, 4);

        for (int i = 0; i < waterCount; i++)
        {
            // 确保随机生成的 x 和 y 在地图范围内
            int x = UnityEngine.Random.Range(startX, Mathf.Min(startX + groupSize, mapWidth - 1));
            int y = UnityEngine.Random.Range(startY, Mathf.Min(startY + groupSize, mapHeight - 1));

            // 只有在合法范围内时才设置
            if (x >= 1 && x < mapWidth - 1 && y >= 1 && y < mapHeight - 1)
            {
                mapData[x, y] = 1; // 水：1
            }
        }
    }

    // 在2*2组内随机生成树
    void GenerateTreeGroup(int startX, int startY)
    {
        int treeCount = UnityEngine.Random.Range(2, 4);

        for (int i = 0; i < treeCount; i++)
        {
            // 确保随机生成的 x 和 y 在地图范围内
            int x = UnityEngine.Random.Range(startX, Mathf.Min(startX + groupSize, mapWidth - 1));
            int y = UnityEngine.Random.Range(startY, Mathf.Min(startY + groupSize, mapHeight - 1));

            // 只有在合法范围内时才设置
            if (x >= 1 && x < mapWidth - 1 && y >= 1 && y < mapHeight - 1)
            {
                mapData[x, y] = 2; // 树：2
            }
        }
    }

    // 渲染地图
    void RenderMap()
    {
        tilemap.ClearAllTiles(); // 清除 Tilemap 中的现有 Tile

        // 偏移量，使地图居中
        int offsetX = -mapWidth / 2;
        int offsetY = -mapHeight / 2;

        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                Vector3Int tilePosition = new Vector3Int(x + offsetX, y + offsetY, 0);

                // 绘制草地
                tilemap.SetTile(tilePosition, groundTile);

                if (mapData[x, y] == 1) // 水
                {
                    tilemap.SetTile(tilePosition, waterTile);
                }
                else if (mapData[x, y] == 2) // 树
                {
                    Tile treeType = treeTiles[UnityEngine.Random.Range(0, treeTiles.Length)];
                    tilemap.SetTile(tilePosition, treeType);
                }
                else if (mapData[x, y] == 0) // 草地区域可能出现小花
                {
                    if (UnityEngine.Random.Range(0, 10) < 2) // 20% 概率生成小花
                    {
                        tilemap.SetTile(tilePosition, flowerTile);
                    }
                }
            }
        }
    }
}