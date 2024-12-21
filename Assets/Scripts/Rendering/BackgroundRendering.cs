using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Rendering;

public class NewBehaviourScript : MonoBehaviour
{
    public Material grassMaterial; // 用于指定绿色材质
    private CommandBuffer commandBuffer;
    private Mesh quadMesh;

    void Start()
    {
        // 检查材质是否正确引用
        if (grassMaterial == null)
        {
            UnityEngine.Debug.LogError("Grass Material is null!");
            return;
        }
        else
        {
            UnityEngine.Debug.Log("Grass Material is successfully referenced!");
        }

        // 创建一个命令缓冲区
        commandBuffer = new CommandBuffer { name = "Draw Fullscreen Grass" };

        // 创建一个四边形网格
        quadMesh = new Mesh();
        quadMesh.vertices = new Vector3[]
        {
            new Vector3(-1, -1, 0), // 左下角
            new Vector3(1, -1, 0),  // 右下角
            new Vector3(1, 1, 0),   // 右上角
            new Vector3(-1, 1, 0)   // 左上角
        };

        quadMesh.uv = new Vector2[]
        {
            new Vector2(0, 0), // 左下角 UV
            new Vector2(1, 0), // 右下角 UV
            new Vector2(1, 1), // 右上角 UV
            new Vector2(0, 1)  // 左上角 UV
        };

        quadMesh.triangles = new int[]
        {
            0, 1, 2, // 第一个三角形
            0, 2, 3  // 第二个三角形
        };

        // 设置命令缓冲区的渲染目标和绘制指令
        commandBuffer.SetRenderTarget(BuiltinRenderTextureType.CameraTarget);
        commandBuffer.DrawMesh(quadMesh, Matrix4x4.identity, grassMaterial);

        // 获取摄像机组件并添加命令缓冲区
        Camera camera = GetComponent<Camera>();
        if (camera == null)
        {
            UnityEngine.Debug.LogError("Camera component not found!");
            return;
        }
        else
        {
            UnityEngine.Debug.Log("Camera component is successfully referenced!");
        }

        camera.AddCommandBuffer(CameraEvent.AfterEverything, commandBuffer);
    }

    void OnDisable()
    {
        UnityEngine.Debug.Log("移除 CommandBuffer");
        // 从相机移除 CommandBuffer
        Camera camera = GetComponent<Camera>();
        if (camera != null)
        {
            camera.RemoveCommandBuffer(CameraEvent.AfterEverything, commandBuffer);
        }

        // 清理资源
        if (commandBuffer != null)
        {
            commandBuffer.Dispose();
        }
    }
}