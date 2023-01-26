using System;
using System.Windows.Forms;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

namespace spMain.cs {
  public partial class PGfrmObjectEditor{

    MethodInfo miRefreshForGridItem = null;
    // ======================================= Add/Remove collection item ===============================
		GridItem CollectionItem_GetNextItem(GridItem item) {
			// Если item - это последний элемент в коллекции, то нужно получить предыдущий item
			// Используется, чтобы получить какой элемент будет активирован после удаления
			if (item.Parent == null) return null;
			GridItemCollection items = item.Parent.GridItems;// childs of item
			if (items[items.Count - 1] == item) {
				if (items.Count > 1) return items[items.Count - 2];
				else return item.Parent;
			}
			return null;
		}

    private void CollectionItem_Up() {
      GridItem giArrayParent;
      bool flagFixedSizeCollection = false;
      GridItem giArray = this.CollectionItem_GetEditableArrayGridItem(out giArrayParent, out flagFixedSizeCollection);
      int i = GetArrayItemNo();
      if (i > 0) {
        if (giArray.Value is IList) {// Array + IList
          IList list = (IList)giArray.Value;
          object o = list[i];
          list[i] = list[i - 1];
          list[i - 1] = o;
          pg.SelectedGridItem = giArray.GridItems[i - 1];
          pg.Refresh();
          return;
        }
        else if (giArray.Value is IDictionary) {
          IDictionary dict = (IDictionary)giArray.Value;
          List<DictionaryEntry> x = new List<DictionaryEntry>();
          foreach (DictionaryEntry de in dict) {
            x.Add(de);
          }
          dict.Clear();
          DictionaryEntry o = x[i];
          x[i] = x[i - 1];
          x[i - 1] = o;
          for (int i1 = 0; i1 < x.Count; i1++) {
            dict.Add(x[i1].Key, x[i1].Value);
          }
          pg.Refresh();
//          pg.SelectedGridItem = giArray.GridItems[i - 1];
          return;
        }
      }
    }

    private void CollectionItem_Down() {
      GridItem giArrayParent;
      bool flagFixedSizeCollection = false;
      GridItem giArray = this.CollectionItem_GetEditableArrayGridItem(out giArrayParent, out flagFixedSizeCollection);
      int i = GetArrayItemNo();
      if (i < giArray.GridItems.Count-1) {
        if (giArray.Value is IList) {// Array + IList
          IList list = (IList)giArray.Value;
          object o = list[i];
          list[i] = list[i + 1];
          list[i + 1] = o;
          pg.SelectedGridItem = giArray.GridItems[i + 1];
          pg.Refresh();
          return;
        }
        else if (giArray.Value is IDictionary) {
          IDictionary dict = (IDictionary)giArray.Value;
          List<DictionaryEntry> x = new List<DictionaryEntry>();
          foreach (DictionaryEntry de in dict) {
            x.Add(de);
          }
          dict.Clear();
          DictionaryEntry o = x[i];
          x[i] = x[i + 1];
          x[i + 1] = o;
          for (int i1 = 0; i1 < x.Count; i1++) {
            dict.Add(x[i1].Key, x[i1].Value);
          }
          pg.Refresh();
          //          pg.SelectedGridItem = giArray.GridItems[i - 1];
          return;
        }
      }
    }

    private void CollectionItem_Delete() {
      if (pg.SelectedGridItem == null) return;
      GridItem giArrayParent;
      bool flagFixedSizeCollection = false;
      GridItem giArray = this.CollectionItem_GetEditableArrayGridItem(out giArrayParent, out flagFixedSizeCollection);
      try {
        if ((giArray != null) && (pg.SelectedGridItem != giArray)) {
          if (giArray.Value is Array) {
            if (pg.SelectedGridItem.PropertyDescriptor is PGPropertyDescriptorArray) {
              PGPropertyDescriptorArray pd = (PGPropertyDescriptorArray)pg.SelectedGridItem.PropertyDescriptor;
              if (pd.itemNo >= 0) {
								GridItem prevItem = this.CollectionItem_GetNextItem(pg.SelectedGridItem);
								Array a = (Array)giArray.Value;
                ArrayList list = new ArrayList(a);
                list.RemoveAt(pd.itemNo);
                Array destinationArray = (Array)Activator.CreateInstance(a.GetType(), list.Count);
                Array.Copy(list.ToArray(), destinationArray, destinationArray.Length);// Copy old array elements
                giArray.PropertyDescriptor.SetValue(giArrayParent.Value, destinationArray);
								if (prevItem != null) pg.SelectedGridItem = prevItem;
								pg.Refresh();
								return;
              }
            }
          }
          else if (giArray.Value is IList) {
            if (pg.SelectedGridItem.PropertyDescriptor is cs.PGPropertyDescriptorArray) {
							GridItem prevItem =this.CollectionItem_GetNextItem(pg.SelectedGridItem);
              ((cs.PGPropertyDescriptorArray)pg.SelectedGridItem.PropertyDescriptor).RemoveItem();
							if (prevItem != null) pg.SelectedGridItem = prevItem;
							pg.Refresh();
              return;
            }
          }
          else if (giArray.Value is IDictionary) {
            if (pg.SelectedGridItem.PropertyDescriptor is cs.PGPropertyDescriptorArray) {
							GridItem prevItem = this.CollectionItem_GetNextItem(pg.SelectedGridItem);
							((cs.PGPropertyDescriptorArray)pg.SelectedGridItem.PropertyDescriptor).RemoveItem();
							if (prevItem != null) pg.SelectedGridItem = prevItem;
							pg.Refresh();
							return;
            }
          }
        }
        MessageBox.Show("Feature is imposible");
      }
      catch (Exception ex) {
        MessageBox.Show("Function error! Error message: " + Environment.NewLine + ex.ToString());
      }
    }

