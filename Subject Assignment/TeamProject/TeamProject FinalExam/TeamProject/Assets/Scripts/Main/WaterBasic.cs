using System;
using UnityEngine;

namespace UnityStandardAssets.Water
{
    [ExecuteInEditMode]
    public class WaterBasic : MonoBehaviour
    {
        public float x_move= 1.0f;
        public float y_move= 1.0f;
        public float z_move= 1.0f;
        public float w_move= 1.0f;

        void Update()
        {

            Renderer r = GetComponent<Renderer>();
            if (!r)
            {
                return;
            }
            Material mat = r.sharedMaterial;
            if (!mat)
            {
                return;
            }

            Vector4 waveSpeed = mat.GetVector("WaveSpeed");
            float waveScale = mat.GetFloat("_WaveScale");
            float t = Time.time / 20.0f;

            Vector4 move = waveSpeed * (t * waveScale);
            Vector4 offsetClamped = new Vector4(Mathf.Repeat(move.x, x_move), Mathf.Repeat(move.y, y_move),
                Mathf.Repeat(move.z, z_move), Mathf.Repeat(move.w, w_move));
            mat.SetVector("_WaveOffset", offsetClamped);
        }
    }
}