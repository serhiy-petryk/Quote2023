using System;
using System.Drawing.Printing;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
//using System.Runtime.Serialization;
using ZedGraph;

namespace spMain.Comp {

  public partial class StockGraph : ZedGraphControl{

    VScrollBar _vScrollBar = null;
    HScrollBar _hScrollBar = null;

    public StockGraph()
      : this(null) {
    }

    public StockGraph(QData.UI.UIGraph uiGraph)
      : base() {
      if (!this.DesignMode ) {
        this._uiGraph = uiGraph;
        this.InitMy();

        this.IsShowPointValues = false; // Show Tooltips
        this.IsShowCursorValues = false;
        this.IsZoomOnMouseCenter = false;

        this.MouseMove += new MouseEventHandler(StockGraph_MouseMove);
        this.MouseMoveEvent += new ZedMouseEventHandler(StockGraph_MouseMoveEvent);

        //      this.MouseDown += new MouseEventHandler(StockGraph_MouseDown);
        this.MouseDownEvent += new ZedMouseEventHandler(StockGraph_MouseDownEvent);
        this.MouseUpEvent += new ZedMouseEventHandler(StockGraph_MouseUpEvent);

        this.MouseLeave += new EventHandler(StockGraph_MouseLeave);
        this.ScrollEvent += new ScrollEventHandler(StockGraph_ScrollEvent);
        this.ScrollDoneEvent += new ScrollDoneHandler(StockGraph_ScrollDoneEvent);
        this.ZoomEvent += new ZoomEventHandler(StockGraph_ZoomEvent);
        this.Resize += new EventHandler(StockGraph_Resize);
        foreach (Control c in this.Controls) {
          if (c is HScrollBar) {
            this._hScrollBar = (HScrollBar)c;
            this._hScrollBar.MouseWheel += new MouseEventHandler(sb_MouseWheel);
          }
          else if (c is VScrollBar) _vScrollBar = (VScrollBar)c;
        }
        this.ContextMenuBuilder += new ContextMenuBuilderEventHandler(StockGraph_ContextMenuBuilder);
        this.Disposed += new EventHandler(StockGraph_Disposed);
      }
    }

    void item_Click(object sender, EventArgs e) {
//      this.DoMyPrint();
      this._DoPrintPreview();
    }

    public void InitMy() {
      this.MasterPane.Title.IsVisible = true;
      this.MasterPane.Title.FontSpec.Family = "Tahoma";
      this.MasterPane.Title.FontSpec.IsBold = false;
      this.MasterPane.Title.FontSpec.Size = 12f;
      this.MasterPane.TitleGap = 0;

      this.MasterPane.Margin.All = 0;
      this.MasterPane.InnerPaneGap = 0;
      this.MasterPane.IsFontsScaled = false;
      this.MasterPane.Border.IsVisible = false;
      this.MasterPane.IsCommonScaleFactor = true;// Allign pane while printing

      // Scrolling
      this.IsShowHScrollBar = true;
      this.IsShowVScrollBar = false;
      //      this.IsAutoScrollRange = true;// false = плывет —кролЅар
      this.IsAutoScrollRange = false;// false = плывет —кролЅар
      this.IsEnableHPan = true;
      this.IsEnableHZoom = true;// Graph Zoom
      this.IsEnableVPan = true;
      this.IsEnableVZoom = false;
      this.IsSynchronizeXAxes = true;
      this.ScrollGrace = 0;
      this.ScrollMinX = 0;
      this.ScrollMaxX = 0;
      // End of scrolling
    }


    private void InitializeComponent() {
      this.SuspendLayout();
      // 
      // StockGraph
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.Name = "StockGraph";
      this.Size = new System.Drawing.Size(344, 265);
      this.ResumeLayout(false);

    }

  }
}
