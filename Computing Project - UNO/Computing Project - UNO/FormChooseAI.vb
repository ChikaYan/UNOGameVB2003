Public Class FormChooseAI

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        FormPlayAgainstAI.Difficulty = "Easy"
        Me.Close()
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        FormPlayAgainstAI.Difficulty = "Cheater"
        Me.Close()
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        FormPlayAgainstAI.Difficulty = "Hard"
        Me.Close()
    End Sub

    Private Sub FormChooseAI_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        FormPlayAgainstAI.Difficulty = "Unselected"
    End Sub
End Class