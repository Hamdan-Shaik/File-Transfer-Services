namespace Fxfer.ExecutionWorker.Batch.Business;

public interface IPatternMatcher
{
    bool StrictMatchPattern(string expression, string name);
}
