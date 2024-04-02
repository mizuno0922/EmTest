using Rhino.Geometry;
using Grasshopper.Kernel;
using System;
using System.Runtime.InteropServices;
using RsPoint;
using CsBindgen;

namespace ConstructPoint3d
{
    public class CreatePoint3dComponent : GH_Component
    {
        public CreatePoint3dComponent()
          : base("Create Rust Point3d", "RustPoint3d",
              "Creates a point3d using Rust",
              "Category", "Subcategory")
        {
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("B1018161-0C6A-41C9-A3B4-A06AFFC38AAF"); }
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("X", "X", "", GH_ParamAccess.item);
            pManager.AddNumberParameter("Y", "Y", "", GH_ParamAccess.item);
            pManager.AddNumberParameter("Z", "Z", "", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddPointParameter("Point3d", "RsPoint", "Output Point3d", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            double x = 0, y = 0, z = 0;
            //IntPtr pptr = NativeMethods.construct_point3(x,y,z);
            if (!DA.GetData(0, ref x) || !DA.GetData(1, ref y) || !DA.GetData(2, ref z))
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid input");
                return;
            }

            IntPtr pptr = NativeMethods.construct_point3(x,y,z);
            if (pptr == IntPtr.Zero)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Failed to create cube from Rust");
                return;
            }

            Point3d point3 = RsPointtoRhino.ToRhinoPoint(pptr);
            // 出力パラメータにセット
            DA.SetData(0, point3);

            // 使用後にメモリを解放
            NativeMethods.point3_free(pptr);
        }
    }
}