Public Class FormChooseWildColour

    Private Sub PictureBox1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureBox1.Click, PictureBox2.Click, PictureBox3.Click, PictureBox4.Click
        Dim PBClicked As PictureBox = DirectCast(sender, PictureBox)

        Select Case PBClicked.Name
            Case "PictureBox1"
                FormGameMenu.WildColour = "Blue"
            Case "PictureBox2"
                FormGameMenu.WildColour = "Golden"
            Case "PictureBox3"
                FormGameMenu.WildColour = "Green"
            Case "PictureBox4"
                FormGameMenu.WildColour = "Red"
        End Select
        Me.Close()
    End Sub

    Private Sub FormChooseWildColour_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        FormGameMenu.WildColour = "Cancel"
    End Sub
End Class