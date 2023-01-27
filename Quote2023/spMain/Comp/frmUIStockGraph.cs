using System;
using System.Windows.Forms;
using spMain.QData.UI;

namespace spMain.Comp
{
    public partial class frmUIStockGraph : Form
    {
        private UIGraph _initialGraph;
        public frmUIStockGraph()
        {
            InitializeComponent();
        }

        public frmUIStockGraph(UIGraph graph):base()
        {
            InitializeComponent();
            _initialGraph = graph;
        }

        private void frmUIStockGraph_Load(object sender, EventArgs e)
        {
            if (!csIni.isDesignMode)
            {
                if (_initialGraph != null)
                {
                    uI_StockGraph1._SetUIGraph(_initialGraph);
                    _initialGraph = null;
                    return;
                }

                var o = cs.PGfrmObjectEditor._GetObject(new spMain.QData.UI.UIGraph(), QData.UI.UIGraph._serializationFileName, true);
                if (o == null)
                    BeginInvoke(new MethodInvoker(Close));
                else
                    uI_StockGraph1._SetUIGraph((QData.UI.UIGraph) o);
            }
        }
    }
}