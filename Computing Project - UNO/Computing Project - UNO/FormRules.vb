Public Class FormRules

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Dim help As String
        help = "Uno Rules - Playing Cards:" + vbCrLf + vbCrLf
        help += "On a player’s turn, he/she must do one of the following:" + vbCrLf + vbCrLf
        help += "1. Play a card matching the last played card, which means they have the same colour or number." + vbCrLf + vbCrLf
        help += "2. Draw an extra card as penalty." + vbCrLf + vbCrLf
        help += "If the player chooses to draw an extra card, he/she still has a chance to play a matching card, otherwise the turn would pass to the next player in order."

        MsgBox(help)
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim help As String
        help = "Uno Rules - Card Types:" + vbCrLf + vbCrLf
        help += "A standard Uno deck consists of 108 cards, including Blue, Golden, Green and Red number cards 1 ~ 9 two each, four colour number card 0 one each, four colour word card Skip, Reverse and +2 one each, as well as wild card Wild and Wild +4 four each. " + vbCrLf
        help += "Word cards have different abilities and have different playing rules." + vbCrLf + vbCrLf
        help += "Skip: Can match non-word card of the same colour; next player in sequence misses a turn" + vbCrLf + vbCrLf
        help += "Reverse: Can match non-word card of the same colour; order of play switches directions (when there are only two players, this is equivalent to Skip)" + vbCrLf + vbCrLf
        help += "+2: Can match non-word card of the same colour; next player in sequence draws two cards" + vbCrLf + vbCrLf
        help += "Wild: Can match non-word cards of any colour; player declares next colour to be matched" + vbCrLf + vbCrLf
        help += "Wild +4: Can match any card except for card with skip ability (Skip, Reverse or Wild +4); player declares next colour to be matched; next player in sequence draws four cards and misses a turn." + vbCrLf + vbCrLf

        MsgBox(help)
    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        Dim help As String

        help = "Uno Rules - Playing First:" + vbCrLf + vbCrLf
        help += "At the beginning of a game, each player would be dealt a card randomly from the deck, and their numbers would be compared to decide which player plays first. In the case of non-number card or cards with the same number, new cards will be dealt until one player holds a card with lager number."

        MsgBox(help)
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        Dim help As String

        help = "Uno Rules - Winning:" + vbCrLf + vbCrLf

        help += "After deciding the play order, each player will be dealt with seven cards from the deck. Players can discard their hand on their turns." + vbCrLf + vbCrLf
        help += "The game proceeds until one player has reduced his/her hand to zero. This player then wins and the rest lose the game."

        MsgBox(help)
    End Sub
End Class