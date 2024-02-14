using spMain.QData.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using Skender.Stock.Indicators;
using spMain.QData.DataAdapters;

namespace spMain.QData.DataDB
{
  public partial class DBIndicator
  {
    private static void LoadFromTradeIndicatorAssembly()
    {
      var a1 = Assembly.GetAssembly(typeof(Skender.Stock.Indicators.Quote));
      var t1 = a1.GetType("Skender.Stock.Indicators.Indicator");
      var mm = t1.GetMethods().OrderBy(a => a.Name);

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
          if (returnGenericType.GetInterface("IReusableResult") == null) continue;

          var flag = true;
          for (var k = 1; k < parameters.Length && flag; k++)
            flag = parameters[k].ParameterType.IsValueType;
          if (!flag) continue;

          var seriesDataType = parameters[0].ParameterType.GenericTypeArguments[0];

          var newIndicator = new DBIndicator(m);
          if (_allIndicators.ContainsKey(newIndicator._id))
          {
            var oldIndicator = _allIndicators[newIndicator._id];
            if (seriesDataType == typeof((DateTime, double)))
            {
              _allIndicators.Remove(newIndicator._id);
              _allIndicators.Add(newIndicator._id, newIndicator);
            }
            else if (seriesDataType == typeof(IReusableResult))
            {
              if (oldIndicator.GetSeriesDataType() != typeof((DateTime, double)))
              {
                _allIndicators.Remove(newIndicator._id);
                _allIndicators.Add(newIndicator._id, newIndicator);
              }
            }
            else if (seriesDataType.Name == "TQuote")
            {
            }
            else
                  throw new Exception("LoadFromTradeIndicatorAssembly error. Check series data types in TradeIndicator parser");

            continue;
          }

          _allIndicators.Add(newIndicator._id, newIndicator);
        }
        var s = m.Name + "\t" + GetTypeDescription(m.ReturnType) + "\t" + string.Join("\t", m.GetParameters().Select(p => p.Name + "->" + GetTypeDescription(p.ParameterType)));
        Debug.Print(s);

        string GetTypeDescription(Type t) => t.Name + "(" + string.Join(", ", t.GetGenericArguments().Select(a => a.Name)) + ")";
      }
    }

    // =========  Static section  =========
    private static string GetIdOfTradeIndicator(MethodInfo mi) => $"ti_{mi.Name.Substring(3).ToLower()}";

    // ====================================
    private MethodInfo _method;
    public Type GetSeriesDataType() => _method.GetParameters()[0].ParameterType.GenericTypeArguments[0];
    public MethodInfo GetMethod() => _method;

    // ==========  Constructor =============
    public DBIndicator(MethodInfo mi)
    {
      _method = mi.IsGenericMethod ? mi.MakeGenericMethod(typeof(IQuote)) : mi;
      this._hash = _cntHash++;
      _name = mi.Name.Substring(3);
      _id = GetIdOfTradeIndicator(mi);
      _valueDataType = typeof(double);
      _dp = 4;
      SetPossibleCurveStyles();
      _defCurveStyle = General.CurveStyle.Solid;
      _labelTemplate = mi.Name.Substring(3) + ":\\n{X}";
      _defColor = Color.Blue;
      _dependedInds.Add(new DependedIndicator("FramedQuote"));

      if (GetSeriesDataType().Name.Contains("Tup"))
      {

      }
      if (GetSeriesDataType() == typeof((DateTime, double)))
      {
        _inputs.Add(new Data.DataInput("value", "Value", typeof(spMain.QData.DataFormat.Quote.ValueProperty),
          spMain.QData.DataFormat.Quote.ValueProperty.Close, "Value property of Quote"));
      }

      var pp = mi.GetParameters();
      for (var k = 1; k < pp.Length; k++)
      {
        var p = pp[k];
        _inputs.Add(new Data.DataInput(p.Name.ToLower(), p.Name, p.ParameterType,
          Activator.CreateInstance(p.ParameterType), "Value of " + p.Name));
      }
    }
  }
}
