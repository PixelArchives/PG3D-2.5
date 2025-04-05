Shader "Hidden/Nature/Tree Soft Occlusion Bark Rendertex" {
Properties {
 _Color ("Main Color", Color) = (1,1,1,0)
 _MainTex ("Main Texture", 2D) = "white" {}
 _BaseLight ("Base Light", Range(0,1)) = 0.35
 _AO ("Amb. Occlusion", Range(0,10)) = 2.4
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