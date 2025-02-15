using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Data;

namespace Shree_API_AWS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class ExcelExportController : ControllerBase
    {
        private static string _connectionString;
        public ExcelExportController(IConfiguration config) {
            _connectionString = config.GetConnectionString("PostgresConnection");
        }

        [HttpGet("export")]
        public IActionResult ExportExcel(int year, int month)
        {
            try
            {
                using (MemoryStream stream = new MemoryStream())
                using (IWorkbook workbook = new XSSFWorkbook())
                using (NpgsqlConnection conn = new NpgsqlConnection(_connectionString))
                {
                    conn.Open();
                    ISheet sheet = workbook.CreateSheet($"Attendance_{month}_{year}");
                    ICellStyle headerStyle = CreateHeaderStyle(workbook);

                    // Fetch employee data
                    string empQuery = "SELECT employeeid, firstname, lastname, position FROM employees WHERE isactive = TRUE";
                    DataTable employees = GetDataFromTable(conn, empQuery);

                    // Create header row
                    IRow headerRow = sheet.CreateRow(0);
                    string[] fixedHeaders = { "S.NO", "Emp. ID", "Name", "Position" };
                    for (int i = 0; i < fixedHeaders.Length; i++)
                    {
                        ICell cell = headerRow.CreateCell(i);
                        cell.SetCellValue(fixedHeaders[i]);
                        cell.CellStyle = headerStyle;
                    }

                    // Generate day columns dynamically
                    int daysInMonth = DateTime.DaysInMonth(year, month);
                    for (int i = 1; i <= daysInMonth; i++)
                    {
                        ICell cell = headerRow.CreateCell(i + fixedHeaders.Length - 1);
                        cell.SetCellValue(i);
                        cell.CellStyle = headerStyle;
                    }

                    // Fetch attendance data
                    string attendanceQuery = @"SELECT *
                                       FROM employeeattendance 
                                       WHERE entryformonth = @entryMonth";

                    using (NpgsqlCommand cmd = new NpgsqlCommand(attendanceQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@entryMonth", $"{new DateTime(year, month, 1):MMMM yyyy}");
                        using (NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(cmd))
                        {
                            DataTable attendance = new DataTable();
                            adapter.Fill(attendance);

                            // Populate rows with employee attendance data
                            for (int rowIdx = 0; rowIdx < employees.Rows.Count; rowIdx++)
                            {
                                DataRow emp = employees.Rows[rowIdx];
                                string empId = emp["employeeid"].ToString();
                                IRow row = sheet.CreateRow(rowIdx + 1);

                                row.CreateCell(0).SetCellValue(rowIdx + 1); // S.NO
                                row.CreateCell(1).SetCellValue(empId); // Emp. ID
                                row.CreateCell(2).SetCellValue(emp["firstname"] + " " + emp["lastname"]);
                                row.CreateCell(3).SetCellValue(emp["position"].ToString());

                                // Fill attendance data for each day
                                foreach (DataRow att in attendance.Rows)
                                {
                                    if (Convert.ToString(att["employeeid"]) == empId)
                                    {
                                        for (int day = 1; day <= daysInMonth; day++)
                                        {
                                            DateTime currentDate = new DateTime(year, month, day);
                                            string columnValue = "";

                                            if (Convert.ToBoolean(att["ispresent"]) && att["lastpresentdate"] != DBNull.Value && Convert.ToDateTime(att["lastpresentdate"]).Date == currentDate)
                                                columnValue = "P";
                                            else if (Convert.ToBoolean(att["isabsent"]) && att["lastabsentdate"] != DBNull.Value && Convert.ToDateTime(att["lastabsentdate"]).Date == currentDate)
                                                columnValue = "A";
                                            else if (Convert.ToBoolean(att["ispaidleave"]) && att["lastpaidleavedate"] != DBNull.Value && Convert.ToDateTime(att["lastpaidleavedate"]).Date == currentDate)
                                                columnValue = "PL";
                                            else if (Convert.ToBoolean(att["islate"]) && att["lastlateday"] != DBNull.Value && Convert.ToDateTime(att["lastlateday"]).Date == currentDate)
                                                columnValue = "L";
                                            else if (Convert.ToBoolean(att["ishalfday"]) && att["lasthalfdaydate"] != DBNull.Value && Convert.ToDateTime(att["lasthalfdaydate"]).Date == currentDate)
                                                columnValue = "H";
                                            else if (Convert.ToBoolean(att["issundyduty"]) && att["lastsundaydutydate"] != DBNull.Value && Convert.ToDateTime(att["lastsundaydutydate"]).Date == currentDate)
                                                columnValue = "E";
                                            else if (Convert.ToBoolean(att["ispublicholidayduty"]) && att["lastpublicholidaydutydate"] != DBNull.Value && Convert.ToDateTime(att["lastpublicholidaydutydate"]).Date == currentDate)
                                                columnValue = "PH";

                                            row.CreateCell(day + fixedHeaders.Length - 1).SetCellValue(columnValue);
                                        }
                                    }
                                }
                            }
                        }
                    }

                    workbook.Write(stream);
                    byte[] fileBytes = stream.ToArray();
                    return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"Attendance_{month}_{year}.xlsx");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }



        private DataTable GetDataFromTable(NpgsqlConnection conn, string query)
        {
            using (NpgsqlCommand cmd = new NpgsqlCommand(query, conn))
            using (NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(cmd))
            {
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                return dt;
            }
        }

        private ICellStyle CreateHeaderStyle(IWorkbook workbook)
        {
            ICellStyle style = workbook.CreateCellStyle();
            IFont font = workbook.CreateFont();
            font.IsBold = true;
            font.Color = IndexedColors.White.Index;
            style.SetFont(font);
            style.FillForegroundColor = IndexedColors.DarkBlue.Index;
            style.FillPattern = FillPattern.SolidForeground;
            style.BorderBottom = BorderStyle.Thin;
            style.BorderTop = BorderStyle.Thin;
            style.BorderLeft = BorderStyle.Thin;
            style.BorderRight = BorderStyle.Thin;
            return style;
        }
    }
}
