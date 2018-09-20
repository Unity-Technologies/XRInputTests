Shader "Custom/GazeShader"
{
    Properties
    {
        _TintColor("Tint Color", Color) = (0, 0, 0, 0)
    }
        SubShader
        {
            Tags { "RenderType" = "Transparent" "Queue" = "Transparent" }
            LOD 100
            ZTest Always
            Cull Off
            ZWrite Off
            Blend One One

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
            };

            fixed4 _TintColor;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = _TintColor;

                return col;
            }
            ENDCG
        }
    }
}
