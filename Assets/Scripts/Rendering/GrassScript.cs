using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class NewBehaviourScript : MonoBehaviour
{
    public Material grassMaterial; // 公共变量，用于在编辑器中指定绿色材质
    private CommandBuffer commandBuffer;

    void Start()
    {
        if (grassMaterial == null)
        {
            UnityEngine.Debug.LogError("Grass Material is null!");
            return;
        }

        // 创建一个新的 CommandBuffer
        commandBuffer = new CommandBuffer { name = "Draw Fullscreen grass" };

        // 创建一个绘制全屏四边形的命令并应用绿色材质
        commandBuffer.DrawProcedural(Matrix4x4.identity, grassMaterial, 0, MeshTopology.Triangles, 6);

        // 获取摄像机组件并添加 CommandBuffer
        Camera camera = GetComponent<Camera>();
        if (camera == null)
        {
            UnityEngine.Debug.LogError("Camera component not found!");
            return;
        }

        camera.AddCommandBuffer(CameraEvent.BeforeForwardOpaque, commandBuffer);
    }

    void OnDisable()
    {
        // 清理，从相机移除 CommandBuffer
        Camera camera = GetComponent<Camera>();
        if (camera != null)
        {
            camera.RemoveCommandBuffer(CameraEvent.BeforeForwardOpaque, commandBuffer);
        }

        // 清理资源
        if (commandBuffer != null)
        {
            commandBuffer.Dispose();
        }
    }
}