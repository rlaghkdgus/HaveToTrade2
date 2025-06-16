Shader "Unlit/NewUnlitShader"
{
    Properties
    {
        _HoleRect("Hole Rect (x, y, width, height)", Vector) = (0.3, 0.3, 0.4, 0.4)
        _DarkColor("Dark Color", Color) = (0, 0, 0, 0.6)
    }
        SubShader
    {
        Tags { "RenderType" = "Transparent" "Queue" = "Transparent" }
        LOD 100

        Pass
        {
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            fixed4 _DarkColor;
            float4 _HoleRect; // x, y, width, height

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float2 uv = i.uv;

                float2 min = _HoleRect.xy;
                float2 max = _HoleRect.xy + _HoleRect.zw;

                bool inRect = uv.x >= min.x && uv.x <= max.x &&
                              uv.y >= min.y && uv.y <= max.y;

                if (inRect)
                    return fixed4(0, 0, 0, 0); // ±¸¸Û: Åõ¸í

                return _DarkColor; // ³ª¸ÓÁö: ¾îµÓ°Ô
            }
            ENDCG
        }
    }
}
