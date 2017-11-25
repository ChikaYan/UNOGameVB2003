Public Class Game

    Public P1Hand(19), P2Hand(19), Deck(107) As Card
    Public DeckRemained As Integer
    Public LastPlayed As LastPlayedCard
    Public DealingPlaceP1, DealingPlaceP2 As Integer
    Public WhoseTurn As String
    Public AlreadyDraw As Boolean 'determine whether player has drawn a card from deck in this turn
    Public Form As FormPlayAgainstPlayer

    Structure Card
        Dim Type As String
        Dim Colour As String
    End Structure

    Structure LastPlayedCard
        Dim Type As String
        Dim Colour As String
        Dim WildColour As String 'if LastPlayed card is a wild or wild +4 card, this variable is used to store the colour named by player
        Dim IfPunished As Boolean 'if LastPlayed is word card, this value indicates whether the punishment has been applied. If ture, next required card would only be card with same colour
    End Structure

    Sub New(ByVal fm As FormPlayAgainstPlayer)
        SetUpDeck()
        SetUpHand()

        Me.Form = fm
        LastPlayed.Colour = ""
        LastPlayed.Type = "None"
        LastPlayed.WildColour = ""
        LastPlayed.IfPunished = False

        WhoseTurn = "P2"
        TurnStart()
    End Sub

    Private Sub SetUpDeck()
        Dim i, j As Integer

        DeckRemained = 108
        For i = 0 To 7
            For j = 0 To 11
                If j + 1 <= 9 Then '1 to 9
                    Deck(12 * i + j).Type = (j + 1)
                ElseIf j + 1 = 10 Then 'Skip
                    Deck(12 * i + j).Type = "Skip"
                ElseIf j + 1 = 11 Then 'Reverse
                    Deck(12 * i + j).Type = "Reverse"
                ElseIf j + 1 = 12 Then '+2
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
            Deck(i).Type = "0"
        Next
        Deck(96).Colour = "Blue"
        Deck(97).Colour = "Golden"
        Deck(98).Colour = "Green"
        Deck(99).Colour = "Red"

        For i = 100 To 107
            Deck(i).Colour = "Wild"
            If i <= 103 Then
                Deck(i).Type = "" 'word card Wild
            Else
                Deck(i).Type = "+4" 'word card Wild +4
            End If
        Next


    End Sub

    Private Sub SetUpHand()
        Dim i As Integer

        For i = 0 To 19  'clean all the hand
            P1Hand(i).Type = "None"
            P1Hand(i).Colour = ""
            P2Hand(i).Type = "None"
            P2Hand(i).Colour = ""
        Next
        DealingPlaceP1 = 0   'start dealing at 0
        DealingPlaceP2 = 0

        For i = 1 To 7 'deal 7 cards to p1 and p2 
            Deal(P1Hand, DealingPlaceP1)
            Deal(P2Hand, DealingPlaceP2)
        Next

    End Sub

    Public Sub Deal(ByRef PlayerHand() As Card, ByRef DealingPlace As Integer)
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
                Deck(indicator).Colour = "None"

                DealingPlace = DealingPlace + 1
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

    Public Sub TurnStart()

        If WhoseTurn = "P1" Then
            WhoseTurn = "P2"
        ElseIf WhoseTurn = "P2" Then
            WhoseTurn = "P1"
        End If
        AlreadyDraw = False
        RefreshCards()

        If LastPlayed.IfPunished = False Then 'Give punishment
            If LastPlayed.Type = "Skip" Then 'Punish "Skip"
                MsgBox(WhoseTurn & "'s Turn has been skipped!")
                LastPlayed.IfPunished = True
                TurnStart()
                Exit Sub
            ElseIf LastPlayed.Type = "Reverse" Then 'Punish "Reverse"
                MsgBox("Play order reversed!")
                LastPlayed.IfPunished = True
                If WhoseTurn = "P1" Then
                    MsgBox("P2 gains one more turn")
                Else
                    MsgBox("P1 gains one more turn")
                End If
                TurnStart()
                Exit Sub
            ElseIf LastPlayed.Type = "+4" Then
                MsgBox(WhoseTurn & "'s Turn has been skipped!") 'punish "Wild +4"
                MsgBox("Draw four more cards!")
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

        'wait for player move

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

    Public Sub PlayCard(ByRef PlayerHand() As Card, ByVal HandCardNumber As Integer, ByRef DealingPlace As Integer)

        Dim WildColourInteger As Integer

        If ((PlayerHand(HandCardNumber).Type = LastPlayed.Type Or PlayerHand(HandCardNumber).Colour = LastPlayed.Colour Or PlayerHand(HandCardNumber).Colour = "Wild") And (Len(LastPlayed.Type) = 1)) Or (LastPlayed.Type = "+2" And PlayerHand(HandCardNumber).Type = "+4") Or (LastPlayed.Colour = "Wild" And PlayerHand(HandCardNumber).Colour = LastPlayed.WildColour Or PlayerHand(HandCardNumber).Colour = "Wild") Or (LastPlayed.IfPunished = True And PlayerHand(HandCardNumber).Colour = LastPlayed.Colour Or PlayerHand(HandCardNumber).Colour = "Wild") Or LastPlayed.Type = "None" Then

            LastPlayed.Type = PlayerHand(HandCardNumber).Type
            LastPlayed.Colour = PlayerHand(HandCardNumber).Colour
            LastPlayed.IfPunished = False
            LastPlayed.WildColour = ""

            If LastPlayed.Colour = "Wild" Then
                Do While LastPlayed.WildColour = ""
                    WildColourInteger = InputBox("Select next colour: 1 for Blue, 2 for Golden, 3 for Green, 4 for Red")
                    Select Case WildColourInteger
                        Case 1
                            LastPlayed.WildColour = "Blue"
                        Case 2
                            LastPlayed.WildColour = "Golden"
                        Case 3
                            LastPlayed.WildColour = "Green"
                        Case 4
                            LastPlayed.WildColour = "Red"
                    End Select
                Loop
            End If
            RemoveCard(PlayerHand, HandCardNumber, DealingPlace)

            If DealingPlace <> 0 Then
                TurnStart()
            End If
        Else
            MsgBox("You can't play this card! Choose another card to play")
        End If

    End Sub

    Public Sub RefreshCards()
        Dim i As Integer

        For i = 1 To 43 'Refresh all picturebox
            DrawPictureBox("PictureBox" & i, "", "None")
        Next

        DrawPictureBox("PictureBox41", LastPlayed.Colour, LastPlayed.Type) 'Draw Lastplayed Card
        If LastPlayed.Colour = "Wild" Then
            DrawPictureBox("PictureBox43", LastPlayed.WildColour, "PureColour")
        End If
        DrawPictureBox("PictureBox42", "", "Back") 'draw deck
        'if lasplayed.wildcolour <> "None" then ShowWildColour()

        Select Case WhoseTurn
            Case "P1" 'Display P1's hand and hide P2's hand
                For i = 0 To DealingPlaceP1 - 1 'i = hand number = picturebox number -1
                    DrawPictureBox("PictureBox" & i + 1, P1Hand(i).Colour, P1Hand(i).Type)
                Next
                For i = 0 To DealingPlaceP2 - 1
                    DrawPictureBox("PictureBox" & i + 21, "", "Back")
                Next
            Case "P2"
                For i = 0 To DealingPlaceP2 - 1
                    DrawPictureBox("PictureBox" & i + 21, P2Hand(i).Colour, P2Hand(i).Type)
                Next
                For i = 0 To DealingPlaceP1 - 1
                    DrawPictureBox("PictureBox" & i + 1, "", "Back")
                Next
            Case ""
                For i = 0 To DealingPlaceP1 - 1 'i = hand number = picturebox number -1
                    DrawPictureBox("PictureBox" & i + 1, P1Hand(i).Colour, P1Hand(i).Type)
                Next
                For i = 0 To DealingPlaceP2 - 1
                    DrawPictureBox("PictureBox" & i + 21, P2Hand(i).Colour, P2Hand(i).Type)
                Next
        End Select
    End Sub

    Public Sub DrawPictureBox(ByVal PictureBoxName As String, ByVal CardColour As String, ByVal CardType As String)


        Select Case PictureBoxName
            Case "PictureBox1"
                If CardType <> "None" Then
                    Form.PictureBox1.Image = My.Resources.ResourceManager.GetObject(CardColour & CardType)
                    form.PictureBox1.Show()
                    form.PictureBox1.Enabled = True
                Else
                    form.PictureBox1.Image = Nothing
                    form.PictureBox1.Hide()
                    form.PictureBox1.Enabled = False
                End If
            Case "PictureBox2"
                If CardType <> "None" Then
                    form.PictureBox2.Image = My.Resources.ResourceManager.GetObject(CardColour & CardType)
                    form.PictureBox2.Show()
                    form.PictureBox2.Enabled = True
                Else
                    form.PictureBox2.Image = Nothing
                    form.PictureBox2.Hide()
                    form.PictureBox2.Enabled = False
                End If
            Case "PictureBox3"
                If CardType <> "None" Then
                    form.PictureBox3.Image = My.Resources.ResourceManager.GetObject(CardColour & CardType)
                    form.PictureBox3.Show()
                    form.PictureBox3.Enabled = True
                Else
                    form.PictureBox3.Image = Nothing
                    form.PictureBox3.Hide()
                    form.PictureBox3.Enabled = False
                End If
            Case "PictureBox4"
                If CardType <> "None" Then
                    form.PictureBox4.Image = My.Resources.ResourceManager.GetObject(CardColour & CardType)
                    form.PictureBox4.Show()
                    form.PictureBox4.Enabled = True
                Else
                    form.PictureBox4.Image = Nothing
                    form.PictureBox4.Hide()
                    form.PictureBox4.Enabled = False
                End If
            Case "PictureBox5"
                If CardType <> "None" Then
                    form.PictureBox5.Image = My.Resources.ResourceManager.GetObject(CardColour & CardType)
                    form.PictureBox5.Show()
                    form.PictureBox5.Enabled = True
                Else
                    form.PictureBox5.Image = Nothing
                    form.PictureBox5.Hide()
                    form.PictureBox5.Enabled = False
                End If
            Case "PictureBox6"
                If CardType <> "None" Then
                    form.PictureBox6.Image = My.Resources.ResourceManager.GetObject(CardColour & CardType)
                    form.PictureBox6.Show()
                    form.PictureBox6.Enabled = True
                Else
                    form.PictureBox6.Image = Nothing
                    form.PictureBox6.Hide()
                    form.PictureBox6.Enabled = False
                End If
            Case "PictureBox7"
                If CardType <> "None" Then
                    form.PictureBox7.Image = My.Resources.ResourceManager.GetObject(CardColour & CardType)
                    form.PictureBox7.Show()
                    form.PictureBox7.Enabled = True
                Else
                    form.PictureBox7.Image = Nothing
                    form.PictureBox7.Hide()
                    form.PictureBox7.Enabled = False
                End If
            Case "PictureBox8"
                If CardType <> "None" Then
                    form.PictureBox8.Image = My.Resources.ResourceManager.GetObject(CardColour & CardType)
                    form.PictureBox8.Show()
                    form.PictureBox8.Enabled = True
                Else
                    form.PictureBox8.Image = Nothing
                    form.PictureBox8.Hide()
                    form.PictureBox8.Enabled = False
                End If
            Case "PictureBox9"
                If CardType <> "None" Then
                    form.PictureBox9.Image = My.Resources.ResourceManager.GetObject(CardColour & CardType)
                    form.PictureBox9.Show()
                    form.PictureBox9.Enabled = True
                Else
                    form.PictureBox9.Image = Nothing
                    form.PictureBox9.Hide()
                    form.PictureBox9.Enabled = False
                End If
            Case "PictureBox10"
                If CardType <> "None" Then
                    form.PictureBox10.Image = My.Resources.ResourceManager.GetObject(CardColour & CardType)
                    form.PictureBox10.Show()
                    form.PictureBox10.Enabled = True
                Else
                    form.PictureBox10.Image = Nothing
                    form.PictureBox10.Hide()
                    form.PictureBox10.Enabled = False
                End If
            Case "PictureBox11"
                If CardType <> "None" Then
                    form.PictureBox11.Image = My.Resources.ResourceManager.GetObject(CardColour & CardType)
                    form.PictureBox11.Show()
                    form.PictureBox11.Enabled = True
                Else
                    form.PictureBox11.Image = Nothing
                    form.PictureBox11.Hide()
                    form.PictureBox11.Enabled = False
                End If
            Case "PictureBox12"
                If CardType <> "None" Then
                    form.PictureBox12.Image = My.Resources.ResourceManager.GetObject(CardColour & CardType)
                    form.PictureBox12.Show()
                    form.PictureBox12.Enabled = True
                Else
                    form.PictureBox12.Image = Nothing
                    form.PictureBox12.Hide()
                    form.PictureBox12.Enabled = False
                End If
            Case "PictureBox13"
                If CardType <> "None" Then
                    form.PictureBox13.Image = My.Resources.ResourceManager.GetObject(CardColour & CardType)
                    form.PictureBox13.Show()
                    form.PictureBox13.Enabled = True
                Else
                    form.PictureBox13.Image = Nothing
                    form.PictureBox13.Hide()
                    form.PictureBox13.Enabled = False
                End If
            Case "PictureBox14"
                If CardType <> "None" Then
                    form.PictureBox14.Image = My.Resources.ResourceManager.GetObject(CardColour & CardType)
                    form.PictureBox14.Show()
                    form.PictureBox14.Enabled = True
                Else
                    form.PictureBox14.Image = Nothing
                    form.PictureBox14.Hide()
                    form.PictureBox14.Enabled = False
                End If


            Case "PictureBox15"
                If CardType <> "None" Then
                    form.PictureBox15.Image = My.Resources.ResourceManager.GetObject(CardColour & CardType)
                    form.PictureBox15.Show()
                    form.PictureBox15.Enabled = True
                Else
                    form.PictureBox15.Image = Nothing
                    form.PictureBox15.Hide()
                    form.PictureBox15.Enabled = False
                End If
            Case "PictureBox16"
                If CardType <> "None" Then
                    form.PictureBox16.Image = My.Resources.ResourceManager.GetObject(CardColour & CardType)
                    form.PictureBox16.Show()
                    form.PictureBox16.Enabled = True
                Else
                    form.PictureBox16.Image = Nothing
                    form.PictureBox16.Hide()
                    form.PictureBox16.Enabled = False
                End If
            Case "PictureBox17"
                If CardType <> "None" Then
                    form.PictureBox17.Image = My.Resources.ResourceManager.GetObject(CardColour & CardType)
                    form.PictureBox17.Show()
                    form.PictureBox17.Enabled = True
                Else
                    Form.PictureBox17.Image = Nothing
                    Form.PictureBox17.Hide()
                    Form.PictureBox17.Enabled = False
                End If
            Case "PictureBox18"
                If CardType <> "None" Then
                    form.PictureBox18.Image = My.Resources.ResourceManager.GetObject(CardColour & CardType)
                    form.PictureBox18.Show()
                    form.PictureBox18.Enabled = True
                Else
                    Form.PictureBox18.Image = Nothing
                    Form.PictureBox18.Hide()
                    Form.PictureBox18.Enabled = False
                End If
            Case "PictureBox19"
                If CardType <> "None" Then
                    form.PictureBox19.Image = My.Resources.ResourceManager.GetObject(CardColour & CardType)
                    form.PictureBox19.Show()
                    form.PictureBox19.Enabled = True
                Else
                    form.PictureBox19.Image = Nothing
                    form.PictureBox19.Hide()
                    form.PictureBox19.Enabled = False
                End If
            Case "PictureBox20"
                If CardType <> "None" Then
                    form.PictureBox20.Image = My.Resources.ResourceManager.GetObject(CardColour & CardType)
                    form.PictureBox20.Show()
                    form.PictureBox20.Enabled = True
                Else
                    form.PictureBox20.Image = Nothing
                    form.PictureBox20.Hide()
                    form.PictureBox20.Enabled = False
                End If



            Case "PictureBox21"
                If CardType <> "None" Then
                    form.PictureBox21.Image = My.Resources.ResourceManager.GetObject(CardColour & CardType)
                    form.PictureBox21.Show()
                    form.PictureBox21.Enabled = True
                Else
                    Form.PictureBox21.Image = Nothing
                    Form.PictureBox21.Hide()
                    Form.PictureBox21.Enabled = False
                End If
            Case "PictureBox22"
                If CardType <> "None" Then
                    form.PictureBox22.Image = My.Resources.ResourceManager.GetObject(CardColour & CardType)
                    form.PictureBox22.Show()
                    form.PictureBox22.Enabled = True
                Else
                    Form.PictureBox22.Image = Nothing
                    Form.PictureBox22.Hide()
                    Form.PictureBox22.Enabled = False
                End If
            Case "PictureBox23"
                If CardType <> "None" Then
                    form.PictureBox23.Image = My.Resources.ResourceManager.GetObject(CardColour & CardType)
                    form.PictureBox23.Show()
                    form.PictureBox23.Enabled = True
                Else
                    form.PictureBox23.Image = Nothing
                    form.PictureBox23.Hide()
                    form.PictureBox23.Enabled = False
                End If
            Case "PictureBox24"
                If CardType <> "None" Then
                    form.PictureBox24.Image = My.Resources.ResourceManager.GetObject(CardColour & CardType)
                    form.PictureBox24.Show()
                    form.PictureBox24.Enabled = True
                Else
                    form.PictureBox24.Image = Nothing
                    form.PictureBox24.Hide()
                    form.PictureBox24.Enabled = False
                End If
            Case "PictureBox25"
                If CardType <> "None" Then
                    form.PictureBox25.Image = My.Resources.ResourceManager.GetObject(CardColour & CardType)
                    form.PictureBox25.Show()
                    form.PictureBox25.Enabled = True
                Else
                    form.PictureBox25.Image = Nothing
                    form.PictureBox25.Hide()
                    form.PictureBox25.Enabled = False
                End If
            Case "PictureBox26"
                If CardType <> "None" Then
                    form.PictureBox26.Image = My.Resources.ResourceManager.GetObject(CardColour & CardType)
                    form.PictureBox26.Show()
                    form.PictureBox26.Enabled = True
                Else
                    form.PictureBox26.Image = Nothing
                    form.PictureBox26.Hide()
                    form.PictureBox26.Enabled = False
                End If
            Case "PictureBox27"
                If CardType <> "None" Then
                    form.PictureBox27.Image = My.Resources.ResourceManager.GetObject(CardColour & CardType)
                    form.PictureBox27.Show()
                    form.PictureBox27.Enabled = True
                Else
                    form.PictureBox27.Image = Nothing
                    form.PictureBox27.Hide()
                    form.PictureBox27.Enabled = False
                End If
            Case "PictureBox28"
                If CardType <> "None" Then
                    form.PictureBox28.Image = My.Resources.ResourceManager.GetObject(CardColour & CardType)
                    form.PictureBox28.Show()
                    form.PictureBox28.Enabled = True
                Else
                    form.PictureBox28.Image = Nothing
                    form.PictureBox28.Hide()
                    form.PictureBox28.Enabled = False
                End If
            Case "PictureBox29"
                If CardType <> "None" Then
                    form.PictureBox29.Image = My.Resources.ResourceManager.GetObject(CardColour & CardType)
                    form.PictureBox29.Show()
                    form.PictureBox29.Enabled = True
                Else
                    form.PictureBox29.Image = Nothing
                    form.PictureBox29.Hide()
                    form.PictureBox29.Enabled = False
                End If
            Case "PictureBox30"
                If CardType <> "None" Then
                    form.PictureBox30.Image = My.Resources.ResourceManager.GetObject(CardColour & CardType)
                    form.PictureBox30.Show()
                    form.PictureBox30.Enabled = True
                Else
                    form.PictureBox30.Image = Nothing
                    form.PictureBox30.Hide()
                    form.PictureBox30.Enabled = False
                End If
            Case "PictureBox31"
                If CardType <> "None" Then
                    form.PictureBox31.Image = My.Resources.ResourceManager.GetObject(CardColour & CardType)
                    form.PictureBox31.Show()
                    form.PictureBox31.Enabled = True
                Else
                    form.PictureBox31.Image = Nothing
                    form.PictureBox31.Hide()
                    form.PictureBox31.Enabled = False
                End If
            Case "PictureBox32"
                If CardType <> "None" Then
                    form.PictureBox32.Image = My.Resources.ResourceManager.GetObject(CardColour & CardType)
                    form.PictureBox32.Show()
                    form.PictureBox32.Enabled = True
                Else
                    form.PictureBox32.Image = Nothing
                    form.PictureBox32.Hide()
                    form.PictureBox32.Enabled = False
                End If
            Case "PictureBox33"
                If CardType <> "None" Then
                    form.PictureBox33.Image = My.Resources.ResourceManager.GetObject(CardColour & CardType)
                    form.PictureBox33.Show()
                    form.PictureBox33.Enabled = True
                Else
                    form.PictureBox33.Image = Nothing
                    form.PictureBox33.Hide()
                    form.PictureBox33.Enabled = False
                End If
            Case "PictureBox34"
                If CardType <> "None" Then
                    form.PictureBox34.Image = My.Resources.ResourceManager.GetObject(CardColour & CardType)
                    form.PictureBox34.Show()
                    form.PictureBox34.Enabled = True
                Else
                    form.PictureBox34.Image = Nothing
                    form.PictureBox34.Hide()
                    form.PictureBox34.Enabled = False
                End If
            Case "PictureBox35"
                If CardType <> "None" Then
                    form.PictureBox35.Image = My.Resources.ResourceManager.GetObject(CardColour & CardType)
                    form.PictureBox35.Show()
                    form.PictureBox35.Enabled = True
                Else
                    form.PictureBox35.Image = Nothing
                    form.PictureBox35.Hide()
                    form.PictureBox35.Enabled = False
                End If
            Case "PictureBox36"
                If CardType <> "None" Then
                    form.PictureBox36.Image = My.Resources.ResourceManager.GetObject(CardColour & CardType)
                    form.PictureBox36.Show()
                    form.PictureBox36.Enabled = True
                Else
                    form.PictureBox36.Image = Nothing
                    form.PictureBox36.Hide()
                    form.PictureBox36.Enabled = False
                End If
            Case "PictureBox37"
                If CardType <> "None" Then
                    form.PictureBox37.Image = My.Resources.ResourceManager.GetObject(CardColour & CardType)
                    form.PictureBox37.Show()
                    form.PictureBox37.Enabled = True
                Else
                    form.PictureBox37.Image = Nothing
                    form.PictureBox37.Hide()
                    form.PictureBox37.Enabled = False
                End If
            Case "PictureBox38"
                If CardType <> "None" Then
                    form.PictureBox38.Image = My.Resources.ResourceManager.GetObject(CardColour & CardType)
                    form.PictureBox38.Show()
                    form.PictureBox38.Enabled = True
                Else
                    form.PictureBox38.Image = Nothing
                    form.PictureBox38.Hide()
                    form.PictureBox38.Enabled = False
                End If
            Case "PictureBox39"
                If CardType <> "None" Then
                    form.PictureBox39.Image = My.Resources.ResourceManager.GetObject(CardColour & CardType)
                    form.PictureBox39.Show()
                    form.PictureBox39.Enabled = True
                Else
                    form.PictureBox39.Image = Nothing
                    form.PictureBox39.Hide()
                    form.PictureBox39.Enabled = False
                End If
            Case "PictureBox40"
                If CardType <> "None" Then
                    form.PictureBox40.Image = My.Resources.ResourceManager.GetObject(CardColour & CardType)
                    form.PictureBox40.Show()
                    form.PictureBox40.Enabled = True
                Else
                    form.PictureBox40.Image = Nothing
                    form.PictureBox40.Hide()
                    form.PictureBox40.Enabled = False
                End If
            Case "PictureBox41"
                If CardType <> "None" Then
                    form.PictureBox41.Image = My.Resources.ResourceManager.GetObject(CardColour & CardType)
                    form.PictureBox41.Show()
                    form.PictureBox41.Enabled = True
                Else
                    form.PictureBox41.Image = Nothing
                    form.PictureBox41.Hide()
                    form.PictureBox41.Enabled = False
                End If
            Case "PictureBox42"
                If CardType <> "None" Then
                    form.PictureBox42.Image = My.Resources.ResourceManager.GetObject(CardColour & CardType)
                    form.PictureBox42.Show()
                    form.PictureBox42.Enabled = True
                Else
                    form.PictureBox42.Image = Nothing
                    form.PictureBox42.Hide()
                    form.PictureBox42.Enabled = False
                End If
            Case "PictureBox43"
                If CardType <> "None" Then
                    form.PictureBox43.Image = My.Resources.ResourceManager.GetObject(CardColour & CardType)
                    form.PictureBox43.Show()
                    form.PictureBox43.Enabled = True
                Else
                    form.PictureBox43.Image = Nothing
                    form.PictureBox43.Hide()
                    form.PictureBox43.Enabled = False
                End If
        End Select
    End Sub

End Class
