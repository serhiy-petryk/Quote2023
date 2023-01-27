using System;
using System.Windows.Forms;

namespace spMain.Comp
{
  public partial class frmUIStockGraph : Form
  {
    public frmUIStockGraph()
    {
      InitializeComponent();
    }

    private void frmUIStockGraph_Load(object sender, EventArgs e)
    {
      if (!csIni.isDesignMode)
      {
        var x = cs.PGfrmObjectEditor._GetObject(new spMain.QData.UI.UIGraph(), QData.UI.UIGraph._serializationFileName, true);
        if (x == null)
          BeginInvoke(new MethodInvoker(Close));
        else
          uI_StockGraph1._SetUIGraph((QData.UI.UIGraph)x);
      }
    }
  }
}