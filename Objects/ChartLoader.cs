using System;
using System.IO;
using System.Text.Json;

namespace Comp_296_project_ARMA.Objects
{
    public class ChartLoader
    {
        public static Chart LoadChart(string filePath)
        {
            // Implement file reading and parsing logic here
            // For example, you could read a JSON or XML file and populate the Chart object
            // This is just a placeholder implementation
            string json = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<Chart>(json);
        }
    }
}
