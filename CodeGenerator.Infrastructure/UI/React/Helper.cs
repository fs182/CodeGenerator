namespace CodeGenerator.Infrastructure.UI.React
{
    public static class Helper
    {
        public static string GetCamel(string value)
        {
            return string.Concat(value.Substring(0, 1).ToLower(), value.Substring(1));
        }

        public static (List<int>, bool, int, int) GetColumnLayout(int columns)
        {
            return columns switch
            {
                1 => (new List<int> { 7 }, true, 3, 2),
                2 => (new List<int> { 4, 3 }, true, 3, 2),
                3 => (new List<int> { 3, 3, 3 }, true, 2, 1),
                4 => (new List<int> { 2, 2, 2, 2 }, true, 2, 2),
                5 => (new List<int> { 2, 2, 2, 2, 2 }, true, 1, 1),
                6 => (new List<int> { 2, 2, 2, 2, 2, 2 }, false, 0, 0),
                7 => (new List<int> { 2, 2, 2, 2, 2, 1, 1 }, false, 0, 0),
                8 => (new List<int> { 2, 2, 2, 2, 1, 1, 1, 1 }, false, 0, 0),
                9 => (new List<int> { 2, 2, 2, 1, 1, 1, 1, 1, 1 }, false, 0, 0),
                10 => (new List<int> { 2, 2, 1, 1, 1, 1, 1, 1, 1, 1 }, false, 0, 0),
                11 => (new List<int> { 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }, false, 0, 0),
                12 => (new List<int> { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }, false, 0, 0),
                13 => (new List<int> { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }, false, 0, 0),
                14 => (new List<int> { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }, false, 0, 0),
                15 => (new List<int> { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }, false, 0, 0),
                16 => (new List<int> { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }, false, 0, 0),
                17 => (new List<int> { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }, false, 0, 0),
                18 => (new List<int> { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }, false, 0, 0),
                19 => (new List<int> { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }, false, 0, 0),
                20 => (new List<int> { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }, false, 0, 0),
                _ => throw new NotImplementedException(),
            };
        }
        public static List<List<int>> GetFormLayout(int columns)
        {
            return columns switch
            {
                1 => new List<List<int>> { new List<int> { 12 } },
                2 => new List<List<int>> { new List<int> { 6, 6 } },
                3 => new List<List<int>> { new List<int> { 4, 4, 4 } },
                4 => new List<List<int>> { new List<int> { 6, 6 }, new List<int> { 6, 6 } },
                5 => new List<List<int>> { new List<int> { 6, 6 }, new List<int> { 4, 4, 4 } },
                6 => new List<List<int>> { new List<int> { 4, 4, 4 }, new List<int> { 4, 4, 4 } },
                7 => new List<List<int>> { new List<int> { 4, 4, 4 }, new List<int> { 6, 6 }, new List<int> { 6, 6 } },
                8 => new List<List<int>> { new List<int> { 4, 4, 4 }, new List<int> { 4, 4, 4 }, new List<int> { 6, 6 } },
                9 => new List<List<int>> { new List<int> { 4, 4, 4 }, new List<int> { 4, 4, 4 }, new List<int> { 4, 4, 4 } },
                10 => new List<List<int>> { new List<int> { 4, 4, 4 }, new List<int> { 4, 4, 4 }, new List<int> { 6, 6 }, new List<int> { 6, 6 } },
                11 => new List<List<int>> { new List<int> { 4, 4, 4 }, new List<int> { 4, 4, 4 }, new List<int> { 4, 4, 4 }, new List<int> { 6, 6 } },
                12 => new List<List<int>> { new List<int> { 4, 4, 4 }, new List<int> { 4, 4, 4 }, new List<int> { 4, 4, 4 }, new List<int> { 4, 4, 4 } },
                13 => new List<List<int>> { new List<int> { 4, 4, 4 }, new List<int> { 4, 4, 4 }, new List<int> { 4, 4, 4 }, new List<int> { 6, 6 }, new List<int> { 6, 6 } },
                14 => new List<List<int>> { new List<int> { 4, 4, 4 }, new List<int> { 4, 4, 4 }, new List<int> { 4, 4, 4 }, new List<int> { 4, 4, 4 }, new List<int> { 6, 6 } },
                15 => new List<List<int>> { new List<int> { 4, 4, 4 }, new List<int> { 4, 4, 4 }, new List<int> { 4, 4, 4 }, new List<int> { 4, 4, 4 }, new List<int> { 4, 4, 4 } },
                16 => new List<List<int>> { new List<int> { 4, 4, 4 }, new List<int> { 4, 4, 4 }, new List<int> { 4, 4, 4 }, new List<int> { 4, 4, 4 }, new List<int> { 6, 6 }, new List<int> { 6, 6 } },
                17 => new List<List<int>> { new List<int> { 4, 4, 4 }, new List<int> { 4, 4, 4 }, new List<int> { 4, 4, 4 }, new List<int> { 4, 4, 4 }, new List<int> { 4, 4, 4 }, new List<int> { 6, 6 } },
                18 => new List<List<int>> { new List<int> { 4, 4, 4 }, new List<int> { 4, 4, 4 }, new List<int> { 4, 4, 4 }, new List<int> { 4, 4, 4 }, new List<int> { 4, 4, 4 }, new List<int> { 4, 4, 4 } },
                33 => new List<List<int>> { new List<int> { 4, 4, 4 }, new List<int> { 4, 4, 4 }, new List<int> { 4, 4, 4 }, new List<int> { 4, 4, 4 }, new List<int> { 4, 4, 4 }, new List<int> { 4, 4, 4 }, new List<int> { 4, 4, 4 }, new List<int> { 4, 4, 4 }, new List<int> { 4, 4, 4 }, new List<int> { 4, 4, 4 }, new List<int> { 4, 4, 4 } },
                35 => new List<List<int>> { new List<int> { 4, 4, 4 }, new List<int> { 4, 4, 4 }, new List<int> { 4, 4, 4 }, new List<int> { 4, 4, 4 }, new List<int> { 4, 4, 4 }, new List<int> { 4, 4, 4 }, new List<int> { 4, 4, 4 }, new List<int> { 4, 4, 4 }, new List<int> { 4, 4, 4 }, new List<int> { 4, 4, 4 }, new List<int> { 4, 4, 4 }, new List<int> { 6, 6 } },
                36 => new List<List<int>> { new List<int> { 4, 4, 4 }, new List<int> { 4, 4, 4 }, new List<int> { 4, 4, 4 }, new List<int> { 4, 4, 4 }, new List<int> { 4, 4, 4 }, new List<int> { 4, 4, 4 }, new List<int> { 4, 4, 4 }, new List<int> { 4, 4, 4 }, new List<int> { 4, 4, 4 }, new List<int> { 4, 4, 4 }, new List<int> { 4, 4, 4 }, new List<int> { 4, 4, 4 } },
                37 => new List<List<int>> { new List<int> { 4, 4, 4 }, new List<int> { 4, 4, 4 }, new List<int> { 4, 4, 4 }, new List<int> { 4, 4, 4 }, new List<int> { 4, 4, 4 }, new List<int> { 4, 4, 4 }, new List<int> { 4, 4, 4 }, new List<int> { 4, 4, 4 }, new List<int> { 4, 4, 4 }, new List<int> { 4, 4, 4 }, new List<int> { 4, 4, 4 }, new List<int> { 6, 6 }, new List<int> { 6, 6 } },
                38 => new List<List<int>> { new List<int> { 4, 4, 4 }, new List<int> { 4, 4, 4 }, new List<int> { 4, 4, 4 }, new List<int> { 4, 4, 4 }, new List<int> { 4, 4, 4 }, new List<int> { 4, 4, 4 }, new List<int> { 4, 4, 4 }, new List<int> { 4, 4, 4 }, new List<int> { 4, 4, 4 }, new List<int> { 4, 4, 4 }, new List<int> { 4, 4, 4 }, new List<int> { 4, 4, 4 }, new List<int> { 6, 6 } },
                _ => throw new NotImplementedException(columns.ToString()),
            };
        }
    }
}
