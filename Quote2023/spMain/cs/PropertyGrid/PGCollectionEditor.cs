using System;
using System.Collections;
using System.Windows.Forms;
using System.Reflection;
using System.ComponentModel;

namespace spMain.cs {

	public class PGCollectionEditor : System.ComponentModel.Design.CollectionEditor {

		public ITypeDescriptorContext baseContext;
		public object baseValue;
		private bool? _isFixedCollection;
    private bool? _isReadOnly;
    PropertyGrid _pg;
    CollectionForm collectionForm;
    bool _cancelFlag = false;
    ListBox _formListBox;
    object _pgGridView;
    GridItemCollection _pgItems;
    bool _refreshFlag = true;

    private bool IsFixedCollection() {
      if (!this._isFixedCollection.HasValue) {
        if (baseValue is Array) {
          this._isFixedCollection = !PGUtils.PD_CanWrite(baseContext.PropertyDescriptor);
        }
        else if (baseValue is IList) 
          this._isFixedCollection = ((IList)this.baseValue).IsFixedSize;
        else if (baseValue is IDictionary) 
          this._isFixedCollection = ((IDictionary)this.baseValue).IsFixedSize;
        else this._isFixedCollection = true;

        if (!this._isFixedCollection.Value) 
          this._isFixedCollection = baseContext.PropertyDescriptor.Attributes.Matches(new PG_IsFixedSizeCollectionAttribute(true));
      }
      return this._isFixedCollection.Value;
    }
		private bool IsReadOnly() {
			if (!this._isReadOnly.HasValue) {
				AttributeCollection x = this.baseContext.PropertyDescriptor.Attributes;
				Attribute x1 = x[typeof(ReadOnlyAttribute)];
				this._isReadOnly = x1 != null && ((ReadOnlyAttribute)x1).IsReadOnly;
			}
			return this._isReadOnly.Value;
		}
		private void SetCollectionEditable(CollectionForm form, bool IsCollectionEditable) {
			csUtilsReflection.SetProperty(form, "CollectionEditable", IsCollectionEditable);
		}


		public PGCollectionEditor(Type type)
			: base(type) {
		}