    private void CollectionItem_AddNew() {
      GridItem arrayParent;
      bool flagFixedSizeCollection = false;
      GridItem gi = this.CollectionItem_GetEditableArrayGridItem(out arrayParent, out flagFixedSizeCollection);
      try {
        if (gi != null) {
          // !!! 2010-05-22
          //          need to correct: UICreateCollectionItem last argument must be hashtable of all keys of dictionary
          ArrayList dictKeys = null;
          if (gi.Value is IDictionary) {
            dictKeys = new ArrayList(((IDictionary)gi.Value).Keys);
          }
          object owner = arrayParent == null ? null : arrayParent.Value;
          object newItem = cs.PGUtils.UICreateCollectionItem(gi.Value.GetType(), null, dictKeys, owner);
					if (newItem == null) return;// Cancel add new item
					if (gi.Value is Array) {
						Array a = (Array)gi.Value;
						Array destinationArray = (Array)Activator.CreateInstance(a.GetType(), a.Length + 1);
						Array.Copy(a, destinationArray, destinationArray.Length - 1);// Copy old array elements
						Array.Copy(new object[] { newItem }, 0, destinationArray, destinationArray.Length - 1, 1);// Add new element
						gi.PropertyDescriptor.SetValue(arrayParent.Value, destinationArray);
            RefreshAfterAddItem(gi);
//            this.pg.Refresh();
						return;
					}
					else if (gi.Value is IList) {
						((IList)gi.Value).Add(newItem);
            RefreshAfterAddItem(gi);
            return;
					}
					else if (gi.Value is IDictionary) {
						object key = cs.PGUtils.GetValueFromObject(newItem, "Key");
						object value = cs.PGUtils.GetValueFromObject(newItem, "Value");
						((IDictionary)gi.Value).Add(key, value);
            RefreshAfterAddItem(gi);
            return;
					}
				}
        MessageBox.Show("Feature is imposible");
      }
      catch (Exception ex) {
        MessageBox.Show("Function error! Error message: " + Environment.NewLine + ex.ToString());
      }
    }

    void RefreshAfterAddItem(GridItem arrayItem) {
      List<string> path = this.GetGridEntryPath(arrayItem);
      this.pg.Refresh();
      GridItem x = GetGridEntryByPath(path);
      if (x != null) {
        if (!x.Expanded) {
          Expand(x, 1);
        }
        GridItem x1 = x.GridItems[x.GridItems.Count - 1];
        Expand(x1, 1);
        if (x1.GridItems.Count == 0) x1.Select();
        else x1.GridItems[0].Select();
      }
    }

    GridItem  CollectionItem_GetEditableArrayGridItem(out GridItem arrayParent, out bool flagFixedSizeCollection) {
      arrayParent = null;
      flagFixedSizeCollection = false; 
      if (pg.SelectedGridItem != null && pg.SelectedGridItem.Value != null) {
        GridItem gi = pg.SelectedGridItem;
        if (!(gi.Value is IList || gi.Value is IDictionary)) {// gi must be collection
          List<GridItem> parents = spMain.cs.PGUtils.GetParents(pg.SelectedGridItem);
          if (parents.Count > 0) {
            gi = parents[0];
          }
        }
				List<GridItem> arrayParents = spMain.cs.PGUtils.GetParents(gi);
				if (this._isFixedSizeCollection && arrayParents.Count == 0) return null;
        if (arrayParents.Count > 0) {
          arrayParent = arrayParents[0];
        }

        if (gi.PropertyDescriptor != null) {
          flagFixedSizeCollection = gi.PropertyDescriptor.Attributes.Matches(new PG_IsFixedSizeCollectionAttribute(true));
        }

        if (gi.Value is Array) {
          if (!((Array)gi.Value).IsReadOnly) {// Array is editable
            if (gi.PropertyDescriptor != null && !gi.PropertyDescriptor.IsReadOnly && cs.PGUtils.PD_CanWrite(gi.PropertyDescriptor)) {
              if (arrayParents != null) return gi;
            }
          }
          return null;
        }
        if (gi.Value is IList) {
          if (gi.PropertyDescriptor != null && gi.PropertyDescriptor.IsReadOnly) return null;// IDcitionary is ReadOnly
          if (((IList)gi.Value).IsFixedSize || ((IList)gi.Value).IsReadOnly) return null; ;
          return gi;
        }
        if (gi.Value is IDictionary) {
          if (gi.PropertyDescriptor != null && gi.PropertyDescriptor.IsReadOnly) return null;// IDcitionary is ReadOnly
          if (((IDictionary)gi.Value).IsFixedSize || ((IDictionary)gi.Value).IsReadOnly) return null; ;
          return gi;
        }
      }
      return null;
    }

