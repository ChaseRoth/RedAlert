﻿using RedAlertBot.Util;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using HWND = System.IntPtr;

namespace RedAlertBot
{
    public class RedAlertBot : Notifier
    {
        #region Instance Getter
        private static readonly RedAlertBot bot = new RedAlertBot();
        public static RedAlertBot Bot => bot;
        #endregion

        #region Events
        public event EventHandler<BotIsEnabledChangedArgs> BotIsEnabledChanged; // Bot has been enabled or disabled
        #endregion

        #region Properties
        private bool isBotEnabled;
        /// <summary>
        /// Toggles the bot on and off.
        /// </summary>
        public bool IsBotEnabled
        {
            get => isBotEnabled;
            set
            {
                if (IsBotEnabled == value) return;
                isBotEnabled = value;
                BotIsEnabledChanged?.Invoke(this, new BotIsEnabledChangedArgs(IsBotEnabled));
                NotifyPropertyChanged();
            }
        }

        public ImageRecorder Recorder { get; set; }
        
        internal WindowsUtil WindowUtility { get; set; }
        #endregion

        private RedAlertBot()
        {
            Recorder = new ImageRecorder(this);
#if DEBUG
            WindowsUtil.InitConsole();
#endif
        }

        ~RedAlertBot()
        {
#if DEBUG
            WindowsUtil.CloseConsole();
#endif
        }

        /// <summary>
        /// Initializes the RedAlert bot.
        /// </summary>
        /// <param name="sourceWindow">The window the hotkey should hook into</param>
        /// <returns>Boolean indicating a successful or failure to initialize</returns>
        public bool Init(HWND sourceWindow)
        {
            bot.Recorder.TargetWindowHWND = WindowsUtil.FindWindowA(null, "ARK: Survival Evolved");
            WindowUtility = new WindowsUtil(sourceWindow);
            if (bot.Recorder.TargetWindowHWND != HWND.Zero && WindowUtility != null)
            {
                return true;
            }
            return false;
        }
    }   

    public class BotIsEnabledChangedArgs : EventArgs
    {
        public bool IsEnabled { get; set; }

        public BotIsEnabledChangedArgs(bool _isEnabled) => IsEnabled = _isEnabled;       
    }
}