		protected override void CancelChanges() {
      _cancelFlag = true;
		}
		protected override bool CanRemoveInstance(object value) {
			return base.CanRemoveInstance(value);
		}
		protected override bool CanSelectMultipleInstances() {
			return base.CanSelectMultipleInstances();
		}
		protected override CollectionForm CreateCollectionForm() {
			// Form structure 
			//	form.Controls[0].TableLayoutPanel(8 controls)
			//	form.Controls[0](TableLayoutPanel).Controls[0](button:DownButton) 
			//	form.Controls[0](TableLayoutPanel).Controls[1](TableLayoutPanel) (Add/Remove buttons)
			//	form.Controls[0](TableLayoutPanel).Controls[2](Label:Properties)
			//	form.Controls[0](TableLayoutPanel).Controls[3](Label:Members)
			//	form.Controls[0](TableLayoutPanel).Controls[4](ListBox)
			//	form.Controls[0](TableLayoutPanel).Controls[5](vsPropertyGrid)
			//	form.Controls[0](TableLayoutPanel).Controls[6](Panel) ( + OK/Cancel buttons) 
			//	form.Controls[0](TableLayoutPanel).Controls[7](button:UpButton)

			CollectionForm form = base.CreateCollectionForm();
      collectionForm = form;
			Control x = form.Controls[0].Controls[5];
//      form.Controls[0].Controls.Remove(x);
      /*      Control newPG = new TestPropertyGrid.cs.Support.PG_TestControl();
            newPG.BackColor = Color.Yellow;
            form.Controls[0].Controls.Add(newPG );
            FieldInfo[] ff = form.GetType().GetFields(spMain.csUtilsReflection.allBFs);// private VsPropertyGrid propertyBrowser;
            ff[11].SetValue(form, newPG);*/
      if (x is PropertyGrid) {
				/*				form.Controls[0].Controls[0].BackColor = Color.Red;
								form.Controls[0].Controls[1].BackColor = Color.Yellow;
								form.Controls[0].Controls[2].BackColor = Color.Green;
								form.Controls[0].Controls[3].BackColor = Color.LightBlue;
								form.Controls[0].Controls[4].BackColor = Color.Violet;
								form.Controls[0].Controls[5].BackColor = Color.RosyBrown;
								form.Controls[0].Controls[6].BackColor = Color.Orange;
								form.Controls[0].Controls[7].BackColor = Color.Blue;
								form.Controls[0].BackColor = Color.White;*/
        this._pg = (PropertyGrid) form.Controls[0].Controls[5];
				string descr = this.baseContext.PropertyDescriptor.Description;
				if (!String.IsNullOrEmpty(descr)) {
					if (form.Controls[0].Controls.Count == 8) {
						Label lbl = new Label();
						lbl.AutoSize = false;
						lbl.Dock = System.Windows.Forms.DockStyle.Fill;
						lbl.Location = new System.Drawing.Point(3, 180);
						lbl.Name = "lblDescription";
						lbl.Size = new System.Drawing.Size(123, 39);// two lines
						TableLayoutPanel panel = (TableLayoutPanel)form.Controls[0];
						((TableLayoutPanel)form.Controls[0]).Controls.Add(lbl, 0, 4);
						panel.SetColumn(form.Controls[0].Controls[8], 0);
						panel.SetColumnSpan(form.Controls[0].Controls[8], 2);
						panel.SetColumn(form.Controls[0].Controls[6], 2);
						panel.SetColumnSpan(form.Controls[0].Controls[6], 1);
					}
					form.Controls[0].Controls[8].Text = descr;
				}
				PropertyGrid pg = (PropertyGrid)x;
				this.pg_PropertySortChanged(pg, new EventArgs());
				pg.PropertySortChanged += new EventHandler(pg_PropertySortChanged);
				pg.SelectedObjectsChanged += new EventHandler(pg_SelectedObjectsChanged);
				ToolStrip oToolStrip = (ToolStrip)pg.Controls[3];
				oToolStrip.Items.RemoveAt(4);
				oToolStrip.Items.RemoveAt(3);
			}
			form.Tag = this;
      if (this.IsReadOnly()) this.SetCollectionEditable(form, false); // Read only 
      else if (this.IsFixedCollection()) {// Edit fixed size
        this.SetCollectionEditable(form, true);
        form.Controls[0].Controls[1].Enabled = false;// Disabled Add/Remove buttons
      }
      else {// Can edit, not fixed size
        this.SetCollectionEditable(form, true);
        form.Controls[0].Controls[1].Enabled = true;// Enabled Add/Remove buttons
      }

      if (this.IsReadOnly()) {
        form.Controls[0].Controls[5].Controls[2].Enabled = false;
      }

      FieldInfo fi = form.GetType().GetField("listbox", BindingFlags.NonPublic | BindingFlags.Instance);
      this._formListBox = (ListBox)fi.GetValue(form);
      //      form.Size = new Size(800, 500);
      form.Text = "\"" + form.Text;
			return form;
		}

		void pg_SelectedObjectsChanged(object sender, EventArgs e) {
			PropertyGrid pg = (PropertyGrid)sender;
			object x = pg.SelectedObject;
			if (x is PropertyDescriptor) {// Dictionary (SelectionWrapper object)
        FieldInfo fi = x.GetType().GetField("value", BindingFlags.Instance | BindingFlags.NonPublic);
        if (fi != null) {
					object x1 = fi.GetValue(x);
					PGUtils.PropertyType myType = PGUtils.GetPropertyTypeFromValue(x1);
          if (myType == PGUtils.PropertyType.Collection || myType == PGUtils.PropertyType.KeyValuePair || myType == PGUtils.PropertyType.Object) {
						pg.SelectedObject = x1;
					}
				}
			}
		}

		void pg_PropertySortChanged(object sender, EventArgs e) {
			PropertyGrid pg = (PropertyGrid)sender;
			if (pg.PropertySort == PropertySort.CategorizedAlphabetical) pg.PropertySort = PropertySort.Categorized;
		}

		protected override Type CreateCollectionItemType() {
      Type colllectionType = PGUtils.GetCollectionItemType(this.baseValue.GetType());
      if (colllectionType == null) return base.CreateCollectionItemType();
      else return colllectionType;
		}

		protected override object CreateInstance(Type itemType) {// return = null : No Instance 
      ArrayList keys=null;
      if (this.baseValue is IDictionary) {
        keys=new ArrayList();
        for (int i=0; i<this._formListBox.Items.Count; i++) {
          object o = this._formListBox.Items[i]; // o is ListItem (internal class)
          object itemValue = PGUtils.GetValueFromObject(o, "Value");
          keys.Add(PGUtils.GetValueFromObject(itemValue, "Key"));
        }
      }
      object newObject =  PGUtils.UICreateCollectionItem(this.baseValue.GetType(), base.CreateInstance(itemType), keys, this.baseContext.Instance);
      this._cancelFlag = true;
      PGUtils.ApplyTypeConverter(newObject);
      return newObject;
		}

