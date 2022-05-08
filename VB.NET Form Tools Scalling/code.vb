  Dim ProportionsArray() As CtrlProportions
 
    Private Structure CtrlProportions
        Dim HeightProportions As Double
        Dim WidthProportions As Double
        Dim TopProportions As Double
        Dim LeftProportions As Double
    End Structure
 
    Private Sub Form1_HandleCreated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.HandleCreated
        On Error Resume Next
        Application.DoEvents()
        ReDim ProportionsArray(0 To Controls.Count - 1)
        For I As Integer = 0 To Controls.Count - 1
            With ProportionsArray(I)
                .HeightProportions = Controls(I).Height / Height
                .WidthProportions = Controls(I).Width / Width
                .TopProportions = Controls(I).Top / Height
                .LeftProportions = Controls(I).Left / Width
            End With
        Next
    End Sub
   
    Private Sub Form1_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize
        On Error Resume Next
        For I As Integer = 0 To Controls.Count - 1
            Controls(I).Left = ProportionsArray(I).LeftProportions * Me.Width
            Controls(I).Top = ProportionsArray(I).TopProportions * Me.Height
            Controls(I).Width = ProportionsArray(I).WidthProportions * Me.Width
            Controls(I).Height = ProportionsArray(I).HeightProportions * Me.Height
        Next
    End Sub

