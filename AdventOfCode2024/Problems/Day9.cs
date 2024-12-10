﻿namespace AdventOfCode2024.Problems
{
    public class Day9 : DayBase
    {



        #region Fields
        string _inputPath = @"C:\Users\Craig\Desktop\AdventOfCodePuzzleInputs\2024\AdventOfCode2024Day9PuzzleInput.txt";
        ulong _firstResult = 0;
        ulong _secondResult = 0;
        int _sum = 0;
        string _diskMap = string.Empty;
        List<(string id, int length)> _diskMapDecoded = new List<(string id, int length)>();
        char[] _antennaFrequencies;
        List<(int row, int col)> _antinodes = new List<(int, int)>();
        #endregion

        #region Properties
        protected override string InputPath
        {
            get => _inputPath;
            set
            {
                if (_inputPath != value)
                {
                    _inputPath = value;
                }
            }
        }

        ulong FirstResult
        {
            get => _firstResult;
            set
            {
                if (_firstResult != value)
                {
                    _firstResult = value;
                }
            }
        }
        ulong SecondResult
        {
            get => _secondResult;
            set
            {
                if (_secondResult != value)
                {
                    _secondResult = value;
                }
            }
        }

        int Sum
        {
            get => _sum;
            set
            {
                if (_sum != value)
                {
                    _sum = value;
                }
            }
        }
        string DiskMap
        {
            get => _diskMap;
            set
            {
                if (_diskMap != value)
                {
                    _diskMap = value;
                }
            }
        }
        List<(string id, int length)> DiskMapDecoded
        {
            get => _diskMapDecoded;
            set
            {
                if (_diskMapDecoded != value)
                {
                    _diskMapDecoded = value;
                }
            }
        }

        char[] AntennaFrequencies
        {
            get => _antennaFrequencies;
            set
            {
                if (_antennaFrequencies != value)
                {
                    _antennaFrequencies = value;
                }
            }
        }

        List<(int row, int col)> Antinodes
        {
            get => _antinodes;
            set
            {
                if (_antinodes != value)
                {
                    _antinodes = value;
                }
            }
        }


        #endregion

        #region Constructor
        public Day9()
        {
            InitialiseProblem();
            FirstResult = SolveFirstProblem<ulong>();
            SecondResult = SolveSecondProblem<ulong>(); //PART 2 = 6390781891880, work back from this to find part 2 solution
            OutputSolution();
        }
        #endregion

        #region Methods
        public override void InitialiseProblem()
        {
            DiskMap = File.ReadAllText(_inputPath).Replace("\r", string.Empty).Replace("\n", string.Empty);
            bool isFile = true;
            int id = 0;
            foreach (var numberChar in DiskMap)
            {
                int length = int.Parse(numberChar.ToString());
                if (isFile)
                {
                    for (int i = 0; i <= length - 1; i++)
                    {
                        DiskMapDecoded.Add((id.ToString(), length));
                    }
                    id++;
                }
                else
                {
                    for (int i = 0; i <= length - 1; i++)
                    {
                        DiskMapDecoded.Add((".", length));
                    }
                }
                isFile = !isFile;
            }
        }

        public override T SolveFirstProblem<T>()
        {
            var diskMapDecodedPart1Copy = SortDiskMapFragmented();
            var result = CalculateDiskMap(diskMapDecodedPart1Copy);

            return (T)Convert.ChangeType(result, typeof(T));
        }
        public override T SolveSecondProblem<T>()
        {
            var disMapDecodedPart2Copy = SortDiskMapUnfragmented();
            var result = CalculateDiskMap(disMapDecodedPart2Copy);
            return (T)Convert.ChangeType(result, typeof(T));
        }

        public override void OutputSolution()
        {
            Console.WriteLine($"First Solution is: {FirstResult}");
            Console.WriteLine($"Second Solution is: {SecondResult}");
        }

        private List<(string id, int length)> SortDiskMapFragmented()
        {
            int reversedIndex = DiskMapDecoded.Count() - 1;
            var diskMapDecodedPart1Copy = DiskMapDecoded.ToList();

            for (int i = 0; i <= diskMapDecodedPart1Copy.Count() - 1; i++)
            {
                if (diskMapDecodedPart1Copy[i].id == ".")
                {
                    for (int j = reversedIndex; j >= i + 1; j--)
                    {
                        if (diskMapDecodedPart1Copy[j].id != ".")
                        {
                            var value = diskMapDecodedPart1Copy[j];
                            diskMapDecodedPart1Copy[j] = diskMapDecodedPart1Copy[i];
                            diskMapDecodedPart1Copy[i] = value;
                            reversedIndex = j;
                            break;
                        }
                        else
                            continue;
                    }
                }
            }

            return diskMapDecodedPart1Copy;
        }

        private List<(string id, int length)> SortDiskMapUnfragmented2()
        {
            var diskMapDecodedPart1Copy = DiskMapDecoded.ToList();

            for (int i = 0; i <= diskMapDecodedPart1Copy.Count() - 1; i++)
            {
                if (diskMapDecodedPart1Copy[i].id == ".")
                {
                    for (int j = diskMapDecodedPart1Copy.Count - 1; j >= i + 1; j--)
                    {
                        if (diskMapDecodedPart1Copy[j].id != ".")
                        {
                            if (diskMapDecodedPart1Copy[i].length >= diskMapDecodedPart1Copy[j].length)
                            {
                                int leftOverFreeSpaceLength = 0;
                                var temp = diskMapDecodedPart1Copy[j];
                                if (diskMapDecodedPart1Copy[i].length - diskMapDecodedPart1Copy[j].length != 0)
                                {
                                    leftOverFreeSpaceLength = diskMapDecodedPart1Copy[i].length - diskMapDecodedPart1Copy[j].length;
                                    for (int k = diskMapDecodedPart1Copy[j].length; k <= diskMapDecodedPart1Copy[i].length; k++)
                                    {
                                        diskMapDecodedPart1Copy[i + k] = new(diskMapDecodedPart1Copy[i + k].id, leftOverFreeSpaceLength);
                                    }
                                }


                                for (int k = 0; k <= diskMapDecodedPart1Copy[i].length - 1; k++)
                                {
                                    var value = diskMapDecodedPart1Copy[j - k];
                                    diskMapDecodedPart1Copy[j - k] = diskMapDecodedPart1Copy[i + k];
                                    diskMapDecodedPart1Copy[i + k] = value;
                                }
                                break;
                            }

                        }
                        else
                        {
                            continue;
                        }

                    }
                }
            }

            return diskMapDecodedPart1Copy;
        }

        private List<(string id, int length)> SortDiskMapUnfragmented()
        {
            var diskMapDecodedPart1Copy = DiskMapDecoded.ToList();
            //int i = 0; i <= diskMapDecodedPart1Copy.Count() - 1; i++
            for (int i = diskMapDecodedPart1Copy.Count() - 1; i >= 0; i--)
            {
                if (diskMapDecodedPart1Copy[i].id != ".")
                {
                    for (int j = 0; j <= i; j++)
                    {
                        if (diskMapDecodedPart1Copy[j].id == ".")
                        {
                            if (diskMapDecodedPart1Copy[j].length >= diskMapDecodedPart1Copy[i].length)
                            {
                                int leftOverFreeSpaceLength = 0;
                                var temp = diskMapDecodedPart1Copy[i];
                                if (diskMapDecodedPart1Copy[j].length - diskMapDecodedPart1Copy[i].length != 0)
                                {
                                    leftOverFreeSpaceLength = diskMapDecodedPart1Copy[j].length - diskMapDecodedPart1Copy[i].length;
                                    for (int k = diskMapDecodedPart1Copy[i].length; k <= diskMapDecodedPart1Copy[j].length; k++)
                                    {
                                        diskMapDecodedPart1Copy[j + k] = new(diskMapDecodedPart1Copy[j + k].id, leftOverFreeSpaceLength);
                                    }
                                }


                                for (int k = 0; k <= diskMapDecodedPart1Copy[j].length - 1; k++)
                                {
                                    var value = diskMapDecodedPart1Copy[i - k];
                                    diskMapDecodedPart1Copy[i - k] = diskMapDecodedPart1Copy[j + k];
                                    diskMapDecodedPart1Copy[j + k] = value;
                                }
                                break;
                            }
                        }
                    }
                }





                //    if (diskMapDecodedPart1Copy[i].id == ".")
                //{
                //    for (int j = diskMapDecodedPart1Copy.Count - 1; j >= i + 1; j--)
                //    {
                //        if (diskMapDecodedPart1Copy[j].id != ".")
                //        {
                //            if (diskMapDecodedPart1Copy[i].length >= diskMapDecodedPart1Copy[j].length)
                //            {
                //                int leftOverFreeSpaceLength = 0;
                //                var temp = diskMapDecodedPart1Copy[j];
                //                if (diskMapDecodedPart1Copy[i].length - diskMapDecodedPart1Copy[j].length != 0)
                //                {
                //                    leftOverFreeSpaceLength = diskMapDecodedPart1Copy[i].length - diskMapDecodedPart1Copy[j].length;
                //                    for (int k = diskMapDecodedPart1Copy[j].length; k <= diskMapDecodedPart1Copy[i].length; k++)
                //                    {
                //                        diskMapDecodedPart1Copy[i + k] = new(diskMapDecodedPart1Copy[i + k].id, leftOverFreeSpaceLength);
                //                    }
                //                }


                //                for (int k = 0; k <= diskMapDecodedPart1Copy[i].length - 1; k++)
                //                {
                //                    var value = diskMapDecodedPart1Copy[j - k];
                //                    diskMapDecodedPart1Copy[j - k] = diskMapDecodedPart1Copy[i + k];
                //                    diskMapDecodedPart1Copy[i + k] = value;
                //                }
                //                break;
                //            }

                //        }
                //        else
                //        {
                //            continue;
                //        }

                //    }
                //}
            }

            return diskMapDecodedPart1Copy;
        }

        private ulong CalculateDiskMap(List<(string id, int length)> diskMapDecodedPart1Copy)
    {
        ulong checkSum = 0;
        for (int i = 0; i <= diskMapDecodedPart1Copy.Count() - 1; i++)
        {
            if (ulong.TryParse(diskMapDecodedPart1Copy[i].id, out ulong number))
            {
                checkSum += number * (ulong)i;
            }
        }
        return checkSum;
            //6367087064415	THATS A BINGO

            //6390216683958 PART 2 TOO LOW
            //6390682211954	TOO LOW
            //6390781741404
            //6389514248351


        }

        #endregion
    }
}