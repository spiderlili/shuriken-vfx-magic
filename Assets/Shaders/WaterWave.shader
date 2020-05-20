Shader "Transparent/WaterWave"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color("Color Tint", Color) = (1, 1, 1, 1)
        _Magnitude("Distortion Magnitude", Range(-1, 1)) = 0.01
        _Frequency("Distortion Frequency", Range(-1, 1)) = 0
        _InverseWavelength("Distortion Inverse Wavelength", Range(-1, 1)) = 1 //spatial frequency
        _Speed("Speed", Range(-10, 10)) = 0.5
    }

    SubShader
    {
        Tags 
        { 
            "RenderType"="Transparent" "Queue" = "Transparent" "IgnoreProjector" = "True" 
            "DisableBatching" = "True" 
            "LightMode" = "ForwardBase"
        }
        LOD 100

        Pass
        {
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _Frequency;
            float _Magnitude;
            float _InverseWavelength;
            float _Speed;
            fixed3 _Color;

            v2f vert (appdata v)
            {
                v2f o;
                float4 offset;
                offset.xzw = float3(0.0, 0.0, 0.0);
                offset.y = sin(_Frequency * _Time.y + v.vertex.x * _InverseWavelength + v.vertex.y * _InverseWavelength + v.vertex.z * _InverseWavelength) * _Magnitude;

                o.vertex = UnityObjectToClipPos(v.vertex + offset);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.uv += float2(0.0, _Time.y * _Speed);

                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                col.rgb *= _Color.rgb;
                
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
    Fallback "Transparent/VertexLit"
}
