
Public Class Card
    Public Type As String = "None"
    Public Colour As String = ""
End Class

Public Class LastPlayedCard
    Inherits Card
    Public WildColour As String = "" 'if LastPlayed card is a wild or wild +4 card, this variable is used to store the colour named by player
    Public IfPunished As Boolean = False 'if LastPlayed is word card, this value indicates whether the punishment has been applied. If ture, next required card would only be card with same colour
End Class

Public Class Game
    Public P1Hand(19), P2Hand(19), Deck(107) As Card
    Public DeckRemained As Integer
    Public LastPlayed As LastPlayedCard
    Public DealingPlaceP1, DealingPlaceP2 As Integer
    Public WhoseTurn As String
    Public AlreadyDraw As Boolean 'determine whether player has drawn a card from deck in this turn

    Public FixedDeck(107) As Card 'the variables needed by CheaterAI
    Public HandData As Data

    Structure Data
        Dim P1Blue As Integer 'number of cards of different colours in P1's hand
        Dim P1Golden As Integer
        Dim P1Green As Integer
        Dim P1Red As Integer
        Dim P1NonWild As Integer
        Dim P1MinColour As String

        Dim P2Blue As Integer
        Dim P2Golden As Integer
        Dim P2Green As Integer
        Dim P2Red As Integer
        Dim P2NonWild As Integer
        Dim P2MaxColour As String
        Dim P2NonWord As Integer
    End Structure

    Sub New()
        LastPlayed = New LastPlayedCard
        SetUpDeck()
        DecideFirstTurn()
        SetUpHand()

        If WhoseTurn = "P1" Then
            WhoseTurn = "P2"
        ElseIf WhoseTurn = "P2" Then
            WhoseTurn = "P1"
        End If
        'a reverse is needed before starting the first turn, as WhoseTurn will be reversed again in the beginning of TurnStart sub

        TurnStart()
    End Sub

    Private Sub SetUpHand()
        Dim i As Integer
        For i = 0 To 19  'clean all the hand
            P1Hand(i) = New Card
            P2Hand(i) = New Card
        Next
        DealingPlaceP1 = 0   'start dealing at 0
        DealingPlaceP2 = 0
        For i = 1 To 7 'deal 7 cards to p1 and p2 
            Deal(P1Hand, DealingPlaceP1)
            RefreshCards()

            FormGameMenu.Sleeping = True
            My.Application.DoEvents()
            System.Threading.Thread.Sleep(60)

            Deal(P2Hand, DealingPlaceP2)
            RefreshCards()
            My.Application.DoEvents()
            System.Threading.Thread.Sleep(60)
            FormGameMenu.Sleeping = False
        Next

    End Sub

    Private Sub DecideFirstTurn() 'decide which player will play at the frist turn
        Dim p1, p2 As Integer
        Dim i As Integer

        Randomize()

        Do
            p1 = Int(Rnd() * 108)
            p2 = Int(Rnd() * 108)
        Loop Until Len(Deck(p1).Type) = 1 And Len(Deck(p2).Type) = 1 And Deck(p1).Type <> Deck(p2).Type
        'Deal each player a card. If the cards have same number or any of them is not a number card, deal another two

        DrawPictureBox(1, Deck(p1).Colour, Deck(p1).Type)
        DrawPictureBox(21, Deck(p2).Colour, Deck(p2).Type)

        For i = 1 To 43
            If i <> 1 And i <> 21 And i <> 42 Then
                DrawPictureBox(i, "", "None")
            End If
        Next
        'show two cards

        FormGameMenu.Sleeping = True
        My.Application.DoEvents()
        System.Threading.Thread.Sleep(500)
        FormGameMenu.Sleeping = False

        If CInt(Deck(p1).Type) > CInt(Deck(p2).Type) Then 'p1's number is larger
            WhoseTurn = "P1"

            If Not FormPlayAgainstAI.Msg Is Nothing Then 'the game is AI game
                FormPlayAgainstAI.Msg.Text = "Player plays first!"
            Else 'the game is pvp game
                FormPlayAgainstPlayer.Msg.Text = "P1 plays first!"
            End If

            FormGameMenu.Sleeping = True
            My.Application.DoEvents()
            System.Threading.Thread.Sleep(1500)
            FormGameMenu.Sleeping = False

        Else 'p2's number is larger
            WhoseTurn = "P2"

            If Not FormPlayAgainstAI.Msg Is Nothing Then 'the game is AI game
                FormPlayAgainstAI.Msg.Text = "AI plays first!"
            Else 'the game is pvp game
                FormPlayAgainstPlayer.Msg.Text = "P2 plays first!"
            End If

            FormGameMenu.Sleeping = True
            My.Application.DoEvents()
            System.Threading.Thread.Sleep(1500)
            FormGameMenu.Sleeping = False

        End If

    End Sub

    Public Sub RemoveCard(ByRef PlayerHand() As Card, ByRef CardNumber As Integer, ByRef DealingPlace As Integer)
        Dim counter As Integer

        For counter = CardNumber To DealingPlace - 2
            PlayerHand(counter).Type = PlayerHand(counter + 1).Type
            PlayerHand(counter).Colour = PlayerHand(counter + 1).Colour
        Next

        PlayerHand(DealingPlace - 1).Type = "None"
        PlayerHand(DealingPlace - 1).Colour = ""

        DealingPlace = DealingPlace - 1

    End Sub

    Protected Function CheckValidity(ByVal Colour As String, ByVal Type As String, ByVal LP As LastPlayedCard) As Boolean
        If ((Type = LP.Type Or Colour = LP.Colour Or Colour = "Wild") And (Len(LP.Type) = 1)) Then
            'a non-word card followed by a card with same number or colour or wild card
            Return True
        ElseIf (LP.Type = "+2" And Type = "+4") Then
            'a +2 card overided by a wild +4 card
            Return True
        ElseIf (LP.Colour = "Wild" And (Colour = LP.WildColour Or Colour = "Wild")) Then
            'a wild card with colour only
            Return True
        ElseIf (LP.IfPunished = True And (Colour = LP.Colour Or Colour = "Wild")) Then
            'a punished word card
            Return True
        ElseIf LP.Type = "None" Then
            'no previous played card
            Return True
        Else
            Return False
        End If
    End Function

    Public Sub RevealALLHand()
        For i = 0 To DealingPlaceP1 - 1
            DrawPictureBox(i + 1, P1Hand(i).Colour, P1Hand(i).Type)
        Next
        For i = DealingPlaceP1 To 19
            DrawPictureBox(i + 1, "", "None")
        Next
        For i = 0 To DealingPlaceP2 - 1
            DrawPictureBox(i + 21, P2Hand(i).Colour, P2Hand(i).Type)
        Next
        For i = DealingPlaceP2 To 19
            DrawPictureBox(i + 21, "", "None")
        Next
    End Sub

    Public Sub GiveUpTurn() 'this procdure would only be used on player

        If WhoseTurn = "P1" Then
            If AlreadyDraw Then 'Pass turn
                If Not FormPlayAgainstAI.Msg Is Nothing Then 'the game is AI game
                    FormPlayAgainstAI.Msg.Text = FormPlayAgainstAI.Msg.Text + vbCrLf + "Player has given up the turn"
                Else
                    FormPlayAgainstPlayer.Msg.Text = FormPlayAgainstPlayer.Msg.Text + vbCrLf + "P1 has given up the turn"
                End If
                TurnStart()
            ElseIf AlreadyDraw = False And LastPlayed.Type = "+2" And LastPlayed.IfPunished = False Then 'punish +2
                If Not FormPlayAgainstAI.Msg Is Nothing Then 'the game is AI game
                    FormPlayAgainstAI.Msg.Text = "Player draw two cards!"
                Else
                    FormPlayAgainstPlayer.Msg.Text = "P1 has drawn two cards!"
                End If
                LastPlayed.IfPunished = True
                Deal(P1Hand, DealingPlaceP1)
                Deal(P1Hand, DealingPlaceP1)
                RefreshCards()
                TurnStart()
            Else
                If Not FormPlayAgainstAI.Msg Is Nothing Then 'the game is AI game
                    FormPlayAgainstAI.Msg.Text = "Player has drawn a card"
                Else
                    FormPlayAgainstPlayer.Msg.Text = "P1 has drawn a card"
                End If
                Deal(P1Hand, DealingPlaceP1)
                AlreadyDraw = True
                RefreshCards()
            End If

        ElseIf WhoseTurn = "P2" Then
            If AlreadyDraw Then
                If Not FormPlayAgainstAI.Msg Is Nothing Then 'the game is AI game
                    FormPlayAgainstAI.Msg.Text = FormPlayAgainstAI.Msg.Text + vbCrLf + "AI has given up the turn"
                Else
                    FormPlayAgainstPlayer.Msg.Text = FormPlayAgainstPlayer.Msg.Text + vbCrLf + "P2 has given up the turn"
                End If
                TurnStart()
            ElseIf AlreadyDraw = False And LastPlayed.Type = "+2" And LastPlayed.IfPunished = False Then 'punish +2
                If Not FormPlayAgainstAI.Msg Is Nothing Then 'the game is AI game
                    FormPlayAgainstAI.Msg.Text = "AI draw two cards!"
                Else
                    FormPlayAgainstPlayer.Msg.Text = "P2 has drawn two cards!"
                End If
                LastPlayed.IfPunished = True
                Deal(P2Hand, DealingPlaceP2)
                Deal(P2Hand, DealingPlaceP2)
                RefreshCards()
                TurnStart()
            Else
                If Not FormPlayAgainstAI.Msg Is Nothing Then 'the game is AI game
                    FormPlayAgainstAI.Msg.Text = "AI has drawn a card"
                Else
                    FormPlayAgainstPlayer.Msg.Text = "P2 has drawn a card"
                End If
                Deal(P2Hand, DealingPlaceP2)
                AlreadyDraw = True
                RefreshCards()
            End If
        End If
    End Sub

    Protected Overridable Sub SetUpDeck()
        Dim i, j As Integer

        DeckRemained = 108
        For i = 0 To 7
            For j = 0 To 11
                If j + 1 <= 9 Then '1 to 9
                    Deck(12 * i + j) = New Card
                    Deck(12 * i + j).Type = (j + 1)
                ElseIf j + 1 = 10 Then 'Skip
                    Deck(12 * i + j) = New Card
                    Deck(12 * i + j).Type = "Skip"
                ElseIf j + 1 = 11 Then 'Reverse
                    Deck(12 * i + j) = New Card
                    Deck(12 * i + j).Type = "Reverse"
                ElseIf j + 1 = 12 Then '+2
                    Deck(12 * i + j) = New Card
                    Deck(12 * i + j).Type = "+2"
                End If

                Select Case i
                    Case 0 To 1
                        Deck(12 * i + j).Colour = "Blue"
                    Case 2 To 3
                        Deck(12 * i + j).Colour = "Golden"
                    Case 4 To 5
                        Deck(12 * i + j).Colour = "Green"
                    Case 6 To 7
                        Deck(12 * i + j).Colour = "Red"
                End Select
            Next
        Next

        For i = 96 To 99 '0 in four colours 
            Deck(i) = New Card
            Deck(i).Type = "0"
        Next
        Deck(96).Colour = "Blue"
        Deck(97).Colour = "Golden"
        Deck(98).Colour = "Green"
        Deck(99).Colour = "Red"

        For i = 100 To 107
            Deck(i) = New Card
            Deck(i).Colour = "Wild"
            If i <= 103 Then
                Deck(i).Type = "" 'word card Wild
            Else
                Deck(i).Type = "+4" 'word card Wild +4
            End If
        Next


    End Sub

    Public Overridable Sub Deal(ByRef PlayerHand() As Card, ByRef DealingPlace As Integer)
        Dim indicator As Integer
        If DeckRemained <> 0 Then
            If DealingPlace <= 19 Then
                Randomize()
                Do
                    indicator = Int(Rnd() * 108)
                Loop Until Deck(indicator).Type <> "None"
                PlayerHand(DealingPlace).Type = Deck(indicator).Type
                PlayerHand(DealingPlace).Colour = Deck(indicator).Colour
                Deck(indicator).Type = "None"
                Deck(indicator).Colour = ""
                DealingPlace += 1
                DeckRemained -= 1
            Else
                MsgBox("Can't hold more than 20 cards in hand!")
            End If
        Else
            MsgBox("No more card in the deck!")
            WhoseTurn = ""
            RefreshCards()
        End If
    End Sub

    Public Overridable Sub DrawPictureBox(ByVal PictureBoxNumber As Integer, ByVal CardColour As String, ByVal CardType As String)

        If CardType <> "None" Then
            FormPlayAgainstPlayer.PB(PictureBoxNumber - 1).Image = My.Resources.ResourceManager.GetObject(CardColour & CardType)
            FormPlayAgainstPlayer.PB(PictureBoxNumber - 1).Show()
            FormPlayAgainstPlayer.PB(PictureBoxNumber - 1).Enabled = True
        Else
            FormPlayAgainstPlayer.PB(PictureBoxNumber - 1).Image = Nothing
            FormPlayAgainstPlayer.PB(PictureBoxNumber - 1).Hide()
            FormPlayAgainstPlayer.PB(PictureBoxNumber - 1).Enabled = False
        End If

    End Sub

    Public Overridable Sub TurnStart()

        If WhoseTurn = "P1" Then
            WhoseTurn = "P2"
        ElseIf WhoseTurn = "P2" Then
            WhoseTurn = "P1"
        End If
        AlreadyDraw = False
        RefreshCards()

        'punishment

    End Sub

    Public Overridable Sub PlayCard(ByRef PlayerHand() As Card, ByVal HandCardNumber As Integer, ByRef DealingPlace As Integer)
        LastPlayed = New LastPlayedCard
        LastPlayed.Type = PlayerHand(HandCardNumber).Type
        LastPlayed.Colour = PlayerHand(HandCardNumber).Colour
        RemoveCard(PlayerHand, HandCardNumber, DealingPlace)
    End Sub

    Public Overridable Sub RefreshCards()
        DrawPictureBox(41, LastPlayed.Colour, LastPlayed.Type) 'Draw Lastplayed Card
        If LastPlayed.Colour = "Wild" Then
            DrawPictureBox(43, LastPlayed.WildColour, "PureColour")
        Else
            DrawPictureBox(43, "", "None")
        End If
    End Sub

