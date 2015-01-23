Imports System.Net
Imports MySql.Data.MySqlClient

Public Class RegisterUser
    Public stuNumber As Double
    Public userName As String = Form1.userName

    Private Sub btnRegister_Click(sender As System.Object, e As System.EventArgs) Handles btnRegister.Click
        Dim conn As New MySqlConnection()   ' connection object
        Dim cmd As New MySqlCommand         ' sql command object
        Dim c() As Char = txtStuNumber.Text.ToCharArray ' for testing whitespace characters
        Dim flag As Boolean = False ' flag for whitespace detection

        ' loop for detecting whitespace
        For i As Integer = 0 To c.Length - 1
            If c(i) = " " Then
                flag = True
            End If
        Next

        ' open the connection
        conn.ConnectionString = "server=www.Evilscriptmonkeys.com;user id=capstone;password=capstone;database=attendance"
        conn.Open()

        ' apply connection to command object
        cmd.Connection = conn

        ' test the length and required starting numbers for the input
        If (txtStuNumber.TextLength <> 9) Then
            MessageBox.Show("Your student number must be 9 digits long. Please re-enter.")
            txtStuNumber.Clear()
            txtStuNumber.Focus()
        ElseIf (Not txtStuNumber.Text.StartsWith("90")) Then
            MessageBox.Show("Your student number must begin with 90. Please re-enter.")
            txtStuNumber.Clear()
            txtStuNumber.Focus()
        ElseIf Not Integer.TryParse(txtStuNumber.Text, stuNumber) Then
            MessageBox.Show("You must enter only numeric digits.")
            txtStuNumber.Clear()
            txtStuNumber.Focus()
        ElseIf flag Then
            MessageBox.Show("There should be no spaces in your student number. Please re-enter.")
            txtStuNumber.Clear()
            txtStuNumber.Focus()
        Else
            ' if data is verified, continue to register user
            stuNumber = CInt(txtStuNumber.Text)
            Form1.stuNumber = stuNumber
            cmd.CommandText = "INSERT INTO students (UserName, StuNumber) VALUES (@userName, @stuNumber);"

            cmd.Parameters.AddWithValue("@userName", userName)
            cmd.Parameters.AddWithValue("@stuNumber", stuNumber)

            cmd.Prepare()
            cmd.ExecuteNonQuery()

            MessageBox.Show("You have been registered. Welcome to Northeast State!", "Registered!", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Me.Close()
        End If
        conn.Close()
    End Sub

End Class