		protected override Type[] CreateNewItemTypes() {
			return base.CreateNewItemTypes();
		}
		protected override void DestroyInstance(object instance) {
        base.DestroyInstance(instance);
		}

    public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value) {
			this.baseContext = context;
			this.baseValue = value;
//			PGUtils.ApplyTypeConverter(value);

			object outValue = base.EditValue(context, provider, value);
			// Refresh Items of Parent PropertyGrid
//			if (!(this.baseValue is Array)) {// Array data copied and do not need to refresh
      if (this._refreshFlag) {
        object pg = csUtilsReflection.GetPropertyRecurs(this.baseContext, "OwnerGrid");
        if (pg is PropertyGrid) ((PropertyGrid)pg).Refresh();
      }
	//		}

/*      if (this.baseContext is GridItem) {
        ((GridItem)this.baseContext).Tag = "Refresh";
      }*/
      return outValue;
		}

    protected override string GetDisplayText(object value) {
      // change by sp at 2010-05-17
      spMain.cs.PGUtils.PropertyType myType = PGUtils.GetPropertyTypeFromValue(value);
      if (myType == PGUtils.PropertyType.KeyValuePair) {
        object o = PGUtils.GetValueFromObject(value, "Key");
        return o.ToString();
      }
      return base.GetDisplayText(value);
    }
    public override System.Drawing.Design.UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context) {
			//			if (this.IsFixedCollection(context) || this.IsReadOnly(context)) return System.Drawing.Design.UITypeEditorEditStyle.None;
			//      			return System.Drawing.Design.UITypeEditorEditStyle.Modal; 
			return base.GetEditStyle(context);
		}
		protected override object[] GetItems(object editValue) {
			return base.GetItems(editValue);
		}
		protected override System.Collections.IList GetObjectsFromInstance(object instance) {
			return base.GetObjectsFromInstance(instance);
		}
		public override bool GetPaintValueSupported(ITypeDescriptorContext context) {
			return base.GetPaintValueSupported(context);
		}
		public override void PaintValue(System.Drawing.Design.PaintValueEventArgs e) {
			base.PaintValue(e);
		}
		protected override object SetItems(object editValue, object[] value) {
      if (editValue is IDictionary) {
        return SetItemsDictionary((IDictionary)editValue, value);
      }
      this._cancelFlag = false;// reset flag
      if (editValue is Array) {
        if (((Array)editValue).Length == value.Length) {// Array == IsFixedCollection
          value.CopyTo((Array)editValue, 0);
          return editValue;
        }
        else {// Array 
          Array destinationArray = Array.CreateInstance(base.CollectionItemType, value.Length);
          Array.Copy(value, destinationArray, value.Length);
          this._refreshFlag = false;// Array changed == no need to refresh
          return destinationArray;
        }
      }
			if (editValue is IList) {// IList
        ((IList)editValue).Clear();
        for (int i = 0; i < value.Length; ++i)
          ((IList)editValue).Add(value[i]);
			}
      // ???? Error
      return base.SetItems(editValue, value);
		}

    object SetItemsDictionary(IDictionary dict, object[] items) {
      if (!this._cancelFlag) {// Move items from baseItems to new items[] 
        for (int i=0; i<items.Length; i++) {
          object key= PGUtils.GetValueFromObject(items[i], "Key");
          if (((IDictionary)this.baseValue).Contains(key)) {
            object value = ((IDictionary)this.baseValue)[key];
            PropertyInfo pi = items[i].GetType().GetProperty("Value");
            if (pi.CanWrite) {
              pi.SetValue(items[i], value, null);
            }
            else {
              FieldInfo fi = items[i].GetType().GetField("value", BindingFlags.Instance | BindingFlags.NonPublic);
              fi.SetValue(items[i], value);
            }
          }
        }
      }
      this._cancelFlag = false;// Reset flag
      dict.Clear();
      if (items != null) {
        for (int i = 0; i < items.Length; i++) {
          object key = PGUtils.GetValueFromObject(items[i], "Key");
            object value = PGUtils.GetValueFromObject(items[i], "Value");
            dict.Add(key, value);
        }
      }
      return dict;
    }

    public override bool IsDropDownResizable {
      get { return base.IsDropDownResizable; }
    }

  }
}