    // ======================================= ShowStructure ===============================
    MethodInfo miExpand;
    PropertyInfo piChildren;
    void ShowStructure() {
      Expand(null, 10);
    }

    void Expand(object gridEntry, int maxExpands) {
      FieldInfo fi = typeof(PropertyGrid).GetField("peMain", BindingFlags.Instance | BindingFlags.NonPublic);
      object rootGridEntry = fi.GetValue(this.pg);//(GridEntry)
      if (piChildren == null && rootGridEntry != null) {
        piChildren = rootGridEntry.GetType().GetProperty("Children", BindingFlags.Instance | BindingFlags.Public);
        miExpand = this._gridView.GetType().GetMethod("SetExpand", BindingFlags.Instance | BindingFlags.NonPublic);
      }
      if (gridEntry == null)
        this.RecursivelyExpand(rootGridEntry, true, maxExpands);
      else
        this.RecursivelyExpand(gridEntry, true, maxExpands);
    }

    void RecursivelyExpand(object gridEntry, bool expand, int maxExpands) {
      if ((gridEntry != null) && (!expand || (--maxExpands >= 0))) {
        miExpand.Invoke(this._gridView, new object[] { gridEntry, expand });
        object children_ = (GridItemCollection)piChildren.GetValue(gridEntry, null);//GridItemCollection
        if (children_ != null) {
          GridItemCollection children = (GridItemCollection)children_;
          for (int i = 0; i < children.Count; i++) {
            if (children[i].PropertyDescriptor != null) {
              object[] oo = children[i].PropertyDescriptor.PropertyType.GetCustomAttributes(typeof(cs.PG_IsLastStructureNodeAttribute), false);
              if (oo.Length > 0) {
                expand = !((cs.PG_IsLastStructureNodeAttribute)oo[0])._isLastNode;
              }
              else {
                //              children[i].PropertyDescriptor.Attributes.
                cs.PGUtils.PropertyType myType = spMain.cs.PGUtils.GetPropertyTypeFromType(children[i].PropertyDescriptor.PropertyType);
                switch (myType) {
                  case spMain.cs.PGUtils.PropertyType.Enumeration:
                  case spMain.cs.PGUtils.PropertyType.KeyValuePair:
                  case spMain.cs.PGUtils.PropertyType.Primitive:
                  case spMain.cs.PGUtils.PropertyType.Structure:
                    expand = false;
                    break;
                  default: expand = true;
                    break;
                }
              }
            }
            RecursivelyExpand(children[i], expand, maxExpands);
          }
        }
      }
    }

    List<string> GetGridEntryPath(GridItem gridItem) {
      return GetGridEntryPathRecursive(gridItem, new List<string>());
    }

    List<string> GetGridEntryPathRecursive(GridItem gridItem, List<string> path) {
      if (gridItem.Parent == null) return path;
      else {
        if (gridItem.PropertyDescriptor == null)
          return this.GetGridEntryPathRecursive(gridItem.Parent, path);
        else {
          path.Insert(0, gridItem.PropertyDescriptor.Name);
          return this.GetGridEntryPathRecursive(gridItem.Parent, path);
        }
      }
    }

    GridItem GetGridEntryByPath(List<string> path) {
      FieldInfo fi = typeof(PropertyGrid).GetField("peMain", BindingFlags.Instance | BindingFlags.NonPublic);
      object rootGridEntry = fi.GetValue(this.pg);//(GridEntry)
      GridItem root = rootGridEntry as GridItem;
//      List<string> aPath = new List<string>( path.Split('\t'));
      return GetGridEntryByPathRecursive(path, root);
    }

    GridItem GetGridEntryByPathRecursive(List<string> path, GridItem gridItem) {
      if (gridItem.PropertyDescriptor == null) {// Category
        foreach (GridItem item in gridItem.GridItems) {
          GridItem x = GetGridEntryByPathRecursive(path, item);
          if (x != null) return x;
        }
      }
      else {
        if (path.Count>0 && gridItem.PropertyDescriptor.Name == path[0]) {
          if (path.Count == 1) return gridItem;
          path.RemoveAt(0);
          foreach (GridItem item in gridItem.GridItems) {
            GridItem x = GetGridEntryByPathRecursive(path, item);
            if (x != null) return x;
          }
        }
      }
      return null;
    }

  }
}
