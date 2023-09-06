Shader "Toon/Toon Lit Stencil Masked"
{
    Properties
    {
        _Color("Main Color", Color) = (0.5, 0.5, 0.5, 1)
        _MainTex("Base (RGB)", 2D) = "white" {}
        _Ramp("Toon Ramp (RGB)", 2D) = "gray" {}
        _ID("Mask ID", Int) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue" = "Geometry+2"} // Render after the mask - the queue must be higher than mask
        LOD 200
        
        Stencil{ 
            Ref [_ID]
            Comp Equal
        }
        
        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        // #pragma surface surf Standard fullforwardshadows

        // Toon ramp rendering
        #pragma surface surf ToonRamp
        sampler2D _Ramp;

        #pragma lighting ToonRamp exclude_path:forward
        
        // custom lighting function that uses a texture ramp based on angle between light direction & normal
        inline half4 LightingToonRamp(SurfaceOutput s, half3 lightDir, half atten)
        {
            #ifndef USING_DIRECTIONAL_LIGHT
                lightDir = normalize(lightDir);
            #endif
           
            half d = dot (s.Normal, lightDir) * 0.5 + 0.5; // 0 - 1
            half3 ramp = tex2D (_Ramp, float2(d, d)).rgb;
           
            half4 c;
            c.rgb = s.Albedo * _LightColor0.rgb * ramp * (atten * 2);
            c.a = 0;
            return c;
        }
        
        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;
        float4 _Color;

        struct Input
        {
            float2 uv_MainTex : TEXCOORD0;
        };

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)
        
        void surf (Input IN, inout SurfaceOutput o)
        {
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
