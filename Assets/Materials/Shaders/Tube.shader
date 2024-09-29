Shader "Custom/Tube"
{
    Properties
    {
        _Color ("Color",Color) = (1,1,1,1)
        _MainTex ("Texture", 2D) = "white" {}
        _Scale ("Scale",float) = 1
        _Size ("Curve", vector) = (1,1,1,1)
        //_Curve ("Curve",vector) = (1,1,0.2,0.2)
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
            float4 _Size;
            v2f vert (appdata v)
            {
                v2f o;
                nv = v.vertex;
                //nv.x = -cos(v.vertex.x / _Size.x) * _Size.w;
                nv.y = -((cos(v.vertex.y / _Size.y) * _Size.w) * (cos(v.vertex.x / _Size.x) * _Size.w));
                nv.z = -((sin(v.vertex.y / _Size.y) * _Size.w) * (cos(v.vertex.x / _Size.x) * _Size.w));
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
