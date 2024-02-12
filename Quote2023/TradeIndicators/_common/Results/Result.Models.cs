using System;

namespace Skender.Stock.Indicators
{

    // RESULT MODELS

    public interface IResult : ISeries
    {
    }

    public interface IReusableResult : IResult
    {
        double? Value { get; }
    }

    [Serializable]
    public abstract class ResultBase : IResult
    {
        public DateTime Date { get; set; }
    }
}
