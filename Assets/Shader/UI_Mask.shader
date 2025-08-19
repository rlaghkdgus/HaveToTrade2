Shader "UI/InverseMask"
{
    Properties
    {
        [PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
        _StencilComp("Stencil Comparison", Float) = 6
        _Stencil("Stencil ID", Float) = 1
        _StencilOp("Stencil Operation", Float) = 0
        _StencilWriteMask("Stencil Write Mask", Float) = 255
        _StencilReadMask("Stencil Read Mask", Float) = 255
        _ColorMask("Color Mask", Float) = 15
    }
    SubShader
    {
        Tags{ "Queue" = "Transparent" "RenderType" = "Transparent" "RenderPipeline" =
           "UniversalPipeline" }

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
                Cull Off
                ZWrite Off
                ZTest[unity_GUIZTestMode]

                Stencil
                {
                Ref[_Stencil]
                    Comp[_StencilComp]
                    Pass[_StencilOp]
                    ReadMask[_StencilReadMask]
                    WriteMask[_StencilWriteMask]
                }

                HLSLPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

                struct Attributes
                {
                    float4 vertex   : POSITION;
                    float4 color    : COLOR;
                    float2 uv       : TEXCOORD0;
                };

                struct Varyings
                {
                    float4 vertex   : SV_POSITION;
                    float4 color    : COLOR;
                    float2 uv       : TEXCOORD0;
                };

                Varyings vert(Attributes input)
                {
                    Varyings output;
                    output.vertex = TransformObjectToHClip(input.vertex.xyz);
                    output.uv = input.uv;
                    output.color = input.color;
                    return output;
                }

                half4 frag(Varyings input) : SV_Target
                {
                    return input.color;
                }
                ENDHLSL
        }
    }
}
