Imports MySql.Data.MySqlClient


Public Class DatabaseLogin

    'define sql connection
    Dim MysqlConn As MySqlConnection

    Private Sub OK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK.Click

        'create connection object
        MysqlConn = New MySqlConnection()

        'store user input into variables
        Dim userName As String = UsernameTextBox.Text.Trim
        Dim password As String = PasswordTextBox.Text.Trim
        Dim passed As Boolean = True


        'log in infomation stored as connection string
        MysqlConn.ConnectionString = "server=www.evilscriptmonkeys.com;" _
        & "user id=" & userName & ";" _
        & "password=" & password & ";" _
        & "database=attendance"

        'begin exception handling
      
            Try
                MysqlConn.Open()
                MessageBox.Show("Connection Successful.")
                MysqlConn.Close()

            Catch myerror As MySqlException 'error occured
                MessageBox.Show("Cannot connect to database: " & myerror.Message)
                passed = False


            Finally 'perform always
                MysqlConn.Dispose()

            End Try



        'If passed = True Then

        'connected - show DB form
        AttendanceServer.Show()
        Me.Close() 'close the login form

        ' End If


    End Sub

    Private Sub Cancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel.Click

        Application.Exit()

    End Sub

End Class
