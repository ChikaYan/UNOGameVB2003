Public Class FormGameMenu
    Public Shared WildColour As String
    Public Shared Sleeping As Boolean = False

    Private Sub ButtonPlayAgainstPlayer_Click(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles ButtonPlayAgainstPlayer.MouseDown
        Dim PVPWindow As New FormPlayAgainstPlayer
        PVPWindow.Show()
        If e.Button = Windows.Forms.MouseButtons.Right Then
            FormPlayAgainstPlayer.Debug = True
        Else
            FormPlayAgainstPlayer.Debug = False
        End If
    End Sub

    Private Sub ButtonPlayAgainstAI_Click(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles ButtonPlayAgainstAI.MouseDown
        Dim AIWindow As New FormPlayAgainstAI
        AIWindow.Show()
        If e.Button = Windows.Forms.MouseButtons.Right Then
            FormPlayAgainstAI.Debug = True
        Else
            FormPlayAgainstAI.Debug = False
        End If
    End Sub

    Private Sub ButtonExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonExit.Click
        Me.Close()
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim HelpWindow As New FormRules
        HelpWindow.Show()
    End Sub

End Class
