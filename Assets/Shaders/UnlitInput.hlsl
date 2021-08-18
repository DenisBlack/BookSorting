#ifndef B1TH0VEN_UNLIT_INPUT_INCLUDED
#define B1TH0VEN_UNLIT_INPUT_INCLUDED

#include "Assets/ShaderLibrary/SurfaceInput.hlsl"

CBUFFER_START(UnityPerMaterial)
float4 _BaseMap_ST;
half4 _BaseColor;
half _Cutoff;
half _Glossiness;
half _Metallic;
CBUFFER_END

#endif
