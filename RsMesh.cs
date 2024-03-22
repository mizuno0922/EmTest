using Rhino.Geometry;
using System;
using System.Runtime.InteropServices;
using CsBindgen; 

namespace RsMesh
{
    public class RustMeshToRhino
    {
        // メッシュデータをRhinoのメッシュに変換するメソッド
        public static Mesh ToRhinoGeometry(IntPtr meshPtr)
        {
            Mesh rhinoMesh = new Mesh();

            // 頂点数と面数を取得
            int vertexCount = NativeMethods.get_vertex_count(meshPtr);
            int faceCount = NativeMethods.get_face_count(meshPtr) * 3; // 1つの面は3つのインデックス

            unsafe
            {
                // 頂点データの取得
                float* verticesPtr = NativeMethods.get_vertices(meshPtr);
                float[] vertices = new float[vertexCount * 3]; // X, Y, Zの3要素ごとに1頂点
                Marshal.Copy((IntPtr)verticesPtr, vertices, 0, vertices.Length);
                NativeMethods.free_vertices((IntPtr)verticesPtr); // Rust側で確保されたメモリを解放

                // 面データの取得
                int* facesPtr = NativeMethods.get_faces(meshPtr);
                int[] faces = new int[faceCount]; // 各面のインデックス
                Marshal.Copy((IntPtr)facesPtr, faces, 0, faces.Length);
                NativeMethods.free_faces((IntPtr)facesPtr); // Rust側で確保されたメモリを解放

                // 頂点データの追加
                for (int i = 0; i < vertices.Length; i += 3)
                {
                    rhinoMesh.Vertices.Add(vertices[i], vertices[i + 1], vertices[i + 2]);
                }

                // 面データの追加
                for (int i = 0; i < faces.Length; i += 3)
                {
                    rhinoMesh.Faces.AddFace(faces[i], faces[i + 1], faces[i + 2]);
                }
            }

            rhinoMesh.Normals.ComputeNormals();
            rhinoMesh.UnifyNormals();

            // Rust側で確保されたメモリを解放
            NativeMethods.free_polygon_mesh(meshPtr);

            return rhinoMesh;
        }
    }
}