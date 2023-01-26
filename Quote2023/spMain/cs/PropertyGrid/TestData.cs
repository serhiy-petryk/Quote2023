using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace TestPropertyGrid.Test {
  
  [Serializable]
//  [TypeConverter(typeof(spMain.cs.PGTypeConverter))]
  class TestData {

    SubClass _sub = new SubClass();
    List<int> _ii = new List<int>(new int[] { 1, 2, 34, 5, 67 });
//    int[] _iii = new int[0];
    int[] _iii = new int[2] { 5, 6 };
    ComplexList _complexList = new ComplexList();
    ArrayList _array_List = new ArrayList();
    Dictionary<string, string> _dict = new Dictionary<string, string>();
    Dictionary<string, ArrayList> _dictComplex = new Dictionary<string, ArrayList>();
    Dictionary<SubClass, string> _dictComplex1 = new Dictionary<SubClass,string>();
    Hashtable _hash = new Hashtable();
    string _testSting = "dfhgfh";
    Point _pp = new Point(12, 45);
    bool? _bool;
    string _name;

    public TestData() {
      _complexList.Add(4);
      _complexList.Add(5);
      _array_List.Add(_complexList);
      _array_List.Add(23);
      _dict.Add("key", "value");
      ArrayList aa = new ArrayList();
      aa.Add("test");
//      aa.Add("test1");
      _dictComplex.Add("key", aa);
      _dictComplex1.Add(new SubClass(), "21");
      _dictComplex1.Add(new SubClass(), "1");
      _hash.Add("123", 123);
      foreach (DictionaryEntry de in _hash) {
      }
      foreach (KeyValuePair<string, string> kvp in _dict) {
      }
    }

    [Category("_Name")]
    public string Name {
      get { return this._name; }
      set { this._name = value; }
    }

    public bool? TestNullBool {
      get { return this._bool; }
      set { this._bool = value; }
    }

    [ReadOnly(true)]
    public Point TestPoint {
      get { return this._pp; }
      set { this._pp = value; }
    }

    public String TestString {
      get { return this._testSting; }
      set { this._testSting = value; }
    }

    [ReadOnly(true)]
    public ComplexList PComplexList {
      get { return _complexList; }
      set { this._complexList = value; }
    }

    public Dictionary<SubClass, string> DictComplex1 {
      get { return _dictComplex1; }
    }
    public Dictionary<string, ArrayList> DictComplex {
      get { return _dictComplex; }
    }
    [spMain.cs.PG_IsFixedSizeCollection(true)]
    public Dictionary<string, string> Dict {
      get { return _dict; }
    }
    [ReadOnly(true)]
    public ArrayList PArrayList {
      get { return this._array_List; }
      set { this._array_List=value; }
    }

    public SubClass Sub {
      get { return _sub; }
      set { _sub = value; }
    }

//    [ReadOnly(true)]
    public List<int> IntList {
      get { return this._ii; }
    }
//    [ReadOnly(true)]
    [spMain.cs.PG_IsFixedSizeCollection(true)]
    public Int32[] IntArray {
      get { return this._iii; }
      set { this._iii=value; }
    }

    public override string ToString() {
      return (this._name==null ? "" : this._name);
    }
  }

  [Serializable]
  class SubClass {

    int i = 0;
    Point pp = new Point(12, 10);

    public SubClass() {
    }

    public int I {
      get { return i; }
      set { i = value; }
    }

    public Point Point {
      get { return pp; }
      set { pp = value; }
    }

    public override string ToString() {
      return "SubClass Object";
    }
  }

  [Serializable]
  class ComplexList : List<int> {
    SubClass _sub = new SubClass();
    List<int> _ii = new List<int>();
    int[] _iii = new int[2] { 5, 6 };

    [Browsable(true)]
    public SubClass Sub {
      get { return _sub; }
      set { _sub = value; }
    }

    [Browsable(true)]
    public List<int> IntList {
      get { return this._ii; }
      set { this._ii=value; }
    }
    [Browsable(true)]
    public Int32[] IntArray {
      get { return this._iii; }
      set {this._iii=value; }
    }

    public override string ToString() {
      return "TestList Object";
    }
  }
}