End Class

Public Class PlayerGame
    Inherits Game

    Public Overrides Sub TurnStart()
        MyBase.TurnStart()

        If LastPlayed.IfPunished = False Then 'Give punishment
            If LastPlayed.Type = "Skip" Then 'Punish "Skip"
                FormPlayAgainstPlayer.Msg.Text = WhoseTurn + "'s turn has been skipped!"
                LastPlayed.IfPunished = True
                TurnStart()
                Exit Sub
            ElseIf LastPlayed.Type = "Reverse" Then 'Punish "Reverse"
                FormPlayAgainstPlayer.Msg.Text = "Play order has been reversed!"
                LastPlayed.IfPunished = True
                TurnStart()
                Exit Sub
            ElseIf LastPlayed.Type = "+4" Then 'punish wild +4
                LastPlayed.IfPunished = True
                If WhoseTurn = "P1" Then
                    FormPlayAgainstPlayer.Msg.Text = "P1's turn has been skipped!" + vbCrLf + "P1 has drawn four extra card!"
                    Deal(P1Hand, DealingPlaceP1)
                    Deal(P1Hand, DealingPlaceP1)
                    Deal(P1Hand, DealingPlaceP1)
                    Deal(P1Hand, DealingPlaceP1)
                Else
                    FormPlayAgainstPlayer.Msg.Text = "P2's turn has been skipped!" + vbCrLf + "P2 has drawn four extra card!"
                    Deal(P2Hand, DealingPlaceP2)
                    Deal(P2Hand, DealingPlaceP2)
                    Deal(P2Hand, DealingPlaceP2)
                    Deal(P2Hand, DealingPlaceP2)
                End If
                TurnStart()
                Exit Sub
            End If
        End If

        'wait for player move
        FormPlayAgainstPlayer.Msg.Text = FormPlayAgainstPlayer.Msg.Text + vbCrLf + "Waiting for " + WhoseTurn + "'s move"

    End Sub

    Public Overrides Sub PlayCard(ByRef PlayerHand() As Card, ByVal HandCardNumber As Integer, ByRef DealingPlace As Integer)
        If CheckValidity(PlayerHand(HandCardNumber).Colour, PlayerHand(HandCardNumber).Type, LastPlayed) Then
            FormPlayAgainstPlayer.Msg.Text = WhoseTurn + " has played a " + PlayerHand(HandCardNumber).Colour + " " + PlayerHand(HandCardNumber).Type
            MyBase.PlayCard(PlayerHand, HandCardNumber, DealingPlace)

            If LastPlayed.Colour = "Wild" Then 'Request wild colour
                Do
                    Dim ChooseColourWindow As New FormChooseWildColour
                    ChooseColourWindow.ShowDialog()
                    If FormGameMenu.WildColour <> "Cancel" Then
                        LastPlayed.WildColour = FormGameMenu.WildColour
                        FormGameMenu.WildColour = ""
                    End If
                Loop Until FormGameMenu.WildColour <> "Cancel"
            End If

            If DealingPlace <> 0 Then
                TurnStart()
            ElseIf WhoseTurn = "P1" Then
                FormPlayAgainstPlayer.Msg.Text = "        P1 WON" + vbCrLf + "Click deck to start another game"
                FormPlayAgainstPlayer.P1WinCounter += 1
                FormPlayAgainstPlayer.Lab2.Text = FormPlayAgainstPlayer.P1WinCounter
                WhoseTurn = ""
                RefreshCards()
                FormPlayAgainstPlayer.GameStart = False
            ElseIf WhoseTurn = "P2" Then
                FormPlayAgainstPlayer.Msg.Text = "        P2 WON" + vbCrLf + "Click deck to start another game"
                FormPlayAgainstPlayer.P2WinCounter += 1
                FormPlayAgainstPlayer.Lab3.Text = FormPlayAgainstPlayer.P2WinCounter
                WhoseTurn = ""
                RefreshCards()
                FormPlayAgainstPlayer.GameStart = False
            End If
        Else
            FormPlayAgainstPlayer.Msg.Text = FormPlayAgainstPlayer.Msg.Text + vbCrLf + "You can't play this card!" + vbCrLf + "Choose another card to play"
        End If
    End Sub

    Public Overrides Sub RefreshCards()
        Dim i As Integer
        MyBase.RefreshCards()

        If WhoseTurn = "P1" Or WhoseTurn = "P2" Then
            For i = 0 To DealingPlaceP1 - 1
                DrawPictureBox(i + 1, "", "Back") 'display P1's hand (back)
            Next
            For i = DealingPlaceP1 To 19
                DrawPictureBox(i + 1, "", "None") 'hide the rest of hand
            Next

            For i = 0 To DealingPlaceP2 - 1
                DrawPictureBox(i + 21, "", "Back") 'display P2's hand (back)
            Next
            For i = DealingPlaceP2 To 19
                DrawPictureBox(i + 21, "", "None") 'hide the rest of hand
            Next
        ElseIf WhoseTurn = "" Then
            For i = 0 To DealingPlaceP1 - 1
                DrawPictureBox(i + 1, P1Hand(i).Colour, P1Hand(i).Type)
            Next
            For i = DealingPlaceP1 To 19
                DrawPictureBox(i + 1, "", "None")
            Next
            For i = 0 To DealingPlaceP2 - 1
                DrawPictureBox(i + 21, P2Hand(i).Colour, P2Hand(i).Type)
            Next
            For i = DealingPlaceP2 To 19 'end of turn
                DrawPictureBox(i + 21, "", "None")
            Next
        End If

    End Sub

