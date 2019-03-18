Shader "Custom/ArrowShader"
{

    Properties
    {
        _TintColor("Tint Color", Color) = (0.5,0.5,0.5,0.5)
        _BaseColor("Base Color", Color) = (0.0, 0.0, 0.0, 0.0)
        _LerpFactor("Lerp Factor", Range(0.0,1.0)) = 0.0
        _LerpRate("Lerp Rate", Range(1, 3)) = 2

    }
    SubShader
    {
        Tags{ "RenderType" = "Transparent" "Queue" = "Transparent" }
        LOD 100

        Cull Off
        Blend SrcAlpha One

        Pass
    {
        CGPROGRAM
#pragma vertex vert
#pragma fragment frag

#include "UnityCG.cginc"


        struct appdata
    {
        float4 vertex : POSITION;
        fixed4 color : COLOR;
    };

    struct v2f
    {
        fixed4 color : COLOR;
        float4 vertex : SV_POSITION;
    };

    half _LerpFactor;
    fixed _LerpRate;
    fixed4 _TintColor;
    fixed4 _BaseColor;

    v2f vert(appdata v)
    {
        v2f o;
        o.vertex = UnityObjectToClipPos(v.vertex);
        o.color = v.color;
        return o;
    }

    fixed4 frag(v2f i) : SV_Target
    {
        float alpha = saturate(i.color.r + (_LerpRate * _LerpFactor - 1.0));

    fixed4 col = _BaseColor;// +_TintColor * alpha;
    col.rgb += _TintColor * alpha;
    col.a += alpha;
        // sample the texture

    // apply fog
    return col;
    }
        ENDCG
    }
    }
}
