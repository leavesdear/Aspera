Shader "Custom/GlitchEffect"
{
Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        
        _Intensity ("Overall Intensity", Range(0, 1)) = 0.5
        _JumpIntensity ("Jump Intensity", Range(0, 0.1)) = 0.02
        _StaticIntensity ("Static Intensity", Range(0, 1)) = 0.3
        
        // 彩色条带
        [HDR]_ColorBand1 ("Color Band 1", Color) = (1, 0.2, 0.2, 1)
        [HDR]_ColorBand2 ("Color Band 2", Color) = (0.2, 1, 0.2, 1) 
        [HDR]_ColorBand3 ("Color Band 3", Color) = (0.2, 0.2, 1, 1)
        
        _BandSpeed ("Band Speed", Float) = 2
        _BandSize ("Band Size", Range(0.1, 1)) = 0.3
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        Cull Off
        Lighting Off
        ZWrite Off
        Blend One OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float4 color : COLOR;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                fixed4 color : COLOR;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed4 _Color;
            
            float _Intensity;
            float _JumpIntensity;
            float _StaticIntensity;
            fixed4 _ColorBand1, _ColorBand2, _ColorBand3;
            float _BandSpeed;
            float _BandSize;

            float hash(float2 p)
            {
                return frac(sin(dot(p, float2(127.1, 311.7))) * 43758.5453);
            }

            float noise(float2 p)
            {
                float2 i = floor(p);
                float2 f = frac(p);
                f = f * f * (3.0 - 2.0 * f);
                
                float a = hash(i);
                float b = hash(i + float2(1.0, 0.0));
                float c = hash(i + float2(0.0, 1.0));
                float d = hash(i + float2(1.0, 1.0));
                
                return lerp(lerp(a, b, f.x), lerp(c, d, f.x), f.y);
            }

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.color = v.color * _Color;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = i.uv;
                float time = _Time.y;
                
                // 基础颜色
                fixed4 col = tex2D(_MainTex, uv) * i.color;
                if (col.a == 0) return col;
                
                // === 强烈的垂直跳动 ===
                float jump = (hash(float2(time * 10, uv.y)) - 0.5) * _JumpIntensity * _Intensity;
                uv.x += jump;
                
                // === 彩色静态噪点 ===
                float2 staticUV = uv * 50 + time * 20;
                float staticNoise = noise(staticUV);
                
                // 彩色静态
                float3 colorStatic;
                colorStatic.r = noise(staticUV + float2(100, 100));
                colorStatic.g = noise(staticUV + float2(200, 200));
                colorStatic.b = noise(staticUV + float2(300, 300));
                colorStatic = (colorStatic - 0.5) * _StaticIntensity * _Intensity;
                
                // === 移动的彩色条带 ===
                float3 colorBands = float3(0, 0, 0);
                float bandPos = frac(uv.y * 3 + time * _BandSpeed);
                
                // 多个彩色条带
                if (bandPos < _BandSize)
                {
                    float bandIntensity = (1 - bandPos / _BandSize) * _Intensity;
                    float bandSelect = hash(float2(floor(uv.y * 3), time));
                    
                    if (bandSelect < 0.33)
                        colorBands = _ColorBand1.rgb * bandIntensity;
                    else if (bandSelect < 0.66)
                        colorBands = _ColorBand2.rgb * bandIntensity;
                    else
                        colorBands = _ColorBand3.rgb * bandIntensity;
                }
                
                // === 块状撕裂效果 ===
                float blockSize = 15;
                float2 blockUV = floor(uv * blockSize) / blockSize;
                float blockGlitch = step(0.9, hash(blockUV + time * 0.5)) * _Intensity;
                
                if (blockGlitch > 0)
                {
                    uv.x += (hash(blockUV + float2(10, 10)) - 0.5) * 0.1 * blockGlitch;
                }
                
                // 重新采样处理形变后的颜色
                fixed4 glitchCol = tex2D(_MainTex, uv) * i.color;
                glitchCol.a = col.a;
                
                // === 最终混合 ===
                // RGB分离
                float shift = 0.01 * _Intensity;
                fixed4 shiftedCol;
                shiftedCol.r = tex2D(_MainTex, uv + float2(shift, 0)).r;
                shiftedCol.g = glitchCol.g;
                shiftedCol.b = tex2D(_MainTex, uv - float2(shift, 0)).b;
                shiftedCol.a = col.a;
                
                // 组合所有效果
                fixed4 finalColor = shiftedCol;
                finalColor.rgb += colorStatic;
                finalColor.rgb += colorBands;
                
                // 在故障区域增加对比度
                if (blockGlitch > 0 || length(colorBands) > 0)
                {
                    finalColor.rgb = lerp(finalColor.rgb, finalColor.rgb * 1.3, _Intensity);
                }
                
                finalColor.rgb *= finalColor.a;
                return finalColor;
            }
            ENDCG
        }
    }
}