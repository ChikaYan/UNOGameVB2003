Module Module1

    Sub SettingUpDeck(ByRef cp1() As Integer, ByVal cp2() As Integer, ByVal cdeck() As Integer, ByRef sp1() As Integer, ByVal sp2() As Integer, ByVal sdeck() As Integer)
        Dim i, j, k As Integer

        k = 1
        For i = 1 To 108
            cp1(i) = -1
            cp2(i) = -1
            sp1(i) = -1
            sp2(i) = -1
        Next

        For i = 1 To 8
            For j = 1 To 12
                sdeck(12 * (i - 1) + j) = j

                If i <= 4 Then
                    cdeck(12 * (i - 1) + j) = i
                Else
                    cdeck(12 * (i - 1) + j) = i - 4
                End If
            Next
        Next

        For i = 97 To 100
            sdeck(i) = 0
            cdeck(i) = i - 96
        Next

        For i = 101 To 108
            cdeck(i) = 5
            If i <= 104 Then
                sdeck(i) = 13
            Else
                sdeck(i) = 14
            End If
        Next

        For i = 1 To 108
            If cdeck(i) = 1 Then
                Console.Write("Red")
            ElseIf cdeck(i) = 2 Then
                Console.Write("Golden")
            ElseIf cdeck(i) = 3 Then
                Console.Write("Green")
            ElseIf cdeck(i) = 4 Then
                Console.Write("Blue")
            ElseIf cdeck(i) = 5 Then
            End If

            If sdeck(i) >= 0 And sdeck(i) <= 9 Then
                Console.WriteLine(sdeck(i))
            ElseIf sdeck(i) = 10 Then
                Console.WriteLine("Skip")
            ElseIf sdeck(i) = 11 Then
                Console.WriteLine("Skip")
            ElseIf sdeck(i) = 12 Then
                Console.WriteLine("+2")
            ElseIf sdeck(i) = 13 Then
                Console.WriteLine("Wild")
            ElseIf sdeck(i) = 14 Then
                Console.WriteLine("Wild +4")
            End If
        Next

        Console.ReadLine()
    End Sub

    Sub Dealing()

    End Sub
    Function GetMenuChoice()

        Console.WriteLine("1. Play against AI")
        Console.WriteLine("2. Play against player")
        Console.WriteLine("0. End game")
        Console.WriteLine("")
        Console.WriteLine("Please enter your choice")
        Return (Console.ReadLine())

    End Function

    Sub Main()
        Dim MenuChoice As Integer
        Dim cp1(108), cp2(108), cdeck(108) As Integer ' 1 means Red, 2 means Golden, 3 means Green, 4 means Blue, 5 means wild, -1 means none
        Dim sp1(108), sp2(108), sdeck(108) As Integer ' 0~9, 10 means skip, 11 means skip, 12 means +2, 13 means wild, 14 means wile +4, -1 means none

        Console.WriteLine("UNO Games")
        Console.WriteLine("")

        MenuChoice = GetMenuChoice()
        If MenuChoice = 1 Or 2 Then
            SettingUpDeck(cp1, sp1, cp2, sp2, cdeck, sdeck)
        End If

    End Sub

End Module
