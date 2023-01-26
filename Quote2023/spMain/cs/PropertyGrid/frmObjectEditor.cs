using System;
using System.Collections;
using System.Reflection;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace spMain.cs {
  public partial class PGfrmObjectEditor : Form {

    //    static string collectionFN = csIni.pathObjectDB + "PaneSet.srl";
    public static void Test() {

      QData.UI.UIGraph xx1 = new spMain.QData.UI.UIGraph();
      object xx2 = _GetObject(xx1, null, true);
      if (xx2 != null) {
        QData.UI.UIGraph xx3 = (QData.UI.UIGraph)xx2;
        xx3.CreateDataSources();
        xx3.UpdateData(0);
        xx3.ClearDataSources();
      }
      return;


      TestPropertyGrid.Test.TestData x1 = new TestPropertyGrid.Test.TestData();

//      TestPropertyGrid.cs.Serialization.AppSettings x3 = new TestPropertyGrid.cs.Serialization.AppSettings();
  //    x3.Write("asd", x1);
    //  object x4 = x3.Read("asd");

      string collectionFN = @"c:\test.srl";

      object x2 = _GetObject(x1, collectionFN, true);
      //      object x2 = GetObject(x1, null);

      //      QData.Indicators.PaneSet gps = new spMain.QData.Indicators.PaneSet();
      //    object x = GetObject(gps, csIni.pathObjectDB + "PaneSet.srl");
      return;
    }


    /*    public static PaneSet GetNewPaneSet(PaneSet oldPaneSet) {
          if (oldPaneSet == null) oldPaneSet = new PaneSet();
          object x = frmObjectEditor.GetObject(oldPaneSet, collectionFN);
          if (x == null) return null;
          else return (PaneSet)x;
        }*/

    public static object _GetObject(object data, string collectionFileName, bool isFixedSizeCollection) {

      if (collectionFileName != null) {
        string folder = Path.GetDirectoryName(collectionFileName);
        if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);
      }
      using (PGfrmObjectEditor form = new PGfrmObjectEditor(data, collectionFileName, isFixedSizeCollection)) {
        form.ShowDialog();
        return form._data;
      }
    }


    public object _data;
    bool _isFixedSizeCollection = false;
    public object _dataOriginal;
    ArrayList _items;
    string _collectionFileName;
    object _gridView = null; //PropertyGridView: Control, IWin32Window, IWindowsFormsEditorService, IServiceProvider


    //==========================  Constructor ===========================
    public PGfrmObjectEditor(object pData, string collectionFileName, bool isFixedSizeCollection) {
      if (!this.DesignMode) {
        PGUtils.ApplyTypeConverter(pData);
        this._dataOriginal = pData;
        this._isFixedSizeCollection = isFixedSizeCollection;
        //      if (pData == null) throw new Exception("Parameter 'pData' can not be null in ObjEditorForm");
        this._collectionFileName = collectionFileName;
        if (this._dataOriginal != null && this._collectionFileName != null && !pData.GetType().IsSerializable) {
          throw new Exception(pData.GetType().FullName + " type must be serializable");
        }
        if (this._collectionFileName == null)
          _items = null;
        else {
          object o = csFastSerializer.Utils.File_ReadObject(this._collectionFileName);
          if (o == null) this._items = new ArrayList();
          else this._items = (ArrayList)o;

          /*        if (File.Exists(this._collectionFileName)) {
                    Stream stream = File.Open(this._collectionFileName, FileMode.Open);
                    BinaryFormatter bformatter = new BinaryFormatter();
                    _items = (ArrayList)bformatter.Deserialize(stream);
                    stream.Close();
                  }
                  else {
                    _items = new ArrayList();
                  }*/
        }
      }
      this.InitializeComponent();
      if (!this.DesignMode) {
        this.Init();
        pg_PropertySortChanged(null, null);
        this.pg.SelectedObject = pData;
        this.ShowStructure();
      }
    }

    void Init() {
      _data = null;
      SetMode(false);
      FieldInfo fi = typeof(PropertyGrid).GetField("gridView", BindingFlags.Instance | BindingFlags.NonPublic);
      _gridView = fi.GetValue(this.pg);

      if (this._collectionFileName == null) {
        this.splitContainer1.Panel1Collapsed = true;
        this.btnPG_Save.Enabled = false;
        //        this.splitContainer1.SplitterWidth = 1;
        //        this.splitContainer1.IsSplitterFixed=true;
      }
      else {
        this.btnPG_Save.Enabled = true;
        SetItems();
      }
    }

    void Init_PG() {// Called after root entry was created
      //      this.ShowStructure();
    }

    void SetItems() {
      this.listItems.Items.Clear();
      this.listItems.Items.AddRange(this._items.ToArray());
      this.listItem_SetButtons();
      if (this.listItems.Items.Count > 0) {
        this.listItems.SelectedItem = this.listItems.Items[0];
      }
    }

    //=======================  PropertyGrid Events ============================
    private void btnByCategory_Click(object sender, EventArgs e) {
      SetMode(false);
    }
    private void btnByAlpha_Click(object sender, EventArgs e) {
      SetMode(true);
    }
    void SetMode(bool sortByAlpha) {
      this.btnByAlpha.Checked = sortByAlpha;
      this.btnByCategory.Checked = !sortByAlpha;
      if (sortByAlpha) {
        this.pg.PropertySort = PropertySort.Alphabetical;
      }
      else {
        this.pg.PropertySort = PropertySort.Categorized;
        pg.SelectedObject = pg.SelectedObject;// Refresh select object
      }
    }

    private void btnPG_OK_Click(object sender, EventArgs e) {
      if (this.pg.SelectedObject is cs.IPG_ValidateSupport) {
        string s = ((cs.IPG_ValidateSupport)this.pg.SelectedObject).GetErrorDescription();
        if (!String.IsNullOrEmpty(s)) {
          MessageBox.Show(s, "Object error", MessageBoxButtons.OK, MessageBoxIcon.Error);
          return;
        }
      }
      this._data = this.pg.SelectedObject;
      this.Close();
    }
    private void btnPG_Cancel_Click(object sender, EventArgs e) {
      this.Close();
    }

    private void pg_PropertySortChanged(object sender, EventArgs e) {
      if (pg.PropertySort == PropertySort.CategorizedAlphabetical) pg.PropertySort = PropertySort.Categorized;
    }

    private void btnPG_ExpandAll_Click(object sender, EventArgs e) {
      this.pg.ExpandAllGridItems();
    }

    private void btnPG_CollapseAll_Click(object sender, EventArgs e) {
      this.pg.CollapseAllGridItems();
    }

    private void btnPG_ShowStructure_Click(object sender, EventArgs e) {
      this.ShowStructure();
    }


    bool initPGFlag = false;
    private void pg_Paint(object sender, PaintEventArgs e) {
      if (this.DesignMode) return;
      if (!initPGFlag) {// Not init yet
        FieldInfo fi = typeof(PropertyGrid).GetField("peMain", BindingFlags.Instance | BindingFlags.NonPublic);
        object rootGridEntry = fi.GetValue(this.pg);//(GridEntry)
        if (rootGridEntry != null) {// rootGridEntry exists == can call init procedure
          initPGFlag = true;
          this.pg.Paint -= new System.Windows.Forms.PaintEventHandler(this.pg_Paint);// detach this event
          this.Init_PG();
        }
      }
    }

    private void pg_SelectedGridItemChanged(object sender, SelectedGridItemChangedEventArgs e) {
      if (this.DesignMode) return;
      // Add/Remove collection item: set enabled/disabled
      if (pg.SelectedGridItem == null) {
        this.btnPG_AddItem.Enabled = false;
        this.btnPG_RemoveItem.Enabled = false;
        this.btnPG_ItemUp.Enabled = false;
        this.btnPG_ItemDown.Enabled = false;
        return;
      }

      GridItem giArrayParent;
      bool flagFixedSizeCollection = false;
      GridItem giArray = this.CollectionItem_GetEditableArrayGridItem(out giArrayParent, out flagFixedSizeCollection);
      this.btnPG_RemoveItem.Enabled = (giArray != null && pg.SelectedGridItem != giArray) && !flagFixedSizeCollection;
      this.btnPG_AddItem.Enabled = (giArray != null) && !flagFixedSizeCollection;
      bool flagUp = false;
      bool flagDown = false;
      if (giArray != null && pg.SelectedGridItem != giArray && (giArray.Value is IList || giArray.Value is IDictionary)) {
        flagUp = (giArray.GridItems.Count > 1) && GetArrayItemNo() != 0;
        flagDown = (giArray.GridItems.Count > 1) && (GetArrayItemNo() < giArray.GridItems.Count - 1);
      }
      if (this.btnPG_ItemUp.Enabled != flagUp) this.btnPG_ItemUp.Enabled = flagUp;
      if (this.btnPG_ItemDown.Enabled != flagDown) this.btnPG_ItemDown.Enabled = flagDown;
    }

    int GetArrayItemNo() {
      GridItem parent = pg.SelectedGridItem.Parent;
      for (int i = 0; i < parent.GridItems.Count; i++) {
        if (parent.GridItems[i] == pg.SelectedGridItem) return i;
      }
      return 0;
    }

    private void btnPG_Add_Click(object sender, EventArgs e) {
      this.CollectionItem_AddNew();
    }

    private void btnPG_Remove_Click(object sender, EventArgs e) {
      this.CollectionItem_Delete();
    }

    private void btnPG_Save_Click(object sender, EventArgs e) {
      string thisName = pg.SelectedObject.ToString();
      for (int i = 0; i < this._items.Count; i++) {
        if (this._items[i].ToString() == thisName) {
          if (MessageBox.Show("Item with name '" + thisName + "' already exists! Overwrite it?", "", MessageBoxButtons.YesNo,
            MessageBoxIcon.Question) == DialogResult.No) return;
          this._items.RemoveAt(i);
          break;
        }
      }
      this._items.Insert(0, csFastSerializer.Utils.GetClone(this.pg.SelectedObject));
      btnList_Save_Click(sender, e);// Save collection
      this.SetItems();
    }

    // ==========================  ListItems events =====================================
    private void btnList_Delete_Click(object sender, EventArgs e) {
      ListBox.SelectedObjectCollection selItems = this.listItems.SelectedItems;
      for (int i = 0; i < selItems.Count; i++) {
        this._items.Remove(selItems[i]);
      }
      this.SetItems();
    }

    private void btnList_Save_Click(object sender, EventArgs e) {
      csFastSerializer.Utils.File_SaveObject(this._items, this._collectionFileName);
      MessageBox.Show("Collection data saved");
    }

    private void btnList_Edit_Click(object sender, EventArgs e) {
      if (this.listItems.SelectedItems.Count != 1) {
        MessageBox.Show("Choose single item from list!", null, MessageBoxButtons.OK, MessageBoxIcon.Error);
        return;
      }
      this.pg.SelectedObject = csFastSerializer.Utils.GetClone(this.listItems.SelectedItems[0]);
      this.ShowStructure();
      //      this.pg.Refresh();
    }

    private void listItems_SelectedIndexChanged(object sender, EventArgs e) {
      if (this.DesignMode) return;
      this.listItem_SetButtons();
    }

    private void listItem_SetButtons() {
      this.btnList_Delete.Enabled = this.listItems.SelectedItems.Count > 0;
      this.btnList_Edit.Enabled = this.listItems.SelectedItems.Count==1;
    }

    private void listItems_DoubleClick(object sender, EventArgs e) {

    }

    private void btnPG_ItemUp_Click(object sender, EventArgs e) {
      this.CollectionItem_Up();
    }

    private void btnPG_ItemDown_Click(object sender, EventArgs e) {
      this.CollectionItem_Down();
    }

  }
}

