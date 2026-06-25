public class DashboardCountDto
{
    public int totalDoctors { get; set; }
    public int totalPatients { get; set; }
    public int totalAppointments { get; set; }
}

public class DoctorChartDto
{
    public int DoctorId { get; set; }
    public int Count { get; set; }
}

public class DepartmentChartDto
{
    public string Department { get; set; }
    public int Count { get; set; }
}