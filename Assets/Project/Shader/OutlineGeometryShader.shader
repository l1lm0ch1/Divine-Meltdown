Shader "Custom/OutlineGeometryShader"
{
    Properties
    {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _OutlineColor ("Outline Color", Color) = (0,0,0,1)
        _Outline ("Outline width", Range (.002, 0.03)) = .005
    }
    SubShader
    {
        Tags {"RenderType"="Opaque"}

        // Render the main texture
        Pass
        {
            Name "BASE"
            Tags {"LightMode"="ForwardBase"}
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : POSITION;
                float2 uv : TEXCOORD0;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            half4 frag (v2f i) : COLOR
            {
                half4 col = tex2D(_MainTex, i.uv);
                return col;
            }
            ENDCG
        }

        // Render the outline
        Pass
        {
            Name "OUTLINE"
            Tags { "LightMode" = "Always" }
            Cull Front
            ZWrite On
            ZTest LEqual
            ColorMask RGB
            Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM
            #pragma vertex vert
            #pragma geometry geom
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 pos : POSITION;
                float4 color : COLOR;
            };

            uniform float _Outline;
            uniform float4 _OutlineColor;

            void vert (appdata_t v, inout v2f o)
            {
                o.pos = UnityObjectToClipPos(v.vertex);
                o.color = _OutlineColor;
            }

            [maxvertexcount(18)]
            void geom(triangle appdata_t input[3], inout TriangleStream<v2f> output)
            {
                for (int i = 0; i < 3; i++)
                {
                    v2f o;
                    float3 norm = mul((float3x3)unity_ObjectToWorld, input[i].normal);
                    o.pos = UnityObjectToClipPos(input[i].vertex + float4(norm * _Outline, 0));
                    o.color = _OutlineColor;
                    output.Append(o);
                }
            }

            half4 frag (v2f i) : COLOR
            {
                return i.color;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
