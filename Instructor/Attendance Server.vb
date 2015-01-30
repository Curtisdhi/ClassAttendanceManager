Imports MySql.Data.MySqlClient
Imports System.IO
Imports System.Globalization

Public Class AttendanceServer


    '**********************    PROBLEMS  ****************************************************
    '  VALIDATE user input
    '  Remove / Update User Combo Box selected Content. 
    ' delete db content
    ' modify student content
    '*****************************************************************************************

    '4.22

    '(1-7) integer variables for days of the week - view SQL DB structure for clarification. 
    Dim sun As Integer 'saves as 1 in DB
    Dim mon As Integer 'saves as 2 in DB
    Dim tues As Integer 'saves as 3 in DB
    Dim wed As Integer  'saves as 4 in DB
    Dim thurs As Integer    'saves as 5 in DB
    Dim fri As Integer  'saves as 6 in DB
    Dim sat As Integer  'saves as 7 in DB


    Dim instructor As String
    Dim room As String
    Dim courseName As String
    Dim courseNumber As String
    Dim section As String
    Dim beginDate As Date
    Dim endDate As Date
    Dim beginningTime As String
    Dim endingTime As String
    Dim txtFile As String = "C:\Courses.txt"
    Private output As StreamWriter
    Dim provider As CultureInfo = CultureInfo.InvariantCulture 'used for date time formater
    'collection that stores class properties
    Private cList As New List(Of ClassList)

    'declare connection variable 
    ' Dim myconn As New MySqlConnection("Server=localhost;User Id=root;Password=;Database=Attendance")
    Dim myconn As New MySqlConnection("Server=www.evilscriptmonkeys.com;User Id=capstone;Password=capstone;Database=attendance")

    Private Sub CreateClassList()

        Dim currentClasses As ClassList 'control variable

        ExtractClassName()

        For Each currentClasses In cList
            ClassComboBox.Items.Add(currentClasses.Name)
        Next

    End Sub

    Private Sub ExtractData() 'Sends user comboBox selected input class's data to SQL server

        Dim lines() As String = IO.File.ReadAllLines(txtFile)
        Dim i As Integer = ClassComboBox.SelectedIndex

        MsgBox(lines(i)) 'debug-msg This displays one class info that is highlighted in combobox

        Dim ClassInfo() As String ' Array of Strings to hold user input data in preperation for push to server
        ClassInfo = lines(i).Split(ControlChars.Tab)


        'declare local variables for sql statement

        Dim cName As String = ClassInfo(0) 'class name to upper
        Dim rm As String = ClassInfo(1) 'instructor entered Room number to upper
        Dim cNumber As String = ClassInfo(2) 'course number to upper
        Dim sect As String = ClassInfo(3) 'course section to upper 
        Dim bt As String = ClassInfo(4) 'begin time (HH:mm)
        Dim et As String = ClassInfo(5) 'end time(HH:mm)
        Dim inst As String = ClassInfo(6) 'instructor name
        Dim sunDay As String = ClassInfo(7) 'Days of week, saved as Numerical data type in SQL DB 
        Dim monDay As String = ClassInfo(8)
        Dim tueDay As String = ClassInfo(9)
        Dim wedDay As String = ClassInfo(10)
        Dim thurDay As String = ClassInfo(11)
        Dim friDay As String = ClassInfo(12)
        Dim satDay As String = ClassInfo(13)

        'run sql statement
        Dim cmd As New MySqlCommand
        'myconn.Ping()
        cmd.Connection = myconn

        cmd.CommandText = "INSERT INTO courses VALUES ( @cName ,@rm , @cNumber , @sect , @bt , @et, @inst, @sunDay, @monDay, @tueDay, @wedDay, @thurDay, @friDay, @satDay );"
        cmd.Parameters.AddWithValue("@cName", cName)
        cmd.Parameters.AddWithValue("@rm", rm)
        cmd.Parameters.AddWithValue("@cNumber", cNumber)
        cmd.Parameters.AddWithValue("@sect", sect)
        cmd.Parameters.AddWithValue("@bt", bt)
        cmd.Parameters.AddWithValue("@et", et)
        cmd.Parameters.AddWithValue("@inst", inst)
        cmd.Parameters.AddWithValue("@sunDay", sunDay)
        cmd.Parameters.AddWithValue("@monDay", monDay)
        cmd.Parameters.AddWithValue("@tueDay", tueDay)
        cmd.Parameters.AddWithValue("@wedDay", wedDay)
        cmd.Parameters.AddWithValue("@thurDay", thurDay)
        cmd.Parameters.AddWithValue("@friDay", friDay)
        cmd.Parameters.AddWithValue("@satDay", satDay)

        'cmd.CommandText = "Select * FROM student WHERE Time>= @bt AND Time <= @et;"
        'cmd.Parameters.AddWithValue("@bt", bt)
        'cmd.Parameters.AddWithValue("@et", et)

        myconn.Open()

        'parse read SQL statement to ensure data exist
        Try
            cmd.ExecuteNonQuery()

            MsgBox("Successful Transfer - Select " & cName & " from the dropdown")
            ''code in here for pushing data over
            myconn.Close()

        Catch ex As Exception
            MessageBox.Show("Course Data did not transfer to the server. Please contact your administrator")
            myconn.Close()
        End Try


        '' write information to file
        'Dim StuInfo() As String
        'myconn.Open()
        'Dim reader = cmd.ExecuteReader()
        'While reader.Read()
        '    StuInfo = reader(1).ToString().Split(ControlChars.Tab)

        'End While

        'Try
        '    If StuInfo.Length > 0 Then
        '        Dim MessageString As String = String.Empty
        '        For Each S As String In StuInfo
        '            MessageString &= S & Environment.NewLine
        '        Next
        '        MsgBox(MessageString)
        '        ''code in here for pushing data over
        '        myconn.Close()
        '    End If
        'Catch ex As Exception
        '    MessageBox.Show("No Student Records Found")
        '    myconn.Close()
        'End Try





        'The file will display user names vertically in a row, each time a date was record it wil be displayed in a column to the corresponding username


    End Sub


    Private Sub ExtractClassName()
        'Extracts class name from local File. C:\Courses.txt
        Dim input As StreamReader = New StreamReader(txtFile)
        Dim data() As String
        Dim line As String


        Do Until input.EndOfStream
            line = input.ReadLine
            data = line.Split(ControlChars.Tab)

            Dim c1 As New ClassList()
            c1.Name = data(0)
            cList.Add(c1)
        Loop

        input.Close()



    End Sub


    Private Sub AddClass() 'This method is for adding class data to a local file


        If CourseNameTextBox.Text = "" Then

            MsgBox("Line 189 - Please enter a valid Course Name")

        Else

            courseName = CourseNameTextBox.Text.ToString
            courseNumber = CourseNumberTextBox.ToString
            section = SectionTextBox.ToString
            room = RoomTextBox.ToString
            beginningTime = BeginDateTimePicker.Value.ToString("HH:mm")
            endingTime = EndDateTimePicker.Value.ToString("HH:mm")

            'If statements to check text boxes, store result in int variable
            If SunCheckBox.Checked = True Then
                sun = 1
            End If
            If MonCheckBox.Checked = True Then
                mon = 2
            End If

            If TuesCheckBox.Checked = True Then
                tues = 3
            End If

            If WedCheckBox.Checked = True Then
                wed = 4
            End If

            If ThursCheckBox.Checked = True Then
                thurs = 5
            End If

            If FriCheckBox.Checked = True Then
                fri = 6
            End If

            If SatCheckBox.Checked = True Then
                sat = 7
            End If



            'write with streamwriter
            output = New StreamWriter(txtFile, True)

            ' using StreamWriter to write data to file
            output.Write(CourseNameTextBox.Text.ToString() & ControlChars.Tab)
            output.Write(RoomTextBox.Text.ToString() & ControlChars.Tab)
            output.Write(CourseNumberTextBox.Text.ToString() & ControlChars.Tab)
            output.Write(SectionTextBox.Text.ToString() & ControlChars.Tab)
            output.Write(beginningTime.ToString() & ControlChars.Tab)
            output.Write(endingTime.ToString() & ControlChars.Tab)

            output.Write(InstructorNameTextBox.Text.ToString() & ControlChars.Tab)



            output.Write(sun & ControlChars.Tab)
            output.Write(mon & ControlChars.Tab)
            output.Write(tues & ControlChars.Tab)
            output.Write(wed & ControlChars.Tab)
            output.Write(thurs & ControlChars.Tab)
            output.Write(fri & ControlChars.Tab)
            output.Write(sat & ControlChars.Tab)

            output.Write(BeginDateTimePicker.Value.ToString("ddd d MMM") & ControlChars.Tab)
            output.Write(EndDateTimePicker.Value.ToString("ddd d MMM") & ControlChars.Tab)

            output.WriteLine()
            output.Close()

            ' TestAddClass()


            MsgBox("Successfully added " & courseName & " to you local Course file C:\Courses.txt")
            ClassComboBox.Items.Add(courseName)

            'class has been added, msg has been show, clear contents of text and check boxes
            ClearData()
            AddClassGroupBox.Visible = False

            'reset day variables
            mon = 0
            tues = 0
            wed = 0
            thurs = 0
            fri = 0
            sat = 0
            sun = 0

        End If
    End Sub

    Private Sub PushClass()

        ExtractData()
        ClearData()
        AddClassGroupBox.Visible = False
    End Sub


    Private Sub TestAddClass()

        'TEST *************************************************************************
        MsgBox("Instructor Name: " & instructor & " Course Name: " & courseName & " Course Number: " & courseNumber)
        MsgBox("Section: " & section & " Beginning Date: " & beginDate & " End Date: " & endDate)
        MsgBox(" Beginning Time: " & beginningTime & " Ending time: " & endingTime)
        MsgBox("Mon: " & mon & " Tues: " & tues & " Wed: " & wed & " Thurs: " & thurs & " Fri: " & fri & " Sat: " & sat)

        'TEST *************************************************************************

    End Sub

    Private Sub ClearData() ' this method clears all input

        InstructorNameTextBox.Text = ""
        CourseNameTextBox.Text = ""
        CourseNameTextBox.Text = ""
        CourseNumberTextBox.Text = ""
        SectionTextBox.Text = ""
        RoomTextBox.Text = ""

        MonCheckBox.Checked = False
        TuesCheckBox.Checked = False
        WedCheckBox.Checked = False
        ThursCheckBox.Checked = False
        FriCheckBox.Checked = False
        SatCheckBox.Checked = False

        ClassComboBox.SelectedIndex = -1
        AddClassGroupBox.Visible = False
        AddToServerButton.Visible = False
        AddClassButton.Visible = True

        BeginDateTimePicker.Refresh()
        EndDateTimePicker.Refresh()

    End Sub

    Private Sub AddClassButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddClassButton.Click

        AddClass()
        ExtractClassName()
        AddClassButton.Visible = True

    End Sub


    Private Sub AddNewClassButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddNewClassButton.Click
        ClearData()
        ClassComboBox.SelectedIndex = -1

        AddClassGroupBox.Visible = True
        AddToServerButton.Visible = False

    End Sub

    Private Sub ViewFileButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ViewFileButton.Click

        Dim lines() As String = IO.File.ReadAllLines(txtFile)
            Dim i As Integer = ClassComboBox.SelectedIndex
        Try
            MsgBox("Debugging Message Line 351 -Selected Class -" & lines(i)) 'debug-msg This displays one class info that is highlighted in combobox
        Catch ex As Exception
            MsgBox("Please select a course")

        End Try


            Dim ClassInfo() As String ' Array of Strings to hold read file selected line
            ClassInfo = lines(i).Split(ControlChars.Tab)

            AddClassGroupBox.Visible = True ' make class box visible
            AddClassButton.Visible = False 'remove add class to file button
            AddToServerButton.Visible = True ' make Add to server button visible

            'Set Text to Course Array Index Values based on user selection
            CourseNameTextBox.Text = ClassInfo(0)
            RoomTextBox.Text = ClassInfo(1) 'instructor entered Room number to upper
            CourseNumberTextBox.Text = ClassInfo(2) 'course number to upper
            SectionTextBox.Text = ClassInfo(3) 'course section to upper 
            InstructorNameTextBox.Text = ClassInfo(6) 'instructor name

            'Determine Days of week useing Numerical DB Dataype Format
            If ClassInfo(7) = 1 Then
                SunCheckBox.Checked = True
            End If
            If ClassInfo(8) = 2 Then
                MonCheckBox.Checked = True
            End If
            If ClassInfo(9) = 3 Then
                TuesCheckBox.Checked = True
            End If
            If ClassInfo(10) = 4 Then
                WedCheckBox.Checked = True
            End If
            If ClassInfo(11) = 5 Then
                ThursCheckBox.Checked = True
            End If
            If ClassInfo(12) = 6 Then
                FriCheckBox.Checked = True
            End If
            If ClassInfo(13) = 7 Then
                SatCheckBox.Checked = True
            End If

            Dim beginDate As String = ClassInfo(14) 'Begin Date timestamp
            Dim endDate As String = ClassInfo(15)   'End Date timestamp

            BeginDateTimePicker.Value = CDate(beginDate) & ", " & ClassInfo(4) 'begin time (HH:mm)
            EndDateTimePicker.Value = CDate(endDate) & ", " & ClassInfo(5) 'end time(HH:mm)


    End Sub

    Private Sub AttendanceServer_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load


        If File.Exists(txtFile) Then
            CreateClassList()

        Else
            Dim sw As StreamWriter = File.CreateText(txtFile)
            sw.Close()
            CreateClassList()
        End If

    End Sub

    Private Sub AddToServerButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddToServerButton.Click
        PushClass()
    End Sub

    Private Sub ClrClassButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ClrClassButton.Click
        ClearData()

    End Sub

    Private Function IsNullOrEmpty(ByVal lines As String) As Boolean
        Throw New NotImplementedException
    End Function

End Class
