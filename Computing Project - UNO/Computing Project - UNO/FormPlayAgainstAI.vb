Public Class FormPlayAgainstAI
    Public Shared P1WinCounter, P2WinCounter As Integer
    Public Shared GameStart, Debug As Boolean
    Public Shared CurrentGameEasy As EasyAIGame
    Public Shared CurrentGameCheater As CheaterAIGame
    Public Shared CurrentGameHard As HardAIGame
    Public Shared PB As List(Of PictureBox)
    Public Shared Lab2, Lab3, Msg As Label
    Public Shared Difficulty As String

    Private Sub FormPlayAgainstAI_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GameStart = False
        PB = New List(Of PictureBox)

        Button1.Hide() 'Hide Debug button

        P1WinCounter = 0 'reset win counter
        P2WinCounter = 0

        Dim Ctr As Control 'put all 43 pictureboxes into the list
        Dim i As Integer
        For i = 1 To 63
            Ctr = Controls("PictureBox" & i)
            PB.Add(DirectCast(Ctr, PictureBox))
        Next

        For i = 43 To 62 'hide hint arrows
            PB(i).Hide()
        Next

            Ctr = Controls("Label2") 'set control of Label2
            Lab2 = DirectCast(Ctr, Label)
            Ctr = Controls("Label3") 'set control of Label3
            Lab3 = DirectCast(Ctr, Label)
            Ctr = Controls("Label4") 'set control of ListBox1
            Msg = DirectCast(Ctr, Label)
        Msg.Text = "Click deck to start game"

        Do
            Dim ChooseDifficulty As New FormChooseAI 'ask for difficulty
            ChooseDifficulty.ShowDialog()
        Loop Until Difficulty <> "Unselected"
    End Sub

    Public Sub PictureBox42_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureBox42.Click
        If Not FormGameMenu.Sleeping Then 'prevent extra click during sleeping
            If Debug Then
                Button1.Show()
            End If

            If Difficulty = "Easy" Then
                If Not GameStart Then
                    CurrentGameEasy = New EasyAIGame()
                    GameStart = True
                Else
                    If CurrentGameEasy.WhoseTurn = "P1" Then
                        CurrentGameEasy.GiveUpTurn()
                    End If
                End If

            ElseIf Difficulty = "Hard" Then
                If Not GameStart Then
                    CurrentGameHard = New HardAIGame()
                    GameStart = True
                Else
                    If CurrentGameHard.WhoseTurn = "P1" Then
                        CurrentGameHard.GiveUpTurn()
                    End If
                End If
            ElseIf Difficulty = "Cheater" Then
                If Not GameStart Then
                    CurrentGameCheater = New CheaterAIGame()
                    GameStart = True
                Else
                    If CurrentGameCheater.WhoseTurn = "P1" Then
                        CurrentGameCheater.GiveUpTurn()
                    End If
                End If
            End If
        End If
    End Sub

    Private Sub PictureBox1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureBox1.Click, PictureBox2.Click, PictureBox3.Click, PictureBox4.Click, PictureBox5.Click, PictureBox6.Click, PictureBox7.Click, PictureBox8.Click, PictureBox9.Click, PictureBox10.Click, PictureBox11.Click, PictureBox12.Click, PictureBox13.Click, PictureBox14.Click, PictureBox15.Click, PictureBox16.Click, PictureBox17.Click, PictureBox18.Click, PictureBox19.Click, PictureBox20.Click
        Dim PBClicked As PictureBox = DirectCast(sender, PictureBox)
        If Not FormGameMenu.Sleeping Then 'prevent extra click while AI's thinking

            If Difficulty = "Easy" Then
                If GameStart = True Then
                    If CurrentGameEasy.WhoseTurn = "P1" Then
                        CurrentGameEasy.PlayCard(CurrentGameEasy.P1Hand, (Mid$(PBClicked.Name, 11, Len(PBClicked.Name) - 10)) - 1, CurrentGameEasy.DealingPlaceP1)
                    End If
                End If

            ElseIf Difficulty = "Hard" Then
                If GameStart = True Then
                    If CurrentGameHard.WhoseTurn = "P1" Then
                        CurrentGameHard.PlayCard(CurrentGameHard.P1Hand, (Mid$(PBClicked.Name, 11, Len(PBClicked.Name) - 10)) - 1, CurrentGameHard.DealingPlaceP1)
                    End If
                End If

            ElseIf Difficulty = "Cheater" Then
                If GameStart = True Then
                    If CurrentGameCheater.WhoseTurn = "P1" Then
                        CurrentGameCheater.PlayCard(CurrentGameCheater.P1Hand, (Mid$(PBClicked.Name, 11, Len(PBClicked.Name) - 10)) - 1, CurrentGameCheater.DealingPlaceP1)
                    End If
                End If
            End If

        End If
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        If GameStart Then
            If Difficulty = "Easy" Then
                CurrentGameEasy.RevealALLHand()
            ElseIf Difficulty = "Hard" Then
                CurrentGameHard.RevealALLHand()
            ElseIf Difficulty = "Cheater" Then
                CurrentGameCheater.RevealALLHand()
            End If
        End If
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        If GameStart Then
            If Difficulty = "Easy" Then
                CurrentGameEasy.Hint()
            ElseIf Difficulty = "Hard" Then
                CurrentGameHard.Hint()
            ElseIf Difficulty = "Cheater" Then
                CurrentGameCheater.Hint()
            End If
        End If
    End Sub
End Class