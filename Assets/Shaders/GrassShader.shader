Shader "Custom/FracColorQuad"
{
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        Pass
        {
            ZWrite Off
            Cull Off
            Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            // 定义输入结构（appdata）
            struct appdata
            {
                float4 vertex : POSITION; // 顶点位置
            };

            // 定义顶点到片段的输出结构
            struct v2f
            {
                float4 vertex : SV_POSITION; // 裁剪空间的顶点位置
                float2 uv : TEXCOORD0;       // UV 坐标
            };

            // 顶点着色器
            v2f vert(appdata v)
            {
                v2f o;

                // 通过 Unity 内置的 OrthoParams 获取相机的正交投影范围
                // unity_OrthoParams.xy = (宽度, 高度)
                float2 scale = unity_OrthoParams.xy;

                // 顶点位置
                o.vertex = UnityObjectToClipPos(v.vertex);

                // UV 坐标映射到相机的正交投影范围
                o.uv = v.vertex.xy * 0.5 * scale + 0.5;

                return o;
            }

            // 片段着色器
            fixed4 frag(v2f i) : SV_Target
            {
                // 使用 frac 生成周期性 UV 颜色
                fixed4 col = fixed4(frac(i.uv), 0.0, 1.0);
                return col;
            }
            ENDCG
        }
    }
}