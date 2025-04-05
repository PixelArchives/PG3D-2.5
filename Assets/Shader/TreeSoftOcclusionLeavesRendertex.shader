Shader "Hidden/Nature/Tree Soft Occlusion Leaves Rendertex" {
Properties {
 _Color ("Main Color", Color) = (1,1,1,0)
 _MainTex ("Main Texture", 2D) = "white" {}
 _Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
 _HalfOverCutoff ("0.5 / Alpha cutoff", Range(0,1)) = 1
 _BaseLight ("Base Light", Range(0,1)) = 0.35
 _AO ("Amb. Occlusion", Range(0,10)) = 2.4
 _Occlusion ("Dir Occlusion", Range(0,20)) = 7.5
 _Scale ("Scale", Vector) = (1,1,1,1)
 _SquashAmount ("Squash", Float) = 1
}
	//DummyShaderTextExporter
	
	SubShader{
		Tags { "RenderType" = "Opaque" }
		LOD 200
		CGPROGRAM
#pragma surface surf Standard fullforwardshadows
#pragma target 3.0
		sampler2D _MainTex;
		struct Input
		{
			float2 uv_MainTex;
		};
		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
			o.Albedo = c.rgb;
		}
		ENDCG
	}
}