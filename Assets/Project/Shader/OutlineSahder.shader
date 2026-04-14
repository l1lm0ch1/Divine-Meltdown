Shader "Custom/OutlineShader"
{
    Properties
    {
        _MainTex ("Base Texture", 2D) = "white" {}
        _OutlineColor ("Outline Color", Color) = (0,0,0,1)
        _OutlineWidth ("Outline Width", Range (1, 5 )) = 5
    }
    SubShader
    {
        Tags { "Queue" = "Overlay" }
        Pass
        {
            Name "OUTLINE"
            Tags { "LightMode" = "Always" }
            ColorMask RGB
            Cull Front
            ZWrite On
            ZTest LEqual
            Blend SrcAlpha OneMinusSrcAlpha
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 pos : POSITION;
            };

            v2f vert (appdata_t v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                return o;
            }

            half4 frag (v2f i) : SV_Target
            {
                return half4(0,0,0,1); // Setze die Farbe der Umrandung auf Schwarz
            }
            ENDCG
        }
        Pass
        {
            Name "BASE"
            Tags { "LightMode" = "ForwardBase" }
            ZWrite On
            ZTest LEqual
            Blend SrcAlpha OneMinusSrcAlpha
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _OutlineColor;
            float _OutlineWidth;

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : POSITION;
                float2 uv : TEXCOORD0;
            };

            v2f vert (appdata_t v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
                return o;
            }

            half4 frag (v2f i) : SV_Target
            {
                // Checke ob die aktuelle Fragmentposition Teil der Umrandung ist
                half4 outline = tex2D(_MainTex, i.uv);
                half4 baseColor = tex2D(_MainTex, i.uv);

                half d = _OutlineWidth * fwidth(max(abs(ddx(i.uv)), abs(ddy(i.uv))));
                if (outline.a < 0.5 || outline.a < tex2D(_MainTex, i.uv + float2( d, 0)).a ||
                    outline.a < tex2D(_MainTex, i.uv + float2(-d, 0)).a ||
                    outline.a < tex2D(_MainTex, i.uv + float2(0,  d)).a ||
                    outline.a < tex2D(_MainTex, i.uv + float2(0, -d)).a)
                {
                    baseColor = _OutlineColor;
                }

                return baseColor;
            }
            ENDCG
        }
    }
}
