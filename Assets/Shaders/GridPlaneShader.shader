Shader "Custom/GridPlaneShader"
{
    Properties
    {
        _BaseColor ("BaseColor", Color) = (1,1,1,1)
        _GridColor("GridColor",Color)=(1,1,1,1)
        _Range("Range",Range(0,1000))=1
        _Thickness("Thickness",Range(0,1000))=0.1
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0


        struct Input
        {
            float2 uv_MainTex;
            float3 worldPos;
        };

        half _Glossiness;
        half _Metallic;

        half _Range;
        half _Thickness;
        fixed4 _BaseColor;
        fixed4 _GridColor;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color
            float2 suv = float2(frac(IN.worldPos.x/_Range)*_Range,frac(IN.worldPos.z/_Range)*_Range);

            fixed2 isgrid2=fixed2(saturate(step(suv.x,_Thickness/2)+step(10-suv.x,_Thickness/2)),saturate(step(suv.y,_Thickness/2)+step(10-suv.y,_Thickness/2)));
            fixed isgrid=step(0.1,length(isgrid2));

            fixed3 c=_BaseColor.rgb*abs(isgrid-1)+_GridColor.rgb*isgrid;
            o.Albedo = c.rgb;
            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = 1.0;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
