Public Class ClassList

    Private cName As String ' class name
    Private bTime As String 'beginning time
    Private eTime As String ' ending time

    Private m As Boolean 'monday
    Private tu As Boolean ' tuesday
    Private w As Boolean ' wednesday
    Private th As Boolean ' thursday
    Private f As Boolean 'friday
    Private s As Boolean ' Sat
    Private su As Boolean 'Sun
    'Class Name
    Public Property Name() As String
        Get
            Return cName
        End Get

        Set(ByVal value As String)
            cName = value
        End Set
    End Property

    'Beginning Time
    Public Property BeginTime() As String
        Get
            Return bTime
        End Get

        Set(ByVal value As String)
            bTime = value
        End Set
    End Property

    'Ending Time
    Public Property endTime() As String
        Get
            Return eTime
        End Get

        Set(ByVal value As String)
            eTime = value
        End Set
    End Property

    Public Property mon() As String
        Get
            Return m
        End Get

        Set(ByVal value As String)
            m = value
        End Set
    End Property

    Public Property tue() As String
        Get
            Return tu
        End Get

        Set(ByVal value As String)
            tu = value
        End Set
    End Property

    Public Property wed() As String
        Get
            Return w
        End Get

        Set(ByVal value As String)
            w = value
        End Set
    End Property

    Public Property thu() As String
        Get
            Return th
        End Get

        Set(ByVal value As String)
            th = value
        End Set
    End Property

    Public Property fri() As String
        Get
            Return f
        End Get

        Set(ByVal value As String)
            f = value
        End Set
    End Property

    Public Property sat() As String
        Get
            Return s
        End Get

        Set(ByVal value As String)
            s = value
        End Set
    End Property

    Public Property sun() As String
        Get
            Return su
        End Get

        Set(ByVal value As String)
            su = value
        End Set
    End Property

End Class
