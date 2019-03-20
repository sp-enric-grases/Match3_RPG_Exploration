// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "socialPointCG/sCG_gradientmapWithSurfaces"
{
	Properties
	{
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Albedo", 2D) = "white" {}
		_MetallicGlossMap("Metallic", 2D) = "white" {}
		_GlossMapScale("Smoothness", Range(0, 1)) = 0
		_MetallicFactor("Metallic Factor",Range(0, 1)) = 0
		_BumpMap("Normal Map", 2D) = "bump" {}
		_BumpScale("Scale", Range(0, 1)) = 1.0
		_DetailAlbedoMap("Detail Albedo x2", 2D) = "white" {}
		_OverlayMap("Overlay", 2D) = "white" {}
		_OverlayColor("Overlay color", Color) = (1,1,1,1)
		_OverlayMult("Overlay Multiplier", Range(1, 10)) = 1
		_OverlaySpeed("Overlay (speed/tile : XY/ZW)", Vector) = (5.0,1.0,1.0,1.0)
	}
	
	SubShader
	{
		Tags
		{ 
			"RenderType" = "Opaque" 
		}
		
		LOD 200

		CGPROGRAM

		/*
		finalcolor:myFinalColor 
				...works only in forward!
		exclude_path:deferred 
				...forces forward rendering for the shader.
		*/

		// Physically based Standard lighting model, and enable shadows on all light types
		// #pragma surface surf Standard fullforwardshadows vertex:vert finalcolor:myFinalColor exclude_path:deferred 
		#pragma surface surf Standard fullforwardshadows vertex:vert

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;
		sampler2D _DetailAlbedoMap;
		sampler2D _BumpMap;
		sampler2D _MetallicGlossMap;
		sampler2D _OverlayMap;

		float _MetallicFactor;
		float _BumpScale;
		float _GlossMapScale;
		float _OverlayMult;
		float4 _OverlayColor;
		float4 _OverlaySpeed;
		fixed4 _Color;

		struct Input 
		{
			float2 uv_MainTex;
			float2 uv2_DetailAlbedoMap;
			float2 overlayCoords;
		};

		/*
		struct appdata_full 
		{
			float4 vertex : POSITION;
			float4 tangent : TANGENT;
			float3 normal : NORMAL;
			fixed4 color : COLOR;
			float4 texcoord : TEXCOORD0;
			float4 texcoord1 : TEXCOORD1;
			half4 texcoord2 : TEXCOORD2;
			half4 texcoord3 : TEXCOORD3;
			half4 texcoord4 : TEXCOORD4;
			half4 texcoord5 : TEXCOORD5;
		};
		*/

		void vert(inout appdata_full v, out Input o)
		{
			UNITY_INITIALIZE_OUTPUT(Input, o);
			o.overlayCoords = v.texcoord1.xy + float2(_OverlaySpeed.x * _Time.x, _OverlaySpeed.y * _Time.x); // Save the Vertex Color in the Input for the surf() method
			o.overlayCoords *= float2(_OverlaySpeed.z, _OverlaySpeed.w);
		}
		
		void surf(Input IN, inout SurfaceOutputStandard o) {
		
			// Albedo
			fixed4 DF = tex2D(_MainTex, IN.uv_MainTex);
			fixed4 DF_detail = tex2D(_DetailAlbedoMap, IN.uv2_DetailAlbedoMap);
			fixed4 Overlay = tex2D(_OverlayMap, IN.overlayCoords) * _OverlayColor * _OverlayMult;

			o.Albedo = (DF * DF_detail * _Color) + Overlay.rgb;
			
			// Bump
			o.Normal = lerp(fixed3(0.0,0.0,1.0), UnpackNormal(tex2D(_BumpMap, IN.uv_MainTex)), _BumpScale).rgb;
			
			// Metallic
			fixed4 mc = tex2D(_MetallicGlossMap, IN.uv_MainTex);
			o.Metallic = mc.r * _MetallicFactor;
			o.Smoothness = mc.a * _GlossMapScale;
		}

		//works only with forward rendering. you need to force it with the "exclude_path:deferred" option on the #pragma
		/*
		void myFinalColor(Input IN, SurfaceOutputStandard o, inout fixed4 color)
		{
			color *= _OverlayColor;
		}
		*/
		

	ENDCG

	}
	
	FallBack "Diffuse"
}