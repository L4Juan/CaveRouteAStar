internal class Program
{
    private static void Main(string[] args)
    {
        List<int> coordinates = new List<int>();
        int numOfCaves;
        int[,] matrix;
        string strExeFilePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
        string strWorkPath = Path.GetDirectoryName(strExeFilePath);
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
        string file = "";

        //read file
        try
        {
            /*
            string[] files = Directory.GetFiles(strWorkPath, "*.cav");
            file = files[0];
            */
            file = args[0];
            for(int i = 0; i<100; ++i)
            {
                
            }
        }
        catch (Exception)
        {
            Environment.Exit(0);
        }

        (numOfCaves, coordinates, matrix) = InterpretFile();
        int nodeStart = 1;
        int nodeGoal = numOfCaves;
        int start = nodeStart - 1;
        int goal = nodeGoal - 1;

        Algorythm(start, goal);

        (int, List<int>, int[,]) InterpretFile()
        {


            try
            {

                FileStream fs = File.OpenRead(strWorkPath + "/" + file + ".cav");
                var sr = new StreamReader(fs);
                var fileData = sr.ReadLine().ToString().Split(',').Select(int.Parse)?.ToList();
                int numOfCaves = fileData[0];


                int[,] matrix = new int[numOfCaves, numOfCaves];
                for (int i = 1; i < numOfCaves * 2 + 1; i++)
                {
                    coordinates.Add(fileData[i]);
                }
                int count = numOfCaves * 2 + 1;

                for (int i = 0; i < numOfCaves; i++)
                {
                    for (int j = 0; j < numOfCaves; j++)
                    {
                        matrix[i, j] = fileData[count];
                        count++;
                    }
                }
                return (numOfCaves, coordinates, matrix);

            }
            catch (Exception)
            {
                Environment.Exit(0);
            }
            return (0, new int[] { 0, 0 }.ToList(), new int[,] { });
        }

        void Algorythm(int start, int goal)
        {
            //add start to open set
            HashSet<int> openSet = new HashSet<int>();
            openSet.Add(start);

            //Came from list contains objects like {1,4}, meaning 1 came from 4
            Dictionary<int, int> cameFrom = new Dictionary<int, int>();

            Dictionary<int, double> gScore = new Dictionary<int, double>();
            Dictionary<int, double> fScore = new Dictionary<int, double>();

            for (int i = 0; i < numOfCaves; i++)
            {
                cameFrom.Add(i, numOfCaves + 1);
                gScore.Add(i, double.PositiveInfinity);
                fScore.Add(i, double.PositiveInfinity);
            }
            gScore[start] = 0;
            fScore[start] = Dis(start, goal);

            while (openSet.Any())
            {
                int current = start;
                var keys = fScore.OrderBy(KeyValuePair => KeyValuePair.Value).ToArray();
                foreach (var key in keys)
                {
                    if (openSet.Contains(key.Key))
                    {
                        current = key.Key;
                        break;
                    }
                }
                if (current == goal)
                {
                    reconstruct_path(cameFrom, current);
                    return;
                }

                openSet.Remove(current);
                for (int i = 0; i < numOfCaves; i++)
                {
                    if (matrix[i, current] == 1 & i != current)
                    {
                        double tentative_gScore = gScore[current] + Dis(current, i);
                        if (tentative_gScore < gScore[i])
                        {
                            cameFrom[i] = current;
                            gScore[i] = tentative_gScore;
                            fScore[i] = tentative_gScore + Dis(i, goal);
                            if (!openSet.Contains(i))
                            {
                                openSet.Add(i);
                            }
                        }
                    }
                }

            }
            File.WriteAllText(file.Substring(0, file.Length - 4) + ".csn", "0");
        }

        void reconstruct_path(Dictionary<int, int> cameFrom, int current)
        {
            List<int> solution = new List<int>();
            solution.Add(current + 1);
            do
            {
                current = cameFrom[current];
                solution.Add(current + 1);
            } while (current != start);

            solution.Reverse();
            File.WriteAllText(strWorkPath + "/" + file + ".csn", string.Join(" ", solution));
        }

        double Dis(int from, int to)
        {
            int dis1 = coordinates[from * 2] - coordinates[to * 2];
            int dis2 = coordinates[from * 2 + 1] - coordinates[to * 2 + 1];
            return Math.Sqrt(dis1 * dis1 + dis2 * dis2);
        }
    }
}





