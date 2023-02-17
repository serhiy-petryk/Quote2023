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
        private readonly bool _isSnapshotLayout = false;
        public frmUIStockGraph()
        {
            InitializeComponent();
        }

        public frmUIStockGraph(UIGraph graph, bool isSnapshotLayout) : this()
        {
            _initialGraph = graph;
            _isSnapshotLayout = isSnapshotLayout;
        }

        public Image _GetImage() => uI_StockGraph1._GetImage();
        public void _SetUIGraph(QData.UI.UIGraph uiGraph, bool isSnapshotLayout) => uI_StockGraph1._SetUIGraph(uiGraph, isSnapshotLayout);

        private void frmUIStockGraph_Load(object sender, EventArgs e)
        {
            if (!csIni.isDesignMode)
            {
                if (_initialGraph == null)
                    _initialGraph = cs.PGfrmObjectEditor._GetObject(new spMain.QData.UI.UIGraph(),
                        QData.UI.UIGraph._serializationFileName, true) as QData.UI.UIGraph;

                if (_initialGraph == null)
                    BeginInvoke(new MethodInvoker(Close));
                else
                    uI_StockGraph1._SetUIGraph(_initialGraph, _isSnapshotLayout);
            }
        }
    }
}