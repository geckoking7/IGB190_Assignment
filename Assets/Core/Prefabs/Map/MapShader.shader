Shader "UI/MapShader"
{
    Properties {
        _MainTex ("Main Tex", 2D) = "white" {}
        _MaskTex ("Mask Tex", 2D) = "white" {}

        _ColorA ("Primary Color", Color) = (0,0,0,1)
        _ColorB ("Secondary Color", Color) = (0,0,0,1)
        _OutlineColor ("Outline Color", Color) = (1,1,1,1)
        _OutlineThickness ("Outline Thickness", Range(0.001, 0.02)) = 0.005

        // --- UI Masking properties (required for UGUI masking) ---
        _StencilComp ("Stencil Comparison", Float) = 8
        _Stencil ("Stencil ID", Float) = 0
        _StencilOp ("Stencil Operation", Float) = 0
        _StencilWriteMask ("Stencil Write Mask", Float) = 255
        _StencilReadMask ("Stencil Read Mask", Float) = 255
        _ColorMask ("Color Mask", Float) = 15
    }

    SubShader {
        Tags {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Off

        Pass {
            // --- Stencil setup to respect UI Mask ---
            Stencil {
                Ref [_Stencil]
                Comp [_StencilComp]
                Pass [_StencilOp]
                ReadMask [_StencilReadMask]
                WriteMask [_StencilWriteMask]
            }
            ColorMask [_ColorMask]

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex, _MaskTex;
            float4 _MainTex_ST;
            fixed4 _ColorA;
            fixed4 _ColorB;
            fixed4 _OutlineColor;
            float _OutlineThickness;

            v2f vert(appdata_t v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target {
                fixed4 col = tex2D(_MainTex, i.uv);
                fixed4 mask = tex2D(_MaskTex, i.uv);
                col = lerp(_ColorA, _ColorB, col.r);

                // --- Outline sampling ---
                float2 offset = float2(_OutlineThickness, 0);
                float outlineSample =
                    tex2D(_MaskTex, i.uv + offset).a +
                    tex2D(_MaskTex, i.uv - offset).a +
                    tex2D(_MaskTex, i.uv + offset.yx).a +
                    tex2D(_MaskTex, i.uv - offset.yx).a;

                // If pixel is near edge (some neighbors opaque, current transparent)
                float isEdge = step(0.5, outlineSample) * (1.0 - mask.a);

                fixed4 finalCol = lerp(col, _OutlineColor, isEdge);
                finalCol.a = max(mask.a * 0.8, isEdge); // keep solid outline
                return finalCol;
            }
            ENDCG
        }
    }

    FallBack "UI/Default"
}