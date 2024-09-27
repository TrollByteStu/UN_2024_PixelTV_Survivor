Shader "Custom/Globe"
{
    Properties
    {
        _Curve ("Curve",vector) = (1,1,0.2,0.2)
        _Color ("Color",Color) = (1,1,1,1)
        _MainTex ("Texture", 2D) = "white" {}
        _Scale ("Scale",float) = 1
        _Offset ("Offset",vector) = (0,0,0,0)
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always
        Tags { "RenderType"="Opaque" }
        LOD 200

        Pass
        {
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

            float4 _Curve;
            float4 nv;
            v2f vert (appdata v)
            {
                v2f o;
                nv = v.vertex;
                nv.y = v.vertex.y + cos(abs(v.vertex.x) * _Curve.z)* _Curve.x;
                nv.z = v.vertex.z - cos(abs(v.vertex.x) * _Curve.w) * _Curve.y - cos(abs(v.vertex.y) * _Curve.w) * _Curve.y +10;
                o.vertex = UnityObjectToClipPos(nv);
                o.uv = v.uv;
                return o;
            }

            fixed4 _Color;
            sampler2D _MainTex;
            float2 _Offset ;
            float _Scale = 1;

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = (i.uv/ _Scale + _Offset % 1 + 1 )% 1;
                fixed4 col = tex2D(_MainTex, uv) * _Color;
                // just invert the colors
                //col.rgb = 1 - col.rgb;
                return col;
            }
            ENDCG
        }
    }
}
