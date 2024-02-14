Shader "Hidden/FogShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Weight ("wight", Float) = 1
        _Color ("Color", Color) = (0, 0, 0, 0)
        
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite On ZTest LEqual 

        Pass
        {
            CGPROGRAM
// Upgrade NOTE: excluded shader from DX11, OpenGL ES 2.0 because it uses unsized arrays

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
                float3 viewVector : TEXCOORD1;
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            v2f vert (appdata v)
            {
                v2f output;
                output.pos = UnityObjectToClipPos(v.vertex);
                output.uv = v.uv;
                float3 viewVector = mul(unity_CameraInvProjection, float4(v.uv * 2 - 1, 0, -1));
                output.viewVector = mul(unity_CameraToWorld, float4(viewVector,0));
                return output;
            }
            float _Weight;

            float rayBoxDst(float3 boundsMin, float3 boundsMax, float3 rayOrigin, float3 rayDir, float depth){
                float3 t0 = (boundsMin - rayOrigin) / rayDir;
                float3 t1 = (boundsMax - rayOrigin) / rayDir;
                float3 tmin = min(t0, t1);
                float3 tmax = max(t0, t1);

                float dstA = max(max(tmin.x, tmin.y), tmin.z);
                float dstB = min(tmax.x, min(tmax.y, tmax.z));

                float dstToBox = max(0, dstA);
                float dstInsideBox = max(0, dstB - dstToBox);
                
                float transmittance = (dstInsideBox >0 && dstToBox < depth) ? (1 - (dstInsideBox * _Weight)) : 1;
                return max(transmittance, 1 - clamp(abs(depth-dstToBox) * _Weight, 0, 1));
            }

            sampler2D _MainTex;
            sampler2D _CameraDepthTexture;
            float3 _BoundsMin0;
            float3 _BoundsMax0;
            float3 _BoundsMin1;
            float3 _BoundsMax1;
            float3 _BoundsMin2;
            float3 _BoundsMax2;
            float3 _BoundsMin3;
            float3 _BoundsMax3;
            float3 _BoundsMin4;
            float3 _BoundsMax4;
            float3 _BoundsMin5;
            float3 _BoundsMax5;
            int _Count;
            float NumSteps = 2;
            float4 _Color;
            float4 frag (v2f input) : SV_Target
            {
                float4 col = tex2D(_MainTex, input.uv);
                float3 rayOrigin = _WorldSpaceCameraPos;
                float3 rayDir = normalize(input.viewVector);

                float nonLinearDepth = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, input.uv);
                float depth = LinearEyeDepth(nonLinearDepth) * length(input.viewVector);
                float2 rayBoxInfo;
                
                float dstToBox = 0;
                float dstInsideBox = 0;
                float transmittance = 0;
                if(_Count > 0 && _Count < 7){
                    if(_Count <= 1){
                        transmittance = rayBoxDst(_BoundsMin0, _BoundsMax0, rayOrigin, rayDir, depth);
                    }

                    else if(_Count <= 2){
                        transmittance = rayBoxDst(_BoundsMin0, _BoundsMax0, rayOrigin, rayDir, depth);
                        transmittance += rayBoxDst(_BoundsMin1, _BoundsMax1, rayOrigin, rayDir, depth);

                        transmittance = transmittance >= 2 ? transmittance * 0.5 : 1 - (2 - transmittance);
                    }
                    else if(_Count <= 3){
                        transmittance = rayBoxDst(_BoundsMin0, _BoundsMax0, rayOrigin, rayDir, depth);
                        transmittance += rayBoxDst(_BoundsMin1, _BoundsMax1, rayOrigin, rayDir, depth);
                        transmittance += rayBoxDst(_BoundsMin2, _BoundsMax2, rayOrigin, rayDir, depth);

                        transmittance = transmittance >= 3 ? transmittance / 3 : 1 - (3 - transmittance);
                    }
                    else if(_Count <= 4){
                        transmittance = rayBoxDst(_BoundsMin0, _BoundsMax0, rayOrigin, rayDir, depth);
                        transmittance += rayBoxDst(_BoundsMin1, _BoundsMax1, rayOrigin, rayDir, depth);
                        transmittance += rayBoxDst(_BoundsMin2, _BoundsMax2, rayOrigin, rayDir, depth);
                        transmittance += rayBoxDst(_BoundsMin3, _BoundsMax3, rayOrigin, rayDir, depth);

                        transmittance = transmittance >= 4 ? transmittance / 4 : 1 - (4 - transmittance);
                    }
                    else if(_Count <= 5){
                        transmittance = rayBoxDst(_BoundsMin0, _BoundsMax0, rayOrigin, rayDir, depth);
                        transmittance += rayBoxDst(_BoundsMin1, _BoundsMax1, rayOrigin, rayDir, depth);
                        transmittance += rayBoxDst(_BoundsMin2, _BoundsMax2, rayOrigin, rayDir, depth);
                        transmittance += rayBoxDst(_BoundsMin3, _BoundsMax3, rayOrigin, rayDir, depth);
                        transmittance += rayBoxDst(_BoundsMin4, _BoundsMax4, rayOrigin, rayDir, depth);

                        transmittance = transmittance >= 5 ? transmittance / 5 : 1 - (5 - transmittance);
                    }
                    else if(_Count <= 6){
                        transmittance = rayBoxDst(_BoundsMin0, _BoundsMax0, rayOrigin, rayDir, depth);
                        transmittance += rayBoxDst(_BoundsMin1, _BoundsMax1, rayOrigin, rayDir, depth);
                        transmittance += rayBoxDst(_BoundsMin2, _BoundsMax2, rayOrigin, rayDir, depth);
                        transmittance += rayBoxDst(_BoundsMin3, _BoundsMax3, rayOrigin, rayDir, depth);
                        transmittance += rayBoxDst(_BoundsMin4, _BoundsMax4, rayOrigin, rayDir, depth);
                        transmittance += rayBoxDst(_BoundsMin5, _BoundsMax5, rayOrigin, rayDir, depth);

                        transmittance = transmittance >= 6 ? transmittance / 6 : 1 - (6 - transmittance);
                    }

                    transmittance = clamp(transmittance, 0, 1);
                    float4 result = lerp(_Color, col, transmittance);
                    return result;
                }
                else{
                    return col;
                }
            }
            ENDCG
        }
    }
}
