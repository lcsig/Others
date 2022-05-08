	'//////////////////////////////////////////////////////////////
	'// WebCam Class 
	'// Purpose : Simple Class TO Capturing Images From WebCam Using Windows API's
	'// Language : Visual Basic .NET
	'///////////////////////////////////////////////////////////////////////////

'// Class ...
Imports System.Runtime.InteropServices
Public Class Cam

    Private Frm As Form
    Private m_CapHwnd As IntPtr
    Private PicBox As PictureBox
    Public Event ErrorThrow(ErrText As String)
    Public Event Started(YesOrNo As Boolean, ErrText As String)
    Public Event Stoped(YesOrNo As Boolean, ErrText As String)

    <DllImport("User32")> _
    Private Shared Function SendMessageA(hWnd As IntPtr, Msg As Int32, wParam As Int32, lParam As Int32) As Int32
    End Function
    <DllImport("Avicap32")> _
    Private Shared Function capCreateCaptureWindowA(lpszWindowName As String, dwStyle As Int32, x As Int32, y As Int32, _
                                                    nWidth As Int32, nHeight As Int32, hwndParent As IntPtr, nID As Int32) As Int32
    End Function
    Const WM_CAP_CONNECT As Int32 = 1034
    Const WM_CAP_DISCONNECT As Int32 = 1035
    Const WM_CAP_GT_FRAME As Int32 = 1084
    Const WM_CAP_COPY As Int32 = 1054

    Dim Time As Threading.Thread
    Private Interval As Int32
    Sub Timers()
RE:     Try
            SendMessageA(m_CapHwnd, WM_CAP_GT_FRAME, 0, 0)
            SendMessageA(m_CapHwnd, WM_CAP_COPY, 0, 0)
            SetImg()
            Threading.Thread.Sleep(Interval)
        Catch ex As Exception
            RaiseEvent ErrorThrow(ex.Message)
            [Stop]()
        End Try

        GoTo RE
    End Sub

    Delegate Sub SetImg_CallBack()
    Sub SetImg()
        Try
            If Frm.InvokeRequired Then
                Frm.Invoke(New SetImg_CallBack(AddressOf SetImg), New Object() {})
                Exit Sub
            End If
            Dim img As Image = Clipboard.GetData(DataFormats.Bitmap)
            Application.DoEvents()
            '// Resize ...


            PicBox.Image = ResizeImage(img, New Point(PicBox.Width, PicBox.Height))
        Catch ex As Exception
            RaiseEvent ErrorThrow(ex.Message)
            [Stop]()
        End Try
    End Sub

    Public Shared Function ResizeImage(ByVal image As Image, _
  ByVal size As Size, Optional ByVal preserveAspectRatio As Boolean = True) As Image
        Dim newWidth As Integer
        Dim newHeight As Integer
        If preserveAspectRatio Then
            Dim originalWidth As Integer = image.Width
            Dim originalHeight As Integer = image.Height
            Dim percentWidth As Single = CSng(size.Width) / CSng(originalWidth)
            Dim percentHeight As Single = CSng(size.Height) / CSng(originalHeight)
            Dim percent As Single = If(percentHeight < percentWidth,
                    percentHeight, percentWidth)
            newWidth = CInt(originalWidth * percent)
            newHeight = CInt(originalHeight * percent)
        Else
            newWidth = size.Width
            newHeight = size.Height
        End If
        Dim newImage As Image = New Bitmap(newWidth, newHeight)
        Using graphicsHandle As Graphics = Graphics.FromImage(newImage)
            graphicsHandle.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
            graphicsHandle.DrawImage(image, 0, 0, newWidth, newHeight)
        End Using
        Return newImage
    End Function

    Public Sub Start(FormHandle As Form, ImgBox As PictureBox)
        Try
            Frm = FormHandle
            If m_CapHwnd <> 0 Then Me.[Stop]()
            PicBox = ImgBox
            m_CapHwnd = capCreateCaptureWindowA("WebCap", 0, 0, 0, 700, 700, Frm.Handle, 0)
            SendMessageA(m_CapHwnd, WM_CAP_CONNECT, 0, 0)
            System.Threading.Thread.Sleep(150)
            If SendMessageA(m_CapHwnd, WM_CAP_CONNECT, 0, 0) = 0 Or m_CapHwnd = 0 Then Throw New Exception("Can't start the camera")

            Application.DoEvents()
            Time = New Threading.Thread(AddressOf Timers)
            Interval = 1
            Time.Start()
            RaiseEvent Started(True, String.Empty)
        Catch ex As Exception
            If Time.ThreadState = Threading.ThreadState.Running Then Time.Abort()
            SendMessageA(m_CapHwnd, WM_CAP_DISCONNECT, 0, 0)
            m_CapHwnd = 0
            RaiseEvent Started(False, ex.Message)
        End Try
    End Sub

    Public Sub [Stop]()
        Try
            If Time.ThreadState = Threading.ThreadState.Running Then Time.Abort()
        Catch ex As Exception
            SendMessageA(m_CapHwnd, WM_CAP_DISCONNECT, 0, 0)
            m_CapHwnd = 0
            RaiseEvent Stoped(False, ex.Message)
            Exit Sub
        End Try
        RaiseEvent Stoped(True, String.Empty)
    End Sub

End Class






'// Usage Of Class ...
    Dim WithEvents x As New Cam

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        x.Start(Me, PictureBox1)
    End Sub

    Private Sub x_Started(YesOrNo As Boolean, ErrText As String) Handles x.Started
        If YesOrNo = False Then
            MsgBox(ErrText)
        End If
    End Sub

