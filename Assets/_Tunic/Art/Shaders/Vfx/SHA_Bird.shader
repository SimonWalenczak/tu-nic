// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "SHA_Bird"
{
	Properties
	{
		_Cutoff( "Mask Clip Value", Float ) = 0.5
		_Line_Thickness("Line_Thickness", Float) = 5.58
		_Line_Power("Line_Power", Float) = 3.66
		_OpacityTexture("Opacity Texture", 2D) = "white" {}
		_Speed("Speed", Float) = 11.8
		_Ampliftude("Ampliftude", Float) = 0.58
		_Color0("MainColor", Color) = (0,0,0,0)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "TransparentCutout"  "Queue" = "Transparent+0" }
		Cull Off
		Blend SrcAlpha OneMinusSrcAlpha
		
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows vertex:vertexDataFunc 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform float _Speed;
		uniform float _Ampliftude;
		uniform float _Line_Thickness;
		uniform float _Line_Power;
		uniform float4 _Color0;
		uniform sampler2D _OpacityTexture;
		uniform float4 _OpacityTexture_ST;
		uniform float _Cutoff = 0.5;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float3 ase_vertex3Pos = v.vertex.xyz;
			float3 break16 = ( ase_vertex3Pos * 1 );
			float temp_output_34_0 = ( sin( ( _Time.y * _Speed ) ) * _Ampliftude );
			float4 appendResult17 = (float4(break16.x , ( ( break16.y * temp_output_34_0 ) * pow( ( pow( v.texcoord.xy.x , _Line_Thickness ) + pow( ( 1.0 - v.texcoord.xy.x ) , _Line_Thickness ) ) , _Line_Power ) ) , break16.z , 0.0));
			float4 transform45 = mul(unity_WorldToObject,appendResult17);
			v.vertex.xyz = transform45.xyz;
			v.vertex.w = 1;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			o.Albedo = _Color0.rgb;
			o.Alpha = 1;
			float2 uv_OpacityTexture = i.uv_texcoord * _OpacityTexture_ST.xy + _OpacityTexture_ST.zw;
			clip( tex2D( _OpacityTexture, uv_OpacityTexture ).r - _Cutoff );
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18935
0;0;1920;1019;871.339;213.7196;1;True;False
Node;AmplifyShaderEditor.TextureCoordinatesNode;2;-2065.343,1316.245;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.BreakToComponentsNode;3;-1842.343,1316.245;Inherit;False;FLOAT2;1;0;FLOAT2;0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.SimpleTimeNode;20;-855.1037,402.231;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;21;-862.0106,575.5051;Inherit;False;Property;_Speed;Speed;4;0;Create;True;0;0;0;False;0;False;11.8;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PosVertexDataNode;15;-932.1,151.3;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;4;-1620.343,1387.245;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;7;-1788.343,1130.245;Inherit;False;Property;_Line_Thickness;Line_Thickness;1;0;Create;True;0;0;0;False;0;False;5.58;5.58;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;22;-685.3266,411.2243;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;5;-1386.343,1316.245;Inherit;True;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ScaleNode;36;-679.0198,164.5803;Inherit;False;1;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.PowerNode;1;-1434.342,1072.245;Inherit;True;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SinOpNode;19;-439.1268,441.3254;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;33;-484.3832,674.0588;Inherit;False;Property;_Ampliftude;Ampliftude;5;0;Create;True;0;0;0;False;0;False;0.58;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;9;-882.2514,1414.62;Inherit;False;Property;_Line_Power;Line_Power;2;0;Create;True;0;0;0;False;0;False;3.66;3.66;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;8;-833.2514,1051.62;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.BreakToComponentsNode;16;-422.7,164.2;Inherit;True;FLOAT3;1;0;FLOAT3;0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;34;-169.8577,605.0237;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;10;-540.6873,1208.603;Inherit;True;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;43;182.8274,421.1716;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;38;514.9802,449.5803;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;17;812.1001,207.3;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.WorldToObjectTransfNode;45;965.661,214.2804;Inherit;False;1;0;FLOAT4;0,0,0,1;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;44;739.2267,-280.8283;Inherit;False;Property;_Color0;MainColor;6;0;Create;True;0;0;0;False;0;False;0,0,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;11;328.3,-108.3;Inherit;True;Property;_OpacityTexture;Opacity Texture;3;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;27;290.5186,341.047;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1167,-85;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;SHA_Bird;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;True;0;True;TransparentCutout;;Transparent;All;18;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;True;Absolute;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;3;0;2;0
WireConnection;4;0;3;0
WireConnection;22;0;20;0
WireConnection;22;1;21;0
WireConnection;5;0;4;0
WireConnection;5;1;7;0
WireConnection;36;0;15;0
WireConnection;1;0;3;0
WireConnection;1;1;7;0
WireConnection;19;0;22;0
WireConnection;8;0;1;0
WireConnection;8;1;5;0
WireConnection;16;0;36;0
WireConnection;34;0;19;0
WireConnection;34;1;33;0
WireConnection;10;0;8;0
WireConnection;10;1;9;0
WireConnection;43;0;16;1
WireConnection;43;1;34;0
WireConnection;38;0;43;0
WireConnection;38;1;10;0
WireConnection;17;0;16;0
WireConnection;17;1;38;0
WireConnection;17;2;16;2
WireConnection;45;0;17;0
WireConnection;27;0;16;1
WireConnection;27;1;34;0
WireConnection;0;0;44;0
WireConnection;0;10;11;0
WireConnection;0;11;45;0
ASEEND*/
//CHKSM=5E5FE1828D05056D6DD2E78D35B381A86CC998FE