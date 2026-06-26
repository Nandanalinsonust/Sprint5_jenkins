public class DashboardCountDto
{
    public int TotalDoctors { get; set; }
    public int TotalPatients { get; set; }
    public int TotalAppointments { get; set; }
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
public class UserDto
{
    public string Id { get; set; }
    public string Email { get; set; }
    public string Role { get; set; }
}