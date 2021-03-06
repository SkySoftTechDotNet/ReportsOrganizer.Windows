﻿using ReportsOrganizer.UI.Views.Windows;
using System;

namespace ReportsOrganizer.UI.Managers
{
    public class ApplicationManager
    {
        private readonly Lazy<MainWindowView> _mainWindowView;
        private readonly Lazy<NotificationWindowView> _notificationWindowView;

        public MainWindowView MainWindow => _mainWindowView.Value;
        public NotificationWindowView NotificationWindow => _notificationWindowView.Value;

        public ApplicationManager()
        {
            _mainWindowView = new Lazy<MainWindowView>(() => new MainWindowView());
            _notificationWindowView = new Lazy<NotificationWindowView>(() => new NotificationWindowView());
        }
    }
}
