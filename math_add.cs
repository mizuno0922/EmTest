using Grasshopper.Kernel;
using System;
using CsBindgen;
public class RustAddComponent : GH_Component
{
    public RustAddComponent() : base("RustAdd", "RustAdd",
      "Adds two numbers using a Rust library", "Category", "Subcategory")
    {
    }

    protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
    {
        pManager.AddNumberParameter("A", "A", "First number", GH_ParamAccess.item);
        pManager.AddNumberParameter("B", "B", "Second number", GH_ParamAccess.item);
    }

    protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
    {
        pManager.AddNumberParameter("Result", "Result", "Addition result", GH_ParamAccess.item);
    }

    protected override void SolveInstance(IGH_DataAccess DA)
    {
        double a = 0;
        double b = 0;
        if (!DA.GetData(0, ref a) || !DA.GetData(1, ref b)) return;

        int result = NativeMethods.my_add((int)a, (int)b);
        DA.SetData(0, result);
    }

    public override Guid ComponentGuid => new Guid("F66009FD-B82D-4F8E-8843-C939A40DEF5F");
}
