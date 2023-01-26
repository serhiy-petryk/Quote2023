using System;
using System.Collections;
using System.Collections.Generic;

namespace spMain.QData.Data {

  [Serializable]
  [cs.PG_IsLastStructureNodeAttribute(true)]
  public abstract class DataAdapter {

    public abstract IList GetData(List<object> inputs, int lastDataOffset, out int newDataOffset);

    public abstract List<DataInput> GetInputs();
    public abstract bool IsStream { get;}
    public abstract Common.TimeInterval BaseTimeInterval {get;}
    public abstract string CheckDataInputs(List<Data.DataInput> inputs);

    public override string ToString() {
      return this.GetType().Name;
    }

  }
}
