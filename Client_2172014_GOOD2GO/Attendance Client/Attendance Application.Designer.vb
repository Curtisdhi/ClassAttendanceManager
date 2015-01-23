<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
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
        Me.RecordAttendanceButton = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'RecordAttendanceButton
        '
        Me.RecordAttendanceButton.Font = New System.Drawing.Font("Verdana", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RecordAttendanceButton.Location = New System.Drawing.Point(12, 13)
        Me.RecordAttendanceButton.Name = "RecordAttendanceButton"
        Me.RecordAttendanceButton.Size = New System.Drawing.Size(213, 98)
        Me.RecordAttendanceButton.TabIndex = 0
        Me.RecordAttendanceButton.Text = "Push to Record Attendance"
        Me.RecordAttendanceButton.UseVisualStyleBackColor = True
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(237, 124)
        Me.Controls.Add(Me.RecordAttendanceButton)
        Me.Name = "Form1"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Attendance App"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents RecordAttendanceButton As System.Windows.Forms.Button

End Class
