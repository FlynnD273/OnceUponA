Shader "Custom/PaperEdge"
{
    Properties
    {
        _Threshold ("Threshold", Float) = 0.5
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float _Threshold;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 worldUv : TEXCOORD1;
                float4 vertex : SV_POSITION;
                float2 modelScale : TEXCOORD2;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                
                o.worldUv = mul(unity_ObjectToWorld, v.vertex);
                o.modelScale = float2(length(unity_ObjectToWorld._m00_m01_m02), length(unity_ObjectToWorld._m10_m11_m12));
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float2 uv = 1.0 - abs(i.uv - 0.5) * 2.0;

                float gradient1 = uv.x * i.modelScale.x;
                float gradient2 = uv.y * i.modelScale.y;
                float gradient = min(gradient1, gradient2);
                gradient *= 0.25 / _Threshold;

                float midpoint = 0.7;
                float scale = 1.5;
                float2 offsetUv = frac((i.worldUv.xy - unity_ObjectToWorld._m03_m13) / 20);
                fixed3 tex = ((tex2D(_MainTex, offsetUv).rgb - midpoint) * scale) + midpoint;
                float texLum = dot(tex, fixed3(1.0 / 3.0, 1.0 / 3.0, 1.0 / 3.0));

                float alpha = step(smoothstep(_Threshold, 0.0, gradient), texLum);

                return fixed4(tex, alpha);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}

