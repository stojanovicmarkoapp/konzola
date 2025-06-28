using konzola.Services;
using ScottPlot;
using System;
using System.IO;
using System.Linq;
using System.Text;

namespace konzola
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var service = new TimeService();
            var entries = service.GetTimesAsync().GetAwaiter().GetResult();
            var totalByEmployee = entries
                .Where(e => !string.IsNullOrWhiteSpace(e.Employee))
                .GroupBy(e => e.Employee)
                .Select(g => new
                {
                    Name = g.Key,
                    TotalHours = g.Sum(e => (e.EndTime - e.StartTime).TotalHours)
                })
                .OrderByDescending(e => e.TotalHours)
                .ToList();
            var plot = new ScottPlot.Plot(500, 500);
            double[] values = totalByEmployee.Select(e => e.TotalHours).ToArray();
            string[] names = totalByEmployee.Select(e => $"{e.Name}").ToArray();
            var pie = plot.AddPie(values);
            pie.SliceLabels = names;
            pie.ShowPercentages = true;
            plot.Legend(true);
            plot.Legend(location: Alignment.UpperRight);
            plot.SaveFig("piechart.png");
            using var imageStream = File.OpenRead("piechart.png");
            using var memoryStream = new MemoryStream();
            imageStream.CopyTo(memoryStream);
            string base64 = Convert.ToBase64String(memoryStream.ToArray());
            var html = new StringBuilder();
            html.AppendLine("<html><body><div style='display: flex; flex-wrap: wrap; justify-content: center;'><table border='1'>");
            html.AppendLine("<tr><th>Name</th><th>Total Time in Hours</th></tr>");
            foreach (var employee in totalByEmployee)
            {
                Console.WriteLine($"{employee.Name}|{employee.TotalHours}");
                var color = employee.TotalHours < 100 ? " style='background-color:red'" : "";
                html.AppendLine($"<tr{color}><td>{employee.Name}</td><td>{employee.TotalHours:F2}</td></tr>");
            }
            html.AppendLine("</table>");
            html.AppendLine("<div><h2 style='float: right'>Pie Chart</h2><br/>");
            html.AppendLine($"<img src=\"data:image/png;base64,{base64}\"/>");
            html.AppendLine("</div></div></body></html>");
            File.WriteAllText("report.html", html.ToString());
            var psi = new System.Diagnostics.ProcessStartInfo
            {
                FileName = "report.html",
                UseShellExecute = true
            };
            System.Diagnostics.Process.Start(psi);
        }
    }
}
