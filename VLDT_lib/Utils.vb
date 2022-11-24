Imports System.IO
Imports System.Threading

Namespace Utils
    Public Module Logging

        Public LogFilePath As String = "C:\VLTD_log\"
        Public ShowErrors As Boolean = True
        Public LogErrors As Boolean = True
        Public LogIsInMultiThreadApplication As Boolean = False

        Public LoggingSpinLock As New Threading.SpinLock

        Public Sub SendInfoToLog(ByVal Message As String,
                                 Optional ByVal LogFileNameWithoutExtension As String = "",
                                 Optional ByVal LogFileTemporaryPath As String = "",
                                 Optional ByVal OmitDateInsideLog As Boolean = False,
                                 Optional ByVal OmitDateInFileName As Boolean = False,
                                 Optional ByVal OverWrite As Boolean = False)

            Dim SpinLockTaken As Boolean = False

            Try

                'Attempts to enter a spin lock to avoid multiple thread conflicts when saving to the same file
                LoggingSpinLock.Enter(SpinLockTaken)

                If LogFileTemporaryPath = "" Then LogFileTemporaryPath = Utils.LogFilePath

                Dim FileNameToUse As String = ""

                If OmitDateInFileName = False Then
                    If LogFileNameWithoutExtension = "" Then
                        FileNameToUse = "log-" & DateTime.Now.ToShortDateString.Replace("/", "-") & ".txt"
                    Else
                        FileNameToUse = LogFileNameWithoutExtension & "-" & DateTime.Now.ToShortDateString.Replace("/", "-") & ".txt"
                    End If
                Else
                    If LogFileNameWithoutExtension = "" Then
                        FileNameToUse = "LDT_log.txt"
                    Else
                        FileNameToUse = LogFileNameWithoutExtension & ".txt"
                    End If

                End If

                Dim OutputFilePath As String = Path.Combine(LogFileTemporaryPath, FileNameToUse)

                'Adds a thread ID if in multi thread app
                If LogIsInMultiThreadApplication = True Then
                    Dim TreadName As String = Thread.CurrentThread.ManagedThreadId
                    OutputFilePath &= "ThreadID_" & TreadName
                End If

                'Adds the file to AddFileToCopy for later copying of test results
                AddFileToCopy(OutputFilePath)

                Try
                    'If File.Exists(logFilePathway) Then File.Delete(logFilePathway)
                    If Not Directory.Exists(LogFileTemporaryPath) Then Directory.CreateDirectory(LogFileTemporaryPath)

                    If OverWrite = True Then
                        'Deleting the files before writing if overwrite is true
                        If IO.File.Exists(OutputFilePath) Then IO.File.Delete(OutputFilePath)
                    End If

                    Dim Writer As New StreamWriter(OutputFilePath, FileMode.Append)
                    If OmitDateInsideLog = False Then
                        Writer.WriteLine(DateTime.Now.ToString & vbCrLf & Message)
                    Else
                        Writer.WriteLine(Message)
                    End If
                    Writer.Close()

                Catch ex As Exception
                    Errors(ex.ToString, "Error saving to log file!")
                End Try

            Finally

                'Releases any spinlock
                If SpinLockTaken = True Then LoggingSpinLock.Exit()
            End Try

        End Sub

        Public Sub Errors(ByVal errorText As String, Optional ByVal errorTitle As String = "Error")

            If ShowErrors = True Then
                MsgBox(errorText, MsgBoxStyle.Critical, errorTitle)
            End If

            If LogErrors = True Then
                Utils.SendInfoToLog("The following error occurred: " & vbCrLf & errorTitle & errorText, "Errors")
            End If

        End Sub


        Private FilesToCopy As New SortedSet(Of String)

        ''' <summary>
        ''' Add file paths that should later be copied to a specific folder using the public sub CopyFilesToFolder.
        ''' </summary>
        ''' <param name="FullFilePath"></param>
        Public Sub AddFileToCopy(ByVal FullFilePath As String)
            If Not FilesToCopy.Contains(FullFilePath) Then
                FilesToCopy.Add(FullFilePath)
            End If
        End Sub

        ''' <summary>
        ''' Checks if the input filename exists in the specified folder. If it doesn't exist, the input filename is returned, 
        ''' but if it already exists, a new numeral suffix is added to the file name. The file name extension is never changed.
        ''' </summary>
        ''' <returns></returns>
        Public Function CheckFileNameConflict(ByVal InputFilePath As String) As String

            Dim WorkingFilePath As String = InputFilePath

0:

            If File.Exists(WorkingFilePath) Then

                Dim Folder As String = Path.GetDirectoryName(WorkingFilePath)
                Dim FileNameExtension As String = Path.GetExtension(WorkingFilePath)
                Dim FileNameWithoutExtension As String = Path.GetFileNameWithoutExtension(WorkingFilePath)

                'Getting any numeric end, separated by a _, in the input file name
                Dim NumericEnd As String = ""
                Dim FileNameSplit() As String = FileNameWithoutExtension.Split("_")
                Dim NewFileNameWithoutNumericString As String = ""
                If Not IsNumeric(FileNameSplit(FileNameSplit.Length - 1)) Then

                    'Creates a new WorkingFilePath with a numeric suffix, and checks if it also exists
                    WorkingFilePath = Path.Combine(Folder, FileNameWithoutExtension & "_000" & FileNameExtension)

                    'Checking the new file name
                    GoTo 0

                Else

                    'Creates a new WorkingFilePath with a iterated numeric suffix, and checks if it also exists by restarting at 0

                    'Reads the current numeric value, stored after the last _
                    Dim NumericValue As Integer = CInt(FileNameSplit(FileNameSplit.Length - 1))

                    'Increases the value of the numeric suffix by 1
                    Dim NewNumericString As String = (NumericValue + 1).ToString("000")

                    'Creates a new WorkingFilePath with the increased numeric suffix, and checks if it also exists by restarting at 0
                    If FileNameSplit.Length > 1 Then
                        For n = 0 To FileNameSplit.Length - 2
                            NewFileNameWithoutNumericString &= FileNameSplit(n)
                        Next
                    End If
                    WorkingFilePath = Path.Combine(Folder, NewFileNameWithoutNumericString & "_" & NewNumericString & FileNameExtension)

                    'Checking the new file name
                    GoTo 0
                End If

            Else
                Return WorkingFilePath
            End If

        End Function


    End Module

    Public Module Math

        ''' <summary>
        ''' Returns a vector of length n, with random integers sampled in from the range of min (includive) to max (exclusive).
        ''' </summary>
        ''' <returns></returns>
        Public Function SampleWithoutReplacement(ByVal n As Integer, ByVal min As Integer, ByVal max As Integer,
                                             Optional randomSource As Random = Nothing) As Integer()

            If randomSource Is Nothing Then randomSource = New Random()

            If n > max - min Then Throw New ArgumentException("max minus min must be equal to or greater than n")

            Dim SampleData As New HashSet(Of Integer)
            Dim NewSample As Integer

            'Sampling data until the length of SampleData equals n
            Do Until SampleData.Count >= n

                'Getting a random sample
                NewSample = randomSource.Next(min, max)

                'Adding the sample only if it is not already present in SampleData 
                If Not SampleData.Contains(NewSample) Then SampleData.Add(NewSample)
            Loop

            Return SampleData.ToArray

        End Function


    End Module

End Namespace