
Namespace Utils

    Public Module GuiStrings

        Private LanguageStrings As New SortedList(Of Integer, String) 'GuiStringKey, GuiString

        Public CurrentGuiStringType As Type

        Public Enum VldtGuiStringKeys
            None
            SelectFolder
            StartBySpace
            FinishedTest
            CloseApp
            AppPtcTitle
            AppPtcID
            AppPtcNr
            AppPtcOK
            PractiseTitle
            PractiseText1
            PractiseText2
            PractiseREDO
            PractiseSKIP
        End Enum

        Public Enum VrtGuiStringKeys
            None
            SelectFolder
            FinishedTest
            ChangeView
            Replay
            [Next]
            FirstVideo
            LastVideo
            SavedToFile
            CloseApp
            CloseApp2
            AppPtcTitle
            AppPtcID
            AppPtcNr
            AppPtcOK
        End Enum

        Public Function GetGuiString(ByVal GuiStringKey As VldtGuiStringKeys) As String

            If CurrentGuiStringType <> GetType(VldtGuiStringKeys) Then
                Throw New Exception("Incorrect GuiStringKeyType specified in GetGuiString.")
            End If

            Return LanguageStrings(GuiStringKey)

        End Function

        Public Function GetGuiString(ByVal GuiStringKey As VrtGuiStringKeys) As String

            If CurrentGuiStringType <> GetType(VrtGuiStringKeys) Then
                Throw New Exception("Incorrect GuiStringKeyType specified in GetGuiString.")
            End If

            Return LanguageStrings(GuiStringKey)

        End Function

        Public Function Load(ByVal GuiStringsFilePath As String, ByVal GuiLanguage As String, ByVal GuiStringsType As Type) As Boolean

            CurrentGuiStringType = GuiStringsType

            Try

                Dim InputLines() As String = System.IO.File.ReadAllLines(GuiStringsFilePath, System.Text.Encoding.UTF8)

                'Dim CurrentGuiStringKey As GuiStringKeys = GuiStringKeys.None
                Dim CurrentGuiStringKey = GuiStringsType.GetEnumValues(0)

                Dim ValidGuiStringKeys As List(Of String) = [Enum].GetNames(GuiStringsType).ToList
                Dim ValidGuiStringKeysWithBrackets As New List(Of String)
                For Each Key In ValidGuiStringKeys
                    ValidGuiStringKeysWithBrackets.Add("<" & Key & ">")
                Next

                For Each Line In InputLines

                    If Line.Trim = "" Then Continue For
                    If Line.Trim.StartsWith("//") Then Continue For

                    If Line.Trim.StartsWith("<") Then
                        'New GuiStringKey
                        Dim NewKey = Line.Trim.Split({"//"}, StringSplitOptions.None)(0).Trim.Trim("<").Trim(">")
                        If ValidGuiStringKeys.Contains(NewKey) = False Then
                            MsgBox("Unable to read the following GUI-string key: " & NewKey & " specified in the file :" & GuiStringsFilePath & vbCrLf & vbCrLf & "It should be any of the following set: " & vbCrLf & String.Join(", ", ValidGuiStringKeysWithBrackets), MsgBoxStyle.Critical, "Unable to continue!")
                            Return False
                        Else
                            CurrentGuiStringKey = [Enum].Parse(GuiStringsType, NewKey)
                        End If
                    Else

                        'It should be a language line
                        Dim LanguageCode As String = Line.Trim.Split({"//"}, StringSplitOptions.None)(0).Trim.Split(":")(0).Trim
                        If LanguageCode = GuiLanguage Then
                            'We have the value
                            Dim Key_ValueSplit = Line.Trim.Split({"//"}, StringSplitOptions.None)(0).Trim.Split(":")
                            If Key_ValueSplit.Length > 1 Then
                                Dim LanguageString As String = Key_ValueSplit(1).Trim

                                'Putting back any bits of the value that were removed when splitting by colons above
                                For n = 2 To Key_ValueSplit.Length - 1
                                    LanguageString = LanguageString & ":" & Key_ValueSplit(n).Trim
                                Next

                                'Checks if the value is already assigned
                                If LanguageStrings.ContainsKey(CurrentGuiStringKey) Then
                                    'Multiple specifications
                                    MsgBox("There are multiple text specified for the GUI-string key: " & CurrentGuiStringKey.ToString & " in the file :" & GuiStringsFilePath, MsgBoxStyle.Critical, "Unable to continue!")
                                    Return False
                                End If

                                'Adds the value
                                LanguageStrings.Add(CurrentGuiStringKey, LanguageString)

                            Else
                                'Missing value
                                MsgBox("There is no text specified for the GUI-string key: <" & CurrentGuiStringKey.ToString & "> in the file :" & GuiStringsFilePath, MsgBoxStyle.Critical, "Unable to continue!")
                                Return False
                            End If
                        Else
                            'It's another language, just ignores it
                        End If

                    End If

                Next

                'Checks if all values have been set
                For Each GuiStringKey In [Enum].GetValues(GuiStringsType)

                    If GuiStringKey = 0 Then Continue For

                    If LanguageStrings.ContainsKey(GuiStringKey) = False Then
                        'Multiple specifications
                        MsgBox("The GUI-string key: <" & GuiStringKey.ToString & "> is missing for the language code " & GuiLanguage & " in the file :" & GuiStringsFilePath, MsgBoxStyle.Critical, "Unable to continue!")
                        Return False
                    End If
                Next

            Catch ex As Exception
                MsgBox("Unable to load the GuiStrings file containing the strings to show in the graphical user interface.", MsgBoxStyle.Critical, "Unable to continue!")
                Return False
            End Try

            Return True

        End Function


    End Module

End Namespace
