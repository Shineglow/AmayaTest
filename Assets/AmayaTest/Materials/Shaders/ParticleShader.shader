Shader "Unlit/ParticleShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _OutlineColor ("Outline Color", Color) = (0, 0, 0, 1)
        _OutlineThickness ("Outline Thickness", Range(0, 0.1)) = 0.02
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 100

        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Off
        
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            sampler2D _MainTex;
            fixed4 _OutlineColor;
            float _OutlineThickness;

            struct appdata_t {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                fixed4 color : COLOR; // Цвет частиц
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                float4 pos : SV_POSITION;
                fixed4 color : COLOR;
            };

            v2f vert (appdata_t v) {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.color = v.color; // Передаём цвет частиц
                return o;
            }

            fixed4 frag (v2f i) : SV_Target {
                fixed4 texColor = tex2D(_MainTex, i.uv);

                float2 offsets[8] = {
                    float2(-1,  0), float2(1,  0),
                    float2( 0, -1), float2(0,  1),
                    float2(-1, -1), float2(1, -1),
                    float2(-1,  1), float2(1,  1)
                };

                bool isOutline = false;

                for (int j = 0; j < 8; j++) {
                    float2 offsetUV = i.uv + offsets[j] * _OutlineThickness;
                    fixed4 neighborColor = tex2D(_MainTex, offsetUV);

                    if (neighborColor.a < 0.1 && texColor.a > 0.1) {
                        isOutline = true;
                        break;
                    }
                }
                
                return isOutline ? _OutlineColor : texColor * i.color;
            }
            ENDCG
        }
    }
}
