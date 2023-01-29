using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using spMain.QData.UI;

namespace spMain.Comp
{
    public partial class frmUIStockGraph : Form
    {
        private UIGraph _initialGraph;
        private bool _isSnapshotLayout;
        public frmUIStockGraph()
        {
            InitializeComponent();
        }

        public frmUIStockGraph(UIGraph graph, bool isSnapshotLayout) : base()
        {
            InitializeComponent();
            _initialGraph = graph;
            _isSnapshotLayout = isSnapshotLayout;
        }

        private void frmUIStockGraph_Load(object sender, EventArgs e)
        {
            if (!csIni.isDesignMode)
            {
                if (_initialGraph != null)
                {
                    uI_StockGraph1._SetUIGraph(_initialGraph, _isSnapshotLayout);
                    _initialGraph = null;
                    if (_isSnapshotLayout)
                    {
                        Control stockGraphControl = null;
                        foreach (var c in uI_StockGraph1.Controls)
                            if (c is StockGraph graph)
                                stockGraphControl = graph;

                        if (stockGraphControl != null)
                        {
                            stockGraphControl.Dock = DockStyle.None;
                            stockGraphControl.Size = new Size(90, 60);
                            uI_StockGraph1._CopyToClipboard();
                            if (Clipboard.ContainsImage())
                                Clipboard.GetImage().Save(@"E:\Temp\test.png", ImageFormat.Png);

                            // BeginInvoke(new MethodInvoker(Close));
                        }
                    }
                    return;
                }

                var o = cs.PGfrmObjectEditor._GetObject(new spMain.QData.UI.UIGraph(), QData.UI.UIGraph._serializationFileName, true);
                if (o == null)
                    BeginInvoke(new MethodInvoker(Close));
                else
                    uI_StockGraph1._SetUIGraph((QData.UI.UIGraph) o, _isSnapshotLayout);
            }
        }
    }
}