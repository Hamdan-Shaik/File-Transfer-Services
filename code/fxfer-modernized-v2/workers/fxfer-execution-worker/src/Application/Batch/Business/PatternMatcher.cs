using System;

namespace Fxfer.ExecutionWorker.Batch.Business;

public sealed class PatternMatcher : IPatternMatcher
{
    private const int MATCHESARRAYSIZE = 16;
    private const char ANSIDOSSTAR = '>';
    private const char ANSIDOSQM = '<';
    private const char DOSDOT = '"';

    public bool StrictMatchPattern(string expression, string name)
    {
        int nameOffset;
        int exprOffset;
        int length;

        int srcCount;
        int destCount;
        int previousDestCount;
        int matchesCount;

        char nameChar = '\0';
        char exprChar = '\0';

        int[] previousMatches = new int[MATCHESARRAYSIZE];
        int[] currentMatches = new int[MATCHESARRAYSIZE];

        int maxState;
        int currentState;

        bool nameFinished = false;

        if (name == null || name.Length == 0 || expression == null || expression.Length == 0)
        {
            return false;
        }

        if (expression.Equals("*") || expression.Equals("*.*"))
        {
            return true;
        }

        if (expression[0] == '*' && expression.IndexOf('*', 1) == -1)
        {
            int rightLength = expression.Length - 1;

            if (name.Length >= rightLength && string.Compare(expression, 1, name, name.Length - rightLength, rightLength, StringComparison.OrdinalIgnoreCase) == 0)
            {
                return true;
            }
        }



        previousMatches[0] = 0;
        matchesCount = 1;

        nameOffset = 0;
        maxState = expression.Length * 2;

        while (!nameFinished)
        {
            if (nameOffset < name.Length)
            {
                nameChar = name[nameOffset];
                length = 1;
                nameOffset++;
            }
            else
            {
                nameFinished = true;

                if (previousMatches[matchesCount - 1] == maxState)
                {
                    break;
                }
            }

            srcCount = 0;
            destCount = 0;
            previousDestCount = 0;

            while (srcCount < matchesCount)
            {
                exprOffset = (previousMatches[srcCount++] + 1) / 2;
                length = 0;

                while (true)
                {
                    if (exprOffset == expression.Length)
                    {
                        break;
                    }

                    exprOffset += length;

                    currentState = exprOffset * 2;

                    if (exprOffset == expression.Length)
                    {
                        currentMatches[destCount++] = maxState;
                        break;
                    }

                    exprChar = expression[exprOffset];
                    length = 1;

                    if (destCount >= MATCHESARRAYSIZE - 2)
                    {
                        int newSize = currentMatches.Length * 2;
                        int[] tmp = new int[newSize];
                        Array.Copy(currentMatches, tmp, currentMatches.Length);
                        currentMatches = tmp;

                        tmp = new int[newSize];
                        Array.Copy(previousMatches, tmp, previousMatches.Length);
                        previousMatches = tmp;
                    }

                    if (exprChar == '*')
                    {
                        currentMatches[destCount++] = currentState;
                        currentMatches[destCount++] = currentState + 1;
                        continue;
                    }

                    if (exprChar == ANSIDOSSTAR)
                    {
                        bool icanEatADot = false;

                        if (!nameFinished && (nameChar == '.'))
                        {
                            char tmpChar;
                            int offset;

                            int nameLength = name.Length;
                            for (offset = nameOffset; offset < nameLength; offset++)
                            {
                                tmpChar = name[offset];
                                length = 1;

                                if (tmpChar == '.')
                                {
                                    icanEatADot = true;
                                    break;
                                }
                            }
                        }

                        if (nameFinished || (nameChar != '.') || icanEatADot)
                        {
                            currentMatches[destCount++] = currentState;
                            currentMatches[destCount++] = currentState + 1;
                            continue;
                        }
                        else
                        {
                            currentMatches[destCount++] = currentState + 1;
                            continue;
                        }
                    }

                    currentState += length * 2;

                    if (exprChar == ANSIDOSQM)
                    {
                        if (nameFinished || (nameChar == '.'))
                        {
                            continue;
                        }

                        currentMatches[destCount++] = currentState;
                        break;
                    }

                    if (exprChar == DOSDOT)
                    {
                        if (nameFinished)
                        {
                            continue;
                        }

                        if (nameChar == '.')
                        {
                            currentMatches[destCount++] = currentState;
                            break;
                        }
                    }

                    if (nameFinished)
                    {
                        break;
                    }

                    if (exprChar == '?')
                    {
                        currentMatches[destCount++] = currentState;
                        break;
                    }

                    if (exprChar == nameChar)
                    {
                        currentMatches[destCount++] = currentState;
                        break;
                    }

                    break;
                }

                if ((srcCount < matchesCount) && (previousDestCount < destCount))
                {
                    while (previousDestCount < destCount)
                    {
                        int previousLength = previousMatches.Length;
                        while ((srcCount < previousLength) && (previousMatches[srcCount] < currentMatches[previousDestCount]))
                        {
                            srcCount += 1;
                        }

                        previousDestCount += 1;
                    }
                }
            }

            if (destCount == 0)
            {
                return false;
            }

            {
                int[] tmp;

                tmp = previousMatches;

                previousMatches = currentMatches;

                currentMatches = tmp;
            }

            matchesCount = destCount;
        }

        currentState = previousMatches[matchesCount - 1];

        return currentState == maxState;
    }
}
