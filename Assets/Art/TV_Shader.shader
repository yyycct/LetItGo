Shader "Custom/UVScrollShader"
{
    Properties
    {
        _MainTex ("Main Texture", 2D) = "white" {}
        _Rows ("Number of Rows", Float) = 4
        _Interval ("Scroll Interval (Seconds)", Float) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            sampler2D _MainTex;
            float _Rows;
            float _Interval;
            float4 _MainTex_ST;

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // 使用间隔时间来计算当前翻页的阶段
                float timePhase = floor(_Time.y / _Interval);

                // 计算行偏移，确保每隔Interval秒翻动一帧
                float rowOffset = fmod(timePhase, _Rows) / _Rows;

                // 调整UV坐标
                float2 uv = i.uv;
                uv.y = uv.y / _Rows + rowOffset;

                // 取样纹理
                fixed4 col = tex2D(_MainTex, uv);
                return col;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
