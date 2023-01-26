using System;
using System.Text;
using System.Collections.Generic;

namespace spMain.QData.Data {
  public class DataManager {
    // =======================  Static section =============================
    public static Dictionary<Type, DataAdapter> dataProviders = new Dictionary<Type, DataAdapter>();
    public static List<DataIndicator> dataIndicators = new List<DataIndicator>();

    static DataManager() {
      InitDataAdapters();
//      timer.Tick += new EventHandler(timer_Tick);
  //    timer.Interval = timerInterval; // in milisecs
    }

    // ============================  Public section =============================
    public static DataIndicator GetDataIndicator(string indID, List<DataInput> localInputs, List<DataInput> globalInputs, object dataSink) {
      string uniqueID = GetIndicatorUniqueID(indID, localInputs, globalInputs);
      foreach (DataIndicator ind in dataIndicators) {
        if (uniqueID == ind._uniqueID) {
          ind.Register(dataSink);
          return ind;
        }
      }
      DataIndicator ind1 = new DataIndicator(indID, localInputs, globalInputs);
      dataIndicators.Add(ind1);
      ind1.Register(dataSink);
      return ind1;
    }

    internal static void RemoveDataIndicator(DataIndicator dataSource) {
      if (dataIndicators.Contains(dataSource)) {
        dataIndicators.Remove(dataSource);
      }
    }

    public static string GetIndicatorUniqueID(string indID, List<DataInput> localInputs, List<DataInput> globalInputs) {
      StringBuilder sb = new StringBuilder(indID);
      foreach (DataInput di in localInputs) {
        sb.Append("\t" + csUtils.StringFromObject(di._value));
      }
      foreach (DataInput di in globalInputs) {
        sb.Append("\t" + csUtils.StringFromObject(di._value));
      }
      return sb.ToString();
    }

    // ============================  Private section =============================
    static void InitDataAdapters() {
      dataProviders.Clear();
      Type[] types = typeof(DataManager).Assembly.GetTypes();
      Type adapterType = typeof(DataAdapter);

      foreach (Type t in types) {
        if (!t.IsAbstract) {
          if (adapterType.IsAssignableFrom(t)) {
            DataAdapter adapter = (DataAdapter)Activator.CreateInstance(t);
            dataProviders.Add(t, adapter);
          }
        }
      }
    }


  }
}
