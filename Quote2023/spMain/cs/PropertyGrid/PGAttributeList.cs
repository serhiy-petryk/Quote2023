using System;
using System.Reflection;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;

namespace spMain.cs {

  # region  ////////////////////// List Attribute ////////////////////////////
  /// <summary>
  ///  
  /// </summary>
  [AttributeUsage(AttributeTargets.Property)]
  public class PG_ListAttribute : Attribute {

    IEnumerable _origList;
    bool _sortable=false;
		IDictionary _dataByKey;
		public Dictionary<object, string> _dataByValue;

		public PG_ListAttribute(object[] pList, bool sortable) {
			this._origList = pList as IEnumerable;
			this._sortable = sortable;
			this.Refresh();
		}
    public PG_ListAttribute(IEnumerable pList, bool sortable) {
      this._origList = pList;
      this._sortable = sortable;
      this.Refresh();
    }

    ////////////////////////////  Public Properties  ///////////////////////////////////
    public void SetList(IEnumerable list, bool sortable) {
      if (this._origList == null || this._origList != list || sortable != this._sortable) {
        this._origList = list; this._sortable = sortable; this.Refresh();
      }
    }
    public Array GetValueList() {
      if (this._dataByKey == null) {
        this.Refresh();
        return null; // add at 2010-07-25
      }
			object[] x = new object[this._dataByKey.Count];
      int cnt=0;
			foreach (DictionaryEntry de in this._dataByKey) x[cnt++] = de.Value;
      return x;
    }

    public object GetValue(string key) {
			if (this._dataByKey == null) return null;
			return this._dataByKey[key];
    }
    public object GetKeyStringFromValue(object value) {
			if (this._dataByValue == null) return null;
      return this._dataByValue[value];
/*      string s;
      if (this._dataByValue.TryGetValue(value, out s)) return s;
      foreach (KeyValuePair<object, string> kvp in this._dataByValue) {
        if (kvp.Key.GetHashCode() == value.GetHashCode()) return kvp.Value;
      }
      throw new Exception ("PG_ListAttribute.GetKeyStringFromValue. Value '" + value.ToString() +"' not found in collection");*/
    }

    ////////////////////////////  Private  ///////////////////////////////////
    private void Refresh() {
			this._dataByKey = null;
			this._dataByValue = null;
      if (this._origList == null) return;
      else if (this._origList is IDictionary) {
        if (this._sortable && !this._origList.GetType().Name.StartsWith("Sorted")) {
          this._dataByKey = new SortedDictionary<string, object>();
          foreach (DictionaryEntry de in (IDictionary)this._origList) {
            ((SortedDictionary<string, object>)this._dataByKey).Add(de.Key.ToString(), de.Value);
          }
        }
        else {
          this._dataByKey = (IDictionary)this._origList;
        }
				this.PrepareDataByValue();
			}
      else {
        if (this._sortable) this._dataByKey = new SortedDictionary<string, object>();
        else this._dataByKey = new Dictionary<string, object>();
        IEnumerator en = this._origList.GetEnumerator();
        en.Reset();
        while (en.MoveNext()) {
          ((IDictionary<string, object>)this._dataByKey).Add(en.Current.ToString(), en.Current);
        }
				this.PrepareDataByValue();
      }
    }
		private void PrepareDataByValue() {
      this._dataByValue = new Dictionary<object, string>();
			foreach (DictionaryEntry de in this._dataByKey) {
				this._dataByValue.Add(de.Value, de.Key.ToString());
			}
		}
  }

# endregion

  # region  ////////////////////// IsFixedSizeCollection Attribute ////////////////////////////
  /// <summary>
  ///  
  /// </summary>
  [AttributeUsage(AttributeTargets.Property)]
  public class PG_IsFixedSizeCollectionAttribute : Attribute {

    public readonly bool _isFixedSizeCollection;

    public PG_IsFixedSizeCollectionAttribute(bool isFixedSizeCollection) {
      this._isFixedSizeCollection = isFixedSizeCollection;
    }
  }
  #endregion

  # region  ////////////////////// DynamicCall Attribute ////////////////////////////
  /// <summary>
  ///  
  /// </summary>
  [AttributeUsage(AttributeTargets.Property)]
  class PG_DynamicCallAttribute : Attribute {

    public readonly string dynamicCallName;
    public readonly object args;

    public PG_DynamicCallAttribute(string pDynamicCallName, object pArgs) {
      this.dynamicCallName = pDynamicCallName;
      this.args = pArgs;
    }
  }
  #endregion

  # region  ////////////////////// IsLastStructureNode Attribute ////////////////////////////
  /// <summary>
  ///  
  /// </summary>
  [AttributeUsage(AttributeTargets.Class)]
  public class PG_IsLastStructureNodeAttribute : Attribute {

    public readonly bool _isLastNode;

    public PG_IsLastStructureNodeAttribute(bool isLastStructureNode) {
      this._isLastNode = isLastStructureNode;
    }
  }
  #endregion

  # region  ////////////////////// IsShowNameOfArrayElement Attribute ////////////////////////////
  /// <summary>
  ///  
  /// </summary>
  [AttributeUsage(AttributeTargets.Property|AttributeTargets.Class)]
  public class PG_IsShowNameOfArrayElement : Attribute {

    public readonly bool _isShowName;

    public PG_IsShowNameOfArrayElement(bool isShowNameOfArrayElement) {
      this._isShowName =isShowNameOfArrayElement;
    }
  }
  #endregion

}
