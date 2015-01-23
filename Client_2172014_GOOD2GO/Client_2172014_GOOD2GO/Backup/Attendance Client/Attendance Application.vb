Imports System.Net
Imports MySql.Data.MySqlClient

Public Class Form1

    Dim MysqlConn As MySqlConnection
    Dim time As Date = time.Now
    Dim userName As String = System.Environment.UserName.ToString
    Dim strHostName As String = System.Net.Dns.GetHostName()

    Private Sub RecordAttendanceButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RecordAttendanceButton.Click

        'hardcoded connection, not a good idea,  can be reverse engineered
        'need to set special user for client version.  CAN ONLY WRITE TO SPECIFIED FIELDS
        MysqlConn = New MySqlConnection()
        MysqlConn.ConnectionString = "server=localhost;user id=root;password=;database=attendance"

        'Add try, catch, Finally block here.  Needs to connect to db, insert values, close connection, display confirmation.

        TextBox1.Text = userName
        TextBox4.Text = strHostName
        TextBox3.Text = time

    End Sub
End Class
