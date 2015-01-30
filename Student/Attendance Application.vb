Imports System.Net
Imports MySql.Data.MySqlClient


Public Class Form1

    Dim MysqlConn As MySqlConnection
    Dim time As String = System.DateTime.Now.ToString(("hh:mm:ss"))

    'dayOfWeek stores a number between 1-7
    '1 = sunday    2 = monday  3 = tuesday etc
    Dim selectedDate As Date = Date.Now
    Dim dayOfWeek As Integer = Weekday(selectedDate)
    Public stuNumber As Double

    Public userName As String = System.Environment.UserName.ToString
    Dim strHostName As String = System.Net.Dns.GetHostName()
    Dim courseName As String
    Dim roomName As String = strHostName.Substring(0, 5)

    Private Sub RecordAttendanceButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RecordAttendanceButton.Click

        MysqlConn = New MySqlConnection()
        MysqlConn.ConnectionString = "server=www.Evilscriptmonkeys.com;user id=capstone;password=capstone;database=attendance"
        'MysqlConn.ConnectionString = "server=csciapps.northeaststate.edu;user id=capstone;password=capstone;database=attendance"

        Try
            MysqlConn.Open()

            getCourseName()
            MessageBox.Show(courseName)
            RecordAttendance()

            Application.Exit()

        Catch myerror As MySqlException 'error occured
            MessageBox.Show("Cannot connect to database: " & myerror.Message)
            MsgBox("If this error persists please notify your instructor.")

        Finally 'perform always
            MysqlConn.Dispose()
            MysqlConn.Close()

        End Try


    End Sub

    Private Sub RecordAttendance()

        ' Verify student exists
        DoesStudentExist()

        Dim cmd As New MySqlCommand()
        cmd.Connection = MysqlConn


        'SQL Statement
        cmd.CommandText = "INSERT INTO student (DayOfWeek, UserName, StuNumber, PcName, Time, Date, CourseName) VALUES (@DayOfWeek, @user, @stuNumber, @pc, @time, CURDATE(), @course);"

        'insert variable values into sql string
        cmd.Parameters.AddWithValue("@DayOfWeek", dayOfWeek)
        cmd.Parameters.AddWithValue("@user", userName)
        cmd.Parameters.AddWithValue("@stuNumber", stuNumber)
        cmd.Parameters.AddWithValue("@pc", strHostName)
        cmd.Parameters.AddWithValue("@time", time)
        cmd.Parameters.AddWithValue("@course", courseName)
        'perform query
        cmd.Prepare()
        cmd.ExecuteNonQuery()
        MessageBox.Show("Your attendance has been recorded.", "Attendance Recorded!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk)

    End Sub

    Private Sub DoesStudentExist()

        Dim cmd As New MySqlCommand
        cmd.Connection = MysqlConn

        ' SQL query to select the username
        ' if you add a WHERE clause into this statement, and add a parameter that does not exist into the database, the error message will occur
        ' This simulates the non-existance of a student for testing purposes and avoids truncating the table 
        cmd.CommandText = "SELECT UserName FROM students;"
        cmd.Prepare()
        cmd.ExecuteNonQuery()

        ' reader reads from database and stores the username into String variable student for testing
        Dim reader = cmd.ExecuteReader()
        Dim student As String
        While reader.Read()
            student = reader(0).ToString()
        End While
        reader.Close()
        ' tests if the username and the retrieved username (student) are equal
        If userName <> student Then
            ' opens registration dialog for student to register the first time
            MessageBox.Show(userName + " does not exist!", "Not Registered!", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Dim reg As New RegisterUser
            reg.ShowDialog()
        End If

        ' if student does exist, this will pull the 9000 number from the students table and plug it into the "class of attendance" table
        cmd.CommandText = "SELECT StuNumber FROM students WHERE UserName = @userName;"
        cmd.Parameters.AddWithValue("@userName", userName)

        cmd.Prepare()
        cmd.ExecuteNonQuery()
        reader = cmd.ExecuteReader()
        While reader.Read()
            stuNumber = reader(0)
        End While

        reader.Close()
    End Sub

    ' Gets correct course from courses table to ensure student is correctly entered into correct course, on day, time, and room number
    Private Sub getCourseName()
        Dim cmd As New MySqlCommand
        cmd.Connection = MysqlConn

        cmd.CommandText = "SELECT CourseName FROM courses WHERE @time BETWEEN StartTime AND EndTime AND RoomNumber = @roomName AND (@DayOfWeek = Sunday OR @DayOfWeek = Monday OR @DayOfWeek = Tuesday OR @DayOfWeek = Wednesday OR @DayOfWeek = Thursday OR @DayOfWeek = Friday OR @DayOfWeek = Saturday );"
        cmd.Parameters.AddWithValue("@roomName", roomName)
        cmd.Parameters.AddWithValue("@time", time)
        cmd.Parameters.AddWithValue("@DayOfWeek", dayOfWeek)
        cmd.Prepare()
        cmd.ExecuteNonQuery()

        Dim reader = cmd.ExecuteReader()
        Dim course As String
        While reader.Read()
            course = reader(0).ToString()
        End While
        reader.Close()

        courseName = course

    End Sub
End Class

