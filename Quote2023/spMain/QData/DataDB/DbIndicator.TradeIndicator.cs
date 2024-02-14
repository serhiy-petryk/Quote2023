using spMain.QData.Common;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;

namespace spMain.QData.DataDB
{
  public partial class DBIndicator
  {
    private static void LoadFromTradeIndicatorAssembly()
    {
      var a1 = Assembly.GetAssembly(typeof(Skender.Stock.Indicators.Quote));
      var t1 = a1.GetType("Skender.Stock.Indicators.Indicator");
      var mm = t1.GetMethods().OrderBy(a => a.Name);
      // _allIndicators = new Dictionary<string, DBIndicator>();
      foreach (var m in mm)
      {
        if (m.Name.StartsWith("Get") && !string.Equals(m.Name, "GetHashCode") && !string.Equals(m.Name, "GetType"))
        {
          var parameters = m.GetParameters();
          if (!(m.ReturnType.GetInterface("IEnumerable") != null && m.ReturnType.GenericTypeArguments.Length == 1) || parameters.Length == 0)
          {
            throw new Exception("LoadFromTradeIndicatorAssembly error. Check TradeIndicator parser!");
          }

          var returnGenericType = m.ReturnType.GenericTypeArguments[0];
          var inputDataType = parameters[0].ParameterType.GenericTypeArguments[0];

          if (returnGenericType.GetInterface("IReusableResult") != null)
          {
            var pp = m.GetParameters();
            // if (pp.Length == 2 && pp[0].ParameterType.GetInterface("IEnumerable") != null && pp[0].ParameterType.GenericTypeArguments.Length == 1 && pp[0].ParameterType.GenericTypeArguments[0].Name == "TQuote")
            if (pp.Length == 2 && pp[0].ParameterType.GetInterface("IEnumerable") != null && pp[0].ParameterType.GenericTypeArguments.Length == 1 && pp[0].ParameterType.GenericTypeArguments[0] == typeof(Skender.Stock.Indicators.IReusableResult))
            {
              if (pp[1].ParameterType == typeof(int))
              {
                var item = new DBIndicator(m);
                _allIndicators.Add(item._id, item);
              }
            }
            else
            {
            }
          }

        }
        var s = m.Name + "\t" + GetTypeDescription(m.ReturnType) + "\t" + string.Join("\t", m.GetParameters().Select(p => p.Name + "->" + GetTypeDescription(p.ParameterType)));
        Debug.Print(s);


        string GetTypeDescription(Type t) => t.Name + "(" + string.Join(", ", t.GetGenericArguments().Select(a => a.Name)) + ")";
      }
    }

    // =======================  Constructor ========================
    private MethodInfo _method;

    public MethodInfo GetMethod() => _method;

    public DBIndicator(MethodInfo mi)
    {
      _method = mi;
      this._hash = _cntHash++;
      _name = mi.Name.Substring(3);
      _id = "ti_" + _name.ToLower();
      _valueDataType = typeof(double);
      _dp = 4;
      SetPossibleCurveStyles();
      _defCurveStyle = General.CurveStyle.Solid;
      _labelTemplate = mi.Name.Substring(3) + ":\\n{X}";
      _defColor = Color.Blue;
      _dependedInds.Add(new DependedIndicator("FramedQuote"));
      _inputs.Add(new Data.DataInput("value", "Value", typeof(spMain.QData.DataFormat.Quote.ValueProperty), spMain.QData.DataFormat.Quote.ValueProperty.Close, "Value property of Quote"));

      var parName = mi.GetParameters()[1].Name;
      _inputs.Add(new Data.DataInput(parName.ToLower(), parName, typeof(int), 21, "Value of " + parName));
    }
  }
}
