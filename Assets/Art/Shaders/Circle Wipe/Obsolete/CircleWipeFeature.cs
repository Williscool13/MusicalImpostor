using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CircleWipeFeature : ScriptableRendererFeature
{
    [Serializable]
    public class PassSettings 
    {
        public RenderPassEvent renderPassEvent = RenderPassEvent.AfterRenderingOpaques;

        // Circle Wipe specific variable
        [Range(0, 1)] public float wipeAmount = 0.5f;

        public Material material;
    }

    CircleWipePass pass;
    public PassSettings passSettings = new();

    public override void Create() {
        // Pass the settings as a parameter to the constructor of the pass.
        pass = new CircleWipePass(passSettings);
        
    }
    // Injects one or multiple render passes in the renderer.
    // Gets called when setting up the renderer, once per-camera.
    // Gets called every frame, once per-camera.
    // Will not be called if the renderer feature is disabled in the renderer inspector.
    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData) {
        // Here you can queue up multiple passes after each other.
        renderer.EnqueuePass(pass);
    }

    protected override void Dispose(bool disposing) {
        pass.Dispose();
    }

}
