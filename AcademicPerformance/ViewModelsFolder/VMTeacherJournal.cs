﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using AcademicPerformance.ClassFolder;
using AcademicPerformance.CommandsFolder;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace AcademicPerformance.ViewModelFolder

{
    public class VMTeacherJournal : INotifyPropertyChanged
    {
        
        public VMTeacherJournal()
        {
            saveCommand = new RelayCommand(Save);
            teacherJournalController = new JournalController();
            disciplineController = new DisciplineController();
            evaluationController = new EvaluationController();
            DisciplineList = new ObservableCollection<DisciplineModel>(disciplineController.GetAll());
            EvaluationList = new ObservableCollection<EvaluationModel>(evaluationController.GetAll());
            SelectedRow = new JournalModel();
            SearchText = "";
            Filter();
        }


        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private JournalController teacherJournalController;
        private DisciplineController disciplineController;
        private EvaluationController evaluationController;



        private ObservableCollection<JournalModel> filteredJournalList;

        public ObservableCollection<JournalModel> FilteredJournalList
        {
            get { return filteredJournalList; }
            set { filteredJournalList = value; OnPropertyChanged("FilteredJournalList"); }

        }


        private ObservableCollection<JournalModel> journalList;
        public ObservableCollection<JournalModel> JournalList
        {
            get { return journalList; }
            set { journalList = value; OnPropertyChanged("JournalList"); }

        }


        private ObservableCollection<EvaluationModel> evaluationList;
        public ObservableCollection<EvaluationModel> EvaluationList
        {
            get { return evaluationList; }
            set { evaluationList = value;}

        }

        private ObservableCollection<DisciplineModel> disciplineList;
        public ObservableCollection<DisciplineModel> DisciplineList
        {
            get { return disciplineList; }
            set { disciplineList = value; }

        }

        private string searchText;
        public string SearchText
        {
            get { return searchText; }
            set { searchText = value; Filter(); OnPropertyChanged("SearchText"); }

        }
        private EvaluationModel selectedNumber;
        public EvaluationModel SelectedNumber
        {
            set
            {
                selectedNumber = value;
                if (EvaluationList != null && selectedNumber != null && SelectedRow != null
                    && SelectedRow.NumberEvaluation != selectedNumber.NumberEvaluation)
                {
                    SelectedRow.NameEvaluation = EvaluationList[EvaluationList.Count -selectedNumber.NumberEvaluation].NameEvaluation;
                    SelectedRow.IdEvaluation = selectedNumber.IdEvaluation;
                }
            }

        }

        private JournalModel selectedRow;
        public JournalModel SelectedRow
        {
            get { return selectedRow; }
            set { selectedRow = value; OnPropertyChanged("SelectedRow"); }

        }


        private void Filter()
        {
            JournalList = new ObservableCollection<JournalModel>(teacherJournalController.GetAll());
            FilteredJournalList =
                new ObservableCollection<JournalModel>(
                    from item
                    in JournalList
                    where item.NameEvaluation.ToUpper().Contains(SearchText.ToUpper())
                      || item.FIOTeacher.ToUpper().Contains(SearchText.ToUpper())
                      || item.FIOStudent.ToUpper().Contains(SearchText.ToUpper())
                      || item.NameDiscipline.ToUpper().Contains(SearchText.ToUpper())
                      || item.NumberEvaluation.ToString().ToUpper().Contains(SearchText.ToUpper())
                      || item.IdJournal.ToString().ToUpper().Contains(SearchText.ToUpper())
                    select item);
            if (FilteredJournalList.Any())
            {
                SelectedRow = FilteredJournalList[0];
            }
        }


        
        private RelayCommand saveCommand;

        public RelayCommand SaveCommand
        {
            get { return saveCommand; }
        }

        private string message;

        public string Message
        {
            get { return message; }
            set { message = value; /*OnPropertyChanged(Message);*/ }
        }


        public void Save(object param)
        {
            foreach (var item in filteredJournalList)
            {
                teacherJournalController.Update(item);
            }
            
            MessageBox.Show(Message);
        }


    }
}