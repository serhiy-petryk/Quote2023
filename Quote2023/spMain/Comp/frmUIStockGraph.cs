using System;
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
                    if (!_initialGraph.DataAdapter.IsStream)
                        uI_StockGraph1._Autosize();
                    _initialGraph = null;
                    return;
                }

                var o = cs.PGfrmObjectEditor._GetObject(new spMain.QData.UI.UIGraph(), QData.UI.UIGraph._serializationFileName, true);
                if (o == null)
                    BeginInvoke(new MethodInvoker(Close));
                else
                {
                    uI_StockGraph1._SetUIGraph((QData.UI.UIGraph)o, _isSnapshotLayout);
                    uI_StockGraph1._Autosize();
                }
            }
        }
    }
}