End Class

Public Class AIGame
    Inherits Game

    Protected Sub AINoSolutionFound()

        If AlreadyDraw Then
            FormPlayAgainstAI.Msg.Text = FormPlayAgainstAI.Msg.Text + vbCrLf + "AI has given up the turn"
            TurnStart()
        ElseIf AlreadyDraw = False And LastPlayed.Type = "+2" And LastPlayed.IfPunished = False Then 'punish +2
            FormPlayAgainstAI.Msg.Text = "AI draw two extra cards!"
            LastPlayed.IfPunished = True
            Deal(P2Hand, DealingPlaceP2)
            Deal(P2Hand, DealingPlaceP2)
            RefreshCards()
            TurnStart()
        Else
            FormPlayAgainstAI.Msg.Text = "AI has drawn one card"
            Deal(P2Hand, DealingPlaceP2)
            AlreadyDraw = True
            RefreshCards()
            AIFindSolution()
        End If

    End Sub

    Public Sub Hint()
        Dim i As Integer
        If WhoseTurn = "P1" Then
            For i = 0 To DealingPlaceP1 - 1
                If CheckValidity(P1Hand(i).Colour, P1Hand(i).Type, LastPlayed) Then
                    FormPlayAgainstAI.PB(i + 43).Show()
                End If
            Next
            If AlreadyDraw Then
                FormPlayAgainstAI.Msg.Text = "Click deck to pass the turn"
            Else
                If LastPlayed.Type <> "+2" Then
                    FormPlayAgainstAI.Msg.Text = "Click deck to draw a card"
                Else
                    FormPlayAgainstAI.Msg.Text = "Click deck to draw two cards"
                End If
            End If
        End If
    End Sub

    Public Overrides Sub TurnStart()
        MyBase.TurnStart()
        If LastPlayed.IfPunished = False Then 'Give punishment

            If LastPlayed.Type = "Skip" Then 'Punish "Skip"
                If WhoseTurn = "P1" Then
                    FormPlayAgainstAI.Msg.Text = "Your turn has been skipped!"
                ElseIf WhoseTurn = "P2" Then
                    FormPlayAgainstAI.Msg.Text = "AI's Turn has been skipped!"
                End If
                LastPlayed.IfPunished = True
                TurnStart()
                Exit Sub

            ElseIf LastPlayed.Type = "Reverse" Then 'Punish "Reverse"
                FormPlayAgainstAI.Msg.Text = "Play order has been reversed!"
                LastPlayed.IfPunished = True
                TurnStart()
                Exit Sub

            ElseIf LastPlayed.Type = "+4" Then
                If WhoseTurn = "P1" Then 'punish "Wild +4"
                    FormPlayAgainstAI.Msg.Text = "Your Turn has been skipped!" + vbCrLf + "Draw four extra cards!"
                ElseIf WhoseTurn = "P2" Then
                    FormPlayAgainstAI.Msg.Text = "AI's Turn has been skipped!" + vbCrLf + "AI has drawn four extra cards!"
                End If
                LastPlayed.IfPunished = True
                If WhoseTurn = "P1" Then
                    Deal(P1Hand, DealingPlaceP1)
                    Deal(P1Hand, DealingPlaceP1)
                    Deal(P1Hand, DealingPlaceP1)
                    Deal(P1Hand, DealingPlaceP1)
                Else
                    Deal(P2Hand, DealingPlaceP2)
                    Deal(P2Hand, DealingPlaceP2)
                    Deal(P2Hand, DealingPlaceP2)
                    Deal(P2Hand, DealingPlaceP2)
                End If
                TurnStart()
                Exit Sub
            End If
        End If

        If WhoseTurn = "P1" Then 'wait for player's move / AI find solution
            FormPlayAgainstAI.Msg.Text = FormPlayAgainstAI.Msg.Text + vbCrLf + "Waiting for player's move"
        ElseIf WhoseTurn = "P2" Then
            AIFindSolution()
        End If

    End Sub

    Public Overrides Sub PlayCard(ByRef PlayerHand() As Card, ByVal HandCardNumber As Integer, ByRef DealingPlace As Integer)

        If WhoseTurn = "P1" Then 'Player's move
            If CheckValidity(PlayerHand(HandCardNumber).Colour, PlayerHand(HandCardNumber).Type, LastPlayed) Then
                MyBase.PlayCard(PlayerHand, HandCardNumber, DealingPlace)
                If LastPlayed.Colour = "Wild" Then 'Request wild colour
                    Do
                        Dim ChooseColourWindow As New FormChooseWildColour
                        ChooseColourWindow.ShowDialog()

                        If FormGameMenu.WildColour <> "Cancel" Then
                            LastPlayed.WildColour = FormGameMenu.WildColour
                            FormGameMenu.WildColour = ""
                        End If
                    Loop Until FormGameMenu.WildColour <> "Cancel"
                End If

                If DealingPlace <> 0 Then
                    TurnStart()
                Else 'P1 has no hand - Player wins
                    FormPlayAgainstAI.Msg.Text = "        YOU WON" + vbCrLf + "Click deck to start another game"
                    FormPlayAgainstAI.P1WinCounter += 1
                    FormPlayAgainstAI.Lab2.Text = FormPlayAgainstAI.P1WinCounter
                    WhoseTurn = ""
                    RefreshCards()
                    FormPlayAgainstAI.GameStart = False
                End If

            Else
                FormPlayAgainstAI.Msg.Text = "You can't play this card!" + vbCrLf + "Choose another card to play"
            End If

        ElseIf WhoseTurn = "P2" Then 'AI's turn
            FormPlayAgainstAI.Msg.Text = "AI has played a " + PlayerHand(HandCardNumber).Colour + " " + PlayerHand(HandCardNumber).Type
            MyBase.PlayCard(PlayerHand, HandCardNumber, DealingPlace)
            If LastPlayed.Colour = "Wild" Then
                LastPlayed.WildColour = AIDecideWildColour()
            End If

            RefreshCards()
            If LastPlayed.Type = "Skip" Or LastPlayed.Type = "Reverse" Or LastPlayed.Type = "+4" Then 'AI has played a combo - give player more time to react
                FormGameMenu.Sleeping = True
                My.Application.DoEvents()
                System.Threading.Thread.Sleep(1500)
                FormGameMenu.Sleeping = False
            End If

            If DealingPlace <> 0 Then '
                TurnStart()
            Else 'P2 has no hand - AI wins
                FormPlayAgainstAI.Msg.Text = "        YOU LOSE" + vbCrLf + "Click deck to start another game"
                FormPlayAgainstAI.P2WinCounter += 1
                FormPlayAgainstAI.Lab3.Text = FormPlayAgainstAI.P2WinCounter
                WhoseTurn = ""
                RefreshCards()
                FormPlayAgainstAI.GameStart = False
            End If
        End If
    End Sub

    Public Overrides Sub RefreshCards()
        Dim i As Integer
        MyBase.RefreshCards()

        If WhoseTurn = "" Then 'end of turn
            For i = 0 To DealingPlaceP1 - 1
                DrawPictureBox(i + 1, P1Hand(i).Colour, P1Hand(i).Type)
            Next
            For i = DealingPlaceP1 To 19
                DrawPictureBox(i + 1, "", "None")
            Next
            For i = 0 To DealingPlaceP2 - 1
                DrawPictureBox(i + 21, P2Hand(i).Colour, P2Hand(i).Type)
            Next
            For i = DealingPlaceP2 To 19
                DrawPictureBox(i + 21, "", "None")
            Next
        Else 'always display player's hand
            For i = 0 To DealingPlaceP1 - 1 'i = last hand number = picturebox number -1
                DrawPictureBox(i + 1, P1Hand(i).Colour, P1Hand(i).Type) 'display P1's hand
            Next
            For i = DealingPlaceP1 To 19
                DrawPictureBox(i + 1, "", "None") 'hide the rest of hand
            Next
            For i = 0 To DealingPlaceP2 - 1
                DrawPictureBox(i + 21, "", "Back") 'display P2's hand (back)
            Next
            For i = DealingPlaceP2 To 19
                DrawPictureBox(i + 21, "", "None") 'hide the rest of hand
            Next
        End If

        For i = 43 To 62 'reset hint arrows
            FormPlayAgainstAI.PB(i).Hide()
        Next
    End Sub

    Public Overrides Sub DrawPictureBox(ByVal PictureBoxNumber As Integer, ByVal CardColour As String, ByVal CardType As String)

        If CardType <> "None" Then
            FormPlayAgainstAI.PB(PictureBoxNumber - 1).Image = My.Resources.ResourceManager.GetObject(CardColour & CardType)
            FormPlayAgainstAI.PB(PictureBoxNumber - 1).Show()
            FormPlayAgainstAI.PB(PictureBoxNumber - 1).Enabled = True
        Else
            FormPlayAgainstAI.PB(PictureBoxNumber - 1).Image = Nothing
            FormPlayAgainstAI.PB(PictureBoxNumber - 1).Hide()
            FormPlayAgainstAI.PB(PictureBoxNumber - 1).Enabled = False
        End If


    End Sub

    Protected Overridable Sub AIFindSolution()
    End Sub

    Protected Overridable Function AIDecideWildColour()
        Return ""
    End Function

