Shader "Custom/StandardDissolve"
{
    Properties
    {
        _MainTex("Albedo (RGB)", 2D) = "white" {}
        _Color("Color", Color) = (1,1,1,1)
        _Glossiness("Smoothness", Range(0,1)) = 0.5
        _Metallic("Metallic", Range(0,1)) = 0.0
        _BumpMap("Normal Map", 2D) = "bump" {}
        [HDR] _EmissionColor("Emission Color", Color) = (0,0,0)
        _EmissionMap("Emission Map", 2D) = "black" {}

        // Dissolve properties
        _NoiseTex("Dissolve Noise", 2D) = "gray" {}
        _DissolveAmount("Dissolve Amount", Range(0,1)) = 0.0
        _EdgeWidth("Edge Width", Range(0.0, 0.2)) = 0.05
        [HDR] _EdgeColor("Edge Color", Color) = (1,0.4,0,1)
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows
        #pragma target 3.0

        sampler2D _MainTex;
        sampler2D _BumpMap;
        sampler2D _EmissionMap;
        sampler2D _NoiseTex;
        fixed4 _Color;
        half _Glossiness;
        half _Metallic;
        fixed4 _EmissionColor;
        half _DissolveAmount;
        half _EdgeWidth;
        fixed4 _EdgeColor;

        struct Input
        {
            float2 uv_MainTex;
            float2 uv_BumpMap;
            float2 uv_EmissionMap;
            float2 uv_NoiseTex;
        };

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
            o.Emission = tex2D(_EmissionMap, IN.uv_EmissionMap).rgb * _EmissionColor.rgb;

            // --- Dissolve ---
            float noise = tex2D(_NoiseTex, IN.uv_NoiseTex).r;

            // Clip pixels *above* the dissolve threshold (so they disappear gradually)
            float threshold = _DissolveAmount;
            float edgeWidth = _EdgeWidth;

            // Active flag: 0 when off, 1 when dissolve > 0
            float active = step(0.001, threshold);

            // Calculate how close this pixel is to the edge
            float edge = smoothstep(threshold - edgeWidth, threshold, noise);

            // Only keep pixels above the threshold (the solid part)
            clip(noise - threshold * active);

            // Add emission *just before* clipping to form the edge
            float edgeGlow = (1.0 - smoothstep(threshold, threshold + edgeWidth, noise)) * active;
            o.Emission += _EdgeColor.rgb * edgeGlow;
        }
        ENDCG
    }

    FallBack "Diffuse"
}