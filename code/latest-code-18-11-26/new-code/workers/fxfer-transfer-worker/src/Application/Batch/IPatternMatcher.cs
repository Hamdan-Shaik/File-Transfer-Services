namespace Fxfer.TransferWorker.Application.Batch;

public interface IPatternMatcher
{
    bool StrictMatchPattern(string expression, string name);
}

