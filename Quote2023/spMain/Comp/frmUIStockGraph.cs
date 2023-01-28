using System;
using System.Windows.Forms;
using spMain.QData.UI;

namespace spMain.Comp
{
    public partial class frmUIStockGraph : Form
    {
        private UIGraph _initialGraph;
        private bool _isSaveFileLayout;
        public frmUIStockGraph()
        {
            InitializeComponent();
        }

        public frmUIStockGraph(UIGraph graph, bool isSaveFileLayout) : base()
        {
            InitializeComponent();
            _initialGraph = graph;
            _isSaveFileLayout = isSaveFileLayout;
        }

        private void frmUIStockGraph_Load(object sender, EventArgs e)
        {
            if (!csIni.isDesignMode)
            {
                if (_initialGraph != null)
                {
                    uI_StockGraph1._SetUIGraph(_initialGraph, _isSaveFileLayout);
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
                    uI_StockGraph1._SetUIGraph((QData.UI.UIGraph)o, _isSaveFileLayout);
                    uI_StockGraph1._Autosize();
                }
            }
        }
    }
}