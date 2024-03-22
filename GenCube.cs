using Rhino.Geometry;
using Grasshopper.Kernel;
using System;
using System.Runtime.InteropServices;
using RsMesh;
using CsBindgen;

namespace GenCube
{
    public class CreateCubeComponent : GH_Component
    {
        public CreateCubeComponent()
          : base("Create Rust Cube", "RustCube",
              "Creates a cube mesh using Rust",
              "Category", "Subcategory")
        {
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("B4F18990-F9B4-4FC2-AA42-B1EDB3443CB7"); }
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("Size", "S", "Size of the cube", GH_ParamAccess.item, 1.0);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddMeshParameter("Mesh", "M", "Output mesh", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            double size = 0;
            if (!DA.GetData(0, ref size))
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "No size input provided.");
                return; // 入力が提供されていない場合はここで処理を中断
            }

            if (size <= 0)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Size must be greater than 0.");
                return; // サイズが0以下の場合は処理を中断
            }

            // Rust関数を呼び出してメッシュデータを生成
            IntPtr meshPtr = NativeMethods.create_cube(size);
            if (meshPtr == IntPtr.Zero)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Failed to create cube from Rust");
                return;
            }

            // Rustで生成されたメッシュデータをRhinoのメッシュに変換
            Mesh rhinoMesh = RustMeshToRhino.ToRhinoGeometry(meshPtr);

            // 出力パラメータにセット
            DA.SetData(0, rhinoMesh);

            // 使用後にメモリを解放
            NativeMethods.free_polygon_mesh(meshPtr);
        }
    }
}
