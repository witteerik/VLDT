Public Class FindMatches

    ''' <summary>
    ''' A list of items onto which items in the TargetLists should be matched.
    ''' </summary>
    ''' <returns></returns>
    Public Property SourceList As New List(Of MatchItem)

    Public Property TargetLists As New List(Of List(Of MatchItem))

    ''' <summary>
    ''' Containing the list names in the order loaded, with SourceList at index zero and each TargetList in TargetLists following
    ''' </summary>
    ''' <returns></returns>
    Public Property ListNames As New List(Of String)

    Public Rnd As Random
    Public Sub New(Optional ByVal Seed As Integer = 42)

        Rnd = New Random(Seed)

    End Sub

    Public Property FinalResultLists As New SortedList(Of Integer, List(Of MatchItem))


    Public Sub Feed(ByVal InputPath As String)

        'Storing the current culture decimal separator, in order for parsing of Double values to work (independently of current culture). (N.B. The SMA decimal separator should always be point (.), not comma)
        Dim CDS = System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator

        Dim InputLines() As String = System.IO.File.ReadAllLines(InputPath, System.Text.Encoding.UTF8)

        Dim VariableNames As New List(Of String)
        Dim FirstLineRead As Boolean = False

        For i = 0 To InputLines.Length - 1

            Dim Line = InputLines(i)
            If Line.Trim = "" Then Continue For
            If Line.Trim.StartsWith("//") Then Continue For

            Dim LineSplit = Line.Trim.Split(vbTab)
            If LineSplit.Length < 3 Then Throw New Exception("There must be at least three (tab-delimited) columns in the input file.")

            If FirstLineRead = False Then
                'First line should contain headings and at columns beyond the second one, variable names

                'Reading variable names
                For col = 2 To LineSplit.Length - 1
                    Dim VariebleName = LineSplit(col).Trim
                    If VariebleName = "" Then Throw New Exception("Missing variable name for column " & col)
                    If VariableNames.Contains(VariebleName) = False Then VariableNames.Add(VariebleName)
                Next

                FirstLineRead = True
                Continue For
            End If

            'First column should be ListName, and the first list should be the source list
            'Second column should be ItemName
            'Remaining columns should be variables to match

            Dim ListName = LineSplit(0).Trim
            If ListName = "" Then Throw New Exception("Missing list name on line " & i)
            If ListNames.Contains(ListName) = False Then
                ListNames.Add(ListName)

                If ListNames.Count = 1 Then
                    SourceList = New List(Of MatchItem)
                Else
                    TargetLists.Add(New List(Of MatchItem))
                End If

            End If

            Dim ItemName = LineSplit(1).Trim
            If ItemName = "" Then Throw New Exception("Missing item name on line " & i)

            Dim VariableValues As New List(Of Double)
            'Reading variable values
            For col = 2 To LineSplit.Length - 1
                Dim VariebleValue As Double
                If Double.TryParse(LineSplit(col).Trim().Replace(",", CDS).Replace(".", CDS), VariebleValue) = False Then
                    Throw New Exception("Unable to read the variable value at line " & i & " column " & col)
                End If
                VariableValues.Add(VariebleValue)
            Next

            'Creating a new MatchItem
            Dim NewMatchItem = New MatchItem With {.ItemName = ItemName}
            For j = 0 To VariableNames.Count - 1
                NewMatchItem.MatchVariables.Add(VariableNames(j), VariableValues(j))
            Next

            'Adding the otem into the appropriate list
            If ListNames.Count = 1 Then
                'If there is only one list name loaded yet (the source list) the item should go into that list
                SourceList.Add(NewMatchItem)
            Else
                'If more than onle list has been loaded, the item goes into the last added target list
                TargetLists(TargetLists.Count - 1).Add(NewMatchItem)
            End If

        Next

    End Sub

    Public Sub MatchLists(ByVal OutputPath As String, Optional ByVal Count As Integer = 100, Optional Iterations As Integer = 10000, Optional ByVal IncludeSd As Boolean = False)

        'Limiting Count to the available items in each list
        If SourceList.Count < Count Then Count = SourceList.Count
        For Each TargetList In TargetLists
            If TargetList.Count < Count Then Count = TargetList.Count
        Next

        Dim BestIterationResult As SortedList(Of Integer, List(Of MatchItem)) = Nothing
        Dim BestIterationStatistics As List(Of Double) = Nothing

        For iteration = 0 To Iterations - 1

            If iteration Mod 500 = 0 Then Console.WriteLine("Started iteration: " & iteration & " / " & Iterations)

            'Creating a new ResultLists object
            Dim CurrentResultLists As New SortedList(Of Integer, List(Of MatchItem))

            'Adding keys in the ResultLists object
            CurrentResultLists.Add(0, New List(Of MatchItem))
            For i = 1 To TargetLists.Count
                CurrentResultLists.Add(i, New List(Of MatchItem))
            Next

            'Copying all items to temporary source and target lists
            Dim SourceListCopy As New List(Of MatchItem)
            SourceListCopy.AddRange(SourceList.GetRange(0, SourceList.Count))
            Dim TargetListsCopy As New List(Of List(Of MatchItem))
            For n = 0 To TargetLists.Count - 1
                Dim NewTargetListCopy As New List(Of MatchItem)
                NewTargetListCopy.AddRange(TargetLists(n).GetRange(0, TargetLists(n).Count))
                TargetListsCopy.Add(NewTargetListCopy)
            Next

            'Goes through every item in SourceList
            For AddedItems = 1 To Count

                'Draws a random item from SourceList
                Dim RandomSourceListIndex = Rnd.Next(SourceListCopy.Count)

                'Adding the source item to the ResultLists (at index 0)
                CurrentResultLists(0).Add(SourceListCopy(RandomSourceListIndex))

                'Finding best match to the current source item within each target list
                Dim SelectedIndices_TargetLists As New List(Of Integer)
                For Each TargetList In TargetListsCopy

                    Dim SelectedIndex = FindBestMatchInList(SourceListCopy(RandomSourceListIndex), TargetList)

                    SelectedIndices_TargetLists.Add(SelectedIndex)
                Next

                For i = 0 To SelectedIndices_TargetLists.Count - 1

                    'Adding the selected items in ResultLists at the index corresponding to each target list
                    CurrentResultLists(i + 1).Add(TargetListsCopy(i)(SelectedIndices_TargetLists(i)))

                    'Removing the selected items from the TargetLists
                    TargetListsCopy(i).RemoveAt(SelectedIndices_TargetLists(i))
                Next

                'Removing the source items from the SourceList
                SourceListCopy.RemoveAt(RandomSourceListIndex)

            Next

            'Getting the current iteration statistics (mean and sd ranges)
            Dim CurrentIterationStatistics = GetStatistics(CurrentResultLists, IncludeSd, False)

            If BestIterationStatistics Is Nothing Then
                'Storing the initial iteration results and statistisct
                Console.WriteLine("Results in iteration: " & iteration + 1 & " Variable ranges: " & String.Join(" ", CurrentIterationStatistics))
                GetStatistics(CurrentResultLists, IncludeSd, True)
                BestIterationStatistics = CurrentIterationStatistics
                BestIterationResult = CurrentResultLists

            Else

                If AllEqualOrLower(CurrentIterationStatistics.ToArray, BestIterationStatistics.ToArray) = True Then
                    'Storing the iteration results and statistict only if it is better than the best so far
                    Console.WriteLine("Improved results in iteration: " & iteration + 1 & " Variable ranges: " & String.Join(" ", CurrentIterationStatistics))
                    GetStatistics(CurrentResultLists, IncludeSd, True)
                    BestIterationStatistics = CurrentIterationStatistics
                    BestIterationResult = CurrentResultLists
                End If
            End If
        Next

        FinalResultLists = BestIterationResult

        SaveResults(OutputPath)

        Console.WriteLine("Finished matching items")
        MsgBox("Finished matching items")

    End Sub


    ''' <summary>
    ''' Returns the index within TargetList of the best match to SourceItem 
    ''' </summary>
    ''' <param name="SourceItem"></param>
    ''' <param name="TargetList"></param>
    ''' <returns></returns>
    Private Function FindBestMatchInList(ByRef SourceItem As MatchItem, ByRef TargetList As List(Of MatchItem))

        Dim BestMatchIndex As Integer = 0
        Dim LowestDistance As Double = Double.MaxValue

        'Getting an array of the variable values to compare from the SourceItem 
        Dim SourceItemVariableArray = SourceItem.MatchVariables.Values.ToArray

        For i = 0 To TargetList.Count - 1

            'Skipping to next if the compared items are the same (this should really never happen, but may be the case if this code is modifed in the future.
            If SourceItem Is TargetList(i) Then
                Continue For
            End If

            'Getting an array of the variable values to compare from the current item in TargetList 
            Dim TargetItemVariableArray = TargetList(i).MatchVariables.Values.ToArray

            'Calculating the euclidean distance between the variable values
            Dim Distance As Double = GetEuclideanDistance(SourceItemVariableArray, TargetItemVariableArray)

            'Storing the distance and the best matching index only if the current distance is lower than the previoulsy lowest distance value.
            If Distance < LowestDistance Then
                LowestDistance = Distance
                BestMatchIndex = i
            End If
        Next

        Return BestMatchIndex

    End Function

    Public Function GetEuclideanDistance(ByRef Array1 As Double(), ByRef Array2 As Double()) As Double

        'Checking that the arrays have the same lengths
        If Array1.Length <> Array2.Length Then Throw New ArgumentException("Input arrays must have the same length.")

        Dim Sum As Double = 0
        For n = 0 To Array1.Length - 1
            Sum += (Array1(n) - Array2(n)) ^ 2
        Next

        Return System.Math.Sqrt(Sum)

    End Function

    Public Function GetStatistics(ByRef ResultLists As SortedList(Of Integer, List(Of MatchItem)), ByVal IncludeSd As Boolean, Optional ByVal WriteToConsole As Boolean = False) As List(Of Double)

        Dim VariableMeanList As New SortedList(Of String, List(Of Double))
        Dim VariableSdList As New SortedList(Of String, List(Of Double))

        For Each ResultList In ResultLists

            'Listing all values for each variable 
            Dim VariableValueList As New SortedList(Of String, List(Of Double))
            For Each Item In ResultList.Value
                For Each Variable In Item.MatchVariables
                    If VariableValueList.ContainsKey(Variable.Key) = False Then VariableValueList.Add(Variable.Key, New List(Of Double))
                    VariableValueList(Variable.Key).Add(Variable.Value)
                Next
            Next

            'Calculating mean and sd for each variable and storing them in VariableMeanList and VariableSdList, one index for each ResultList
            For Each Variable In VariableValueList

                If VariableMeanList.ContainsKey(Variable.Key) = False Then VariableMeanList.Add(Variable.Key, New List(Of Double))
                If VariableSdList.ContainsKey(Variable.Key) = False Then VariableSdList.Add(Variable.Key, New List(Of Double))

                VariableMeanList(Variable.Key).Add(Variable.Value.Average)

                Dim StandardDeviation As Double
                CoefficientOfVariation(Variable.Value, ,,,, StandardDeviation, StandardDeviationTypes.Sample)
                VariableSdList(Variable.Key).Add(StandardDeviation)
            Next

        Next

        If WriteToConsole = True Then
            For Each Variable In VariableMeanList
                Console.WriteLine("     " & Variable.Key & ", mean values: " & String.Join("; ", Variable.Value))
            Next
            For Each Variable In VariableSdList
                Console.WriteLine("     " & Variable.Key & ", sd values: " & String.Join("; ", Variable.Value))
            Next
        End If

        Dim RangeList As New List(Of Double)

        For Each Variable In VariableMeanList
                RangeList.Add(Math.Abs(Variable.Value.Max - Variable.Value.Min))
            Next
            For Each Variable In VariableSdList
                If IncludeSd = True Then RangeList.Add(Math.Abs(Variable.Value.Max - Variable.Value.Min))
            Next

            Return RangeList

    End Function


    ''' <summary>
    ''' Returns True if all values in Array1 are lower than their corresponding values in Array 2 (by pairwise comparison, index by index), otherwise returns False.
    ''' </summary>
    ''' <param name="Array1"></param>
    ''' <param name="Array2"></param>
    ''' <returns></returns>
    Public Function AllLower(ByVal Array1() As Double, ByVal Array2() As Double) As Boolean

        If Array1.Length <> Array2.Length Then Throw New ArgumentException("Array1 and Array2 must have the same length!")

        For i = 0 To Array1.Length - 1
            If Array2(i) < Array1(i) Then Return False
        Next

        Return True

    End Function

    ''' <summary>
    ''' Returns True if all values in Array1 are equal to or lower than their corresponding values in Array 2 (by pairwise comparison, index by index), otherwise returns False.
    ''' </summary>
    ''' <param name="Array1"></param>
    ''' <param name="Array2"></param>
    ''' <returns></returns>
    Public Function AllEqualOrLower(ByVal Array1() As Double, ByVal Array2() As Double) As Boolean

        If Array1.Length <> Array2.Length Then Throw New ArgumentException("Array1 and Array2 must have the same length!")

        For i = 0 To Array1.Length - 1
            If Array1(i) > Array2(i) Then Return False
        Next

        Return True

    End Function

    Public Sub SaveResults(ByVal OutputPath As String)

        Dim OutputList As New List(Of String)

        'Creating and adding column headings
        For i = 0 To FinalResultLists.Count - 1
            Dim RowList As New List(Of String)
            'Adding list name
            RowList.Add("ListName")
            For Each Item In FinalResultLists(i)
                'Adding list name
                RowList.Add("ItemName")
                'Adding all variable names (note that the order here is sorted and therefore mat differ from the variable order in the input file.
                For Each Variable In Item.MatchVariables
                    RowList.Add(Variable.Key)
                Next
                Exit For
            Next
            'Adding the row
            OutputList.Add(String.Join(vbTab, RowList))
            Exit For
        Next

        'Creating and adding data rows
        For i = 0 To FinalResultLists.Count - 1

            For Each Item In FinalResultLists(i)

                Dim RowList As New List(Of String)

                'Adding list name
                RowList.Add(ListNames(i))

                'Adding list name
                RowList.Add(Item.ItemName)

                'Adding all variables (note that the order here is sorted and therefore mat differ from the variable order in the input file.
                For Each Variable In Item.MatchVariables
                    RowList.Add(Variable.Value)
                Next

                'Adding the row
                OutputList.Add(String.Join(vbTab, RowList))

            Next

        Next

        'Exporting the results
        Utils.SendInfoToLog(String.Join(vbCrLf, OutputList), System.IO.Path.GetFileNameWithoutExtension(OutputPath), System.IO.Path.GetDirectoryName(OutputPath), True, True, True)

        'Dim Statistics = GetStatistics(FinalResultLists)
        'Utils.SendInfoToLog(String.Join(vbCrLf, Statistics), "Statistics", System.IO.Path.GetDirectoryName(OutputPath), True, True, True)

    End Sub


    Public Class MatchItem

        Public Property ItemName As String

        ''' <summary>
        ''' A list of variables to match, key=variable name, value=variable value
        ''' </summary>
        ''' <returns></returns>
        Public Property MatchVariables As New SortedList(Of String, Double)


    End Class



    ''' <summary>
    ''' Calculates the the coefficient of variation of a set of input values. Also sum, mean, sum of squares, variance and standard deviation can be attained by using the optional parameters.
    ''' </summary>
    ''' <param name="InputListOfDouble"></param>
    ''' <param name="Sum">Upon return of the function, this variable will contain the arithmetric mean.</param>
    ''' <param name="ArithmetricMean">Upon return of the function, this variable will contain the arithmetric mean.</param>
    ''' <param name="SumOfSquares">Upon return of the function, this variable will contain the SumOfSquares.</param>
    ''' <param name="Variance">Upon return of the function, this variable will contain the variance.</param>
    ''' <param name="StandardDeviation">Upon return of the function, this variable will contain the standard deviation.</param>
    ''' <param name="InputValueType">Default calculation type (Population) uses N in the variance calculation denominator. If Sample type is used, the denominator is N-1.</param>
    ''' <returns>Returns the coefficient of variation.</returns>
    Public Function CoefficientOfVariation(ByRef InputListOfDouble As List(Of Double),
                                      Optional ByRef Sum As Double = Nothing,
                                      Optional ByRef ArithmetricMean As Double = Nothing,
                                      Optional ByRef SumOfSquares As Double = Nothing,
                                      Optional ByRef Variance As Double = Nothing,
                                      Optional ByRef StandardDeviation As Double = Nothing,
                                           Optional ByRef InputValueType As StandardDeviationTypes = StandardDeviationTypes.Population) As Double

        'Notes the number of values in the input list
        Dim n As Integer = InputListOfDouble.Count

        'Calculates the sum of the values in the input list
        Sum = 0
        For i = 0 To InputListOfDouble.Count - 1
            Sum += InputListOfDouble(i)
        Next

        'Calculates the arithemtric mean of the values in the input list
        ArithmetricMean = Sum / n

        'Calculates the sum of squares of the values in the input list
        SumOfSquares = 0
        For i = 0 To InputListOfDouble.Count - 1
            SumOfSquares += (InputListOfDouble(i) - ArithmetricMean) ^ 2
        Next

        'Calculates the variance of the values in the input list
        Select Case InputValueType
            Case StandardDeviationTypes.Population
                Variance = (1 / (n)) * SumOfSquares
            Case StandardDeviationTypes.Sample
                Variance = (1 / (n - 1)) * SumOfSquares
        End Select

        'Calculates, the standard deviation of the values in the input list
        StandardDeviation = System.Math.Sqrt(Variance)

        'Calculates and returns the coefficient of variation
        Return StandardDeviation / ArithmetricMean

    End Function

    Public Enum StandardDeviationTypes
        Population
        Sample
    End Enum


End Class