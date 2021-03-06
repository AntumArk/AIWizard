﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AIWizard.res {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class strings {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal strings() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("AIWizard.res.strings", typeof(strings).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Your selected build folder (one which contains all of the tools) is invalid. Please enter valid path..
        /// </summary>
        public static string BuildFolderPrompt {
            get {
                return ResourceManager.GetString("BuildFolderPrompt", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to https://github.com/AlexeyAB/darknet.
        /// </summary>
        public static string DarkNetGitSite {
            get {
                return ResourceManager.GetString("DarkNetGitSite", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Please enter path to where your darknet.exe is located. This folder is called &quot;root&quot; in our guide..
        /// </summary>
        public static string DarknetPathPrompt {
            get {
                return ResourceManager.GetString("DarknetPathPrompt", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Additions
        ///Additions//Tests
        ///Backup
        ///Backup//Initial
        ///Backup//Initial//logoData
        ///Backup//Initial//weights
        ///Changes
        ///Current
        ///Current//logoData
        ///Current//weights
        ///Current//Configs.
        /// </summary>
        public static string DBFolderTree {
            get {
                return ResourceManager.GetString("DBFolderTree", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Preparing the database is essential. If I tell you to give me some runite ore and a bottle of aggression potion, what would you do? Most likely shrug your shoulders because you have no clue what they are. But if I told you that it was from a certain game, you could answer me that you don’t play it or do not have those items. It shows that with some information from the past, you can give me some defined answer. Same goes for the neural networks as they are based on how human nerves work. If you show it enou [rest of string was truncated]&quot;;.
        /// </summary>
        public static string DBPrep {
            get {
                return ResourceManager.GetString("DBPrep", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Please enter path to folder, where you will be storing backups and changes of your database..
        /// </summary>
        public static string DBWorkspacePrompt {
            get {
                return ResourceManager.GetString("DBWorkspacePrompt", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to What do you want to do? .
        /// </summary>
        public static string FirstPrompt {
            get {
                return ResourceManager.GetString("FirstPrompt", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to •	Make or find videos containing your object, and some videos which don’t. Make sure those videos are as high quality as possible. We do not want to play “what can you see in this ink spot over 100 m away”. Same goes for the objects – make sure someone could easily recognize them without context. They should be fairly big (at least 50x50).
        ///•	Rename videos so they would not contain spaces and foreign symbols.
        ///•	It is wise to crop long videos to pieces containing your objects. You can use VLC media player r [rest of string was truncated]&quot;;.
        /// </summary>
        public static string GatherVideosHint {
            get {
                return ResourceManager.GetString("GatherVideosHint", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 
        ///Welcome to AI Wizard! by Linas Minkevičius
        ///
        ///This helper application is based on YoloGuide you can find in resources. 
        ///I suggest do some reading from links provided inside that document before 
        ///you start. 
        ///
        ///I ASSUME that you have already set up all of the required tools we made.
        ///
        ///You can always type in &quot;help&quot; for possible commands.
        ///
        ///Shall we start (yes/no)?.
        /// </summary>
        public static string Intro {
            get {
                return ResourceManager.GetString("Intro", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to .
        /// </summary>
        public static string TrainingPrepFlows {
            get {
                return ResourceManager.GetString("TrainingPrepFlows", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to So you want to use Daknet neural network? 
        ///You are lucky! 
        ///There are some quite good instructions how to set up (and even use) written on https://github.com/AlexeyAB/darknet. 
        ///Go there and do some reading.                                                                               
        ///
        ///I can’t really help with this yet, as I only did this setup 1-2 times.
        ///
        ///Do you want to visit that website?.
        /// </summary>
        public static string YoloSetup {
            get {
                return ResourceManager.GetString("YoloSetup", resourceCulture);
            }
        }
    }
}
