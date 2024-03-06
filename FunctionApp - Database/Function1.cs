using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Data.SqlClient;
using System.Web;

namespace FunctionApp___Database
{
    public static class Function1
    {
        [FunctionName("SendToArcValues")]
        public static async Task<IActionResult> SendToArcValues(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            string connectionString = "Server=tcp:dwspocliebherr.database.windows.net,1433;Initial Catalog=ArcValues;Persist Security Info=False;User ID=dws1234;Password=dws*1234;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var data = HttpUtility.ParseQueryString(requestBody);
            Console.WriteLine(data);
            int cycleID = int.Parse(data["Cycle_ID"]);
            int point = int.Parse(data["Point"]);
            float trolleyPos = float.Parse(data["Trolley_Position"]);
            float hoistPos = float.Parse(data["Hoist_Position"]);
            DateTime dateTime = DateTime.Parse(data["DateTime"]);
            char mode = char.Parse(data["Mode"]);
            

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    // Insert into ArcValues table
                    string insertQuery = "INSERT INTO ArcValues (Cycle_ID, Point, Trolley_Position, Hoist_Position, DateTime, Mode) " +
                                         "VALUES (@Cycle_ID, @Point, @Trolley_Position, @Hoist_Position, @DateTime, @Mode)";

                    using (SqlCommand command = new SqlCommand(insertQuery, connection))
                    {
                        command.Parameters.AddWithValue("@Cycle_ID", cycleID);
                        command.Parameters.AddWithValue("@Point", point);
                        command.Parameters.AddWithValue("@Trolley_Position", trolleyPos);
                        command.Parameters.AddWithValue("@Hoist_Position", hoistPos);
                        command.Parameters.AddWithValue("@DateTime", dateTime);
                        command.Parameters.AddWithValue("@Mode", mode);

                        await command.ExecuteNonQueryAsync();
                    }
                }

                log.LogInformation("Data saved to ArcValues successfully.");

                return new OkResult(); // Return HTTP 200 OK status
            }
            catch (Exception ex)
            {
                log.LogError(ex, "An error occurred while saving data to Azure SQL.");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError); // Return HTTP 500 Internal Server Error status
            }
        }


        [FunctionName("SendToLiftCycles")]
        public static async Task<IActionResult> SendToLiftCycles(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            string connectionString = "Server=tcp:dwspocliebherr.database.windows.net,1433;Initial Catalog=ArcValues;Persist Security Info=False;User ID=dws1234;Password=dws*1234;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var data = HttpUtility.ParseQueryString(requestBody);

            int cycleId = int.Parse(data["Cycle_ID"]);
            DateTime startTime = DateTime.Parse(data["Start_Time"]);
            DateTime endTime = DateTime.Parse(data["End_Time"]);
            float totalTime = float.Parse(data["Total_Time"]);

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    // Insert into LiftCycle table
                    string insertQuery = "INSERT INTO LiftCycles (Cycle_ID, Start_Time, End_Time, Total_Time) " +
                                         "VALUES (@CycleID, @StartTime, @EndTime, @TotalTime)";

                    using (SqlCommand command = new SqlCommand(insertQuery, connection))
                    {
                        command.Parameters.AddWithValue("@CycleID", cycleId);
                        command.Parameters.AddWithValue("@StartTime", startTime);
                        command.Parameters.AddWithValue("@EndTime", endTime);
                        command.Parameters.AddWithValue("@TotalTime", totalTime);

                        await command.ExecuteNonQueryAsync();
                    }
                }

                log.LogInformation("Data saved to LiftCycles successfully.");

                return new OkResult(); // Return HTTP 200 OK status
            }
            catch (Exception ex)
            {
                log.LogError(ex, "An error occurred while saving data to Azure SQL.");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError); // Return HTTP 500 Internal Server Error status
            }
        }
    }
}
