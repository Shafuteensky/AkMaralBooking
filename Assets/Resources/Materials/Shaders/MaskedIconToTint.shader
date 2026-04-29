Shader "Custom/MaskedIconToTint"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        [PerRendererData] _Color ("Color", Color) = (1,1,1,1)

        _MaskColor ("Mask Color", Color) = (1,1,1,1)
        _ColorTolerance ("Color Tolerance", Range(0, 1)) = 0.2

        _Softness ("Edge Softness", Range(0.001, 0.3)) = 0.05
        _MaskPower ("Mask Power", Range(0.25, 4)) = 1.25
        _EdgeAA ("Edge AA", Range(0, 4)) = 1.5
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "RenderType"="Transparent"
            "RenderPipeline"="UniversalPipeline"
            "IgnoreProjector"="True"
            "CanUseSpriteAtlas"="True"
        }

        Cull Off
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            Name "Forward"
            Tags { "LightMode"="UniversalForward" }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            CBUFFER_START(UnityPerMaterial)
                float4 _Color;
                float4 _MaskColor;
                float _ColorTolerance;
                float _Softness;
                float _MaskPower;
                float _EdgeAA;
            CBUFFER_END

            Varyings vert(Attributes input)
            {
                Varyings output;
                output.positionHCS = TransformObjectToHClip(input.positionOS.xyz);
                output.uv = input.uv;
                output.color = input.color * _Color;
                return output;
            }

            half4 frag(Varyings input) : SV_Target
            {
                half4 tex = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, input.uv);

                // расстояние до цвета фона
                half dist = distance(tex.rgb, _MaskColor.rgb);

                // антиалиас ширина
                half edgeWidth = fwidth(dist) * _EdgeAA;

                // маска: всё близкое к цвету фона → прозрачное
                half mask = smoothstep(
                    _ColorTolerance - _Softness - edgeWidth,
                    _ColorTolerance + _Softness + edgeWidth,
                    dist
                );

                // усиление формы
                mask = pow(saturate(mask), _MaskPower);

                // сглаживание
                mask = mask * mask * (3.0h - 2.0h * mask);

                half4 result;
                result.rgb = input.color.rgb;
                result.a = mask * input.color.a;

                return result;
            }
            ENDHLSL
        }
    }
}