End Class

Public Class EasyAIGame
    Inherits AIGame

    Protected Overrides Sub AIFindSolution()
        Dim PossibleSolutions As New List(Of Integer)
        Dim i As Integer

        FormGameMenu.Sleeping = True
        My.Application.DoEvents()
        System.Threading.Thread.Sleep(500)
        FormGameMenu.Sleeping = False

        For i = 0 To DealingPlaceP2 - 1
            If CheckValidity(P2Hand(i).Colour, P2Hand(i).Type, LastPlayed) Then
                PossibleSolutions.Add(i)
            End If
        Next

        AIMakeDecision(PossibleSolutions)
    End Sub

    Private Sub AIMakeDecision(ByVal PossibleSolutions As System.Collections.Generic.List(Of Integer))
        If PossibleSolutions.Count <> 0 Then
            Randomize()
            PlayCard(P2Hand, PossibleSolutions(Int(Rnd() * PossibleSolutions.Count)), DealingPlaceP2)
        Else 'No solution found
            AINoSolutionFound()
        End If
    End Sub

    Protected Overrides Function AIDecideWildColour() As Object
        Dim i As Integer
        Randomize()
        i = Int(Rnd() * 4)
        Select Case i
            Case 0
                Return "Blue"
            Case 1
                Return "Golden"
            Case 2
                Return "Green"
            Case Else
                Return "Red"
        End Select
    End Function

