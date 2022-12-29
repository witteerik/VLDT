Public Class TestMaterial
    Inherits List(Of Block)

    Public Property IsPractiseTestMaterial As Boolean = False

    Private Property CurrentBlockOrderIndex As Integer? = Nothing
    Public Property BlockOrder As List(Of Integer)

    ''' <summary>
    ''' Holds the number of presented items is the  test material
    ''' </summary>
    Public Property PresentedItems As Integer = 0

    Public Sub SetupBlockOrder(ByRef TestSpecification As TestSpecification, ByVal ParticipantNumber As Integer)
        If IsPractiseTestMaterial = True Then
            BlockOrder = New List(Of Integer) From {0}
        Else

            If TestSpecification.RandomizeBlockOrder = True Then

                Dim Randomizer As Random
                If TestSpecification.RandomSeed.HasValue = True Then
                    Randomizer = New Random(TestSpecification.RandomSeed)
                Else
                    Randomizer = New Random
                End If
                BlockOrder = Utils.Math.SampleWithoutReplacement(TestSpecification.NumberOfBlocks, 0, TestSpecification.NumberOfBlocks, Randomizer).ToList

            Else

                'Setting manual block orders (in the settings file) allows for counter-balanced blocks, based on Latin Squares, or similar technique, see https://en.wikipedia.org/wiki/Block_design

                'Converts the one-based participent order to a zero-based index
                Dim ParticipantIndex As Integer = ParticipantNumber - 1

                'Unwraps the ParticipantIndex to the number of available blockorders
                Dim ManualBlockOrderCount As Integer = TestSpecification.ManualBlockOrders.Count
                Dim UnwrappedParticipantIndex As Integer = ParticipantIndex Mod ManualBlockOrderCount

                'Setting the block order to use
                BlockOrder = TestSpecification.ManualBlockOrders(UnwrappedParticipantIndex)

            End If

        End If

    End Sub

    Public Enum NextEventTypes
        PresetTrial
        EndOfBlock
        EndOfTest
    End Enum

    Public Class NextTrialResult
        Public NextEventType As NextEventTypes
        Public NextTrialItem As TestItem
    End Class

    Public Function GetCurrentBlockNumber() As Integer?
        If CurrentBlockOrderIndex Is Nothing Then
            Return Nothing
        Else
            Return Me(BlockOrder(CurrentBlockOrderIndex)).BlockNumber
        End If
    End Function

    Public Function CompletedBlocks() As Integer
        If CurrentBlockOrderIndex Is Nothing Then
            Return 0
        Else
            Return CurrentBlockOrderIndex + 1
        End If
    End Function

    Public Function GetNextEvent(ByRef Randomizer As Random) As NextTrialResult

        Dim NextTrialResult As New NextTrialResult

        If CurrentBlockOrderIndex Is Nothing Then
            CurrentBlockOrderIndex = 0
        End If

        'Checking if there are any remaining items to present
        Dim UnpresentedItems = Me(BlockOrder(CurrentBlockOrderIndex)).GetUnpresentedItems
        If UnpresentedItems.Count = 0 Then

            'Checks if all blocks have been testet
            If CurrentBlockOrderIndex >= BlockOrder.Count - 1 Then
                'This was the last block
                NextTrialResult.NextEventType = NextEventTypes.EndOfTest
                Return NextTrialResult

            Else
                'This was not the last block
                NextTrialResult.NextEventType = NextEventTypes.EndOfBlock
                Return NextTrialResult

            End If

        Else

            'There are more items to present in the current block
            NextTrialResult.NextEventType = NextEventTypes.PresetTrial

            'Selects a random test item
            Dim RandomItemIndex = Randomizer.Next(0, UnpresentedItems.Count)
            NextTrialResult.NextTrialItem = UnpresentedItems(RandomItemIndex)

            'Increments PresentedItems (in the test as a whole)
            PresentedItems += 1
            NextTrialResult.NextTrialItem.WithinTest_TrialPresentationOrder = PresentedItems

            'Increments PresentedItems (in the current block)
            Me(BlockOrder(CurrentBlockOrderIndex)).PresentedItems += 1
            NextTrialResult.NextTrialItem.WithinBlock_TrialPresentationOrder = Me(BlockOrder(CurrentBlockOrderIndex)).PresentedItems

            'Stores the BlockPresentationOrder
            NextTrialResult.NextTrialItem.BlockPresentationOrder = CurrentBlockOrderIndex + 1

            Return NextTrialResult

        End If

    End Function

    ''' <summary>
    ''' Returns the number of presented items in the current block
    ''' </summary>
    ''' <returns></returns>
    Public Function PresentedItemsInCurrentBlock() As Integer

        If CurrentBlockOrderIndex Is Nothing Then
            CurrentBlockOrderIndex = 0
        End If

        Return Me(BlockOrder(CurrentBlockOrderIndex)).PresentedItems

    End Function


    Public Sub DisposeVideos()

        For Each Block In Me
            Block.DisposeVideos()
        Next

    End Sub

    Public Sub GotoNextBlock()
        CurrentBlockOrderIndex += 1
    End Sub

    ''' <summary>
    ''' Exports the test results
    ''' </summary>
    ''' <param name="OutputFolder"></param>
    ''' <param name="ParticipantID"></param>
    ''' <param name="SelectedBlocks">If set, should contain the block number of the blocks to export.</param>
    Public Sub ExportResults(ByVal OutputFolder As String, ByVal ParticipantID As String, ByVal ParticipantNumber As Integer, Optional ByVal SelectedBlocks As SortedSet(Of Integer) = Nothing)

        'Creating a filename
        Dim FileName As String = CreateExportFileName(ParticipantID, ParticipantNumber, SelectedBlocks)

        Dim ExportList As New List(Of String)

        Dim IncludeHeadings As Boolean = True
        For Each Block In Me

            If SelectedBlocks IsNot Nothing Then
                'Skips to next block if the block number is not in the SelectBlocks List
                If SelectedBlocks.Contains(Block.BlockNumber) = False Then Continue For
            End If

            For Each TestItem In Block.RealItems
                ExportList.Add(TestItem.ToString(IncludeHeadings))
                'Setting IncludeHeadings to False in order to only get headings on the first line
                IncludeHeadings = False
            Next

            For Each TestItem In Block.PseudoItems
                ExportList.Add(TestItem.ToString(IncludeHeadings))
                'Setting IncludeHeadings to False in order to only get headings on the first line
                IncludeHeadings = False
            Next

        Next

        Utils.SendInfoToLog(String.Join(vbCrLf, ExportList), FileName, OutputFolder)

    End Sub

    Public Function CreateExportFileName(ByVal ParticipantID As String, ByVal ParticipantNumber As Integer, Optional ByVal SelectedBlocks As SortedSet(Of Integer) = Nothing) As String

        Dim FileName As String
        Dim ParticipantString As String = ParticipantID & "_" & ParticipantNumber.ToString("000")
        If IsPractiseTestMaterial = True Then
            FileName = "VLDT_" & ParticipantString & "_PractiseTest"
        Else
            If SelectedBlocks Is Nothing Then
                FileName = "VLDT_" & ParticipantString & "_AllBlocks"
            Else
                If SelectedBlocks.Count = 1 Then
                    FileName = "VLDT_" & ParticipantString & "_Block_" & String.Join("_", SelectedBlocks)
                Else
                    FileName = "VLDT_" & ParticipantString & "_Blocks_" & String.Join("_", SelectedBlocks)
                End If
            End If
        End If

        Return FileName

    End Function

    Public Function GetProportionCorrect() As Double

        Dim TestedTrials As Integer = 0
        Dim CorrectTrials As Integer = 0

        For Each Block In Me
            For Each TestItem In Block.RealItems
                If TestItem.Response = Response.NotPresented Then Continue For

                TestedTrials += 1
                If TestItem.Result = Result.Correct Then CorrectTrials += 1
            Next

            For Each TestItem In Block.PseudoItems
                If TestItem.Response = Response.NotPresented Then Continue For

                TestedTrials += 1
                If TestItem.Result = Result.Correct Then CorrectTrials += 1
            Next
        Next

        If TestedTrials > 0 Then
            Return CorrectTrials / TestedTrials
        Else
            Return 0
        End If

    End Function

    ''' <summary>
    ''' Clears all test results in the current instance of TestMaterial and prepares the TestMaterial to be run again (useful to redo the practise test)
    ''' </summary>
    Public Sub ClearResults()

        If IsPractiseTestMaterial = False Then
            MsgBox("Only practise blocks can be reset!", , "Invalid operation")
            Exit Sub
        End If

        Me.PresentedItems = 0

        For Each Block In Me

            Block.PresentedItems = 0

            For Each TestItem In Block.RealItems
                TestItem.ClearResults()
            Next

            For Each TestItem In Block.PseudoItems
                TestItem.ClearResults()
            Next
        Next

    End Sub

End Class
