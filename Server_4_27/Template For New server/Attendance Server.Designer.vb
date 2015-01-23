<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class AttendanceServer
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.MonCheckBox = New System.Windows.Forms.CheckBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.ClassComboBox = New System.Windows.Forms.ComboBox()
        Me.AddClassButton = New System.Windows.Forms.Button()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.TuesCheckBox = New System.Windows.Forms.CheckBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.WedCheckBox = New System.Windows.Forms.CheckBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.ThursCheckBox = New System.Windows.Forms.CheckBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.FriCheckBox = New System.Windows.Forms.CheckBox()
        Me.InstructorNameTextBox = New System.Windows.Forms.TextBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.ViewFileButton = New System.Windows.Forms.Button()
        Me.instructionLabel = New System.Windows.Forms.Label()
        Me.AddNewClassButton = New System.Windows.Forms.Button()
        Me.AddClassGroupBox = New System.Windows.Forms.GroupBox()
        Me.AddToServerButton = New System.Windows.Forms.Button()
        Me.SunCheckBox = New System.Windows.Forms.CheckBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.RoomTextBox = New System.Windows.Forms.TextBox()
        Me.Label19 = New System.Windows.Forms.Label()
        Me.Label18 = New System.Windows.Forms.Label()
        Me.EndDateTimePicker = New System.Windows.Forms.DateTimePicker()
        Me.BeginDateTimePicker = New System.Windows.Forms.DateTimePicker()
        Me.Label17 = New System.Windows.Forms.Label()
        Me.CourseNumberTextBox = New System.Windows.Forms.TextBox()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.SectionTextBox = New System.Windows.Forms.TextBox()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.CourseNameTextBox = New System.Windows.Forms.TextBox()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.SatCheckBox = New System.Windows.Forms.CheckBox()
        Me.ClrClassButton = New System.Windows.Forms.Button()
        Me.AddClassGroupBox.SuspendLayout()
        Me.SuspendLayout()
        '
        'MonCheckBox
        '
        Me.MonCheckBox.AutoSize = True
        Me.MonCheckBox.Location = New System.Drawing.Point(89, 238)
        Me.MonCheckBox.Name = "MonCheckBox"
        Me.MonCheckBox.Size = New System.Drawing.Size(15, 14)
        Me.MonCheckBox.TabIndex = 11
        Me.MonCheckBox.TabStop = False
        Me.MonCheckBox.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(32, 238)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(48, 13)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Monday:"
        '
        'ClassComboBox
        '
        Me.ClassComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ClassComboBox.FormattingEnabled = True
        Me.ClassComboBox.Location = New System.Drawing.Point(94, 6)
        Me.ClassComboBox.Name = "ClassComboBox"
        Me.ClassComboBox.Size = New System.Drawing.Size(129, 21)
        Me.ClassComboBox.TabIndex = 1
        '
        'AddClassButton
        '
        Me.AddClassButton.Location = New System.Drawing.Point(424, 267)
        Me.AddClassButton.Name = "AddClassButton"
        Me.AddClassButton.Size = New System.Drawing.Size(126, 23)
        Me.AddClassButton.TabIndex = 15
        Me.AddClassButton.Text = "Add Class to File"
        Me.AddClassButton.UseVisualStyleBackColor = True
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(110, 238)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(51, 13)
        Me.Label3.TabIndex = 13
        Me.Label3.Text = "Tuesday:"
        '
        'TuesCheckBox
        '
        Me.TuesCheckBox.AutoSize = True
        Me.TuesCheckBox.Location = New System.Drawing.Point(167, 238)
        Me.TuesCheckBox.Name = "TuesCheckBox"
        Me.TuesCheckBox.Size = New System.Drawing.Size(15, 14)
        Me.TuesCheckBox.TabIndex = 12
        Me.TuesCheckBox.TabStop = False
        Me.TuesCheckBox.UseVisualStyleBackColor = True
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(188, 238)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(67, 13)
        Me.Label4.TabIndex = 21
        Me.Label4.Text = "Wednesday:"
        '
        'WedCheckBox
        '
        Me.WedCheckBox.AutoSize = True
        Me.WedCheckBox.Location = New System.Drawing.Point(261, 238)
        Me.WedCheckBox.Name = "WedCheckBox"
        Me.WedCheckBox.Size = New System.Drawing.Size(15, 14)
        Me.WedCheckBox.TabIndex = 13
        Me.WedCheckBox.TabStop = False
        Me.WedCheckBox.UseVisualStyleBackColor = True
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(282, 238)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(54, 13)
        Me.Label5.TabIndex = 23
        Me.Label5.Text = "Thursday:"
        '
        'ThursCheckBox
        '
        Me.ThursCheckBox.AutoSize = True
        Me.ThursCheckBox.Location = New System.Drawing.Point(342, 238)
        Me.ThursCheckBox.Name = "ThursCheckBox"
        Me.ThursCheckBox.Size = New System.Drawing.Size(15, 14)
        Me.ThursCheckBox.TabIndex = 14
        Me.ThursCheckBox.TabStop = False
        Me.ThursCheckBox.UseVisualStyleBackColor = True
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(363, 238)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(38, 13)
        Me.Label7.TabIndex = 25
        Me.Label7.Text = "Friday:"
        '
        'FriCheckBox
        '
        Me.FriCheckBox.AutoSize = True
        Me.FriCheckBox.Location = New System.Drawing.Point(407, 238)
        Me.FriCheckBox.Name = "FriCheckBox"
        Me.FriCheckBox.Size = New System.Drawing.Size(15, 14)
        Me.FriCheckBox.TabIndex = 15
        Me.FriCheckBox.TabStop = False
        Me.FriCheckBox.UseVisualStyleBackColor = True
        '
        'InstructorNameTextBox
        '
        Me.InstructorNameTextBox.Location = New System.Drawing.Point(105, 21)
        Me.InstructorNameTextBox.Name = "InstructorNameTextBox"
        Me.InstructorNameTextBox.Size = New System.Drawing.Size(122, 20)
        Me.InstructorNameTextBox.TabIndex = 4
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.ForeColor = System.Drawing.Color.DarkRed
        Me.Label8.Location = New System.Drawing.Point(14, 28)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(85, 13)
        Me.Label8.TabIndex = 1
        Me.Label8.Text = "Instructor Name:"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(12, 9)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(76, 13)
        Me.Label9.TabIndex = 28
        Me.Label9.Text = "Select a class:"
        '
        'ViewFileButton
        '
        Me.ViewFileButton.Location = New System.Drawing.Point(234, 4)
        Me.ViewFileButton.Name = "ViewFileButton"
        Me.ViewFileButton.Size = New System.Drawing.Size(106, 44)
        Me.ViewFileButton.TabIndex = 2
        Me.ViewFileButton.Text = "View Class"
        Me.ViewFileButton.UseVisualStyleBackColor = True
        '
        'instructionLabel
        '
        Me.instructionLabel.AutoSize = True
        Me.instructionLabel.ForeColor = System.Drawing.Color.DarkRed
        Me.instructionLabel.Location = New System.Drawing.Point(33, 127)
        Me.instructionLabel.Name = "instructionLabel"
        Me.instructionLabel.Size = New System.Drawing.Size(471, 13)
        Me.instructionLabel.TabIndex = 35
        Me.instructionLabel.Text = "Check the boxes of day meeting, input a class name, and range of meeting times.  " & _
            "Click Add Class."
        '
        'AddNewClassButton
        '
        Me.AddNewClassButton.Location = New System.Drawing.Point(346, 4)
        Me.AddNewClassButton.Name = "AddNewClassButton"
        Me.AddNewClassButton.Size = New System.Drawing.Size(113, 44)
        Me.AddNewClassButton.TabIndex = 3
        Me.AddNewClassButton.Text = "Create New Class"
        Me.AddNewClassButton.UseVisualStyleBackColor = True
        '
        'AddClassGroupBox
        '
        Me.AddClassGroupBox.Controls.Add(Me.AddToServerButton)
        Me.AddClassGroupBox.Controls.Add(Me.SunCheckBox)
        Me.AddClassGroupBox.Controls.Add(Me.Label6)
        Me.AddClassGroupBox.Controls.Add(Me.Label2)
        Me.AddClassGroupBox.Controls.Add(Me.RoomTextBox)
        Me.AddClassGroupBox.Controls.Add(Me.Label19)
        Me.AddClassGroupBox.Controls.Add(Me.Label18)
        Me.AddClassGroupBox.Controls.Add(Me.EndDateTimePicker)
        Me.AddClassGroupBox.Controls.Add(Me.BeginDateTimePicker)
        Me.AddClassGroupBox.Controls.Add(Me.Label17)
        Me.AddClassGroupBox.Controls.Add(Me.CourseNumberTextBox)
        Me.AddClassGroupBox.Controls.Add(Me.Label16)
        Me.AddClassGroupBox.Controls.Add(Me.SectionTextBox)
        Me.AddClassGroupBox.Controls.Add(Me.Label15)
        Me.AddClassGroupBox.Controls.Add(Me.CourseNameTextBox)
        Me.AddClassGroupBox.Controls.Add(Me.Label10)
        Me.AddClassGroupBox.Controls.Add(Me.SatCheckBox)
        Me.AddClassGroupBox.Controls.Add(Me.instructionLabel)
        Me.AddClassGroupBox.Controls.Add(Me.MonCheckBox)
        Me.AddClassGroupBox.Controls.Add(Me.Label1)
        Me.AddClassGroupBox.Controls.Add(Me.AddClassButton)
        Me.AddClassGroupBox.Controls.Add(Me.TuesCheckBox)
        Me.AddClassGroupBox.Controls.Add(Me.Label3)
        Me.AddClassGroupBox.Controls.Add(Me.Label8)
        Me.AddClassGroupBox.Controls.Add(Me.WedCheckBox)
        Me.AddClassGroupBox.Controls.Add(Me.InstructorNameTextBox)
        Me.AddClassGroupBox.Controls.Add(Me.Label4)
        Me.AddClassGroupBox.Controls.Add(Me.Label7)
        Me.AddClassGroupBox.Controls.Add(Me.ThursCheckBox)
        Me.AddClassGroupBox.Controls.Add(Me.FriCheckBox)
        Me.AddClassGroupBox.Controls.Add(Me.Label5)
        Me.AddClassGroupBox.Location = New System.Drawing.Point(15, 54)
        Me.AddClassGroupBox.Name = "AddClassGroupBox"
        Me.AddClassGroupBox.Size = New System.Drawing.Size(556, 292)
        Me.AddClassGroupBox.TabIndex = 36
        Me.AddClassGroupBox.TabStop = False
        Me.AddClassGroupBox.Text = "Add Class"
        Me.AddClassGroupBox.Visible = False
        '
        'AddToServerButton
        '
        Me.AddToServerButton.Location = New System.Drawing.Point(366, 267)
        Me.AddToServerButton.Name = "AddToServerButton"
        Me.AddToServerButton.Size = New System.Drawing.Size(184, 23)
        Me.AddToServerButton.TabIndex = 0
        Me.AddToServerButton.Text = "Save Class to Records Server"
        Me.AddToServerButton.UseVisualStyleBackColor = True
        '
        'SunCheckBox
        '
        Me.SunCheckBox.AutoSize = True
        Me.SunCheckBox.Location = New System.Drawing.Point(90, 267)
        Me.SunCheckBox.Name = "SunCheckBox"
        Me.SunCheckBox.Size = New System.Drawing.Size(15, 14)
        Me.SunCheckBox.TabIndex = 17
        Me.SunCheckBox.TabStop = False
        Me.SunCheckBox.UseVisualStyleBackColor = True
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(33, 267)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(46, 13)
        Me.Label6.TabIndex = 57
        Me.Label6.Text = "Sunday:"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.ForeColor = System.Drawing.Color.DarkRed
        Me.Label2.Location = New System.Drawing.Point(240, 28)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(78, 13)
        Me.Label2.TabIndex = 56
        Me.Label2.Text = "Room Number:"
        '
        'RoomTextBox
        '
        Me.RoomTextBox.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.RoomTextBox.Location = New System.Drawing.Point(324, 21)
        Me.RoomTextBox.Name = "RoomTextBox"
        Me.RoomTextBox.Size = New System.Drawing.Size(77, 20)
        Me.RoomTextBox.TabIndex = 5
        '
        'Label19
        '
        Me.Label19.AutoSize = True
        Me.Label19.ForeColor = System.Drawing.Color.DarkRed
        Me.Label19.Location = New System.Drawing.Point(339, 178)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(168, 13)
        Me.Label19.TabIndex = 54
        Me.Label19.Text = "Class Ending Date ,  Ending Time:"
        '
        'Label18
        '
        Me.Label18.AutoSize = True
        Me.Label18.ForeColor = System.Drawing.Color.DarkRed
        Me.Label18.Location = New System.Drawing.Point(33, 178)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(162, 13)
        Me.Label18.TabIndex = 53
        Me.Label18.Text = "Class begining date ,  Start Time:"
        '
        'EndDateTimePicker
        '
        Me.EndDateTimePicker.CustomFormat = "ddd d MMM HH:mm"
        Me.EndDateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.EndDateTimePicker.Location = New System.Drawing.Point(333, 194)
        Me.EndDateTimePicker.Name = "EndDateTimePicker"
        Me.EndDateTimePicker.Size = New System.Drawing.Size(200, 20)
        Me.EndDateTimePicker.TabIndex = 10
        '
        'BeginDateTimePicker
        '
        Me.BeginDateTimePicker.CustomFormat = "ddd d MMM HH:mm"
        Me.BeginDateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.BeginDateTimePicker.Location = New System.Drawing.Point(28, 194)
        Me.BeginDateTimePicker.Name = "BeginDateTimePicker"
        Me.BeginDateTimePicker.Size = New System.Drawing.Size(200, 20)
        Me.BeginDateTimePicker.TabIndex = 9
        '
        'Label17
        '
        Me.Label17.AutoSize = True
        Me.Label17.ForeColor = System.Drawing.Color.DarkRed
        Me.Label17.Location = New System.Drawing.Point(14, 76)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(83, 13)
        Me.Label17.TabIndex = 50
        Me.Label17.Text = "Course Number:"
        '
        'CourseNumberTextBox
        '
        Me.CourseNumberTextBox.AcceptsTab = True
        Me.CourseNumberTextBox.Location = New System.Drawing.Point(105, 73)
        Me.CourseNumberTextBox.Name = "CourseNumberTextBox"
        Me.CourseNumberTextBox.Size = New System.Drawing.Size(122, 20)
        Me.CourseNumberTextBox.TabIndex = 7
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.ForeColor = System.Drawing.Color.DarkRed
        Me.Label16.Location = New System.Drawing.Point(51, 102)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(46, 13)
        Me.Label16.TabIndex = 48
        Me.Label16.Text = "Section:"
        '
        'SectionTextBox
        '
        Me.SectionTextBox.AcceptsTab = True
        Me.SectionTextBox.Location = New System.Drawing.Point(105, 99)
        Me.SectionTextBox.Name = "SectionTextBox"
        Me.SectionTextBox.Size = New System.Drawing.Size(122, 20)
        Me.SectionTextBox.TabIndex = 8
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.ForeColor = System.Drawing.Color.DarkRed
        Me.Label15.Location = New System.Drawing.Point(25, 54)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(74, 13)
        Me.Label15.TabIndex = 46
        Me.Label15.Text = "Course Name:"
        '
        'CourseNameTextBox
        '
        Me.CourseNameTextBox.AcceptsTab = True
        Me.CourseNameTextBox.Location = New System.Drawing.Point(105, 47)
        Me.CourseNameTextBox.Name = "CourseNameTextBox"
        Me.CourseNameTextBox.Size = New System.Drawing.Size(220, 20)
        Me.CourseNameTextBox.TabIndex = 6
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(428, 238)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(52, 13)
        Me.Label10.TabIndex = 44
        Me.Label10.Text = "Saturday:"
        '
        'SatCheckBox
        '
        Me.SatCheckBox.AutoSize = True
        Me.SatCheckBox.Location = New System.Drawing.Point(486, 238)
        Me.SatCheckBox.Name = "SatCheckBox"
        Me.SatCheckBox.Size = New System.Drawing.Size(15, 14)
        Me.SatCheckBox.TabIndex = 16
        Me.SatCheckBox.TabStop = False
        Me.SatCheckBox.UseVisualStyleBackColor = True
        '
        'ClrClassButton
        '
        Me.ClrClassButton.Location = New System.Drawing.Point(465, 6)
        Me.ClrClassButton.Name = "ClrClassButton"
        Me.ClrClassButton.Size = New System.Drawing.Size(106, 44)
        Me.ClrClassButton.TabIndex = 37
        Me.ClrClassButton.Text = "Clear Class"
        Me.ClrClassButton.UseVisualStyleBackColor = True
        '
        'AttendanceServer
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(583, 356)
        Me.Controls.Add(Me.ClrClassButton)
        Me.Controls.Add(Me.AddClassGroupBox)
        Me.Controls.Add(Me.AddNewClassButton)
        Me.Controls.Add(Me.ViewFileButton)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.ClassComboBox)
        Me.Name = "AttendanceServer"
        Me.Text = "Attendance"
        Me.AddClassGroupBox.ResumeLayout(False)
        Me.AddClassGroupBox.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents MonCheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents ClassComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents AddClassButton As System.Windows.Forms.Button
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents TuesCheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents WedCheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents ThursCheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents FriCheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents InstructorNameTextBox As System.Windows.Forms.TextBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents ViewFileButton As System.Windows.Forms.Button
    Friend WithEvents instructionLabel As System.Windows.Forms.Label
    Friend WithEvents AddNewClassButton As System.Windows.Forms.Button
    Friend WithEvents AddClassGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents SatCheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents CourseNumberTextBox As System.Windows.Forms.TextBox
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents SectionTextBox As System.Windows.Forms.TextBox
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents CourseNameTextBox As System.Windows.Forms.TextBox
    Friend WithEvents BeginDateTimePicker As System.Windows.Forms.DateTimePicker
    Friend WithEvents EndDateTimePicker As System.Windows.Forms.DateTimePicker
    Friend WithEvents Label19 As System.Windows.Forms.Label
    Friend WithEvents Label18 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents RoomTextBox As System.Windows.Forms.TextBox
    Friend WithEvents SunCheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents AddToServerButton As System.Windows.Forms.Button
    Friend WithEvents ClrClassButton As System.Windows.Forms.Button

End Class
