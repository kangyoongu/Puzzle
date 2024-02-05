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

            float2 rayBoxDst(float3 boundsMin, float3 boundsMax, float3 rayOrigin, float3 rayDir){
                float3 t0 = (boundsMin - rayOrigin) / rayDir;
                float3 t1 = (boundsMax - rayOrigin) / rayDir;
                float3 tmin = min(t0, t1);
                float3 tmax = max(t0, t1);

                float dstA = max(max(tmin.x, tmin.y), tmin.z);
                float dstB = min(tmax.x, min(tmax.y, tmax.z));

                float dstToBox = max(0, dstA);
                float dstInsideBox = max(0, dstB - dstToBox);
                return float2(dstToBox, dstInsideBox);
            }

            float2 rayBoxDst(float3 boundsMin, float3 boundsMax, float3 boundsMin1, float3 boundsMax1, float3 rayOrigin, float3 rayDir){
                float3 t0 = (boundsMin - rayOrigin) / rayDir;
                float3 t1 = (boundsMax - rayOrigin) / rayDir;
                float3 t2 = (boundsMin1 - rayOrigin) / rayDir;
                float3 t3 = (boundsMax1 - rayOrigin) / rayDir;
                float3 tmin = min(t0, t1);
                float3 tmax = max(t0, t1);
                float3 tmin2 = min(t2, t3);
                float3 tmax2 = max(t2, t3);

                float dstA = max(max(tmin.x, tmin.y), tmin.z);
                float dstB = min(tmax.x, min(tmax.y, tmax.z));
                float dstA2 = max(max(tmin2.x, tmin2.y), tmin2.z);
                float dstB2 = min(tmax2.x, min(tmax2.y, tmax2.z));

                float dstToBox = max(0, min(dstA, dstA2));
                float dstInsideBox = max(0, max(dstB, dstB2) - dstToBox);
                return float2(dstToBox, dstInsideBox);
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
            int _Count;
            float NumSteps = 2;
            float _Weight;
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
                if(_Count <= 1){
                    rayBoxInfo = rayBoxDst(_BoundsMin0, _BoundsMax0, rayOrigin, rayDir);
                    dstToBox = rayBoxInfo.x;
                    dstInsideBox = rayBoxInfo.y;
                    transmittance = (dstInsideBox >0 && dstToBox < depth) ? (1 - (dstInsideBox * _Weight)) : 1;
                }
                else if(_Count <= 2){
                    rayBoxInfo = rayBoxDst(_BoundsMin0, _BoundsMax0, rayOrigin, rayDir);
                    dstToBox = rayBoxInfo.x;
                    dstInsideBox = rayBoxInfo.y;
                    transmittance = (dstInsideBox >0 && dstToBox < depth) ? (1 - (dstInsideBox * _Weight)) : 1;
                    transmittance = max(transmittance, 1 - clamp(abs(depth-dstToBox) * _Weight, 0, 1));

                    rayBoxInfo = rayBoxDst(_BoundsMin1, _BoundsMax1, rayOrigin, rayDir);
                    dstToBox = rayBoxInfo.x;
                    dstInsideBox = rayBoxInfo.y;
                    float transmittance1 = (dstInsideBox > 0 && dstToBox < depth) ? (1 - (dstInsideBox * _Weight)) : 1;
                    transmittance1 = max(transmittance1, 1 - clamp(abs(depth-dstToBox) * _Weight, 0, 1));

                    transmittance = (transmittance + transmittance1);
                    transmittance = transmittance >= 2 ? transmittance * 0.5 : 1 - (2 - transmittance);
                }


                transmittance = clamp(transmittance, 0, 1);
                float4 result = lerp(_Color, col, transmittance);
                return result;
            }
            ENDCG
        }
    }
}
