using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("alignment", "renderMode", "meshDistribution", "sortMode", "lengthScale", "velocityScale", "cameraVelocityScale", "normalDirection", "shadowBias", "sortingFudge", "minParticleSize", "maxParticleSize", "pivot", "flip", "maskInteraction", "trailMaterial", "enableGPUInstancing", "allowRoll", "freeformStretching", "rotateWithStretchDirection", "mesh", "editorEnabled", "bounds", "localBounds", "enabled", "shadowCastingMode", "receiveShadows", "forceRenderingOff", "staticShadowCaster", "motionVectorGenerationMode", "lightProbeUsage", "reflectionProbeUsage", "renderingLayerMask", "rendererPriority", "rayTracingMode", "sortingLayerName", "sortingLayerID", "sortingOrder", "sortingGroupID", "sortingGroupOrder", "allowOcclusionWhenDynamic", "staticBatchRootTransform", "lightProbeProxyVolumeOverride", "probeAnchor", "lightmapIndex", "realtimeLightmapIndex", "lightmapScaleOffset", "realtimeLightmapScaleOffset", "materials", "material", "sharedMaterial", "sharedMaterials", "name")]
	public class ES3UserType_ParticleSystemRenderer : ES3ComponentType
	{
		public static ES3Type Instance = null;

		public ES3UserType_ParticleSystemRenderer() : base(typeof(UnityEngine.ParticleSystemRenderer)){ Instance = this; priority = 1;}


		protected override void WriteComponent(object obj, ES3Writer writer)
		{
			var instance = (UnityEngine.ParticleSystemRenderer)obj;
			
			writer.WriteProperty("alignment", instance.alignment, ES3Internal.ES3TypeMgr.GetOrCreateES3Type(typeof(UnityEngine.ParticleSystemRenderSpace)));
			writer.WriteProperty("renderMode", instance.renderMode, ES3Internal.ES3TypeMgr.GetOrCreateES3Type(typeof(UnityEngine.ParticleSystemRenderMode)));
			writer.WriteProperty("meshDistribution", instance.meshDistribution, ES3Internal.ES3TypeMgr.GetOrCreateES3Type(typeof(UnityEngine.ParticleSystemMeshDistribution)));
			writer.WriteProperty("sortMode", instance.sortMode, ES3Internal.ES3TypeMgr.GetOrCreateES3Type(typeof(UnityEngine.ParticleSystemSortMode)));
			writer.WriteProperty("lengthScale", instance.lengthScale, ES3Type_float.Instance);
			writer.WriteProperty("velocityScale", instance.velocityScale, ES3Type_float.Instance);
			writer.WriteProperty("cameraVelocityScale", instance.cameraVelocityScale, ES3Type_float.Instance);
			writer.WriteProperty("normalDirection", instance.normalDirection, ES3Type_float.Instance);
			writer.WriteProperty("shadowBias", instance.shadowBias, ES3Type_float.Instance);
			writer.WriteProperty("sortingFudge", instance.sortingFudge, ES3Type_float.Instance);
			writer.WriteProperty("minParticleSize", instance.minParticleSize, ES3Type_float.Instance);
			writer.WriteProperty("maxParticleSize", instance.maxParticleSize, ES3Type_float.Instance);
			writer.WriteProperty("pivot", instance.pivot, ES3Type_Vector3.Instance);
			writer.WriteProperty("flip", instance.flip, ES3Type_Vector3.Instance);
			writer.WriteProperty("maskInteraction", instance.maskInteraction, ES3Internal.ES3TypeMgr.GetOrCreateES3Type(typeof(UnityEngine.SpriteMaskInteraction)));
			writer.WritePropertyByRef("trailMaterial", instance.trailMaterial);
			writer.WriteProperty("enableGPUInstancing", instance.enableGPUInstancing, ES3Type_bool.Instance);
			writer.WriteProperty("allowRoll", instance.allowRoll, ES3Type_bool.Instance);
			writer.WriteProperty("freeformStretching", instance.freeformStretching, ES3Type_bool.Instance);
			writer.WriteProperty("rotateWithStretchDirection", instance.rotateWithStretchDirection, ES3Type_bool.Instance);
			writer.WritePropertyByRef("mesh", instance.mesh);
			writer.WritePrivateProperty("editorEnabled", instance);
			writer.WriteProperty("bounds", instance.bounds, ES3Type_Bounds.Instance);
			writer.WriteProperty("localBounds", instance.localBounds, ES3Type_Bounds.Instance);
			writer.WriteProperty("enabled", instance.enabled, ES3Type_bool.Instance);
			writer.WriteProperty("shadowCastingMode", instance.shadowCastingMode, ES3Type_enum.Instance);
			writer.WriteProperty("receiveShadows", instance.receiveShadows, ES3Type_bool.Instance);
			writer.WriteProperty("forceRenderingOff", instance.forceRenderingOff, ES3Type_bool.Instance);
			writer.WriteProperty("staticShadowCaster", instance.staticShadowCaster, ES3Type_bool.Instance);
			writer.WriteProperty("motionVectorGenerationMode", instance.motionVectorGenerationMode, ES3Type_enum.Instance);
			writer.WriteProperty("lightProbeUsage", instance.lightProbeUsage, ES3Type_enum.Instance);
			writer.WriteProperty("reflectionProbeUsage", instance.reflectionProbeUsage, ES3Type_enum.Instance);
			writer.WriteProperty("renderingLayerMask", instance.renderingLayerMask, ES3Type_uint.Instance);
			writer.WriteProperty("rendererPriority", instance.rendererPriority, ES3Type_int.Instance);
			writer.WriteProperty("rayTracingMode", instance.rayTracingMode, ES3Internal.ES3TypeMgr.GetOrCreateES3Type(typeof(UnityEngine.Experimental.Rendering.RayTracingMode)));
			writer.WriteProperty("sortingLayerName", instance.sortingLayerName, ES3Type_string.Instance);
			writer.WriteProperty("sortingLayerID", instance.sortingLayerID, ES3Type_int.Instance);
			writer.WriteProperty("sortingOrder", instance.sortingOrder, ES3Type_int.Instance);
			writer.WritePrivateProperty("sortingGroupID", instance);
			writer.WritePrivateProperty("sortingGroupOrder", instance);
			writer.WriteProperty("allowOcclusionWhenDynamic", instance.allowOcclusionWhenDynamic, ES3Type_bool.Instance);
			writer.WritePrivatePropertyByRef("staticBatchRootTransform", instance);
			writer.WritePropertyByRef("lightProbeProxyVolumeOverride", instance.lightProbeProxyVolumeOverride);
			writer.WritePropertyByRef("probeAnchor", instance.probeAnchor);
			writer.WriteProperty("lightmapIndex", instance.lightmapIndex, ES3Type_int.Instance);
			writer.WriteProperty("realtimeLightmapIndex", instance.realtimeLightmapIndex, ES3Type_int.Instance);
			writer.WriteProperty("lightmapScaleOffset", instance.lightmapScaleOffset, ES3Type_Vector4.Instance);
			writer.WriteProperty("realtimeLightmapScaleOffset", instance.realtimeLightmapScaleOffset, ES3Type_Vector4.Instance);
			writer.WriteProperty("materials", instance.materials, ES3Type_MaterialArray.Instance);
			writer.WritePropertyByRef("material", instance.material);
			writer.WritePropertyByRef("sharedMaterial", instance.sharedMaterial);
			writer.WriteProperty("sharedMaterials", instance.sharedMaterials, ES3Type_MaterialArray.Instance);
		}

		protected override void ReadComponent<T>(ES3Reader reader, object obj)
		{
			var instance = (UnityEngine.ParticleSystemRenderer)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "alignment":
						instance.alignment = reader.Read<UnityEngine.ParticleSystemRenderSpace>();
						break;
					case "renderMode":
						instance.renderMode = reader.Read<UnityEngine.ParticleSystemRenderMode>();
						break;
					case "meshDistribution":
						instance.meshDistribution = reader.Read<UnityEngine.ParticleSystemMeshDistribution>();
						break;
					case "sortMode":
						instance.sortMode = reader.Read<UnityEngine.ParticleSystemSortMode>();
						break;
					case "lengthScale":
						instance.lengthScale = reader.Read<System.Single>(ES3Type_float.Instance);
						break;
					case "velocityScale":
						instance.velocityScale = reader.Read<System.Single>(ES3Type_float.Instance);
						break;
					case "cameraVelocityScale":
						instance.cameraVelocityScale = reader.Read<System.Single>(ES3Type_float.Instance);
						break;
					case "normalDirection":
						instance.normalDirection = reader.Read<System.Single>(ES3Type_float.Instance);
						break;
					case "shadowBias":
						instance.shadowBias = reader.Read<System.Single>(ES3Type_float.Instance);
						break;
					case "sortingFudge":
						instance.sortingFudge = reader.Read<System.Single>(ES3Type_float.Instance);
						break;
					case "minParticleSize":
						instance.minParticleSize = reader.Read<System.Single>(ES3Type_float.Instance);
						break;
					case "maxParticleSize":
						instance.maxParticleSize = reader.Read<System.Single>(ES3Type_float.Instance);
						break;
					case "pivot":
						instance.pivot = reader.Read<UnityEngine.Vector3>(ES3Type_Vector3.Instance);
						break;
					case "flip":
						instance.flip = reader.Read<UnityEngine.Vector3>(ES3Type_Vector3.Instance);
						break;
					case "maskInteraction":
						instance.maskInteraction = reader.Read<UnityEngine.SpriteMaskInteraction>();
						break;
					case "trailMaterial":
						instance.trailMaterial = reader.Read<UnityEngine.Material>(ES3Type_Material.Instance);
						break;
					case "enableGPUInstancing":
						instance.enableGPUInstancing = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "allowRoll":
						instance.allowRoll = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "freeformStretching":
						instance.freeformStretching = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "rotateWithStretchDirection":
						instance.rotateWithStretchDirection = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "mesh":
						instance.mesh = reader.Read<UnityEngine.Mesh>(ES3Type_Mesh.Instance);
						break;
					case "editorEnabled":
					instance = (UnityEngine.ParticleSystemRenderer)reader.SetPrivateProperty("editorEnabled", reader.Read<System.Boolean>(), instance);
					break;
					case "bounds":
						instance.bounds = reader.Read<UnityEngine.Bounds>(ES3Type_Bounds.Instance);
						break;
					case "localBounds":
						instance.localBounds = reader.Read<UnityEngine.Bounds>(ES3Type_Bounds.Instance);
						break;
					case "enabled":
						instance.enabled = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "shadowCastingMode":
						instance.shadowCastingMode = reader.Read<UnityEngine.Rendering.ShadowCastingMode>(ES3Type_enum.Instance);
						break;
					case "receiveShadows":
						instance.receiveShadows = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "forceRenderingOff":
						instance.forceRenderingOff = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "staticShadowCaster":
						instance.staticShadowCaster = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "motionVectorGenerationMode":
						instance.motionVectorGenerationMode = reader.Read<UnityEngine.MotionVectorGenerationMode>(ES3Type_enum.Instance);
						break;
					case "lightProbeUsage":
						instance.lightProbeUsage = reader.Read<UnityEngine.Rendering.LightProbeUsage>(ES3Type_enum.Instance);
						break;
					case "reflectionProbeUsage":
						instance.reflectionProbeUsage = reader.Read<UnityEngine.Rendering.ReflectionProbeUsage>(ES3Type_enum.Instance);
						break;
					case "renderingLayerMask":
						instance.renderingLayerMask = reader.Read<System.UInt32>(ES3Type_uint.Instance);
						break;
					case "rendererPriority":
						instance.rendererPriority = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "rayTracingMode":
						instance.rayTracingMode = reader.Read<UnityEngine.Experimental.Rendering.RayTracingMode>();
						break;
					case "sortingLayerName":
						instance.sortingLayerName = reader.Read<System.String>(ES3Type_string.Instance);
						break;
					case "sortingLayerID":
						instance.sortingLayerID = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "sortingOrder":
						instance.sortingOrder = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "sortingGroupID":
					instance = (UnityEngine.ParticleSystemRenderer)reader.SetPrivateProperty("sortingGroupID", reader.Read<System.Int32>(), instance);
					break;
					case "sortingGroupOrder":
					instance = (UnityEngine.ParticleSystemRenderer)reader.SetPrivateProperty("sortingGroupOrder", reader.Read<System.Int32>(), instance);
					break;
					case "allowOcclusionWhenDynamic":
						instance.allowOcclusionWhenDynamic = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "staticBatchRootTransform":
					instance = (UnityEngine.ParticleSystemRenderer)reader.SetPrivateProperty("staticBatchRootTransform", reader.Read<UnityEngine.Transform>(), instance);
					break;
					case "lightProbeProxyVolumeOverride":
						instance.lightProbeProxyVolumeOverride = reader.Read<UnityEngine.GameObject>(ES3Type_GameObject.Instance);
						break;
					case "probeAnchor":
						instance.probeAnchor = reader.Read<UnityEngine.Transform>(ES3Type_Transform.Instance);
						break;
					case "lightmapIndex":
						instance.lightmapIndex = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "realtimeLightmapIndex":
						instance.realtimeLightmapIndex = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "lightmapScaleOffset":
						instance.lightmapScaleOffset = reader.Read<UnityEngine.Vector4>(ES3Type_Vector4.Instance);
						break;
					case "realtimeLightmapScaleOffset":
						instance.realtimeLightmapScaleOffset = reader.Read<UnityEngine.Vector4>(ES3Type_Vector4.Instance);
						break;
					case "materials":
						instance.materials = reader.Read<UnityEngine.Material[]>(ES3Type_MaterialArray.Instance);
						break;
					case "material":
						instance.material = reader.Read<UnityEngine.Material>(ES3Type_Material.Instance);
						break;
					case "sharedMaterial":
						instance.sharedMaterial = reader.Read<UnityEngine.Material>(ES3Type_Material.Instance);
						break;
					case "sharedMaterials":
						instance.sharedMaterials = reader.Read<UnityEngine.Material[]>(ES3Type_MaterialArray.Instance);
						break;
					default:
						reader.Skip();
						break;
				}
			}
		}
	}


	public class ES3UserType_ParticleSystemRendererArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_ParticleSystemRendererArray() : base(typeof(UnityEngine.ParticleSystemRenderer[]), ES3UserType_ParticleSystemRenderer.Instance)
		{
			Instance = this;
		}
	}
}