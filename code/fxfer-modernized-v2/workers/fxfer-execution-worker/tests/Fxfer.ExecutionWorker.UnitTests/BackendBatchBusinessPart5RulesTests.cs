using Fxfer.ExecutionWorker.Batch.Business;
using Xunit;

namespace Fxfer.ExecutionWorker.UnitTests;

public sealed class BackendBatchBusinessPart5RulesTests
{
    [Fact]
    public void Strict_match_pattern_returns_false_for_null_or_empty_inputs()
    {
        var matcher = new PatternMatcher();

        Assert.False(matcher.StrictMatchPattern(null!, "file.txt"));
        Assert.False(matcher.StrictMatchPattern(string.Empty, "file.txt"));
        Assert.False(matcher.StrictMatchPattern("*", string.Empty));
        Assert.False(matcher.StrictMatchPattern("*", null!));
    }

    [Fact]
    public void Strict_match_pattern_accepts_star_and_star_dot_star()
    {
        var matcher = new PatternMatcher();

        Assert.True(matcher.StrictMatchPattern("*", "anything"));
        Assert.True(matcher.StrictMatchPattern("*.*", "anything"));
    }

    [Fact]
    public void Strict_match_pattern_supports_simple_suffix_mask_with_case_insensitive_compare()
    {
        var matcher = new PatternMatcher();

        Assert.True(matcher.StrictMatchPattern("*.txt", "Report.TXT"));
        Assert.False(matcher.StrictMatchPattern("*.txt", "Report.csv"));
    }

    [Fact]
    public void Strict_match_pattern_supports_question_mark_and_star_wildcards()
    {
        var matcher = new PatternMatcher();

        Assert.True(matcher.StrictMatchPattern("a?c", "abc"));
        Assert.False(matcher.StrictMatchPattern("a?c", "ac"));

        Assert.True(matcher.StrictMatchPattern("a*c", "ac"));
        Assert.True(matcher.StrictMatchPattern("a*c", "abbbbbc"));
        Assert.False(matcher.StrictMatchPattern("a*c", "abbbbbd"));
    }
}
