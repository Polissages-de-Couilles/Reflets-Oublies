using Unity.VisualScripting;
using UnityEngine;

namespace AfterimageSample
{
    public class AfterImage
    {
        RenderParams[] _params;
        Mesh[] _meshes;
        Matrix4x4[] _matrices;
        public int MaxFrameCount { get; set; }

        /// <summary>
        /// 描画された回数.
        /// </summary>
        public int FrameCount { get; private set; }

        /// <summary>
        /// コンストラクタ.
        /// </summary>
        /// <param name="meshCount">描画するメッシュの数.</param>
        public AfterImage(int meshCount)
        {
            _params = new RenderParams[meshCount];
            _meshes = new Mesh[meshCount];
            _matrices = new Matrix4x4[meshCount];
            Reset();
        }

        /// <summary>
        /// 描画前もしくは後に実行する.
        /// </summary>
        public void Reset()
        {
            FrameCount = 0;
        }

        /// <summary>
        /// メッシュごとに使用するマテリアルを用意し、現在のメッシュの形状を記憶させる.
        /// </summary>
        /// <param name="material">使用するマテリアル. </param>
        /// <param name="layer">描画するレイヤー.</param>
        /// <param name="renderers">記憶させるSkinnedMeshRendereの配列.</param>
        public void Setup(Material material, int layer, Component[] renderers, Vector3 customScale)
        {
            for (int i = 0; i < renderers.Length; i++)
            {
                // マテリアルにnullが渡されたらオブジェクトのマテリアルをそのまま使う.
                if (material == null)
                {
                    if (renderers[i] is Renderer r)
                        material = r.sharedMaterial;
                }
                if (_params[i].material != material)
                {
                    _params[i] = new RenderParams(material);
                }
                // レイヤーを設定する.
                if (_params[i].layer != layer)
                {
                    _params[i].layer = layer;
                }
                // 現在のメッシュの状態を格納する.
                if (_meshes[i] == null)
                {
                    _meshes[i] = new Mesh();
                }

                if (renderers[i] is SkinnedMeshRenderer smr)
                {
                    smr.BakeMesh(_meshes[i]);
                }
                else if (renderers[i] is MeshRenderer)
                {
                    MeshFilter mf = renderers[i].GetComponent<MeshFilter>();
                    if (mf)
                    {
                        // Copy the mesh if you need to modify it separately.
                        _meshes[i] = Object.Instantiate(mf.sharedMesh);
                    }
                }

                var transform = renderers[i].transform;
                var scaleMatrix = Matrix4x4.Scale(customScale);
                var matrix = transform.localToWorldMatrix;
                _matrices[i] = matrix * scaleMatrix;
            }
        }

        /// <summary>
        /// 記憶したメッシュを全て描画する.
        /// </summary>
        public void RenderMeshes()
        {
            //float alpha = 1f;
            //if (MaxFrameCount > 0)
            //    alpha = Mathf.Clamp01(1f - (float)FrameCount / MaxFrameCount);

            for (int i = 0; i < _meshes.Length; i++)
            {
                //Debug.Log("COLOR " + _params[i].material.GetColor("_BaseColor"));
                //_params[i].material.SetColor("_BaseColor", _params[i].material.GetColor("_BaseColor").WithAlpha(alpha));
                //_params[i].material.SetColor("_EdgeColor", _params[i].material.GetColor("_EdgeColor").WithAlpha(alpha));
                Graphics.RenderMesh(_params[i], _meshes[i], 0, _matrices[i]);
            }

            FrameCount++;
        }
    }
}
