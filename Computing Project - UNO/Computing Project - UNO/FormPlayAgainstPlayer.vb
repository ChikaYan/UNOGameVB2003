Public Class FormPlayAgainstPlayer
    Public Shared P1WinCounter, P2WinCounter As Integer
    Public Shared GameStart As Boolean
    Public Shared CurrentGame As PlayerGame
    Public Shared PB As List(Of PictureBox)
    Public Shared Lab2 As Label
    Public Shared Lab3 As Label
    Public Shared Debug As Boolean
    Public Shared Msg As Label

    Private Sub FormPlayAgainstPlayer_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GameStart = False
        PB = New List(Of PictureBox)

        Button1.Hide()

        P1WinCounter = 0 'reset win counter
        P2WinCounter = 0

        Dim Ctr As Control 'put all 43 pictureboxes into the list
        Dim i As Integer
        For i = 1 To 43
            Ctr = Controls("PictureBox" & i)
            PB.Add(DirectCast(Ctr, PictureBox))
        Next

        Ctr = Controls("Label2") 'set control of Label2
        Lab2 = DirectCast(Ctr, Label)
        Ctr = Controls("Label3") 'set control of Label3
        Lab3 = DirectCast(Ctr, Label)
        Ctr = Controls("Label4") 'set control of ListBox1
        Msg = DirectCast(Ctr, Label)
        Msg.Text = "Click deck to start game"

    End Sub

    Private Sub PictureBox42_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureBox42.Click
        If Not FormGameMenu.Sleeping Then 'prevent extra click during sleeping
            If Debug Then
                Button1.Show()
            End If
            If Not GameStart Then
                CurrentGame = New PlayerGame()
                GameStart = True
            Else
                CurrentGame.GiveUpTurn()
            End If
        End If
    End Sub

    Private Sub PictureBox1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureBox1.Click, PictureBox2.Click, PictureBox3.Click, PictureBox4.Click, PictureBox5.Click, PictureBox6.Click, PictureBox7.Click, PictureBox8.Click, PictureBox9.Click, PictureBox10.Click, PictureBox11.Click, PictureBox12.Click, PictureBox13.Click, PictureBox14.Click, PictureBox15.Click, PictureBox16.Click, PictureBox17.Click, PictureBox18.Click, PictureBox19.Click, PictureBox20.Click
        Dim PBClicked As PictureBox = DirectCast(sender, PictureBox)
        If GameStart = True Then
            If CurrentGame.WhoseTurn = "P1" Then
                CurrentGame.PlayCard(CurrentGame.P1Hand, (Mid$(PBClicked.Name, 11, Len(PBClicked.Name) - 10)) - 1, CurrentGame.DealingPlaceP1)
            End If
        End If
    End Sub

    Private Sub PictureBox21_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureBox21.Click, PictureBox22.Click, PictureBox23.Click, PictureBox24.Click, PictureBox25.Click, PictureBox26.Click, PictureBox27.Click, PictureBox28.Click, PictureBox29.Click, PictureBox30.Click, PictureBox31.Click, PictureBox32.Click, PictureBox33.Click, PictureBox34.Click, PictureBox35.Click, PictureBox36.Click, PictureBox37.Click, PictureBox38.Click, PictureBox39.Click, PictureBox40.Click
        Dim PBClicked As PictureBox = DirectCast(sender, PictureBox)
        If GameStart = True Then
            If CurrentGame.WhoseTurn = "P2" Then
                CurrentGame.PlayCard(CurrentGame.P2Hand, CInt(Mid$(PBClicked.Name, 11, Len(PBClicked.Name) - 10)) - 21, CurrentGame.DealingPlaceP2)
            End If
        End If
    End Sub

    Private Sub PictureBox1_MouseEnter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureBox1.MouseEnter, PictureBox2.MouseEnter, PictureBox3.MouseEnter, PictureBox4.MouseEnter, PictureBox5.MouseEnter, PictureBox6.MouseEnter, PictureBox7.MouseEnter, PictureBox8.MouseEnter, PictureBox9.MouseEnter, PictureBox10.MouseEnter, PictureBox11.MouseEnter, PictureBox12.MouseEnter, PictureBox13.MouseEnter, PictureBox14.MouseEnter, PictureBox15.MouseEnter, PictureBox16.MouseEnter, PictureBox17.MouseEnter, PictureBox18.MouseEnter, PictureBox19.MouseEnter, PictureBox20.MouseEnter
        Dim PBClicked As PictureBox = DirectCast(sender, PictureBox)
        Dim HandNumber As Integer

        HandNumber = CInt(Mid$(PBClicked.Name, 11, Len(PBClicked.Name) - 10)) - 1

        If GameStart = True Then
            If CurrentGame.WhoseTurn = "P1" Then
                CurrentGame.DrawPictureBox((HandNumber + 1), CurrentGame.P1Hand(HandNumber).Colour, CurrentGame.P1Hand(HandNumber).Type)
            End If
        End If
    End Sub

    Private Sub PictureBox21_MouseEnter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureBox21.MouseEnter, PictureBox22.MouseEnter, PictureBox23.MouseEnter, PictureBox24.MouseEnter, PictureBox25.MouseEnter, PictureBox26.MouseEnter, PictureBox27.MouseEnter, PictureBox28.MouseEnter, PictureBox29.MouseEnter, PictureBox30.MouseEnter, PictureBox31.MouseEnter, PictureBox32.MouseEnter, PictureBox33.MouseEnter, PictureBox34.MouseEnter, PictureBox35.MouseEnter, PictureBox36.MouseEnter, PictureBox37.MouseEnter, PictureBox38.MouseEnter, PictureBox39.MouseEnter, PictureBox40.MouseEnter
        Dim PBClicked As PictureBox = DirectCast(sender, PictureBox)
        Dim HandNumber As Integer

        HandNumber = CInt(Mid$(PBClicked.Name, 11, Len(PBClicked.Name) - 10)) - 21

        If GameStart = True Then
            If CurrentGame.WhoseTurn = "P2" Then
                CurrentGame.DrawPictureBox((HandNumber + 21), CurrentGame.P2Hand(HandNumber).Colour, CurrentGame.P2Hand(HandNumber).Type)
            End If
        End If

    End Sub

    Private Sub PictureBox1_MouseLeave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureBox1.MouseLeave, PictureBox2.MouseLeave, PictureBox3.MouseLeave, PictureBox4.MouseLeave, PictureBox5.MouseLeave, PictureBox6.MouseLeave, PictureBox7.MouseLeave, PictureBox8.MouseLeave, PictureBox9.MouseLeave, PictureBox10.MouseLeave, PictureBox11.MouseLeave, PictureBox12.MouseLeave, PictureBox13.MouseLeave, PictureBox14.MouseLeave, PictureBox15.MouseLeave, PictureBox16.MouseLeave, PictureBox17.MouseLeave, PictureBox18.MouseLeave, PictureBox19.MouseLeave, PictureBox20.MouseLeave
        Dim PBClicked As PictureBox = DirectCast(sender, PictureBox)

        If GameStart = True Then
            If CurrentGame.WhoseTurn = "P1" Then
                CurrentGame.DrawPictureBox((Mid$(PBClicked.Name, 11, Len(PBClicked.Name) - 10)), "", "Back")
            End If
        End If
    End Sub

    Private Sub PictureBox21_MouseLeave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureBox21.MouseLeave, PictureBox22.MouseLeave, PictureBox23.MouseLeave, PictureBox24.MouseLeave, PictureBox25.MouseLeave, PictureBox26.MouseLeave, PictureBox27.MouseLeave, PictureBox28.MouseLeave, PictureBox29.MouseLeave, PictureBox30.MouseLeave, PictureBox31.MouseLeave, PictureBox32.MouseLeave, PictureBox33.MouseLeave, PictureBox34.MouseLeave, PictureBox35.MouseLeave, PictureBox36.MouseLeave, PictureBox37.MouseLeave, PictureBox38.MouseLeave, PictureBox39.MouseLeave, PictureBox40.MouseLeave
        Dim PBClicked As PictureBox = DirectCast(sender, PictureBox)

        If GameStart = True Then
            If CurrentGame.WhoseTurn = "P2" Then
                CurrentGame.DrawPictureBox((Mid$(PBClicked.Name, 11, Len(PBClicked.Name) - 10)), "", "Back")
            End If
        End If
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        CurrentGame.RevealALLHand()
    End Sub

End Class
