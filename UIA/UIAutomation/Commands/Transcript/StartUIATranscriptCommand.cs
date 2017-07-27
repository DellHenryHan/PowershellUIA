﻿/*
 * Created by SharpDevelop.
 * User: Alexander Petrovskiy
 * Date: 30.11.2011
 * Time: 11:12
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

namespace UIAutomation.Commands
{
    extern alias UIANET; extern alias UIACOM;// using System.Windows.Automation;
    using System;
    using System.Management.Automation;
    using classic = UIANET::System.Windows.Automation; using viacom = UIACOM::System.Windows.Automation; // using System.Windows.Automation;
    using System.Diagnostics;
    using System.IO;

    /// <summary>
    /// Description of StartUiaTranscriptCommand.
    /// </summary>
    [Cmdlet(VerbsLifecycle.Start, "UiaTranscript")]
    //[OutputType(new[]{ typeof(object) })]
    
    public class StartUiaTranscriptCommand : TranscriptCmdletBase
    {
        #region Constructor
        public StartUiaTranscriptCommand()
        {
            LongRecordingFileName = String.Empty;
            ShortRecordingFileName = String.Empty;
        }
        #endregion Constructor
        
        #region Parameters
        [UiaParameter][Parameter(Mandatory = false)]
        public string LongRecordingFileName { get; set; }
        [UiaParameter][Parameter(Mandatory = false)]
        public string ShortRecordingFileName { get; set; }
        #endregion Parameters
        
        #region BeginProcessing
        
// string errorMessageInTheGatheringCycle = String.Empty;
// bool errorInTheGatheringCycle = false;
// string errorMessageInTheInnerCycle = String.Empty;
// bool errorInTheInnerCycle = false;
        
        protected override void BeginProcessing()
        {
            
            if (!NoUI) {
                Timeout = 604800000;
                // frmRecorder formRecorder = 
                CurrentData.formRecorder = 
                    new RecorderForm(this);
                // formRecorder.ShowDialog();
                CurrentData.formRecorder.Show();
                CurrentData.formRecorder.Hide();
                try {
                Events.SubscribeEvent((object)CurrentData.formRecorder.btnStop,
                                           "BtnStopClick",
                                           "formRecorder",
                                           new PSObject(),
                                           ScriptBlock.Create(""), // CurrentData.formRecorder.BtnStopClick,
                                           true,
                                           false);
                } catch { }
                CurrentData.formRecorder.ShowDialog();
                return;
            } else {
                UiaHelper.ProcessingTranscript(this);
            }
#region old
// Global.GTranscript = true;
// int counter = 0;
// 
// if (!this.NoUI) {
// this.Timeout = 604800000;
// }
// 
//  // 20120206 System.Windows.Automation.AutomationElement rootElement = 
//  // 20120206     System.Windows.Automation.AutomationElement.RootElement;
// rootElement = 
// System.Windows.Automation.AutomationElement.RootElement;
//  // 20120206 System.DateTime startDate = 
// startDate =
// System.DateTime.Now;
// do
// {
// RunOnSleepScriptBlocks(this);
// System.Threading.Thread.Sleep(Preferences.TranscriptInterval);
// while (Paused) {
// System.Threading.Thread.Sleep(Preferences.TranscriptInterval);
// }
// counter++;
// 
// try {
//  // use Windows forms mouse code instead of WPF
// System.Drawing.Point mouse = System.Windows.Forms.Cursor.Position;
// System.Windows.Automation.AutomationElement element = 
// System.Windows.Automation.AutomationElement.FromPoint(
// new System.Windows.Point(mouse.X, mouse.Y));
// if (element != null)
// {
// processingElement(element);
// }
// if (errorInTheGatheringCycle) {
// WriteDebug(this, "An error is in the control hierarchy gathering cycle");
// WriteDebug(this, errorMessageInTheGatheringCycle);
// errorInTheGatheringCycle = false;
// }
// } catch (Exception eUnknown) {
// WriteDebug(this, eUnknown.Message);
// }
// System.DateTime nowDate = System.DateTime.Now;
// if ((nowDate - startDate).TotalSeconds > this.Timeout/1000) break;
// } while (Global.GTranscript);
#endregion old
        }
        #endregion BeginProcessing
        
        #region Script header
        private void WriteHeader(ref StreamWriter fileWriter, 
                                         string fileName) {
            try {
        
            fileWriter.WriteLine(@"################################################################################");
            fileWriter.WriteLine("#\tScript name:\t" + fileName);
            fileWriter.WriteLine("#\tScript usage:\tpowershell.exe -command " + fileName);
            fileWriter.WriteLine("#\tNote:\tThe script is generated by automatic means. It may or may not require");
            fileWriter.WriteLine("#\t\t\tmanual amendment. Use the script in production with care.");
            fileWriter.WriteLine("#\t\t\t");
            fileWriter.WriteLine("#\tHelp:\tIn case you've got stuck with the script or the UIAutomation module itself,");
            fileWriter.WriteLine("#\t\t\tplease follow the troubleshooting procedure:");
            fileWriter.WriteLine("#\t\t\tTurn on $VerbosePreference and $DebugPreference variables");
            fileWriter.WriteLine("#\t\t\tIt will give you a clue in case a cmdlet is waiting for something extremely long time.");
            fileWriter.WriteLine("#\t\t\tBy default, Select- cmdlets are equipped with Timeout equalling to 5 seconds (5000)");
            fileWriter.WriteLine("#\t\t\tWhen -Verbose and -Debug keay are activated, Select- cmdlets produce richer output.");
            fileWriter.WriteLine("#\t\t\tIf you see repetitive lines looking like 'title:' ... 'classname:' ...");
            fileWriter.WriteLine("#\t\t\tit's a hint that a control or a window is unavailable at the moment.");
            fileWriter.WriteLine("#\t\t\t");
            fileWriter.WriteLine("#\t\t\tOne more advice with the module is on how and what to use.");
            fileWriter.WriteLine("#\t\t\tIf you are especially interested in a control of specific type, issue the command like this:");
            fileWriter.WriteLine("#\t\t\tGet-Command -Module UIA* *Edit* #(or, what's equal to the previous, Get-Command -Module UIA* *TextBox*)");
            fileWriter.WriteLine("#\t\t\tThe output returns current offering for the control you want");
            fileWriter.WriteLine("#\t\t\t");
            fileWriter.WriteLine("#\t\t\tSimilarly, you can filter cmdlets by an action you are planning to perform with the control");
            fileWriter.WriteLine("#\t\t\tGet-Command -Module UIA* *Click* #(or Get-Command -Module UIA* *Toggle*)");
            fileWriter.WriteLine("#\t\t\t");
            fileWriter.WriteLine("#\tAuthor:\tAlexander Petrovskiy");
            fileWriter.WriteLine("#\tContact:\thttp://SoftwareTestingUsingPowerShell.WordPress.com");
            fileWriter.WriteLine("################################################################################");
            fileWriter.WriteLine(@"cls");
            fileWriter.WriteLine(@"Set-StrictMode -Version Latest;");
            fileWriter.WriteLine(@"# the following lines are not necessary for script to be working, they're for your information only");
            fileWriter.WriteLine(@"#region Preferences");
            fileWriter.WriteLine(@"$DebugPreference = [System.Management.Automation.ActionPreference]::Continue;");
            fileWriter.WriteLine(@"$VerbosePreference = [System.Management.Automation.ActionPreference]::Continue;");
            fileWriter.WriteLine(@"[UIAutomation.Preferences]::Timeout = 5000;");
            fileWriter.WriteLine(@"#endregion Preferences");
            fileWriter.Flush();
            
            } 
            catch (Exception eUnknown) {
                WriteDebug(this, eUnknown.Message);
            }
        }
        #endregion Script header
        
        #region EndProcessing
        protected override void EndProcessing()
        {
            try {
                WriteVerbose(this, "preparing results");
                if (Recording.Count <= 0) return;
                WriteVerbose(this, "there are " + Recording.Count.ToString() + " records");
                #region preparing script files
                // use user's %TEMP%
                string recordingFileName =
                    Environment.GetEnvironmentVariable(
                        "TEMP",
                        EnvironmentVariableTarget.User) + 
                    @"\";
                string shRecordingFileName = 
                    recordingFileName;
//                    // file names from parameters -Long... and -Short...
//                    if (this.LongRecordingFileName.Length > 0) {
//                        recordingFileName +=
//                            this.LongRecordingFileName;
//                    }
//                    if (this.ShortRecordingFileName.Length > 0) {
//                        shRecordingFileName +=
//                            this.ShortRecordingFileName;
//                    }
                // genearated file names
                if (LongRecordingFileName.Length == 0) {
                    recordingFileName += @"UIAutomation_recording_";
                }
                if (ShortRecordingFileName.Length == 0) {
                    shRecordingFileName += 
                        @"UIAutomation_recording_short_";
                }
                string datetime = 
                    (((((DateTime.Now.ToShortDateString().ToString() +
                         "_" + 
                         DateTime.Now.ToShortTimeString()).Replace(":", "_")).Replace("/", "_")).Replace(";", "_")).Replace(@"\", "_")).Replace(" ", "_");
                if (LongRecordingFileName.Length == 0) {
                    recordingFileName += datetime;
                    recordingFileName += ".ps1";
                }
                if (ShortRecordingFileName.Length == 0) {
                    shRecordingFileName += datetime;
                    shRecordingFileName += ".ps1";
                }
                    
                    
                // file names from parameters -Long... and -Short...
                if (LongRecordingFileName.Length > 0) {
                    recordingFileName =
                        LongRecordingFileName;
                }
                if (ShortRecordingFileName.Length > 0) {
                    shRecordingFileName =
                        ShortRecordingFileName;
                }
                    
                    
                WriteVerbose(this, "long recording file name " + 
                                   recordingFileName);
                WriteVerbose(this, "short recording file name " +
                                   shRecordingFileName);
                    
                StreamWriter writerToLongFile = 
                    new StreamWriter(recordingFileName, true);
                StreamWriter writerToShortFile = 
                    new StreamWriter(shRecordingFileName, true);
                WriteVerbose(this, "log writers created");
    
                if (!NoScriptHeader) {
                    WriteVerbose(this, "writing the script header");
                    WriteHeader(ref writerToLongFile, recordingFileName);
                    WriteHeader(ref writerToShortFile, shRecordingFileName);
                }
                #endregion preparing script files

                #region writing the script
                for (int j = 0; j < Recording.Count; j++) {
                    object patternsInfo = null;
                    if (WriteCurrentPattern) {
                        patternsInfo = RecordingPatterns[j];
                    }
                    WritingRecord(
                        Recording[j], 
                        //RecordingPatterns[j],
                        patternsInfo,
                        writerToLongFile,
                        writerToShortFile);
                    WriteVerbose(this, "the record " + j.ToString() + " has been written");
                }
                #endregion writing the script
                writerToLongFile.Flush(); writerToLongFile.Close();
                writerToShortFile.Flush(); writerToShortFile.Close();
                try {
                    Process.Start("notepad.exe", recordingFileName);
                    Process.Start("notepad.exe", shRecordingFileName);
                } catch {
                    WriteObject(this, "The full script recorded is here: " + 
                                      recordingFileName);
                    WriteObject(this, "The short script recorded is here: " + 
                                      shRecordingFileName);
                }

                /*
                if (Recording.Count > 0) {
                    WriteVerbose(this, "there are " + Recording.Count.ToString() + " records");
                    #region preparing script files
                    // use user's %TEMP%
                    string recordingFileName =
                        System.Environment.GetEnvironmentVariable(
                            "TEMP",
                            System.EnvironmentVariableTarget.User) + 
                        @"\";
                    string shRecordingFileName = 
                        recordingFileName;
//                    // file names from parameters -Long... and -Short...
//                    if (this.LongRecordingFileName.Length > 0) {
//                        recordingFileName +=
//                            this.LongRecordingFileName;
//                    }
//                    if (this.ShortRecordingFileName.Length > 0) {
//                        shRecordingFileName +=
//                            this.ShortRecordingFileName;
//                    }
                    // genearated file names
                    if (this.LongRecordingFileName.Length == 0) {
                        recordingFileName += @"UIAutomation_recording_";
                    }
                    if (this.ShortRecordingFileName.Length == 0) {
                        shRecordingFileName += 
                            @"UIAutomation_recording_short_";
                    }
                    string datetime = 
                        (((((System.DateTime.Now.ToShortDateString().ToString() +
                            "_" + 
                            System.DateTime.Now.ToShortTimeString()).Replace(":", "_")).Replace("/", "_")).Replace(";", "_")).Replace(@"\", "_")).Replace(" ", "_");
                    if (this.LongRecordingFileName.Length == 0) {
                        recordingFileName += datetime;
                        recordingFileName += ".ps1";
                    }
                    if (this.ShortRecordingFileName.Length == 0) {
                        shRecordingFileName += datetime;
                        shRecordingFileName += ".ps1";
                    }
                    
                    
                    // file names from parameters -Long... and -Short...
                    if (this.LongRecordingFileName.Length > 0) {
                        recordingFileName =
                            this.LongRecordingFileName;
                    }
                    if (this.ShortRecordingFileName.Length > 0) {
                        shRecordingFileName =
                            this.ShortRecordingFileName;
                    }
                    
                    
                    WriteVerbose(this, "long recording file name " + 
                               recordingFileName);
                    WriteVerbose(this, "short recording file name " +
                               shRecordingFileName);
                    
                    System.IO.StreamWriter writerToLongFile = 
                        new System.IO.StreamWriter(recordingFileName, true);
                    System.IO.StreamWriter writerToShortFile = 
                        new System.IO.StreamWriter(shRecordingFileName, true);
                    WriteVerbose(this, "log writers created");
    
                    if (!this.NoScriptHeader) {
                        WriteVerbose(this, "writing the script header");
                        writeHeader(ref writerToLongFile, recordingFileName);
                        writeHeader(ref writerToShortFile, shRecordingFileName);
                    }
                    #endregion preparing script files

                    #region writing the script
                    for (int j = 0; j < Recording.Count; j++) {
                        object patternsInfo = null;
                        if (this.WriteCurrentPattern) {
                            patternsInfo = RecordingPatterns[j];
                        }
                        WritingRecord(
                            Recording[j], 
                            //RecordingPatterns[j],
                            patternsInfo,
                            writerToLongFile,
                            writerToShortFile);
                        WriteVerbose(this, "the record " + j.ToString() + " has been written");
                    }
                    #endregion writing the script
                    writerToLongFile.Flush(); writerToLongFile.Close();
                    writerToShortFile.Flush(); writerToShortFile.Close();
                    try {
                        System.Diagnostics.Process.Start("notepad.exe", recordingFileName);
                        System.Diagnostics.Process.Start("notepad.exe", shRecordingFileName);
                    } catch {
                        WriteObject(this, "The full script recorded is here: " + 
                                    recordingFileName);
                        WriteObject(this, "The short script recorded is here: " + 
                                    shRecordingFileName);
                    }
                } // the end of if (recording.Count > 0)
                */
            } catch (Exception eRecording) {
                WriteObject(this, false);
                WriteDebug(this, "Could not save the recording");
                WriteDebug(this, eRecording.Message);
            }
        }
        #endregion EndProcessing
        
        #region getControlTypeNameOfAutomationElement
        private string GetControlTypeNameOfAutomationElement(
            // 20131109
            //AutomationElement element,
            //AutomationElement element2)
            IUiElement element,
            IUiElement element2)
        {
            string result = String.Empty;
            // 20140312
//            if (element != null && (int)element.Current.ProcessId > 0 && 
//                element2 != null && (int)element2.Current.ProcessId > 0) {
//                element.Current.ControlType.ProgrammaticName.Substring(
//                    element2.Current.ControlType.ProgrammaticName.IndexOf('.') + 1);
            if (element != null && (int)element.GetCurrent().ProcessId > 0 && 
                element2 != null && (int)element2.GetCurrent().ProcessId > 0) {
                element.GetCurrent().ControlType.ProgrammaticName.Substring(
                    element2.GetCurrent().ControlType.ProgrammaticName.IndexOf('.') + 1);
            }
            return result;
        }
        #endregion getControlTypeNameOfAutomationElement
    }
    
    /// <summary>
    /// Description of StartUiaRecorderCommand.
    /// </summary>
    [Cmdlet(VerbsLifecycle.Start, "UiaRecorder")]
    //[OutputType(new[]{ typeof(object) })]
    
    public class StartUiaRecorderCommand : StartUiaTranscriptCommand
    { public StartUiaRecorderCommand() { } }
}