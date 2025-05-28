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
        /// �`�悳�ꂽ��.
        /// </summary>
        public int FrameCount { get; private set; }

        /// <summary>
        /// �R���X�g���N�^.
        /// </summary>
        /// <param name="meshCount">�`�悷�郁�b�V���̐�.</param>
        public AfterImage(int meshCount)
        {
            _params = new RenderParams[meshCount];
            _meshes = new Mesh[meshCount];
            _matrices = new Matrix4x4[meshCount];
            Reset();
        }

        /// <summary>
        /// �`��O�������͌�Ɏ��s����.
        /// </summary>
        public void Reset()
        {
            FrameCount = 0;
        }

        /// <summary>
        /// ���b�V�����ƂɎg�p����}�e���A����p�ӂ��A���݂̃��b�V���̌`����L��������.
        /// </summary>
        /// <param name="material">�g�p����}�e���A��. </param>
        /// <param name="layer">�`�悷�郌�C���[.</param>
        /// <param name="renderers">�L��������SkinnedMeshRendere�̔z��.</param>
        public void Setup(Material material, int layer, Component[] renderers, Vector3 customScale)
        {
            for (int i = 0; i < renderers.Length; i++)
            {
                // �}�e���A����null���n���ꂽ��I�u�W�F�N�g�̃}�e���A�������̂܂܎g��.
                if (material == null)
                {
                    if (renderers[i] is Renderer r)
                        material = r.sharedMaterial;
                }
                if (_params[i].material != material)
                {
                    _params[i] = new RenderParams(material);
                }
                // ���C���[��ݒ肷��.
                if (_params[i].layer != layer)
                {
                    _params[i].layer = layer;
                }
                // ���݂̃��b�V���̏�Ԃ��i�[����.
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
        /// �L���������b�V����S�ĕ`�悷��.
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
