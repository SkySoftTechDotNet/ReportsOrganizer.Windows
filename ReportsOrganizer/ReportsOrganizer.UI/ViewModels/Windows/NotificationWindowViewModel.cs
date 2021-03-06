﻿using ReportsOrganizer.UI.Abstractions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using ReportsOrganizer.Core.Managers;
using ReportsOrganizer.Core.Services;
using ReportsOrganizer.Models;
using ReportsOrganizer.UI.Command;
using ReportsOrganizer.UI.Managers;

namespace ReportsOrganizer.UI.ViewModels.Windows
{
    public class NotificationWindowViewModel : BaseViewModel
    {
        private readonly INotificationManager _notificationManager;
        private readonly IReportService _reportService;
        private readonly IProjectService _projectService;
        private readonly ApplicationManager _applicationManager;

        private Visibility _windowVisibility;
        private string _description;
        private Project _selectedProject;
        private TimeSpan _selectedTime;
        private BindingList<Project> _projectList;


        public bool UsePreviousAvailable => LastReport != null;
        public Report LastReport { get; private set; }
        public CultureInfo DurationCultureInfo { get; }

        public ICommand WindowClosingCommand { get; }
        public ICommand UsePreviousCommand { get; }
        public ICommand PostponeCommand { get; }
        public ICommand OkCommand { get; }

        public Visibility WindowVisibility
        {
            get => _windowVisibility;
            set
            {
                if (value == Visibility.Visible)
                {
                    var desktopWorkingArea = SystemParameters.WorkArea;
                    _applicationManager.NotificationWindow.Left = desktopWorkingArea.Right - _applicationManager.NotificationWindow.Width - 10;
                    _applicationManager.NotificationWindow.Top = desktopWorkingArea.Bottom - _applicationManager.NotificationWindow.Height - 10;

                    LastReport = _reportService.FindLastReportAsync(CancellationToken.None).Result;
                    _projectList = _projectService.ToBindingListAsync(CancellationToken.None).Result;
                    _projectList.ListChanged += ProjectsUpdated;

                    NotifyPropertyChanged(nameof(ProjectList));
                    NotifyPropertyChanged(nameof(UsePreviousAvailable));

                    SelectedProject = LastReport?.Project != null && LastReport.Project.IsActive ? LastReport.Project : ProjectList.FirstOrDefault();
                    _description = null;
                    SelectedTime = new TimeSpan();

                    NotifyPropertyChanged(nameof(Description));
                    ClearValidationErrors(nameof(Description));
                }
                SetValue(ref _windowVisibility, value, nameof(WindowVisibility));
            }
        }

        [Required(AllowEmptyStrings = false)]
        public string Description
        {
            get => _description;
            set => SetAndValidateProperty(ref _description, value, nameof(Description));
        }

        public Project SelectedProject
        {
            get => _selectedProject;
            set => SetValue(ref _selectedProject, value, nameof(SelectedProject));
        }

        public TimeSpan SelectedTime
        {
            get => _selectedTime;
            set => SetValue(ref _selectedTime, value, nameof(SelectedTime));
        }

        public IEnumerable<Project> ProjectList
        {
            get
            {
                return _projectList
                  .Where(property => property.IsActive)
                  .OrderBy(property => property.ShortName);
            }
        }

        public NotificationWindowViewModel(IReportService reportService, IProjectService projectService, ApplicationManager applicationManager)
        {
            _reportService = reportService;
            _projectService = projectService;
            _applicationManager = applicationManager;

            WindowClosingCommand = new RelayCommand(WindowClosingAction, true);
            UsePreviousCommand = new RelayCommand(UsePreviousAction, true);
            PostponeCommand = new RelayCommand(PostponeAction, true);
            OkCommand = new RelayCommand(OkAction, true);

            DurationCultureInfo = new CultureInfo("uk-UA")
            {
                DateTimeFormat =
                {
                    ShortTimePattern = "HH:mm",
                    LongTimePattern = "HH:mm"
                }
            };
        }

        private void WindowClosingAction(object sender)
        {
            ((CancelEventArgs)sender).Cancel = true;
            WindowVisibility = Visibility.Hidden;
        }

        private void UsePreviousAction(object obj)
        {
            if (LastReport.Project != null && LastReport.Project.IsActive)
                SelectedProject = LastReport.Project;
            Description = LastReport.Description;
        }

        private void PostponeAction(object obj)
        {
            WindowVisibility = Visibility.Hidden;
        }

        private void OkAction(object obj)
        {
            Validate();
            if (HasErrors)
                return;

            _reportService.AddAsync(new Report
            {
                Description = _description,
                Created = DateTime.Now,
                Duration = (int)_selectedTime.TotalMinutes,
                Project = _selectedProject
            }, CancellationToken.None);

            WindowVisibility = Visibility.Hidden;
        }

        private void ProjectsUpdated(object sender, ListChangedEventArgs e)
        {
            var tmpId = _selectedProject?.Id;
            SelectedProject = null;
            SelectedProject = ProjectList.FirstOrDefault(p => p.Id == tmpId) ?? ProjectList.FirstOrDefault();
            NotifyPropertyChanged(nameof(ProjectList));
        }
    }
}
