using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class CircleWipePass : ScriptableRenderPass
{
    const string ProfilerTag = "Circle Wipe Pass";


    // We will store our pass settings in this variable.
    CircleWipeFeature.PassSettings passSettings;

    RTHandle source;
    RTHandle destination;
    RTHandle temp;

    Material material;

    // It is good to cache the shader property IDs here.
    static readonly int CompareValue = Shader.PropertyToID("_CompareValue");

    public CircleWipePass(CircleWipeFeature.PassSettings passSettings) : base() {
        Debug.Log("Circle Wipe Pass"); 
        this.passSettings = passSettings;

        
        // Set the render pass event.
        renderPassEvent = passSettings.renderPassEvent;

        // We create a material that will be used during our pass. You can do it like this using the 'CreateEngineMaterial' method, giving it
        // a shader path as an input or you can use a 'public Material material;' field in your pass settings and access it here through 'passSettings.material'.
        material = passSettings.material;

        // Set any material properties based on our pass settings.
        //material.SetFloat(CompareValue, passSettings.wipeAmount);
    }


    public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData) {
        RenderTextureDescriptor desc = renderingData.cameraData.cameraTargetDescriptor;
        desc.depthBufferBits = 0;

        RenderingUtils.ReAllocateIfNeeded(ref temp, desc, name: "_TemporaryColorTexture");

        var renderer = renderingData.cameraData.renderer;

        source = renderer.cameraColorTargetHandle;
        destination = renderer.cameraColorTargetHandle;
    }

    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData) {
        if (renderingData.cameraData.cameraType == CameraType.Preview) return;
        // Grab a command buffer. We put the actual execution of the pass inside of a profiling scope.
        CommandBuffer cmd = CommandBufferPool.Get(ProfilerTag);


        Blitter.BlitCameraTexture(cmd, source, temp, material, 0);
        Blitter.BlitCameraTexture(cmd, temp, destination, Vector2.one);

        // Execute the command buffer and release it.
        context.ExecuteCommandBuffer(cmd);
        CommandBufferPool.Release(cmd);
    }

    // Called when the camera has finished rendering.
    // Here we release/cleanup any allocated resources that were created by this pass.
    // Gets called for all cameras i na camera stack.
    public override void OnCameraCleanup(CommandBuffer cmd) {
        source = null;
        destination = null;

        if (cmd == null) throw new ArgumentNullException("cmd");

    }

    public void Dispose() {
        temp?.Release();

    }
}
