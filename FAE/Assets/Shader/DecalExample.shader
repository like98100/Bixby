Shader "URP/DecalExample"
{
    Properties
    {
        [MainColor] _BaseColor("Base Color", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags { "RenderPipeline" = "UniversalPipeline" "RenderType"="Opaque" }
        LOD 100

        Pass
        {
			Name "FowardLit"
			Tags { "LightMode" = "UniversalForward" }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag


            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"	

            struct appdata
            {
                float4 positionOS : POSITION;
				float3 NormalOS : NORMAL;
            };

            struct v2f
            {
                float4 positionHCS : SV_POSITION;
				float3 normal : TEXCOORD0;
            };

			CBUFFER_START(UnityPerMaterial)
			float4 _BaseColor;
			CBUFFER_END

            v2f vert (appdata v)
            {
                v2f o;
				VertexPositionInputs vertexInput = GetVertexPositionInputs(v.positionOS.xyz);
				o.positionHCS = vertexInput.positionCS;
				o.normal = TransformObjectToWorldNormal(v.NormalOS);
                return o;
            }

			void RenderDecal(float4 positionCS, inout half4 _Basecolor_var, inout half3 normalWS)
			{
				half3 specular = 0;
				half metallic = 0;
				half occlusion = 0;
				half smoothness = 0;

				ApplyDecal(positionCS, _Basecolor_var.rgb, specular, normalWS, metallic, occlusion, smoothness);
			}



            half4 frag (v2f i) : SV_Target
            {
				half4 _color = _BaseColor;

				RenderDecal(i.positionHCS, _color, i.normal);
                return _color;
            }
            ENDHLSL
        }


		pass
		{
			Name "DepthNormals"
			Tags { "LightMode" = "DepthNormals" }

			HLSLPROGRAM

			#pragma vertex DepthNormalsVertex
			#pragma fragment DepthNormalsFragment

			#include "Packages/com.unity.render-pipelines.universal/Shaders/LitInput.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/Shaders/LitDepthNormalsPass.hlsl"

			ENDHLSL
		}
    }
}