End Class

Public Class HardAIGame
    Inherits AIGame

    Private Sub GatherData()
        Dim i As Integer
        Dim P2MaxNum As Integer

        HandData.P2Blue = 0
        HandData.P2Golden = 0
        HandData.P2Green = 0
        HandData.P2Red = 0
        HandData.P2NonWild = 0
        HandData.P2NonWord = 0

        For i = 0 To DealingPlaceP2 - 1
            Select Case P2Hand(i).Colour
                Case "Blue"
                    HandData.P2Blue += 1
                Case "Golden"
                    HandData.P2Golden += 1
                Case "Green"
                    HandData.P2Green += 1
                Case "Red"
                    HandData.P2Red += 1
            End Select
            If Len(P2Hand(i).Type) = 1 Then
                HandData.P2NonWord += 1
            End If
        Next

        HandData.P2NonWild = HandData.P2Blue + HandData.P2Golden + HandData.P2Green + HandData.P2Red

        P2MaxNum = HandData.P2Blue
        HandData.P2MaxColour = "Blue"
        If P2MaxNum < HandData.P2Golden Then
            P2MaxNum = HandData.P2Golden
            HandData.P2MaxColour = "Golden"
        End If
        If P2MaxNum < HandData.P2Green Then
            P2MaxNum = HandData.P2Green
            HandData.P2MaxColour = "Green"
        End If
        If P2MaxNum < HandData.P2Red Then
            P2MaxNum = HandData.P2Red
            HandData.P2MaxColour = "Red"
        End If
    End Sub

    Protected Overrides Sub AIFindSolution()
        Dim i As Integer
        Dim PossibleSolutions As New List(Of Integer)

        GatherData()
        FormGameMenu.Sleeping = True
        My.Application.DoEvents()
        System.Threading.Thread.Sleep(500)
        FormGameMenu.Sleeping = False

        If DealingPlaceP1 = 1 Then 'P1 has potential to win next turn
            For i = 0 To DealingPlaceP2 - 1
                If CheckValidity(P2Hand(i).Colour, P2Hand(i).Type, LastPlayed) And (P2Hand(i).Type = "+2" Or P2Hand(i).Type = "+4" Or P2Hand(i).Type = "Skip" Or P2Hand(i).Type = "Reverse") Then
                    PossibleSolutions.Add(i)
                End If
            Next

            If PossibleSolutions.Count = 0 Then 'no best "Preventing" card found - try to simply convert colour
                For i = 0 To DealingPlaceP2 - 1
                    If CheckValidity(P2Hand(i).Colour, P2Hand(i).Type, LastPlayed) And P2Hand(i).Colour <> LastPlayed.Colour Then
                        PossibleSolutions.Add(i)
                    End If
                Next
            End If
        Else 'normal situation : sticking with same colour if possible, otherwise using connectors to change colour

            For i = 0 To DealingPlaceP2 - 1
                If CheckValidity(P2Hand(i).Colour, P2Hand(i).Type, LastPlayed) And ((LastPlayed.Colour = P2Hand(i).Colour And LastPlayed.Colour <> "Wild") Or (LastPlayed.Colour = "Wild" And LastPlayed.WildColour = P2Hand(i).Colour)) Then
                    PossibleSolutions.Add(i)
                End If
            Next

            If PossibleSolutions.Count = 0 Then 'no card of same colour found - searching forconnector to the colour that it has most hand of.
                For i = 0 To DealingPlaceP2 - 1
                    If CheckValidity(P2Hand(i).Colour, P2Hand(i).Type, LastPlayed) Then
                        Select Case HandData.P2MaxColour
                            Case "Blue"
                                If P2Hand(i).Colour = "Blue" Then
                                    PossibleSolutions.Add(i)
                                End If
                            Case "Golden"
                                If P2Hand(i).Colour = "Golden" Then
                                    PossibleSolutions.Add(i)
                                End If
                            Case "Green"
                                If P2Hand(i).Colour = "Green" Then
                                    PossibleSolutions.Add(i)
                                End If
                            Case "Red"
                                If P2Hand(i).Colour = "Red" Then
                                    PossibleSolutions.Add(i)
                                End If
                        End Select
                    End If

                    If P2Hand(i).Colour = "Wild" And (LastPlayed.Type <> "+2" Or LastPlayed.IfPunished = True) Then
                        PossibleSolutions.Add(i)
                    End If

                Next
            End If
        End If

        If PossibleSolutions.Count = 0 And DealingPlaceP1 > 1 Then 'No best solution found - add all non-best solustions
            For i = 0 To DealingPlaceP2 - 1
                If CheckValidity(P2Hand(i).Colour, P2Hand(i).Type, LastPlayed) Then
                    PossibleSolutions.Add(i)
                End If
            Next
        End If

        AIMakeDecision(PossibleSolutions)

    End Sub

    Private Sub AIMakeDecision(ByVal PossibleSolutions As System.Collections.Generic.List(Of Integer))
        'weight: number card > Skip,Reverse and +2 > Wild card

        If PossibleSolutions.Count > 0 Then
            If HandData.P2NonWord > 1 Then

                For Each Solution In PossibleSolutions
                    If P2Hand(Solution).Type = "Skip" Or P2Hand(Solution).Type = "Reverse" Or P2Hand(Solution).Type = "+2" Then
                        PlayCard(P2Hand, Solution, DealingPlaceP2)
                        Exit Sub
                    End If
                Next

                For Each Solution In PossibleSolutions
                    If Len(P2Hand(Solution).Type) = 1 Then
                        PlayCard(P2Hand, Solution, DealingPlaceP2)
                        Exit Sub
                    End If
                Next

                For Each Solution In PossibleSolutions
                    If P2Hand(Solution).Colour = "Wild" Then
                        PlayCard(P2Hand, Solution, DealingPlaceP2)
                        Exit Sub
                    End If
                Next
            Else 'only one non-word card remained - priority to word card except for Wild

                For Each Solution In PossibleSolutions
                    If P2Hand(Solution).Type = "Skip" Or P2Hand(Solution).Type = "Reverse" Then
                        PlayCard(P2Hand, Solution, DealingPlaceP2)
                        Exit Sub
                    End If
                Next

                For Each Solution In PossibleSolutions
                    If P2Hand(Solution).Type = "+4" Then
                        PlayCard(P2Hand, Solution, DealingPlaceP2)
                        Exit Sub
                    End If
                Next

                For Each Solution In PossibleSolutions
                    If P2Hand(Solution).Type = "+2" Then
                        PlayCard(P2Hand, Solution, DealingPlaceP2)
                        Exit Sub
                    End If
                Next

                For Each Solution In PossibleSolutions
                    If Len(P2Hand(Solution).Type) = 1 Then
                        PlayCard(P2Hand, Solution, DealingPlaceP2)
                        Exit Sub
                    End If
                Next

                For Each Solution In PossibleSolutions
                    If P2Hand(Solution).Colour = "Wild" Then
                        PlayCard(P2Hand, Solution, DealingPlaceP2)
                        Exit Sub
                    End If
                Next
            End If
        Else
            AINoSolutionFound()
        End If

    End Sub

    Protected Overrides Function AIDecideWildColour() As Object
        Return HandData.P2MaxColour
    End Function

