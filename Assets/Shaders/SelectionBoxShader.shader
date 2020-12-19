Shader "Custom/SelectionBoxShader"
{
    Properties
    {
        _MainTex("MainTexture",2D)="white"{}
        _Color ("Color", Color) = (1,1,1,1)
        _EdgeColor("EdgeColor",Color)=(1,1,1,1)
        _Thickness("Thickness",Range(0,1))=0.05
        _EmissionThr("EmissionThreshold",Range(0,0.5))=0.2
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue" = "Transparent" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types

        #pragma surface surf Standard alpha:fade

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0
        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;
        fixed4 _EdgeColor;
        half _Thickness;
        half _EmissionThr;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {

            fixed2 isedge2=fixed2(saturate(step(IN.uv_MainTex.x,_Thickness)+step(1-IN.uv_MainTex.x,_Thickness)),saturate(step(IN.uv_MainTex.y,_Thickness)+step(1-IN.uv_MainTex.y,_Thickness)));
            fixed isedge=step(0.1,length(isedge2));
            fixed2 isemis2=fixed2(saturate(step(IN.uv_MainTex.x,_EmissionThr)+step(1-IN.uv_MainTex.x,_EmissionThr)),saturate(step(IN.uv_MainTex.y,_EmissionThr)+step(1-IN.uv_MainTex.y,_EmissionThr)));
            fixed isemis=saturate(isemis2.x*isemis2.y)*isedge;

            // Albedo comes from a texture tinted by color

            fixed4 c = _Color*abs(isemis-1)+_EdgeColor*isemis;
            o.Albedo = c.rgb;
            o.Emission=c.rgb;
            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
