using System;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class RandomMapGenerator : MonoBehaviour
{
    public Tilemap groundTilemap; // 草地和水的 Tilemap
    public Tilemap treeTilemap;   // 树的 Tilemap
    public Tilemap edgeTilemap;   // 边缘的 Tilemap

    public Tile[] groundTile;     // 草地 Tile
    public RuleTile waterRuleTile;// 水 Tile
    public Tile[] treeTiles;      // 树 Tile 数组（四种树）

    public BoardManager boardManager;

    private int mapWidth = 16 + 2; // 地图宽度
    private int mapHeight = 16 + 2; // 地图高度
    private int groupSize = 2; // 每组的大小

    private int minTotlaGroups = 4; // 最小分组数量
    private int maxTotlaGroups = 8; // 最大分组数量

    private byte[,] mapData; // 存储地图数据

    private void Start()
    {
        boardManager = BoardManager.instance;
        GenerateMapData(); // 生成地图数据
        RenderMap();       // 绘制地图
        boardManager.boardController.UpdateCells(mapData);
    }

    // 生成地图数据
    private void GenerateMapData()
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

        int totlaGroups = UnityEngine.Random.Range(minTotlaGroups, maxTotlaGroups); // 总水分组数量
        int waterGroups = UnityEngine.Random.Range(0, totlaGroups); // 总水分组数量
        int treeGroups = totlaGroups - waterGroups; // 树分组数量

        // 用来标记哪些分组已经被占用
        bool[,] occupiedGroups = new bool[totalGroupsX, totalGroupsY];

        // 随机生成水分组
        for (int i = 0; i < waterGroups; i++)
        {
            // 找到一个未被占用的分组
            int groupX, groupY;
            groupX = UnityEngine.Random.Range(1, totalGroupsX); // 从第1组开始
            groupY = UnityEngine.Random.Range(1, totalGroupsY); // 从第1组开始
            if (!occupiedGroups[groupX, groupY])
            {
                // 标记该分组为已占用
                occupiedGroups[groupX, groupY] = true;

                GenerateWaterGroup(groupX * groupSize - 1, groupY * groupSize - 1);
            }
            else
            {
                i--;
            }
        }

        // 随机生成树分组
        for (int i = 0; i < treeGroups; i++)
        {
            // 找到一个未被占用的分组
            int groupX, groupY;
            groupX = UnityEngine.Random.Range(1, totalGroupsX); // 从第1组开始
            groupY = UnityEngine.Random.Range(1, totalGroupsY); // 从第1组开始

            if (!occupiedGroups[groupX, groupY])
            {
                // 标记该分组为已占用
                occupiedGroups[groupX, groupY] = true;

                GenerateTreeGroup(groupX * groupSize - 1, groupY * groupSize - 1);
            }
            else
            {
                i--;
            }
        }
    }

    // 在边缘放置随机树
    private void PlaceEdgeTrees()
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
    private void GenerateWaterGroup(int startX, int startY)
    {
        int waterCount = UnityEngine.Random.Range(2, 5);

        for (int i = 0; i < waterCount; i++)
        {
            // 确保随机生成的 x 和 y 在地图范围内
            int x = UnityEngine.Random.Range(startX, Mathf.Min(startX + groupSize, mapWidth - 1));
            int y = UnityEngine.Random.Range(startY, Mathf.Min(startY + groupSize, mapHeight - 1));

            if (mapData[x, y] == 1)
            {
                i--; // 如果已经是水，则重新生成
            }

            // 只有在合法范围内时才设置
            if (x >= 1 && x < mapWidth - 1 && y >= 1 && y < mapHeight - 1)
            {
                mapData[x, y] = 1; // 水：1
            }
        }
    }

    // 在2*2组内随机生成树
    private void GenerateTreeGroup(int startX, int startY)
    {
        int treeCount = UnityEngine.Random.Range(2, 5);

        for (int i = 0; i < treeCount; i++)
        {
            // 确保随机生成的 x 和 y 在地图范围内
            int x = UnityEngine.Random.Range(startX, Mathf.Min(startX + groupSize, mapWidth - 1));
            int y = UnityEngine.Random.Range(startY, Mathf.Min(startY + groupSize, mapHeight - 1));

            if (mapData[x, y] == 2)
            {
                i--; // 如果已经是树，则重新生成
            }

            // 只有在合法范围内时才设置
            if (x >= 1 && x < mapWidth - 1 && y >= 1 && y < mapHeight - 1)
            {
                mapData[x, y] = 2; // 树：2
            }
        }
    }

    // 渲染地图
    private void RenderMap()
    {
        groundTilemap.ClearAllTiles(); // 清除地形 Tilemap
        treeTilemap.ClearAllTiles();   // 清除树 Tilemap
        edgeTilemap.ClearAllTiles();   // 清除边缘 Tilemap

        // 偏移量，使地图居中
        int offsetX = -mapWidth / 2;
        int offsetY = -mapHeight / 2;

        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                Vector3Int tilePosition = new Vector3Int(x + offsetX, y + offsetY, 0);

                // 绘制草地或水
                if (mapData[x, y] == 0) // 草地
                {
                    Tile groundType = groundTile[UnityEngine.Random.Range(0, groundTile.Length)];
                    groundTilemap.SetTile(tilePosition, groundType);
                }
                else if (mapData[x, y] == 1) // 水
                {
                    groundTilemap.SetTile(tilePosition, waterRuleTile); // 使用 RuleTile 绘制水
                }

                // 绘制树
                if (mapData[x, y] == 2) // 树
                {
                    Tile groundType = groundTile[UnityEngine.Random.Range(0, groundTile.Length)];
                    groundTilemap.SetTile(tilePosition, groundType);
                    Tile treeType = treeTiles[UnityEngine.Random.Range(0, treeTiles.Length)];
                    if (x == 0 || x == mapWidth - 1 || y == 0 || y == mapWidth - 1)
                    {
                        edgeTilemap.SetTile(tilePosition, treeType);
                    }
                    else
                    {
                        treeTilemap.SetTile(tilePosition, treeType);
                    }

                    // 动态调整树的 Sorting Order，使 y 轴靠下的树覆盖靠上的树
                    var tileRenderer = treeTilemap.GetComponent<TilemapRenderer>();
                    tileRenderer.mode = TilemapRenderer.Mode.Individual; // 确保使用 Individual 模式
                    tileRenderer.sortingOrder = -(tilePosition.y);       // 按 y 值排序
                }
            }
        }
    }
}