End Class

Public Class CheaterAIGame
    Inherits AIGame

    Private Sub GatherData()
        Dim i As Integer
        Dim P1MinNum As Integer
        Dim P2MaxNum As Integer

        HandData.P1Blue = 0
        HandData.P1Golden = 0
        HandData.P1Green = 0
        HandData.P1Red = 0
        HandData.P1NonWild = 0
        HandData.P2Blue = 0
        HandData.P2Golden = 0
        HandData.P2Green = 0
        HandData.P2Red = 0
        HandData.P2NonWild = 0
        HandData.P2NonWord = 0

        For i = 0 To DealingPlaceP1 - 1
            Select Case P1Hand(i).Colour
                Case "Blue"
                    HandData.P1Blue += 1
                Case "Golden"
                    HandData.P1Golden += 1
                Case "Green"
                    HandData.P1Green += 1
                Case "Red"
                    HandData.P1Red += 1
            End Select
        Next

        For i = 0 To DealingPlaceP2 - 1
            Select Case P2Hand(i).Colour
                Case "Blue"
                    HandData.P2Blue += 1
                Case "Golden"
                    HandData.P2Golden += 1
                Case "Green"
                    HandData.P2Green += 1
                Case "Red"
                    HandData.P2Red += 1
            End Select
            If Len(P2Hand(i).Type) = 1 Then
                HandData.P2NonWord += 1
            End If
        Next

        HandData.P1NonWild = HandData.P1Blue + HandData.P1Golden + HandData.P1Green + HandData.P1Red
        HandData.P2NonWild = HandData.P2Blue + HandData.P2Golden + HandData.P2Green + HandData.P2Red

        'determining P1Min and P2Max
        P1MinNum = HandData.P1Blue
        HandData.P1MinColour = "Blue"
        If P1MinNum > HandData.P1Golden Then
            P1MinNum = HandData.P1Golden
            HandData.P1MinColour = "Golden"
        End If
        If P1MinNum > HandData.P1Green Then
            P1MinNum = HandData.P1Green
            HandData.P1MinColour = "Green"
        End If
        If P1MinNum > HandData.P1Red Then
            P1MinNum = HandData.P1Red
            HandData.P1MinColour = "Red"
        End If

        P2MaxNum = HandData.P2Blue
        HandData.P2MaxColour = "Blue"
        If P2MaxNum < HandData.P2Golden Then
            P2MaxNum = HandData.P2Golden
            HandData.P2MaxColour = "Golden"
        End If
        If P2MaxNum < HandData.P2Green Then
            P2MaxNum = HandData.P2Green
            HandData.P2MaxColour = "Green"
        End If
        If P2MaxNum < HandData.P2Red Then
            P2MaxNum = HandData.P2Red
            HandData.P2MaxColour = "Red"
        End If

    End Sub

    Private Sub Solution_AIHasAd(ByRef PossibleSolutions As List(Of Integer)) 'AI's Non-Wild hand is less
        'AI should ensure its hand can be played successfully

        For i = 0 To DealingPlaceP2 - 1
            If CheckValidity(P2Hand(i).Colour, P2Hand(i).Type, LastPlayed) And ((LastPlayed.Colour = P2Hand(i).Colour And LastPlayed.Colour <> "Wild") Or (LastPlayed.Colour = "Wild" And LastPlayed.WildColour = P2Hand(i).Colour)) Then
                PossibleSolutions.Add(i)
            End If
        Next

        If PossibleSolutions.Count = 0 Then 'no card of same colour found - searching forconnector to the colour that it has most hand of.
            For i = 0 To DealingPlaceP2 - 1
                If CheckValidity(P2Hand(i).Colour, P2Hand(i).Type, LastPlayed) Then
                    Select Case HandData.P2MaxColour
                        Case "Blue"
                            If P2Hand(i).Colour = "Blue" Then
                                PossibleSolutions.Add(i)
                            End If
                        Case "Golden"
                            If P2Hand(i).Colour = "Golden" Then
                                PossibleSolutions.Add(i)
                            End If
                        Case "Green"
                            If P2Hand(i).Colour = "Green" Then
                                PossibleSolutions.Add(i)
                            End If
                        Case "Red"
                            If P2Hand(i).Colour = "Red" Then
                                PossibleSolutions.Add(i)
                            End If
                    End Select
                End If

                If P2Hand(i).Colour = "Wild" And (LastPlayed.Type <> "+2" Or LastPlayed.IfPunished = True) Then
                    PossibleSolutions.Add(i)
                End If
            Next

        End If

    End Sub

    Private Sub Solution_PlayerHasAd(ByRef PossibleSolutions As List(Of Integer)) 'Player's Non-Wild hand is less
        'AI should prevent player from playing cards successfully
        Dim i As Integer

        For i = 0 To DealingPlaceP2 - 1
            If CheckValidity(P2Hand(i).Colour, P2Hand(i).Type, LastPlayed) And ((LastPlayed.Colour = P2Hand(i).Colour And LastPlayed.Colour <> "Wild") Or (LastPlayed.Colour = "Wild" And LastPlayed.WildColour = P2Hand(i).Colour)) Then
                PossibleSolutions.Add(i)
            End If
        Next

        If PossibleSolutions.Count = 0 Then 'no card of same colour found - searching for connector to the colour that player has least hand of.

            For i = 0 To DealingPlaceP2 - 1
                If CheckValidity(P2Hand(i).Colour, P2Hand(i).Type, LastPlayed) Then
                    Select Case HandData.P1MinColour
                        Case "Blue"
                            If P2Hand(i).Colour = "Blue" Then
                                PossibleSolutions.Add(i)
                            End If
                        Case "Golden"
                            If P2Hand(i).Colour = "Golden" Then
                                PossibleSolutions.Add(i)
                            End If
                        Case "Green"
                            If P2Hand(i).Colour = "Green" Then
                                PossibleSolutions.Add(i)
                            End If
                        Case "Red"
                            If P2Hand(i).Colour = "Red" Then
                                PossibleSolutions.Add(i)
                            End If
                    End Select
                End If

                If P2Hand(i).Colour = "Wild" And (LastPlayed.Type <> "+2" Or LastPlayed.IfPunished = True) Then
                    PossibleSolutions.Add(i)
                End If
            Next

        End If
    End Sub

    Private Sub AIMakeDecision(ByVal PossibleSolutions As System.Collections.Generic.List(Of Integer))
        'weight: number card > Skip,Reverse and +2 > Wild card

        If PossibleSolutions.Count > 0 Then
            If HandData.P2NonWord > 1 Then

                For Each Solution In PossibleSolutions
                    If P2Hand(Solution).Type = "Skip" Or P2Hand(Solution).Type = "Reverse" Or P2Hand(Solution).Type = "+2" Then
                        PlayCard(P2Hand, Solution, DealingPlaceP2)
                        Exit Sub
                    End If
                Next

                For Each Solution In PossibleSolutions
                    If Len(P2Hand(Solution).Type) = 1 Then
                        PlayCard(P2Hand, Solution, DealingPlaceP2)
                        Exit Sub
                    End If
                Next

                For Each Solution In PossibleSolutions
                    If P2Hand(Solution).Colour = "Wild" Then
                        PlayCard(P2Hand, Solution, DealingPlaceP2)
                        Exit Sub
                    End If
                Next
            Else 'only one non-word card remained - priority to word card except for Wild

                For Each Solution In PossibleSolutions
                    If P2Hand(Solution).Type = "Skip" Or P2Hand(Solution).Type = "Reverse" Then
                        PlayCard(P2Hand, Solution, DealingPlaceP2)
                        Exit Sub
                    End If
                Next

                For Each Solution In PossibleSolutions
                    If P2Hand(Solution).Type = "+4" Then
                        PlayCard(P2Hand, Solution, DealingPlaceP2)
                        Exit Sub
                    End If
                Next

                For Each Solution In PossibleSolutions
                    If P2Hand(Solution).Type = "+2" Then
                        PlayCard(P2Hand, Solution, DealingPlaceP2)
                        Exit Sub
                    End If
                Next

                For Each Solution In PossibleSolutions
                    If Len(P2Hand(Solution).Type) = 1 Then
                        PlayCard(P2Hand, Solution, DealingPlaceP2)
                        Exit Sub
                    End If
                Next

                For Each Solution In PossibleSolutions
                    If P2Hand(Solution).Colour = "Wild" Then
                        PlayCard(P2Hand, Solution, DealingPlaceP2)
                        Exit Sub
                    End If
                Next
            End If
        Else
            AINoSolutionFound()
        End If
    End Sub

    Protected Overrides Sub AIFindSolution()
        Dim i As Integer
        Dim PossibleSolutions As New List(Of Integer)

        GatherData()
        FormGameMenu.Sleeping = True
        My.Application.DoEvents()
        System.Threading.Thread.Sleep(500)
        FormGameMenu.Sleeping = False

        If DealingPlaceP1 = 1 And CheckValidity(P1Hand(0).Colour, P1Hand(0).Type, LastPlayed) Then 'player is going to win next turn
            For i = 0 To DealingPlaceP2 - 1
                If CheckValidity(P2Hand(i).Colour, P2Hand(i).Type, LastPlayed) Then
                    ' prevent player from winning
                    Dim SL As New LastPlayedCard 'run a Simulation Lastplayed card
                    SL.Colour = P2Hand(i).Colour
                    SL.Type = P2Hand(i).Type
                    If SL.Colour = "Wild" Then
                        SL.WildColour = AIDecideWildColour()
                    End If

                    If Not CheckValidity(P1Hand(0).Colour, P1Hand(0).Type, SL) Then 'preventing success
                        PossibleSolutions.Add(i)
                    End If
                End If
            Next
        ElseIf FixedDeck(108 - DeckRemained).Colour = "Wild" And AlreadyDraw = False And LastPlayed.Type <> "+2" Then 'draw valuable card
            Randomize()
            If Rnd() < 0.7 Then
                AINoSolutionFound()
                Exit Sub
            End If
        Else 'normal situation : sticking with same colour if possible, otherwise using connectors to change colour
            If HandData.P1NonWild <= HandData.P2NonWild Then
                Solution_PlayerHasAd(PossibleSolutions)
            Else
                Solution_AIHasAd(PossibleSolutions)
            End If

        End If

        If PossibleSolutions.Count = 0 And DealingPlaceP1 > 1 Then 'No best solution found - add all non-best solustions
            For i = 0 To DealingPlaceP2 - 1
                If CheckValidity(P2Hand(i).Colour, P2Hand(i).Type, LastPlayed) Then
                    PossibleSolutions.Add(i)
                End If
            Next
        End If

        AIMakeDecision(PossibleSolutions)

    End Sub

    Protected Overrides Function AIDecideWildColour() As Object

        If LastPlayed.Type = "" Then
            If HandData.P1NonWild <= HandData.P2NonWild Then
                Select Case HandData.P1MinColour
                    Case "Blue"
                        If HandData.P2Blue > 0 Then
                            Return HandData.P1MinColour
                        End If
                    Case "Golden"
                        If HandData.P2Golden > 0 Then
                            Return HandData.P1MinColour
                        End If
                    Case "Green"
                        If HandData.P2Green > 0 Then
                            Return HandData.P1MinColour
                        End If
                    Case "Red"
                        If HandData.P2Red > 0 Then
                            Return HandData.P1MinColour
                        End If
                End Select
                Return HandData.P2MaxColour
            Else
                Return HandData.P2MaxColour
            End If
        Else 'wild +4 is played
            If HandData.P1NonWild + 4 <= HandData.P2NonWild Then

                Select Case HandData.P1MinColour
                    Case "Blue"
                        If HandData.P2Blue > 0 Then
                            Return HandData.P1MinColour
                        End If
                    Case "Golden"
                        If HandData.P2Golden > 0 Then
                            Return HandData.P1MinColour
                        End If
                    Case "Green"
                        If HandData.P2Green > 0 Then
                            Return HandData.P1MinColour
                        End If
                    Case "Red"
                        If HandData.P2Red > 0 Then
                            Return HandData.P1MinColour
                        End If
                End Select
                Return HandData.P2MaxColour
            Else
                Return HandData.P2MaxColour
            End If
        End If
    End Function

    Protected Overrides Sub SetUpDeck()
        MyBase.SetUpDeck()
        Dim counter, cardnum As Integer

        Randomize() 'generate randomly ordered deck
        For counter = 0 To 107
            Do
                cardnum = Int(Rnd() * 108)
            Loop Until Deck(cardnum).Type <> "None"
            FixedDeck(counter) = New Card
            FixedDeck(counter).Type = Deck(cardnum).Type
            FixedDeck(counter).Colour = Deck(cardnum).Colour
            Deck(cardnum).Type = "None"
            Deck(cardnum).Colour = ""
        Next

        For counter = 0 To 107
            Deck(counter).Type = FixedDeck(counter).Type
            Deck(counter).Colour = FixedDeck(counter).Colour
        Next
    End Sub

    Public Overrides Sub Deal(ByRef PlayerHand() As Card, ByRef DealingPlace As Integer)
        If DeckRemained <> 0 Then
            If DealingPlace <= 19 Then
                PlayerHand(DealingPlace).Type = FixedDeck(108 - DeckRemained).Type
                PlayerHand(DealingPlace).Colour = FixedDeck(108 - DeckRemained).Colour
                FixedDeck(108 - DeckRemained).Type = "None"
                FixedDeck(108 - DeckRemained).Colour = ""
                DealingPlace += 1
                DeckRemained -= 1
            Else
                MsgBox("Can't hold more than 20 cards in hand!")
            End If
        Else
            MsgBox("No more card in the deck!")
            WhoseTurn = ""
            RefreshCards()
        End If

    End Sub

End Class


